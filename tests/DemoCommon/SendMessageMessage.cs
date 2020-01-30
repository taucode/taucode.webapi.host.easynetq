using System;
using TauCode.Mq.Abstractions;

namespace DemoCommon
{
    public class SendMessageMessage : IMessage
    {
        public SendMessageMessage(
            string correlationId,
            DateTime createdAt,
            string sender,
            string recipient,
            string subject,
            string content)
        {
            this.CorrelationId = correlationId;
            this.CreatedAt = createdAt;

            this.Sender = sender;
            this.Recipient = recipient;
            this.Subject = subject;
            this.Content = content;
        }


        public string CorrelationId { get; }
        public DateTime CreatedAt { get; }

        public string Sender { get; }
        public string Recipient { get; }
        public string Subject { get; }
        public string Content { get; }
    }
}
