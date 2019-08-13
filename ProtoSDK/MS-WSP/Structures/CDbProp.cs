// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CDbProp structure contains a database property. These properties control how queries are interpreted by the GSS.
    /// </summary>
    public struct CDbProp : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer indicating the property ID.This field uniquely identifies each property in a particular query, but has no other interpretation.
        /// </summary>
        public UInt32 DBPROPID;

        /// <summary>
        /// Property options.This field MUST be set to 0x00000001 if the property is optional and to 0x00000000 otherwise.
        /// </summary>
        public UInt32 DBPROPOPTIONS;

        /// <summary>
        /// Property status.
        /// </summary>
        public UInt32 DBPROPSTATUS;

        /// <summary>
        /// A CDbColId structure that defines the database property being passed.
        /// </summary>
        public CDbColId colid;

        /// <summary>
        /// A CBaseStorageVariant containing the property value.
        /// </summary>
        public CBaseStorageVariant vValue;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(DBPROPID);

            buffer.Add(DBPROPOPTIONS);

            buffer.Add(DBPROPSTATUS);

            colid.ToBytes(buffer);

            vValue.ToBytes(buffer);
        }
    }
}
