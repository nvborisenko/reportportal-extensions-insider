using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Interception
{

    public class MethodInterceptor : IInterceptor
    {
        public MethodInterceptor()
        {
            Console.WriteLine("Hi from ctor");
        }

        public static void Execute(string a)
        {
            Console.WriteLine($"Hi new version from interceptor: {a}");
        }

        public void OnBefore()
        {
            Console.WriteLine("Hi from OnBefore");
        }

        public void OnException(Exception exp)
        {

        }

        public void OnAfter()
        {

        }
    }
}
