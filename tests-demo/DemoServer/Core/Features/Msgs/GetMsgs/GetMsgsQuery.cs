using Domain.Msgs;
using TauCode.Cqrs.Queries;

namespace Core.Features.Msgs.GetMsgs
{
    public class GetMsgsQuery : Query<GetMsgsQueryResult>
    {
        public MsgId Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
    }
}
