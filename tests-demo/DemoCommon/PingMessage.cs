using System;
using TauCode.Mq.Abstractions;

namespace DemoCommon
{
    public class PingMessage : IMessage
    {
        public PingMessage()
        {   
        }

        public PingMessage(
            string correlationId,
            DateTime createdAt,
            string to,
            string text)
        {
            this.CorrelationId = correlationId;
            this.CreatedAt = createdAt;
            this.To = to ?? throw new ArgumentNullException(nameof(text));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string CorrelationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string To { get; set; }
        public string Text { get; set; }

        public static PingMessage FromDomainEvent(PingDomainEvent pingDomainEvent)
        {
            return new PingMessage(
                pingDomainEvent.CorrelationId,
                pingDomainEvent.OccurredAt,
                pingDomainEvent.To,
                pingDomainEvent.Text);
        }
    }
}
