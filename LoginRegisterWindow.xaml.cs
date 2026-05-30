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
        List<User> users;
        User LoginUser = null;

        public LoginRegisterWindow(List<User> users)
        {
            InitializeComponent();
            this.users = users;
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
                MessageBox.Show("Username Required!");
            }
            else if (TextBoxPasswd.Text == string.Empty)
            {
                MessageBox.Show("Password Required!");
            }
            else if(TextBoxRepeatPasswd.Text == string.Empty)
            {
                MessageBox.Show("Please repead your Password!");
            }
            else if (TextBoxRepeatPasswd.Text != TextBoxPasswd.Text)
            {
                MessageBox.Show("Passwords don't align.");
            }
            else if (userAlreadyExists)
            {
                MessageBox.Show($"Username {TextBoxUserName.Text} already taken please choose another Name.");
            }
            else
            {
                User userToAdd = new User()
                {
                    Username = TextBoxUserName.Text,
                    passwordHash = TextBoxPasswd.Text,
                };
                users.Add(userToAdd);
                LoginUser = userToAdd;
                MessageBox.Show("You're now Registered");
                MainWindow window = new MainWindow(users, LoginUser);
                window.Show();
                this.Close();
            }
        }
        private void TryLogin()
        {
            User existingUser = new User();
            bool userNameExists = false;
            bool userPasswordAligns = false;

            foreach(User user in users)
            {
                if(TextBoxUserName.Text == user.Username)
                {
                    userNameExists = true;
                    existingUser = user;
                    break;
                }
            }
            if (userNameExists)
            {
                foreach(User user in users)
                {
                    if(existingUser.passwordHash == user.passwordHash)
                    {
                        userPasswordAligns = true;
                        LoginUser = user;
                        break;
                    }
                }
            }

            if(TextBoxUserName.Text == string.Empty)
            {
                MessageBox.Show("Username Required!");
            }
            else if (TextBoxPasswd.Text == string.Empty)
            {
                MessageBox.Show("Password Required!");
            }
            else if (!userPasswordAligns)
            {
                MessageBox.Show("Username or Password is false!");
            }
            else
            {
                MessageBox.Show("Login successful");
                MainWindow window = new MainWindow(users, LoginUser);
                window.Show();
                this.Close();
            }
        }
    }
}
