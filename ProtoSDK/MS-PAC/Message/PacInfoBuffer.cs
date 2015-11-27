// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  Following the PACTYPE structure is an array of PAC_INFO_BUFFER
    ///  structures that each define the type and byte offset
    ///  to a buffer of the PAC. The PAC_INFO_BUFFER array
    ///  has no defined ordering.  Therefore, the order of the
    ///  PAC_INFO_BUFFER buffers has no significance.  However,
    ///  once the Key Distribution Center (KDC) and server signatures
    ///  are generated, the ordering of the buffers MUST NOT
    ///  change, or signature verification of the PAC contents
    ///  will fail. 
    /// </summary>
    public abstract class PacInfoBuffer
    {
        #region Virtual Methods

        /// <summary>
        /// Decode specified buffer from specified index, with specified count
        /// of bytes, into the instance of current class.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The specified index from beginning of buffer.</param>
        /// <param name="count">The specified count of bytes to be decoded.</param>
        abstract internal void DecodeBuffer(byte[] buffer, int index, int count);


        /// <summary>
        /// Encode the instance of current class into byte array,
        /// according to TD specification.
        /// </summary>
        /// <returns>The encoded byte array</returns>
        abstract internal byte[] EncodeBuffer();


        /// <summary>
        /// Calculate size of current instance's encoded buffer, in bytes.
        /// </summary>
        /// <returns>The size of current instance's encoded buffer, in bytes.</returns>
        abstract internal int CalculateSize();


        /// <summary>
        /// Get the ulType of current instance's PAC_INFO_BUFFER.
        /// </summary>
        /// <returns>The ulType of current instance's PAC_INFO_BUFFER.</returns>
        abstract internal PAC_INFO_BUFFER_Type_Values GetBufferInfoType();

        #endregion

        #region Internal Methods

        /// <summary>
        /// Create a native PAC_INFO_BUFFER of instance of current class,
        /// leaves cbBufferSize and Offset empty.
        /// </summary>
        /// <returns>The native PAC_INFO_BUFFER of instance of current class.</returns>
        internal PAC_INFO_BUFFER CreateNativeInfoBuffer()
        {
            PAC_INFO_BUFFER nativeInfo = new PAC_INFO_BUFFER();
            nativeInfo.ulType = GetBufferInfoType();

            return nativeInfo;
        }


        /// <summary>
        /// Decode the specified PAC_TYPE buffer into an instance of PacInfoBuffer,
        /// according to specified PAC_INFO_BUFFER native structure.
        /// </summary>
        /// <param name="nativePacInfoBuffer">The specified PAC_INFO_BUFFER native structure.</param>
        /// <param name="buffer">The specified PAC_TYPE buffer.</param>
        /// <returns>The decoded instance of PacInfoBuffer.</returns>
        static internal PacInfoBuffer DecodeBuffer(PAC_INFO_BUFFER nativePacInfoBuffer, byte[] buffer)
        {
            PacInfoBuffer pacInfoBuffer = CreatePacInfoBuffer(nativePacInfoBuffer.ulType);

            pacInfoBuffer.DecodeBuffer(
                buffer,
                (int)nativePacInfoBuffer.Offset,
                (int)nativePacInfoBuffer.cbBufferSize);

            return pacInfoBuffer;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Create an instance of current class according to specified ulType_Values.
        /// </summary>
        /// <param name="ulType">The specified ulType_Values.</param>
        /// <returns>The created instance of current class.</returns>
        private static PacInfoBuffer CreatePacInfoBuffer(PAC_INFO_BUFFER_Type_Values ulType)
        {
            PacInfoBuffer pacInfoBuffer;
            switch (ulType)
            {
                case PAC_INFO_BUFFER_Type_Values.LogonInformation:
                    pacInfoBuffer = new KerbValidationInfo();
                    break;

                case PAC_INFO_BUFFER_Type_Values.CredentialsInformation:
                    pacInfoBuffer = new PacCredentialInfo();
                    break;

                case PAC_INFO_BUFFER_Type_Values.ServerChecksum:
                    pacInfoBuffer = new PacServerSignature();
                    break;

                case PAC_INFO_BUFFER_Type_Values.KdcChecksum:
                    pacInfoBuffer = new PacKdcSignature();
                    break;

                case PAC_INFO_BUFFER_Type_Values.ClientNameAndTicketInformation:
                    pacInfoBuffer = new PacClientInfo();
                    break;

                case PAC_INFO_BUFFER_Type_Values.ConstrainedDelegationInformation:
                    pacInfoBuffer = new S4uDelegationInfo();
                    break;

                case PAC_INFO_BUFFER_Type_Values.UpnAndDnsInformation:
                    pacInfoBuffer = new UpnDnsInfo();
                    break;

                case PAC_INFO_BUFFER_Type_Values.ClientClaimsInformation:
                    pacInfoBuffer = new ClientClaimsInfo();
                    break;

                case PAC_INFO_BUFFER_Type_Values.PacDeviceInfo:
                    pacInfoBuffer = new PacDeviceInfo();
                    break;
                case PAC_INFO_BUFFER_Type_Values.DeviceClaimsInformation:
                    pacInfoBuffer = new DeviceClaimsInfo();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ulType");
            }
            return pacInfoBuffer;
        }
        #endregion
    }
}
