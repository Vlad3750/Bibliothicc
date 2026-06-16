using Bibliothicc.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bibliothicc
{
    public partial class BrowseWindow : Window
    {
        private List<Library> _publicLibraries = new();

        public BrowseWindow()
        {
            InitializeComponent();
            Loaded += BrowseWindow_Loaded;
        }

        private async void BrowseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                _publicLibraries = await App.Service.GetPublicLibraries();
                ListViewPublicLibraries.ItemsSource = _publicLibraries;
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
            if (ListViewPublicLibraries.SelectedItem is not Library lib) return;

            TextBlockLibraryName.Text = lib.Name;
            TextBlockLibraryType.Text = lib.FileType;

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var mediaList = await App.Service.GetMedias(lib.LibraryID);
                ListViewMedia.ItemsSource = mediaList;
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
            var lib = (Library)((Button)sender).Tag;

            var dialog = new OpenFolderDialog
            {
                Title = $"Choose folder to save '{lib.Name}'"
            };
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
                    var path = Path.Combine(folder, media.Name);
                    await File.WriteAllBytesAsync(path, bytes);
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
    }
}
