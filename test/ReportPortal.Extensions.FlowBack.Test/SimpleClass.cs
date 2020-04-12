using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ReportPortal.Extensions.FlowBack.Test
{
    [TestClass]
    public class SimpleClass
    {
        private Internal.SimpleClass _simpleClass = new Internal.SimpleClass();

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            var assemblyToInstrument = "ReportPortal.Extensions.FlowBack.Test.Internal.dll";

            var instrumentator = new Task.AssemblyInstrumentator(assemblyToInstrument);

            instrumentator.Instrument();
        }

        [TestMethod]
        public void SimpleEmptyMethod()
        {
            _simpleClass.SimpleEmptyMethod();
        }

        [TestMethod]
        public void SimpleMethod()
        {
            _simpleClass.SimpleMethod();
        }

        [TestMethod]
        public void SimpleReturnValueMethod()
        {
            var res = _simpleClass.SimpleReturnValueMethod();
            Assert.AreEqual(5, res);
        }

        [TestMethod]
        public void SimpleReturnRefMethod()
        {
            var res = _simpleClass.SimpleReturnRefMethod();
            Assert.AreEqual(res, typeof(Internal.SimpleClass));
        }

        [TestMethod]
        public void SimpleThrowsExceptionMethod()
        {
            var exp = Assert.ThrowsException<Exception>(() => _simpleClass.ThrowsExceptionMethod());
            Assert.AreEqual("asd", exp.Message);
        }

        [TestMethod]
        public void SimpleThrowsExceptionWithTryCatchBlock()
        {
            var a = _simpleClass.WithTryCatchBlockMethod();
            Assert.AreEqual(1, a);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task SimpleAsyncMethod()
        {
            await _simpleClass.SimpleAsyncMethod();
        }
    }
}