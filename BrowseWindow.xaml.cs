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

            string icon = lib.FileType switch
            {
                "Image" => "○",
                "Text"  => "🗎",
                _       => "▶"
            };

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
                        Width = 36, Height = 36, Padding = new Thickness(0),
                        Margin = new Thickness(8, 0, 0, 0),
                        ToolTip = "Open file",
                        Tag = media,
                        Content = new TextBlock { Text = icon, FontSize = 13 }
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
            var lib = (Library)((Button)sender).Tag;

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
    }
}
