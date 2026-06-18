using Bibliothicc.Models;
using Bibliothicc.Services;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bibliothicc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool LoggedOn = false;
        bool isLightModeOn = true;
        bool isGridView = false;
        System.Windows.Controls.Border? _selectedCard = null;

        public List<Library> libs;

        Library currentLib;
        User currentUser;


        public MainWindow(User loggedUser)
        {
            InitializeComponent();

            this.libs = new List<Library>();
            this.currentUser = loggedUser;
            LabelUserName.Content = loggedUser.Username;

            Loaded += async (_, __) =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    libs = await App.Service.GetLibraries();
                    foreach (var lib in libs)
                    {
                        lib.mediaCollection = await App.Service.GetMedias(lib.LibraryID);
                        ListViewLibraries.Items.Add(lib);
                    }
                    if (libs.Count > 0)
                    {
                        ListViewLibraries.SelectedIndex = 0;
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Error("Failed to load libraries/media", ex);
                    CustomMessageBox.Show($"Load error: {ex.Message}", this, "❌");
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            };
        }

        private void ButtonQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void ButtonAddFile_Click(object sender, RoutedEventArgs e)
        {
            if (currentLib == null) { CustomMessageBox.Show("Please create a library first.", this, "⚠️"); return; }
            string LabelAddChange = "Add ";
            string LabelDataType = GetDataTypeLib() + ":";
            string ButtonContentAddChange = "Add";

            AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, ButtonContentAddChange, true, currentLib.FileType, currentUser.SystemCategories);

            if (window.ShowDialog() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    if (currentLib.LibraryID == 0)
                    {
                        var createdLib = await App.Service.CreateLibrary(currentLib);
                        currentLib.LibraryID = createdLib.LibraryID;
                    }
                    var created = await App.Service.CreateMedia(currentLib.LibraryID, window.itemToAdd);
                    window.itemToAdd.MediaID = created.MediaID;
                    window.itemToAdd.FileUrl = created.FileUrl;
                    window.itemToAdd.LibId = created.LibId;
                    currentLib.mediaCollection.Add(window.itemToAdd);
                    RefreshCategoryComboBox();
                    RefreshFileList();
                }
                catch (System.Exception ex)
                {
                    Logger.Error("Failed to add media", ex);
                    CustomMessageBox.Show($"Save failed: {ex.Message}", this, "❌");
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private async void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            if (currentLib == null) { CustomMessageBox.Show("Please create a library first.", this, "⚠️"); return; }
            if (ListViewFiles.SelectedItem == null)
            {
                CustomMessageBox.Show("Please select a file to change it", this);
                return;
            }

            int idx = ListViewFiles.SelectedIndex;
            string LabelAddChange = "Change ";
            string LabelDataType = GetDataTypeLib() + ":";
            string ButtonContentAddChange = "Change";

            AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, currentLib.mediaCollection[idx], ButtonContentAddChange, false, currentLib.FileType, currentUser.SystemCategories);

            if (window.ShowDialog() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    currentLib.mediaCollection[idx] = window.itemToChange;
                    await App.Service.ChangeMedia(window.itemToChange);
                    RefreshFileList();
                }
                catch (System.Exception ex)
                {
                    Logger.Error("Failed to update media", ex);
                    CustomMessageBox.Show($"Save failed: {ex.Message}", this, "❌");
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private async void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentLib == null) { CustomMessageBox.Show("Please create a library first.", this, "⚠️"); return; }
            if (ListViewFiles.SelectedItem == null) { CustomMessageBox.Show("Please select a file to delete it", this); return; }

            int idx = ListViewFiles.SelectedIndex;
            var media = currentLib.mediaCollection[idx];

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (media.MediaID != 0)
                    await App.Service.DeleteMedia(media.MediaID);
                currentLib.mediaCollection.RemoveAt(idx);
                RefreshCategoryComboBox();
                RefreshFileList();
            }
            catch (System.Exception ex)
            {
                Logger.Error("Failed to delete media", ex);
                CustomMessageBox.Show($"Delete failed: {ex.Message}", this, "❌");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ButtonLoginLogout_Click(object sender, RoutedEventArgs e)
        {
            LoggedOn = false;
            App.CurrentUser = null;
            LoginRegisterWindow window = new LoginRegisterWindow();
            window.Show();
            Close();
        }

        private async void ButtonAddLib_Click(object sender, RoutedEventArgs e)
        {
            AddLibWindow window = new AddLibWindow();

            if (window.ShowDialog() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    Library newLib = new Library()
                    {
                        Name = window.TextBoxLibName.Text,
                        FileType = window.fileNameString
                    };
                    var created = await App.Service.CreateLibrary(newLib);
                    newLib.LibraryID = created.LibraryID;

                    libs.Add(newLib);
                    ListViewLibraries.Items.Add(newLib);
                    currentLib = newLib;
                    ListViewLibraries.SelectedItem = newLib;
                }
                catch (System.Exception ex)
                {
                    Logger.Error("Failed to create library", ex);
                    CustomMessageBox.Show($"Save failed: {ex.Message}", this, "❌");
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }


        private string GetDataTypeLib()
        {
            return currentLib.FileType;
        }

        private void TextBoxSearchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            if(TextBoxSearchBar.Text == "Search here ..." && TextBoxSearchBar.Foreground == (SolidColorBrush)Application.Current.Resources["TextSecondaryBrush"])
            {
                TextBoxSearchBar.Text = "";
            }
            TextBoxSearchBar.Foreground = (SolidColorBrush)Application.Current.Resources["TextPrimaryBrush"];
        }

        private void TextBoxSearchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            if(TextBoxSearchBar.Text == string.Empty)
            {
                TextBoxSearchBar.Text = "Search here ...";
                TextBoxSearchBar.Foreground = (SolidColorBrush)Application.Current.Resources["TextSecondaryBrush"];
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindows window = new SettingsWindows(isLightModeOn);
            window.ShowDialog();

            isLightModeOn = window.isDark;
            if (isLightModeOn)
            {
                var img = new System.Windows.Controls.Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Bilder/log_out_icon_white.png")),
                    Width = 16,
                    Height = 16,
                    Stretch = Stretch.Uniform
                };
                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                ButtonLoginLogout.Content = img;
            }
            else
            {
                var img = new System.Windows.Controls.Image()
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Bilder/log_out_icon.png")),
                    Width = 16,
                    Height = 16,
                    Stretch = Stretch.Uniform
                };
                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                ButtonLoginLogout.Content = img;
            }

        }

        private void ListViewLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewLibraries.SelectedItem is not Library lib) return;
            currentLib = lib;
            TextBlockPublishLabel.Text = currentLib.IsPublic ? "Unpublish" : "Publish";

            if(currentLib.FileType == "Text")
            {
                DockPanelCategory.Visibility = Visibility.Collapsed;
                TextBlockPlay.Text = "🗎";
                TextBlockPlay.FontSize = 25;
            }
            else if (currentLib.FileType == "Image")
            {
                DockPanelCategory.Visibility = Visibility.Collapsed;
                TextBlockPlay.Text = "▣";
                TextBlockPlay.FontSize = 25;
            }
            else if(currentLib.FileType == "Audio")
            {
                DockPanelCategory.Visibility = Visibility.Collapsed;
                TextBlockPlay.Text = "▶";
                TextBlockPlay.FontSize = 13;
            }
            else
            {
                DockPanelCategory.Visibility = Visibility.Visible;
                ButtonPlay.Visibility = Visibility.Visible;
                TextBlockPlay.Text = "▶";
                TextBlockPlay.FontSize = 13;
            }

            RefreshCategoryComboBox();
            RefreshFileList();
        }

        // AI (Claude)
        // Start
        private void RefreshFileList(string searchText = "", string categoryFilter = "All Categories")
        {
            ListViewFiles.Items.Clear();
            WrapPanelGrid.Children.Clear();
            _selectedCard = null;

            foreach (Media media in currentLib.mediaCollection)
            {
                bool matchesSearch = string.IsNullOrEmpty(searchText)
                    || media.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase);

                bool matchesCategory = categoryFilter == "All Categories"
                    || (media.CategoryList != null
                        && media.CategoryList.Any(c => c.Name == categoryFilter));

                if (matchesSearch && matchesCategory)
                {
                    ListViewFiles.Items.Add(media);
                    WrapPanelGrid.Children.Add(CreateGridCard(media));
                }
            }
        }

        private System.Windows.Controls.Border CreateGridCard(Media media)
        {
            int index = WrapPanelGrid.Children.Count;

            var card = new System.Windows.Controls.Border
            {
                Width = 120, Height = 140, Margin = new Thickness(6),
                CornerRadius = new CornerRadius(8),
                ClipToBounds = true,
                BorderThickness = new Thickness(1),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            card.SetResourceReference(System.Windows.Controls.Border.BackgroundProperty, "BgSurfaceBrush");
            card.SetResourceReference(System.Windows.Controls.Border.BorderBrushProperty, "BorderBrush2");

            card.MouseLeftButtonUp += (_, __) =>
            {
                // Deselect previous
                if (_selectedCard != null)
                {
                    _selectedCard.BorderBrush = (System.Windows.Media.Brush)Application.Current.Resources["BorderBrush2"];
                    _selectedCard.BorderThickness = new Thickness(1);
                }
                // Select this card
                card.BorderBrush = (System.Windows.Media.Brush)Application.Current.Resources["AccentBrush"];
                card.BorderThickness = new Thickness(2);
                _selectedCard = card;
                // Sync with list view
                ListViewFiles.SelectedIndex = index;
            };

            var stack = new StackPanel();

            var imgBorder = new System.Windows.Controls.Border
            {
                Height = 90,
                Clip = new System.Windows.Media.RectangleGeometry(new Rect(0, 0, 120, 90), 8, 8)
            };
            imgBorder.SetResourceReference(System.Windows.Controls.Border.BackgroundProperty, "BgElevatedBrush");

            string thumbUrl = media.CoverUrl;
            if (string.IsNullOrEmpty(thumbUrl) && currentLib.FileType == "Image")
                thumbUrl = media.FileUrl;

            if (!string.IsNullOrEmpty(thumbUrl))
            {
                try
                {
                    string uri = thumbUrl;
                    if (uri.StartsWith("/") && !System.IO.File.Exists(uri))
                        uri = $"http://bibliothicc.duckdns.org:8000{thumbUrl}";

                    var bmp = new System.Windows.Media.Imaging.BitmapImage();
                    bmp.BeginInit();
                    bmp.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                    bmp.DecodePixelWidth = 120;
                    bmp.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    bmp.EndInit();

                    imgBorder.Child = new System.Windows.Controls.Image
                    {
                        Source = bmp,
                        Stretch = System.Windows.Media.Stretch.UniformToFill,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }
                catch
                {
                    imgBorder.Child = MakePlaceholder();
                }
            }
            else
            {
                imgBorder.Child = MakePlaceholder();
            }

            stack.Children.Add(imgBorder);
            var titleBlock = new TextBlock
            {
                Text = media.Title,
                FontSize = 11,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Margin = new Thickness(6, 5, 6, 4),
                TextWrapping = TextWrapping.NoWrap
            };
            titleBlock.SetResourceReference(TextBlock.ForegroundProperty, "TextPrimaryBrush");
            stack.Children.Add(titleBlock);

            card.Child = stack;
            return card;
        }

        private TextBlock MakePlaceholder() => new TextBlock
        {
            Text = "🖼",
            FontSize = 28,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        private void ListViewFiles_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.ScrollViewer ||
                e.OriginalSource is System.Windows.Controls.ListView)
                ListViewFiles.SelectedIndex = -1;
        }

        private void WrapPanelGrid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Deselect if click didn't land on a card or its children
            var hit = e.OriginalSource as System.Windows.DependencyObject;
            while (hit != null)
            {
                if (hit == _selectedCard || WrapPanelGrid.Children.Contains(hit as UIElement))
                    return; // clicked on a card — handled by card's own event
                hit = System.Windows.Media.VisualTreeHelper.GetParent(hit);
            }

            if (_selectedCard != null)
            {
                _selectedCard.SetResourceReference(System.Windows.Controls.Border.BorderBrushProperty, "BorderBrush2");
                _selectedCard.BorderThickness = new Thickness(1);
                _selectedCard = null;
            }
            ListViewFiles.SelectedIndex = -1;
        }

        private void ButtonToggleView_Click(object sender, RoutedEventArgs e)
        {
            isGridView = !isGridView;
            if (isGridView)
            {
                ListViewFiles.Visibility = Visibility.Collapsed;
                ScrollViewerGrid.Visibility = Visibility.Visible;
                TextBlockToggleView.Text = "☰";
            }
            else
            {
                ListViewFiles.Visibility = Visibility.Visible;
                ScrollViewerGrid.Visibility = Visibility.Collapsed;
                TextBlockToggleView.Text = "⊞";
            }
        }
        private void RefreshCategoryComboBox()
        {
            ComboBoxCategory.Items.Clear();
            ComboBoxCategory.Items.Add(new ComboBoxItem() { Content = "All Categories", IsSelected = true });

            var allCategories = currentLib.mediaCollection
                .Where(m => m.CategoryList != null)
                .SelectMany(m => m.CategoryList)
                .Select(c => c.Name)
                .Distinct()
                .OrderBy(n => n);

            foreach (string cat in allCategories)
            {
                ComboBoxCategory.Items.Add(new ComboBoxItem() { Content = cat });
            }

            ComboBoxCategory.SelectedIndex = 0;
        }

        private void TextBoxSearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchText = TextBoxSearchBar.Text == "Search here ..." ? "" : TextBoxSearchBar.Text;

                // Filter auf "All Categories" zurücksetzen
                ComboBoxCategory.SelectedIndex = 0;

                RefreshFileList(searchText, "All Categories");

                Keyboard.ClearFocus();
                FocusManager.SetFocusedElement(this, null);
            }
        }

        private void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Guard: ComboBox noch nicht initialisiert
            if (ComboBoxCategory.SelectedItem == null || currentLib == null) return;

            string selected = ((ComboBoxItem)ComboBoxCategory.SelectedItem).Content.ToString();
            string searchText = TextBoxSearchBar.Text == "Search here ..." ? "" : TextBoxSearchBar.Text;

            RefreshFileList(searchText, selected);
        }

        private async void ButtonDeleteLib_Click(object sender, RoutedEventArgs e)
        {
            var lib = (sender as System.Windows.Controls.Button)?.DataContext as Library;
            if (lib == null) return;

            var confirm = CustomMessageBox.ShowConfirm($"Delete library \"{lib.Name}\"? This cannot be undone.", this);
            if (!confirm) return;

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (lib.LibraryID != 0)
                    await App.Service.DeleteLibrary(lib.LibraryID);

                libs.Remove(lib);
                ListViewLibraries.Items.Remove(lib);

                if (libs.Count > 0)
                    ListViewLibraries.SelectedIndex = 0;
                else
                {
                    currentLib = null;
                    ListViewFiles.Items.Clear();
                }
            }
            catch (System.Exception ex)
            {
                Logger.Error("Failed to delete library", ex);
                CustomMessageBox.Show($"Delete failed: {ex.Message}", this, "❌");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            BrowseWindow window = new BrowseWindow(currentUser.IsAdmin);
            window.ShowDialog();
        }


        private async void ButtonPublish_Click(object sender, RoutedEventArgs e)
        {
            if (currentLib == null) { CustomMessageBox.Show("Please create a library first.", this, "⚠️"); return; }

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                currentLib.IsPublic = !currentLib.IsPublic;
                await App.Service.PublishLibrary(currentLib.LibraryID, currentLib.IsPublic);
                TextBlockPublishLabel.Text = currentLib.IsPublic ? "Unpublish" : "Publish";
            }
            catch (System.Exception ex)
            {
                currentLib.IsPublic = !currentLib.IsPublic;
                Mouse.OverrideCursor = null;
                Logger.Error($"Failed to publish library ID={currentLib.LibraryID}", ex);
                CustomMessageBox.Show($"Failed: {ex.Message}", this, "❌");
                return;
            }
            Mouse.OverrideCursor = null;
        }

        private async void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (currentLib == null) return;
            if (ListViewFiles.SelectedItem == null)
            {
                if(currentLib.FileType == "Image")
                {
                    CustomMessageBox.Show("Please select a file to display", this, "⚠️");
                }
                else if(currentLib.FileType == "Text")
                {
                    CustomMessageBox.Show("Please select a file to view", this, "⚠️");
                }
                else
                {
                    CustomMessageBox.Show("Please select a file to play", this, "⚠️");
                }
                return;
            }

            Media selectedMedia = currentLib.mediaCollection[ListViewFiles.SelectedIndex];

            string filePath = selectedMedia.FileUrl;

            if (!System.IO.File.Exists(filePath))
            {
                // File is on server — download to temp and open
                if (string.IsNullOrEmpty(selectedMedia.FileUrl))
                {
                    CustomMessageBox.Show("File not found.", this, "⚠️");
                    return;
                }
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    var bytes = await App.Service.DownloadFile(selectedMedia.FileUrl);
                    filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), selectedMedia.Name);
                    await System.IO.File.WriteAllBytesAsync(filePath, bytes);
                }
                catch (System.Exception ex)
                {
                    Mouse.OverrideCursor = null;
                    Logger.Error($"Failed to download file '{selectedMedia.Name}'", ex);
                    CustomMessageBox.Show($"Download failed: {ex.Message}", this, "❌");
                    return;
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }

        // End
    }
}
