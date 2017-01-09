// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{ 
    /// <summary>
    /// A class dealing with checks conditions accumulating diagnostics.
    /// </summary>
    public static class Checks
    {
        static string diagnostics = String.Empty;
        static string strLogPurpose = String.Empty;

        /// <summary>
        /// A property indicating whether there are diagnostics.
        /// </summary>
        public static bool HasDiagnostics { get { return diagnostics.Length > 0; } }

        /// <summary>
        /// A property indicating whether there are log entry.
        /// </summary>
        public static bool HasLogEntry { get { return strLogPurpose.Length > 0; } }

        /// <summary>
        /// Call this method to get and clear accumlated diagnostics.
        /// </summary>
        /// <returns></returns>
        public static string GetAndClearDiagnostics()
        {
            string result = diagnostics;
            diagnostics = String.Empty;

            return result;
        }

        /// <summary>
        /// Get and clear log.
        /// </summary>
        /// <returns></returns>
        public static string GetAndClearLog()
        {
            string result = strLogPurpose;
            strLogPurpose = String.Empty;

            return result;
        }
        
        /// <summary>
        /// Check whether condition is true, if not, remember diagnostics.
        /// </summary>
        /// <param name="cond">Condition will be checked.</param>
        /// <param name="message">Message will be presented in log.</param>
        /// <param name="parameters">Objects parameters.</param>
        public static void IsTrue(bool cond, string message, params object[] parameters)
        {
            if (!cond)
            {
                diagnostics += "\r\nERROR: " + String.Format(message, parameters);
            }
        }

        /// <summary>
        /// Indicate a check as failed.
        /// </summary>
        /// <param name="message">Message will be presented in log.</param>
        /// <param name="parameters">Objects parameters.</param>
        public static void Fail(string message, params object[] parameters)
        {
            IsTrue(false, String.Format(message, parameters));
        }

        /// <summary>
        /// Make a log and indicate the log purpose.
        /// </summary>
        /// <param name="message">Message will be presented in log.</param>
        /// <param name="parameters">Objects parameters.</param>
        public static void MakeLog(string message, params object[] parameters)
        {
            strLogPurpose += "\r\n DataSchema Log : " + String.Format(message, parameters);
        }
                     
    }

    /// <summary>
    /// A class dealing with assertion failures, which are considered error in modeling/programming.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Check whether condition is true and fail if not.
        /// </summary>
        /// <param name="cond">Condition will be checked.</param>
        /// <param name="message">Message will be presented in log.</param>
        /// <param name="parameters">Objects parameters.</param>
        public static void IsTrue(bool cond, string message, params object[] parameters)
        {
            Modeling.Condition.IsTrue(cond, "\r\nERROR: " + String.Format(message, parameters));
        }

        /// <summary>
        /// Make log fail.
        /// </summary>
        /// <param name="message">Message will be presented in log.</param>
        /// <param name="parameters">Objects parameters.</param>
        public static void Fail(string message, params object[] parameters)
        {
            IsTrue(false, message, parameters);
        }
    }
}
