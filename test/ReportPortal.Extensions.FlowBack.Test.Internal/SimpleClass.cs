using System;
using System.Threading.Tasks;

namespace ReportPortal.Extensions.FlowBack.Test.Internal
{
    public class SimpleClass
    {
        private string _p1;
        public SimpleClass(string p1)
        {
            try
            {
                _p1 = p1;
            }
            catch (Exception)
            {

            }
        }

        [FlowBack]
        public void SimpleEmptyMethod()
        {

        }

        [FlowBack(Name = null)]
        public void SimpleMethod()
        {
            var a = DateTime.Now;
            var b = a.AddDays(5);

        }

        [FlowBack("a")]
        public int SimpleReturnValueMethod()
        {
            var a = 2;
            var b = 3;
            return a + b;
        }

        [FlowBack("q")]
        public int Sum(int a, int b)
        {
            var isNull = (object)string.IsNullOrEmpty(null);

            var res = a + b;
            var res2 = a + b + 1;
            if (res < res2)
            {
                return res;
            }

            return res2;
        }

        [FlowBack("a")]
        public Type SimpleReturnRefMethod()
        {
            return typeof(SimpleClass);
        }

        [FlowBack("asd")]
        public void ThrowsExceptionMethod()
        {
            for (int i = 0; i < 5; i++)
            {

            }

            ThrowInner();
        }

        private void ThrowInner()
        {
            throw new Exception("asd");
        }

        [FlowBack("asd")]
        public int WithTryCatchBlockMethod()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        [FlowBack(null)]
        public async Task SimpleAsyncMethod()
        {
            await Task.Delay(0);
        }

        public async Task<int> SimpleSumAsyncMethod(int a, int b, Type type)
        {
            await Task.Delay(1000);
            await Task.Delay(2000);
            using (var scope = Shared.Log.BeginScope("qwe"))
            {

            }
            return a + b;
        }

        [FlowBack("a")]
        public async Task<int> SimpleAsyncRetValueMethod()
        {
            await Task.Delay(1);
            return 6;
        }

        [FlowBack("a")]
        public async Task SimpleAsyncThrowsMethod()
        {
            await Task.Delay(0);

            throw new Exception("asd");
        }

        public async Task SimpleEmptyAsyncMethod()
        {

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

        [FlowBack(Ignore = true)]
        public void IggnoredMethof()
        {

        }
    }
}
