using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bibliothicc.Models
{
    public class Media
    {
        [JsonPropertyName("mediaID")]
        public int MediaID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("coverURL")]
        public string CoverUrl { get; set; } = string.Empty;
        [JsonPropertyName("mediaURL")]
        public string FileUrl { get; set; } = string.Empty;
        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; } = string.Empty;
        [JsonPropertyName("lib_id")]
        public int LibId { get; set; }
        [JsonIgnore]
        public List<Category> CategoryList { get; set; } = new();
        [JsonIgnore]
        public User? UploadedBy { get; set; }
    }
}
