using System;
using TauCode.Mq.Abstractions;

namespace DemoCommon
{
    public class SendMessageMessage : IMessage
    {
        public SendMessageMessage(
            string correlationId,
            DateTime createdAt,
            string from,
            string to,
            string subject,
            string content)
        {
            this.CorrelationId = correlationId;
            this.CreatedAt = createdAt;

            this.From = from;
            this.To = to;
            this.Subject = subject;
            this.Content = content;
        }


        public string CorrelationId { get; }
        public DateTime CreatedAt { get; }

        public string From { get; }
        public string To { get; }
        public string Subject { get; }
        public string Content { get; }
    }
}
