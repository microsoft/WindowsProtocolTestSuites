// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// A 32-bit integer specifying the relation to perform on the property. 
    /// </summary>
    public enum CPropertyRestriction_relop_Values : uint
    {
        /// <summary>
        /// A less-than comparison.
        /// </summary>
        PRLT = 0x00000000,

        /// <summary>
        /// A less-than or equal-to comparison.
        /// </summary>
        PRLE = 0x00000001,

        /// <summary>
        /// A greater-than comparison.
        /// </summary>
        PRGT = 0x00000002,

        /// <summary>
        /// A greater-than or equal-to comparison.
        /// </summary>
        PRGE = 0x00000003,

        /// <summary>
        /// An equality comparison.
        /// </summary>
        PREQ = 0x00000004,

        /// <summary>
        /// A not-equal comparison.
        /// </summary>
        PRNE = 0x00000005,

        /// <summary>
        /// A regular expression comparison.
        /// </summary>
        PRRE = 0x00000006,

        /// <summary>
        /// A bitwise AND that returns the value equal to _prval.
        /// </summary>
        PRAllBits = 0x00000007,

        /// <summary>
        /// A bitwise AND that returns a nonzero value.
        /// </summary>
        PRSomeBits = 0x00000008,

        /// <summary>
        /// The restriction is true if every element in a property value has the relationship with some element in the _prval field.
        /// </summary>
        PRAll = 0x00000100,

        /// <summary>
        /// The restriction is true if any element in the property value has the relationship with some element in the _prval field.
        /// </summary>
        PRAny = 0x00000200,
    }

    /// <summary>
    /// The CPropertyRestriction structure contains a property to get from each row, a comparison operator and a constant.
    /// For each row, the value returned by the specific property in the row is compared against the constant to determine if it has the relationship specified by the _relop field.
    /// For the comparison to be true, the datatypes of the values MUST match.
    /// </summary>
    public struct CPropertyRestriction : IWspRestriction
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the relation to perform on the property.
        /// </summary>
        public CPropertyRestriction_relop_Values _relop;

        /// <summary>
        /// A CFullPropSpec structure indicating the property on which to perform a match operation.
        /// </summary>
        public CFullPropSpec _Property;

        /// <summary>
        /// A CBaseStorageVariant structure containing the value to relate to the property.
        /// </summary>
        public CBaseStorageVariant _prval;

        /// <summary>
        /// A 32-bit unsigned integer representing locale for the string contained in _prval, as specified in [MS-LCID].
        /// </summary>
        public uint _lcid;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_relop);

            _Property.ToBytes(buffer);

            buffer.AlignWrite(4);

            _prval.ToBytes(buffer);

            buffer.Add(_lcid, 4);
        }
    }
}
