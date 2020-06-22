using Moq;
using NUnit.Framework;
using ReportPortal.Extensions.Insider.Core.Abstractions;
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
            var logger = new Mock<IInstrumentationLogger>();

            var assemblyToInstrument = TestContext.CurrentContext.TestDirectory + "/ReportPortal.Extensions.Insider.Test.Internal.NetStandard.dll";

            var instrumentator = new Core.AssemblyInstrumentator(logger.Object);

            instrumentator.Instrument(assemblyToInstrument);

            var filePath = TestContext.CurrentContext.TestDirectory + "/ReportPortal.Extensions.Insider.Test.Internal.NetCoreApp.dll";
            if (File.Exists(filePath))
            {
                var assemblyToAppCoreInstrument = filePath;
                var instrumentator2 = new Core.AssemblyInstrumentator(logger.Object);

                instrumentator2.Instrument(assemblyToAppCoreInstrument);
            }


        }
    }
}
