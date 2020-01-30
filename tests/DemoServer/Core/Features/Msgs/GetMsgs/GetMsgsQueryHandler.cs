using Domain.Msgs;
using NHibernate;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Queries;

namespace Core.Features.Msgs.GetMsgs
{
    public class GetMsgsQueryHandler : IQueryHandler<GetMsgsQuery>
    {
        private readonly ISession _session;

        public GetMsgsQueryHandler(ISession session)
        {
            _session = session;
        }

        public void Execute(GetMsgsQuery query)
        {
            var qry = _session.Query<Msg>();
            if (query.Id != null)
            {
                qry = qry.Where(x => x.Id == query.Id);
            }

            if (query.Sender != null)
            {
                qry = qry.Where(x => x.Sender == query.Sender);
            }

            if (query.Recipient != null)
            {
                qry = qry.Where(x => x.Recipient == query.Recipient);
            }

            var list = qry.ToList();
            var queryResult = new GetMsgsQueryResult
            {
                Items = list
                    .Select(x => new GetMsgsQueryResult.MsgDto
                    {
                        Id = x.Id,
                        CorrelationId = x.CorrelationId,
                        SentAt = x.SentAt,
                        Sender = x.Sender,
                        Recipient = x.Recipient,
                        Subject = x.Subject,
                        Content = x.Content,
                    })
                    .ToList(),
            };
            query.SetResult(queryResult);
        }

        public Task ExecuteAsync(GetMsgsQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            this.Execute(query);
            return Task.CompletedTask;
        }
    }
}
