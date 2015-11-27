// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  The PACTYPE structure is the topmost structure of the
    ///  PAC and specifies the number of elements in the PAC_INFO_BUFFER
    ///  array. The PACTYPE structure serves as the header
    ///  for the complete PAC data.
    /// </summary>
    public class PacType : IHasInterrelatedFields
    {
        /// <summary>
        /// TD section 2.3: "All PAC elements MUST be placed on an 8-byte boundary."
        /// </summary>
        private const int PacTypeAlignUnit = 8;

        private PacInfoBuffer[] pacInfoBuffers;

        /// <summary>
        /// The native PACTYPE object.
        /// </summary>
        public PACTYPE NativePacType;

        /// <summary>
        /// The list of PacInfoBuffer objects contained in the PACTYPE structure.
        /// </summary>
        public PacInfoBuffer[] PacInfoBuffers
        {
            get
            {
                return pacInfoBuffers;
            }
        }


        /// <summary>
        /// Calculate Server checksum (TD section 2.8.1) of the PAC message, 
        /// place the checksum as a PacInfoBuffer whose ulType_Values is V3.
        /// Then calculate KDC checksum (TD section 2.8.2) of the PAC message, 
        /// place the checksum as a PacInfoBuffer whose ulType_Values is V4.
        /// </summary>
        /// <param name="serverSignKey">The server signature key used to generate server signature.</param>
        /// <param name="kdcSignKey">The KDC signature key used to generate KDC signature.</param>
        /// <exception cref="ArgumentNullException">serverSignkey or kdcSignKey is null.</exception>
        public void Sign(byte[] serverSignKey, byte[] kdcSignKey)
        {
            // locate server and KDC signatures.
            PacSignatureData serverSign;
            PacSignatureData kdcSign;

            FindSignatures(out serverSign, out kdcSign);

            // types
            PAC_SIGNATURE_DATA_SignatureType_Values serversignType = serverSign.NativePacSignatureData.SignatureType;
            PAC_SIGNATURE_DATA_SignatureType_Values kdcSignType = kdcSign.NativePacSignatureData.SignatureType;

            // clear signatures.
            int serverLength = PacSignatureData.CalculateSignatureLength(serversignType);
            serverSign.NativePacSignatureData.Signature = new byte[serverLength];

            int kdcLength = PacSignatureData.CalculateSignatureLength(kdcSignType);
            kdcSign.NativePacSignatureData.Signature = new byte[kdcLength];

            // sign server
            byte[] serverSignResult = PacSignatureData.Sign(
                ToBytes(), serversignType, serverSignKey);
            serverSign.NativePacSignatureData.Signature = serverSignResult;

            // sign KDC
            kdcSign.NativePacSignatureData.Signature = PacSignatureData.Sign(
                serverSignResult, kdcSignType, kdcSignKey);
        }


        /// <summary>
        /// Verify Server checksum (TD section 2.8.1) and KDC checksum (TD section 2.8.2)
        /// of the PAC message.
        /// </summary>
        /// <param name="serverSignKey">The server signature key used to generate server signature.</param>
        /// <param name="kdcSignKey">The KDC signature key used to generate KDC signature.</param>
        /// <param name="isServerSignValid">true if server signature is valid.</param>
        /// <param name="isKdcSignValid">true if KDC signature is valid.</param>
        /// <exception cref="ArgumentNullException">serverSignkey or kdcSignKey is null.</exception>
        public void ValidateSign(byte[] serverSignKey, byte[] kdcSignKey, 
            out bool isServerSignValid, out bool isKdcSignValid)
        {
            if (serverSignKey == null)
            {
                throw new ArgumentNullException("serverSignKey");
            }
            if (kdcSignKey == null)
            {
                throw new ArgumentNullException("kdcSignKey");
            }

            // locate server and KDC signatures.
            PacSignatureData serverSign;
            PacSignatureData kdcSign;

            FindSignatures(out serverSign, out kdcSign);

            // these validate operation will change signature values, so must backup and restore.
            byte[] oldServerSign = null;
            byte[] oldKdcSign = null;

            isServerSignValid = false;
            isKdcSignValid = false;

            try
            {
                // types
                PAC_SIGNATURE_DATA_SignatureType_Values serverSignType = serverSign.NativePacSignatureData.SignatureType;
                PAC_SIGNATURE_DATA_SignatureType_Values kdcSignType = kdcSign.NativePacSignatureData.SignatureType;

                // backup and clear signatures.
                int serverLength = PacSignatureData.CalculateSignatureLength(serverSignType);
                oldServerSign = serverSign.NativePacSignatureData.Signature;
                serverSign.NativePacSignatureData.Signature = new byte[serverLength];

                int kdcLength = PacSignatureData.CalculateSignatureLength(kdcSignType);
                oldKdcSign = kdcSign.NativePacSignatureData.Signature;
                kdcSign.NativePacSignatureData.Signature = new byte[kdcLength];

                // validate server sign
                byte[] serverSignResult = PacSignatureData.Sign(
                    ToBytes(), serverSignType, serverSignKey);
                isServerSignValid = ArrayUtility.CompareArrays<byte>(serverSignResult, oldServerSign);

                // validate KDC sign
                byte[] kdcSignResult = PacSignatureData.Sign(
                    serverSignResult, kdcSignType, kdcSignKey);
                isKdcSignValid = ArrayUtility.CompareArrays<byte>(kdcSignResult, oldKdcSign);
            }
            finally
            {
                // restore server signature
                if (oldServerSign != null)
                {
                    serverSign.NativePacSignatureData.Signature = oldServerSign;
                }
                // restore KDC signature
                if (oldKdcSign != null)
                {
                    kdcSign.NativePacSignatureData.Signature = oldKdcSign;
                }
            }
        }

        /// <summary>
        /// Encode the instance of current class into bytes.
        /// </summary>
        /// <returns>The encoded bytes.</returns>
        public byte[] ToBytes()
        {
            UpdateInterrelatedFields();

            byte[] header = PacUtility.ObjectToMemory(NativePacType);

            byte[][] bodyReferences = new byte[NativePacType.cBuffers][];

            for (int i = 0; i < NativePacType.cBuffers; i++)
            {
                if (pacInfoBuffers[i] == null) continue;
                bodyReferences[i] = pacInfoBuffers[i].EncodeBuffer();

                int length = bodyReferences[i].Length;
                // double check the totalLength
                // NativePacType.Buffers[index].cbBufferSize has been updated in previous
                // UpdateInterrelatedFields() method, given by each pacInfoBuffer's
                // CalculateSize() method.
                // following evaluation double check whether CalculateSize() method's
                // result conforms to EncodeBuffer() method's result.
                // Note: some kind of pacInfoBuffers return EncodeBuffer().Length as the
                // CalculateSize(), such as NDR encoded structures. But some other 
                // pacInfoBuffers can calculate totalLength without encoding the structure.
                AssertBufferSize(NativePacType.Buffers, i, length);
            }

            return ConcatenateBuffers(header, bodyReferences);
        }

        #region IHasInterrelatedFields Members

        /// <summary>
        /// Update cbBufferSize and Offset of all PAC_INFO_BUFFERs
        /// after the buffers' content updated.
        /// </summary>
        public void UpdateInterrelatedFields()
        {
            AssertBuffersCount(pacInfoBuffers.Length, NativePacType.Buffers.Length);

            int offset = CalculateHeaderSize();

            for (int i = 0; i < pacInfoBuffers.Length; i++)
            {   
                NativePacType.Buffers[i].Offset = (ulong)offset;

                if (pacInfoBuffers[i] == null) continue;
                int size = pacInfoBuffers[i].CalculateSize();
                NativePacType.Buffers[i].cbBufferSize = (uint)size;

                offset += size;

                //TD section 2.3: "All PAC elements MUST be placed on an 8-byte boundary."
                offset = PacUtility.AlignTo(offset, PacTypeAlignUnit);
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Construct an instance of current class by decoding specified bytes.
        /// </summary>
        /// <param name="buffer">The specified bytes.</param>
        internal PacType(byte[] buffer)
        {
            NativePacType = PacUtility.MemoryToObject<PACTYPE>(buffer);
            pacInfoBuffers = new PacInfoBuffer[NativePacType.Buffers.Length];

            for (int i = 0; i < NativePacType.Buffers.Length; ++i)
            {
                if (NativePacType.Buffers[i].cbBufferSize > 0)
                {
                    PacInfoBuffers[i] = PacInfoBuffer.DecodeBuffer(
                        NativePacType.Buffers[i],
                        buffer);
                }
                else PacInfoBuffers[i] = null;
            }
        }

        /// <summary>
        /// Creates an PacType instance using the specified signature types and PacInfoBuffers
        /// </summary>
        /// <param name="serverSignatureType">Server signature signatureType.</param>
        /// <param name="serverSignKey">The server signature key used to generate server signature.</param>
        /// <param name="kdcSignatureType">KDC signature Type.</param>
        /// <param name="kdcSignKey">The KDC signature key used to generate KDC signature.</param>
        /// <param name="buffers">A list of PacInfoBuffer used to create the PacType.
        /// Note: DO NOT include the signatures (server signature and KDC signature)!</param>
        /// <returns>The created PacType instance.</returns>
        internal PacType(
            PAC_SIGNATURE_DATA_SignatureType_Values serverSignatureType,
            byte[] serverSignKey, 
            PAC_SIGNATURE_DATA_SignatureType_Values kdcSignatureType,
            byte[] kdcSignKey,
            params PacInfoBuffer[] buffers)
        {
            if (buffers == null || buffers.Length == 0)
            {
                throw new ArgumentNullException("buffers");
            }

            InitializePacInfoBuffers(serverSignatureType, kdcSignatureType, buffers);

            InitializeNativePacType();

            Sign(serverSignKey, kdcSignKey);
            // after Sign(), the contentd of signature buffers are filled,
            // and the lengths of which are changed.
            // Then update interrelated fields, mainly totalLength and offset.
            UpdateInterrelatedFields();
        }


       
        #endregion

        #region Private Methods

        /// <summary>
        /// In constructor, initialize the native PACTYPE structure.
        /// </summary>
        private void InitializeNativePacType()
        {
            NativePacType = new PACTYPE();
            NativePacType.Version = PACTYPE_Version_Values.V1;
            NativePacType.cBuffers = (uint)pacInfoBuffers.Length;
            NativePacType.Buffers = new PAC_INFO_BUFFER[pacInfoBuffers.Length];

            for (int i = 0; i < pacInfoBuffers.Length; i++)
            {
                NativePacType.Buffers[i] = pacInfoBuffers[i].CreateNativeInfoBuffer();
            }
        }


        /// <summary>
        /// In constructor, initialize the member PacInfoBuffer array.
        /// </summary>
        /// <param name="serverSignatureType">The specified
        /// Server Signature Type.</param>
        /// <param name="kdcSignatureType">The specified
        /// KDC Signature Type.</param>
        /// <param name="buffers">PacInfoBuffers not including signatures.</param>
        private void InitializePacInfoBuffers(
            PAC_SIGNATURE_DATA_SignatureType_Values serverSignatureType,
            PAC_SIGNATURE_DATA_SignatureType_Values kdcSignatureType,
            PacInfoBuffer[] buffers)
        {
            // allocate 2 more buffers for server signature and KDC signature. 
            pacInfoBuffers = new PacInfoBuffer[buffers.Length + 2];
            for (int i = 0; i < buffers.Length; i++)
            {
                pacInfoBuffers[i] = buffers[i];
            }
            // construct a n empty server signature
            PacServerSignature serverSign = new PacServerSignature();
            serverSign.NativePacSignatureData.SignatureType = serverSignatureType;
            serverSign.NativePacSignatureData.Signature = new byte[0];
            pacInfoBuffers[pacInfoBuffers.Length - 2] = serverSign;
            // construct a n empty KDC signature
            PacKdcSignature kdcSign = new PacKdcSignature();
            kdcSign.NativePacSignatureData.SignatureType = kdcSignatureType;
            kdcSign.NativePacSignatureData.Signature = new byte[0];
            pacInfoBuffers[pacInfoBuffers.Length - 1] = kdcSign;
        }


        /// <summary>
        /// Concatenate specified header buffer and specified body buffers.
        /// </summary>
        /// <param name="header">The specified header buffer.</param>
        /// <param name="bodyReferences">The specified body buffers.</param>
        /// <returns>The integrated buffer.</returns>
        private byte[] ConcatenateBuffers(byte[] header, byte[][] bodyReferences)
        {
            // calculate total totalLength
            int totalLength = header.Length;
            for (int i = 0; i < NativePacType.cBuffers; i++)
            {
                if (bodyReferences[i] == null) continue;
                int length = bodyReferences[i].Length;
                // double check the length
                // NativePacType.Buffers[index].cbBufferSize has been updated in previous
                // UpdateInterrelatedFields() method, given by each pacInfoBuffer's
                // CalculateSize() method.
                // following evaluation double check whether CalculateSize() method's
                // result conforms to EncodeBuffer() method's result.
                // Note: some kind of pacInfoBuffers return EncodeBuffer().Length as the
                // CalculateSize(), such as NDR encoded structures. But some other 
                // pacInfoBuffers can calculate totalLength without encoding the structure.
                AssertBufferSize(NativePacType.Buffers, i, length);

                totalLength += PacUtility.AlignTo(length, PacTypeAlignUnit);
            }

            byte[] total = new byte[totalLength];
            int offset = 0;

            Buffer.BlockCopy(header, 0, total, offset, header.Length);
            offset += header.Length;

            for (int i = 0; i < bodyReferences.Length; ++i)
            {
                if (bodyReferences[i] == null) continue;
                int length = bodyReferences[i].Length;
                Buffer.BlockCopy(bodyReferences[i], 0, total, offset, length);
                offset += PacUtility.AlignTo(length, PacTypeAlignUnit);
            }

            return total;
        }


        /// <summary>
        /// Assert buffers[index].cbBufferSiz == length.
        /// Else throw ArgumentException.
        /// </summary>
        /// <param name="buffers">The specified PAC_INFO_BUFFER[] array.</param>
        /// <param name="index">The specified array index.</param>
        /// <param name="length">The expected length of array element's cbBufferSize.</param>
        private static void AssertBufferSize(PAC_INFO_BUFFER[] buffers, int index, int length)
        {
            if (buffers[index].cbBufferSize != length)
            {
                throw new ArgumentException(
                    string.Format("NativePacType.Buffers[{0}].cbBufferSize {1} != length {2}",
                    index,
                    buffers[index].cbBufferSize,
                    length));
            }
        }


        /// <summary>
        /// Assert buffers' count actual is expected.
        /// </summary>
        /// <param name="expected">The expected buffers' count.</param>
        /// <param name="actual">The actual buffers' count.</param>
        private static void AssertBuffersCount(int expected, int actual)
        {
            if (expected != actual)
            {
                throw new ArgumentException(
                    string.Format("pacInfoBuffers.Length {0} != NativePacType.Buffers.Length {1}",
                    expected,
                    actual));
            }
        }


        /// <summary>
        /// Calculate the header buffer's size, in bytes.
        /// </summary>
        /// <returns>The header buffer's size, in bytes.</returns>
        private int CalculateHeaderSize()
        {
            // uint CBuffers and Versions
            int pacTypeHeaderSize = sizeof(uint) + sizeof(PACTYPE_Version_Values);
            // uint PacInfoType and CbBufferSize, ulong Offset
            long infoBufferHeaderSize = sizeof(PAC_INFO_BUFFER_Type_Values) + sizeof(uint) + sizeof(ulong);
            return (int)(pacTypeHeaderSize + infoBufferHeaderSize * NativePacType.cBuffers);
        }


        /// <summary>
        /// Find Server Signature and KDC signature.
        /// </summary>
        /// <param name="serverSign">The found Server Signature.</param>
        /// <param name="kdcSign">The found KDC Signature.</param>
        private void FindSignatures(out PacSignatureData serverSign, out PacSignatureData kdcSign)
        {
            serverSign = null;
            kdcSign = null;

            foreach (PacInfoBuffer infoBuffer in pacInfoBuffers)
            {
                if (infoBuffer == null) continue;
                PAC_INFO_BUFFER_Type_Values ulType = infoBuffer.GetBufferInfoType();
                if (ulType == PAC_INFO_BUFFER_Type_Values.ServerChecksum || ulType == PAC_INFO_BUFFER_Type_Values.KdcChecksum)
                {
                    PacSignatureData sign = (PacSignatureData)infoBuffer;
                    if (ulType == PAC_INFO_BUFFER_Type_Values.ServerChecksum)
                    {
                        serverSign = sign;
                    }
                    else
                    {
                        kdcSign = sign;
                    }
                }
            }

            AssertSignsNotNull(serverSign, kdcSign);
        }


        /// <summary>
        /// Assert signatures are not null.
        /// </summary>
        /// <param name="serverSign">Server signature to be asserted.</param>
        /// <param name="kdcSign">KDC signature to be asserted.</param>
        private static void AssertSignsNotNull(PacSignatureData serverSign, PacSignatureData kdcSign)
        {
            if (serverSign == null)
            {
                throw new ArgumentException("server signature is not found");
            }
            if (kdcSign == null)
            {
                throw new ArgumentException("KDC signature is not found");
            }
        }
        #endregion
    }
}
