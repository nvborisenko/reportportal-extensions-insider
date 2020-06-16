using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.Insider.Interception
{
    public interface IInterceptor
    {
        void OnBefore(string name);

        void OnException(Exception exp);

        void OnAfter(object result);
    }
}
