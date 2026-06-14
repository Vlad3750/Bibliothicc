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
    /// Interaktionslogik für SettingsWindows.xaml
    /// </summary>
    public partial class SettingsWindows : Window
    {
        public Image buttonOn = new Image();
        public Image buttonOff = new Image();
        public bool isDark;

        public SettingsWindows(bool isDarlModeOn)
        {
            InitializeComponent();

            isDark = isDarlModeOn;

            buttonOn.Source = new BitmapImage(new Uri("pack://application:,,,/Bilder/button_an.png"));
            buttonOff.Source = new BitmapImage(new Uri("pack://application:,,,/Bilder/button_aus.png"));

            if (isDarlModeOn)
            {
                ButtonDarkmode.Content = buttonOff;
            }
            else
            {
                ButtonDarkmode.Content = buttonOn;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isDark = !isDark;
            ApplyTheme(isDark);

            var img = (Image)((Button)sender).Content;
            img.Source = new BitmapImage(new Uri(isDark ? "/Bilder/button_aus.png" : "/Bilder/button_an.png", UriKind.Relative));
        }

        public static void ApplyTheme(bool dark)
        {
            var res = Application.Current.Resources;

            if (dark)
            {
                SetBrush(res, "BgDeepBrush", "#0F1117");
                SetBrush(res, "BgSurfaceBrush", "#1A1D27");
                SetBrush(res, "BgCardBrush", "#22263A");
                SetBrush(res, "BgElevatedBrush", "#2A2F45");
                SetBrush(res, "AccentBrush", "#E8923A");
                SetBrush(res, "AccentHoverBrush", "#F0A855");
                SetBrush(res, "TextPrimaryBrush", "#F0F2F8");
                SetBrush(res, "TextSecondaryBrush", "#8B90A8");
                SetBrush(res, "TextMutedBrush", "#555A72");
                SetBrush(res, "BorderBrush2", "#2E3349");
                SetBrush(res, "DangerBrush", "#E05252");
            }
            else
            {
                SetBrush(res, "BgDeepBrush", "#F0F2F8");
                SetBrush(res, "BgSurfaceBrush", "#FFFFFF");
                SetBrush(res, "BgCardBrush", "#F5F6FA");
                SetBrush(res, "BgElevatedBrush", "#EAECF4");
                SetBrush(res, "AccentBrush", "#D97B2A");
                SetBrush(res, "AccentHoverBrush", "#E8923A");
                SetBrush(res, "TextPrimaryBrush", "#1A1D27");
                SetBrush(res, "TextSecondaryBrush", "#4A4F6A");
                SetBrush(res, "TextMutedBrush", "#9095B0");
                SetBrush(res, "BorderBrush2", "#D5D8E8");
                SetBrush(res, "DangerBrush", "#C94040");
            }
        }

        private static void SetBrush(ResourceDictionary res, string key, string hex)
        {
            var brush = ((SolidColorBrush)res[key]).Clone();
            brush.Color = (Color)ColorConverter.ConvertFromString(hex);
            res[key] = brush;
        }
    }
}
