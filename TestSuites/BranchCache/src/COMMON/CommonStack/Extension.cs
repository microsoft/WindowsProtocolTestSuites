// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// The Event Queue
    /// </summary>
    public class EventQueue : Microsoft.Protocols.TestTools.StackSdk.Messages.EventQueue
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site">The test site</param>
        /// <param name="maxSize">maximum size of the queue</param>
        public EventQueue(ITestSite site, int maxSize)
            : base(ExtensionHelper.GetRuntimeHost(site), maxSize)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site">The test site</param>
        public EventQueue(ITestSite site) :
            base(ExtensionHelper.GetRuntimeHost(site))
        { }
    }

    /// <summary>
    /// Provides a series of helper methods in testtools extension
    /// </summary>
    public static class ExtensionHelper
    {
        /// <summary>
        /// Binding PTF logger checker to Message Runtime Host.
        /// </summary>
        static void RegisterRuntimeHost(ITestSite site, IRuntimeHost host)
        {
            host.AssertChecker += new EventHandler<MessageLogEventArgs>(
                delegate (object sender, MessageLogEventArgs e)
                {
                    if (e == null)
                    {
                        throw new ArgumentNullException("MessageLogEventArgs");
                    }

                    if (e.Condition)
                    {
                        site.Assert.Pass(e.Message, e.Parameters);
                    }
                    else
                    {
                        site.Assert.Fail(e.Message, e.Parameters);
                    }
                });

            host.AssumeChecker += new EventHandler<MessageLogEventArgs>(
                delegate (object sender, MessageLogEventArgs e)
                {
                    if (e == null)
                    {
                        throw new ArgumentNullException("MessageLogEventArgs");
                    }

                    if (e.Condition)
                    {
                        site.Assume.Pass(e.Message, e.Parameters);
                    }
                    else
                    {
                        site.Assume.Fail(e.Message, e.Parameters);
                    }
                });

            host.DebugChecker += new EventHandler<MessageLogEventArgs>(
                delegate (object sender, MessageLogEventArgs e)
                {
                    if (e == null)
                    {
                        throw new ArgumentNullException("MessageLogEventArgs");
                    }

                    if (e.Condition)
                    {
                        site.Debug.Pass(e.Message, e.Parameters);
                    }
                    else
                    {
                        site.Debug.Fail(e.Message, e.Parameters);
                    }
                });

            host.MessageLogger += new EventHandler<MessageLogEventArgs>(
                delegate (object sender, MessageLogEventArgs e)
                {
                    if (e == null)
                    {
                        throw new ArgumentNullException("MessageLogEventArgs");
                    }

                    site.Log.Add(
                        LogKindToLogEntryKind(e.LogEntryKind),
                        e.Message, e.Parameters);
                });

            host.RequirementLogger += new EventHandler<RequirementCaptureEventArgs>(
                delegate (object sender, RequirementCaptureEventArgs e)
                {
                    if (e == null)
                    {
                        throw new ArgumentNullException("RequirementCaptureEventArgs");
                    }

                    site.CaptureRequirement(
                        e.ProtocolName, e.RequirementId, e.RequirementDescription);
                });
        }

        /// <summary>
        /// Runtime log kind to PTF log entry.
        /// </summary>
        static LogEntryKind LogKindToLogEntryKind(LogKind kind)
        {
            LogEntryKind entryKind;
            switch (kind)
            {
                case LogKind.CheckSucceeded:
                    entryKind = LogEntryKind.CheckSucceeded;
                    break;
                case LogKind.CheckFailed:
                    entryKind = LogEntryKind.CheckFailed;
                    break;
                case LogKind.Checkpoint:
                    entryKind = LogEntryKind.Checkpoint;
                    break;
                case LogKind.Comment:
                    entryKind = LogEntryKind.Comment;
                    break;
                case LogKind.Debug:
                    entryKind = LogEntryKind.Debug;
                    break;
                case LogKind.Warning:
                    entryKind = LogEntryKind.Warning;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected LogKind.");
            }
            return entryKind;
        }

        /// <summary>
        /// Gets the runtime host.
        /// </summary>
        /// <param name="site">The test site</param>
        /// <returns>Returns the instance of the runtime host</returns>
        public static IRuntimeHost GetRuntimeHost(ITestSite site)
        {
            if (site == null)
            {
                return null;
            }

            bool marshallerTrace = site != null ? site.Properties["Marshaler.trace"] == "true" : false;
            bool disablevalidation = site != null ? site.Properties["disablevalidation"] == "true" : false;
            RuntimeHostProvider.Initialize(marshallerTrace, disablevalidation);
            IRuntimeHost host = RuntimeHostProvider.RuntimeHost;
            //only register once
            if (string.IsNullOrEmpty(site.Properties["IsRuntimeHostRegistered"]))
            {
                RegisterRuntimeHost(site, host);
                site.Properties["IsRuntimeHostRegistered"] = "true";
            }
            return host;
        }
    }
}