using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using IOPath = System.IO.Path;

namespace Volcano_Launcher.Windows
{
    public partial class BackendFetching : Window
    {
        private readonly HttpClient _httpClient;
        private const string BackendBaseUrl = "http://176.96.137.90:3551";
        private readonly string CurrentVersion;

        public BackendFetching()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            CurrentVersion = "1.0.4";

            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await CheckBackendStatus();
        }

        private async Task CheckBackendStatus()
        {
            try
            {
                StatusText.Text = "Checking if the Backend is online...";

                var statusResponse = await _httpClient.GetAsync($"{BackendBaseUrl}/api/v1/reflex/launcher/status");
                if (statusResponse.IsSuccessStatusCode)
                {
                    var statusContent = await statusResponse.Content.ReadAsStringAsync();
                    var statusData = JsonSerializer.Deserialize<StatusResponse>(statusContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    StatusText.Text = statusData?.Status ?? "Backend is online";
                    await CheckForUpdates();
                }
                else
                {
                    StatusText.Text = "The backend is offline";
                    await DelayAndShutdown();
                }
            }
            catch
            {
                StatusText.Text = "The backend is offline";
                await DelayAndShutdown();
            }
        }

        private async Task CheckForUpdates()
        {
            try
            {
                StatusText.Text = "Checking for updates...";

                var versionResponse = await _httpClient.GetAsync($"{BackendBaseUrl}/api/v1/reflex/launcher/version");
                if (versionResponse.IsSuccessStatusCode)
                {
                    var versionContent = await versionResponse.Content.ReadAsStringAsync();
                    var versionData = JsonSerializer.Deserialize<VersionResponse>(versionContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (versionData?.Version != CurrentVersion)
                    {
                        await DownloadAndInstallUpdate();
                    }
                    else
                    {
                        NavigateToLogin();
                    }
                }
                else
                {
                    NavigateToLogin();
                }
            }
            catch
            {
                NavigateToLogin();
            }
        }

        private async Task DownloadAndInstallUpdate()
        {
            try
            {
                StatusText.Text = "Downloading update...";

                var updateResponse = await _httpClient.GetAsync($"{BackendBaseUrl}/api/v1/reflex/launcher/update");
                if (!updateResponse.IsSuccessStatusCode)
                {
                    NavigateToLogin();
                    return;
                }

                var updateContent = await updateResponse.Content.ReadAsStringAsync();
                var updateData = JsonSerializer.Deserialize<UpdateResponse>(updateContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (string.IsNullOrWhiteSpace(updateData?.Url))
                {
                    NavigateToLogin();
                    return;
                }

                var installerPath = IOPath.Combine(IOPath.GetTempPath(), "REFLEX Installer.msi");
                var installerBytes = await _httpClient.GetByteArrayAsync(updateData.Url);
                await File.WriteAllBytesAsync(installerPath, installerBytes);

                Process.Start(new ProcessStartInfo
                {
                    FileName = installerPath,
                    UseShellExecute = true
                });

                Application.Current.Shutdown();
            }
            catch
            {
                NavigateToLogin();
            }
        }

        private void NavigateToLogin()
        {
            Dispatcher.Invoke(() =>
            {
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            });
        }

        private async Task DelayAndShutdown()
        {
            await Task.Delay(5000);
            Application.Current.Shutdown();
        }
    }

    public class StatusResponse
    {
        public string Status { get; set; }
    }

    public class VersionResponse
    {
        public string Version { get; set; }
    }

    public class UpdateResponse
    {
        public string Url { get; set; }
    }
}
