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
using System.Globalization;
using Microsoft.Protocols.TestTools.MessageAnalyzer;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.MAAdapter
{
    public class MessageAnalyzerAdapter :ManagedAdapterBase, IMessageAnalyzerAdapter
    {
        #region variables

        private ITestSite site;      
        // Message Analyzer monitor
        private MessageAnalyzerMonitor monitor = null;
        // Live capture
        private LiveTraceSession liveCapture = null;
        // File capture
        private CaptureFileSession fileCapture = null;

        #endregion variables

        #region Properties

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
        #endregion

        #region Public Methods

        /// <summary>
        /// Initial the Adapter
        /// </summary>
        /// <param name="testSite">Test Site</param>
        public override void Initialize(ITestSite testSite)
        {
            this.site = testSite;
            // Initialize MA environment
            monitor = MessageAnalyzerMonitor.CreateMonitor(null, true);
        }

        /// <summary>
        /// Reset the adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();

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
                if (monitor != null)
                {
                    monitor.Dispose();
                    monitor = null;
                }
            }
        }

        /// <summary>
        /// Get Messages with filter applied to the capture
        /// </summary>
        /// <param name="capturePath">The capture file path</param>
        /// <param name="frameFilter">The filter applied to the capture</param>
        /// <returns>Return the filtered messages</returns>
        public List<Message> GetMessages(string capturePath, string frameFilter)
        {
            // If monitor is not created, create a new monitor.
            if (monitor == null)
                monitor = MessageAnalyzerMonitor.CreateMonitor(null, true);

            // Create fileCapture from a capture file
            fileCapture = (CaptureFileSession)monitor.CreateCaptureFileSession(capturePath);

            // Add verify process to onNewMessageEvent and start the capture
            fileCapture.Start(frameFilter);

            // Wait until parse completed
            fileCapture.WaitUntilParsingComplete();

            // Stop the capture
            fileCapture.Stop();

            return fileCapture.SortedMessagesWithOperationChildrenList.ToList<Message>();
        }

        /// <summary>
        /// Start a live capture to capture messages
        /// </summary>
        /// <param name="capturePath">The capture file path</param>
        /// <param name="filter">The filter applied to the capture</param>
        public void StartCapture(string capturePath, string filter = null)
        {
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

            liveCapture.Start(capturePath, filter);
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
        #endregion Public Methods
    }
}
