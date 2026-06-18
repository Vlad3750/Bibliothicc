using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Bibliothicc.Models
{
    public class Library : INotifyPropertyChanged
    {
        [JsonPropertyName("libID")]
        public int LibraryID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("fileType")]
        public string FileType { get; set; } = string.Empty;

        private bool _isPublic;
        [JsonPropertyName("isPublic")]
        public bool IsPublic
        {
            get => _isPublic;
            set { _isPublic = value; OnPropertyChanged(); }
        }

        [JsonIgnore]
        public List<Media> mediaCollection { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Library() { }
        public Library(List<Media> col) { mediaCollection = col; }
    }
}
