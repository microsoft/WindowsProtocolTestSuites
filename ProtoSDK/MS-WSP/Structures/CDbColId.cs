// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CDbColId_eKind_Values : uint
    {
        /// <summary>
        /// vString contains a property name.
        /// </summary>
        DBKIND_GUID_NAME = 0x00000000,

        /// <summary>
        /// ulId contains a 4-byte integer indicating the property ID.
        /// </summary>
        DBKIND_GUID_PROPID = 0x00000001,
    }

    /// <summary>
    /// The CDbColId structure contains a column identifier.
    /// </summary>
    public struct CDbColId : IWspStructure
    {
        #region Fields
        /// <summary>
        /// This field indicates the contents of GUID and vValue.
        /// </summary>
        public CDbColId_eKind_Values eKind;

        /// <summary>
        /// The property GUID.
        /// </summary>
        public Guid GUID;

        /// <summary>
        /// If eKind is DBKIND_GUID_PROPID, this field contains an unsigned integer specifying the property ID.
        /// If eKind is DBKIND_GUID_NAME, this field contains an unsigned integer specifying the number of Unicode characters contained in the vString field.
        /// </summary>
        public uint ulId;

        /// <summary>
        /// A non-null-terminated Unicode string representing the property name. It MUST be omitted unless the eKind field is set to DBKIND_GUID_NAME.
        /// </summary>
        public string vString;
        #endregion

        #region Constructors
        public CDbColId(Guid property, uint value)
        {
            eKind = CDbColId_eKind_Values.DBKIND_GUID_PROPID;
            GUID = property;
            ulId = value;
            vString = null;
        }

        public CDbColId(Guid property, string value)
        {
            eKind = CDbColId_eKind_Values.DBKIND_GUID_NAME;
            GUID = property;
            ulId = (uint)value.Length;
            vString = value;
        }
        #endregion

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(eKind);

            buffer.Add(GUID, 8);

            buffer.Add(ulId);

            if (eKind == CDbColId_eKind_Values.DBKIND_GUID_NAME)
            {
                buffer.AddUnicodeString(vString);
            }
        }
    }
}
