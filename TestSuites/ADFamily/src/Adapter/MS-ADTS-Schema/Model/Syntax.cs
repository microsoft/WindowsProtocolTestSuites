// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{

    #region General Syntax and Value Representation

    /// <summary>
    /// A structure describing the context of an attribute in which values are parsed, unparsed, and compared.
    /// </summary>
    public struct AttributeContext
    {
        /// <summary>
        /// The domain controller where this attribute lives. Can be null if the
        /// attribute context is created without having a DC available.
        /// </summary>
        public ModelDomainController dc;

        /// <summary>
        /// Name of the attribute. For error messages.
        /// </summary>
        public string name;

        /// <summary>
        /// Whether the attribute is single valued.
        /// </summary>
        public bool isSingleValued;

        /// <summary>
        /// An optional enumeration type used for symbolic integers
        /// </summary>
        public Type symbolEnumType;

        /// <summary>
        /// The syntax associated with the attribute.
        /// </summary>
        public Syntax syntax;

        /// <summary>
        /// Construct context, with single-valued true, and symbol enum type.
        /// </summary>
        /// <param name="dc">The domain controller of model.</param>
        /// <param name="name">The name of attribute.</param>
        /// <param name="syntax">The syntax of attribute.</param>
        public AttributeContext(ModelDomainController dc, string name, Syntax syntax)
        {
            this.dc = dc;
            this.name = name;
            this.isSingleValued = true;
            this.syntax = syntax;
            this.symbolEnumType = null;
        }


        /// <summary>
        /// Construct context.
        /// </summary>
        /// <param name="dc">The domain controller of model.</param>
        /// <param name="name">The name of attribute.</param>
        /// <param name="syntax">The syntax of attribute.</param>
        /// <param name="isSingleValued">If true,it is single valued.</param>
        /// <param name="symbolEnumType">The type of symbol enum.</param>
        public AttributeContext(
            ModelDomainController dc, 
            string name, 
            Syntax syntax, 
            bool isSingleValued, 
            Type symbolEnumType)
        {
            this.dc = dc;
            this.name = name;
            this.isSingleValued = isSingleValued;
            this.symbolEnumType = symbolEnumType;
            this.syntax = syntax;
        }


        /// <summary>
        /// Parses a value in the given context.
        /// </summary>
        /// <param name="repr">The parsed string.</param>
        /// <returns>Returns parsed value.</returns>
        public Value Parse(string repr)
        {
            return this.syntax.Parse(this, repr);
        }


        /// <summary>
        /// Unparses the value in the given context
        /// </summary>
        /// <param name="value">The value before unparsing.</param>
        /// <returns>Returns unparsed value.</returns>
        public string Unparse(Value value)
        {
            return this.syntax.Unparse(this, value);
        }


        /// <summary>
        /// Checks equality of two values in given context.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if the value1 equals value2.</returns>
        public bool Equals(Value value1, Value value2)
        {
            return this.syntax.Equals(this, value1, value2);
        }
    }


    /// <summary>
    /// Abstract type representing the syntax of a value.
    /// </summary>
    public abstract class Syntax : CompoundValue
    {
        /// <summary>
        /// The attribute syntax identifier (as in 1.1.5.6)
        /// </summary>
        public abstract string AttributeSyntax { get; }

        /// <summary>
        /// The OM syntax value.
        /// </summary>
        public abstract int OMSyntax { get; }

        /// <summary>
        /// The OM object class, if given.
        /// </summary>
        public virtual string OMObjectClass { get { return null; } }

        /// <summary>
        /// Parse one underlying value of this syntax.
        /// </summary>
        /// <param name="context">The context of attribute.</param>
        /// <param name="repr">The parsed string.</param>
        /// <returns>Returns object.</returns>
        public abstract object ParseOne(AttributeContext context, string repr);

        /// <summary>
        /// Unparse one value of this syntax.
        /// </summary>
        /// <param name="context">The context of attribute.</param>
        /// <param name="value">The unparsed value.</param>
        /// <returns>Returns string.</returns>
        public abstract string UnparseOne(AttributeContext context, object value);

        /// <summary>
        /// Checks equality of two values with given syntax.
        /// </summary>
        /// <param name="context">The context of attribute.</param>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if value1 equals value2.</returns>
        public abstract bool EqualsOne(AttributeContext context, object value1, object value2);

        /// <summary>
        /// Parses a value of this syntax.
        /// </summary>
        /// <param name="context">The context of attribute.</param>
        /// <param name="repr">The parsed string.</param>
        /// <returns>Returns parsed value.</returns>
        public Value Parse(AttributeContext context, string repr)
        {
            Sequence<object> result = new Sequence<object>();

            if (!context.isSingleValued)
            {
                string[] values = repr.Split(';', ',');

                foreach (string value in values)
                {
                    result = result.Add(ParseOne(context, value.Trim()));
                }
            }
            else
            {
                result = result.Add(ParseOne(context, repr));
            }

            return new Value(this, result);
        }


        /// <summary>
        /// Unparse a value of this syntax.
        /// </summary>
        /// <param name="context">The context of attribute.</param>
        /// <param name="value">The unparsed value.</param>
        /// <returns>Returns unparsed value.</returns>
        public string Unparse(AttributeContext context, Value value)
        {
            return String.Join(
                ";",
                (from uv in value.UnderlyingValues
                select UnparseOne(context, uv)).ToArray());
        }


        /// <summary>
        /// Checks equality of values of this syntax. This abstracts from the order of values
        /// in the collection.
        /// </summary>
        /// <param name="context">The context of attribute.</param>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if the two values are equal.</returns>
        public bool Equals(AttributeContext context, Value value1, Value value2)
        {
            if (value1 == null)
            {
                return value2 == null;
            }
            else if (value2 == null)
            {
                return value1 == null;
            }
            else
            {
                SequenceContainer<object> uvs1 = new SequenceContainer<object>(value1.UnderlyingValues);
                SequenceContainer<object> uvs2 = new SequenceContainer<object>(value2.UnderlyingValues);

                if (uvs1.Count != uvs2.Count)
                {
                    return false;
                }
                foreach (object uv1 in uvs1)
                {
                    int i = 0;
                    bool found = false;
                    foreach (object uv2 in uvs2)
                    {
                        if (EqualsOne(context, uv1, uv2))
                        {
                            uvs2.RemoveAt(i);
                            found = true;
                            break;
                        }
                        i++;
                    }
                    if (!found)
                    {
                        return false;
                    }
                }
                return uvs2.Count == 0;
            }
        }


         /// <summary>
         ///  Get a name describing this syntax.
         /// </summary>
        public string Name
        {
            get
            {
                string s = this.GetType().Name;
                int i = s.LastIndexOf(".");
                if (i >= 0)
                {
                    return s.Substring(i + 1);
                }
                else
                {
                    return s;
                }
            }
        } 
    

        /// <summary>
        /// The sequence of registered syntax.
        /// </summary>
        protected static SequenceContainer<Syntax> Available = new SequenceContainer<Syntax>();

       
        /// <summary>
        /// Looks-up a syntax based on attribute syntax value and OM syntax value.
        /// </summary>
        /// <param name="attributeSyntax">The syntax of attribute.</param>
        /// <param name="oMSyntax">The syntax of OM.</param>
        /// <param name="oMObjectClass">The object class of OM.</param>
        /// <returns>Returns syntax.</returns>
        public static Syntax Lookup(string attributeSyntax, int oMSyntax, string oMObjectClass)
        {
            Syntax syntax =
                Available.SingleOrDefault<Syntax>(
                    s =>
                    s.AttributeSyntax == attributeSyntax
                    && s.OMSyntax == oMSyntax
                    && s.OMObjectClass == oMObjectClass

                );
            Assert.IsTrue(
                syntax != null, 
                "attribute syntax '{0}/{1}/{2}' not defined", 
                attributeSyntax, 
                oMSyntax, 
                oMObjectClass);
            return syntax;
        }


        /// <summary>
        /// All the syntaxes are adding to the Available syntaxes.
        /// </summary>
        /// <param name="syntax">The syntax needs to added.</param>
        /// <returns>Returns added syntax.</returns>
        private static Syntax AddSyntax(Syntax syntax)
        {
            Available.Add(syntax);
            return syntax;
        }


        /// <summary>
        /// Boolean Syntax.
        /// </summary>
        public static Syntax Boolean = AddSyntax(new BooleanSyntax());

        /// <summary>
        /// Enumeration Syntax.
        /// </summary>
        public static Syntax Enumeration = AddSyntax(new EnumerationSyntax());

        /// <summary>
        /// Integer Syntax.
        /// </summary>
        public static Syntax Integer = AddSyntax(new IntegerSyntax());

        /// <summary>
        /// LargeInteger Syntax.
        /// </summary>
        public static Syntax LargeInteger = AddSyntax(new LargeIntegerSyntax());

        /// <summary>
        /// ObjectAccessPoint Syntax.
        /// </summary>
        public static Syntax ObjectAccessPoint = AddSyntax(new ObjectAccessPointSyntax());

        /// <summary>
        /// ObjectDNString Syntax.
        /// </summary>
        public static Syntax ObjectDNString = AddSyntax(new ObjectDNStringSyntax());

        /// <summary>
        /// ObjectORName Syntax.
        /// </summary>
        public static Syntax ObjectORName = AddSyntax(new ObjectORNameSyntax());

        /// <summary>
        /// ObjectDNBinary Syntax.
        /// </summary>
        public static Syntax ObjectDNBinary = AddSyntax(new ObjectDNBinarySyntax());

        /// <summary>
        /// ObjectDSDN Syntax.
        /// </summary>
        public static Syntax ObjectDSDN = AddSyntax(new ObjectDSDNSyntax());

        /// <summary>
        /// ObjectPresentationAddress Syntax.
        /// </summary>
        public static Syntax ObjectPresentationAddress = AddSyntax(new ObjectPresentationAddressSyntax());

        /// <summary>
        /// ObjectReplicaLink Syntax.
        /// </summary>
        public static Syntax ObjectReplicaLink = AddSyntax(new ObjectReplicaLinkSyntax());

        /// <summary>
        /// StringCase Syntax.
        /// </summary>
        public static Syntax StringCase = AddSyntax(new StringCaseSyntax());

        /// <summary>
        /// StringIA5 Syntax.
        /// </summary>
        public static Syntax StringIA5 = AddSyntax(new StringIA5Syntax());

        /// <summary>
        /// StringNTSecDesc Syntax.
        /// </summary>
        public static Syntax StringNTSecDesc = AddSyntax(new StringNTSecDescSyntax());

        /// <summary>
        /// StringNumeric Syntax.
        /// </summary>
        public static Syntax StringNumeric = AddSyntax(new StringNumericSyntax());

        /// <summary>
        /// StringObjectIdentifier Syntax.
        /// </summary>
        public static Syntax StringObjectIdentifier = AddSyntax(new StringObjectIdentifierSyntax());

        /// <summary>
        /// StringOctet Syntax.
        /// </summary>
        public static Syntax StringOctet = AddSyntax(new StringOctetSyntax());

        /// <summary>
        /// StringPrintable Syntax.
        /// </summary>
        public static Syntax StringPrintable = AddSyntax(new StringPrintableSyntax());

        /// <summary>
        /// StringSid Syntax.
        /// </summary>
        public static Syntax StringSid = AddSyntax(new StringSidSyntax());

        /// <summary>
        /// StringTeletex Syntax.
        /// </summary>
        public static Syntax StringTeletex = AddSyntax(new StringTeletexSyntax());

        /// <summary>
        /// StringUnicode Syntax.
        /// </summary>
        public static Syntax StringUnicode = AddSyntax(new StringUnicodeSyntax());

        /// <summary>
        /// StringUTCTime Syntax.
        /// </summary>
        public static Syntax StringUTCTime = AddSyntax(new StringUTCTimeSyntax());

        /// <summary>
        /// StringGeneralizedTime Syntax.
        /// </summary>
        public static Syntax StringGeneralizedTime = AddSyntax(new StringGeneralizedTimeSyntax());
    }

    /// <summary>
    /// Represents a value, together with its syntax and underlying representation.
    /// NOTE: this type has implicit and explicit conversions attached. A string or
    /// int implicitly converts into a value; a value can be explicitly converted to a string or int.
    /// </summary>
    public class Value : CompoundValue
    {
        /// <summary>
        /// Syntax of this Value object.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public readonly Syntax Syntax;

        /// <summary>
        /// The values of this Value object.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public readonly Sequence<object> UnderlyingValues;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="syntax">Syntax object</param>
        /// <param name="underlyingValues">Underlying values.</param>
        public Value(Syntax syntax, Sequence<object> underlyingValues)
        {
            this.Syntax = syntax;
            this.UnderlyingValues = underlyingValues;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="syntax">Syntax object</param>
        /// <param name="underlyingValues">Underlying values.</param>
        public Value(Syntax syntax, params object[] underlyingValues)
        {
            this.Syntax = syntax;
            this.UnderlyingValues = new Sequence<object>(underlyingValues);
        }

        /// <summary>
        /// ToString method of this object
        /// </summary>
        /// <returns>String representation of this object.</returns>
        public override string ToString()
        {
            return this.Syntax.Unparse(new AttributeContext(null, "<anonymous>", this.Syntax), this);
        }

        /// <summary>
        /// This operator overloading is used to check two values.
        /// </summary>
        /// <param name="value">The second value for comparison.</param>
        /// <returns>Underlying values.</returns>
        public static explicit operator string(Value value)
        {
            if (value == null)
            {
                return null;
            }
            Assert.IsTrue(value.UnderlyingValues.Count > 0, "value expected to have count of at least 1");
            object uv = value.UnderlyingValues[0];
            Assert.IsTrue(uv is string, "value '{0}' expected to be a string (is {1})", uv, uv.GetType());
            return (string)uv; 
        }


        /// <summary>
        /// This operator overloading is used to check two values.
        /// </summary>
        /// <param name="value">The second value for comparison.</param>
        /// <returns>Underlying values.</returns>
        public static explicit operator int(Value value)
        {
            if (value == null)
            {
                return 0;
            }
            Assert.IsTrue(value.UnderlyingValues.Count > 0, "value expected to have count of at least 1");
            object uv = value.UnderlyingValues[0];
            Assert.IsTrue(uv is int, "value '{0}' expected to be an int", uv);
            return (int)uv; 
        }
                
    }

    #endregion

    #region Base class for syntax based on generic string representation
      
    /// <summary>
    /// This class is used to represent the String based syntax.
    /// </summary>
  
    public abstract class StringBasedSyntax : Syntax
    {
        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected virtual string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr;
        }

        /// <summary>
        /// This method is used to parsing the attribute into string.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        public override object ParseOne(AttributeContext context, string repr)
        {
            return CheckAndNormalize(context, repr);
        }

        /// <summary>
        /// This method is used to unparsing the attribute into string.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="value">Value of this attribute.</param>
        /// <returns>String representation.</returns>
        public override string UnparseOne(AttributeContext context, object value)
        {
            return value.ToString();
        }

        /// <summary>
        /// This method is used to check for equality of two objects.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        /// <returns>If both values are equal returns true, else false.</returns>
        public override bool EqualsOne(AttributeContext context, object value1, object value2)
        {
            string sv1 = (string)value1;
            string sv2 = (string)value2;

            if (sv1 == null)
            {
                return sv2 == null;
            }
            else if (sv2 == null)
            {
                return sv1 == null;
            }
            else
            {
                return CheckAndNormalize(context, sv1) == CheckAndNormalize(context, sv2);
            }
        }
    }

    #endregion
    
    #region Boolean

    /// <summary>
    /// This class is used represent the Boolean syntax.
    /// </summary>
    public class BooleanSyntax : Syntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.8"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 1; }
        }

        /// <summary>
        /// This method is used to parsing the attribute into string.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        public override object ParseOne(AttributeContext context, string repr)
        {
            bool result;
            Checks.IsTrue(
                System.Boolean.TryParse(repr, out result), 
                "for attribute '{1}': expected boolean, found '{0}'", 
                repr, 
                context.name);
            return result;
        }

        /// <summary>
        /// This method is used to unparsing the attribute into string.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="value">The value of this attribute.</param>
        /// <returns>String representation.</returns>
        public override string UnparseOne(AttributeContext context, object value)
        {
            return value.ToString();
        }

        /// <summary>
        /// This method is used to check for equality of two objects.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        /// <returns>If both values are equal returns true, else false.</returns>
        public override bool EqualsOne(AttributeContext context, object value1, object value2)
        {
            return (bool)value1 == (bool)value2;
        }
    }

    #endregion

    #region Integer and Enumeration

    /// <summary>
    /// This class is used to represent the integer syntax.
    /// </summary>
  
    public class IntegerSyntax : Syntax
    {
    
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.9"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 2; }
        }

        /// <summary>
        /// This method is used to parsing the attribute into string.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        public override object ParseOne(AttributeContext context, string repr)
        {
            int result = 0;
            if (int.TryParse(repr, out result))
            {
                return result;
            }

            if (context.symbolEnumType != null)
            {
                repr = repr.Replace('|', ','); // for Enum.Parse
                uint uresult = 0;
                if (NativeHelpers.TryParseUInt32Enum(context.symbolEnumType, repr, ref uresult))
                {
                    return (int)uresult;
                }
            }
            Checks.IsTrue(false, "for attribute '{1}': expected int, found '{0}'", repr, context.name);
            return 0;
        }

        /// <summary>
        /// This method is used to unparsing the attribute into string.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="value">The value of this attribute.</param>
        /// <returns>String representation.</returns>
        public override string UnparseOne(AttributeContext context, object value)
        {
            if (context.symbolEnumType != null)
            {
                string result = NativeHelpers.UnparseUInt32Enum(context.symbolEnumType, (uint)(int)value);
                return result;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// This method is used to check for equality of two objects.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        /// <returns>If both values are equal returns true, else false.</returns>
        public override bool EqualsOne(AttributeContext context, object value1, object value2)
        {
            return (int)value1 == (int)value2;
        }
    }

    /// <summary>
    /// This class is used to represent the Enumeration syntax.
    /// </summary>
    public class EnumerationSyntax : IntegerSyntax
    {
        
        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 10; }
        }
    }

    /// <summary>
    /// This class is used to represent the large integer syntax.
    /// </summary>
    public class LargeIntegerSyntax : IntegerSyntax
    {
        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get
            {
                return 65;
            }
        }
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get 
            { 
                return "2.5.5.16";
            }
        }
    }

    #endregion

    #region Objects
      
    /// <summary>
    /// This class is used to represent the object access point syntax.
    /// </summary>
    public class ObjectAccessPointSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.14"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 127; }
        }

        /// <summary>
        /// OMObjectClass
        /// </summary>
        public override string OMObjectClass
        {
            get
            {
                return "1.3.12.2.1011.28.0.702";
            }
        }

        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.ToLower().Replace(" ", String.Empty);
        }
    }

    /// <summary>
    /// This class is used to represent the object DN string syntax.
    /// </summary>
    public class ObjectDNStringSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.14"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 127; }
        }

        /// <summary>
        /// OMObjectClass
        /// </summary>
        public override string OMObjectClass
        {
            get
            {
                return "1.2.840.113556.1.1.1.12";
            }
        }

        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.ToLower().Replace(" ", String.Empty);
        }
    }

    /// <summary>
    /// This class is used to represent the object OR name syntax.
    /// </summary>
    public class ObjectORNameSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.7"; }
        }


        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 127; }
        }

        /// <summary>
        /// OMObjectClass
        /// </summary>
        public override string OMObjectClass
        {
            get
            {
                return "2.6.6.1.2.5.11.29";
            }
        }

        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.ToLower().Replace(" ", String.Empty);
        }
    }


    /// <summary>
    /// This class is used to represent the object DN binary syntax.
    /// </summary>
    public class ObjectDNBinarySyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.7"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 127; }
        }

        /// <summary>
        /// OMObjectClass
        /// </summary>
        public override string OMObjectClass
        {
            get
            {
                return "1.2.840.113556.1.1.1.11";
            }
        }

        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.ToLower().Replace(" ", String.Empty);
        }
    }


    /// <summary>
    /// This class is used to object DSDN syntax.
    /// </summary>
    public class ObjectDSDNSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.1"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 127; }
        }

        /// <summary>
        /// OMObjectClass
        /// </summary>
        public override string OMObjectClass
        {
            get
            {
                return "1.3.12.2.1011.28.0.714";
            }
        }


        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.ToLower().Replace(" ", String.Empty);
        }
    }


    /// <summary>
    /// This class is used to object presentation address syntax.
    /// </summary>
    public class ObjectPresentationAddressSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.10"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 127; }
        }

        /// <summary>
        /// OMObjectClass
        /// </summary>
        public override string OMObjectClass
        {
            get
            {
                return "1.3.12.2.1011.28.0.732";
            }
        }

        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            
            return repr.ToLower().Replace(" ", String.Empty);
        }
    }


    /// <summary>
    /// This class is used to represent the object replica link syntax.
    /// </summary>
    public class ObjectReplicaLinkSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.13"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 127; }
        }

        /// <summary>
        /// OMObjectClass
        /// </summary>
        public override string OMObjectClass
        {
            get
            {
                return "1.2.840.113556.1.1.1.6";
            }
        }


        /// <summary>
        /// This method is used to Check and normalize the string into syntax.
        /// </summary>
        /// <param name="context">Attribute context of this object.</param>
        /// <param name="repr">String representation.</param>
        /// <returns>String representation.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Replace("-", String.Empty).Replace(" ", String.Empty).Trim().ToLower();
        }


        /// <summary>
        /// Unparse one value of this syntax.
        /// </summary>
        /// <param name="context">attribute contrext</param>
        /// <param name="value">The value of this attribute.</param>
        /// <returns>String value of this object.</returns>
        public override string UnparseOne(AttributeContext context, object value)
        {
            if (value is byte[])
            {
                string[] bytes =
                    (from b in (byte[])value select b.ToString("x2")).ToArray<string>();
                return String.Join(String.Empty, bytes);
            }
            else
            {
                return (string)value;
            }
        }


        /// <summary>
        /// This method is used to check for equality of two objects.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        /// <returns>If both values are equal returns true, else false.</returns>
        public override bool EqualsOne(AttributeContext context, object value1, object value2)
        {
            string vr1 = UnparseOne(context, value1);
            string vr2 = UnparseOne(context, value2);
            return vr1 == vr2;
        }
    }
    #endregion

    #region Strings

    /// <summary>
    /// This class is used to represent the string case syntax.
    /// </summary>
    public class StringCaseSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.3"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 27; }
        }
    }


    /// <summary>
    /// This class is used to represent the string IAS syntax.
    /// </summary>
    public class StringIA5Syntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.5"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 22; }
        }

        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">Normalized attribute context.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr;
        }
    }

    /// <summary>
    /// This class is used to represent the string NT syntax.
    /// </summary>
    public class StringNTSecDescSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.15"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 66; }
        }

        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Replace("-", String.Empty).Replace(" ", String.Empty).Trim().ToLower();
        }

        /// <summary>
        /// Unparse attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value">The value needs to be unparsed.</param>
        /// <returns>Returns unparsed value.</returns>
        public override string UnparseOne(AttributeContext context, object value)
        {
            if (value is byte[])
            {
                string[] bytes =
                    (from b in (byte[])value select b.ToString("x2")).ToArray<string>();
                return String.Join(String.Empty, bytes);
            }
            else
            {
                return (string)value;
            }
        }

        /// <summary>
        /// Justify whether the two values of attribute context are equal.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        /// <returns>Returns true if the two values are equal.</returns>
        public override bool EqualsOne(AttributeContext context, object value1, object value2)
        {
            string vr1 = UnparseOne(context, value1);
            string vr2 = UnparseOne(context, value2);
            return vr1 == vr2;
        }
    }

    /// <summary>
    /// This class is used to represent string numeric syntax.
    /// </summary>
    public class StringNumericSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.6"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 18; }
        }

        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
             return repr.Trim();
        }
    }

    /// <summary>
    /// This class is used to represent the string object identifier syntax.
    /// </summary>
    public class StringObjectIdentifierSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.2"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 6; }
       
        }

        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Trim().ToLower();
        }
    }

    
    /// <summary>
    /// This class is used to represent the string printable syntax.
    /// </summary>
    public class StringPrintableSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.5"; }
        }
        
        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 19; }
        }

        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Replace("\"", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty).Replace("+",
                String.Empty).Replace(",", String.Empty).Replace("-", String.Empty).Replace(".", String.Empty).Replace(
                "/", String.Empty).Replace(":", String.Empty).Replace("?", String.Empty).Replace(" ", 
                String.Empty).Trim().ToLower();
        }
    }

    /// <summary>
    /// This class is used to represent the string sid syntax.
    /// </summary>
    public class StringSidSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.17"; }
        }


        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 4; }
        }


        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Replace("-", String.Empty).Replace(" ", String.Empty).Trim().ToLower();
        }

        
        /// <summary>
        /// Unparse attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value">The value needs to be unparsed.</param>
        /// <returns>Returns unparsed value.</returns>
        public override string UnparseOne(AttributeContext context, object value)
        {
            if (value is byte[])
            {
                string[] bytes =
                    (from b in (byte[])value select b.ToString("x2")).ToArray<string>();
                return String.Join(String.Empty, bytes);
            }
            else
            {
                return (string)value;
            }
        }


        /// <summary>
        /// Justify whether the two values of attribute context are equal.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        /// <returns>Returns true if the two values are equal.</returns>
        public override bool EqualsOne(AttributeContext context, object value1, object value2)
        {
            string vr1 = UnparseOne(context, value1);
            string vr2 = UnparseOne(context, value2);
            return vr1 == vr2;
        }
    }

   
    /// <summary>
    /// This class is used to represent the string teletex syntax.
    /// </summary>
    public class StringTeletexSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.4"; }
        }


        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 20; }
        }

        
        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.ToLower();
        }
    }

    /// <summary>
    /// This class is used to represent the string unicode syntax.
    /// </summary>
    public class StringUnicodeSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.12"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 64; }
        }
    }


    /// <summary>
    /// This class is used to represent the string UTC time syntax.
    /// </summary>
    public class StringUTCTimeSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.11"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 23; }
        }

       
        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Replace(" ", String.Empty).Replace(":", String.Empty).Replace("/", String.Empty).Trim().ToLower();
        }
    }

    /// <summary>
    /// This class is used to represent the string generalized time syntax.
    /// </summary>
    public class StringGeneralizedTimeSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax.
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.11"; }
        }

        /// <summary>
        /// OMSyntax.
        /// </summary>
        public override int OMSyntax
        {
            get { return 24; }
        }

       
        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Replace(" ", String.Empty).Replace(":", String.Empty).Replace("/", String.Empty).Trim().ToLower();
        }
    }

    /// <summary>
    ///  This class is used to represent the string octet syntax.
    /// </summary>
    public class StringOctetSyntax : StringBasedSyntax
    {
        /// <summary>
        /// AttributeSyntax
        /// </summary>
        public override string AttributeSyntax
        {
            get { return "2.5.5.10"; }
        }

        /// <summary>
        /// OMSyntax
        /// </summary>
        public override int OMSyntax
        {
            get { return 4; }
        }

        
        /// <summary>
        /// Check and normalize attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="repr">The string needs to be normalized.</param>
        /// <returns>Returns normalized attribute context.</returns>
        protected override string CheckAndNormalize(AttributeContext context, string repr)
        {
            return repr.Replace("-", String.Empty).Replace(" ", String.Empty).Trim().ToLower();
        }

        
        /// <summary>
        /// Unparse attribute context.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value">The value needs to be unparsed.</param>
        /// <returns>Returns unparsed value.</returns>
        public override string UnparseOne(AttributeContext context, object value)
        {
            if (value is byte[])
            {
                string[] bytes =
                    (from b in (byte[])value select b.ToString("x2")).ToArray<string>();
                return String.Join(String.Empty, bytes);
            }
            else
            {
                return (string)value;
            }
        }

        /// <summary>
        /// Justify whether the two values of attribute context are equal.
        /// </summary>
        /// <param name="context">Attribute context.</param>
        /// <param name="value1">First value.</param>
        /// <param name="value2">Second value.</param>
        /// <returns>Returns true if the two values are equal.</returns>
        public override bool EqualsOne(AttributeContext context, object value1, object value2)
        {
            string vr1 = UnparseOne(context, value1);
            string vr2 = UnparseOne(context, value2);

            if (context.name.Equals("schemaidguid") || context.name.Equals("attributesecurityguid"))
            {
                Guid guid = new Guid((byte[])value2);
                vr2 = guid.ToString().Replace("-", String.Empty);
            }

            return vr1 == vr2;
        }
    }

    #endregion
}
