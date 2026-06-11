using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bibliothicc.Models
{
    public class Library
    {
        public int LibraryID;
        public string Name;
        public List<Media> Collection;

        public Library(List<Media> col)
        {
            collection = col;
        }

    }
}
