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
                AddNewFileToLib();
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
                AddNewLibToCollection();
            }
        }

        private void AddNewLibToCollection()
        {
            MessageBox.Show("New lib added to Colletion");
            //ListViewLibraries.Items.Add();
        }

        private void AddNewFileToLib()
        {

        }

        private void ChangeFileInLib()
        {

        }
        private void DeleteSelectedFile()
        {
            MessageBox.Show("{filename} has been deleted");
        }

        private string GetDataTypeLib()
        {
            return "TestDataType";
        }
    }
}