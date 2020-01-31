using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli;
using TauCode.Cli.CommandSummary;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace DemoUser.Cli
{
    public class Worker : CliWorkerBase
    {
        public Worker()
            : base(
                typeof(Host).Assembly.GetResourceText(".Grammar.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            var host = (Host)this.AddIn.Host;

            var to = summary.Keys["to"].Single();
            var text = summary.Arguments["message-text"].Single();

            throw new NotImplementedException();
            //host.Publisher.Publish(new Greeting(
            //    host.UserName,
            //    to,
            //    text),
            //    to);
        }
    }
}
