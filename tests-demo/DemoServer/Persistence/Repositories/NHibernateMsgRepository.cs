using Domain.Msgs;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Repositories
{
    public class NHibernateMsgRepository : IMsgRepository
    {
        private readonly ISession _session;

        public NHibernateMsgRepository(ISession session)
        {
            _session = session;
        }

        public Msg GetById(MsgId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return _session
                .Query<Msg>()
                .SingleOrDefault(x => x.Id == id);
        }

        public IList<Msg> GetBySender(string sender)
        {
            return _session
                .Query<Msg>()
                .Where(x => x.Sender == sender)
                .ToList();
        }

        public IList<Msg> GetByRecipient(string recipient)
        {
            return _session
                .Query<Msg>()
                .Where(x => x.Recipient == recipient)
                .ToList();
        }

        public void Save(Msg msg)
        {
            _session.SaveOrUpdate(msg);
        }

        public bool Delete(MsgId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var msg = _session
                .Query<Msg>()
                .SingleOrDefault(x => x.Id == id);

            if (msg == null)
            {
                return false;
            }

            _session.Delete(msg);

            return true;
        }
    }
}
