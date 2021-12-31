// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CContentRestriction_ulGenerateMethod_Values : uint
    {
        /// <summary>
        /// Exact match. Each word in the phrase matches exactly in the inverted index.
        /// </summary>
        GENERATE_METHOD_EXACT = 0x00000000,

        /// <summary>
        /// Prefix match. Each word in the phrase is considered a match if the word is a prefix of an indexed string.
        /// For example, if the word "barking" is indexed, then "bar" would match when performing a prefix match. 
        /// </summary>
        GENERATE_METHOD_PREFIX = 0x00000001,

        /// <summary>
        /// Matches inflections of a word.
        /// An inflection of a word is a variant of the root word in the same part of speech that has been modified, according to linguistic rules of a given language.
        /// For example, inflections of the verb swim in English include swim, swims, swimming, and swam.
        /// The inflection forms of a word can be determined by calling the Inflect abstract interface (section 3.1.7) to the GSS(Generic Search Service) with "pwcsPhrase" as an argument.
        /// </summary>
        GENERATE_METHOD_INFLECT = 0x00000002,
    }

    /// <summary>
    /// The CContentRestriction structure contains a word or phrase to match in the inverted index for a specific property. 
    /// </summary>
    public struct CContentRestriction : IWspRestriction
    {
        /// <summary>
        /// A CFullPropSpec structure. This field indicates the property on which to perform a match operation.
        /// </summary>
        public CFullPropSpec _Property;

        /// <summary>
        /// A 32-bit unsigned integer, specifying the number of characters in the _pwcsPhrase field.
        /// </summary>
        public uint Cc;

        /// <summary>
        /// A non-null-terminated Unicode string representing the word or phrase to match for the property.
        /// This field MUST NOT be empty. The Cc field contains the length of the string.
        /// </summary>
        public string _pwcsPhrase;

        /// <summary>
        /// A 32-bit unsigned integer, indicating the locale of _pwcsPhrase, as specified in [MS-LCID].
        /// </summary>
        public uint Lcid;

        /// <summary>
        /// A 32-bit unsigned integer, specifying the method to use when generating alternate word forms.
        /// </summary>
        public CContentRestriction_ulGenerateMethod_Values _ulGenerateMethod;

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

            buffer.Add(_ulGenerateMethod);
        }
    }
}
