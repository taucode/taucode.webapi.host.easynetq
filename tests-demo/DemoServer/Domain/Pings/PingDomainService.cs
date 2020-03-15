using DemoCommon;
using TauCode.Domain.Events;

namespace Domain.Pings
{
    public class PingDomainService
    {
        public void Ping(string to, string text)
        {
            var pingDomainEvent = new PingDomainEvent(to, text);
            DomainEventPublisher.Current.Publish(pingDomainEvent);
        }
    }
}
