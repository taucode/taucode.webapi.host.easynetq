using DemoCommon;
using Domain.Msgs;
using TauCode.Mq.Abstractions;

namespace Core.Handlers.Msgs
{
    public class SendMessageMessageHandler : MessageHandlerBase<SendMessageMessage>
    {
        private readonly IMsgRepository _msgRepository;

        public SendMessageMessageHandler(IMsgRepository msgRepository)
        {
            _msgRepository = msgRepository;
        }

        public override void Handle(SendMessageMessage message)
        {
            var msg = new Msg(
                message.CorrelationId,
                message.CreatedAt,
                message.Sender,
                message.Recipient,
                message.Subject,
                message.Content);

            _msgRepository.Save(msg);
        }
    }
}
