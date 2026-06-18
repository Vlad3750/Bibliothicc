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

        public static bool ShowConfirm(string message, Window owner = null, string icon = "⚠️")
        {
            var box = new CustomMessageBox();
            box.TextBlockMessage.Text = message;
            box.TextBlockIcon.Text = icon;
            box.ButtonCancel.Visibility = Visibility.Visible;
            box.ButtonOK.SetResourceReference(StyleProperty, "DangerButton");
            if (owner != null) box.Owner = owner;
            box.ShowDialog();
            return box.DialogResult == true;
        }

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}