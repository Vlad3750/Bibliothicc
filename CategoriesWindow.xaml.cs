// AI (Claude)
// Start

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
using Bibliothicc_ClassLibrary;

namespace Bibliothicc
{
    public class CategoryItem
    {
        public string Name { get; set; }
        public string Symbol { get; set; } = "○";
    }

    public partial class CategoriesWindow : Window
    {
        ListView LVCategoriesToAdd;
        List<Category> SystemCategories;

        public CategoriesWindow(ListView listViewCategoriesToAdd, List<Category> systemCategories)
        {
            InitializeComponent();
            LVCategoriesToAdd = listViewCategoriesToAdd;
            SystemCategories = systemCategories;

            // SystemCategories des Users laden
            foreach (Category category in SystemCategories)
            {
                // Prüfen ob diese Kategorie schon als Tag hinzugefügt wurde
                bool alreadyAdded = false;
                foreach(CategoryItem item in LVCategoriesToAdd.Items)
                {
                    if(item.Name == category.Name)
                    {
                        alreadyAdded = true;
                        break;
                    }
                }
                ListViewSystemCategories.Items.Add(new CategoryItem
                {
                    Name = category.Name,
                    Symbol = alreadyAdded ? "✓" : "○"
                });
            }
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddChangeCategoryWindow window = new AddChangeCategoryWindow(
                "Add", "Add", true, ListViewSystemCategories);

            if (window.ShowDialog() == true)
            {
                var newCat = new CategoryItem { Name = window.categoryName, Symbol = "○" };
                ListViewSystemCategories.Items.Add(newCat);
                // Auch ins System speichern
                SystemCategories.Add(new Category { Name = window.categoryName });
            }
        }

        private void ButtonChangeCategory_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSystemCategories.SelectedItem == null)
            {
                CustomMessageBox.Show("Please select a category to rename", this, "⚠️");
                return;
            }

            AddChangeCategoryWindow window = new AddChangeCategoryWindow(
                "Change", "Change", false, ListViewSystemCategories);

            if (window.ShowDialog() == true)
            {
                var selected = (CategoryItem)ListViewSystemCategories.SelectedItem;
                // Im System umbenennen
                var cat = SystemCategories.Find(c => c.Name == selected.Name);
                if (cat != null) cat.Name = window.categoryName;
                selected.Name = window.categoryName;
                ListViewSystemCategories.Items.Refresh();
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ListViewItem)sender;
            var data = (CategoryItem)item.DataContext;

            if (data.Symbol == "○")
            {
                data.Symbol = "✓";
                LVCategoriesToAdd.Items.Add(new CategoryItem { Name = data.Name });
                ListViewSystemCategories.Items.Refresh();
                DialogResult = true;
                this.Close();
            }
            else
            {
                CustomMessageBox.Show("Category already added to file", this);
            }
        }
    }
}

// End