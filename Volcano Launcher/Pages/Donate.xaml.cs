using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Volcano_Launcher.Pages
{
    public partial class Donate : Page
    {
        public Donate()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            var slide = new DoubleAnimation
            {
                From = 100,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var fade = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            DonateGrid.BeginAnimation(OpacityProperty, fade);
            SlideTransform.BeginAnimation(TranslateTransform.XProperty, slide);
        }


        private void OpenUrl(string url, string videoName)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Öffnen des {videoName}-Videos: " + ex.Message);
            }
        }

        private void Benjio_Click(object sender, RoutedEventArgs e)
        {
            const string benjioUrl = "https://youtu.be/rxyJ3JpJTZk";
            OpenUrl(benjioUrl, "Benjio");
        }

        private void Diamond_Click(object sender, RoutedEventArgs e)
        {
            const string diamondUrl = "https://youtu.be/xve-4UPDrvE";
            OpenUrl(diamondUrl, "Diamond");
        }

        private void PortalDonator_Click(object sender, RoutedEventArgs e)
        {
            const string portalUrl = "https://youtu.be/uaNAvRNSaAs";
            OpenUrl(portalUrl, "Portal");
        }

        private void ReflexDonator_Click(object sender, RoutedEventArgs e)
        {
            const string reflexUrl = "https://youtu.be/F0X0rgN9N0U";
            OpenUrl(reflexUrl, "Reflex Donator");
        }
    }
}
