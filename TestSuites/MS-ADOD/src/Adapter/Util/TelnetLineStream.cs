// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace KentCe.Sample
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading;    

    /// <summary>
    /// The TelnetLineStream class wraps a raw telnet data stream instance (socket or serial) and
    /// automatically filters/handles the telnet protocol data to present the caller with a clean
    /// command line style interface.
    /// </summary>
    /// <remarks>
    /// The TelnetLineStream class was written to handle the embedded telnet protocol data when
    /// interfacing with network hardware over their command line interface (CLI).  All telnet
    /// protocol information is automatically filtered out and the appropriate responses returned
    /// to the remote endpoint.
    ///
    /// For VT102 screen support, a future TelnetFormStream is possible (depending on future
    /// requirements).
    /// </remarks>
    public class TelnetLineStream : Stream, IAsyncResult
    {
        // Telnet Control Codes.
        private const byte ControlIAC   = 0xFF; // (255) Interpret As Command.
        private const byte ControlDONT  = 0xFE; // (254) Request NOT To Do Option.
        private const byte ControlDO    = 0xFD; // (253) Request To Do Option .
        private const byte ControlWONT  = 0xFC; // (252) Refusal To Do Option.
        private const byte ControlWILL  = 0xFB; // (251) Desire / Confirm Will Do Option.
        private const byte ControlSB    = 0xFA; // (250) Start Subnegotiation. 
        private const byte ControlGA    = 0xF9; // (249) "Go Ahead" Function(you may reverse 
                                                // the line) The line turn-around signal for 
                                                // half-duplex data transfer.
        private const byte ControlEL    = 0xF8; // (248) Requests that the previous line (from
                                                // the current character back to the last 
                                                // newline) be erased from the data stream.
        private const byte ControlEC    = 0xF7; // (247) Requests that the previous character 
                                                // be erased from the data stream.
        private const byte ControlAYT   = 0xF6; // (246) "Are You There?" function requests a 
                                                // visible or audible signal that the remote 
                                                // side is still operating.
        private const byte ControlAO    = 0xF5; // (245) Requests that the current user process 
                                                // be be allowed to run to completion, but that
                                                // no more output be sent to the NVT "printer".
        private const byte ControlIP    = 0xF4; // (244) Requests that the current user process 
                                                // be interrupted permanently .
        private const byte ControlBRK   = 0xF3; // (243) NVT character BRK. This code is to 
                                                // provide a signal outside the ASCII character 
                                                // set to indicate the Break or Attention 
                                                // signal available on many systems.
        private const byte ControlDM    = 0xF2; // (242) Data Mark ( for Sync ). A Stream 
                                                // synchronizing character for use with the 
                                                // Sync signal.
        private const byte ControlNOP   = 0xF1; // (241) No Operation.
        private const byte ControlSE    = 0xF0; // (240) End Of Subnegotiation.

        // Telnet option codes.       
        private const byte OptionECHO     = 1;  // Echo (RFC-857)
        private const byte OptionSOA      = 3;  // Suppress Go Ahead (RFC-858)
        private const byte OptionSTATUS   = 5;  // Status (RFC-859)
        private const byte OptionTERMTYPE = 24; // Terminal Type (RFC-1091)
        private const byte OptionNAWS     = 31; // Window size (RFC-1073)

        // Reference to telnet communication stream to the device to wrap with this instance.
        private readonly Stream telnetStream = null;
        
        // Current parsing state of the telnet input stream.
        private State currentState = State.DATA;

        // Current telnet option (only valid in State.OPTION).
        private int currentOption;

        // On an asynchronous read the caller parameters are saved to the below callerXxx
        // members to permit proxying the I/O to the raw telnet source.  After filtering of telnet
        // data the "clean" data is returned to the caller.
        private byte[] callerBuffer;
        private int callerOffset;
        private int callerSize;
        private AsyncCallback callerCallback;
        private object callerState;

        // Results of the caller async read operation which is the remaining count of bytes after
        // the telnet protocol information has been filtered.
        private int callerResults = 0;
        
        // Indicates whether the read I/O has completed (data available) or not.  Used to support 
        // the IAsyncResult interface.
        private bool completed = false;

        // Delayed created event used by the caller to wait for the async I/O to complete.  Used
        // to support the IAsyncResult interface.
        private ManualResetEvent manualResetEvent = null;

        // IAsyncResult for current read I/O to the telnet stream.
        private IAsyncResult telnetAsyncResult = null;

        // Cache of read callback method to the telnet stream.
        private AsyncCallback telnetCallback = null;

        /// <summary>
        /// Initializes a new instance of the TelnetStream class with the telnetStream parameter.
        /// </summary>
        /// <param name="telnetStream">Specifies the stream used for communication with the telnet device.</param>
        /// <remarks>
        /// Assumption that the read and send timeout values have been set on the telnetStream 
        /// instance by the caller.
        /// </remarks>
        public TelnetLineStream(Stream telnetStream)
        {
            this.telnetStream = telnetStream;
        }
        
        /// <summary>
        /// Internal telnet control code parsing states. 
        /// </summary>
        private enum State
        {
            /// <summary>
            /// Normal data processing (initial state).
            /// </summary>
            DATA,

            /// <summary>
            /// Have seen IAC
            /// </summary>
            IAC,

            /// <summary>
            /// Have seen IAC- { DO | DONT | WILL | WONT } Option
            /// </summary>
            OPTION,

            /// <summary>
            /// Have seen IAC-SB
            /// </summary>
            SUBNEG,

            /// <summary>
            /// Have seen IAC-SB-...-IAC
            /// </summary>
            SUBIAC
        }
        
        /// <summary>
        /// Gets an user-defined object that qualifies or contains information about an 
        /// asynchronous operation.
        /// </summary>
        object IAsyncResult.AsyncState 
        {
            get { return this.callerState; }
        }

        /// <summary>
        /// Gets a WaitHandle that is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <remarks>
        /// The creation of the WaitHandle is delayed to reduce the overhead of creating a Win32
        /// event handle.
        /// </remarks>
        WaitHandle IAsyncResult.AsyncWaitHandle 
        {
            get 
            {
                if (this.manualResetEvent == null)
                {
                    lock (this)
                    {
                        if (this.manualResetEvent == null)
                        {
                            this.manualResetEvent = new ManualResetEvent(this.completed);
                        }
                    }
                }

                return this.manualResetEvent;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the TelnetLineStream supports reading. This property
        /// always returns true.
        /// </summary>
        /// <value>
        /// true to indicate that the TelnetLineStream supports reading.
        /// </value>
        /// <remarks>
        /// The requirement of the stream wrapped by TelnetLineStream is supporting reading and
        /// writing.
        /// </remarks>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the stream supports seeking. This property always 
        /// returns false.
        /// </summary>
        /// <value>
        /// false to indicate that TelnetLineStream cannot seek a specific location in the stream.
        /// </value>
        /// <remarks>
        /// This property is not currently supported and will always return false.
        /// </remarks>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the TelnetLineStream supports writing. This property
        /// always returns true.
        /// </summary>
        /// <value>
        /// true to indicate that the TelnetLineStream supports writing.
        /// </value>
        /// <remarks>
        /// The requirement of the stream wrapped by TelnetLineStream is supporting reading and
        /// writing.
        /// </remarks>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// Gets an indication of whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <value>
        /// true if the asynchronous operation completed synchronously; otherwise, false.
        /// </value>
        bool IAsyncResult.CompletedSynchronously 
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an indication whether the asynchronous operation has completed.
        /// </summary>
        /// <value>
        /// true if the operation is complete; otherwise, false.
        /// </value>
        bool IAsyncResult.IsCompleted 
        {
            get { return this.completed; }
        }

        /// <summary>
        /// Gets the length of the data available on the stream. This property always throws a 
        /// NotSupportedException. 
        /// </summary>
        /// <value>
        /// The length of the data available on the stream. This property is not currently 
        /// supported, and will throw a NotSupportedException.
        /// </value>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the length of the data available on the stream. This property always throws a 
        /// NotSupportedException. 
        /// </summary>
        /// <value>
        /// The current position in the stream. This property is not currently supported, and will
        /// throw a NotSupportedException.
        /// </value>
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Begins an asynchronous read from a telnet session.  The telnet endpoint is assumed
        /// to be a command line interface (not screen mode) and telnet protocol data is removed
        /// from the data returned to the caller.
        /// </summary>
        /// <param name="buffer">
        /// The location in memory that stores the data from the stream.
        /// </param>
        /// <param name="offset">Specifies the location in buffer to begin storing the data to.</param>
        /// <param name="size">Specifies the size of buffer.</param>
        /// <param name="callback">Specifies the delegate to call when the async call is complete.</param>
        /// <param name="state">Specifies the object containing information supplied by the client.</param>
        /// <returns>An IAsyncResult representing the asynchronous call.</returns>
        public override IAsyncResult BeginRead(
            byte[] buffer, int offset, int size, AsyncCallback callback, Object state)
        {
            // Save the caller information while the proxy IO is performed with the telnet
            // endpoint.
            this.callerBuffer = buffer;
            this.callerOffset = offset;
            this.callerSize = size;
            this.callerCallback = callback;
            this.callerState = state;

            // Delay creation of the telnet endpoint callback delegate until the first read.
            if (this.telnetCallback == null)
            {
                this.telnetCallback = new AsyncCallback(this.telnetCallback);
            }

            // Reset state of previous IO.
            this.completed = false;
            if (this.manualResetEvent != null)
            {
                this.manualResetEvent.Reset();
            }

            // Issue a read to the telnet endpoint using the caller's memory buffer, but use
            // our own callback delegate to permit filtering of data before the caller reads it.
            this.telnetAsyncResult = this.telnetStream.BeginRead(
                this.callerBuffer, this.callerOffset, this.callerSize, this.telnetCallback, this);

            // The TelnetLineStream instance is also the IAsyncResult object, return ourselves.
            return this;
        }
        
        /// <summary>
        /// Handles the end of an asynchronous read.
        /// </summary>
        /// <param name="asyncResult">Specifies the result of an asynchronous call represented in IAsyncResult.</param>
        /// <returns>The number of bytes read from the stream.</returns>
        /// <remarks>
        /// The byte count is the result of the telnet protocol data which has been filtered out.
        /// If 0 is returned the stream was closed.  If internally all received bytes were
        /// removed, the TelnetLineStream class will reissue another read IO internal.
        /// </remarks>
        public override int EndRead(IAsyncResult asyncResult)
        {
            if (!this.completed)
            {
                ((IAsyncResult)this).AsyncWaitHandle.WaitOne();
            }

            return this.callerResults;
        }

        /// <summary>
        /// Clears all buffers for the internally wrapped telnet stream and causes any buffered 
        /// data to be written to the underlying device.
        /// </summary>
        /// <remarks>
        /// The TelnetLineStream does not buffer data and the Flush calls the wrapped telnet
        /// stream instance Flush method.
        /// </remarks>
        public override void Flush()
        {
            this.telnetStream.Flush();
        }

        /// <summary>
        /// Reads data from the stream with telnet protocol data removed.
        /// </summary>
        /// <param name="buffer">Specifies the location in memory to store data read from the stream.</param>
        /// <param name="offset">Specifies the location in the buffer to begin storing the data to.</param>
        /// <param name="count">Specifies the number of bytes to read from the stream.</param>
        /// <returns>Number of bytes read from the stream.</returns>
        /// <remarks>
        /// The byte count is the result of the telnet protocol data which has been filtered out.
        /// If 0 is returned the stream was closed.  If internally all received bytes were
        /// removed, the TelnetLineStream class will reissue another read IO internal.
        /// </remarks>
        public override int Read(byte[] buffer, int offset, int count) 
        {
            // To handled the scenario of filtering out all received data from the telnet
            // stream, keep looping until data is available to the caller.  Returning 0 should
            // only occur if the wrapped telnet stream returns 0.
            while (true)
            {
                int readBytes = this.telnetStream.Read(buffer, offset, count);
                if (readBytes == 0)
                {
                    return 0;
                }

                // Filter the raw data by compressing out the telnet protocol data and updating
                // the 'readBytes' value.  Any telnet response is returned.
                byte[] response = this.Filter(buffer, offset, ref readBytes);

                // If telnet protocol response was returned, send sync as this is a sync read.
                if (response.Length > 0)
                {
                    this.telnetStream.Write(response, 0, response.Length);
                }

                // If useful data remains, return to the caller.
                if (readBytes > 0)
                {
                    return readBytes;
                }
            }
        }

        /// <summary>
        /// Sets the current position of the stream to the given value. This method always throws
        /// a NotSupportedException.
        /// </summary>
        /// <param name="offset">Specifies a byte offset relative to the origin parameter.</param>
        /// <param name="origin">
        /// A value of type SeekOrigin indicating the reference point used to obtain the new 
        /// position.
        /// </param>
        /// <returns>
        /// The position in the stream. This method is not currently supported, and will throw a 
        /// NotSupportedException.
        /// </returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the length of the stream. This method always throws a NotSupportedException.
        /// </summary>
        /// <param name="value">This parameter is not used.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes data to the stream.
        /// </summary>
        /// <param name="buffer">Specifies the data to write to the stream.</param>
        /// <param name="offset">Specifies the location in the buffer to start writing data from.</param>
        /// <param name="count">Specifies the number of bytes to write to the stream.</param>
        public override void Write(byte[] buffer, int offset, int count) 
        {
            this.telnetStream.Write(buffer, offset, count);
        }

        //-----------------------------------------------------------------------------------------
        //
        //  Private methods below this point.
        //
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// The data received will be passed through a telnet filter to handle telnet specific 
        /// protocol data, then the remaining non-telnet data is returned to the caller.  If no
        /// data is remaining, the original read is submitted again to prevent the caller from
        /// seeing bogus 0 bytes return values.
        /// </summary>
        /// <param name="asyncResult">Specifies the reference to the pending asynchronous read.</param>
        private void ReadCallback(IAsyncResult asyncResult)
        {
            int readBytes = this.telnetStream.EndRead(asyncResult);
            if (readBytes == 0)
            {
                this.Complete(0);
            }

            // Filter the raw data by compressing out the telnet protocol data and updating
            // the 'readBytes' value.  Any telnet response is returned.
            byte[] response = this.Filter(this.callerBuffer, this.callerOffset, ref readBytes);

            // If telnet protocol response was returned, send to the telnet endpoint.
            if (response.Length > 0)
            {
                // A sync write is performed because very little telnet protocol data is 
                // generated and only at the beginning of a session.
                this.telnetStream.Write(response, 0, response.Length);
            }

            // If all received bytes were was filtered out by the telnet protocol, issue another
            // read for more useful data.  
            if (readBytes == 0)
            {
                this.telnetAsyncResult = this.telnetStream.BeginRead(
                    this.callerBuffer, 
                    this.callerOffset, 
                    this.callerSize, 
                    this.telnetCallback, 
                    this);
            }
            else
            {
                this.Complete(readBytes);
            }
        }

        /// <summary>
        /// Complete the async read operation.
        /// </summary>
        /// <param name="bytes">Number of bytes.</param>
        private void Complete(int bytes)
        {
            lock (this)
            {
                this.callerResults = bytes;

                this.completed = true;

                if (this.manualResetEvent != null)
                {
                    this.manualResetEvent.Set();
                }
            }

            if (this.callerCallback != null)
            {
                this.callerCallback(this);
            }
        }

        /// <summary>
        /// Given an 'input' string containing telnet data and control codes, filter out all pure
        /// data and return.  Generated telnet protocol responses that must be sent to the telnet
        /// remote session are stored in 'response'.
        ///
        /// The remaining non-telnet data is compressed in the input buffer and the length value
        /// updated to reflect the new length.
        /// </summary>
        /// <param name="input">Specifies the storage location for the raw telnet/user data.</param>
        /// <param name="offset">Specifies the zero-based position at which the data was stored.</param>
        /// <param name="length">Specifies the number of bytes to filter.</param>
        /// <returns>Byte array of the telnet protocol responses.</returns>
        private byte[] Filter(byte[] input, int offset, ref int length)
        {
            // The typical case is no embedded telnet data so optimize for it.
            if (this.currentState == State.DATA)
            {
                bool telnetFound = false;
                for (int i = offset; i < length; i++)
                {
                    if (input[i] == ControlIAC)
                    {
                        telnetFound = true;
                        break;
                    }
                }

                if (!telnetFound)
                {
                    return new byte[0];
                }
            }

            // Got to do real work, allocate the result stream.
            MemoryStream responseStream = new MemoryStream();

            // The dataIndex reflects the next location in the input buffer to stored the
            // filtered non-telnet data.  Saves an extra memory allocation.
            int dataIndex = 0;

            // Telnet parser state machine.
            for (int i = offset; i < length; i++)
            {
                byte data = input[i];

                switch (this.currentState)
                {
                    case State.DATA:
                        switch (data)
                        {
                            case ControlIAC:
                                this.currentState = State.IAC;
                                break;

                            default:
                                input[dataIndex++] = data;
                                break;
                        }
                        break;

                    case State.IAC:
                        switch (data)
                        {
                            case ControlDONT:
                            case ControlDO:
                            case ControlWONT:
                            case ControlWILL:
                                this.currentOption = data;
                                this.currentState = State.OPTION;
                                break;

                            default:                                
                                throw new ApplicationException(String.Format(CultureInfo.InvariantCulture, 
                                    "Invalid telnet option code '{0}'.", data));
                        }
                        break;

                    case State.OPTION:
                        switch (data)
                        {
                            case OptionSOA:
                                this.WriteDoOption(responseStream, OptionSOA);
                                this.currentState = State.DATA;
                                break;

                            default:
                                this.WriteWontOption(responseStream, data);
                                this.currentState = State.DATA;
                                break;
                        }
                        break;

                    default:                        
                        throw new ApplicationException("Unhandled state condition.");
                }
            }

            // As the input buffer was compressed, update the length (ref).
            Debug.Assert(length != dataIndex, "Should find telnet protocol data.");
            length = dataIndex;

            // Return the bytes to transmit to the telnet endpoint.
            return responseStream.ToArray();
        }

        /// <summary>
        /// Write out a DO response to an option request from the sender.
        /// </summary>
        /// <param name="output">Specifies the stream to write out the response message.</param>
        /// <param name="option">Option to 'DO'.</param>
        private void WriteDoOption(Stream output, byte option)
        {
            output.WriteByte(ControlIAC);
            output.WriteByte(ControlDO);
            output.WriteByte(option);
        }

        /// <summary>
        /// Write out a WONT response to an option request from the sender.
        /// </summary>
        /// <param name="output">Specifies the stream to write out the response message.</param>
        /// <param name="option">Option to 'WONT'.</param>
        private void WriteWontOption(Stream output, byte option)
        {
            output.WriteByte(ControlIAC);
            output.WriteByte(ControlWONT);
            output.WriteByte(option);
        }        
  }
}