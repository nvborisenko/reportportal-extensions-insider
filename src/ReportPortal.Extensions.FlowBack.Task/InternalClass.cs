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
            Interception.IInterceptor __rp_inter = new Interception.MethodInterceptor();

            __rp_inter.OnBefore();

            try
            {
                // user's code
                Console.WriteLine("asd");
            }
            catch (Exception exp)
            {
                __rp_inter.OnException(exp);

                throw;
            }
            finally
            {
                __rp_inter.OnAfter();
            }
        }
    }
}