using Domain.Msgs;
using System;
using System.Collections.Generic;

namespace Core.Features.Msgs.GetMsgs
{
    public class GetMsgsQueryResult
    {
        public IList<MsgDto> Items { get; set; }

        public class MsgDto
        {
            public MsgId Id { get;  set; }
            public string CorrelationId { get;  set; }
            public DateTime SentAt { get;  set; }
            public string Sender { get;  set; }
            public string Recipient { get;  set; }
            public string Subject { get;  set; }
            public string Content { get;  set; }
        }
    }
}
