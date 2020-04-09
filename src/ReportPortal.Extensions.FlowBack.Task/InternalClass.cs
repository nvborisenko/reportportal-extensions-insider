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
                var a = Guid.NewGuid().ToString();
                return a;
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

        public void ThrowingError()
        {
            throw new Exception("exp");
        }

        public async System.Threading.Tasks.Task AsyncTask()
        {


            Interception.IInterceptor rp_intercepter = new Interception.MethodInterceptor();

            rp_intercepter.OnBefore();

            try
            {
                // user's code
                await System.Threading.Tasks.Task.Delay(5);
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