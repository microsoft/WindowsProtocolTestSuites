// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Protocols.TestTools.StackSdk.Asn1
{
    /// <summary>
    /// Constains some common procedures and properties for all ASN.1 structures.
    /// </summary>
    /// <remarks>
    /// All ASN.1 types including primitive types (like BOOLEAN, INTEGER) and contructed types (like SEQUENCE, CHOICE) should be derived from this class.
    /// </remarks>
    public abstract class Asn1Object
    {
        #region Basic

        /// <summary>
        /// Gets the universal class tag of the structure.
        /// </summary>
        /// <exception cref="Asn1UserDefinedTypeInconsistent">
        /// Thrown when no Universal Class Tag is found.
        /// </exception>
        public virtual Asn1Tag UniversalTag
        {
            get
            {
                Object[] attrs = this.GetType().GetCustomAttributes(typeof(Asn1Tag), true);
                Asn1Tag tempTag = null;
                for (int i = 0; i < attrs.Length; i++)
                {
                    tempTag = attrs[i] as Asn1Tag;
                    if (tempTag.TagType == Asn1TagType.Universal)
                    {
                        return tempTag;
                    }
                }
                throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent + " Missing Universal Class Tag.");
            }
        }

        /// <summary>
        /// Gets the top most tag of the structure in BER encoding.
        /// </summary>
        /// <exception cref="Asn1UserDefinedTypeInconsistent">
        /// Thrown when no Universal Class Tag is found.
        /// </exception>
        public virtual Asn1Tag TopTag
        {
            get
            {
                Object[] attrs = this.GetType().GetCustomAttributes(typeof(Asn1Tag), true);
                //At least there is an Universal Class Tag
                if (attrs.Length == 0)
                {
                    throw new Asn1UserDefinedTypeInconsistent(ExceptionMessages.UserDefinedTypeInconsistent + " Missing Universal Class Tag.");
                }

                Asn1Tag uniTag = null;
                Asn1Tag tempTag = null;

                for (int i = 0; i < attrs.Length; i++)
                {
                    tempTag = attrs[i] as Asn1Tag;
                    if (tempTag.TagType != Asn1TagType.Universal)
                    {
                        return tempTag;
                    }
                    else
                    {
                        if (uniTag == null)
                        {
                            uniTag = tempTag;
                        }
                    }
                }
                return uniTag;
            }
        }

        /// <summary>
        /// Checks whether the data in the ASN.1 object meets the constraints.
        /// </summary>
        /// <returns>True if constraints are met. False if not.</returns>
        /// <remarks>
        /// Constraints include user defined and ASN.1 defined constraints.
        /// If there are constraints in the derived classes, this method should be overrode.
        /// </remarks>
        protected virtual bool VerifyConstraints()
        {
            return true;
        }

        /// <summary>
        /// Gets an indicator which speicifies whether an external object is defined in the structure.
        /// </summary>
        protected bool HasExternalObjects
        {
            get
            {
                MemberInfo[] mis = this.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
                List<MemberInfo> list = new List<MemberInfo>();
                foreach (var mi in mis)
                {
                    if (mi.IsDefined(typeof(Asn1Extension), true))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #region overrode methods from System.Object

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if obj has same data with this instance. False if not.</returns>
        public override bool Equals(System.Object obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Overrode method from System.Object.
        /// </summary>
        /// <returns>A string that represents this instance.</returns>
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the hash code of the instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion overrode methods from System.Object

        #endregion Basic

        #region BER

        /// <summary>
        /// Encodes a length to the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length">The length to be written to the buffer.</param>
        /// <returns>The length of "the encoded length's encoding result".</returns>
        /// <remarks>
        /// This method are implementd by Asn1StandardBerProcedure.LengthBerEncode.
        /// Override this method in a user-defined class only if the standard procedure is not applicable in some special scenarios.
        /// </remarks>
        protected virtual int LengthBerEncode(IAsn1BerEncodingBuffer buffer, int length)
        {
            return Asn1StandardProcedure.LengthBerEncode(buffer, length);
        }

        /// <summary>
        /// Reads/decodes a length from the buffer.
        /// </summary>
        /// <param name="buffer">A buffer that stores a BER encoding result.</param>
        /// <param name="decodedLength">The decoded length will be retrieved by this param.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode the length.</returns>
        /// <remarks>
        /// This method are implementd by Asn1StandardBerProcedure.LengthBerDecode.
        /// Override this method in a user-defined class only if the standard procedure is not applicable in some special scenarios.
        /// </remarks>
        protected virtual int LengthBerDecode(IAsn1DecodingBuffer buffer, out int decodedLength)
        {
            return Asn1StandardProcedure.LengthBerDecode(buffer, out decodedLength);
        }

        /// <summary>
        /// Encodes a tag to the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="tag"></param>
        /// <returns>The length of the encoding result.</returns>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        protected virtual int TagBerEncode(IAsn1BerEncodingBuffer buffer, Asn1Tag tag)
        {
            return Asn1StandardProcedure.TagBerEncode(buffer, tag);
        }

        /// <summary>
        /// Decodes a tag from the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="tag"></param>
        /// <returns>The number of the bytes consumed in the buffer to decode the tag.</returns>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        protected virtual int TagBerDecode(IAsn1DecodingBuffer buffer, out Asn1Tag tag)
        {
            return Asn1StandardProcedure.TagBerDecode(buffer, out tag);
        }

        /// <summary>
        /// Encodes the data of this object to the buffer.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <returns>The length of the encoding result of the data.</returns>
        /// <remarks>
        /// For the TLV (Tag, Length, Value) triple in the encoding result,
        /// this method provides the functionality of encoding Value.
        /// The encoding for Tag and Length will be done in Asn1Object::BerEncode method.
        /// </remarks>
        protected virtual int ValueBerEncode(IAsn1BerEncodingBuffer buffer)
        {
            throw new NotImplementedException("ValueBerEncode should be implemented to support BER Encode.");
        }

        /// <summary>
        /// Decodes the data from the buffer and stores the data in this object.
        /// </summary>
        /// <param name="buffer">A buffer that stores a BER encoding result.</param>
        /// <param name="length">The length of the encoding result of the data in the given buffer.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode the data.</returns>
        /// <remarks>
        /// For the TLV (Tag, Length, Value) triple in the encoding result, 
        /// this method provides the functionality of decoding Value.
        /// The decoding for Tag and Length will be done in Asn1Object::BerDecode method.
        /// </remarks>
        protected virtual int ValueBerDecode(IAsn1DecodingBuffer buffer, int length)
        {
            throw new NotImplementedException("ValueBerDecode should be implemented to support BER Decode.");
        }

        /// <summary>
        /// Encodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that stores the BER encoding result.</param>
        /// <param name="explicitTag">Indicates whether the tags should be encoded explicitly. In our Test Suites, it will always be true.</param>
        /// <returns>The length of the encoding result of this object.</returns>
        /// <exception cref="Asn1ConstraintsNotSatisfied">
        /// Thrown when the constraints are not satisfied before encoding.
        /// </exception>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        public virtual int BerEncode(IAsn1BerEncodingBuffer buffer, bool explicitTag = true)
        {
            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Encode " + this.GetType().Name + ".");
            }

            //Add the encoding result of Value to the front of buffer.
            int resultLen = ValueBerEncode(buffer);

            if (explicitTag)
            {
                //Add the encoding result of Length to the front of buffer.
                resultLen += LengthBerEncode(buffer, resultLen);

                //Add the encoding result of Universal Class Tag to the front of buffer.
                Asn1Tag uniTag = this.UniversalTag;
                resultLen += TagBerEncode(buffer, uniTag);

                //Add the encoding result of the top most tag (in most cases it's Application Class Tag) to the front of buffer if it is defined.
                Asn1Tag topTag = this.TopTag;
                if (topTag.TagType != Asn1TagType.Universal)
                {
                    resultLen += LengthBerEncode(buffer, resultLen);
                    resultLen += TagBerEncode(buffer, topTag);
                }
            }

            return resultLen;
        }

        /// <summary>
        /// Decodes the object by BER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a BER encoding result.</param>
        /// <param name="explicitTag">Indicates whether the tags should be encoded explicitly. In our Test Suites, it will always be true.</param>
        /// <returns>The number of the bytes consumed in the buffer to decode this object.</returns>
        /// <exception cref="Asn1ConstraintsNotSatisfied">
        /// Thrown when the constraints are not satisfied after decoding.
        /// </exception>
        /// <exception cref="Asn1DecodingUnexpectedData">
        /// Thrown when the data in the buffer can not be properly decoded.
        /// </exception>
        /// <remarks>Override this method in a user-defined class only if the procedure is not applicable in some special scenarios.</remarks>
        public virtual int BerDecode(IAsn1DecodingBuffer buffer, bool explicitTag = true)
        {
            int returnVal = 0, lengthAfterTopTag = 0, lengthAfterUniTag = 0;
            if (explicitTag)
            {
                //Decode the top most tag and universal class tag
                Asn1Tag topTag;
                returnVal += TagBerDecode(buffer, out topTag);

                Asn1Tag topTagInDefinition = this.TopTag;

                if (topTag.TagType != Asn1TagType.Universal)
                {
                    returnVal += LengthBerDecode(buffer, out lengthAfterTopTag);
                    Asn1Tag uniTag;
                    returnVal += TagBerDecode(buffer, out uniTag);
                    Asn1Tag uniTagInDefinition = this.UniversalTag;
                    if (!uniTag.Equals(uniTagInDefinition))
                    {
                        throw new Asn1DecodingUnexpectedData(ExceptionMessages.DecodingUnexpectedData + " Universal Class Tag decoding fail.");
                    }
                }
            }

            returnVal += LengthBerDecode(buffer, out lengthAfterUniTag);

            //Decode data
            returnVal += ValueBerDecode(buffer, lengthAfterUniTag);

            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Decode " + this.GetType().Name + ".");
            }

            return returnVal;
        }

        /// <summary>
        /// Encodes only the data and length of the data.
        /// </summary>
        /// <param name="buffer">A buffer that stores the BER encoding result.</param>
        public int BerEncodeWithoutUnisersalTag(IAsn1BerEncodingBuffer buffer)
        {
            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Encode " + this.GetType().Name + ".");
            }

            //Encoding inversely since buffer is reversed.

            //Add the encoding result of Value to the front of buffer.
            int resultLen = ValueBerEncode(buffer);

            //Add the encoding result of Length to the front of buffer.
            resultLen += LengthBerEncode(buffer, resultLen);

            return resultLen;
        }

        /// <summary>
        /// Decodes the data and length of the data.
        /// </summary>
        /// <param name="buffer">A buffer that contains a BER encoding result without tags.</param>
        public int BerDecodeWithoutUnisersalTag(IAsn1DecodingBuffer buffer)
        {
            int returnVal = 0;
            //Decode length
            int length;
            returnVal += LengthBerDecode(buffer, out length);
            //Decode data
            returnVal += ValueBerDecode(buffer, length);

            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied
                    + " Decode " + this.GetType().Name + ".");
            }

            return returnVal;
        }

        #endregion BER

        #region PER

        /// <summary>
        /// Encodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        protected virtual void ValuePerEncode(IAsn1PerEncodingBuffer buffer)
        {
            throw new NotImplementedException("PER encoding is not implemented yet.");
        }

        /// <summary>
        /// Decodes the content of the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        protected virtual void ValuePerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            throw new NotImplementedException("PER decoding is not implemented yet.");
        }

        /// <summary>
        /// Encodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer to which the encoding result will be written.</param>
        /// <remarks>
        /// Includes the constraints verification and external object check.
        /// </remarks>
        public virtual void PerEncode(IAsn1PerEncodingBuffer buffer)
        {
            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied);
            }
            if (HasExternalObjects)
            {
                buffer.WriteBit(false);
            }
            ValuePerEncode(buffer);
        }

        /// <summary>
        /// Decodes the object by PER.
        /// </summary>
        /// <param name="buffer">A buffer that contains a PER encoding result.</param>
        /// <param name="aligned">Indicating whether the PER decoding is aligned.</param>
        /// <remarks>
        /// Includes the constraints verification and external object check.
        /// </remarks> 
        public virtual void PerDecode(IAsn1DecodingBuffer buffer, bool aligned = true)
        {
            if (aligned == false)
            {
                throw new NotImplementedException(ExceptionMessages.UnalignedNotImplemented);
            }
            if (HasExternalObjects)
            {
                bool hasExt = buffer.ReadBit();
                if (hasExt)
                {
                    throw new NotImplementedException("External marker is not implemented yet.");
                }
            }
            ValuePerDecode(buffer, aligned);
            if (!VerifyConstraints())
            {
                throw new Asn1ConstraintsNotSatisfied(ExceptionMessages.ConstraintsNotSatisfied);
            }
        }

        #endregion PER

        /// <summary>
        /// Check if tag from buffer is match with input Asn1Tag
        /// </summary>
        /// <param name="buffer">decode buffer</param>
        /// <param name="tag">compare Asn1Tag</param>
        /// <param name="length">Tag Length</param>
        /// <param name="IsForward">If true Postion + 1, otherwise not change Postion</param>
        /// <returns>Tag from Buffer is match with input Tag</returns>
        public virtual bool IsTagMatch(IAsn1DecodingBuffer buffer, Asn1Tag tag, out int length, bool IsForward = false)
        {
            length = 0;
            if (buffer.IsNomoreData())
            {
                return false;
            }

            byte tagByte = Asn1StandardProcedure.GetEncodeTag(tag);
            byte bufferTagByte = IsForward ? buffer.ReadByte() : buffer.PeekByte();
            length = IsForward ? 1 : 0;
            return tagByte == bufferTagByte;
        }
    }
}
