using CommandLib.Constraints;

namespace ConsumingApp.CustomConstraints
{
    /// <summary>
    /// This example constraint makes sure that an argument ends with a specified substring.
    /// </summary>
    public class EndsWithAttribute : ConstraintAttribute
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
