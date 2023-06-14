using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;

namespace ChatApp.Functions
{
    public static class NegotiateFunction
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "negotiate/{userId}")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "chat", UserId = "{userId}")] SignalRConnectionInfo connectionInfo,
            ILogger log)
        {
            return connectionInfo;
        }
    }
}
