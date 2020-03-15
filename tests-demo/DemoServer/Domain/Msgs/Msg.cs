using System;

namespace Domain.Msgs
{
    public class Msg
    {
        private Msg()
        {
        }

        public Msg(
            string correlationId,
            DateTime sentAt,
            string sender,
            string recipient,
            string subject,
            string content)
        {
            this.Id = new MsgId();
            this.CorrelationId = correlationId;
            this.SentAt = sentAt;
            this.Sender = sender;
            this.Recipient = recipient;
            this.Subject = subject;
            this.Content = content;
        }

        public MsgId Id { get; private set; }
        public string CorrelationId { get; private set; }
        public DateTime SentAt { get; private set; }
        public string Sender { get; private set; }
        public string Recipient { get; private set; }
        public string Subject { get; private set; }
        public string Content { get; private set; }
    }
}