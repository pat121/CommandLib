using System;

namespace CommandLib.Constraints
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class ConstraintAttribute : Attribute
    {
        public abstract string GetFailureMessage();
        public abstract bool MeetsConstraint(string argument);
    }
}
