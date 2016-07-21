// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// the base class of security provider,<para/>
    /// which provides the interfaces of encode/decode with security mechanism.
    /// </summary>
    public abstract class AdtsLdapSecurityLayer : IDisposable
    {
        #region Fields

        /// <summary>
        /// a bool value that indicates whether using the message security.<para/>
        /// if true, message encrypt/decrypt is enabled.<para/>
        /// if false, message will transport as plaintext.
        /// </summary>
        private bool usingMessageSecurity;

        /// <summary>
        /// an int value that indicates the consumed length by security layer.
        /// </summary>
        protected int consumedLength;

        /// <summary>
        /// get a bool value that indicates whether security layer consumed data.<para/>
        /// if true, the decoder will set the consumedLength to the ConsumedLength of security layer.<para/>
        /// if false, the decoder will ignore the ConsumedLength of security layer.
        /// </summary>
        protected bool consumedData;

        #endregion

        #region Properties

        /// <summary>
        /// get an int value that indicates the consumed length by security layer.
        /// </summary>
        public int ConsumedLength
        {
            get
            {
                return this.consumedLength;
            }
        }


        /// <summary>
        /// get/set a bool value that indicates whether using the message security.<para/>
        /// if true, message encrypt/decrypt is enabled.<para/>
        /// if false, message will transport as plaintext.
        /// </summary>
        internal bool UsingMessageSecurity
        {
            get
            {
                return this.usingMessageSecurity;
            }
            set
            {
                this.usingMessageSecurity = value;
            }
        }


        /// <summary>
        /// get a bool value that indicates whether security layer consumed data.<para/>
        /// if true, the decoder will set the consumedLength to the ConsumedLength of security layer.<para/>
        /// if false, the decoder will ignore the ConsumedLength of security layer.
        /// </summary>
        public bool ConsumedData
        {
            get
            {
                return this.consumedData;
            }
            set
            {
                this.consumedData = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// encoding the data with security provider.<para/>
        /// if encrypted failed, return the plaintext, no exception will be threw.
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be encoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the encoded data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public abstract byte[] Encode(byte[] data);


        /// <summary>
        /// decoding the data with security provider.<para/>
        /// if the UsingMessageSecurity is true, decode the data and set the ConsumedLength.
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be decoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the decoded data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public abstract byte[] Decode(byte[] data);

        #endregion

        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Release the managed and unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~AdtsLdapSecurityLayer()
        {
            Dispose(false);
        }

        #endregion
    }
}