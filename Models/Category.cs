using System.Text.Json.Serialization;

namespace Bibliothicc.Models
{
    public class Category
    {
        [JsonPropertyName("categoryID")]
        public int CategoryID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}
