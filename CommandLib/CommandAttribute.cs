using System;

namespace CommandLib
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; }

        public CommandAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public bool CommandNameEquals(string commandName)
        {
            return commandName.Equals(CommandName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
