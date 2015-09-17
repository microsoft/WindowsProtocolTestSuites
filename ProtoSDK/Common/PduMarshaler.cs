// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// This abstract class is used as PDU's interface.
    /// </summary>
    public abstract class BasePDU
    {
        /// <summary>
        /// Encode this PDU to the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to encode the fields of this PDU.</param>
        public abstract void Encode(PduMarshaler marshaler);

        /// <summary>
        /// Decode this PDU from the PduMarshaler.
        /// </summary>
        /// <param name="marshaler">This is used to decode the fields of this PDU.</param>
        public abstract bool Decode(PduMarshaler marshaler);

        /// <summary>
        /// Convert the fields of this PDU to a string.
        /// </summary>
        /// <returns>A string contains the fields of this PDU.</returns>
        public override string ToString()
        {
            return "BasePDU";
        }
    }

    /// <summary>
    /// The class PduMarshaler is used to help to marshal a PDU to a bytes array and to unmarshal a PDU from a bytes array.
    /// </summary>
    public class PduMarshaler : IDisposable
    {
        #region Protected Fileds

        /// <summary>
        /// The memory stream to store the bytes array.
        /// </summary>
        private MemoryStream stream = null;

        /// <summary>
        /// Indicates whether to decode or encode the numbers in little endian.
        /// </summary>
        private bool isLittleEndian;

        /// <summary>
        /// Constructor to write to the bytes array.
        /// </summary>
        private PduMarshaler()
        {
            stream = new MemoryStream();
            isLittleEndian = BitConverter.IsLittleEndian; //Align the default value with system
        }

        /// <summary>
        /// Constructor to write to the bytes array.
        /// </summary>
        /// <param name="isLitEndian">Set to true if encode numbers to Little Endian, false if Big Endian</param>
        private PduMarshaler(bool isLitEndian)
        {
            stream = new MemoryStream();
            isLittleEndian = isLitEndian;
        }

        /// <summary>
        /// Constructor to read from the bytes array.
        /// </summary>
        /// <param name="data">The byte data to be read.</param>
        private PduMarshaler(byte[] data)
        {
            stream = new MemoryStream(data);
            isLittleEndian = BitConverter.IsLittleEndian; //Align the default value with system
        }

        /// <summary>
        /// Constructor to read from the bytes array.
        /// </summary>
        /// <param name="data">The byte data to be read.</param>
        /// <param name="isLitEndian">Set to true if decode numbers to Little Endian, false if Big Endian</param>
        private PduMarshaler(byte[] data, bool isLitEndian)
        {
            stream = new MemoryStream(data);
            isLittleEndian = isLitEndian;
        }

        /// <summary>
        /// Destruct this instance.
        /// </summary>
        ~PduMarshaler()
        {
            Dispose(false);
        }

        /// <summary>
        /// Return the bytes array.
        /// </summary>
        public byte[] RawData
        {
            get
            {
                if (null == stream)
                {
                    return null;
                }
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Set the position to the begin of the bytes array.
        /// </summary>
        public void Reset()
        {
            if (null != stream)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
        }

        #endregion

        #region Help Members
        /// <summary>
        /// Marshal the PDU structure to bytes array.
        /// </summary>
        /// <param name="pdu">The PDU to marshal.</param>
        /// <returns>The marshaled byte array</returns>
        public static byte[] Marshal(BasePDU pdu)
        {
            PduMarshaler m = new PduMarshaler();
            pdu.Encode(m);
            return m.RawData;
        }

        /// <summary>
        /// Marshal the PDU structure to bytes array.
        /// </summary>
        /// <param name="pdu">The PDU to marshal.</param>
        /// <param name="bLittleEndian">Indicates whether encode the numbers to little endian.</param>
        /// <returns>The marshaled byte array</returns>
        public static byte[] Marshal(BasePDU pdu, bool bLittleEndian)
        {
            PduMarshaler m = new PduMarshaler(bLittleEndian);
            pdu.Encode(m);
            return m.RawData;
        }

        /// <summary>
        /// Unmarshal bytes array to the PDU structure.
        /// </summary>
        /// <param name="data">The bytes array to unmarshal.</param>
        /// <param name="pdu">The PDU to be filled.</param>
        /// <returns>The PDU</returns>
        public static bool Unmarshal(byte[] data, BasePDU pdu)
        {
            PduMarshaler m = new PduMarshaler(data);
            return pdu.Decode(m);
        }

        /// <summary>
        /// Unmarshal bytes array to the PDU structure.
        /// </summary>
        /// <param name="data">The bytes array to unmarshal.</param>
        /// <param name="pdu">The PDU to be filled.</param>
        /// <param name="bLittleEndian">Indicates whether encode the numbers to little endian.</param>
        /// <returns>The PDU</returns>
        public static bool Unmarshal(byte[] data, BasePDU pdu, bool bLittleEndian)
        {
            PduMarshaler m = new PduMarshaler(data, bLittleEndian);
            return pdu.Decode(m);
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Indicates whether the instance is disposed
        /// </summary>
        protected bool disposed = false;

        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// For derived classes to release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (stream != null)
                    {
                        stream.Dispose();
                    }
                }

                //Note disposing has been done.
                disposed = true;
            }
        }

        #endregion

        #region Write Members

        /// <summary>
        /// Writes a byte to the bytes array at the current position.
        /// </summary>
        /// <param name="data">The byte to write.</param>
        public void WriteByte(byte data)
        {
            stream.WriteByte(data);
        }

        /// <summary>
        /// Writes a block of bytes to the bytes array using data read from data.
        /// </summary>
        /// <param name="data">The data to write data from.</param>
        public void WriteBytes(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
        
        /// <summary>
        /// Writes a block of bytes to the bytes array using data read from data.
        /// </summary>
        /// <param name="data">The data to write data from.</param>
        /// <param name="offset">The byte offset in data at which to begin writing from.</param>
        /// <param name="len">The maximum number of bytes to write.</param>
        public void WriteBytes(byte[] data, int offset, int len)
        {
            stream.Write(data, offset, len);
        }

        /// <summary>
        /// Writes a block of bytes which are converted from a numeric to the bytes array using data read from data.
        /// This method can autoconvert the byte order depending on the architecture in this machine.
        /// </summary>
        /// <param name="data">The data to write data from.</param>
        public void WriteBytesNumeric(byte[] data)
        {
            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                Array.Reverse(data);
            }
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Writes a bool to the bytes array.
        /// </summary>
        /// <param name="data">The bool to write.</param>
        public void WriteBool(bool data)
        {
            WriteBytes(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a char to the bytes array.
        /// </summary>
        /// <param name="data">The char to write.</param>
        public void WriteChar(char data)
        {
            WriteBytes(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a double to the bytes array.
        /// </summary>
        /// <param name="data">The double to write.</param>
        public void WriteDouble(double data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a float to the bytes array.
        /// </summary>
        /// <param name="data">The float to write.</param>
        public void WriteFloat(float data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a short to the bytes array.
        /// </summary>
        /// <param name="data">The short to write.</param>
        public void WriteInt16(short data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes an int to the bytes array.
        /// </summary>
        /// <param name="data">The int to write.</param>
        public void WriteInt32(int data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a long to the bytes array.
        /// </summary>
        /// <param name="data">The long to write.</param>
        public void WriteInt64(long data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a ushort to the bytes array.
        /// </summary>
        /// <param name="data">The ushort to write.</param>
        public void WriteUInt16(ushort data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a uint to the bytes array.
        /// </summary>
        /// <param name="data">The uint to write.</param>
        public void WriteUInt32(uint data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a ulong to the bytes array.
        /// </summary>
        /// <param name="data">The ulong to write.</param>
        public void WriteUInt64(ulong data)
        {
            WriteBytesNumeric(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes an ASCII string to the bytes array.
        /// </summary>
        /// <param name="data">The ASCII string to write.</param>
        public void WriteASCIIString(string data)
        {
            WriteBytes(Encoding.ASCII.GetBytes(data));
            stream.WriteByte(0);
        }

        /// <summary>
        /// Writes a Unicode string to the bytes array.
        /// </summary>
        /// <param name="data">The Unicode string to write.</param>
        public void WriteUnicodeString(string data)
        {
            WriteBytes(Encoding.Unicode.GetBytes(data));
            stream.WriteByte(0);
            stream.WriteByte(0);
        }

        #endregion

        #region Read Members

        /// <summary>
        /// Reads a byte from the bytes array.
        /// </summary>
        /// <returns>The byte to read.</returns>
        public byte ReadByte()
        {
            return Convert.ToByte(stream.ReadByte());
        }

        /// <summary>
        /// Reads a block of bytes from the bytes array.
        /// </summary>
        /// <param name="len">The maximum number of bytes to read.</param>
        /// <returns>The specified byte array with the values between current position and (current position + len - 1).</returns>
        public byte[] ReadBytes(int len)
        {
            byte[] b = new byte[len];
            stream.Read(b, 0, len);

            return b;
        }

        /// <summary>
        /// Reads a block of bytes which is a numeric from the bytes array.
        /// </summary>
        /// <param name="len">The maximum number of bytes to read.</param>
        /// <returns>The specified byte array with the values between current position and (current position + len - 1).</returns>
        public byte[] ReadBytesNumeric(int len)
        {
            byte[] b = new byte[len];
            stream.Read(b, 0, len);

            if (BitConverter.IsLittleEndian != isLittleEndian)
            {
                Array.Reverse(b);
            }

            return b;
        }

        /// <summary>
        /// Reads a block of bytes from the bytes array between current position and the end.
        /// </summary>
        /// <returns>The specified byte array with the values between current position and the end.</returns>
        public byte[] ReadToEnd()
        {
            long len = stream.Length - stream.Position;
            if (len <= 0)
            {
                return null;
            }

            return ReadBytes((int)len);
        }

        /// <summary>
        /// Reads a bool from the bytes array.
        /// </summary>
        /// <returns>The bool to read.</returns>
        public bool ReadBool()
        {
            return BitConverter.ToBoolean(ReadBytes(1), 0);
        }

        /// <summary>
        /// Reads a char from the bytes array.
        /// </summary>
        /// <returns>The char to read.</returns>
        public char ReadChar()
        {
            return BitConverter.ToChar(ReadBytes(2), 0);
        }

        /// <summary>
        /// Reads a double from the bytes array.
        /// </summary>
        /// <returns>The double to read.</returns>
        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBytesNumeric(8), 0);
        }

        /// <summary>
        /// Reads a float from the bytes array.
        /// </summary>
        /// <returns>The float to read.</returns>
        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBytesNumeric(4), 0);
        }

        /// <summary>
        /// Reads a short from the bytes array.
        /// </summary>
        /// <returns>The short to read.</returns>
        public short ReadInt16()
        {
            return BitConverter.ToInt16(ReadBytesNumeric(2), 0);
        }

        /// <summary>
        /// Reads an int from the bytes array.
        /// </summary>
        /// <returns>The int to read.</returns>
        public int ReadInt32()
        {
            return BitConverter.ToInt32(ReadBytesNumeric(4), 0);
        }

        /// <summary>
        /// Reads a long from the bytes array.
        /// </summary>
        /// <returns>The long to read.</returns>
        public long ReadInt64()
        {
            return BitConverter.ToInt64(ReadBytesNumeric(8), 0);
        }

        /// <summary>
        /// Reads a ushort from the bytes array.
        /// </summary>
        /// <returns>The ushort to read.</returns>
        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadBytesNumeric(2), 0);
        }

        /// <summary>
        /// Reads a uint from the bytes array.
        /// </summary>
        /// <returns>The uint to read.</returns>
        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadBytesNumeric(4), 0);
        }

        /// <summary>
        /// Reads a ulong from the bytes array.
        /// </summary>
        /// <returns>The ulong to read.</returns>
        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadBytesNumeric(8), 0);
        }

        /// <summary>
        /// Reads an ASCII string from the bytes array.
        /// </summary>
        /// <returns>The ASCII string to read.</returns>
        public string ReadASCIIString()
        {
            return Encoding.ASCII.GetString(ReadToEnd());
        }

        /// <summary>
        /// Reads an ASCII string from the bytes array .
        /// </summary>
        /// <param name="len">The maximum number of bytes to read. It must be bigger than 1 
        /// because contains the length of null terminated.</param>
        /// <returns>The ASCII string to read.</returns>
        public string ReadASCIIString(int len)
        {
            if (len <= 1)
            {
                throw new ArgumentException("The length must be bigger than 1.");
            }
            return Encoding.ASCII.GetString(ReadBytes(len), 0, len - 1);
        }

        /// <summary>
        /// Reads a Unicode string from the bytes array.
        /// </summary>
        /// <returns>The Unicode string to read.</returns>
        public string ReadUnicodeString()
        {
            return Encoding.Unicode.GetString(ReadToEnd());
        }

        /// <summary>
        /// Reads a Unicode string from the bytes array .
        /// </summary>
        /// <param name="len">The maximum number of bytes to read. It contains the length of null terminated.</param>
        /// <returns>The Unicode string to read.</returns>
        public string ReadUnicodeString(int len)
        {
            if (len <= 1)
            {
                throw new ArgumentException("The length must be bigger than 1.");
            }
            try
            {
                return Encoding.Unicode.GetString(ReadBytes(checked(2 * len)), 0, checked(2 * (len - 1)));
            }
            catch (System.OverflowException)
            {
               throw new ArgumentException("The length is too big so that an OverflowException is thrown.");
            }
        }

        #endregion
    }
}