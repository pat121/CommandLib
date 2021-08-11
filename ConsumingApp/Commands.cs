using System;
using System.IO;

using CommandLib;
using CommandLib.Constraints;

using ConsumingApp.CustomConstraints;

namespace ConsumingApp
{
    /// <summary>
    /// This class defines all the commands supported by the app. Commands have to be public static methods with a return type of <see cref="Result"/>.
    /// They also must have a <see cref="CommandAttribute"/> defined on them.
    /// </summary>
    public class Commands
    {
        /// <summary>
        /// This example command takes two parameters, <paramref name="action"/> and <paramref name="file"/>, both strings.
        /// The first has our custom <see cref="MatchesOneAttribute"/> constraint defined on it, which ensures that the argument provided
        /// for <paramref name="action"/> is either "view" or "delete". The second parameter has the built-in
        /// <see cref="FileExistsAttribute"/> constraint, which makes sure that whatever value is passed in as <paramref name="file"/>
        /// corresponds to an existing file.
        /// </summary>
        [Command("file")]
        public static Result FileMethod([MatchesOne("view", "delete")] string action, [FileExists] string file)
        {
            switch (action)
            {
                case "delete":
                    File.Delete(file);
                    break;
                case "view":
                    Console.WriteLine(File.ReadAllText(file));
                    break;
            }

            return Result.Success;
        }

        /// <summary>
        /// This example command demonstrates that arguments are automatically converted to numeric types, if possible.
        /// It also demonstrates that CommandLib supports default arguments.
        /// </summary>
        [Command("add")]
        public static Result AddMethod(int a, int b = 5)
        {
            Console.WriteLine("{0} + {1} = {2}", a, b, a + b);
            return Result.Success;
        }

        /// <summary>
        /// This example command demonstrates that multiple constraints can be applied to a single parameter. All constraints applied
        /// to a parameter must be satisfied, unless the <see cref="RequireAnyAttribute"/> is also applied to that parameter. This
        /// attribute makes it so that only one constraint on a parameter must be met for the command to execute.
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        [Command("run")]
        public static Result RunMethod([FileExists, EndsWith(".exe")] string program)
        {
            try
            {
                System.Diagnostics.Process.Start(program);
                return Result.Succeed("Process started successfully.");
            }
            catch (Exception e)
            {
                return Result.Fail(e.Message);
            }
        }

        /// <summary>
        /// In cases where the command manager can't automatically convert a command argument to its required type, you can use
        /// the <see cref="ConversionAttribute"/> to specify a method that will perform the conversion. The specified method
        /// has to be public and takes a single string parameter.
        /// </summary>
        [Command("delete-all")]
        public static Result DeleteAllMethod([Conversion(typeof(Commands), "Convert")] string[] files)
        {
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    Console.Write("Are you sure you want to delete {0}? ", file);
                    var key = Console.ReadKey();
                    
                    if (key.Key == ConsoleKey.Y)
                    {
                        File.Delete(file);
                        Console.WriteLine(" Deleted");
                    }
                    else
                    {
                        Console.WriteLine(" Skipping...");
                    }
                }
                else
                {
                    Console.WriteLine("Couldn't find file {0}, skipping...", file);
                }
            }

            return Result.Success;
        }

        public static string[] Convert(string input)
        {
            return input.Split(',');
        }
    }
}
