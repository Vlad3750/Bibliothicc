using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bibliothicc.Models
{
    public class Library
    {
        [JsonPropertyName("libID")]
        public int LibraryID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("fileType")]
        public string FileType { get; set; } = string.Empty;
        [JsonPropertyName("isPublic")]
        public bool IsPublic { get; set; }
        [JsonIgnore]
        public List<Media> mediaCollection { get; set; } = new();

        public Library() { }
        public Library(List<Media> col) { mediaCollection = col; }
    }
}
