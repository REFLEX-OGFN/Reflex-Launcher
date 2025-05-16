using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Newtonsoft.Json.Linq;
using Volcano_Launcher.Services;
using WpfApp6.Services;

namespace Volcano_Launcher.Pages
{
    public partial class Home : Page
    {
        private readonly HttpClient _httpClient;

        public Home()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadUsername();
        }

        private async void LoadUsername()
        {
            try
            {
                string email = UpdateINI.ReadValue("Auth", "Email");

                if (email != "NONE")
                {
                    string username = await GetUsernameByEmail(email);

                    if (username != null)
                    {
                        UsernameTextBlock.Text = username;
                    }
                    else
                    {
                        UsernameTextBlock.Text = email.Split('@')[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot fetch your username. it seems like the backend is offline! " + ex.Message);
            }
        }

        private async Task<string> GetUsernameByEmail(string email)
        {
            try
            {
                string url = $"http://176.96.137.90:3551/username?email={email}";
                var response = await _httpClient.GetStringAsync(url);
                var jsonResponse = JObject.Parse(response);
                if (jsonResponse.ContainsKey("username"))
                {
                    return jsonResponse["username"].ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot fetch your username. it seems like the backend is offline! " + ex.Message);
                return null;
            }
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

            SlideTransform.BeginAnimation(TranslateTransform.XProperty, slide);
            MainGrid.BeginAnimation(OpacityProperty, fade);
        }
    }
}
