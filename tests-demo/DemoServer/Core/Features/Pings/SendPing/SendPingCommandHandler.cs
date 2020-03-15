using DemoCommon;
using Domain.Pings;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Mq;
using TauCode.Domain.Events;
using TauCode.Mq;

namespace Core.Features.Pings.SendPing
{
    public class SendPingCommandHandler : DomainEventAwareCommandHandler<SendPingCommand>
    {
        private readonly PingDomainService _service;

        public SendPingCommandHandler(
            PingDomainService service,
            IMessagePublisher messagePublisher,
            IDomainEventConverter domainEventConverter)
        : base(messagePublisher, domainEventConverter)
        {
            _service = service;
        }

        protected override string GetTopic(IDomainEvent domainEvent) => ((PingDomainEvent)domainEvent).To;

        protected override void ExecuteImpl(SendPingCommand command)
        {
            _service.Ping(command.To, command.Text);
        }

        protected override Task ExecuteAsyncImpl(SendPingCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            this.ExecuteImpl(command);
            return Task.CompletedTask;
        }
    }
}
