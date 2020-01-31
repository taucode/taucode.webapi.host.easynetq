using System.Collections.Generic;
using System.Linq;
using TauCode.Cli;
using TauCode.Cli.CommandSummary;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace DemoUser.Cli.Workers
{
    public interface INameWorker : ICliWorker
    {
        string UserName { get; }
    }

    public class NameWorker : CliWorkerBase, INameWorker
    {
        public NameWorker()
            : base(
                typeof(Host).Assembly.GetResourceText(".Name.lisp", true),
                null,
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);

            var userName = summary.Arguments["name"].Single();
            this.UserName = userName;

            this.Output.WriteLine($"Name is set to '{this.UserName}'.");
        }

        public string UserName { get; private set; }
    }
}
