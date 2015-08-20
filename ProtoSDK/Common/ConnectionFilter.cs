// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Customize filter. User can add customize filter by this callback.
    /// </summary>
    /// <param name="endPoint">
    /// the endPoint of connection to be filtered.
    /// </param>
    /// <returns>If the packet agrees with filter, return true. Otherwise false.</returns>
    public delegate bool CustomizeConnectionFilterCallback(IPEndPoint endPoint);

    /// <summary>
    /// the filter for server to filter connections
    /// </summary>
    public class ConnectionFilter : StackFilter, IDisposable
    {
        /// <summary>
        /// User can set this property to add customize filter.
        /// </summary>
        private CustomizeConnectionFilterCallback customizeFilterCallback;

        /// <summary>
        /// User can set this property to add customize filter.
        /// </summary>
        public CustomizeConnectionFilterCallback CustomizeFilterCallback
        {
            get
            {
                return this.customizeFilterCallback;
            }
            set
            {
                this.customizeFilterCallback = value;
            }
        }

        /// <summary>
        /// the source address to filter
        /// </summary>
        private System.Collections.Generic.List<string> sourceAddressList;

        /// <summary>
        /// add the source address to filter
        /// </summary>
        public void AddSourceAddress(string sourceAddress)
        {
            this.sourceAddressList.Add(sourceAddress);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionFilter()
        {
            this.sourceAddressList = new System.Collections.Generic.List<string>();
            this.CustomizeFilterCallback += new CustomizeConnectionFilterCallback(this.FilterClientConnection);
        }

        /// <summary>
        /// the connection filter callback function
        /// </summary>
        /// <param name="endPoint">
        /// the endPoint of connection to be filtered.
        /// </param>
        /// <returns>the filter result</returns>
        private bool FilterClientConnection(IPEndPoint endPoint)
        {
            foreach (string sourceAddress in this.sourceAddressList)
            {
                // get ip HostEntry of source address
                IPHostEntry ipHostEntry = Dns.GetHostEntry(sourceAddress);
                if (ipHostEntry != null && ipHostEntry.AddressList.Length > 0)
                {
                    // filter the address for all addresses in HostEntry.
                    foreach (IPAddress address in ipHostEntry.AddressList)
                    {
                        if (address.Equals(endPoint.Address))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Invoke customize filter.
        /// </summary>
        /// <param name="endPoint">endpoint to be filtered.</param>
        /// <returns>Filter result. True if the packet should be filtered by the customized filter and discarded. Otherwise false.</returns>
        public bool FilterConnection(IPEndPoint endPoint)
        {
            if (this.CustomizeFilterCallback != null)
            {
                return this.CustomizeFilterCallback(endPoint);
            }

            return false;
        }


        #region IDisposable

        private bool disposed;

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }

                //Note disposing has been done.
                disposed = true;
            }
        }

        #endregion
    }
}