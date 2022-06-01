// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CNatLanguageRestriction structure contains a natural language query match for a property.
    /// Natural language simply means that the string has no formal meaning.
    /// The GSS is free to match on the string in a variety of ways.
    /// It can drop words, add alternate forms, or make no changes.
    /// </summary>
    public struct CNatLanguageRestriction : IWspRestriction
    {
        /// <summary>
        /// A CFullPropSpec structure. This field indicates the property on which to perform the match operation.
        /// </summary>
        public CFullPropSpec _Property;

        /// <summary>
        /// A 32-bit unsigned integer. The number of characters in the _pwcsPhrase field.
        /// </summary>
        public uint Cc;

        /// <summary>
        /// A non-null-terminated Unicode string containing the text to search for within the specific property.
        /// MUST NOT be empty. The Cc field contains the length of the string.
        /// </summary>
        public string _pwcsPhrase;

        /// <summary>
        /// A 32-bit unsigned integer indicating the locale of _pwcsPhrase, as specified in [MS-LCID].
        /// </summary>
        public uint Lcid;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            _Property.ToBytes(buffer);

            buffer.Add(Cc, 4);

            buffer.AddUnicodeString(_pwcsPhrase, false);

            buffer.Add(Lcid, 4);
        }
    }
}
