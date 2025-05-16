using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WindowsAPICodePack.Dialogs;
using WpfApp6.Services;
using WpfApp6.Services.Launch;

namespace Volcano_Launcher.Pages
{
    public partial class Library : Page
    {
        public Library()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard slideInStoryboard = (Storyboard)this.Resources["SlideInStoryboard"];
            slideInStoryboard.Begin();
        }

        private void Button_SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = "REFLEX Fortnite Build",
                Multiselect = false
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var selectedFolderPath = dialog.FileName;

                string fortniteDir = Path.Combine(selectedFolderPath, "FortniteGame");
                string engineDir = Path.Combine(selectedFolderPath, "Engine");
                string exePath = Path.Combine(fortniteDir, "Binaries", "Win64", "FortniteClient-Win64-Shipping.exe");

                if (Directory.Exists(fortniteDir) && Directory.Exists(engineDir) && File.Exists(exePath))
                {
                    PathBox.Text = selectedFolderPath;

                    UpdateINI.WriteToConfig("Auth", "FortnitePath", selectedFolderPath);
                    string savedPath = UpdateINI.ReadValue("Auth", "FortnitePath");
                    if (string.IsNullOrEmpty(savedPath))
                    {
                        MessageBox.Show("Failed to save Fortnite path.");
                    }

                    string splashPath = Path.Combine(fortniteDir, "Content", "Splash");

                    if (Directory.Exists(splashPath))
                    {
                        string[] splashImages = Directory.GetFiles(splashPath, "*.bmp");

                        if (splashImages.Length > 0)
                        {
                            try
                            {
                                var image = new BitmapImage(new Uri(splashImages[0]));
                                SplashPreview.Source = image;
                                SplashContainer.Visibility = Visibility.Visible;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to load image: {ex.Message}");
                            }
                        }
                        else
                        {
                            SplashContainer.Visibility = Visibility.Collapsed;
                            MessageBox.Show("No .bmp splash images found in the Splash folder.");
                        }
                    }
                    else
                    {
                        SplashContainer.Visibility = Visibility.Collapsed;
                        MessageBox.Show("Splash folder not found.");
                    }
                }
                else
                {
                    MessageBox.Show("❌ Please make sure the folder contains both 'FortniteGame' and 'Engine' directories and the Fortnite executable.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private async void SplashContainer_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string path = UpdateINI.ReadValue("Auth", "FortnitePath");
                if (string.IsNullOrEmpty(path) || path == "NONE")
                {
                    path = UpdateINI.ReadValue("Auth", "FortnitePath");
                    if (string.IsNullOrEmpty(path) || path == "NONE")
                    {
                        MessageBox.Show("Invalid Fortnite Path was given.");
                        return;
                    }
                }

                string exePath = Path.Combine(path, "FortniteGame", "Binaries", "Win64", "FortniteClient-Win64-Shipping.exe");

                if (File.Exists(exePath))
                {
                    string email = UpdateINI.ReadValue("Auth", "Email");
                    string password = UpdateINI.ReadValue("Auth", "Password");

                    if (email == "NONE" || password == "NONE")
                    {
                        MessageBox.Show("Invalid Email or Password!");
                        return;
                    }

                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(
                            "https://www.dropbox.com/scl/fi/u8416cqyevslfrkjzpd91/Starfall.dll?rlkey=s5jd03593e0xhgbx0ckvlhwnx&st=p8z184wv&dl=1",
                            Path.Combine(path, "Engine", "Binaries", "ThirdParty", "NVIDIA", "NVaftermath", "Win64", "GFSDK_Aftermath_Lib.x64.dll"));

                        string pakPath = Path.Combine(path, "FortniteGame", "Content", "Paks");
                        Directory.CreateDirectory(pakPath);

                        client.DownloadFile(
                            "https://www.dropbox.com/scl/fi/6kz8ubj4ac7vei7u2d7ha/pakchunkReflex-WindowsClient.pak?rlkey=ag8gz5mmb59niitu34intn8b8&st=tupgbvfd&dl=1",
                            Path.Combine(pakPath, "pakchunkReflex-WindowsClient.pak"));

                        client.DownloadFile(
                            "https://www.dropbox.com/scl/fi/gm8jf2baln2h91mumsxpu/pakchunkReflex-WindowsClient.sig?rlkey=ob2krncezdb4mqiaixkcjptzc&st=ajf4c9nr&dl=1",
                            Path.Combine(pakPath, "pakchunkReflex-WindowsClient.sig"));
                    }

                    string args = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck";

                    PSBasics.Start(path, args, email, password);
                    FakeAC.Start(path, "FortniteClient-Win64-Shipping_BE.exe", args, "r");
                    FakeAC.Start(path, "FortniteLauncher.exe", args, "dsf");

                    await Task.Run(() =>
                    {
                        PSBasics._FortniteProcess.WaitForExit();
                    });

                    try
                    {
                        FakeAC._FNLauncherProcess?.Close();
                        FakeAC._FNAntiCheatProcess?.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Could not close Fortnite properly, please close manually!");
                    }
                }
                else
                {
                    MessageBox.Show("Fortnite executable not found at the provided path.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while trying to launch:\n" + ex.Message);
            }
        }
    }
}
