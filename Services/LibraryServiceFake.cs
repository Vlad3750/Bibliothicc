using Bibliothicc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bibliothicc.Services
{
    public class LibraryServiceFake : ILibraryService
    {
        private readonly List<Library> _libraries;
        private readonly List<Category> _categories;
        private int _nextMediaId = 10;
        private int _nextLibId = 10;
        private int _nextCategoryId = 10;

        public LibraryServiceFake()
        {
            var cat1 = new Category { CategoryID = 1, Name = "Nature" };
            var cat2 = new Category { CategoryID = 2, Name = "Travel" };
            _nextCategoryId = 3;

            var media1 = new Media
            {
                MediaID = 1, Name = "sunset.jpg", Title = "Sunset",
                MimeType = ".jpg", FileUrl = "C:/Users/Vlad/Documents/Schule/POS/Bibliothicc/FakeLibPics/picture.jpg",
                LibId = 1, CategoryList = new() { cat1 }
            };
            var media2 = new Media
            {
                MediaID = 2, Name = "ocean.mp4", Title = "Ocean Waves",
                MimeType = ".mp4", FileUrl = "", LibId = 1
            };
            var media3 = new Media
            {
                MediaID = 3, Name = "chill.mp3", Title = "Chill Song",
                MimeType = ".mp3", FileUrl = "", LibId = 2
            };
            _nextMediaId = 4;

            _libraries = new()
            {
                new Library { LibraryID = 1, Name = "Photos", FileType = "Image", IsPublic = true,  mediaCollection = new() { media1, media2 } },
                new Library { LibraryID = 2, Name = "Music",  FileType = "Audio", IsPublic = false, mediaCollection = new() { media3 } },
            };
            _nextLibId = 3;

            _categories = new() { cat1, cat2 };
        }

        public Task<User?> Login(User user)
            => Task.FromResult<User?>(new User { UserID = 1, Username = user.Username });

        public Task<List<Library>> GetLibraries() => Task.FromResult(_libraries);

        public Task<Library> CreateLibrary(Library library)
        {
            library.LibraryID = _nextLibId++;
            library.mediaCollection ??= new();
            _libraries.Add(library);
            return Task.FromResult(library);
        }

        public Task PublishLibrary(int libraryId, bool isPublic)
        {
            var lib = _libraries.FirstOrDefault(l => l.LibraryID == libraryId);
            if (lib != null) lib.IsPublic = isPublic;
            return Task.CompletedTask;
        }

        public Task<List<Library>> GetPublicLibraries()
            => Task.FromResult(_libraries.Where(l => l.IsPublic).ToList());

        public Task<List<Media>> GetMedias(int libraryId)
        {
            var lib = _libraries.FirstOrDefault(l => l.LibraryID == libraryId);
            return Task.FromResult(lib?.mediaCollection ?? new());
        }

        public Task<Media> CreateMedia(int libraryId, Media file)
        {
            var lib = _libraries.FirstOrDefault(l => l.LibraryID == libraryId)
                ?? throw new System.Exception($"Library {libraryId} not found");
            file.MediaID = _nextMediaId++;
            file.LibId = libraryId;
            lib.mediaCollection.Add(file);
            return Task.FromResult(file);
        }

        public Task ChangeMedia(Media file)
        {
            foreach (var lib in _libraries)
            {
                var existing = lib.mediaCollection.FirstOrDefault(m => m.MediaID == file.MediaID);
                if (existing != null)
                {
                    existing.Name = file.Name;
                    existing.Title = file.Title;
                    existing.CoverUrl = file.CoverUrl;
                    existing.CategoryList = file.CategoryList;
                    return Task.CompletedTask;
                }
            }
            throw new System.Exception($"Media {file.MediaID} not found");
        }

        public Task DeleteMedia(int fileId)
        {
            foreach (var lib in _libraries)
            {
                var m = lib.mediaCollection.FirstOrDefault(m => m.MediaID == fileId);
                if (m != null) { lib.mediaCollection.Remove(m); return Task.CompletedTask; }
            }
            throw new System.Exception($"Media {fileId} not found");
        }

        public async Task<byte[]> DownloadFile(string mediaUrl)
        {
            if (System.IO.File.Exists(mediaUrl))
                return await System.IO.File.ReadAllBytesAsync(mediaUrl);
            return Array.Empty<byte>();
        }

        public Task<List<Category>> GetCategories() => Task.FromResult(_categories);

        public Task CreateCategory(Category category)
        {
            category.CategoryID = _nextCategoryId++;
            _categories.Add(category);
            return Task.CompletedTask;
        }

        public Task ChangeCategory(Category category)
        {
            var existing = _categories.FirstOrDefault(c => c.CategoryID == category.CategoryID);
            if (existing != null) existing.Name = category.Name;
            return Task.CompletedTask;
        }
    }
}
