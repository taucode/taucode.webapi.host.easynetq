using System;
using TauCode.Domain.Events;

namespace DemoCommon
{
    public class SendMessageDomainEvent : IDomainEvent
    {
        public SendMessageDomainEvent(string sender, string recipient, string subject, string content)
        {
            this.CorrelationId = Guid.NewGuid().ToString();
            this.OccurredAt = DateTime.UtcNow;

            this.Sender = sender;
            this.Recipient = recipient;
            this.Subject = subject;
            this.Content = content;
        }

        public string CorrelationId { get; }
        public DateTime OccurredAt { get; }
        public string Sender { get; }
        public string Recipient { get; }
        public string Subject { get; }
        public string Content { get; }
    }
}
