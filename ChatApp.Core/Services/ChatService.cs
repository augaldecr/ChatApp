using ChatApp.Core.EventHandlers;
using ChatApp.Core.Models;
using ChatApp.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;

namespace ChatApp.Core.Services
{
    public class ChatService : IChatService
    {
        public bool IsConnected { get; set; }

        public string ConnectionToken { get; set; }

        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private HttpClient httpClient;

        HubConnection hub;

        public event EventHandler<MessageEventArgs> OnMessageReceived;

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

            hub.On<object>("ReceivedMessage", (message) =>
            {
                var json = message.ToString();
                var obj = JsonConvert.DeserializeObject<ChatMessage>(json);
                var msg = (ChatMessage)JsonConvert.DeserializeObject(json, obj.TypeInfo);
                OnMessageReceived?.Invoke(this, new MessageEventArgs(msg));
            });

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

        public async Task SendMessageAsync(ChatMessage message)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Not connected");
            }

            var json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await httpClient.PostAsync(Config.MessagesEndpoint, content);
        }
    }
}
