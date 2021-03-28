using ReportPortal.Shared;
using ReportPortal.Shared.Execution.Logging;
using System;

namespace ReportPortal.Extensions.Insider.Interception
{
    public class MethodInterceptor : IInterceptor
    {
        private readonly ILogScope _parentScope;
        private ILogScope _scope;
        public MethodInterceptor()
        {
            _parentScope = Context.Current.Log;
        }

        public void OnBefore(string name, ParamInfo[] parameters)
        {
            _scope = _parentScope.BeginScope(name);

            if (parameters != null)
            {
                foreach (var paramInfo in parameters)
                {
                    _scope.Trace($"{paramInfo.Name} `{paramInfo.Value}`");
                }
            }
        }

        public void OnException(Exception exp)
        {
            _scope.Warn(exp.ToString());
        }

        public void OnAfter(object result)
        {
            string ret = result == null ? "null" : result.ToString();
            var message = $"Returning `{ret}`";
            _scope.Trace(message);

            _scope.Dispose();
        }

        public void OnAfter()
        {
            _scope.Dispose();
        }
    }
}
