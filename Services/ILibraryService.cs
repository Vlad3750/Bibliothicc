using Bibliothicc.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bibliothicc.Services
{
    public interface ILibraryService
    {
        // User
        Task<User?> Login(User user);
        Task<User?> Register(User user);

        // Library

        // Admin methods
        Task<List<(Library lib, string ownerName)>> GetAllLibrariesWithOwner();
        Task AdminUnpublishLibrary(int libraryId);

        Task<List<Library>> GetLibraries();

        Task<Library> CreateLibrary(Library library);
        Task DeleteLibrary(int libraryId);
        Task PublishLibrary(int libraryId, bool isPublic);
        Task<List<Library>> GetPublicLibraries();

        // Media
        Task<List<Media>> GetMedias(int libraryId);
        Task<Media> CreateMedia(int libraryId, Media file);
        Task ChangeMedia(Media file);
        Task DeleteMedia(int fileId);

        // Files
        Task<byte[]> DownloadFile(string mediaUrl);

        // Category
        Task<List<Category>> GetCategories();
        Task CreateCategory(Category category);
        Task ChangeCategory(Category category);
    }
}
