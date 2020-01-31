using System.Collections.Generic;
using TauCode.Cli;

namespace DemoUser.Cli
{
    public class AddIn : CliAddInBase
    {
        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new List<ICliWorker>
            {
                new Worker(),
            };
        }
    }
}
