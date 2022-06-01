// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CInternalPropertyRestriction structure contains a property value to match with an operation.
    /// </summary>
    public struct CInternalPropertyRestriction : IWspRestriction
    {
        /// <summary>
        /// A 32-bit integer specifying the relation to perform on the property.
        /// </summary>
        public CPropertyRestriction_relop_Values _relop;

        /// <summary>
        /// A 32-bit unsigned integer, representing the property ID.
        /// </summary>
        public uint _pid;

        /// <summary>
        /// A CBaseStorageVariant that contains the value to relate to the property.
        /// </summary>
        public CBaseStorageVariant _prval;

        /// <summary>
        /// A 32-bit unsigned integer, indicating the locale of a string, contained in _prval value, as specified in [MS-LCID]. 
        /// </summary>
        public uint _lcid;

        /// <summary>
        /// A byte value. MUST be set to one of the following values.
        /// 0x00	restrictionPresent indicates that the nextRestriction field is not present.
        /// 0x01	restrictionPresent indicates that the nextRestriction field is present.
        /// </summary>
        public byte restrictionPresent;

        /// <summary>
        /// A CRestriction structure specifying a further restriction.
        /// </summary>
        public CRestriction nextRestriction;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_relop);

            buffer.Add(_pid);

            _prval.ToBytes(buffer);

            buffer.Add(_lcid, 4);

            buffer.Add(restrictionPresent);

            if (restrictionPresent == 0x01)
            {
                nextRestriction.ToBytes(buffer);
            }
        }
    }
}
