using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLib
{
    public struct Result
    {
        public readonly string Message;
        public readonly ResultType Type;

        /// <summary>
        /// Represents a generic failure with no specific message.
        /// </summary>
        public static readonly Result Failure = new(null, ResultType.Failure);
        /// <summary>
        /// Represents a generic success with no specific message.
        /// </summary>
        public static readonly Result Success = new(null, ResultType.Success);
        
        public Result(string message, ResultType type)
        {
            Message = message;
            Type = type;
        }

        /// <summary>
        /// Generates a failure result with the given message.
        /// </summary>
        public static Result Fail(string message)
        {
            return new Result(message, ResultType.Failure);
        }
        /// <summary>
        /// Generates a successful result with the given message.
        /// </summary>
        public static Result Succeed(string message)
        {
            return new Result(message, ResultType.Success);
        }
        /// <summary>
        /// Converts this Result instance to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Message == null)
                return Type.ToString();

            if (Type == ResultType.Success)
                return Message;

            return $"{Type}: {Message}";
        }
    }

    public enum ResultType
    {
        Failure,
        Success
    }
}
