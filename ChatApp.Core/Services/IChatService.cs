namespace ChatApp.Core.Services
{
    public interface IChatService
    {
        bool IsConnected { get; }
        string ConnectionToken { get; set; }
        Task ConnectAsync(string userId);
        Task DisconnectAsync();
    }
}
