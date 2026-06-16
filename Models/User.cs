using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bibliothicc.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int UserID { get; set; }
        [JsonPropertyName("name")]
        public string Username { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        [JsonIgnore]
        public List<Category> SystemCategories { get; set; } = new();
    }
}
