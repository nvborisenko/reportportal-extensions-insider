using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Interception
{
    public class MethodInterceptor : IInterceptor
    {
        public MethodInterceptor()
        {
            Console.WriteLine("Hi from .ctor");
        }

        public void OnBefore()
        {
            Console.WriteLine("Hi from .OnBefore");
        }

        public void OnException(Exception exp)
        {
            Console.WriteLine("Hi from .OnException: " + exp.Message);
        }

        public void OnAfter()
        {
            Console.WriteLine("Hi from .OnAfter");
        }
    }
}
