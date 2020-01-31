using DemoUser.Cli.Workers;
using System.Collections.Generic;
using TauCode.Cli;

namespace DemoUser.Cli
{
    public class AddIn : CliAddInBase
    {
        private readonly INameWorker _nameWorker;
        private readonly ISendWorker _sendWorker;

        public AddIn(INameWorker nameWorker, ISendWorker sendWorker)
        {
            _nameWorker = nameWorker;
            _sendWorker = sendWorker;
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new List<ICliWorker>
            {
                _nameWorker,
                _sendWorker,
            };
        }

        protected override void OnNodeCreated()
        {
        }
    }
}
