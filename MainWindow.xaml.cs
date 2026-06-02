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

        public MainWindow(List<User> users, User loggedUser)
        {
            InitializeComponent();
            this.users = users;
            LabelUserName.Content = loggedUser.Username;
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

            AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, ButtonContentAddChange, true);

            if (window.ShowDialog() == true)
            {
                ListViewItem FileToAdd = new ListViewItem();

                FileToAdd.Content = window.TextBoxFileName.Text;
                ListViewFiles.Items.Add(FileToAdd);
                ListViewLibraries.Items.Refresh();
            }
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            string LabelAddChange = "Change ";
            string LabelDataType = GetDataTypeLib() + ":";
            string ButtonContentAddChange = "Change";

            AddChangeFileWindow window = new AddChangeFileWindow(LabelAddChange, LabelDataType, ButtonContentAddChange, false);

            if (window.ShowDialog() == true)
            {
                ChangeFileInLib();
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

                LibToAdd.Content = window.TextBoxLibName.Text;
                ListViewLibraries.Items.Add(LibToAdd);
                ListViewLibraries.Items.Refresh();
            }
        }

        private void AddNewFileToLib()
        {

        }

        private void ChangeFileInLib()
        {

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
            return "TestDataType";
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
    }
}