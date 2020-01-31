using Domain.Msgs;
using FluentNHibernate.Mapping;

namespace Persistence.Maps
{
    public class MsgMap : ClassMap<Msg>
    {
        public MsgMap()
        {
            this.Id(x => x.Id);

            this.Map(x => x.CorrelationId);
            this.Map(x => x.SentAt);

            this.Map(x => x.Sender);
            this.Map(x => x.Recipient);
            this.Map(x => x.Subject);
            this.Map(x => x.Content);
        }
    }
}
