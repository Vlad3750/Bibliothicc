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
    /// Interaktionslogik für SettingsWindows.xaml
    /// </summary>
    public partial class SettingsWindows : Window
    {
        public Image buttonAn = new Image();
        public Image buttonAus = new Image();
        bool button_state = false;


        public SettingsWindows()
        {
            InitializeComponent();

            buttonAn.Source = new BitmapImage(new Uri("pack://application:,,,/Bilder/button_an.png"));
            buttonAus.Source = new BitmapImage(new Uri("pack://application:,,,/Bilder/button_aus.png"));

            ButtonDarkmode.Content = buttonAus;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!button_state)
            {
                ButtonDarkmode.Content = buttonAn;
                button_state = true;
            }
            else if (button_state)
            {
                ButtonDarkmode.Content = buttonAus;
                button_state = false;
            }


        }
    }
}
