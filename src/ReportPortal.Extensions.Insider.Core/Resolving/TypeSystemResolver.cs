using Mono.Cecil;
using ReportPortal.Extensions.Insider.Core.Abstractions;
using System;
using System.Linq;

namespace ReportPortal.Extensions.Insider.Core.Resolving
{
    class TypeSystemResolver
    {
        private IInstrumentationLogger _logger;

        public TypeSystemResolver(IInstrumentationLogger logger)
        {
            _logger = logger;
        }

        public TypeReference Resolve(ModuleDefinition module, string @namespace, string name)
        {
            var netstandard = module.AssemblyReferences.FirstOrDefault(a => a.Name == "netstandard");
            var systemRuntimeRef = module.AssemblyReferences.FirstOrDefault(a => a.Name == "System.Runtime");
            var msCorLib = module.AssemblyReferences.FirstOrDefault(a => a.Name == "mscorlib");

            AssemblyDefinition referencedAssemblyDef = null;
            if (netstandard != null)
            {
                referencedAssemblyDef = module.AssemblyResolver.Resolve(netstandard);
                _logger.Info($"netstandard resolved as {referencedAssemblyDef}");
            }
            else if (msCorLib != null)
            {
                referencedAssemblyDef = module.AssemblyResolver.Resolve(msCorLib);
                _logger.Info($"mscorlib resolved as {referencedAssemblyDef}");
            }
            else if (systemRuntimeRef != null)
            {
                referencedAssemblyDef = module.AssemblyResolver.Resolve(systemRuntimeRef);
                _logger.Info($"System.Runtime resolved as {referencedAssemblyDef}");
            }

            if (referencedAssemblyDef == null)
            {
                throw new Exception($"Cannot resolve {@namespace}.{name} type. Target framework for `{module.Name}` module is not yet supported.");
            }

            var exceptionTypeRef = new TypeReference(@namespace, name, referencedAssemblyDef.MainModule, referencedAssemblyDef.MainModule);

            return module.ImportReference(exceptionTypeRef);
        }
    }
}
