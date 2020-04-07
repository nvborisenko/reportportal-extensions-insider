using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Task
{
    class AssemblyInstrumentator
    {
        private string _assemblyPath;
        public AssemblyInstrumentator(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        public void Instrument()
        {
            using (ModuleDefinition module = ModuleDefinition.ReadModule(_assemblyPath, new ReaderParameters { ReadWrite = true, ReadSymbols = true }))
            {
                foreach (var type in module.Types)
                {
                    foreach (var method in type.Methods)
                    {
                        var processor = method.Body.GetILProcessor();

                        Instruction ld = processor.Create(OpCodes.Ldstr, "Startinggg " + method.FullName);
                        var call = processor.Create(OpCodes.Call, method.Module.ImportReference(typeof(Console).GetMethod("WriteLine", new[] { typeof(string) })));

                        processor.InsertBefore(method.Body.Instructions[0], call);
                        processor.InsertBefore(method.Body.Instructions[0], ld);
                    }
                }

                module.Write(new WriterParameters { WriteSymbols = true });
            }
        }
    }
}
