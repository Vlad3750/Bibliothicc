using Bibliothicc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bibliothicc.Services
{
    public class LibraryServiceFake : ILibraryService
    {
        private Library fakelibrarypics = new Library(new List<Media>
        {
            new Media() { Name = "picture", Title = "Subaru", FileURL = new Uri("C:/Users/Vlad/Documents/Schule/POS/Bibliothicc/FakeLibPics/picture.jpg")},
            // new Media() 
        });

        public Task ChangeCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public Task ChangeFile(Media file)
        {
            throw new NotImplementedException();
        }

        public Task CreateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task CreateFile(int libraryId, Media file)
        {
            fakelibrarypics.Collection.Add(file);
        }

        public Task CreateLibrary(Library library)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(int fileId)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            return obj is LibraryServiceFake fake &&
                   EqualityComparer<Library>.Default.Equals(fakelibrarypics, fake.fakelibrarypics);
        }

        public Task<List<Category>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<List<Media>> GetFiles(int libraryId)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(fakelibrarypics);
        }

        public Task<List<Library>> GetLibraries()
        {
            throw new NotImplementedException();
        }
    }
}
