// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// Base class of KileClient and KileServer.
    /// Maintain the common functions.
    /// </summary>
    public abstract class KerberosRole : IDisposable
    {
        #region Private Members
        /// <summary>
        /// Represents whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        public abstract KerberosContext Context
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Decode KILE PDUs from received message bytes
        /// </summary>
        /// <param name="endPoint">An endpoint from which the message bytes are received</param>
        /// <param name="receivedBytes">The received bytes to be decoded</param>
        /// <param name="consumedLength">Length of message bytes consumed by decoder</param>
        /// <param name="expectedLength">Length of message bytes the decoder expects to receive</param>
        /// <returns>The decoded KILE PDUs</returns>
        /// <exception cref="System.FormatException">thrown when a kile message type is unsupported</exception>
        public abstract KerberosPdu[] DecodePacketCallback(object endPoint,
                                                byte[] receivedBytes,
                                                out int consumedLength,
                                                out int expectedLength);

        /// <summary>
        /// Updated context based on Kerberos pdu
        /// </summary>
        /// <param name="pdu">Kerberos pdu</param>
        public virtual void UpdateContext(KerberosPdu pdu)
        { 
        }

        #region IDisposable
        /// <summary>
        /// Release the managed and unmanaged resources.
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
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                }

                //Note disposing has been done.
                disposed = true;
            }
        }


        /// <summary>
        /// Destruct this instance.
        /// </summary>
        ~KerberosRole()
        {
            Dispose(false);
        }
        #endregion
    }
}
