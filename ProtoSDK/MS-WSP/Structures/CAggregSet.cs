// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CAggregSet structure contains information about aggregates.
    /// Aggregate is a database concept for a field calculated using the information retrieved from the query.
    /// The different aggregates that are supported by GSS are defined in the CAggregSpec's type field (specified in section 2.2.1.25).
    /// </summary>
    public struct CAggregSet : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the number of entries in AggregSpecs.
        /// </summary>
        public uint cCount;

        /// <summary>
        /// An array of CAggregSpec structures, each describing individual aggregation.
        /// </summary>
        public CAggregSpec[] AggregSpecs;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(cCount);

            if (cCount != 0)
            {
                foreach (var spec in AggregSpecs)
                {
                    buffer.AlignWrite(4);
                    spec.ToBytes(buffer);
                }
            }
        }
    }
}
