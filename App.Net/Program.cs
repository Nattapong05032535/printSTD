using App.Net.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.Net
{
    internal static class Program
    {
        private static IHost? _webHost;
        private static Server_API_Print? _mainForm;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // ˹ Application Configuration
            ApplicationConfiguration.Initialize();

            // ʴ Splash Screen
            using (var splash = new SplashScreen())
            {
                splash.ShowDialog();
            }

            // Դѡ
            _mainForm = new Server_API_Print();
            
            // Start the web host in background AFTER main form exists (for DI)
            _ = Task.Run(StartWebHost);
            Application.Run(_mainForm);
        }

        private static async Task StartWebHost()
        {
            try
            {
                var builder = WebApplication.CreateBuilder();
                
                // Configure URL binding via UseUrls (simpler and avoids ConfigureKestrel extension dependency)
                builder.WebHost.UseUrls("http://" + (string.IsNullOrWhiteSpace(Setting.hostURL) ? "+" : Setting.hostURL.Trim()) + ":" + Setting.portURL);

                // Add services
                builder.Services.AddControllers();
                builder.Services.AddSingleton<Server_API_Print>(_ => _mainForm!);

                var app = builder.Build();

                // Configure the HTTP request pipeline
                app.UseRouting();
                app.MapControllers();

                _webHost = app;
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start web server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void StopWebHost()
        {
            _webHost?.StopAsync();
        }
    }
}