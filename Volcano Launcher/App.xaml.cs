using System;
using System.Windows;
using DiscordRPC;

namespace Volcano_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DiscordRpcClient _rpcClient;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            InitializeDiscordRPC();
        }


        private void InitializeDiscordRPC()
        {

            _rpcClient = new DiscordRpcClient("1332055279935950858");


            _rpcClient.Initialize();
            _rpcClient.SetPresence(new RichPresence()
            {
                State = "Play Chapter 2 Season 1 Again!",
                Details = "Playing REFLEX",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = "Reflex",
                    LargeImageText = "REFLEX Launcher"
                },
                Buttons = new[]
                {
                    new DiscordRPC.Button()
                    {
                        Label = "JOIN REFLEX!",
                        Url = "https://discord.gg/sd2HPRPhb3"
                    }
                }
            });


            _rpcClient.OnReady += (sender, e) =>
            {
                Console.WriteLine("Connected to Discord as " + e.User.Username);
            };


            _rpcClient.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Presence updated");
            };


        }


        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);


            _rpcClient.Dispose();
        }
    }
}
