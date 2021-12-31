// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.WspTS
{
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public partial class WspModelTestBase : PtfTestClassBase
    {
        private IWspAdapter cachedWspAdapter = null;

        private Stack<(EventInfo info, Delegate handler)> cachedInfoHandlerPairs = new Stack<(EventInfo, Delegate)>();

        /// <summary>
        /// Let test manager subscribe to the given event. 
        /// Events raised on this eventInfo will be propagated to the event queue.
        /// </summary>
        /// <param name="eventInfo">The event reflection information.</param>
        /// <param name="wspAdapter">The target (IWspAdapter instance to which the event belongs).</param>
        protected void Subscribe(EventInfo eventInfo, IWspAdapter wspAdapter)
        {
            if (cachedWspAdapter == null)
            {
                cachedWspAdapter = wspAdapter;
            }

            Action<uint> responseHandler = (uint errorCode) => this.Manager.AddEvent(eventInfo, wspAdapter, new object[] { errorCode });
            var eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, responseHandler.Target, responseHandler.Method);
            eventInfo.AddEventHandler(wspAdapter, eventHandler);

            cachedInfoHandlerPairs.Push((eventInfo, eventHandler));
        }

        /// <summary>
        /// Remove all handlers added to cached EventInfo instances and reset the cached IWspAdapter instance.
        /// </summary>
        protected override void TestCleanup()
        {
            while (cachedInfoHandlerPairs.Count > 0)
            {
                var (info, handler) = cachedInfoHandlerPairs.Pop();
                info.RemoveEventHandler(cachedWspAdapter, handler);
            }

            cachedWspAdapter = null;

            base.TestCleanup();
        }
    }
}