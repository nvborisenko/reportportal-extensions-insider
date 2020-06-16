using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReportPortal.Extensions.Insider.Task
{
    class AssemblyReaderParametersFactory
    {
        public ReaderParameters CreateReaderParameters(string assemblyPath)
        {
            var readerParameters = new ReaderParameters()
            {
                ReadWrite = true
            };

            var dirPath = Path.GetDirectoryName(assemblyPath);
            var assemblyName = Path.GetFileNameWithoutExtension(assemblyPath);
            if (File.Exists(Path.Combine(dirPath, assemblyName + ".pdb")))
            {
                readerParameters.ReadSymbols = true;
            }

            return readerParameters;
        }
    }
}
