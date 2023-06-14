using ChatApp.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace ChatApp.Functions
{
    public static class MessagesFunction
    {
        [FunctionName("Messages")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalR(HubName = "chat")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var message = JsonConvert.DeserializeObject<SimpleTextMessage>(requestBody);
            await signalRMessages.AddAsync(
                               new SignalRMessage
                               {
                                   Target = "ReceivedMessage",
                                   Arguments = new[] { message }
                               });
        }
    }
}
