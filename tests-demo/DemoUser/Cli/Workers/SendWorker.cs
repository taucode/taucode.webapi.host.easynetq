using DemoCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli;
using TauCode.Cli.CommandSummary;
using TauCode.Cli.Data;
using TauCode.Extensions;
using TauCode.Mq;

namespace DemoUser.Cli.Workers
{
    public interface ISendWorker : ICliWorker
    {
    }

    public class SendWorker : CliWorkerBase, ISendWorker
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly INameWorker _nameWorker;

        public SendWorker(IMessagePublisher messagePublisher, INameWorker nameWorker)
            : base(
                typeof(Host).Assembly.GetResourceText(".Send.lisp", true),
                null,
                true)
        {
            _messagePublisher = messagePublisher;
            _nameWorker = nameWorker;
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);

            var userName = _nameWorker.UserName;
            if (userName == null)
            {
                throw new InvalidOperationException("Name not send. Use 'name' command to set name.");
            }

            var to = summary.Keys["to"].Single();
            var subject = summary.Keys["subject"].Single();
            var content = summary.Keys["content"].Single();

            var message = new SendMessageMessage(
                Guid.NewGuid().ToString(),
                DateTime.UtcNow,
                userName,
                to,
                subject,
                content);

            _messagePublisher.Publish(message);
        }
    }
}
