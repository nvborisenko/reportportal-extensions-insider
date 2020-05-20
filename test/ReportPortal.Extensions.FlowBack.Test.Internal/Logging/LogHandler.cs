using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Test.Internal.Logging
{
    [FlowBack(Ignore = true)]
    class LogHandler : ILogHandler
    {
        [FlowBack(Ignore = true)]
        public LogHandler()
        {

        }

        [FlowBack(Ignore = true)]
        public int Order => 1;

        [FlowBack(Ignore = true)]
        public void BeginScope(ILogScope logScope)
        {
            Console.WriteLine($"Beginning log scope: {logScope.Id}");
        }

        [FlowBack(Ignore = true)]
        public void EndScope(ILogScope logScope)
        {
            Console.WriteLine($"Ending log scope: {logScope.Id}");
        }

        [FlowBack(Ignore = true)]
        public bool Handle(ILogScope logScope, CreateLogItemRequest logRequest)
        {
            Console.WriteLine($"Log message, active scope: {logScope?.Id}");

            return true;
        }
    }
}
