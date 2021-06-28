// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.FileServerPlugin
{
    /// <summary>
    /// All log will be added to file
    /// </summary>
    public enum LogLevel
    {
        Information,   // Added to File only
        Warning,       // Added to UI
        Error,         // To throw exception
    }

    public class Logger
    {
        /// <summary>
        /// Write a log message.
        /// </summary> 
        /// <param name="level">Log level. Will throw exception when using LogLevel.Error</param>
        /// <param name="msg">Log message.</param>
        /// <param name="startNewLine">Start a new line before this message when to UI. True by default.</param>
        /// <param name="style">The style of the message</param>
        public void AddLog(LogLevel level, string msg, bool startNewLine = true, LogStyle style = LogStyle.Default)
        {
            switch (level)
            {
                case LogLevel.Information:
                case LogLevel.Warning:
                    DetectorUtil.WriteLog(msg, startNewLine, style);
                    break;
                case LogLevel.Error:
                    AddLog(LogLevel.Information, msg);
                    // PTM will handle the exception and pop up a dialog box to user.
                    throw new Exception(msg);
                default:
                    break;
            }
        }

        public void AddLineToLog(LogLevel level)
        {
            AddLog(level, string.Empty);
        }

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

        public string RunCmdAndGetOutput(string command)
        {
            Process p = RunCmd(command);
            return p.StandardOutput.ReadToEnd();
        }
    }
}
