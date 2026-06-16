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
    /// Interaktionslogik für AddLibWindow.xaml
    /// </summary>
    public partial class AddLibWindow : Window
    {
        public string fileNameString;

        public AddLibWindow()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxLibName.Text))
            {
                TextBoxLibName.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
            }
            else
            {
                if (ComboBoxDataTyp.SelectedIndex == 0)
                {
                    fileNameString = "Video";
                }
                else if (ComboBoxDataTyp.SelectedIndex == 1)
                {
                    fileNameString = "Movie";
                }
                else if (ComboBoxDataTyp.SelectedIndex == 2)
                {
                    fileNameString = "Text";
                }
                else if (ComboBoxDataTyp.SelectedIndex == 3)
                {
                    fileNameString = "Image";
                }
                else if (ComboBoxDataTyp.SelectedIndex == 4)
                {
                    fileNameString = "Audio";
                }
                DialogResult = true;
                this.Close();
            }
        }

        private void TextBoxLibName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxLibName.Text))
            {
                TextBoxLibName.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
            }
        }
    }
}
