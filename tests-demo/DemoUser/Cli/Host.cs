using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Cli;

namespace DemoUser.Cli
{
    public class Host : CliHostBase
    {
        private readonly AddIn _addIn;

        public Host(AddIn addIn)
            : base("demo", "1.0", true)
        {
            _addIn = addIn;
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new List<ICliAddIn>
            {
                _addIn,
            };
        }

        protected override string GetHelpImpl()
        {
            var workers = this
                .GetAddIns()
                .Single()
                .GetWorkers();

            var sb = new StringBuilder($"Available commands:{Environment.NewLine}");
            foreach (var worker in workers)
            {
                sb.AppendLine(worker.Descriptor.Verb);
            }

            return sb.ToString();
        }
    }
}
