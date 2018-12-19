// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Microsoft.Protocols.TestManager.Detector
{
    /// <summary>
    /// This class defines utility for detector.
    /// </summary>
    public static class DetectorUtil
    {
        /// <summary>
        /// Write a log message.
        /// </summary>
        /// <param name="message">Log message.</param>
        /// <param name="startNewLine">Start a new line before this message. True by default.</param>
        /// <param name="style">The style of the message</param>
        public static void WriteLog(string message, bool startNewLine = true, LogStyle style = LogStyle.Default)
        {
            if (UtilCallBackFunctions.WriteLog != null)
                UtilCallBackFunctions.WriteLog(message, startNewLine, style);
        }

        /// <summary>
        /// Get the property value of current prfconfig file.
        /// </summary>
        /// <param name="name">Property name,</param>
        /// <returns></returns>
        public static string GetPropertyValue(string name)
        {
            if (UtilCallBackFunctions.GetPropertyValue != null)
                return UtilCallBackFunctions.GetPropertyValue(name);
            return null;
        }

        /// <summary>
        /// Get properties defined in the specific file.
        /// </summary>
        /// <param name="file">ptfconfig file name.</param>
        /// <returns></returns>
        public static List<string> GetPropertiesByFile(string file)
        {
            if (UtilCallBackFunctions.GetPropertiesByFile != null)
                return UtilCallBackFunctions.GetPropertiesByFile(file);
            return new List<string>();
        }
    }

    /// <summary>
    /// A static class including callback functions.
    /// </summary>
    public static class UtilCallBackFunctions
    {
        /// <summary>
        /// Writes log.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="startNewLine">Indicates start a new line or not</param>
        /// <param name="style">Log style</param>
        public delegate void WriteLogDelegate(string message, bool startNewLine, LogStyle style);

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns></returns>
        public delegate string GetPropertyValueDelegate(string name);

        /// <summary>
        /// Gets a list of properties.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public delegate List<string> GetPropertiesByFileDelegate(string file);

        /// <summary>
        /// Sends log content.
        /// </summary>
        /// <param name="detectingItemsList"></param>
        public delegate void SendLogContentDelegate(List<DetectingItem> detectingItemsList);

        /// <summary>
        ///  Instance of WriteLogDelegate
        /// </summary>
        public static WriteLogDelegate WriteLog;

        /// <summary>
        /// Instance of GetPropertyValueDelegate
        /// </summary>
        public static GetPropertyValueDelegate GetPropertyValue;

        /// <summary>
        /// Instance of GetPropertiesByFileDelegate
        /// </summary>
        public static GetPropertiesByFileDelegate GetPropertiesByFile;

        /// <summary>
        /// Instance of SendLogContentDelegate
        /// </summary>
        public static SendLogContentDelegate SendLogContent;
    }

    /// <summary>
    /// Content and status of detecting item.
    /// </summary>
    public class DetectingItem : INotifyPropertyChanged
    {
        /// <summary>
        /// The content of detecting item.
        /// </summary>
        public string DetectingContent { set; get; }

        private DetectingStatus _detectingStatus;

        /// <summary>
        /// The status of detecting item.
        /// </summary>
        public DetectingStatus DetectingStatus 
        {
            set
            {
                _detectingStatus = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("DetectingStatus"));
            }
            get
            {
                return _detectingStatus;
            }
        }

        /// <summary>
        /// The log style: Default, StepPassed, StepFailed, StepSkipped, StepNotFound or Error.
        /// </summary>
        public LogStyle Style { set; get; }

        public DetectingItem(string content, DetectingStatus status, LogStyle style)
        {
            this.DetectingContent = content;
            this.DetectingStatus = status;
            this.Style = style;
        }

        /// <summary>
        /// Changes property event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Style of the log message.
    /// </summary>
    public enum LogStyle
    {
        /// <summary>
        /// Default Style
        /// </summary>
        Default,
        /// <summary>
        /// The message means a step in the auto-dection passed.
        /// </summary>
        StepPassed,
        /// <summary>
        /// The message means a step in the auto-dection failed. The detection continues.
        /// </summary>
        StepFailed,
        /// <summary>
        /// The item to detect is nout found.
        /// </summary>
        StepSkipped,
        /// <summary>
        /// The item to detect is nout found.
        /// </summary>
        StepNotFound,
        /// <summary>
        /// The message means an error occurs in the detection. The detection cannot proceed.
        /// </summary>
        Error
    }

    /// <summary>
    /// Enumerates the status of detecting.
    /// </summary>
    public enum DetectingStatus
    {
        /// <summary>
        /// Default Status
        /// </summary>
        Pending,
        /// <summary>
        /// Detecting Status
        /// </summary>
        Detecting,
        /// <summary>
        /// The status means a step in the auto-dection passed.
        /// </summary>
        Finished,
        /// <summary>
        /// The detecting object is not found.
        /// </summary>
        Skipped,
        /// <summary>
        /// The detecting object is not found.
        /// </summary>
        NotFound,
        /// <summary>
        /// The status means a step in the auto-dection failed. The detection continues.
        /// </summary>
        Failed,
        /// <summary>
        /// The status means an error occurs in the detection. The detection cannot proceed.
        /// </summary>
        Error,
        /// <summary>
        /// The status means user cancelled the detection.
        /// </summary>
        Canceling
    }
}
