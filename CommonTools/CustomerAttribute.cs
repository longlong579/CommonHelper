using System;

namespace HZZG.Common.Tolls
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class NameAttribute : Attribute
    {
        public string Name { get; private set; }

        public NameAttribute(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class DataUnitAttribute : Attribute
    {
        public string Unit { get; private set; }

        public DataUnitAttribute(string unit)
        {
            this.Unit = unit;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class AutoGenerateAttribute : Attribute
    {
        public bool IsGenerate { get; private set; }

        public AutoGenerateAttribute(bool isGenerate)
        {
            this.IsGenerate = isGenerate;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DsAttribute : Attribute
    {
        public string Des { get; private set; }

        public DsAttribute(string des)
        {
            this.Des = des;
        }
    }
}
