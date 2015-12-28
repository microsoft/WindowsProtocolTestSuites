// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Contains some messages for exceptions.
    /// </summary>
    public class ExceptionMessages
    {
        /// <summary>
        /// Error message shown when the buffer is out of range in decoding.
        /// </summary>
        public const string DecodingOutOfRange = "Decoding error. No enough data to read.";

        /// <summary>
        /// Error message shown when decoding an unexpected data.
        /// </summary>
        public const string DecodingUnexpectedData = "Decoding error. Unexpected data is read.";

        /// <summary>
        /// Error message shown when the constraints are not satisfied for the data in an ASN.1 structure.
        /// </summary>
        public const string ConstraintsNotSatisfied = "The value violates the constraints.";

        /// <summary>
        /// Error message shown when a negative value is passed as a length.
        /// </summary>
        public const string LengthNegative = "Length could not be negative.";

        /// <summary>
        /// Error message shown when trying to encode an undefined enumerated value.
        /// </summary>
        public const string EnumeratedValueUndefined = "Undefined enumerated value.";

        /// <summary>
        /// Error message shown when trying to encode a structure which doesn't have data.
        /// </summary>
        public const string EmptyData = "This object doesn't contains data.";

        /// <summary>
        /// Error message shown when decoding an unexpected data.
        /// </summary>
        public const string UserDefinedTypeInconsistent = "The user defined type is inconsistent.";

        /// <summary>
        /// Error message shown when encoding a structure of which some manadatory fields are empty.
        /// </summary>
        public const string SomeMandatoryFieldsAreEmpty = "Some of the mandatory fields are empty.";

        /// <summary>
        /// Error message shown when using of a invalid choice index.
        /// </summary>
        public const string InvalidChoiceIndex = "The choice index doesn't exist.";

        /// <summary>
        /// Error message shown when reaching the unreachable part in source code.
        /// </summary>
        public const string Unreachable = "This part of code is unreachable.";

        /// <summary>
        /// Error message shown when user trying to use UNALIGNED variant in PER.
        /// </summary>
        public const string UnalignedNotImplemented = "UNALIGNED variant is not implemented in the runtime.";

        /// <summary>
        /// Error message shown when seeking position failed.
        /// </summary>
        public const string SeekPositionFailed = "Seek position failed. New position is out of range.";
    }
}
