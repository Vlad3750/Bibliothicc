using Bibliothicc.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace Bibliothicc.Services
{
    public interface ILibraryService
    {
        // Library
        Task<List<Library>> GetLibraries();
        Task CreateLibrary(Library library);


        // Media
        Task<List<Media>> GetFiles(int libraryId);
        Task CreateFile(int libraryId, Media file);
        Task ChangeFile(Media file);
        Task DeleteFile(int fileId);


        // Category
        Task<List<Category>> GetCategories();
        Task CreateCategory(Category category);
        Task ChangeCategory(Category category);
    }
}
