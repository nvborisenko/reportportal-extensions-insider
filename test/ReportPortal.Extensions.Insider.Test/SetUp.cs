using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReportPortal.Extensions.Insider.Test
{
    [SetUpFixture]
    public class SetUp
    {
        [OneTimeSetUp]
        public static void SetUpMethod()
        {
            var assemblyToInstrument = Environment.CurrentDirectory + "/ReportPortal.Extensions.Insider.Test.Internal.dll";

            var instrumentator = new Task.AssemblyInstrumentator(assemblyToInstrument);

            instrumentator.Instrument();
        }
    }
}
