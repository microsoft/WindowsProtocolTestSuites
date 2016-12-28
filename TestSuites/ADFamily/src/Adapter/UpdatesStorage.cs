// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Common
{
    /// <summary>
    /// Unique storage for updates invoke and revert
    /// </summary>
    public class UpdatesStorage 
    {
        /// <summary>
        /// private constructor
        /// </summary>
        private UpdatesStorage() { }

        /// <summary>
        /// static instance
        /// </summary>
        static UpdatesStorage inst = new UpdatesStorage();

        /// <summary>
        ///stores for updates
        /// </summary>
        Stack<IUpdate> store = new Stack<IUpdate>();

        /// <summary>
        /// ptf test site
        /// </summary>
        ITestSite testSite;

        /// <summary>
        /// singleton
        /// </summary>
        /// <returns>IUpdatesStorage object.</returns>
        public static UpdatesStorage GetInstance()
        {
            return inst;
        }

        /// <summary>
        /// return test site for updates usage
        /// </summary>
        public ITestSite TestSite
        {
            get
            {
                return testSite;
            }
        }

        /// <summary>
        /// initialize storage
        /// </summary>
        /// <param name="site">ptf test iste</param>
        public void Initialize(ITestSite site)
        {
            testSite = site;
            //clear existing changes to revert environment
            Clear();
        }

        /// <summary>
        /// clear all updates and revert them
        /// </summary>
        public void Clear()
        {
            while (store.Count > 0)
            {
                store.Pop().Revert();
            }
        }

        /// <summary>
        /// push in a update and invoke
        /// </summary>
        /// <param name="update">update</param>
        /// <returns>true if success</returns>
        public bool PushUpdate(IUpdate update)
        {
            if (update.Invoke())
            {
                store.Push(update);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// pop out an update and revert it
        /// </summary>
        /// <param name="ret">update</param>
        /// <returns>true if success reverted</returns>
        public bool PopUpdate(ref IUpdate ret)
        {
            ret = store.Pop();
            return ret.Revert();
        }

        /// <summary>
        /// get update in stack top
        /// </summary>
        public IUpdate Top
        {
            get { return store.Peek(); }
        }

        /// <summary>
        /// count the updates
        /// </summary>
        public int Count
        {
            get { return store.Count; }
        }
    }
}
