// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Represents a NumericString in ASN.1 Definition.
    /// </summary>
    [Asn1Tag(Asn1TagType.Universal, Asn1TagValue.NumericString)]
    public class Asn1NumericString : Asn1ByteString
    {
        /// <summary>
        /// Initializes a new instance of the Asn1NumericString class with an empty string.
        /// </summary>
        public Asn1NumericString()
        {
            Value = string.Empty;
        }

        /// <summary>
        /// Gets or sets to the data of this object.
        /// </summary>
        public override sealed string Value
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        /// <summary>
        /// Initializes a new instance of the Asn1NumericString class with a given string.
        /// </summary>
        /// <param name="s"></param>
        public Asn1NumericString(string s)
        {
            Value = s;
        }

        /// <summary>
        /// Gets the built in charset of the string type, in string form or regex form.
        /// </summary>
        protected override string TypeBuiltInCharSet
        {
            get { return @"\d "; }
        }

        //BER encoding/decoding are implemented in base class Asn1ByteString.

        #region PER

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            Asn1StandardProcedure.PerEncodeArray(buffer, Value.ToCharArray(),
                (encodingBuffer, b) =>
                {
                    int val;
                    if (Constraint == null || Constraint.PermittedCharSet == null)
                    {
                        if (b == ' ') //SPACE->0000, '0'->0001, ..., '9'->1010
                        {
                            val = 0;
                        }
                        else
                        {
                            val = 1 + b - '0';
                        }
                    }
                    else
                    {
                        for (val = 0; val != CharSetInArray.Length && b != CharSetInArray[val]; val++)
                        {
                        }
                    }
                    Asn1Integer ai = new Asn1Integer(val, 0, 15);
                    ai.PerEncode(buffer);
                }
            , Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null,
            true);
        }

        /// <summary>
        /// Decodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        /// <remarks>Length is included.</remarks>
        protected override void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            ByteArrayValue = Asn1StandardProcedure.PerDecodeArray<byte>(buffer,
                decodingBuffer =>
                {
                    Asn1Integer ai = new Asn1Integer(null, 0, 15);
                    ai.PerDecode(buffer);
                    if (Constraint == null || Constraint.PermittedCharSet == null)
                    {
                        if (ai.Value == 0)
                        {
                            return (byte)' ';
                        }
                        else if (ai.Value >= 1 && ai.Value <= 10)
                        {
                            return (byte)((byte)'0' + ai.Value - 1);
                        }
                        else
                        {
                            throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData +
                                                                 "Only 0~9 and SPACE are allowed in NumericString.");
                        }
                    }
                    else
                    {
                        int index = (int)ai.Value;
                        return (byte)CharSetInArray[(int)ai.Value];
                    }
                },
            Constraint != null && Constraint.HasMinSize ? Constraint.MinSize : (long?)null, Constraint != null && Constraint.HasMaxSize ? Constraint.MaxSize : (long?)null,
            true);
        }

        #endregion PER
    }
}

