using ChatApp.Core.Services;

public class Program
{
    static ChatService service;
    static string userName;

    static async Task Main(string[] args)
    {
       Console.WriteLine("User name: ");
        userName = Console.ReadLine();

        service = new ChatService();
        
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
        } while (keepGoing);
    }
}