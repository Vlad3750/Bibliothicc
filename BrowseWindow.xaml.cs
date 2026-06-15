using Bibliothicc_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bibliothicc
{
    /// <summary>
    /// Interaktionslogik für BrowseWindow.xaml
    /// </summary>
    public partial class BrowseWindow : Window
    {
        List<User> users;
        User currentUser;
        public BrowseWindow(List<User> users, User currentUser)
        {
            InitializeComponent();

            this.users = users;
            this.currentUser = currentUser;
        }
    }
}
