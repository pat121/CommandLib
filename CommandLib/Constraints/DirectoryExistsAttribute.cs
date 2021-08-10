using System.IO;

namespace CommandLib.Constraints
{
    public sealed class DirectoryExistsAttribute : ConstraintAttribute
    {
        public override string GetFailureMessage()
        {
            return "The given directory does not exist.";
        }
        public override bool MeetsConstraint(string argument)
        {
            return Directory.Exists(argument);
        }
    }
}
