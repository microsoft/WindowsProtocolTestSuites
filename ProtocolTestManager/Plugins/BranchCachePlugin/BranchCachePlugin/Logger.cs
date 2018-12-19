// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestManager.Detector;

namespace Microsoft.Protocols.TestManager.BranchCachePlugin
{
    /// <summary>
    /// All log will be added to file
    /// </summary>
    public enum LogLevel
    {
        Information, // Added to File only
        Warning,     // Added to UI
        Error        // To throw exception
    }

    public class Logger
    {
        /// <summary>
        /// Write a log message.
        /// </summary>
        /// <param name="msg">Log message.</param>
        /// <param name="level">Log level.</param>
        /// <param name="startNewLine">Start a new line before this message when to UI. True by default.</param>
        /// <param name="style">The style of the message</param>
        public void AddLog(LogLevel level, string msg, bool startNewLine = true, LogStyle style = LogStyle.Default)
        {
            switch (level)
            {
                case LogLevel.Information:
                    DetectorUtil.WriteLog("[" + DateTime.Now + "]" + msg);
                    break;
                case LogLevel.Warning:
                    DetectorUtil.WriteLog(msg, startNewLine, style);
                    break;
                case LogLevel.Error:
                    AddLog(LogLevel.Information, msg);
                    throw new Exception(msg);
                default:
                    break;
            }
        }

        /// <summary>
        /// Add one empty line to log
        /// </summary>
        /// <param name="level"></param>
        public void AddLineToLog(LogLevel level)
        {
            AddLog(level, string.Empty);
        }

        /// <summary>
        /// Run the command
        /// </summary>
        /// <param name="command">The command to be executed</param>
        /// <returns></returns>
        public Process RunCmd(string command)
        {
            Process p = new System.Diagnostics.Process();

            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + command;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();
            return p;
        }

        /// <summary>
        /// Run the command and get the output
        /// </summary>
        /// <param name="command">The command to be executed</param>
        /// <returns></returns>
        public string RunCmdAndGetOutput(string command)
        {
            Process p = RunCmd(command);
            return p.StandardOutput.ReadToEnd();
        }
    }
}
