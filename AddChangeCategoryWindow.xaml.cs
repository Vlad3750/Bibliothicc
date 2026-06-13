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
                var item = (ListViewItem)categoryListView.SelectedItem;
                categoryName = item.Content.ToString();
                TextBoxAddChangeCategory.Text = categoryName;
            }
        }

        private void ButtonAddChangeCategory_Click(object sender, RoutedEventArgs e)
        {
            if (isPressedAdd)
            {
                categoryName = TextBoxAddChangeCategory.Text;
                MessageBox.Show("New Category Added");
            }
            else
            {
                MessageBox.Show("Category Changed");
            }
            DialogResult = true;
        }
    }
}
