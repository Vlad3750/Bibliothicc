using Bibliothicc.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bibliothicc.Services
{
    public class LibraryServiceRest : ILibraryService
    {
        private readonly HttpClient _client;

        public int CurrentUserId { get; set; } = 1;

        public LibraryServiceRest(HttpClient client)
        {
            _client = client;
        }

        public async Task<User?> Login(User user)
        {
            var result = await _client.PostAsJsonAsync("user/login", new
            {
                name = user.Username,
                password = user.passwordHash
            });
            if (!result.IsSuccessStatusCode) return null;
            var loggedIn = await result.Content.ReadFromJsonAsync<User>();
            if (loggedIn != null) CurrentUserId = loggedIn.UserID;
            return loggedIn;
        }

        public async Task<User?> Register(User user)
        {
            var result = await _client.PostAsJsonAsync("user/", new
            {
                name = user.Username,
                password = user.passwordHash
            });
            if (result.StatusCode == System.Net.HttpStatusCode.Conflict) return null;
            result.EnsureSuccessStatusCode();
            var registered = await result.Content.ReadFromJsonAsync<User>();
            if (registered != null) CurrentUserId = registered.UserID;
            return registered;
        }

        public async Task<List<Library>> GetLibraries()
        {
            return await _client.GetFromJsonAsync<List<Library>>($"library/users/{CurrentUserId}/libraries/") ?? new();
        }

        public async Task<Library> CreateLibrary(Library library)
        {
            var result = await _client.PostAsJsonAsync($"library/users/{CurrentUserId}/libraries/", new
            {
                name = library.Name,
                fileType = library.FileType,
                isPublic = library.IsPublic
            });
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<Library>() ?? library;
        }

        public async Task DeleteLibrary(int libraryId)
        {
            var result = await _client.DeleteAsync($"library/{libraryId}");
            result.EnsureSuccessStatusCode();
        }

        public async Task PublishLibrary(int libraryId, bool isPublic)
        {
            var result = await _client.PutAsJsonAsync($"library/{libraryId}", new { isPublic });
            result.EnsureSuccessStatusCode();
        }

        public async Task<List<Library>> GetPublicLibraries()
        {
            return await _client.GetFromJsonAsync<List<Library>>("library/public") ?? new();
        }

        public async Task<List<Media>> GetMedias(int libraryId)
        {
            return await _client.GetFromJsonAsync<List<Media>>($"libraries/{libraryId}/media/") ?? new();
        }

        public async Task<Media> CreateMedia(int libraryId, Media file)
        {
            // Step 1: upload the local file to the server
            using var form = new MultipartFormDataContent();
            var fileBytes = File.ReadAllBytes(file.FileUrl);
            form.Add(new ByteArrayContent(fileBytes), "file", file.Name);
            var uploadResult = await _client.PostAsync("upload", form);
            uploadResult.EnsureSuccessStatusCode();
            var uploadResponse = await uploadResult.Content.ReadFromJsonAsync<UploadResponse>();

            // Step 2: create the media record with the returned URL
            var result = await _client.PostAsJsonAsync($"libraries/{libraryId}/media/", new
            {
                name = file.Name,
                title = file.Title,
                mimeType = file.MimeType,
                mediaURL = uploadResponse!.url,
                coverURL = string.IsNullOrEmpty(file.CoverUrl) ? (string?)null : file.CoverUrl
            });
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync<Media>() ?? file;
        }

        public async Task ChangeMedia(Media file)
        {
            var result = await _client.PutAsJsonAsync($"libraries/{file.LibId}/media/{file.MediaID}", new
            {
                name = file.Name,
                title = file.Title,
                coverURL = string.IsNullOrEmpty(file.CoverUrl) ? (string?)null : file.CoverUrl
            });
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteMedia(int fileId)
        {
            var result = await _client.DeleteAsync($"media/{fileId}");
            result.EnsureSuccessStatusCode();
        }

        public async Task<byte[]> DownloadFile(string mediaUrl)
        {
            return await _client.GetByteArrayAsync(mediaUrl.TrimStart('/'));
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _client.GetFromJsonAsync<List<Category>>($"users/{CurrentUserId}/categories/") ?? new();
        }

        public async Task CreateCategory(Category category)
        {
            var result = await _client.PostAsJsonAsync($"users/{CurrentUserId}/categories/", new
            {
                name = category.Name
            });
            result.EnsureSuccessStatusCode();
        }

        public async Task ChangeCategory(Category category)
        {
            var result = await _client.PutAsJsonAsync($"users/{CurrentUserId}/categories/{category.CategoryID}", new
            {
                name = category.Name
            });
            result.EnsureSuccessStatusCode();
        }
    }

    internal record UploadResponse(string url);
}
