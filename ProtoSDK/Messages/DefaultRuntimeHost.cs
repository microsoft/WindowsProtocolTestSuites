// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

using Microsoft.Protocols.TestTools.StackSdk.Messages.Runtime;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// The runtime host provider
    /// </summary>
    public static class RuntimeHostProvider
    {
        private static DefaultRuntimeHost host;

        /// <summary>
        /// Initialize message runtime host.
        /// </summary>
        /// <param name="marshallerTrace">If marshaller tracing is enabled</param>
        /// <param name="disablevalidation">If validation is disabled</param>
        public static void Initialize(bool marshallerTrace, bool disablevalidation)
        {
            if (host == null)
            {
                host = new DefaultRuntimeHost(marshallerTrace, disablevalidation);
                host.AssertChecker += new EventHandler<MessageLogEventArgs>(HostAssertChecker);
                host.AssumeChecker += new EventHandler<MessageLogEventArgs>(HostAssumeChecker);
                host.DebugChecker += new EventHandler<MessageLogEventArgs>(HostDebugChecker);
                host.MessageLogger += new EventHandler<MessageLogEventArgs>(HostMessageLogger);
                host.RequirementLogger += new EventHandler<RequirementCaptureEventArgs>(HostRequirementLogger);
            }
            else
            {
                if (host.marshallerTrace != marshallerTrace)
                {
                    host.marshallerTrace = marshallerTrace;
                }

                if (host.disablevalidation != disablevalidation)
                {
                    host.disablevalidation = disablevalidation;
                }
            }
        }

        /// <summary>
        /// Cleanup message runtime host.
        /// </summary>
        public static void Cleanup()
        {
            if (host != null)
            {
                host = null;
            }
        }

        /// <summary>
        /// Gets runtime host.
        /// </summary>
        public static IRuntimeHost RuntimeHost
        {
            get
            {
                if (host == null)
                {
                    throw new InvalidOperationException(
                        "RuntimeHostProvider is not inistilized," +
                        " please call RuntimeHostProvider.Initialize to initialize.");
                }

                return host;
            }
        }

        #region Private methods

        private static void HostRequirementLogger(object sender, RequirementCaptureEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("RequirementCaptureEventArgs");
            }
            string reqId = string.Format(CultureInfo.InvariantCulture, MessageRuntime.ReqId, e.ProtocolName, e.RequirementId);
            RuntimeAppLog.TraceLog(
                string.Format(CultureInfo.InvariantCulture, MessageRuntime.ReqCaptureFormat, reqId, e.RequirementDescription));
        }

        private static void HostMessageLogger(object sender, MessageLogEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("MessageLogEventArgs");
            }
            string message = e.Parameters == null ? e.Message : string.Format(CultureInfo.InvariantCulture, e.Message, e.Parameters);
            RuntimeAppLog.TraceLog(
                string.Format(CultureInfo.InvariantCulture, MessageRuntime.MessageLogFormat, e.LogEntryKind, message));
        }

        private static void HostDebugChecker(object sender, MessageLogEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("MessageLogEventArgs");
            }

            string message = e.Parameters == null ? e.Message : string.Format(CultureInfo.InvariantCulture, e.Message, e.Parameters);
            if (e.Condition)
            {
                RuntimeAppLog.TraceLog(
                    string.Format(CultureInfo.InvariantCulture, MessageRuntime.DebugCheckSucceed, message));
            }
            else
            {
                RuntimeAppLog.TraceLog(
                    string.Format(CultureInfo.InvariantCulture, MessageRuntime.DebugCheckFail, message));
            }
        }

        private static void HostAssumeChecker(object sender, MessageLogEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("MessageLogEventArgs");
            }
            string message = e.Parameters == null ? e.Message : string.Format(CultureInfo.InvariantCulture, e.Message, e.Parameters);
            if (e.Condition)
            {
                RuntimeAppLog.TraceLog(
                    string.Format(CultureInfo.InvariantCulture, MessageRuntime.AssumeCheckSucceed, message));
            }
            else
            {
                RuntimeAppLog.TraceLog(
                    string.Format(CultureInfo.InvariantCulture, MessageRuntime.AssumeCheckFail, message));
            }
        }

        private static void HostAssertChecker(object sender, MessageLogEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            string message = e.Parameters == null ? e.Message : string.Format(CultureInfo.InvariantCulture, e.Message, e.Parameters);
            if (e.Condition)
            {
                RuntimeAppLog.TraceLog(
                    string.Format(CultureInfo.InvariantCulture, MessageRuntime.AssertCheckSucceed, message));
            }
            else
            {
                RuntimeAppLog.TraceLog(
                    string.Format(CultureInfo.InvariantCulture, MessageRuntime.AssertCheckFail, message));
            }
        }
        #endregion
    }

    internal class DefaultRuntimeHost : IRuntimeHost
    {
        private Dictionary<string, EventHandler<MessageLogEventArgs>> eventCheckerTable
            = new Dictionary<string, EventHandler<MessageLogEventArgs>>();

        private Dictionary<string, EventHandler<RequirementCaptureEventArgs>> eventReqTable
            = new Dictionary<string, EventHandler<RequirementCaptureEventArgs>>();

        internal bool marshallerTrace;
        internal bool disablevalidation;

        /// <summary>
        /// Disable default constructor
        /// </summary>
        private DefaultRuntimeHost() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="marshallerTrace">is marshaller tracing enabled</param>
        /// <param name="disablevalidation">is disable validation</param>
        public DefaultRuntimeHost(bool marshallerTrace, bool disablevalidation)
        {
            Inistialize();
            this.marshallerTrace = marshallerTrace;
            this.disablevalidation = disablevalidation;
        }

        private void Inistialize()
        {
            eventCheckerTable.Add("AssertChecker", null);
            eventCheckerTable.Add("AssumeChecker", null);
            eventCheckerTable.Add("DebugChecker", null);
            eventCheckerTable.Add("MessageLogger", null);
            eventReqTable.Add("ReqCaptureLogger", null);
        }

        #region Properties

        /// <summary>
        /// Implement <see cref="IRuntimeHost.MarshallerTrace"/>
        /// </summary>
        public bool MarshallerTrace
        {
            get
            {
                return marshallerTrace;
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.DisableValidation"/>
        /// </summary>
        public bool DisableValidation
        {
            get
            {
                return disablevalidation;
            }
        }

        #endregion

        #region IRuntimeHost Members

        /// <summary>
        /// Implement <see cref="IRuntimeHost.AssertChecker"/>
        /// </summary>
        public event EventHandler<MessageLogEventArgs> AssertChecker
        {
            add
            {
                eventCheckerTable["AssertChecker"] += value;
            }
            remove
            {
                eventCheckerTable["AssertChecker"] -= value;
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.AssumeChecker"/>
        /// </summary>
        public event EventHandler<MessageLogEventArgs> AssumeChecker
        {
            add
            {
                eventCheckerTable["AssumeChecker"] += value;
            }
            remove
            {
                eventCheckerTable["AssumeChecker"] -= value;
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.DebugChecker"/>
        /// </summary>
        public event EventHandler<MessageLogEventArgs> DebugChecker
        {
            add
            {
                eventCheckerTable["DebugChecker"] += value;
            }
            remove
            {
                eventCheckerTable["DebugChecker"] -= value;
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.MessageLogger"/>
        /// </summary>
        public event EventHandler<MessageLogEventArgs> MessageLogger
        {
            add
            {
                eventCheckerTable["MessageLogger"] += value;
            }
            remove
            {
                eventCheckerTable["MessageLogger"] -= value;
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.RequirementLogger"/>
        /// </summary>
        public event EventHandler<RequirementCaptureEventArgs> RequirementLogger
        {
            add
            {
                eventReqTable["ReqCaptureLogger"] += value;
            }
            remove
            {
                eventReqTable["ReqCaptureLogger"] -= value;
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.Assert"/>
        /// </summary>
        public void Assert(bool condition, string message, params object[] parameters)
        {
            EventHandler<MessageLogEventArgs> handler = eventCheckerTable["AssertChecker"];
            if (handler != null)
            {
                handler(this, new MessageLogEventArgs(LogKind.UnKnown, condition, message, parameters));
            }
            else
            {
                throw new InvalidOperationException(
                    "Runtime host is not initialized, please call RuntimeHostProvider.Initialize before using it.");
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.Assume"/>
        /// </summary>
        public void Assume(bool condition, string message, params object[] parameters)
        {
            EventHandler<MessageLogEventArgs> handler = eventCheckerTable["AssumeChecker"];
            if (handler != null)
            {
                handler(this, new MessageLogEventArgs(LogKind.UnKnown, condition, message, parameters));
            }
            else
            {
                throw new InvalidOperationException(
                    "Runtime host is not initialized, please call RuntimeHostProvider.Initialize before using it.");
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.Debug"/>
        /// </summary>
        public void Debug(bool condition, string message, params object[] parameters)
        {
            EventHandler<MessageLogEventArgs> handler = eventCheckerTable["DebugChecker"];
            if (handler != null)
            {
                handler(this, new MessageLogEventArgs(LogKind.UnKnown, condition, message, parameters));
            }
            else
            {
                throw new InvalidOperationException(
                    "Runtime host is not initialized, please call RuntimeHostProvider.Initialize before using it.");
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.AddLog"/>
        /// </summary>
        public void AddLog(LogKind kind, string message, params object[] parameters)
        {
            EventHandler<MessageLogEventArgs> handler = eventCheckerTable["MessageLogger"];
            if (handler != null)
            {
                handler(this, new MessageLogEventArgs(kind, message, parameters));
            }
            else
            {
                throw new InvalidOperationException(
                    "Runtime host is not initialized, please call RuntimeHostProvider.Initialize before using it.");
            }
        }

        /// <summary>
        /// Implement <see cref="IRuntimeHost.CaptureRequirement"/>
        /// </summary>
        public void CaptureRequirement(string protocolName, int requirementId, string description)
        {
            EventHandler<RequirementCaptureEventArgs> handler = eventReqTable["ReqCaptureLogger"];
            if (handler != null)
            {
                handler(this, new RequirementCaptureEventArgs(
                    protocolName, requirementId, description));
            }
            else
            {
                throw new InvalidOperationException(
                    "Runtime host is not initialized, please call RuntimeHostProvider.Initialize before using it.");
            }
        }

        #endregion
    }
}
