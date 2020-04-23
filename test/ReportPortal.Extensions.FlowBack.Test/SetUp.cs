using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Test
{
    [SetUpFixture]
    public class SetUp
    {
        [OneTimeSetUp]
        public static void SetUpMethod()
        {
            var assemblyToInstrument = Environment.CurrentDirectory + "/ReportPortal.Extensions.FlowBack.Test.Internal.dll";

            var instrumentator = new Task.AssemblyInstrumentator(assemblyToInstrument);

            instrumentator.Instrument();
        }
    }
}
