using DemoUser.Handlers;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli;
using TauCode.Cli.CommandSummary;
using TauCode.Cli.Data;
using TauCode.Extensions;
using TauCode.Mq;

namespace DemoUser.Cli.Workers
{
    public interface INameWorker : ICliWorker
    {
        string UserName { get; }
    }

    public class NameWorker : CliWorkerBase, INameWorker
    {
        private readonly IMessageSubscriber _messageSubscriber;

        public NameWorker(IMessageSubscriber messageSubscriber)
            : base(
                typeof(Host).Assembly.GetResourceText(".Name.lisp", true),
                null,
                true)
        {
            _messageSubscriber = messageSubscriber;
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);

            var userName = summary.Arguments["name"].Single();

            this.UserName = userName;
            this.Output.WriteLine($"Name is set to '{this.UserName}'.");

            try
            {
                _messageSubscriber.Stop();
            }
            catch
            {
                // dismiss
            }

            _messageSubscriber.Subscribe(typeof(PingHandler), this.UserName);
            _messageSubscriber.Start();
        }

        public string UserName { get; private set; }
    }
}
