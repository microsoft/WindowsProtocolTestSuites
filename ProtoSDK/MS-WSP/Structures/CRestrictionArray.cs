// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CRestrictionArray structure contains an array of restriction nodes.
    /// </summary>
    public struct CRestrictionArray : IWspStructure
    {
        /// <summary>
        /// An 8-bit unsigned integer specifying the number of CRestriction records contained in the Restriction field.
        /// This field MUST be set to 0x01.
        /// </summary>
        public byte count;

        /// <summary>
        /// An 8-bit unsigned integer. MUST be set to one of the following values.
        /// 0x00	The _padding and Restriction fields are omitted.
        /// 0x01	The _padding and Restriction fields are present.
        /// </summary>
        public byte isPresent;

        /// <summary>
        /// A CRestriction structure, specifying a node of a query command tree.
        /// Note Restriction MUST be omitted if the value of isPresent is set to 0x00.
        /// </summary>
        public CRestriction Restriction;

        public void FromBytes(WspBuffer buffer)
        {
            throw new System.NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(count);

            buffer.Add(isPresent);

            if (isPresent == 0x01)
            {
                buffer.AlignWrite(4);

                Restriction.ToBytes(buffer);
            }
        }
    }
}
