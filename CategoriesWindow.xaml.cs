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
    public class CategoryItem
    {
        public string Name { get; set; }
        public string Symbol { get; set; } = "○";
    }

    /// <summary>
    /// Interaktionslogik für CategoriesWindow.xaml
    /// </summary>
    public partial class CategoriesWindow : Window
    {
        ListView LVCategoriesToAdd;
        public CategoriesWindow(ListView ListViewCategoriesToAdd)
        {
            InitializeComponent();
            LVCategoriesToAdd = ListViewCategoriesToAdd;
            //ListViewSystemCategories.Items.Add(new CategoryItem { Name = "TestCategory1", Symbol = "✓" });
            //ListViewSystemCategories.Items.Add(new CategoryItem { Name = "TestCategory2" });
            //ListViewSystemCategories.Items.Add(new CategoryItem { Name = "TestCategory3" });
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string LabelCategory = "Add";
            string ButtonContentCategory = "Add";

            AddChangeCategoryWindow window = new AddChangeCategoryWindow(LabelCategory, ButtonContentCategory, true, ListViewSystemCategories);

            if (window.ShowDialog() == true)
            {
                ListViewSystemCategories.Items.Add(new CategoryItem { Name = window.categoryName });
            }
        }

        private void ButtonChangeCategory_Click(object sender, RoutedEventArgs e)
        {
            if(ListViewSystemCategories.SelectedItem != null)
            {
                string LabelCategory = "Change";
                string ButtonContentCategory = "Change";

                AddChangeCategoryWindow window = new AddChangeCategoryWindow(LabelCategory, ButtonContentCategory, false, ListViewSystemCategories);

                if (window.ShowDialog() == true)
                {
                    var item = (ListViewItem)ListViewSystemCategories.SelectedItem;
                    item.Content = window.categoryName;
                    ListViewSystemCategories.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a category to change its name");
            }
        }

        // Double clicking an ListViewItem -> adds it to ListViewCategoriesToAdd

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ListViewItem)sender;
            var data = (CategoryItem)item.DataContext;

            if (data.Symbol == "○")
            {
                data.Symbol = "✓";
                LVCategoriesToAdd.Items.Add(new CategoryItem { Name = data.Name });
            }
            else
            {
                MessageBox.Show("Category already added to file");
            }

            ListViewSystemCategories.Items.Refresh();
            DialogResult = true;
            this.Close();
        }
    }
}
