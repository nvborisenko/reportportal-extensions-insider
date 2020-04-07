using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Task
{
    class InternalClass
    {

        public void SimpleMethod()
        {
            Console.WriteLine("qwe");
        }

        public void SyncMethod()
        {
            using (var scope = Log.ActiveScope.BeginNewScope("qwe"))
            {
                try
                {
                    // user's code
                    Console.WriteLine("asd");
                }
                catch (Exception exp)
                {
                    scope.Warn(exp.ToString());

                    throw;
                }
            }
        }
    }
}