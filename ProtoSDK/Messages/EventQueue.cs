// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk.Messages
{
    /// <summary>
    /// A wrapper class for the event observation queue.
    /// This class can be used from programmed test suites.
    /// </summary>
    public class EventQueue
    {
        TimeSpan timeout = TimeSpan.FromSeconds(5);
        IRuntimeHost host;
        ObservationQueue<AvailableEvent> queue;
        const int defaultMaxSize = 200;

        /// <summary>
        /// Initializes event queue for the given test site.
        /// </summary>
        /// <param name="host">The message runtime host.</param>
        /// <param name="maxSize">The max queue size</param>
        public EventQueue( IRuntimeHost host, int maxSize)
        {
            this.host = host;
            this.queue = new ObservationQueue<AvailableEvent>(maxSize);
        }

        /// <summary>
        /// Initializes event queue for the given test site.
        /// The default max queue size is 200.
        /// </summary>
        /// <param name="host">The message runtime host.</param>
        public EventQueue(IRuntimeHost host)
            : this(host, defaultMaxSize) 
        { }

        /// <summary>
        /// Gets or sets timeout value which is to be used when expecting events.
        /// The default timeout value is 5 seconds.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <summary>
        /// Waits for the given event and using given handler to check its contents. 
        /// An assertion failure is raised on test site if expected event does not occur after timeout.
        /// </summary>
        /// <typeparam name="T">The type of the event handler.</typeparam>
        /// <param name="eventInfo">The event info of the expected event.</param>
        /// <param name="handler">The event handler</param>
        public void Expect<T>(EventInfo eventInfo, T handler)
        {
            if (eventInfo.EventHandlerType != typeof(T))
                host.Assume(false, string.Format(CultureInfo.InvariantCulture, "passing wrong event handler type '{0}' to Expect method; expected type '{1}'",
                                    typeof(T), eventInfo.EventHandlerType));

            AvailableEvent entry;
            if (!queue.TryGet(timeout, false, out entry) || entry.Event != eventInfo)
            {
                host.Assert(false, string.Format(CultureInfo.InvariantCulture, "did not receive expected event '{0}' after waiting '{1}'; queue: {2}",
                                    eventInfo.Name, timeout, DumpQueue()));
            }

            // call handler
            Delegate del = handler as Delegate;
            del.DynamicInvoke(entry.Parameters);

            // consume event
            queue.TryGet(timeout, true, out entry);
        }

        /// <summary>
        /// Waits for one of the given events and uses given handlers to check contents.
        /// An assertion failure is raised on test site if any expected event does not occur after timeout. 
        /// </summary>
        /// <typeparam name="T1">The type of the first event handler.</typeparam>
        /// <typeparam name="T2">The type of the second event handler.</typeparam>
        /// <param name="info1">The event info of the first expected event.</param>
        /// <param name="handler1">The first event handler.</param>
        /// <param name="info2">The event info of the second expected event.</param>
        /// <param name="handler2">The second event handler.</param>
        /// <returns>0 if the first event occurs, or 1 if the second event occurs.</returns>
        public int ExpectOneOf<T1, T2>(EventInfo info1, T1 handler1, EventInfo info2, T2 handler2)
        {
            if (info1.EventHandlerType != typeof(T1))
                host.Assume(false, string.Format(CultureInfo.InvariantCulture, "passing wrong event handler type '{0}' to Expect method; expected type '{1}'",
                                    typeof(T1), info1.EventHandlerType));
            if (info2.EventHandlerType != typeof(T2))
                host.Assume(false, string.Format(CultureInfo.InvariantCulture, "passing wrong event handler type '{0}' to Expect method; expected type '{1}'",
                                    typeof(T2), info2.EventHandlerType));

            int result = -1;
            AvailableEvent entry;

            if (!queue.TryGet(timeout, false, out entry) || (entry.Event != info1 && entry.Event != info2))
            {
                host.Assert(false, string.Format(CultureInfo.InvariantCulture, "did not receive expected event '{0}' or '{1}' after waiting '{2}'; queue: {3}",
                                    info1.Name, info2.Name, timeout, DumpQueue()));
            }

            // call handler
            if (entry.Event == info1)
            {
                Delegate del = handler1 as Delegate;
                del.DynamicInvoke(entry.Parameters);
                result = 0;
            }
            else
            {
                Delegate del = handler2 as Delegate;
                del.DynamicInvoke(entry.Parameters);
                result = 1;
            }

            // consume
            queue.TryGet(timeout, true, out entry);
            return result;
        }

        /// <summary>
        /// Logs an event occurrence in the event queue.
        /// </summary>
        /// <param name="info">The event info to be logged.</param>
        /// <param name="arguments">The extra arguments for logging.</param>
        // This suppression is adopted to prevent breaking change to existing samples, test cases and user projects.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public void LogEvent(EventInfo info, params object[] arguments)
        {
            queue.Add(new AvailableEvent(info, null, arguments));
        }

        private string DumpQueue()
        {
            StringBuilder sb = new StringBuilder();
            foreach (AvailableEvent entry in queue.GetEnumerator())
            {
                sb.AppendLine(entry.ToString());
            }
            return sb.ToString();
        }

    }

    /// <summary>
    /// A type to describe an available event.
    /// </summary>
    public struct AvailableEvent
    {
        /// <summary>
        /// The event identified by its
        /// reflection representation.
        /// </summary>
        public EventInfo Event
        {
            get;
            private set;
        }

        /// <summary>
        /// The target of the event (the instance object where the event
        /// belongs too), or null, if it is a static or an adapter event.
        /// </summary>
        public object Target
        {
            get;
            private set;
        }

        /// <summary>
        /// The parameters passed to the event.
        /// </summary>
        public object[] Parameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="eventInfo">The event identified by its reflection representation</param>
        /// <param name="target">The target of the event (the instance object where the event belongs too)</param>
        /// <param name="parameters">Parameters passed to the event</param>
        public AvailableEvent(EventInfo eventInfo, object target, object[] parameters)
            : this()
        {
            this.Event = eventInfo;
            this.Target = target;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Delivers readable representation.
        /// </summary>
        /// <returns>the representation string</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            if (Target != null)
            {
                result.Append(MessageRuntimeHelper.Describe(Target));
                result.Append(".");
            }
            result.Append("event ");
            result.Append(Event.Name);
            result.Append("(");
            bool first = true;
            foreach (object param in Parameters)
            {
                if (first)
                    first = false;
                else
                    result.Append(",");
                result.Append(MessageRuntimeHelper.Describe(param));
            }
            result.Append(")");
            return result.ToString();
        }

    }
}
