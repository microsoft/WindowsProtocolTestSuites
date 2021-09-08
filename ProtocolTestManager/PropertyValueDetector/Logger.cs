// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Protocols.TestManager.Detector
{
    /// <summary>
    /// All log will be added to file
    /// </summary>
    public enum DetectLogLevel
    {
        Information,    // Added to file only
        Warning,        // Added to UI
        Error,          // To throw exception
    }

    public class DetectLogger
    {
        private int StepIndex { get; set; }

        private DetectContext DetectContext;

        private Dictionary<int, LogStyle> stepStatus = new Dictionary<int, LogStyle>();

        /// <summary>
        /// Write a log message.
        /// </summary> 
        /// <param name="level">Log level. Will throw exception when using DetectLogLevel.Error</param>
        /// <param name="msg">Log message.</param>
        /// <param name="startNewLine">Start a new line before this message when to UI. True by default.</param>
        /// <param name="style">The style of the message</param>
        public void AddLog(DetectLogLevel level, string msg, bool startNewLine = true, LogStyle style = LogStyle.Default)
        {
            switch (level)
            {
                case DetectLogLevel.Information:
                    ProcessStepStatus(style);
                    DetectorUtil.WriteLog(msg, startNewLine, style);
                    break;
                case DetectLogLevel.Warning:
                    ProcessStepStatus(style);
                    break;
                case DetectLogLevel.Error:
                    AddLog(DetectLogLevel.Information, msg);
                    // PTM will handle the exception and pop up a dialog box to user.
                    throw new Exception(msg);
                default:
                    break;
            }
        }

        public void AddLineToLog(DetectLogLevel level)
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

        public void ApplyDetectContext(DetectContext detectContext)
        {
            this.DetectContext = detectContext;
            StepIndex = 0;
            stepStatus.Clear();
        }

        private void ProcessStepStatus(LogStyle style)
        {
            if (this.DetectContext != null
                        && this.DetectContext.StepStatusChanged != null
                        && (!stepStatus.ContainsKey(StepIndex) || stepStatus[StepIndex] != style))
            {
                this.DetectContext.StepStatusChanged(this.DetectContext.Id, StepIndex, style);
                this.stepStatus[StepIndex] = style;
            }

            if (style != LogStyle.Default)
            {
                StepIndex++;
            }
        }
    }
}
