using Microsoft.Build.Utilities;
using ReportPortal.Extensions.Insider.Core.Abstractions;

namespace ReportPortal.Extensions.Insider.Sdk.Internal
{
    class LogAdapter : IInstrumentationLogger
    {
        private TaskLoggingHelper _msBuildLogger;

        public LogAdapter(TaskLoggingHelper msBuildLoger)
        {
            _msBuildLogger = msBuildLoger;
        }

        public void Info(string message)
        {
            _msBuildLogger.LogMessage(message);
        }

        public void Error(string message)
        {
            _msBuildLogger.LogError(message);
        }
    }
}
