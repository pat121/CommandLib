using CommandLib.Constraints;

namespace ConsumingApp.CustomConstraints
{
    /// <summary>
    /// This example constraint makes sure that an argument is contained in a list of pre-approved values.
    /// If the argument isn't in the list, the constraint is not met and the command isn't executed.
    /// </summary>
    public class MatchesOneAttribute : ConstraintAttribute
    {
        private readonly string[] _options;

        public MatchesOneAttribute(params string[] options)
        {
            _options = options;
        }

        public override string GetFailureMessage()
        {
            return string.Format("The following values are allowed: {0}", string.Join(", ", _options));
        }
        public override bool MeetsConstraint(string argument)
        {
            foreach (var item in _options)
                if (item == argument)
                    return true;
            return false;
        }
    }
}
