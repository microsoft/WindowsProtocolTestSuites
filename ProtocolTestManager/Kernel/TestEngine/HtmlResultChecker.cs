// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// It's used to check the html results by analyzing all the files under the html result folder
    /// </summary>
    public class HtmlResultChecker
    {
        private HtmlResultChecker()
        {
        }

        private static HtmlResultChecker htmlResultChecker = null;
        /// <summary>
        /// The directory where to execute vstest.
        /// </summary>
        private string workingDir;

        /// <summary>
        /// Notify the file system changes.
        /// </summary>
        private FileSystemWatcher watcher;

        /// <summary>
        /// Path of index.html
        /// </summary>
        private string indexHtmlFilePath;

        /// <summary>
        /// The path of index.html
        /// </summary>
        public string IndexHtmlFilePath
        {
            get
            {
                return indexHtmlFilePath;
            }
        }

        /// <summary>
        /// Gets a singleton instance of HtmlResultChecker.
        /// </summary>
        /// <returns>An instance of HtmlResultChecker.</returns>
        public static HtmlResultChecker GetHtmlResultChecker()
        {
            if (htmlResultChecker == null) htmlResultChecker = new HtmlResultChecker();
            return htmlResultChecker;
        }

        /// <summary>
        /// Delegate to be passed to Logger, then status and log of the case could be shown on UI.
        /// </summary>
        public delegate void UpdateCaseDelegate(TestCaseStatus status, string testCaseName, string log);
        public UpdateCaseDelegate UpdateCase;

        /// <summary>
        /// Registers the callback function. Wait to be notified when there're file changes.
        /// </summary>
        public void Start(string workingDirectory)
        {
            if (workingDir == workingDirectory && watcher != null) return;

            Stop();
            workingDir = workingDirectory;
            watcher = new FileSystemWatcher(workingDir);
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.html"; // Only cares about html files

            // Register a callback function, then when there're changes in the workingDir, the function will be called.
            // Do not care the create event, only care the change event. The reason is that the log files will be written after created.
            watcher.Changed += new FileSystemEventHandler(OnHtmlFileChanged);
            watcher.EnableRaisingEvents = true; // Enable the event notification.

        }

        private void OnHtmlFileChanged(object sender, FileSystemEventArgs e)
        {
            // The html files are under the below folder, so any file that is not in that folder is not interesting.
            // E.g. C:\MicrosoftProtocolTests\FileSharing\Server-Endpoint\1.0.5812.0\HtmlTestResults\2014-11-09-21-59-16\Html\
            if (!e.FullPath.StartsWith(Path.Combine(workingDir, AppConfig.HtmlResultFolderName)))
            {
                return;
            }

            if (e.FullPath.Contains(AppConfig.IndexHtmlFileName))
            {
                this.indexHtmlFilePath = e.FullPath;
                return;
            }

            if (!e.FullPath.Contains(AppConfig.HtmlLogFileFolder))
            {
                return;
            }

            string caseName = Path.GetFileNameWithoutExtension(e.FullPath);

            TestCaseStatus status;
            if (!ParseFileGetStatus(e.FullPath, out status))
            {
                // The file name format is not correct, ignore it.
                return;
            }

            // Pass case status/name/log path/ to logger to update UI.
            UpdateCase(status, caseName, e.FullPath);
        }

        /// <summary>
        /// Disables notifying the file system changes.
        /// </summary>
        public void Stop()
        {
            if (watcher != null)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            watcher = null;
        }

        // Parse the file content to get the case status
        // Result format in file: "Result":"Result: Passed"
        private bool ParseFileGetStatus(string filePath, out TestCaseStatus status)
        {
            status = TestCaseStatus.NotRun;

            string content = File.ReadAllText(filePath);
            int startIndex = content.IndexOf(AppConfig.ResultKeyword);
            startIndex += AppConfig.ResultKeyword.Length;
            int endIndex = content.IndexOf("\"", startIndex);
            string statusStr = content.Substring(startIndex, endIndex - startIndex);
            switch (statusStr)
            {
                case AppConfig.HtmlLogStatusPassed:
                    status = TestCaseStatus.Passed;
                    break;
                case AppConfig.HtmlLogStatusFailed:
                    status = TestCaseStatus.Failed;
                    break;
                case AppConfig.HtmlLogStatusInconclusive:
                    status = TestCaseStatus.Other;
                    break;
                default:
                    // The file name format is not correct
                    return false;
            }

            return true;
        }
    }
}
