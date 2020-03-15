namespace DemoCommon
{
    public static class DemoCommonExtensions
    {
        public static SendMessageMessage ConvertDomainEvent(this SendMessageDomainEvent domainEvent)
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
