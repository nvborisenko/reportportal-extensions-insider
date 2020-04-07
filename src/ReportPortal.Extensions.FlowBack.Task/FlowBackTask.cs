using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Extensions.FlowBack.Task
{
    public class FlowBackTask : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            this.Log.LogWarning(TargetAssemblyPath);

            this.Log.LogError("I'm executed!");

            //throw new Exception("hmmmm");

            var inst = new AssemblyInstrumentator(TargetAssemblyPath);
            inst.Instrument();

            return true;
        }

        public string TargetAssemblyPath { get; set; }
    }
}
