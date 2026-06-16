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

            if (string.IsNullOrWhiteSpace(TextBoxUserName.Text))
            {
                TextBoxUserName.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];

            }
            else if (string.IsNullOrWhiteSpace(PasswordBoxPasswd.Password))
            {
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
            }
            else if(string.IsNullOrWhiteSpace(PasswordBoxRepeatPasswd.Password))
            {
                PasswordBoxRepeatPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
            }
            else if (PasswordBoxRepeatPasswd.Password != PasswordBoxPasswd.Password)
            {
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                PasswordBoxRepeatPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
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
                TextBoxUserName.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
            }
            else if (PasswordBoxPasswd.Password == string.Empty)
            {
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
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

        private void TextBoxUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxUserName.Text))
            {
                TextBoxUserName.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
            }
        }

        private void PasswordBoxPasswd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PasswordBoxPasswd.Password))
            {
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
            }
        }

        private void PasswordBoxRepeatPasswd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];

            if (!string.IsNullOrWhiteSpace(PasswordBoxRepeatPasswd.Password))
            {
                PasswordBoxRepeatPasswd.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
            }
        }
    }
}
