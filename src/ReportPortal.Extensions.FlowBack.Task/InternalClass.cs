using ReportPortal.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Task
{
    class InternalClass
    {
        public string SyncMethod()
        {
            Interception.IInterceptor rp_intercepter = new Interception.MethodInterceptor();

            rp_intercepter.OnBefore();

            try
            {
                // user's code
                return "asd";
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

            throw new Exception("exp");
        }

        public void ThrowingError()
        {
            throw new Exception("exp");
        }
    }
}