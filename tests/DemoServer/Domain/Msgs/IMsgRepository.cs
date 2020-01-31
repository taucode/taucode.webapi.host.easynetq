using System.Collections.Generic;

namespace Domain.Msgs
{
    public interface IMsgRepository
    {
        Msg GetById(MsgId id);
        IList<Msg> GetBySender(string sender);
        IList<Msg> GetByRecipient(string recipient);
        void Save(Msg msg);
        bool Delete(MsgId id);
    }
}
