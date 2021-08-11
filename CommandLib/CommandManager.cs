using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLib
{
    public static class CommandManager
    {
        private static Type _type;
        private static object Convert(string input, ParameterInfo param)
        {
            var destType = param.ParameterType;

            MethodInfo method;

            var attr = param.GetCustomAttribute<ConversionAttribute>();
            if (attr != null)
            {
                method = attr.GetMethod();
                if (method != null)
                    return method.Invoke(null, new object[] { input });
            }

            if (destType == typeof(string))
                return input;

            method = destType.GetMethod("Parse", new Type[] { typeof(string) });
            if (method != null)
                return method.Invoke(null, new object[] { input });

            var msg = string.Format("Could not convert argument {0} to required type {1}", input, destType);
            throw new CommandException(msg);
        }
        public static Result Invoke(string command, string[] arguments)
        {
            if (_type == null)
                throw new InvalidOperationException("The command system has not yet been initialized with a call to RegisterCommandClass()");

            var m = GetMethod(command);

            if (m == null)
                return Result.Fail("Command not found: " + command);

            var info = new Command(m);

            if (arguments.Length < info.RequiredArgCount)
                return Result.Fail("Too few arguments for command " + info.GetSignature());

            if (arguments.Length > info.RequiredArgCount + info.OptionalArgCount)
                return Result.Fail("Too many arguments for command " + info.GetSignature());

            var args = new object[info.OptionalArgCount + info.RequiredArgCount];

            for (var i = 0; i < args.Length; i++)
            {
                var param = info.Parameters[i];
                if (i >= arguments.Length)
                    args[i] = param.DefaultValue;
                else
                {
                    try
                    {
                        args[i] = Convert(arguments[i], param);
                    }
                    catch (CommandException e)
                    {
                        return Result.Fail(e.Message);
                    }
                    catch (Exception)
                    {
                        return Result.Fail($"Could not convert {arguments[i]} to required type {info.Parameters[i].ParameterType.Name}");
                    }

                    var constraints = param.GetCustomAttributes<Constraints.ConstraintAttribute>();
                    if (!constraints.Any())
                        continue;

                    var requireAny = param.IsDefined(typeof(RequireAnyAttribute));

                    bool success = true;
                    var messages = new List<string>();

                    foreach (var constraint in constraints)
                    {
                        var isMet = constraint.MeetsConstraint(arguments[i]);

                        if (!isMet)
                        {
                            messages.Add(constraint.GetFailureMessage());
                            if (requireAny)
                                continue;

                            success = false;
                            break;
                        }

                        if (requireAny)
                            break;
                    }

                    if (!success)
                        return Result.Fail(string.Join(Environment.NewLine, messages));
                }
            }

            return (Result)info._method.Invoke(null, args);
        }
        private static MethodInfo GetMethod(string name)
        {
            var flags = BindingFlags.Static | BindingFlags.Public;
            foreach (var m in _type.GetMethods(flags))
            {
                if (m.GetCustomAttribute(typeof(CommandAttribute)) is CommandAttribute attr)
                {
                    if (attr.CommandNameEquals(name))
                        return m;
                }
            }

            return default;
        }
        public static void RegisterCommandClass<T>() where T : class
        {
            _type = typeof(T);
        }
    }
}
