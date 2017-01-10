// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Detector.Common
{
    /// <summary>
    /// All log will be added to file
    /// </summary>
    public enum LogLevel
    {
        Advanced,   // Added to file only
        Normal,     // Added to UI
        Error,      // To throw exception
    }

    public class Logger
    {
        public string FileName { get; private set; }

        public Logger(string fileName)
        {
            FileName = fileName;
        }

        #region Helper functions for adding Log

        private void AppendLog(string msg)
        {
            using (StreamWriter sw = File.AppendText(FileName))
            {
                sw.WriteLine(msg);
            }
        }

        /// <summary>
        /// Write a log message.
        /// </summary>
        /// <param name="msg">Log message.</param>
        /// <param name="level">Log level.</param>
        /// <param name="startNewLine">Start a new line before this message when to UI. True by default.</param>
        /// <param name="style">The style of the message</param>
        public void AddLog(string msg, LogLevel level, bool startNewLine = true, LogStyle style = LogStyle.Default)
        {
            switch (level)
            {
                case LogLevel.Advanced:
                    if (this != null)
                        this.AppendLog("[" + DateTime.Now + "]" + msg);
                    break;
                case LogLevel.Normal:
                    AddLog(msg, LogLevel.Advanced);
                    DetectorUtil.WriteLog(msg, startNewLine, style);
                    break;
                case LogLevel.Error:
                    AddLog(msg, LogLevel.Advanced);
                    throw new Exception(msg);
                default:
                    throw new Exception(string.Format("UnKnown log level:{0}", level));
            }
        }

        public void AddLineToLog(LogLevel level)
        {
            AddLog(string.Empty, level);
        }

        #endregion
    }
}
