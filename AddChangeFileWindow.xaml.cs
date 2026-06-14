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

            LabelAddChange.Text = LabelAddChangeText + LabelDataTypeText;
            ButtonAddChange.Content = ButtonContentAddChange;
            isPressedAdd = AddOrChange;
            getFilters = filters;
            if (filters == "Text")
            {
                LabelCategory.Visibility = Visibility.Collapsed;
                LabelThumbnail.Visibility = Visibility.Collapsed;
                DockPanelCategory.Visibility = Visibility.Collapsed;
                DockPanelThumbnail.Visibility = Visibility.Collapsed;

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
                LabelCategory.Visibility = Visibility.Collapsed;
                DockPanelCategory.Visibility = Visibility.Collapsed;

                ButtonFileOpenerThumbnail.Visibility = Visibility.Hidden;
                filtersForFiles = "JPG (*.jpeg, *.jpg)|*.jpeg;*.jpg |PNG (*.png)|*.png";
            }
        }

        public AddChangeFileWindow(string LabelAddChangeText, string LabelDataTypeText, Media itemToChange, string ButtonContentAddChange, bool AddOrChange, string filters): this(LabelAddChangeText, LabelDataTypeText, ButtonContentAddChange, AddOrChange, filters)
        {
            if (!isPressedAdd)
            {
                this.itemToChange = itemToChange;
                LabelPath.Content = this.itemToChange.FileUrl;
                LabelThumbnail.Text = this.itemToChange.CoverUrl;
                TextBoxFileName.Text = this.itemToChange.Title;
                if (getFilters != "Text" && getFilters != "Image")
                {
                    foreach (Category category in this.itemToChange.CategoryList)
                    {
                        ListViewCategoriesToAdd.Items.Add(new CategoryItem() { Name = category.Name, Symbol = "✓" });
                    }
                }
            }
        }

        private void ButtonAddChange_Click(object sender, RoutedEventArgs e)
        {
            if (LabelPath.Content == null)
            {
                CustomMessageBox.Show("File is missing a path", this, "⚠️");
            }

            else if (isPressedAdd)
            {
                itemToAdd.Title = TextBoxFileName.Text;
                CustomMessageBox.Show("New File added", this);
                DialogResult = true;
            }
            else
            {
                itemToChange.Title = TextBoxFileName.Text;
                CustomMessageBox.Show("File changed", this);
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
                        itemToChange.CategoryList.Add(new Category() { Name = lvItem.Content.ToString() });
                    }
                    else
                    {
                        itemToAdd.CategoryList.Add(new Category() { Name = lvItem.Content.ToString() });
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
                    TextBoxFileName.Text = System.IO.Path.GetFileNameWithoutExtension(fileName);
                    if (itemToChange != null)
                    {
                        itemToChange.FileUrl = openFileDialog.FileName;
                        itemToChange.Name = fileName;
                        itemToChange.MimeType = System.IO.Path.GetExtension(fileName);
                    }
                    else
                    {
                        itemToAdd.FileUrl = openFileDialog.FileName;
                        itemToAdd.Name = fileName;
                        itemToAdd.MimeType = System.IO.Path.GetExtension(fileName);
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
                LabelThumbnail.Text = openFileDialog.FileName;
                itemToAdd.CoverUrl = LabelThumbnail.Text;
            }
        }
    }
}
