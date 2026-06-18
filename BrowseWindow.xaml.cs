using Bibliothicc.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Bibliothicc
{
    // Wrapper für Admin-Ansicht: Library + Besitzer-Name
    public class PublicLibraryEntry
    {
        public Library Library { get; set; }
        public string OwnerName { get; set; }
        public string DisplayName => $"{Library.Name}";
        public string FileType => Library.FileType;
        public string Name => Library.Name;
    }

    public partial class BrowseWindow : Window
    {
        private bool _isAdmin;
        private List<PublicLibraryEntry> _entries = new();

        public BrowseWindow(bool isAdmin = false)
        {
            InitializeComponent();
            _isAdmin = isAdmin;

            if (_isAdmin)
                BorderAdminPanel.Visibility = Visibility.Visible;

            Loaded += BrowseWindow_Loaded;
        }

        private async void BrowseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (_isAdmin)
                {
                    // Admin sieht alle public Libraries mit Owner
                    var withOwners = await App.Service.GetAllLibrariesWithOwner();
                    _entries = withOwners.Select(x => new PublicLibraryEntry
                    {
                        Library = x.lib,
                        OwnerName = x.ownerName
                    }).ToList();
                }
                else
                {
                    // Normaler User sieht nur public Libraries
                    var libs = await App.Service.GetPublicLibraries();
                    _entries = libs.Select(l => new PublicLibraryEntry
                    {
                        Library = l,
                        OwnerName = ""
                    }).ToList();
                }

                ListViewPublicLibraries.ItemsSource = null;
                ListViewPublicLibraries.ItemsSource = _entries;
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.Show($"Could not load libraries: {ex.Message}", this, "❌");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private async void ListViewPublicLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewPublicLibraries.SelectedItem is not PublicLibraryEntry entry) return;
            var lib = entry.Library;

            TextBlockLibraryName.Text = lib.Name;
            TextBlockLibraryType.Text = _isAdmin
                ? $"{lib.FileType}  ·  👤 {entry.OwnerName}"
                : lib.FileType;
            ButtonAdminUnpublish.Visibility = _isAdmin ? Visibility.Visible : Visibility.Collapsed;
            ButtonAdminUnpublish.Tag = entry;

            string icon = lib.FileType == "Image" ? "▣" : "▶";
            int iconSize = lib.FileType == "Image" ? 18 : 13;

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var mediaList = await App.Service.GetMedias(lib.LibraryID);
                ListViewMedia.Items.Clear();

                foreach (var media in mediaList)
                {
                    var btn = new Button
                    {
                        Style = (Style)Application.Current.Resources["AccentButton"],
                        Width = 36,
                        Height = 36,
                        Padding = new Thickness(0),
                        Margin = new Thickness(8, 0, 0, 0),
                        ToolTip = "Open file",
                        Tag = media,
                        Content = new TextBlock { Text = icon, FontSize = iconSize }
                    };
                    var cornerStyle = new Style(typeof(Border));
                    cornerStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(18)));
                    btn.Resources[typeof(Border)] = cornerStyle;
                    btn.Click += ButtonOpenFile_Click;

                    var title = new TextBlock
                    {
                        Text = media.Title,
                        FontSize = 13,
                        Foreground = (Brush)Application.Current.Resources["TextPrimaryBrush"]
                    };
                    var mime = new TextBlock
                    {
                        Text = media.MimeType,
                        FontSize = 10,
                        Foreground = (Brush)Application.Current.Resources["TextMutedBrush"]
                    };
                    var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                    stack.Children.Add(title);
                    stack.Children.Add(mime);

                    var dock = new DockPanel { Margin = new Thickness(0, 2, 0, 2) };
                    DockPanel.SetDock(btn, Dock.Right);
                    dock.Children.Add(btn);
                    dock.Children.Add(stack);

                    ListViewMedia.Items.Add(new ListViewItem
                    {
                        Content = dock,
                        Style = (Style)Application.Current.Resources["ModernListViewItem"]
                    });
                }
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.Show($"Could not load media: {ex.Message}", this, "❌");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        private async void ButtonDownloadLibrary_Click(object sender, RoutedEventArgs e)
        {
            var entry = (PublicLibraryEntry)((Button)sender).Tag;
            var lib = entry.Library;

            var dialog = new OpenFolderDialog { Title = $"Choose folder to save '{lib.Name}'" };
            if (dialog.ShowDialog() != true) return;
            string folder = dialog.FolderName;

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var mediaList = await App.Service.GetMedias(lib.LibraryID);
                foreach (var media in mediaList)
                {
                    if (string.IsNullOrEmpty(media.FileUrl)) continue;
                    var bytes = await App.Service.DownloadFile(media.FileUrl);
                    await File.WriteAllBytesAsync(Path.Combine(folder, media.Name), bytes);
                }
                CustomMessageBox.Show($"Downloaded {mediaList.Count} file(s) to:\n{folder}", this);
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.Show($"Download failed: {ex.Message}", this, "❌");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private async void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var media = (Media)((Button)sender).Tag;
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var bytes = await App.Service.DownloadFile(media.FileUrl);
                var tempPath = Path.Combine(Path.GetTempPath(), media.Name);
                await File.WriteAllBytesAsync(tempPath, bytes);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempPath,
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.Show($"Could not open file: {ex.Message}", this, "❌");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private async void ButtonAdminUnpublish_Click(object sender, RoutedEventArgs e)
        {
            if (!_isAdmin) return;
            var entry = (PublicLibraryEntry)((Button)sender).Tag;
            var lib = entry.Library;

            if (!CustomMessageBox.ShowConfirm($"Unpublish \"{lib.Name}\" von {entry.OwnerName}?", this)) return;

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                await App.Service.AdminUnpublishLibrary(lib.LibraryID);
                _entries.Remove(entry);
                ListViewPublicLibraries.ItemsSource = null;
                ListViewPublicLibraries.ItemsSource = _entries;
                ListViewMedia.Items.Clear();
                TextBlockLibraryName.Text = "Select a library";
                TextBlockLibraryType.Text = "";
                ButtonAdminUnpublish.Visibility = Visibility.Collapsed;
            }
            catch (System.Exception ex)
            {
                CustomMessageBox.Show($"Failed: {ex.Message}", this, "❌");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
    }
}