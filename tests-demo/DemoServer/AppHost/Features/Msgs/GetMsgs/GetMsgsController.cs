using Core.Features.Msgs.GetMsgs;
using Domain.Msgs;
using Microsoft.AspNetCore.Mvc;
using TauCode.Cqrs.Queries;

namespace AppHost.Features.Msgs.GetMsgs
{
    [ApiController]
    public class GetMsgsController : ControllerBase
    {
        private readonly IQueryRunner _queryRunner;

        public GetMsgsController(IQueryRunner queryRunner)
        {
            _queryRunner = queryRunner;
        }

        [HttpGet]
        [Route("api/msgs")]
        public IActionResult GetMsgs(
            [FromQuery] MsgId id = null,
            [FromQuery] string sender = null,
            [FromQuery] string recipient = null)
        {
            var query = new GetMsgsQuery
            {
                Id = id,
                Recipient = recipient,
                Sender = sender,
            };

            _queryRunner.Run(query);
            return this.Ok(query.GetResult());
        }
    }
}
