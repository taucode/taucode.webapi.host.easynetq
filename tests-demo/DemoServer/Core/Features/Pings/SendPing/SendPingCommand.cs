using TauCode.Cqrs.Commands;

namespace Core.Features.Pings.SendPing
{
    public class SendPingCommand : ICommand
    {
        public string To { get; set; }
        public string Text { get; set; }
    }
}
