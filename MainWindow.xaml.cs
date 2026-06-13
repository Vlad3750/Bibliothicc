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
using Bibliothicc_ClassLibrary;

namespace Bibliothicc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool LoggedOn = false;
        List<User> users;

        Library currentLib;
        public List<Library> libs;

        public MainWindow(List<User> users, List<Library> libs, User loggedUser)
        {
            InitializeComponent();

            this.users = users;
            this.libs = libs;
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

            AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, ButtonContentAddChange, true, currentLib.FileType);

            if (window.ShowDialog() == true)
            {
                ListViewItem FileToAdd = new ListViewItem();

                FileToAdd.Content = window.TextBoxFileName.Text;
                ListViewFiles.Items.Add(FileToAdd);
                ListViewFiles.Items.Refresh();

                currentLib.mediaCollection.Add(window.itemToAdd);
            }
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewFiles.SelectedItem == null)
            {
                MessageBox.Show("Please select a file to change it");
            }

            else
            {
                string LabelAddChange = "Change ";
                string LabelDataType = GetDataTypeLib() + ":";
                string ButtonContentAddChange = "Change";

                AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, currentLib.mediaCollection[ListViewFiles.SelectedIndex], ButtonContentAddChange, false, currentLib.FileType);

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
                MessageBox.Show($"New Library for {window.fileNameString}s added to Collection");
            }
        }

        private void DeleteSelectedFile()
        {
            if(ListViewFiles.SelectedItem == null)
            {
                MessageBox.Show("Please Select a File to delete it");
            }
            else
            {
                MessageBox.Show($"{ListViewFiles.SelectedItem.ToString().Substring(37)} has been deleted");
                ListViewFiles.Items.Remove(ListViewFiles.SelectedItem);
                ListViewFiles.Items.Refresh();
            }
        }


        private string GetDataTypeLib()
        {
            return currentLib.FileType;
        }

        private void TextBoxSearchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            if(TextBoxSearchBar.Text == "Search here ..." && TextBoxSearchBar.Foreground == Brushes.LightGray)
            {
                TextBoxSearchBar.Text = "";
            }
            TextBoxSearchBar.Foreground = Brushes.Black;
        }

        private void TextBoxSearchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            if(TextBoxSearchBar.Text == string.Empty)
            {
                TextBoxSearchBar.Text = "Search here ...";
                TextBoxSearchBar.Foreground = Brushes.LightGray;
            }
        }

        private void ButtonSetttings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindows window = new SettingsWindows();
            window.Show();
        }

        private void ButtonStats_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListViewLibraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentLib = libs[ListViewLibraries.SelectedIndex];

            if(currentLib.FileType == "Text" || currentLib.FileType == "Image")
            {
                DockPanelCategory.Visibility = Visibility.Collapsed;
                ButtonPlay.Visibility = Visibility.Hidden;
                ButtonStats.Visibility = Visibility.Hidden;
            }
            else
            {
                DockPanelCategory.Visibility = Visibility.Visible;
                ButtonPlay.Visibility = Visibility.Visible;
                ButtonStats.Visibility = Visibility.Visible;
            }
        }
    }
}