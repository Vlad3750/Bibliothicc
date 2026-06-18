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
            Logger.Info($"Login attempt: {user.Username}");
            var result = await _client.PostAsJsonAsync("user/login", new
            {
                name = user.Username,
                password = user.passwordHash
            });
            if (!result.IsSuccessStatusCode)
            {
                Logger.Warn($"Login failed for '{user.Username}': HTTP {(int)result.StatusCode}");
                return null;
            }
            var loggedIn = await result.Content.ReadFromJsonAsync<User>();
            if (loggedIn != null)
            {
                CurrentUserId = loggedIn.UserID;
                Logger.Info($"Login successful: {user.Username} (ID={loggedIn.UserID})");
            }
            return loggedIn;
        }

        public async Task<User?> Register(User user)
        {
            Logger.Info($"Register attempt: {user.Username}");
            var result = await _client.PostAsJsonAsync("user/", new
            {
                name = user.Username,
                password = user.passwordHash
            });
            if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                Logger.Warn($"Register failed: username '{user.Username}' already taken");
                return null;
            }
            result.EnsureSuccessStatusCode();
            var registered = await result.Content.ReadFromJsonAsync<User>();
            if (registered != null)
            {
                CurrentUserId = registered.UserID;
                Logger.Info($"Register successful: {user.Username} (ID={registered.UserID})");
            }
            return registered;
        }

        public async Task<List<Library>> GetLibraries()
        {
            Logger.Info($"Loading libraries for user {CurrentUserId}");
            var libs = await _client.GetFromJsonAsync<List<Library>>($"library/users/{CurrentUserId}/libraries/") ?? new();
            Logger.Info($"Loaded {libs.Count} libraries");
            return libs;
        }

        public async Task<Library> CreateLibrary(Library library)
        {
            Logger.Info($"Creating library '{library.Name}' (type={library.FileType})");
            var result = await _client.PostAsJsonAsync($"library/users/{CurrentUserId}/libraries/", new
            {
                name = library.Name,
                fileType = library.FileType,
                isPublic = library.IsPublic
            });
            result.EnsureSuccessStatusCode();
            var created = await result.Content.ReadFromJsonAsync<Library>() ?? library;
            Logger.Info($"Library created: '{created.Name}' (ID={created.LibraryID})");
            return created;
        }

        public async Task DeleteLibrary(int libraryId)
        {
            Logger.Info($"Deleting library ID={libraryId}");
            var result = await _client.DeleteAsync($"library/{libraryId}");
            result.EnsureSuccessStatusCode();
            Logger.Info($"Library ID={libraryId} deleted");
        }

        public async Task PublishLibrary(int libraryId, bool isPublic)
        {
            Logger.Info($"Library ID={libraryId} → isPublic={isPublic}");
            var result = await _client.PutAsJsonAsync($"library/{libraryId}", new { isPublic });
            result.EnsureSuccessStatusCode();
        }

        public async Task<List<Library>> GetPublicLibraries()
        {
            return await _client.GetFromJsonAsync<List<Library>>("library/public") ?? new();
        }

        public async Task<List<Media>> GetMedias(int libraryId)
        {
            Logger.Info($"Loading media for library ID={libraryId}");
            var medias = await _client.GetFromJsonAsync<List<Media>>($"libraries/{libraryId}/media/") ?? new();
            Logger.Info($"Loaded {medias.Count} media items for library ID={libraryId}");
            return medias;
        }

        public async Task<Media> CreateMedia(int libraryId, Media file)
        {
            Logger.Info($"Uploading file '{file.Name}' to library ID={libraryId}");
            using var form = new MultipartFormDataContent();
            var fileBytes = File.ReadAllBytes(file.FileUrl);
            form.Add(new ByteArrayContent(fileBytes), "file", file.Name);
            var uploadResult = await _client.PostAsync("upload", form);
            uploadResult.EnsureSuccessStatusCode();
            var uploadResponse = await uploadResult.Content.ReadFromJsonAsync<UploadResponse>();
            Logger.Info($"File uploaded → {uploadResponse!.url}");

            var result = await _client.PostAsJsonAsync($"libraries/{libraryId}/media/", new
            {
                name = file.Name,
                title = file.Title,
                mimeType = file.MimeType,
                mediaURL = uploadResponse!.url,
                coverURL = string.IsNullOrEmpty(file.CoverUrl) ? (string?)null : file.CoverUrl
            });
            result.EnsureSuccessStatusCode();
            var created = await result.Content.ReadFromJsonAsync<Media>() ?? file;
            Logger.Info($"Media created: '{created.Title}' (ID={created.MediaID})");
            return created;
        }

        public async Task ChangeMedia(Media file)
        {
            Logger.Info($"Updating media '{file.Title}' (ID={file.MediaID})");
            var result = await _client.PutAsJsonAsync($"libraries/{file.LibId}/media/{file.MediaID}", new
            {
                name = file.Name,
                title = file.Title,
                coverURL = string.IsNullOrEmpty(file.CoverUrl) ? (string?)null : file.CoverUrl
            });
            result.EnsureSuccessStatusCode();
            Logger.Info($"Media updated: '{file.Title}' (ID={file.MediaID})");
        }

        public async Task DeleteMedia(int fileId)
        {
            Logger.Info($"Deleting media ID={fileId}");
            var result = await _client.DeleteAsync($"media/{fileId}");
            result.EnsureSuccessStatusCode();
            Logger.Info($"Media ID={fileId} deleted");
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
