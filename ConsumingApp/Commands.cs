using System;

using CommandLib;
using CommandLib.Constraints;

namespace ConsumingApp
{
    public class Commands
    {
        [Command("Example")]
        public static Result Method([FileExists, EndsWith(".html")] string path)
        {
            Console.WriteLine(path);
            return Result.Success;
        }
    }

    class EndsWithAttribute : ConstraintAttribute
    {
        private string _suffix;

        public EndsWithAttribute(string suffix)
        {
            _suffix = suffix;
        }

        public override string GetFailureMessage()
        {
            return $"The given string did not end with '{_suffix}'.";
        }
        public override bool MeetsConstraint(string argument)
        {
            return argument.EndsWith(_suffix);
        }
    }
}
