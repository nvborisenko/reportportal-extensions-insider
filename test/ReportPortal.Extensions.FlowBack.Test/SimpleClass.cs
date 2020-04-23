using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ReportPortal.Extensions.FlowBack.Test
{
    public class SimpleClass
    {
        private Internal.SimpleClass _simpleClass = new Internal.SimpleClass("q");

        [Test]
        public void SimpleEmptyMethod()
        {
            _simpleClass.SimpleEmptyMethod();
        }

        [Test]
        public void SimpleMethod()
        {
            _simpleClass.SimpleMethod();
        }

        [Test]
        public void SimpleReturnValueMethod()
        {
            var res = _simpleClass.SimpleReturnValueMethod();
            Assert.AreEqual(5, res);
        }

        [Test]
        [TestCase(1, 4, 5)]
        [TestCase(1, 5, 6)]
        [TestCase(33, 22, 55)]
        public void SimpleReturnSum(int a, int b, int c)
        {
            var res = _simpleClass.Sum(a, b);
            Assert.AreEqual(c, res);
        }

        [Test]
        [TestCase(1, 4, 5)]
        [TestCase(1, 5, 6)]
        [TestCase(33, 22, 55)]
        public async System.Threading.Tasks.Task SimpleAsyncReturnSum(int a, int b, int c)
        {
            var res = await _simpleClass.SimpleSumAsyncMethod(a, b, null);
            Assert.AreEqual(c, res);
        }

        [Test]
        public void SimpleReturnRefMethod()
        {
            var res = _simpleClass.SimpleReturnRefMethod();
            Assert.AreEqual(res, typeof(Internal.SimpleClass));
        }

        [Test]
        public void SimpleThrowsExceptionMethod()
        {
            var exp = Assert.Throws<Exception>(() => _simpleClass.ThrowsExceptionMethod());
            Assert.AreEqual("asd", exp.Message);
        }

        [Test]
        public void SimpleThrowsExceptionWithTryCatchBlock()
        {
            var a = _simpleClass.WithTryCatchBlockMethod();
            Assert.AreEqual(1, a);
        }

        [Test]
        public async System.Threading.Tasks.Task SimpleAsyncMethod()
        {
            await _simpleClass.SimpleAsyncMethod();
        }

        [Test]
        public async System.Threading.Tasks.Task SimpleAsyncRetValueTask()
        {
            var res = await _simpleClass.SimpleAsyncRetValueMethod();
            Assert.AreEqual(6, res);
        }

        [Test]
        public void SimpleAsyncThrowsTask()
        {
            Assert.ThrowsAsync<Exception>(() => _simpleClass.SimpleAsyncThrowsMethod());
        }
    }
}