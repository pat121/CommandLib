using System;
using System.Reflection;

namespace CommandLib
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ConversionAttribute : Attribute
    {
        private string _method;
        private Type _type;

        public ConversionAttribute(Type type, string methodName)
        {
            _method = methodName;
            _type = type;
        }

        public MethodInfo GetMethod()
        {
            return _type.GetMethod(_method);
        }
    }
}
