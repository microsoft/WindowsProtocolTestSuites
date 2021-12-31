// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CFullPropSpec_ulKind_Values : uint
    {
        /// <summary>
        /// The PrSpec field specifies the number of non-null characters in the Property name field.
        /// </summary>
        PRSPEC_LPWSTR = 0x00000000,

        /// <summary>
        /// The PrSpec field specifies the property ID (PROPID).
        /// </summary>
        PRSPEC_PROPID = 0x00000001,
    }

    /// <summary>
    /// The CFullPropSpec structure contains a property set GUID and a property identifier to uniquely identify a property.
    /// A CFullPropSpec instance has a property set GUID and either an integer property ID or a string property name.
    /// For properties to match, the CFullPropSpec structure MUST match the column identifier in the index.
    /// There is no conversion between property IDs and property names. Property names are case insensitive.
    /// (The CFullPropSpec structure corresponds to the FULLPROPSPEC structure described in [MSDN-FULLPROPSPEC].)
    /// </summary>
    public struct CFullPropSpec : IWspStructure
    {
        /// <summary>
        /// The GUID of the property set to which the property belongs.
        /// </summary>
        public Guid _guidPropSet;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the contents of PrSpec.
        /// </summary>
        public CFullPropSpec_ulKind_Values ulKind;

        /// <summary>
        /// A 32-bit unsigned integer with a meaning as indicated by the ulKind field.
        /// </summary>
        public uint PrSpec;

        /// <summary>
        /// If ulKind is set to PRSPEC_PROPID, this field MUST NOT be present.
        /// If ulKind is set to PRSPEC_LPWSTR, this field MUST contain a case-insensitive array of PrSpec non-null Unicode characters that contains the name of the property.
        /// </summary>
        public string Property_name;

        #region Constructors
        /// <summary>
        /// Construct an integer property.
        /// </summary>
        /// <param name="guid">GUID of property.</param>
        /// <param name="propertyID">Identifier of property.</param>
        public CFullPropSpec(Guid guid, uint propertyID)
        {
            _guidPropSet = guid;

            ulKind = CFullPropSpec_ulKind_Values.PRSPEC_PROPID;

            PrSpec = propertyID;

            Property_name = null;
        }
        #endregion

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_guidPropSet, 8);

            buffer.Add(ulKind);

            buffer.Add(PrSpec);

            if (ulKind == CFullPropSpec_ulKind_Values.PRSPEC_LPWSTR)
            {
                buffer.AddUnicodeString(Property_name);
            }
        }
    }
}
