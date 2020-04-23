using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Task
{
    public class AssemblyInstrumentator
    {
        private string _assemblyPath;

        //private TaskLoggingHelper _logger;

        public AssemblyInstrumentator(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
            //_logger = logger;
        }

        public void Instrument()
        {
            //_logger.LogWarning($"Instrumenting: {_assemblyPath}");
            var readerParameters = new AssemblyReaderParametersFactory().CreateReaderParameters(_assemblyPath);
            using (AssemblyDefinition assemblyDef = AssemblyDefinition.ReadAssembly(_assemblyPath, readerParameters))
            {
                foreach (var module in assemblyDef.Modules)
                {
                    var allTypes = new List<TypeDefinition>();

                    foreach (var type in module.Types)
                    {
                        allTypes.Add(type);

                        foreach (var nestedType in type.NestedTypes)
                        {
                            allTypes.Add(nestedType);
                        }
                    }

                    foreach (var type in allTypes)
                    {
                        foreach (var method in type.Methods)
                        {
                            if (true)
                            //if (method.HasCustomAttributes && method.HasBody)
                            {
                                var flowBackAttribute = method.CustomAttributes.FirstOrDefault(ca => ca.AttributeType.FullName == typeof(FlowBackAttribute).FullName);

                                //if (flowBackAttribute != null)
                                if (true)
                                {
                                    var processor = method.Body.GetILProcessor();

                                    var isIgnored = false;

                                    var flowBackLogicalName = method.ReturnType.Name + " " + type.Name + "." + method.Name;
                                    if (flowBackAttribute != null)
                                    {
                                        var flowBackAttributeNameFromCtor = flowBackAttribute.ConstructorArguments.FirstOrDefault(a => a.Type == module.TypeSystem.String).Value;
                                        if (flowBackAttributeNameFromCtor != null)
                                        {
                                            flowBackLogicalName = flowBackAttributeNameFromCtor.ToString();
                                        }
                                        else
                                        {
                                            var flowBackNameProperty = flowBackAttribute.Properties.FirstOrDefault(p => p.Name == "Name");

                                            if (flowBackNameProperty.Argument.Value != null)
                                            {
                                                flowBackLogicalName = flowBackNameProperty.Argument.Value.ToString();
                                            }

                                            var flowBackIgnoreProperty = flowBackAttribute.Properties.FirstOrDefault(p => p.Name == "Ignore");
                                            if (flowBackIgnoreProperty.Argument.Value != null)
                                            {
                                                isIgnored = (bool)flowBackIgnoreProperty.Argument.Value;
                                            }
                                        }
                                    }

                                    if (!isIgnored)
                                    {
                                        var firstUserInstruction = processor.Body.Instructions.First();
                                        var lastUserInstrruction = processor.Body.Instructions.Last();

                                        VariableDefinition retValueDef = null;


                                        Instruction lastLdlocInstruction = null;

                                        if (method.ReturnType != module.TypeSystem.Void)
                                        {
                                            retValueDef = new VariableDefinition(method.ReturnType);
                                            processor.Body.Variables.Add(retValueDef);
                                            lastLdlocInstruction = processor.Create(OpCodes.Ldloc, retValueDef);
                                            processor.Append(lastLdlocInstruction);
                                        }
                                        else
                                        {
                                            lastLdlocInstruction = processor.Create(OpCodes.Nop);
                                            processor.Append(lastLdlocInstruction);
                                        }

                                        var retInstruction = processor.Create(OpCodes.Ret);
                                        processor.Append(retInstruction);

                                        var i_interceptor_type = module.ImportReference(typeof(Interception.IInterceptor));

                                        var beforeInstructions = new List<Instruction>();

                                        var varDef = new VariableDefinition(i_interceptor_type);
                                        processor.Body.Variables.Add(varDef);

                                        // Interception.IInterceptor rp_intercepter = new Interception.MethodInterceptor();
                                        beforeInstructions.Add(processor.Create(OpCodes.Newobj, method.Module.ImportReference(typeof(Interception.MethodInterceptor).GetConstructor(new Type[] { }))));
                                        beforeInstructions.Add(processor.Create(OpCodes.Stloc, varDef));
                                        // rp_intercepter.OnBefore();
                                        beforeInstructions.Add(processor.Create(OpCodes.Ldloc, varDef));
                                        beforeInstructions.Add(processor.Create(OpCodes.Ldstr, flowBackLogicalName));
                                        beforeInstructions.Add(processor.Create(OpCodes.Callvirt, method.Module.ImportReference(typeof(Interception.IInterceptor).GetMethod("OnBefore"))));

                                        beforeInstructions.Reverse();

                                        foreach (var instruction in beforeInstructions)
                                        {
                                            processor.InsertBefore(method.Body.Instructions[0], instruction);
                                        }


                                        // inner try
                                        var handlerInstructions = new List<Instruction>();
                                        // Exception exp
                                        var exceptionType = module.ImportReference(typeof(Exception));
                                        var expVar = new VariableDefinition(exceptionType);
                                        method.Body.Variables.Add(expVar);
                                        handlerInstructions.Add(processor.Create(OpCodes.Stloc, expVar));
                                        handlerInstructions.Add(processor.Create(OpCodes.Ldloc, varDef));
                                        handlerInstructions.Add(processor.Create(OpCodes.Ldloc, expVar));
                                        handlerInstructions.Add(processor.Create(OpCodes.Callvirt, method.Module.ImportReference(typeof(Interception.IInterceptor).GetMethod("OnException"))));
                                        handlerInstructions.Add(processor.Create(OpCodes.Rethrow));

                                        foreach (var instruction in handlerInstructions)
                                        {
                                            processor.InsertBefore(lastLdlocInstruction, instruction);
                                        }

                                        //finally try
                                        var finallyInstructions = new List<Instruction>();
                                        finallyInstructions.Add(processor.Create(OpCodes.Ldloc, varDef));
                                        if (retValueDef != null)
                                        {
                                            finallyInstructions.Add(processor.Create(OpCodes.Ldloc, retValueDef));
                                            if (retValueDef.VariableType.IsValueType)
                                            {
                                                finallyInstructions.Add(processor.Create(OpCodes.Box, retValueDef.VariableType));
                                            }
                                        }
                                        else
                                        {
                                            finallyInstructions.Add(processor.Create(OpCodes.Ldnull));
                                        }
                                        finallyInstructions.Add(processor.Create(OpCodes.Callvirt, method.Module.ImportReference(typeof(Interception.IInterceptor).GetMethod(nameof(Interception.IInterceptor.OnAfter)))));

                                        finallyInstructions.Add(processor.Create(OpCodes.Endfinally));

                                        var finalLeave = processor.Create(OpCodes.Leave, lastLdlocInstruction);
                                        processor.InsertBefore(lastLdlocInstruction, finalLeave);

                                        foreach (var instruction in finallyInstructions)
                                        {
                                            processor.InsertBefore(lastLdlocInstruction, instruction);
                                        }

                                        var firstIndex = processor.Body.Instructions.IndexOf(firstUserInstruction);
                                        var lastIndex = processor.Body.Instructions.IndexOf(lastUserInstrruction);
                                        for (int i = firstIndex; i <= lastIndex; i++)
                                        {
                                            var inst = processor.Body.Instructions[i];

                                            if (inst.OpCode.Code == Code.Ret)
                                            {
                                                if (retValueDef != null)
                                                {
                                                    processor.InsertBefore(inst, processor.Create(OpCodes.Stloc, retValueDef));
                                                    lastIndex++;
                                                }

                                                inst.OpCode = OpCodes.Leave;
                                                inst.Operand = finalLeave;
                                            }
                                        }



                                        var catchHandler = new ExceptionHandler(ExceptionHandlerType.Catch)
                                        {
                                            TryStart = firstUserInstruction,
                                            TryEnd = handlerInstructions.First(),
                                            CatchType = module.ImportReference(typeof(Exception)),
                                            HandlerStart = handlerInstructions.First(),
                                            HandlerEnd = handlerInstructions.Last().Next
                                        };

                                        var finallyhandler = new ExceptionHandler(ExceptionHandlerType.Finally)
                                        {
                                            TryStart = firstUserInstruction,
                                            TryEnd = finallyInstructions.First(),
                                            HandlerStart = finallyInstructions.First(),
                                            HandlerEnd = finallyInstructions.Last().Next
                                        };

                                        method.Body.ExceptionHandlers.Add(catchHandler);
                                        method.Body.ExceptionHandlers.Add(finallyhandler);


                                        method.Body.OptimizeMacros();
                                    }
                                }
                            }
                        }
                    }
                }

                var writerParameters = new AssemblyWriterParametersFactory().CreateWriterParameters(_assemblyPath);
                assemblyDef.Write(writerParameters);
            }
        }
    }
}

