// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Protocols.TestTools;
using System.Text.RegularExpressions;
using Microsoft.Protocols.TestTools.MessageAnalyzer;
using Microsoft.Protocol.TestSuites.Azod.Adapter.Util;
using System.Globalization;

namespace Microsoft.Protocol.TestSuites.Azod.Adapter
{
    public class MessageAnalyzerAdapter :ManagedAdapterBase, IMessageAnalyzerAdapter
    {
        #region variables

        private ITestSite site;
        // File path for expected messages sequence
        private string expectedSequenceFilePath;
        // Is true if the expectedSequenceFilePath has been changed but not load yet.
        private bool expectedSequenceFilePathChanged = false;
        // File path to save captured messages
        private string capturedMessagesSavePath = null;
        // File path to save selected messages
        private string selectedMessagesSavePath = null;
        // MA Filter
        private string filter = null;
        // Expected message list
        private ExpectedMessageList expectedMessageList = null;        
        // Message Analyzer monitor
        private MessageAnalyzerMonitor monitor = null;
        // Live capture
        private LiveTraceSession liveCapture = null;
        // File capture
        private CaptureFileSession fileCapture = null;
        // Is true if isVerify=true in StartCapture()
        bool verifyInLiveCapture;
        // Role list for all Roles
        private Dictionary<string, EndpointRole> endpointRoles;
        #endregion variables

        #region Properties

        /// <summary>
        /// File path for expected messages sequence
        /// </summary>
        public string ExpectedSequenceFilePath 
        { 
            get
            {
                return expectedSequenceFilePath;
            }
            set
            {
                expectedSequenceFilePathChanged = true;
                expectedSequenceFilePath = value;
            }
        }

        /// <summary>
        /// File path to save captured messages
        /// </summary>
        public string CapturedMessagesSavePath 
        {
            get
            {
                return capturedMessagesSavePath;
            }
            set
            {
                capturedMessagesSavePath = value;
            }
        }

        /// <summary>
        /// File path to save selected messages
        /// </summary>
        public string SelectedMessagesSavePath 
        {
            get
            {
                return selectedMessagesSavePath;
            }
            set
            {
                selectedMessagesSavePath = value;
            }
        }

        /// <summary>
        /// Test Site
        /// </summary>
        public new ITestSite Site
        {
            get
            {
                return site;
            }
        }

        /// <summary>
        /// MA Filter
        /// </summary>
        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Initial the Adapter
        /// </summary>
        /// <param name="testSite"></param>
        public override void Initialize(ITestSite testSite)
        {
            this.site = testSite;
            // Initialize MA environment
            monitor = MessageAnalyzerMonitor.CreateMonitor(null, true);

        }

        /// <summary>
        /// Configure the adapter
        /// </summary>
        /// <param name="endpointRoles">Role Dictionary</param>
        /// <param name="capturedMessagesSavePath">File path to save captured messages</param>
        /// <param name="selectedMessagesSavePath">File path to save selected messages</param>
        /// <param name="expectedSequenceFilePath">File path for expected messages sequence</param>
        public void ConfigureAdapter(Dictionary<string, EndpointRole> endpointRoles, string capturedMessagesSavePath,
            string selectedMessagesSavePath, string expectedSequenceFilePath)
        {
            this.endpointRoles = endpointRoles;
            if (capturedMessagesSavePath != null)
            {
                this.capturedMessagesSavePath = capturedMessagesSavePath;
            }
            else
            {
                // TODO: create a default file path for capturedMessagesSavePath
            }
            if (selectedMessagesSavePath != null)
            {
                this.selectedMessagesSavePath = selectedMessagesSavePath;
            }
            else
            {
                // TODO: create a default file path for selectedMessagesSavePath
            }
            if (expectedSequenceFilePath != null)
            {
                this.expectedSequenceFilePath = expectedSequenceFilePath;
            }
        }

        /// <summary>
        /// Reset the adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            // Reset expected Message List
            expectedMessageList = null;

