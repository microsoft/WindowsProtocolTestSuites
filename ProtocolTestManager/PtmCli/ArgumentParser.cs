﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.CLI
{
    /// <summary>
    /// Command line argument parser
    /// </summary>
    public class Arguments
    {
        /// <summary>
        /// Show help text.
        /// </summary>
        [Argument("help", "h", ArgType.Switch)]
        public bool Help { get; set; }

        /// <summary>
        /// The path of the profile.
        /// </summary>
        [Argument("profile", "p")]
        public string Profile { get; set; }

        /// <summary>
        /// Run only checked test cases.
        /// </summary>
        [Argument("selected", "s", ArgType.Switch)]
        public bool SelectedOnly { get; set; }

        /// <summary>
        /// The text report file.
        /// </summary>
        [Argument("report", "r")]
        public string Report { get; set; }

        /// <summary>
        /// Choose the outcome of the test cases to be included in the text report.
        /// Possible values are pass, fail, inconclusive, notrun.
        /// Saparate by comma without space.
        /// </summary>
        [Argument("outcome")]
        public string OutCome { get; set; }

        /// <summary>
        /// The way to sort the test cases in the text report.
        /// </summary>
        [Argument("sortby")]
        public string SortBy { get; set; }

        /// <summary>
        /// The separator in the text report. By default is space.
        /// Possible values are space, comma.
        /// </summary>
        [Argument("separator")]
        public string Separator { get; set; }
        /// <summary>
        /// Key: argument name, Value: An instance of Argument to the key.
        /// </summary>
        private Dictionary<string, Argument> KnownArgs;

        /// <summary>
        /// Parses command line arguments.
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <returns>Instance of Arguments</returns>
        public static Arguments Parse(string[] args)
        {
            Arguments arg = new Arguments();
            arg.KnownArgs = new Dictionary<string, Argument>();

            Type arguments = typeof(Arguments);
            foreach (PropertyInfo property in arguments.GetProperties())
            {
                ArgumentAttribute attribute = Attribute.GetCustomAttribute(property, typeof(ArgumentAttribute)) as ArgumentAttribute;
                if (attribute == null) continue;
                if (attribute.ArgumentType == ArgType.Switch) property.SetValue(arg, false, null);
                else property.SetValue(arg, null, null);
                arg.KnownArgs.Add(attribute.Name, new Argument(attribute.ArgumentType, property));
                if (attribute.Alias != null) arg.KnownArgs.Add(attribute.Alias, new Argument(attribute.ArgumentType, property));
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i][0] != '-')
                    throw new InvalidArgumentException(string.Format(StringResources.InvalidArgumentMessage, args[i]));
                if (args[i].Length == 1)
                    throw new InvalidArgumentException(string.Format(StringResources.InvalidArgumentMessage, args[i]));
                string name = args[i].Substring(1);
                if (!arg.KnownArgs.ContainsKey(name))
                    throw new InvalidArgumentException(string.Format(StringResources.UnknownArgumentMessage, args[i]));
                Argument argInfo = arg.KnownArgs[name];
                switch (argInfo.ArgumentType)
                {
                    case ArgType.Switch:
                        argInfo.Property.SetValue(arg, true, null);
                        break;
                    case ArgType.StringValue:
                        i++; // Move to the next arg.
                        if (i >= args.Length) throw new InvalidArgumentException(string.Format(StringResources.MissingArgumentValue, name));
                        argInfo.Property.SetValue(arg, args[i], null);
                        break;
                }
            }
            return arg;
        }

        /// <summary>
        /// Convert argument to Enum. Throws InvalidArgumentException on error.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="argname">argument name</param>
        /// <param name="argValue">argument value</param>
        /// <returns></returns>
        public static T GetEnumArg<T>(string argname, string argValue) where T : struct
        {
            T enumValue;
            bool parse = Enum.TryParse<T>(argValue, true, out enumValue);
            if (!parse) throw new InvalidArgumentException(string.Format(StringResources.InvalidValueOfArgumentMessage, argname, argValue));
            return enumValue;
        }

    }

    /// <summary>
    /// Type of the argument.
    /// </summary>
    enum ArgType
    {
        /// <summary>
        /// The property is a switch. If the argument presents, the property is true. Otherwise false. E.g. "-h"
        /// </summary>
        Switch,
        /// <summary>
        /// The property has a string value. E.g. "-p Profile.ptm"
        /// </summary>
        StringValue
    }

    /// <summary>
    /// Argument class to hold the argument ArgType and the PropertyInfo
    /// </summary>
    sealed class Argument
    {
        public Argument(ArgType type, PropertyInfo property)
        {
            ArgumentType = type;
            Property = property;
        }
        public ArgType ArgumentType;
        public PropertyInfo Property;
    }

    /// <summary>
    /// Used to identify a command line argument.
    /// </summary>
    sealed class ArgumentAttribute : Attribute
    {
        /// <summary>
        /// Name of the argument
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alias of the argument
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Argument type
        /// </summary>
        public ArgType ArgumentType { get; set; }
        public ArgumentAttribute(string name, string alias = null, ArgType type = ArgType.StringValue)
        {
            Name = name;
            Alias = alias;
            ArgumentType = type;
        }
    }

    /// <summary>
    /// Invalid command line argument
    /// </summary>
    [Serializable]
    public class InvalidArgumentException : Exception
    {
        /// <summary>
        /// The constructor of InvalidArgumentException
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidArgumentException(string message)
            : base(message)
        {
        }
    }
}
