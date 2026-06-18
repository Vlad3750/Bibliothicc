using Bibliothicc.Models;
using Bibliothicc.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Bibliothicc
{
    public partial class LoginRegisterWindow : Window
    {
        bool IsActiveLogin = true;

        public LoginRegisterWindow()
        {
            InitializeComponent();
        }

        private void SwitchToLogin()
        {
            IsActiveLogin = true;
            GridRepeatPasswd.Visibility = Visibility.Collapsed;
            LabelLoginRegister.Text = "Welcome back";
            ButtonLogin.IsDefault = true;
            ButtonRegister.IsDefault = false;
            ButtonLogin.Style = (Style)Application.Current.Resources["AccentButton"];
            ButtonRegister.Style = (Style)Application.Current.Resources["GhostButton"];
        }

        private void SwitchToRegister()
        {
            IsActiveLogin = false;
            GridRepeatPasswd.Visibility = Visibility.Visible;
            LabelLoginRegister.Text = "Create account";
            ButtonRegister.IsDefault = true;
            ButtonLogin.IsDefault = false;
            ButtonRegister.Style = (Style)Application.Current.Resources["AccentButton"];
            ButtonLogin.Style = (Style)Application.Current.Resources["GhostButton"];
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (IsActiveLogin)
                TryLogin();
            else
                SwitchToLogin();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!IsActiveLogin)
                TryRegister();
            else
                SwitchToRegister();
        }

        private async void TryLogin()
        {
            if (string.IsNullOrWhiteSpace(TextBoxUserName.Text))
            {
                TextBoxUserName.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                return;
            }
            if (string.IsNullOrWhiteSpace(PasswordBoxPasswd.Password))
            {
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                return;
            }

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var user = await App.Service.Login(new User
                {
                    Username = TextBoxUserName.Text,
                    passwordHash = PasswordBoxPasswd.Password
                });

                if (user == null)
                {
                    Logger.Warn($"Login failed: wrong credentials for '{TextBoxUserName.Text}'");
                    CustomMessageBox.Show("Username or Password is incorrect!", this, "❌");
                    return;
                }

                Logger.Info($"User '{user.Username}' logged in");
                App.CurrentUser = user;
                var window = new MainWindow(user);
                window.Show();
                this.Close();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private async void TryRegister()
        {
            if (string.IsNullOrWhiteSpace(TextBoxUserName.Text))
            {
                TextBoxUserName.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                return;
            }
            if (string.IsNullOrWhiteSpace(PasswordBoxPasswd.Password))
            {
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                return;
            }
            if (PasswordBoxRepeatPasswd.Password != PasswordBoxPasswd.Password)
            {
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                PasswordBoxRepeatPasswd.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                CustomMessageBox.Show("Passwords do not match!", this, "❌");
                return;
            }

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var user = await App.Service.Register(new User
                {
                    Username = TextBoxUserName.Text,
                    passwordHash = PasswordBoxPasswd.Password
                });

                if (user == null)
                {
                    Logger.Warn($"Register failed: username '{TextBoxUserName.Text}' already taken");
                    CustomMessageBox.Show($"Username \"{TextBoxUserName.Text}\" is already taken.", this, "❌");
                    return;
                }

                Logger.Info($"User '{user.Username}' registered successfully");
                App.CurrentUser = user;
                CustomMessageBox.Show("Registration successful!", this);
                var window = new MainWindow(user);
                window.Show();
                this.Close();
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void TextBoxUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxUserName.Text))
                TextBoxUserName.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
        }

        private void PasswordBoxPasswd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PasswordBoxPasswd.Password))
                PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
        }

        private void PasswordBoxRepeatPasswd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBoxPasswd.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
            if (!string.IsNullOrWhiteSpace(PasswordBoxRepeatPasswd.Password))
                PasswordBoxRepeatPasswd.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
        }
    }
}
