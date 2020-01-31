using Core.Features.Pings.SendPing;
using Microsoft.AspNetCore.Mvc;
using TauCode.Cqrs.Commands;

namespace AppHost.Features.Pings.SendPing
{
    [ApiController]
    public class SendPingController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public SendPingController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [Route("api/pings")]
        public IActionResult SendPing([FromBody] SendPingCommand command)
        {
            _commandDispatcher.Dispatch(command);
            return this.NoContent();
        }
    }
}
