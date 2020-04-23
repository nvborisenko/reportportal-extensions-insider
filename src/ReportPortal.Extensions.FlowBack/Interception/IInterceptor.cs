using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Interception
{
    public interface IInterceptor
    {
        void OnBefore(string name);

        void OnException(Exception exp);

        void OnAfter(object result);
    }
}
