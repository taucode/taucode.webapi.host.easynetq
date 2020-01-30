namespace Domain.Msgs
{
    public class Msg
    {
        public MsgId Id { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Content { get; private set; }
    }
}
