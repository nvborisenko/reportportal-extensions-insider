using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Task
{
    class InternalClass
    {
        public void SyncMethod()
        {
            Interception.IInterceptor rp_intercepter = new Interception.MethodInterceptor();

            rp_intercepter.OnBefore();

            try
            {
                // user's code
                Console.WriteLine("asd");
            }
            catch (Exception exp)
            {
                rp_intercepter.OnException(exp);

                throw;
            }
            finally
            {
                rp_intercepter.OnAfter();
            }
        }
    }
}