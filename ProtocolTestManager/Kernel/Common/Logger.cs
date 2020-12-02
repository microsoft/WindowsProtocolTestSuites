// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Level of logs
    /// </summary>
    public enum LogLevel
    {
        Error,         // Indicates an exception is thrown
        Information,   // Informational log
        Debug,         // Only be available when "debug" is true
    }

    /// <summary>
    /// Logger is used to record useful logs for PTM.
    /// </summary>
    public static class Logger
    {
        public static string FileName = "PTMLog_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";

        /// <summary>
        /// Write a log message.
        /// </summary>
        /// <param name="msg">Log message.</param>
        /// <param name="level">Log level.</param>
        public static void AddLog(LogLevel level, string msg)
        {
            // The log of Debug level will only be added when "debug" is true in commandline parameters.
            if (!EnableDebugging && level == LogLevel.Debug)
            {
                return;
            }

            AppendLog($"[{DateTime.Now}][{level}]{msg}");
        }

        public static bool EnableDebugging = false;

        private static void AppendLog(string msg)
        {
            lock (FileName)
            {
                using (StreamWriter sw = File.AppendText(FileName))
                {
                    sw.WriteLine(msg);
                }
            }
        }
    }
}
