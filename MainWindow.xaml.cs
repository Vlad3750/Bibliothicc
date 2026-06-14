using Bibliothicc_ClassLibrary;
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

        public List<Library> libs;
        List<User> users;

        Library currentLib;
        User currentUser;


        public MainWindow(List<User> users, List<Library> libs, User loggedUser)
        {
            InitializeComponent();

            this.users = users;
            this.libs = libs;
            this.currentUser = loggedUser;
            LabelUserName.Content = loggedUser.Username;

            if(libs.Count != 0)
            {
                currentLib = libs[0];
            }

            if (ListViewLibraries.Items.Count == 0)
            {
                ButtonAddLib_Click(ButtonAddLib, new RoutedEventArgs());
            }
        }

        private void ButtonQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAddFile_Click(object sender, RoutedEventArgs e)
        {
            string LabelAddChange = "Add ";
            string LabelDataType = GetDataTypeLib() + ":";
            string ButtonContentAddChange = "Add";

            AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, ButtonContentAddChange, true, currentLib.FileType, currentUser.SystemCategories);

            if (window.ShowDialog() == true)
            {
                currentLib.mediaCollection.Add(window.itemToAdd);
                RefreshCategoryComboBox();
                RefreshFileList();
            }
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewFiles.SelectedItem == null)
            {
                CustomMessageBox.Show("Please select a file to change it", this);
            }

            else
            {
                string LabelAddChange = "Change ";
                string LabelDataType = GetDataTypeLib() + ":";
                string ButtonContentAddChange = "Change";

                AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, currentLib.mediaCollection[ListViewFiles.SelectedIndex], ButtonContentAddChange, false, currentLib.FileType, currentUser.SystemCategories);

                if (window.ShowDialog() == true)
                {
                    currentLib.mediaCollection[ListViewFiles.SelectedIndex] = window.itemToChange;

                    ListViewItem changedFile = (ListViewItem)ListViewFiles.Items[ListViewFiles.SelectedIndex];
                    changedFile.Content = window.TextBoxFileName.Text;
                    ListViewFiles.Items.Refresh();
                }
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedFile();
        }

        private void ButtonLoginLogout_Click(object sender, RoutedEventArgs e)
        {
            LoggedOn = false;
            LoginRegisterWindow window = new LoginRegisterWindow(users);
            window.Show();
            Close();
        }

        private void ButtonAddLib_Click(object sender, RoutedEventArgs e)
        {
            AddLibWindow window = new AddLibWindow();

            if(window.ShowDialog() == true)
            {
                ListViewItem LibToAdd = new ListViewItem();
                Library LibToAddToColllection = new Library()
                {
                    Name = window.TextBoxLibName.Text,
                    FileType = window.fileNameString
                };

                LibToAdd.Content = window.TextBoxLibName.Text;
                ListViewLibraries.Items.Add(LibToAdd);
                ListViewLibraries.Items.Refresh();

                libs.Add(LibToAddToColllection);

                currentLib = LibToAddToColllection;
                ListViewLibraries.SelectedItem = LibToAdd;


                // MessageBox.Show(string.Join(", ", acceptedMimeTypes), LibToAddToColllection.FileType);
                CustomMessageBox.Show($"New Library for {window.fileNameString}s added to Collection", null);
            }
            else if(ListViewLibraries.Items.Count == 0)
            {
                CustomMessageBox.Show("You don't have any libraries, please create one", null, "⚠️");
                ButtonAddLib_Click(ButtonAddLib, new RoutedEventArgs());
            }
        }

        private void DeleteSelectedFile()
        {
            if(ListViewFiles.SelectedItem == null)
            {
                CustomMessageBox.Show("Please Select a File to delete it", this);
            }
            else
            {
                // AI (Claude)
                // Start
                int idx = ListViewFiles.SelectedIndex;
                string name = ((ListViewItem)ListViewFiles.SelectedItem).Content.ToString();
                currentLib.mediaCollection.RemoveAt(idx);
                CustomMessageBox.Show($"{name} has been deleted", this);
                RefreshCategoryComboBox();
                RefreshFileList();
                // End
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
            currentLib = libs[ListViewLibraries.SelectedIndex];

            if(currentLib.FileType == "Text" || currentLib.FileType == "Image")
            {
                DockPanelCategory.Visibility = Visibility.Collapsed;
                ButtonPlay.Visibility = Visibility.Hidden;
            }
            else
            {
                DockPanelCategory.Visibility = Visibility.Visible;
                ButtonPlay.Visibility = Visibility.Visible;
            }

            RefreshCategoryComboBox();
            RefreshFileList();
        }

        // AI (Claude)
        // Start
        private void RefreshFileList(string searchText = "", string categoryFilter = "All Categories")
        {
            ListViewFiles.Items.Clear();

            foreach (Media media in currentLib.mediaCollection)
            {
                // Suchfilter: Name enthält den Suchtext (case-insensitive)
                bool matchesSearch = string.IsNullOrEmpty(searchText)
                    || media.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase);

                // Kategoriefilter
                bool matchesCategory = categoryFilter == "All Categories"
                    || (media.CategoryList != null
                        && media.CategoryList.Any(c => c.Name == categoryFilter));

                if (matchesSearch && matchesCategory)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = media.Title;
                    ListViewFiles.Items.Add(item);
                }
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

        // End
    }
}