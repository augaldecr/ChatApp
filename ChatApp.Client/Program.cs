using ChatApp.Core.Services;
using ChatApp.Messages;

public class Program
{
    static ChatService service;
    static string userName;

    static async Task Main(string[] args)
    {
       Console.WriteLine("User name: ");
        userName = Console.ReadLine();

        service = new ChatService();
        service.OnMessageReceived += Service_OnMessageReceived; 
        
        await service.ConnectAsync(userName);

        await Console.Out.WriteLineAsync("Connected");

        var keepGoing = true;

        do
        {
            var message = Console.ReadLine();

            if (message == "exit")
            {
                await service.DisconnectAsync();
                keepGoing = false;
            }
            else
            {
                var simpleTextMessage = new SimpleTextMessage(userName)
                {
                    Text = message,
                };

                await service.SendMessageAsync(simpleTextMessage);
            }
        } while (keepGoing);
    }

    private static void Service_OnMessageReceived(object? sender, ChatApp.Core.EventHandlers.MessageEventArgs e)
    {
        if (e.Message.Sender == userName)
        {
            return;
        }

        if (e.Message.TypeInfo.Name == nameof(SimpleTextMessage))
        {
            var simpleText = e.Message as SimpleTextMessage;
            var message = $"{simpleText.Sender}: {simpleText.Text}";
            Console.WriteLine(message);
        }
    }
}