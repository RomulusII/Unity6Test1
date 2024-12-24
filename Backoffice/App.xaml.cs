using GameCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Windows;

namespace Backoffice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _webHost;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Start the embedded web server
            StartWebServer();
        }

        private void StartWebServer()
        {
            // Create and configure the web host
            _webHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register the SocketProcessor as a singleton service
                    services.AddSingleton<SocketProcessor>();

                    // Add logging services
                    services.AddLogging(config =>
                    {
                        config.AddConsole();
                        config.AddDebug();
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("http://localhost:8080") // Web server will run on localhost:8080
                        .Configure(app =>
                        {
                            // Enable WebSocket middleware
                            app.UseWebSockets();

                            // Map WebSocket requests to the "/ws" endpoint
                            app.Map("/ws", wsApp =>
                            {
                                wsApp.Run(HandleWebSocketRequests); // Correct usage
                            });

                            // Default response for other requests
                            app.Run(async context =>
                            {
                                await context.Response.WriteAsync("OyunApi is running...");
                            });
                        });
                })
                .Build();

            // Start the web host
            _webHost.Start();
        }

        private static async Task HandleWebSocketRequests(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                // Accept the WebSocket connection
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("Yeni bir istemci bağlandı!");

                // Get the SocketProcessor service
                var socketProcessor = context.RequestServices.GetRequiredService<SocketProcessor>();

                // Manage the player connection
                await socketProcessor.HandleNewConnection(webSocket);
            }
            else
            {
                // If the request is not a WebSocket request, return a 400 Bad Request response
                context.Response.StatusCode = 400;
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            // Stop and dispose of the web host if it exists
            if (_webHost != null)
            {
                await _webHost.StopAsync();
                _webHost.Dispose();
            }

            base.OnExit(e);
        }
    }
}