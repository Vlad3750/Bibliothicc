using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaktionslogik für AddChangeWindow.xaml
    /// </summary>
    public partial class AddChangeFileWindow : Window
    {
        bool isPressedAdd = true;
        public AddChangeFileWindow(string LabelAddChangeText, string LabelDataTypeText, string ButtonContentAddChange, bool AddOrChange)
        {
            InitializeComponent();

            LabelAddChange.Content = LabelAddChangeText + LabelDataTypeText;
            ButtonAddChange.Content = ButtonContentAddChange;
            isPressedAdd = AddOrChange;

            ListViewCategoriesToAdd.Items.Add(new CategoryItem { Name = "TestCategory1" });
        }

        private void ButtonAddChange_Click(object sender, RoutedEventArgs e)
        {
            if(TextBoxPath.Text == string.Empty)
            {
                MessageBox.Show("File is missing a path");
            }

            else if (isPressedAdd)
            {
                MessageBox.Show("New File added");
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("File changed");
                DialogResult = true;
            }
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoriesWindow window = new CategoriesWindow(ListViewCategoriesToAdd);

            if (window.ShowDialog() == true)
            {
                ListViewCategoriesToAdd.Items.Refresh();
            }
        }

        private void ButtonRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (CategoryItem)button.DataContext;
            ListViewCategoriesToAdd.Items.Remove(item);
        }

        private void ButtonFileOpenerPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                TextBoxPath.Text = openFileDialog.FileName;
                if(TextBoxFileName.Text == string.Empty)
                {
                    string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    TextBoxFileName.Text = fileName.Substring(0, fileName.Length - 4);
                }
            }

        }

        private void ButtonFileOpenerThumbnail_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.jpeg)|*.jpeg | (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                TextBoxThumbnail.Text = openFileDialog.FileName;
            }
        }
    }
}
