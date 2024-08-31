namespace GameOfSkateBotApi.Webhook.Exceptions
{
    public class MessageMissingException : Exception
    {
        public MessageMissingException(string? message) : base(message)
        {
        }
    }
}
