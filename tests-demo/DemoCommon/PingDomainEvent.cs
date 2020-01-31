using System;
using TauCode.Domain.Events;

namespace DemoCommon
{
    public class PingDomainEvent : IDomainEvent
    {
        public PingDomainEvent(string to, string text)
        {
            this.CorrelationId = Guid.NewGuid().ToString();
            this.OccurredAt = DateTime.UtcNow;
            this.To = to ?? throw new ArgumentNullException(nameof(text));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string CorrelationId { get; }
        public DateTime OccurredAt { get; }
        public string To { get; }
        public string Text { get; }
    }
}
