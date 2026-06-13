using Bibliothicc_ClassLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaktionslogik für AddChangeWindow.xaml
    /// </summary>
    public partial class AddChangeFileWindow : Window
    {
        bool isPressedAdd = true;
        public Media itemToAdd = new Media();
        public Media itemToChange = null;
        string filtersForFiles = "";
        string getFilters;

        public AddChangeFileWindow(string LabelAddChangeText, string LabelDataTypeText, string ButtonContentAddChange, bool AddOrChange, string filters)
        {
            InitializeComponent();

            LabelAddChange.Content = LabelAddChangeText + LabelDataTypeText;
            ButtonAddChange.Content = ButtonContentAddChange;
            isPressedAdd = AddOrChange;
            getFilters = filters;
            if (filters == "Text")
            {
                LabelCategory.Visibility = Visibility.Hidden;
                LabelThumbnail.Visibility = Visibility.Hidden;
                DockPanelCategory.Visibility = Visibility.Hidden;
                DockPanelThumbnail.Visibility = Visibility.Hidden;

                filtersForFiles = "Text file (*.txt)|*.txt|Markdown (*.md)|*.md|Word (*.docx)|*.docx|PDF (*.pdf)|*.pdf";
            }
            else if (filters == "Video")
            {
                filtersForFiles = "Video (*.mp4)|*.mp4";
            }
            else if (filters == "Movie")
            {
                filtersForFiles = "Movie (*.mp4, *.mkv)|*.mp4;*.mkv";
            }
            else if (filters == "Image")
            {
                ButtonFileOpenerThumbnail.Visibility = Visibility.Hidden;
                filtersForFiles = "JPG (*.jpeg, *.jpg)|*.jpeg;*.jpg |PNG (*.png)|*.png";
            }
        }

        public AddChangeFileWindow(string LabelAddChangeText, string LabelDataTypeText, Media itemToChange, string ButtonContentAddChange, bool AddOrChange, string filters): this(LabelAddChangeText, LabelDataTypeText, ButtonContentAddChange, AddOrChange, filters)
        {
            InitializeComponent();

            if (!isPressedAdd)
            {
                this.itemToChange = itemToChange;
            }

            if (itemToChange != null)
            {
                LabelPath.Content = itemToChange.FileUrl;
                LabelThumbnail.Content = itemToChange.CoverUrl;
                TextBoxFileName.Text = itemToChange.Name;
                foreach(Category category in itemToChange.CategoryList)
                {
                    ListViewCategoriesToAdd.Items.Add(new CategoryItem() { Name = category.Name, Symbol = "✓" });
                }

            }
        }

        private void ButtonAddChange_Click(object sender, RoutedEventArgs e)
        {
            if (LabelPath.Content == null)
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
                foreach(ListViewItem lvItem in ListViewCategoriesToAdd.Items)
                {
                    if(itemToChange != null)
                    {
                        itemToAdd.CategoryList.Add(new Category() { Name = lvItem.Content.ToString() });
                    }
                    else
                    {
                        itemToChange.CategoryList.Add(new Category() { Name = lvItem.Content.ToString() });
                    }
                }
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
            openFileDialog.Filter = filtersForFiles;
            if (openFileDialog.ShowDialog() == true)
            {
                LabelPath.Content = openFileDialog.FileName;

                if (getFilters == "Image")
                {
                    LabelShowcaseThumbnail.Content = openFileDialog.FileName;
                }

                if (TextBoxFileName.Text == string.Empty)
                {
                    string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    TextBoxFileName.Text = fileName.Substring(0, fileName.Length - 4);
                    if(itemToChange != null)
                    {
                        itemToAdd.FileUrl = LabelPath.Content.ToString();
                        itemToAdd.Name = fileName;
                        itemToAdd.Title = TextBoxFileName.Text;
                    }

                }
            }
        }

        private void ButtonFileOpenerThumbnail_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.jpeg, *.jpg)|*.jpeg;*.jpg | (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                LabelThumbnail.Content = openFileDialog.FileName;
                itemToAdd.CoverUrl = LabelThumbnail.Content.ToString();

            }
        }
    }
}
