using System.Windows;

namespace Bibliothicc
{
    public partial class CustomMessageBox : Window
    {
        // Nutzung: CustomMessageBox.Show("Text", this);
        // Optional: CustomMessageBox.Show("Text", this, "⚠️");
        public static void Show(string message, Window owner = null, string icon = "ℹ️")
        {
            var box = new CustomMessageBox();
            box.TextBlockMessage.Text = message;
            box.TextBlockIcon.Text = icon;
            if (owner != null) box.Owner = owner;
            box.ShowDialog();
        }

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}