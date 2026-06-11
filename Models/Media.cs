using System;
using System.Collections.Generic;
using System.Text;

namespace Bibliothicc.Models
{
    public class Media
    {
        public int MediaID;
        public string Name;
        public string Title;
        public string Description;
        public string Cover;
        public Uri FileURL;
        public string MimeType;
        public List<Category> CategoryList;
        public User UploadedBy;
    }
}
