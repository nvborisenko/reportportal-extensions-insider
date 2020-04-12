using System;
using System.Threading.Tasks;

namespace ReportPortal.Extensions.FlowBack.Test.Internal
{
    public class SimpleClass
    {
        [FlowBack("qwe")]
        public void SimpleEmptyMethod()
        {

        }

        [FlowBack("asd")]
        public void SimpleMethod()
        {

        }

        [FlowBack("a")]
        public int SimpleReturnValueMethod()
        {
            return 5;
        }

        [FlowBack("a")]
        public Type SimpleReturnRefMethod()
        {
            return typeof(SimpleClass);
        }

        [FlowBack("asd")]
        public void ThrowsExceptionMethod()
        {
            throw new Exception("asd");
        }

        [FlowBack("asd")]
        public int WithTryCatchBlockMethod()
        {
            try
            {
                throw new Exception();
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        [FlowBack("a")]
        public async Task SimpleAsyncMethod()
        {
            await Task.Delay(0);
        }

        [FlowBack("a")]
        public async Task SimpleAsyncMethodWithTryCatch()
        {
            try
            {
                await Task.Delay(0);
            }
            catch (Exception)
            { }
        }
    }
}
