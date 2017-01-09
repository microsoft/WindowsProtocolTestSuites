// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Text;

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.PublishDc
{
    /// <summary>
    /// Class Containing Helper Methods
    /// </summary>
    public static class HelperClass
    {
        #region DnsBasedDiscoveryHelperMethods

        #region LocalVariables

        static string output = string.Empty;
        static Stream stream;
        static StreamWriter writer;

        #endregion

        /// <summary>
        /// Extracts the required lines from specified file based on the keyword
        /// </summary>
        /// <param name="fileName">Name of the File</param>
        /// <param name="keyWord">Keyword to be presented to extract the required lines</param>
        /// <returns>List of Strings </returns>
        public static ICollection<string> ReadLine(string fileName, string keyWord)
        {
            string line;
            ICollection<string> matchedLines = new List<string>();

            using (StreamReader file = new StreamReader(fileName))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.ToLower().Contains(keyWord.ToLower()))
                    {
                        matchedLines.Add(line);
                    }
                }
                return matchedLines;
            }
        }


        /// <summary>
        /// Executes a nslookup command
        /// </summary>
        /// <param name="parameter">Parameters to be passed to nslookup</param>
        /// <returns>Returns the result of command execution</returns>
        public static string ExecuteCommand(string parameter)
        {
            Process p = new Process();
            p.StartInfo.FileName = "nslookup.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = parameter;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            // Nslookup.exe needs about 10000ms to display response.
            System.Threading.Thread.Sleep(10000);
            output = p.StandardOutput.ReadToEnd();

            if (!string.IsNullOrEmpty(output))
                return output;
            else
                return "Invalid Parameter";
        }


        /// <summary>
        /// Redirects the specified data to a text file
        /// </summary>
        /// <param name="output">Data to be written</param>
        /// <param name="fileName">Name of the File</param>
        /// <returns>True if successful else false</returns>
        public static bool RedirectOutputToTextFile(string output, string fileName)
        {
            try
            {
                stream = File.Create(@"..\..\..\" + fileName);

                writer = new StreamWriter(stream);
                writer.Write(output);
                writer.Close();
            }
            catch (System.IO.IOException)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Appends the specified data to a text file
        /// </summary>
        /// <param name="output">Data to be written</param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>True if successful else false</returns>
        public static bool AppendOutputToTextFile(string output, string fileName)
        {
            try
            {
                writer = File.AppendText(String.Concat(@"..\..\..\" + fileName));
                writer.WriteLine(output);
                writer.Close();
            }
            catch (System.IO.IOException)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
