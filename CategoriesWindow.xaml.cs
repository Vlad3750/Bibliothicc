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
    /// Interaktionslogik für CategoriesWindow.xaml
    /// </summary>
    public partial class CategoriesWindow : Window
    {
        ListView LVCategoriesToAdd;
        public CategoriesWindow(ListView ListViewCategoriesToAdd)
        {
            InitializeComponent();
            LVCategoriesToAdd = ListViewCategoriesToAdd;
        }

        private void ButtonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string LabelCategory = "Add";
            string ButtonContentCategory = "Add";

            AddChangeCategoryWindow window = new AddChangeCategoryWindow(LabelCategory, ButtonContentCategory, true);

            if (window.ShowDialog() == true)
            {
                AddNewCategoryToSystem();
            }
        }

        private void ButtonChangeCategory_Click(object sender, RoutedEventArgs e)
        {
            string LabelCategory = "Change";
            string ButtonContentCategory = "Change";

            AddChangeCategoryWindow window = new AddChangeCategoryWindow(LabelCategory, ButtonContentCategory, false);

            if (window.ShowDialog() == true)
            {
                ChangeCategoryName();
            }
        }

        // Double clicking an ListViewItem -> adds it to ListViewCategoriesToAdd

        private void AddNewCategoryToSystem()
        {

        }
        private void ChangeCategoryName()
        {

        }
    }
}
