using System;
using TauCode.Domain.Identities;

namespace Domain.Msgs
{
    [Serializable]
    public class MsgId : IdBase
    {
        public MsgId()
        {
        }

        public MsgId(Guid id)
            : base(id)
        {
        }

        public MsgId(string id)
            : base(id)
        {
        }
    }
}
