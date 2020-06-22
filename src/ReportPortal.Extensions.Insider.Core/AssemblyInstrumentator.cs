using Mono.Cecil;
using ReportPortal.Extensions.Insider.Core.Abstractions;

namespace ReportPortal.Extensions.Insider.Core
{
    public class AssemblyInstrumentator
    {
        private IInstrumentationLogger _logger;

        public AssemblyInstrumentator(IInstrumentationLogger logger)
        {
            _logger = logger;
        }

        public void Instrument(string assemblyPath)
        {
            _logger.Info($"Instrumenting `{assemblyPath}`");

            var readerParameters = new AssemblyReaderParametersFactory().CreateReaderParameters(assemblyPath);
            using (AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters))
            {
                foreach (var module in assemblyDef.Modules)
                {
                    var moduleInstrumentator = new ModuleInstrumentator(_logger);
                    moduleInstrumentator.Instrument(module);
                }

                var writerParameters = new AssemblyWriterParametersFactory().CreateWriterParameters(assemblyPath);
                assemblyDef.Write(writerParameters);
            }
        }
    }
}

