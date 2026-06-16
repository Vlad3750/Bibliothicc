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
    /// Interaktionslogik für AddChangeCategoryWindow.xaml
    /// </summary>
    public partial class AddChangeCategoryWindow : Window
    {
        bool isPressedAdd = true;
        public string categoryName = "";

        public AddChangeCategoryWindow(string LabelCategory, string ButtonContentCategory, bool AddOrChange, ListView categoryListView)
        {
            InitializeComponent();

            LabelAddChangeCategory.Text = LabelCategory + " Category:";
            ButtonAddChangeCategory.Content = ButtonContentCategory;
            isPressedAdd = AddOrChange;
            if (!isPressedAdd)
            {
                var item = (CategoryItem)categoryListView.SelectedItem;
                TextBoxAddChangeCategory.Text = item.Name;
            }
        }

        private void ButtonAddChangeCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxAddChangeCategory.Text))
            {
                TextBoxAddChangeCategory.BorderBrush = (Brush)Application.Current.Resources["DangerBrush"];
                return;
            }

            categoryName = TextBoxAddChangeCategory.Text;

            if (isPressedAdd)
            {
                categoryName = TextBoxAddChangeCategory.Text;
                CustomMessageBox.Show("New Category Added", this);
            }
            else
            {
                CustomMessageBox.Show("Category Changed", this);
            }
            DialogResult = true;
        }

        private void TextBoxAddChangeCategory_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxAddChangeCategory.Text))
            {
                TextBoxAddChangeCategory.BorderBrush = (Brush)Application.Current.Resources["BorderBrush2"];
            }
        }
    }
}
