using System;
using TauCode.Mq.Abstractions;

namespace DemoCommon
{
    public class SendMessageMessage : IMessage
    {
        public SendMessageMessage()
        {   
        }

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


        public string CorrelationId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public static SendMessageMessage FromDomainEvent(SendMessageDomainEvent domainEvent)
        {
            return new SendMessageMessage(
                domainEvent.CorrelationId,
                domainEvent.OccurredAt,
                domainEvent.Sender,
                domainEvent.Recipient,
                domainEvent.Subject,
                domainEvent.Content);
        }
    }
}
