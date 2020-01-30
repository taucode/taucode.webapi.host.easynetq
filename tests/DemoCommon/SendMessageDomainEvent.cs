using System;
using TauCode.Domain.Events;

namespace DemoCommon
{
    public class SendMessageDomainEvent : IDomainEvent
    {
        public SendMessageDomainEvent(string from, string to, string subject, string content)
        {
            this.CorrelationId = Guid.NewGuid().ToString();
            this.OccurredAt = DateTime.UtcNow;

            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.Content = content;
        }

        public string CorrelationId { get; }
        public DateTime OccurredAt { get; }
        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Content { get; }
    }
}