            // Reset filter
            filter = null;

            // Reset liveCapture and fileCapture
            if (liveCapture != null)
            {
                liveCapture = null;
            }
            if (fileCapture != null)
            {
                fileCapture.Dispose();
                fileCapture = null;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (liveCapture != null)
                {
                    liveCapture = null;
                }
                if (fileCapture != null)
                {
                    fileCapture.Dispose();
                    liveCapture = null;
                }

                monitor.Dispose();
            }
        } 

        /// <summary>
        /// Start a live capture to capture messages
        /// </summary>
        /// <param name="isVerify">If true, verify the messages during capturing; If false, don't verify the messages</param>
        public void StartCapture(bool isVerify = true)
        {
            //Set whether verify the messages
            verifyInLiveCapture = isVerify;
            
            if (isVerify)
            {
                this.site.Log.Add(LogEntryKind.Comment, "MessageAnalyzerAdapter.StartCapture: If isVerify is true, Make sure the expected Sequence has been loaded");
                LoadExpectedSequence();
            }

            //If monitor is not created, create a new monitor.
            if (monitor == null)
            { 
                monitor = MessageAnalyzerMonitor.CreateMonitor(null, true); 
            }

            //If liveCapture is not created, create a new live capture
            if (liveCapture == null)
            {
                // Using default providers if there's no parameter. MMA library will select proper provider according to Windows OS version
                liveCapture = monitor.CreateLiveTraceSession();
            }
             
            liveCapture.Start(capturedMessagesSavePath, filter);
        }

        /// <summary>
        /// Stop the live capture
        /// </summary>
        public void StopCapture()
        {
            if (liveCapture == null)
            {
                this.site.Assert.Fail("MessageAnalyzerAdapter.LoadExpectedSequence: LiveCapture has not been created");
            }
            liveCapture.Stop();
        }
     
        /// <summary>
        /// Parse and verify a capture file
        /// </summary>
        /// <param name="captureFilePath">Path of the capture file</param>
        /// <param name="saveResult">If true, save the captured messages and selected messages. If false, don't save</param>
        public bool ParseAndVerify(string captureFilePath, bool saveResult = false)
        {
            // Make sure the expected Sequence has been loaded
            this.site.Log.Add(LogEntryKind.Comment, "MessageAnalyzerAdapter.StartCapture: Make sure the expected Sequence has been loaded");
            LoadExpectedSequence();

            // If monitor is not created, create a new monitor.
            if (monitor == null)
                monitor = MessageAnalyzerMonitor.CreateMonitor(null, true);

            // Create fileCapture from a capture file
            fileCapture = (CaptureFileSession)monitor.CreateCaptureFileSession(captureFilePath);

            // Add verify process to onNewMessageEvent and start the capture
            fileCapture.Start(filter);

            // Wait until parse completed
            fileCapture.WaitUntilParsingComplete();

            // Stop the capture
            fileCapture.Stop();

            bool isSuccess = VerifyMessageSequence(fileCapture);

            if (saveResult)
            {
                // Save messages if saveResult is true
                if (capturedMessagesSavePath != null)
                {
                    fileCapture.SaveCapturedMessages(capturedMessagesSavePath);
                }
                if (selectedMessagesSavePath != null)
                {
                    fileCapture.SaveCapturedMessages(selectedMessagesSavePath);
                }
            }

            if (isSuccess)
            {
                this.site.Log.Add(LogEntryKind.Comment, "MessageAnalyzerAdapter verify stopped. The capture matches the expected sequences.");
            }
            else
            {
                this.site.Log.Add(LogEntryKind.Comment, "MessageAnalyzerAdapter verify stopped. The capture doesn't match the expected sequences.");
            }
            
            return isSuccess;
            
        }

