using System;
using CommandLib;

namespace ConsumingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            CommandManager.RegisterCommandClass<Commands>();

            var command = args[0];
            var arguments = args.Length > 1 ? args[1..] : Array.Empty<string>();

            try
            {
                var result = CommandManager.Invoke(command, arguments);
                PrintResult(result);
            }
            catch (CommandException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        static void PrintResult(Result result)
        {
            switch (result.Type)
            {
                case ResultType.Failure:
                    Print(result.ToString(), ConsoleColor.Red);
                    break;
                case ResultType.Success:
                    Print(result.ToString(), ConsoleColor.Green);
                    break;
            }
        }
        public static void Print(string message, ConsoleColor color)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = old;
        }
    }
}
