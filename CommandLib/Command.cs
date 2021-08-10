using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLib
{
    internal struct Command
    {
        internal MethodInfo _method;
        public string Name { get; }
        public int OptionalArgCount { get; }
        public ParameterInfo[] Parameters { get; }
        public int RequiredArgCount { get; }

        public Command(MethodInfo method)
        {
            _method = method;

            Name = method.Name;
            Parameters = method.GetParameters();
            OptionalArgCount = RequiredArgCount = 0;

            for (var i = 0; i < Parameters.Length; i++)
            {
                if (Parameters[i].IsOptional)
                {
                    OptionalArgCount = Parameters.Length - RequiredArgCount;
                    break;
                }
                RequiredArgCount++;
            }
        }

        public string GetName()
        {
            var attr = Attribute.GetCustomAttribute(_method, typeof(CommandAttribute)) as CommandAttribute;
            return attr.CommandName;
        }
        public Type[] GetParameterTypes()
        {
            return Parameters.Select(x => x.GetType()).ToArray();
        }
        public string GetSignature()
        {
            var sb = new System.Text.StringBuilder(GetName());
            foreach (var param in Parameters)
            {
                sb.Append(" <");
                sb.Append(param.ParameterType.Name);
                sb.Append(' ');
                sb.Append(param.Name);

                if (param.IsOptional)
                {
                    sb.Append(" default=");
                    sb.Append(param.DefaultValue ?? "");
                }
                sb.Append('>');
            }
            return sb.ToString();
        }
    }
}
