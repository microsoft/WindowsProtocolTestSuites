// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    [Flags]
    public enum CPMCiState_eState_Values : uint
    {
        /// <summary>
        /// None of the following states apply.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The GSS is in the process of optimizing some of the indexes to reduce memory usage and improve query performance.
        /// </summary>
        CI_STATE_SHADOW_MERGE = 0x00000001,

        /// <summary>
        /// The GSS is in the process of full optimization for all indexes.
        /// </summary>
        CI_STATE_MASTER_MERGE = 0x00000002,

        /// <summary>
        /// Some documents in the inverted index have changed and the GSS needs to determine which have been added, changed, or deleted.
        /// </summary>
        CI_STATE_CONTENT_SCAN_REQUIRED = 0x00000004,

        /// <summary>
        /// The GSS is in the process of optimizing indexes to reduce memory usage and improve query performance.
        /// This process is more comprehensive than the one identified by the CI_STATE_SHADOW_MERGE value, but it is not as comprehensive as specified by the CI_STATE_MASTER_MERGE value.
        /// Such optimizations are implementation-specific as they depend on the way data is stored internally; the optimizations do not affect the protocol in any way other than response time.
        /// </summary>
        CI_STATE_ANNEALING_MERGE = 0x00000008,

        /// <summary>
        /// The GSS is checking a directory or a set of directories to determine if any files have been added, deleted, or updated since the last time the directory was indexed.
        /// </summary>
        CI_STATE_SCANNING = 0x00000010,

        /// <summary>
        /// Most of the virtual memory of the server is in use.
        /// </summary>
        CI_STATE_LOW_MEMORY = 0x00000080,

        /// <summary>
        /// The level of input/output(I/O) activity on the server is relatively high.
        /// </summary>
        CI_STATE_HIGH_IO = 0x00000100,

        /// <summary>
        /// The process of full optimization for all indexes that was in progress has been paused. This is given for informative purposes only and does not affect the Windows Search Protocol.
        /// </summary>
        CI_STATE_MASTER_MERGE_PAUSED = 0x00000200,

        /// <summary>
        /// The portion of the GSS that picks up new documents to index has been paused.This is given for informative purposes only and does not affect the Windows Search Protocol.
        /// </summary>
        CI_STATE_READ_ONLY = 0x00000400,

        /// <summary>
        /// The portion of the GSS that picks up new documents to index has been paused to conserve battery lifetime but still replies to the queries.
        /// This is given for informative purposes only and does not affect the Windows Search Protocol.
        /// </summary>
        CI_STATE_BATTERY_POWER = 0x00000800,

        /// <summary>
        /// The portion of the GSS that picks up new documents to index has been paused due to high activity by the user(keyboard or mouse) but still replies to the queries.
        /// This is given for informative purposes only and does not affect the Windows Search Protocol.
        /// </summary>
        CI_STATE_USER_ACTIVE = 0x00001000,

        /// <summary>
        /// The service is paused due to low disk availability.
        /// </summary>
        CI_STATE_LOW_DISK = 0x00010000,

        /// <summary>
        /// The service is paused due to high CPU usage.
        /// </summary>
        CI_STATE_HIGH_CPU = 0x00020000,
    }

    public struct CPMCiState
    {
        /// <summary>
        /// A 32-bit unsigned integer indicating the size, in bytes, of this message (excluding the common header). MUST be set to 0x0000003C.
        /// </summary>
        public uint cbStruct;

        /// <summary>
        /// A 32-bit unsigned integer containing the number of in-memory indexes created for recently indexed documents.
        /// </summary>
        public uint cWordList;

        /// <summary>
        /// A 32-bit unsigned integer containing the number of persisted indexes.
        /// </summary>
        public uint cPersistentIndex;

        /// <summary>
        /// A 32-bit unsigned integer indicating a number of actively running queries.
        /// </summary>
        public uint cQueries;

        /// <summary>
        /// A 32-bit unsigned integer indicating the total number of documents waiting to be indexed.
        /// </summary>
        public uint cDocuments;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of unique documents with information in indexes that are not fully optimized for performance.
        /// </summary>
        public uint cFreshTest;

        /// <summary>
        /// A 32-bit unsigned integer specifying the completion percentage of current full optimization of indexes while optimization is in progress.MUST be less than or equal to 100.
        /// </summary>
        public uint dwMergeProgress;

        /// <summary>
        /// A 32-bit unsigned integer indicating the state of content indexing.MUST be zero or one or more of the CI_STATE_* constants defined in the following table.
        /// </summary>
        public CPMCiState_eState_Values eState;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of documents indexed since content indexing began.
        /// </summary>
        public uint cFilteredDocuments;

        /// <summary>
        /// A 32-bit unsigned integer indicating the total number of documents in the system.
        /// </summary>
        public uint cTotalDocuments;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of pending high-level indexing operations.
        /// The meaning of this value is provider-specific, but larger numbers are expected to indicate that more indexing remains.
        /// </summary>
        public uint cPendingScans;

        /// <summary>
        /// A 32-bit unsigned integer indicating the size, in megabytes, of the index (excluding the property cache).
        /// </summary>
        public uint dwIndexSize;

        /// <summary>
        /// A 32-bit unsigned integer indicating the approximate number of unique keys in the catalog.
        /// </summary>
        public uint cUniqueKeys;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of documents that the GSS will attempt to index again because of a failure during the initial indexing attempt.
        /// </summary>
        public uint cSecQDocuments;

        /// <summary>
        /// A 32-bit unsigned integer indicating the size, in megabytes, of the property cache.
        /// </summary>
        public uint dwPropCacheSize;
    }

    /// <summary>
    /// The CPMCiStateInOut message contains information about the state of the GSS.
    /// </summary>
    public class CPMCiStateInOut : IWspInMessage, IWspOutMessage
    {
        public CPMCiState State;

        public WspMessageHeader Header { get; set; }

        public IWspInMessage Request { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            var header = new WspMessageHeader();

            header.FromBytes(buffer);

            Header = header;

            State = buffer.ToStruct<CPMCiState>();
        }

        public void ToBytes(WspBuffer buffer)
        {
            Header.ToBytes(buffer);

            buffer.Add(State);
        }
    }
}
