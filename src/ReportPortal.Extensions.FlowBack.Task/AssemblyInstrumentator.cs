using Microsoft.Build.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Task
{
    class AssemblyInstrumentator
    {
        private string _assemblyPath;

        private TaskLoggingHelper _logger;

        public AssemblyInstrumentator(string assemblyPath, TaskLoggingHelper logger)
        {
            _assemblyPath = assemblyPath;
            _logger = logger;
        }

        public void Instrument()
        {
            _logger.LogWarning($"Instrumenting: {_assemblyPath}");

            using (ModuleDefinition module = ModuleDefinition.ReadModule(_assemblyPath, new ReaderParameters { ReadWrite = true, ReadSymbols = true }))
            {
                foreach (var type in module.Types)
                {
                    foreach (var method in type.Methods)
                    {
                        if (method.HasCustomAttributes)
                        {
                            if (method.CustomAttributes.Any(ca => ca.AttributeType.FullName == typeof(FlowBackAttribute).FullName))
                            {
                                var processor = method.Body.GetILProcessor();

                                var i_interceptor_type = module.ImportReference(typeof(Interception.IInterceptor));

                                var instructions = new List<Instruction>();
                                instructions.Add(processor.Create(OpCodes.Nop));

                                var varDef = new VariableDefinition(i_interceptor_type);
                                processor.Body.Variables.Add(varDef);

                                // Interception.IInterceptor rp_intercepter = new Interception.MethodInterceptor();
                                instructions.Add(processor.Create(OpCodes.Newobj, method.Module.ImportReference(typeof(Interception.MethodInterceptor).GetConstructor(new Type[] { }))));
                                instructions.Add(processor.Create(OpCodes.Stloc, varDef));
                                // rp_intercepter.OnBefore();
                                instructions.Add(processor.Create(OpCodes.Ldloc, varDef));
                                instructions.Add(processor.Create(OpCodes.Callvirt, method.Module.ImportReference(typeof(Interception.IInterceptor).GetMethod("OnBefore"))));

                                Instruction ld = processor.Create(OpCodes.Ldstr, "SSSS " + method.FullName);
                                var call = processor.Create(OpCodes.Call, method.Module.ImportReference(typeof(Interception.MethodInterceptor).GetMethod("Execute", new Type[] { typeof(string) })));

                                instructions.Reverse();

                                foreach (var instruction in instructions)
                                {
                                    processor.InsertBefore(processor.Body.Instructions[0], instruction);
                                }

                                processor.InsertAfter(instructions[0], call);
                                processor.InsertAfter(instructions[0], ld);
                            }
                        }
                    }
                }

                module.Write(new WriterParameters { WriteSymbols = true });
            }
        }
    }
}

