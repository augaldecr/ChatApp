using ChatApp.Core.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics;
using System.Net.Http.Json;

namespace ChatApp.Core.Services
{
    public class ChatService : IChatService
    {
        public bool IsConnected { get; set; }

        public string ConnectionToken { get; set; }

        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private HttpClient httpClient;

        HubConnection hub;

        public async Task ConnectAsync(string userId)
        {
            await semaphoreSlim.WaitAsync();

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            var result = await httpClient.GetFromJsonAsync<ConnectionInfo>($"{Config.NegotiateEndpoint}/{userId}"); 

            var connectionBuilder = new HubConnectionBuilder();
            connectionBuilder.WithUrl(result.Url, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(result.AccessToken);
            });

            hub = connectionBuilder.Build();
            
            await hub.StartAsync();

            ConnectionToken = hub.ConnectionId;

            IsConnected = true;

            semaphoreSlim.Release();
        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected)
            {
                return;
            }

            try
            {
                  await hub.DisposeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);    
            }

            IsConnected = false;
        }
    }
}
