// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum RANGEBOUNDARY_ulType_Values : uint
    {
        /// <summary>
        /// MUST only be used for Unicode string values in prVal. Items with a value less than the Unicode string immediately preceding prVal lexicographically are included in the range.
        /// </summary>
        DBRANGEBOUNDTTYPE_BEFORE = 0x00000000,

        /// <summary>
        /// Items with a value less than prVal are included in the range.
        /// </summary>
        DBRANGEBOUNDTTYPE_EXACT = 0x00000001,

        /// <summary>
        /// MUST only be used for Unicode string values in prVal. Items with a value less than the Unicode string immediately after prVal lexicographically are included in the range.
        /// </summary>
        DBRANGEBOUNDTTYPE_AFTER = 0x00000002,
    }

    /// <summary>
    /// The RANGEBOUNDARY structure contains a single range.
    /// </summary>
    public struct RANGEBOUNDARY : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer that indicates which type of boundary is represented by this structure.
        /// </summary>
        public RANGEBOUNDARY_ulType_Values ulType;

        /// <summary>
        /// A CBaseStorageVariant structure.
        /// Note Indicates the value for the range boundary.
        /// If ulType is set to DBRANGEBOUNDTTYPE_BEFORE or DBRANGEBOUNDTTYPE_AFTER, then the vType field of prVal MUST be set to a string type(VT_BSTR, VT_LPWSTR, or VT_COMPRESSED_LPWSTR).
        /// </summary>
        public CBaseStorageVariant prVal;

        /// <summary>
        /// An 8-bit unsigned integer. MUST be set to one of the following values.
        /// 0x00	The _padding, ccLabel, and Label fields are omitted.
        /// 0x01	The _padding, ccLabel, and Label fields are present.
        /// </summary>
        public byte labelPresent;

        /// <summary>
        /// A 32-bit unsigned integer representing the number of characters in the Label field.
        /// Note ccLabel MUST be omitted if labelPresent is set to 0x00; otherwise, it MUST be greater than zero.
        /// </summary>
        public uint ccLabel;

        /// <summary>
        /// Label (variable): A non-null-terminated Unicode string representing the label for this range. The ccLabel field contains the length of the string. 
        /// Note Label MUST be omitted if labelPresent is set to 0x00; otherwise, it MUST NOT be empty.
        /// </summary>
        public string Label;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(ulType);

            prVal.ToBytes(buffer);

            buffer.Add(labelPresent);

            if (labelPresent == 0x01)
            {
                buffer.Add(ccLabel, 4);

                buffer.AddUnicodeString(Label, false);
            }
        }
    }
}
