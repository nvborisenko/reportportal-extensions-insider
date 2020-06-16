using System;

namespace ReportPortal.Extensions.Insider
{
    public class InsiderAttribute : Attribute
    {
        public InsiderAttribute()
        {

        }

        public InsiderAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool Ignore { get; set; }
    }
}
