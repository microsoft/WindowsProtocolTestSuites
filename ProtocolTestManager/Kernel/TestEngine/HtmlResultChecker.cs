// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Concurrent;
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
        /// Record whether the log file is accessed the first time or not.
        /// </summary>
        private ConcurrentDictionary<string, bool> isFirstTimeAccess;

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
        public delegate void UpdateCaseDelegate(TestCaseStatus status, string testCaseName, TestCaseDetail detail, string log);
        public UpdateCaseDelegate UpdateCase;

        /// <summary>
        /// Registers the callback function. Wait to be notified when there're file changes.
        /// </summary>
        public void Start(string workingDirectory)
        {
            if (workingDir == workingDirectory && watcher != null) return;

            Stop();
            isFirstTimeAccess = new ConcurrentDictionary<string, bool>();
            workingDir = workingDirectory;
            watcher = new FileSystemWatcher(workingDir);
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.html"; // Only cares about html files

            // Register a callback function, then when there're changes in the workingDir, the function will be called.
            watcher.Created += new FileSystemEventHandler(OnHtmlFileCreated);
            watcher.Changed += new FileSystemEventHandler(OnHtmlFileChanged);
            watcher.EnableRaisingEvents = true; // Enable the event notification.

        }

        private void OnHtmlFileCreated(object sender, FileSystemEventArgs e)
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

            // Run the case for the first time.
            // It will trigger a file create event with a file change event.
            // We are interested in the second event where the log has been saved to file.
            isFirstTimeAccess[caseName] = true;
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

            if (!isFirstTimeAccess.ContainsKey(caseName) || !isFirstTimeAccess[caseName])
            {
                // This is the situation where the case has ran before.
                // Rerun the case will trigger two file change events.
                // We are interested in the second event where the log has been saved to file.
                isFirstTimeAccess[caseName] = true;
                return;
            }

            TestCaseStatus status;
            TestCaseDetail caseDetail;
            if (!Utility.ParseFileGetStatus(e.FullPath, out status, out caseDetail))
            {
                // The file name format is not correct, ignore it.
                return;
            }

            // Pass case status/name/log path/ to logger to update UI.
            UpdateCase(status, caseName, caseDetail, e.FullPath);
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

            if (isFirstTimeAccess != null)
            {
                isFirstTimeAccess.Clear();
            }
            isFirstTimeAccess = null;
        }
    }

    /// <summary>
    /// Represents a detailed StandardOut log
    /// </summary>
    public class StandardOutDetail
    {
        /// <summary>
        /// The type of the StandardOut log
        /// </summary>
        public string Type;

        /// <summary>
        /// The content of the StandardOut log
        /// </summary>
        public string Content;
    }

    /// <summary>
    /// Represents detailed test case information
    /// </summary>
    public class TestCaseDetail
    {
        /// <summary>
        /// The name of the test case
        /// </summary>
        public string Name;

        /// <summary>
        /// The start time of the test case
        /// </summary>
        public DateTimeOffset StartTime;

        /// <summary>
        /// The end time of the test case
        /// </summary>
        public DateTimeOffset EndTime;

        /// <summary>
        /// The result of the test case
        /// </summary>
        public string Result;

        /// <summary>
        /// The source assembly of the test case
        /// </summary>
        public string Source;

        /// <summary>
        /// The ErrorStackTrace log of the test case
        /// </summary>
        public List<string> ErrorStackTrace;

        /// <summary>
        /// The ErrorMessage log of the test case
        /// </summary>
        public List<string> ErrorMessage;

        /// <summary>
        /// The StandardOut log of the test case
        /// </summary>
        public List<StandardOutDetail> StandardOut;

        /// <summary>
        /// The Types in StandardOut log 
        /// </summary>
        public List<string> StandardOutTypes;

        /// <summary>
        /// The path of the capture file if any
        /// </summary>
        public string CapturePath;
    }
}
