// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Customize filter. User can add customize filter by this callback.
    /// </summary>
    /// <param name="stackPacket">
    /// the stackPacket to be filter.
    /// </param>
    /// <returns>If the packet agrees with filter, return true. Otherwise false.</returns>
    public delegate bool CustomizePacketFilterCallback(StackPacket stackPacket);

    /// <summary>
    /// Filters as class name. User can use it to drop some types of packet.implement the IDisposable interface
    /// and the IDisposable.Dispose method.
    /// </summary>
    public class PacketFilter : StackFilter, IDisposable
    {
        private List<Type> typeFilters;

        private CustomizePacketFilterCallback customizeFilterCallback;

        /// <summary>
        /// User can set this property to add customize filter.
        /// </summary>
        public CustomizePacketFilterCallback CustomizeFilterCallback
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
        /// Constructor
        /// </summary>
        public PacketFilter()
        {
            this.typeFilters = new List<Type>();
        }

        /// <summary>
        /// Add packet types as filters.
        /// </summary>
        /// <param name="types">Array of packet types.</param>
        public void AddFilters(Type[] types)
        {
            lock (this)
            {   
             //Adds the elements of types to the end of typeFilters;
                this.typeFilters.AddRange(types);
            }

            //Logging.Logger.INFO(LogFilter.STACK, "Filters to be added.");
        }

        /// <summary>
        /// Clear all filters
        /// </summary>
        public void ClearFilters()
        {
            lock (this)
            {
                this.customizeFilterCallback = null;
                this.typeFilters.Clear();
            }

            //Logging.Logger.INFO(LogFilter.STACK, "Filters to be cleared.");
        }

        /// <summary>
        /// Filter packets.
        /// </summary>
        /// <param name="packet">packet to be filtered.</param>
        /// <returns>Filter result. True if the type of packet agrees with the filter. Otherwise false.</returns>
        public bool FilterPacket(StackPacket packet)
        {
            //Use class name as filter
            lock (this)
            {
                foreach (Type type in typeFilters)
                {
                    if (type == packet.GetType())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Invoke customize filter.
        /// </summary>
        /// <param name="stackPacket">stackPacket to be filtered.</param>
        /// <returns>Filter result. True if the packet should be filtered by the customized filter and discarded. Otherwise false.</returns>
        public bool CustomizeFilter(StackPacket stackPacket)
        {
            if (this.CustomizeFilterCallback != null)
            {
                return this.CustomizeFilterCallback(stackPacket);
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
        /// <param name="disposing">If disposing equals true, managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                    typeFilters.Clear();
                    typeFilters = null;
                }

                //Note disposing has been done.
                disposed = true;
            }
        }

        #endregion
    }
}
