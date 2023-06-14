using ChatApp.Messages;

namespace ChatApp.Core.EventHandlers
{
    public class MessageEventArgs
    {
        public ChatMessage Message { get; set; }

        public MessageEventArgs(ChatMessage message)
        {
            Message = message;
        }
    }
}
