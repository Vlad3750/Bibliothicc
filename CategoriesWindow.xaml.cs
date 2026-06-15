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

            var selected = (CategoryItem)ListViewSystemCategories.SelectedItem;
            string oldName = selected.Name;

            AddChangeCategoryWindow window = new AddChangeCategoryWindow(
                "Change", "Change", false, ListViewSystemCategories);

            if (window.ShowDialog() == true)
            {
                // Im System umbenennen
                var cat = SystemCategories.Find(c => c.Name == oldName);
                if (cat != null) cat.Name = window.categoryName;

                // Tag aus LVCategoriesToAdd entfernen falls er den alten Namen hat
                CategoryItem toRemove = null;
                foreach (CategoryItem ci in LVCategoriesToAdd.Items)
                {
                    if (ci.Name == oldName) { toRemove = ci; break; }
                }
                if (toRemove != null) LVCategoriesToAdd.Items.Remove(toRemove);

                // ListView neu rendern
                selected.Name = window.categoryName;
                selected.Symbol = "○";
                int idx = ListViewSystemCategories.SelectedIndex;
                ListViewSystemCategories.Items.Remove(selected);
                ListViewSystemCategories.Items.Insert(idx, selected);
                ListViewSystemCategories.SelectedIndex = idx;
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