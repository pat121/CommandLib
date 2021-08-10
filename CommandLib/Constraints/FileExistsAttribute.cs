using System.IO;

namespace CommandLib.Constraints
{
    public sealed class FileExistsAttribute : ConstraintAttribute
    {
        public override string GetFailureMessage()
        {
            return "The given file does not exist.";
        }
        public override bool MeetsConstraint(string argument)
        {
            return File.Exists(argument);
        }
    }
}
