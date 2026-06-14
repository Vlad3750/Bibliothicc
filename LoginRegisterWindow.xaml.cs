using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bibliothicc_ClassLibrary;

namespace Bibliothicc
{
    /// <summary>
    /// Interaktionslogik für LoginRegisterWindow.xaml
    /// </summary>
    public partial class LoginRegisterWindow : Window
    {
        bool IsActiveLogin = true;
        List<User> users = new List<User>();
        List<Library> libs = new List<Library>();
        User LoginUser = null;

        public LoginRegisterWindow(List<User> users)
        {
            InitializeComponent();
            this.users = users;
        }
        public LoginRegisterWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (IsActiveLogin)
            {
                TryLogin();
            }
            else
            {
                GridRepeatPasswd.Visibility = Visibility.Collapsed;
                IsActiveLogin = true;
                ButtonRegister.IsDefault = true;
            }
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!IsActiveLogin)
            {
                TryRegister();
            }
            else
            {
                GridRepeatPasswd.Visibility = Visibility.Visible;
                IsActiveLogin = false;
                ButtonLogin.IsDefault = true;
            }
        }


        private void TryRegister()
        {
            bool userAlreadyExists = false;

            foreach(User user in users)
            {
                if(TextBoxUserName.Text == user.Username)
                {
                    userAlreadyExists = true;
                }
            }

            if (TextBoxUserName.Text == string.Empty)
            {
                CustomMessageBox.Show("Username Required!", this, "⚠️");
            }
            else if (PasswordBoxPasswd.Password == string.Empty)
            {
                CustomMessageBox.Show("Password Required!", this, "⚠️");
            }
            else if(PasswordBoxRepeatPasswd.Password == string.Empty)
            {
                CustomMessageBox.Show("Please repead your Password!", this, "⚠️");
            }
            else if (PasswordBoxRepeatPasswd.Password != PasswordBoxPasswd.Password)
            {
                CustomMessageBox.Show("Passwords don't align.", this, "⚠️");
            }
            else if (userAlreadyExists)
            {
                CustomMessageBox.Show($"Username {TextBoxUserName.Text} already taken please choose another Name.", this);
            }
            else
            {
                User userToAdd = new User()
                {
                    Username = TextBoxUserName.Text,
                    passwordHash = PasswordBoxPasswd.Password,
                };
                users.Add(userToAdd);
                LoginUser = userToAdd;
                CustomMessageBox.Show("You're now Registered", this);

                MainWindow window = new MainWindow(users, libs, LoginUser);
                window.Show();
                this.Close();
            }
        }
        private void TryLogin()
        {
            User existingUser = null;
            bool userPasswordAligns = false;

            foreach(User user in users)
            {
                if(TextBoxUserName.Text == user.Username)
                {
                    existingUser = user;
                    break;
                }
            }
            if (existingUser != null)
            {
                if(existingUser.passwordHash == PasswordBoxPasswd.Password)
                {
                    userPasswordAligns = true;
                    LoginUser = existingUser;
                }
            }

            if(TextBoxUserName.Text == string.Empty)
            {
                CustomMessageBox.Show("Username Required!", this, "⚠️");
            }
            else if (PasswordBoxPasswd.Password == string.Empty)
            {
                CustomMessageBox.Show("Password Required!", this, "⚠️");
            }
            else if (!userPasswordAligns)
            {
                CustomMessageBox.Show("Username or Password is false!", this, "❌");
            }
            else
            {
                CustomMessageBox.Show("Login successful", this);
                MainWindow window = new MainWindow(users, libs, LoginUser);
                window.Show();
                this.Close();
            }
        }
    }
}
