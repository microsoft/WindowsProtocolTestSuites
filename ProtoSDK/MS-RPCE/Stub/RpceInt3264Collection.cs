// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// A collection of Int3264 that will release unmanaged resource 
    /// that pointer (directly or indirectly) to at dispose.
    /// </summary>
    public class RpceInt3264Collection : Collection<Int3264>, IDisposable
    {
        // unmanaged resource.
        private IDisposable disposable;


        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="numList">A list of Int3264.</param>
        /// <param name="toDispose">
        /// Extra IDisposable object that keep the reference and release at Dispose.
        /// </param>
        internal RpceInt3264Collection(IList<Int3264> numList, IDisposable toDispose)
            : base(numList)
        {
            disposable = toDispose;
        }


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
            }

            if (disposable != null)
            {
                disposable.Dispose();
                disposable = null;
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~RpceInt3264Collection()
        {
            Dispose(false);
        }

        #endregion
    }
}
