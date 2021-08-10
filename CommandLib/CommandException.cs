using System;

namespace CommandLib
{
    public class CommandException : ApplicationException
    {
        public CommandException()
        {

        }
        public CommandException(string message) : base(message) { }
    }
}
