// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    internal static class RuntimeAppLog
    {
        //Output file stream name
        private const string logFileName = "MessageRuntimeLog.txt";

        //Output file stream
        private static TextWriterTraceListener textListener; 

        //Log message format
        private const string timeStampFormat = "{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}";
            
        /// <summary>
        /// static Constructor for ApplicationLog.
        /// </summary>  
        /// Failure on application log should not prevent the PTF from executing
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        static RuntimeAppLog()
        {
            try
            {
                //Initialize trace listener
                FileStream logFileStream = new FileStream(logFileName,
                    FileMode.Append, 
                    FileAccess.Write, 
                    FileShare.ReadWrite);

                if (logFileStream != null)
                {
                    textListener = new TextWriterTraceListener(logFileStream);
                    Trace.Listeners.Clear();
                    Trace.Listeners.Add(textListener);
                }
            }
            catch(Exception)
            {
                //We shouldn't catch general exception, but failure on application 
                //log should not prevent the PTF from executing.
            }
            
        }

        /// <summary>
        /// Write a message to Trace listeners
        /// </summary>
        /// <param name="message">The message to write to the log file</param>
        /// Failure on application log should not prevent the PTF from executing
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        internal static void TraceLog(string message)
        {
            //format the message
            DateTime timeStamp = DateTime.Now;
            string timeStampInfo = string.Format(CultureInfo.InvariantCulture, 
                timeStampFormat,
                timeStamp.Year,
                timeStamp.Month,
                timeStamp.Day,
                timeStamp.Hour,
                timeStamp.Minute,
                timeStamp.Second,
                timeStamp.Millisecond);

            string logMessage = string.Format(CultureInfo.InvariantCulture, 
                "[MessageRuntime {0}] {1}", 
                timeStampInfo, 
                message);
    
            //Write the message into trace listeners
            try
            {
                Trace.WriteLine(logMessage);

                if (textListener != null)
                {
                    textListener.Flush();
                }
            }
            catch (Exception)
            {
                //We shouldn't catch general exception, but application on internal 
                //log should not prevent the PTF from executing.
            }            
        }
    }
}
