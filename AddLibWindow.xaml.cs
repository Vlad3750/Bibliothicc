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
using static System.Net.Mime.MediaTypeNames;

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
            if (TextBoxLibName.Text == string.Empty)
            {
                CustomMessageBox.Show("Please choose a name for you're Library", this, "⚠️");
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
                DialogResult = true;
                this.Close();
            }
        }
    }
}
