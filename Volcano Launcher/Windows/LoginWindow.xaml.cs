using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp6.Services;
using System.Net.Http;
using Newtonsoft.Json;

namespace Volcano_Launcher.Windows
{

    public partial class LoginWindow : Window
    {
        private readonly HttpClient _httpClient;

        public LoginWindow()
        {
            InitializeComponent();
            RefreshCredentialsFromConfig();
            _httpClient = new HttpClient();
        }

        private void RefreshCredentialsFromConfig()
        {
            string email = UpdateINI.ReadValue("Auth", "Email");
            string password = UpdateINI.ReadValue("Auth", "Password");

            EmailBox.Text = email;
            PasswordBox.Password = password;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailBox.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Please enter a valid email and password.");
                return;
            }

            var payload = new { email = EmailBox.Text, password = PasswordBox.Password };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("http://176.96.137.90:3551/api/v1/reflex/launcher/login", content);
                if (response.IsSuccessStatusCode)
                {
                    UpdateINI.WriteToConfig("Auth", "Email", EmailBox.Text);
                    UpdateINI.WriteToConfig("Auth", "Password", PasswordBox.Password);
                    RefreshCredentialsFromConfig();
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login failed. (" + response.StatusCode + ")");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while calling the API: " + ex.Message);
            }
        }
    }
}
