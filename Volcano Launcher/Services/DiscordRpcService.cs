using DiscordRPC;
using DiscordRPC.Logging;
using System;

namespace Volcano_Launcher.Services
{
    public class DiscordRpcService
    {
        private static DiscordRpcService _instance;
        private DiscordRpcClient _rpcClient;

        private DiscordRpcService()
        {
            _rpcClient = new DiscordRpcClient("1332055279935950858");
            _rpcClient.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            _rpcClient.OnReady += (sender, e) =>
            {
                Console.WriteLine("Connected to Discord as " + e.User.Username);
            };
            _rpcClient.Initialize();
        }

        public static DiscordRpcService Instance => _instance ??= new DiscordRpcService();

        public void SetPresence(string state, string details, string largeImageKey, string largeImageText)
        {
            _rpcClient.SetPresence(new RichPresence()
            {
                State = state,
                Details = details,
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = largeImageKey,
                    LargeImageText = largeImageText
                }
            });
        }

        public void Shutdown()
        {
            _rpcClient.Dispose();
        }
    }
}
