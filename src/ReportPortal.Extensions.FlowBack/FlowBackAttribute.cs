using System;

namespace ReportPortal.Extensions.FlowBack
{
    public class FlowBackAttribute : Attribute
    {
        public FlowBackAttribute()
        {

        }

        public FlowBackAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
