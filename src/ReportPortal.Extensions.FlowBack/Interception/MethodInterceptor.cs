using ReportPortal.Shared;
using ReportPortal.Shared.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Interception
{
    public class MethodInterceptor : IInterceptor
    {
        private readonly ILogScope _parentScope;
        private ILogScope _scope;
        public MethodInterceptor()
        {
            _parentScope = Log.ActiveScope;
            Console.WriteLine("Hi from .ctor");
        }

        public void OnBefore(string name)
        {
            _scope = _parentScope.BeginNewScope(name);

            Console.WriteLine("Hi from .OnBefore: " + name);
        }

        public void OnException(Exception exp)
        {
            _scope.Warn(exp.ToString());
            Console.WriteLine("Hi from .OnException: " + exp.Message);
        }

        public void OnAfter(object result)
        {
            string ret = result == null ? "null" : result.ToString();
            var message = $"Result: `{ret}`";
            _scope.Trace(message);

            _scope.Dispose();
            Console.WriteLine($"Hi from .OnAfter: Message: {message}");
        }
    }
}
