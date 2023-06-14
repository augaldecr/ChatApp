namespace ChatApp.Messages
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public Type TypeInfo { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Sender { get; set; }

        public ChatMessage() { }

        public ChatMessage(string sender)
        {
            Id = Guid.NewGuid().ToString();
            TypeInfo = GetType();
            TimeStamp = DateTime.Now;
            Sender = sender;
        }
    }
}
