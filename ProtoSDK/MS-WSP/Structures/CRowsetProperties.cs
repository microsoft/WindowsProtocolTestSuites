// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CRowsetProperties_uBooleanOptions_Values : uint
    {
        /// <summary>
        /// The cursor can only be moved forward.
        /// </summary>
        eSequential = 0x00000001,

        /// <summary>
        /// The cursor can be moved to any position.
        /// </summary>
        eLocatable = 0x00000003,

        /// <summary>
        /// The cursor can be moved to any position and fetch in any direction.
        /// </summary>
        eScrollable = 0x00000007,

        /// <summary>
        /// The client will not wait for execution completion.
        /// </summary>
        eAsynchronous = 0x00000008,

        /// <summary>
        /// Return the first rows encountered, not the best matches.
        /// </summary>
        eFirstRows = 0x00000080,

        /// <summary>
        /// The server MUST NOT discard rows until the client is done with a query.
        /// </summary>
        eHoldRows = 0x00000200,

        /// <summary>
        /// The rowset supports chapters.
        /// </summary>
        eChaptered = 0x00000800,

        /// <summary>
        /// Use the inverted index to evaluate content restrictions even if it is out of date. If not set, the GSS can opt to execute the query by going directly against the file system.
        /// </summary>
        eUseCI = 0x00001000,

        /// <summary>
        /// Non-indexed trimming operations like scoping or security checking can be expensive. This option gives the GSS the option of deferring these operations until rows are actually requested.
        /// </summary>
        eDeferTrimming = 0x00002000,

        /// <summary>
        /// Enables storage of rowset events on the server side. (For information about how to retrieve stored events, see the CPMGetRowsetNotifyIn message.)
        /// </summary>
        eEnableRowsetEvents = 0x00800000,

        /// <summary>
        /// Prevents computation of expensive properties.
        /// Windows implementations treat cRowsTotal, _maxRank, and _cResultsFound (as specified in CPMGetQueryStatusExOut (section 2.2.3.9)) as expensive properties.
        /// Other implementations could choose different properties and mark them as expensive. 
        /// </summary>
        eDoNotComputeExpensiveProps = 0x00400000,
    }

    /// <summary>
    /// The CRowsetProperties structure contains configuration information for a query.
    /// </summary>
    public struct CRowsetProperties : IWspStructure
    {
        /// <summary>
        /// _uBooleanOptions
        /// </summary>
        public CRowsetProperties_uBooleanOptions_Values _uBooleanOptions;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// Note This field MUST be set to 0x00000000. It is not used and MUST be ignored.
        /// </summary>
        public uint _ulMaxOpenRows;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// Note This field MUST be set to 0x00000000. It is not used and MUST be ignored.
        /// </summary>
        public uint _ulMemoryUsage;

        /// <summary>
        /// A 32-bit unsigned integer specifying the maximum number of rows that are to be returned for the query.
        /// </summary>
        public uint _cMaxResults;

        /// <summary>
        /// A 32-bit unsigned integer, specifying the number of seconds at which a query is to time out and automatically terminate, counting from the time the query starts executing on the server.
        /// Note A value of 0x00000000 means that the query is not to time out.
        /// </summary>
        public uint _cCmdTimeout;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
