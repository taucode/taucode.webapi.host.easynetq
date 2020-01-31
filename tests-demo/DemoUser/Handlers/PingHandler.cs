using DemoCommon;
using Newtonsoft.Json;
using System;
using TauCode.Mq.Abstractions;

namespace DemoUser.Handlers
{
    public class PingHandler : MessageHandlerBase<PingMessage>
    {
        public override void Handle(PingMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            Console.WriteLine(json);
        }
    }
}
