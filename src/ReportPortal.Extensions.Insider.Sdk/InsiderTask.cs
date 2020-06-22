using ReportPortal.Extensions.Insider.Sdk.Internal;
using ReportPortal.Extensions.Insider.Core.Abstractions;
using ReportPortal.Extensions.Insider.Core;

namespace ReportPortal.Extensions.Insider.Sdk
{
    public class InsiderTask : Microsoft.Build.Utilities.Task
    {
        private IInstrumentationLogger _logger;

        public InsiderTask()
        {
            _logger = new LogAdapter(this.Log);
        }

        public override bool Execute()
        {
            var inst = new AssemblyInstrumentator(_logger);
            inst.Instrument(TargetAssemblyPath);

            return true;
        }

        public string TargetAssemblyPath { get; set; }
    }
}
