namespace CommandLib
{
    public sealed class BadInvocationException : CommandException
    {
        public BadInvocationException()
        {

        }

        public BadInvocationException(string message) : base(message)
        {

        }
    }
}
