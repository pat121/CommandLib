using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLib
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class RequireAnyAttribute : Attribute
    {
    }
}