        /// <summary>
        /// Load the expected message sequence from an expected sequence file
        /// </summary>
        /// <param name="expectedSequenceFilePath">File path for expected messages sequence</param>
        public void LoadExpectedSequence(string expectedSequenceFilePath)
        {
            if (expectedSequenceFilePath == null)
            {
                this.site.Assert.Fail("MessageAnalyzerAdapter.LoadExpectedSequence: expectedSequenceFilePath cannot be null");
            }
            this.expectedSequenceFilePath = expectedSequenceFilePath;
            LoadExpectedSequence();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Load expected message sequences
        /// </summary>
        private void LoadExpectedSequence()
        {
            if (expectedSequenceFilePath == null && expectedMessageList == null)
            {
                this.site.Assert.Fail("MessageAnalyzerAdapter.LoadExpectedSequence: ExpectedSequenceFilePath cannot be null");
            }
            if (expectedSequenceFilePath != null && (expectedMessageList == null || expectedSequenceFilePathChanged))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ExpectedMessageList));
                    using (FileStream fs = new FileStream(expectedSequenceFilePath, FileMode.Open))
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.XmlResolver = null;
                        settings.DtdProcessing = DtdProcessing.Prohibit;
                        using (XmlReader reader = XmlReader.Create(fs, settings))
                        {
                            expectedMessageList = (ExpectedMessageList)serializer.Deserialize(reader);
                        }
                    }
                    if(expectedMessageList.Filter!=null)
                        this.Filter = TranslateFilter(expectedMessageList.Filter);
                    expectedSequenceFilePathChanged = false;

                }
                catch (InvalidOperationException e)
                {
                    this.site.Assert.Fail("MessageAnalyzerAdapter.LoadExpectedSequence: Error in xml: {0}", e.Message);
                }
            }

            this.site.Log.Add(LogEntryKind.Comment, "MessageAnalyzerAdapter.LoadExpectedSequence: Expected Sequence has been loaded successfully. Filter = {0}", Filter);
        }

        /// <summary>
        /// Find the length of the longest common subsequences (LCS) between the expected messages and the captured messages
        /// </summary>
        /// <param name="expectedMessages">the expected message list</param>
        /// <param name="capturedMessages">the captured message list</param>
        /// <param name="match">judge whether all expected messages are found in the real capture</param>
        /// <returns>the value of this 2D array will be used to display the LCS, it indicates the direction when retrieving elements from this table to construct the LCS</returns>
        private int[,] LCSLength(List<ExpectedMessage> expectedMessages, IList<Message> capturedMessages, out bool match)
        {
            int m = expectedMessages.Count;
            int n = capturedMessages.Count;
            // the value of this 2D array c[i, j] indicating the length of the LCS of the X and Y axis, for example, c[i, j] = length of LCS(x[i], y[j])
            int[,] c = new int[m + 1, n + 1];
            int[,] b = new int[m + 1, n + 1];
            int i, j;

            for (i = 0; i <= m; i++)
            {
                c[i, 0] = 0;
            }
            for (j = 0; j <= n; j++)
            {
                c[0, j] = 0;
            }
            for (i = 1; i <= m; i++)
            {
                for (j = 1; j <= n; j++)
                {
                    if (expectedMessages[i - 1].Verify(capturedMessages[j - 1], endpointRoles))
                    {
                        c[i, j] = c[i - 1, j - 1] + 1;
                        b[i, j] = 0;
                    }
                    else if (c[i - 1, j] >= c[i, j - 1])
                    {
                        c[i, j] = c[i - 1, j];
                        b[i, j] = 1;
                    }
                    else
                    {
                        c[i, j] = c[i, j - 1];
                        b[i, j] = -1;
                    }
                }
            }

            if (c[m, n] == m)
            {
                match = true;
            }
            else
            {
                match = false;
            }

            return b;
        }

        /// <summary>
        /// Print out the longest common subsequences (LCS) between the expected messages and the captured messages
        /// </summary>
        /// <param name="b">the value of this 2D array indicates the direction when retrieving elements from this table to construct the LCS</param>
        /// <param name="expectedMessages">the expected message list</param>
        /// <param name="capturedMessages">the captured message list</param>
        /// <param name="i">current element of the expected message list</param>
        /// <param name="j">current element of the captured message list</param>
        /// <param name="capture">the capture file for verification</param>
        private void PrintLCS(int[,] b, List<ExpectedMessage> expectedMessages, IList<Message> capturedMessages, int i, int j, Microsoft.Protocols.TestTools.MessageAnalyzer.Session capture)
        {
            if (i == 0 || j == 0)
            {
                return;
            }
            if (b[i, j] == 0)
            {
                PrintLCS(b, expectedMessages, capturedMessages, i - 1, j - 1, capture);
                site.Log.Add(LogEntryKind.CheckSucceeded,
                    String.Format(CultureInfo.InvariantCulture, "Verify message succeed. Expected message is #{0}: {1};\nCaptured message is #{2}: {3}.",
                    i - 1,
                    expectedMessages[i - 1].Name,
                    capturedMessages[j - 1].MessageNumber,
                    capturedMessages[j - 1].TypeName));
            }
            else if (b[i, j] == 1)
            {
                PrintLCS(b, expectedMessages, capturedMessages, i - 1, j, capture);
                site.Log.Add(LogEntryKind.CheckFailed,
                    String.Format(CultureInfo.InvariantCulture, "Expected message not found. Expected message is #{0}: {1}.",
                    i - 1,
                    expectedMessages[i - 1].Name));
                if (expectedMessages[i - 1].Description != null)
                {
                    site.Log.Add(LogEntryKind.Debug, String.Format(CultureInfo.InvariantCulture, "The missing message is for {0}.",
                        expectedMessages[i - 1].Description));
                }
            }
            else
            {
                PrintLCS(b, expectedMessages, capturedMessages, i, j - 1, capture);
            }
        }

        /// <summary>
        /// Used to verify the captured message according to the expected messages 
        /// given in the examples from open specifications
        /// </summary>
        /// <param name="messageList">A real captured message sequence ordered by MessageNumber</param>
        private bool VerifyMessageSequence(Microsoft.Protocols.TestTools.MessageAnalyzer.Session capture)
        {
            IList<Message> capturedMessages = capture.SortedMessagesWithOperationChildrenList;
            List<ExpectedMessage> expectedMessages = expectedMessageList.ExpectedMessages;
            bool match = true;

            int[,] b = LCSLength(expectedMessages, capturedMessages, out match);
            PrintLCS(b, expectedMessages, capturedMessages, expectedMessages.Count, capturedMessages.Count, capture);
            return match;
        }

        /// <summary>
        /// Tanslate Filter from Messge Sequence file to a MA filter
        /// </summary>
        /// <param name="filterStr"></param>
        /// <returns></returns>
        private string TranslateFilter(string filterStr)
        {
            if (filterStr == null || filterStr.Trim().Length == 0)
                return null;

            // Get all address variables in filterStr
            Regex regex = new Regex(@"\{.*?\}");
            MatchCollection matches = regex.Matches(filterStr);

            foreach (Match item in matches)
            {
                // Get key and address type of the address variable
                string variable = item.Value.Replace("{", "").Replace("}", "").Trim();
                int seperatorIndex = variable.LastIndexOf('.');
                string key = variable.Substring(0, seperatorIndex);
                string addressType = variable.Substring(seperatorIndex+1);

                // Get address value
                string value = null;
                EndpointRole role = endpointRoles[key];
                if(addressType.ToLower().Equals("ipv4"))
                {
                    value = role.Ipv4;
                }
                else if(addressType.ToLower().Equals("ipv6"))
                {
                    value = role.Ipv6;
                }
                else if (addressType.ToLower().Equals("mac"))
                {
                    value = role.MAC;
                }

                // Replace the value;
                filterStr = filterStr.Replace(item.Value, value);
            }
            return filterStr;

        }
        #endregion Private Methods
    }
}
