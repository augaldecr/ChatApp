namespace ChatApp.Core
{
    public static class Config
    {
        public static string MainEndpoint = "http://localhost:7155";
        public static string NegotiateEndpoint = $"{MainEndpoint}/api/negotiate";
        public static string MessagesEndpoint = $"{MainEndpoint}/api/messages";
    }
}
