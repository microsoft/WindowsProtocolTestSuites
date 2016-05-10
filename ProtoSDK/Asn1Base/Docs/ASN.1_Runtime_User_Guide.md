| ASN.1 Runtime                                            
                                                           
 User Guide                                                |
|----------------------------------------------------------|
| *Windows is built to be the most interoperable platform* |
| ![](./media/image1.png)                                  |

Introduction
============

This guide provides information about how to define C\# classes for ASN.1 definition in Microsoft Protocols. Test Suites are developed to test implementations of Microsoft Protocols, such as Kile, RDP, FileSharing, etc. In Test Suites, the ASN.1 definition in protocols need to be transformed to the corresponding C\# classes, which provide encoding/decoding methods for the instances. This guide provides information about defining the classes by using of the ASN.1 runtime.

Limitation
==========

The ASN.1 runtime only supports the ASN.1 features that are used in Test Suites, i.e., the runtime should only be used by Test Suites, not by the industry.

General Rules
=============

Inheritance
-----------

All of the primitive and constructed classes are directly or indirectly derived from Asn1Object, an abstract class defined in the runtime.

Encoding and Decoding
---------------------

Asn1Object has some public methods for BER encoding and decoding. The following code demonstrates how to invoke BER encoding and decoding methods.

BER encoding:
```
SomeAsn1Struct obj1 = new SomeAsn1Struct();
//Assign values to obj1
Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
obj1.BerEncode(buffer);
byte[] result = buffer.Data;
```

BER decoding:
```
SomeAsn1Struct obj2 = new SomeAsn1Struct();
Asn1DecodingBuffer decodingBuffer = new Asn1DecodingBuffer(result);
obj2.BerDecode(decodingBuffer); //The data will be retrieved
```

TODO: PER encoding and decoding is not implemented yet. Add a sample when it is done.


Primitive Types are defined in the runtime and can be used directly
-------------------------------------------------------------------

Primitive types like INTEGER, BOOLEAN, OCTET STRING, UniversalString are defined in the runtime. The corresponding C\# classes are as follows:

| **ASN.1 Type**  | **C\# Classes**     |
|-----------------|---------------------|
| INTEGER         | Asn1Integer         |
| BOOLEAN         | Asn1Boolean         |
| OCTET STRING    | Asn1OctetString     |
| BIT STRING      | Asn1BitString       |
| BMPString       | Asn1BmpString       |
| GeneralString   | Asn1GeneralString   |
| NumericString   | Asn1NumericString   |
| UniversalString | Asn1UniversalString |
| GeneralizedTime | Asn1GeneralizedTime |

For the above classes,

1.  All of them have a property named “Value” which could get or set the data of the instances,

2.  Asn1OctetString provides a byte array property “ByteArrayValue”, which could get or set the data in byte array form,

3.  Rule B is applicable.

Some of the ASN.1 primitive types like REAL are not defined in the runtime since they are not used in Test Suites.

Define the structures in Test Suites
====================================

Referencing Type
----------------

A referencing type has the form as follows:

> **ReferencingType ::= ReferencedType**

**Rule 1.** In the C\# definition, class ReferencingType should be derived from class ReferencdType.

**Rule 2.** If there are any constraints, the VerifyConstraints method should be overrode.

**Rule 3.** If there are any tags, Asn1Tag attribute should be applied to ReferencingType.

**Example:**

1.  <span id="OLE_LINK8" class="anchor"></span>**Flag ::= BOOLEAN**

2.  **Int32 ::= INTEGER (-2147483648..2147483647)**

3.  **AppInt32 ::= \[APPLICATION 10\] INTEGER (5)**

4.  **AS-REQ ::= \[APPLICATION 11\] KDC-REQ**

5.  **KerberosFlags ::= BIT STRING (SIZE (32…MAX))**

**C\# Class for Example i:**
```
    class Flag : Asn1Boolean
    {
    
    }
```
<span id="OLE_LINK9" class="anchor"><span id="OLE_LINK10" class="anchor"></span></span>**C\# Class for Example ii:**
```
    class Int32 : Asn1Integer
    {
        protected override bool VerifyConstraints()
        {
            return this.Value >= -2147483648 &&
                this.Value <= 2147483647;
        }
    }

```
**C\# Class for Example iii:**

    [Asn1Tag(Asn1TagType.Application, 10)]
    class AppInt32 : Asn1Integer
    {
        protected override bool VerifyConstraints()
        {
            return this.Value == 5;
        }
    }


**C\# Class for Example iv: **


    [Asn1Tag(Asn1TagType.Application, 11)]
    class AS_REQ : KDC_REQ
    { 
    }


**C\# Class for Example v: **
```
class KerberosFlags : Asn1BitString

{

    protected override bool VerifyConstraints()
    
    {
    
        return this.Length &gt;= 32;
    
    }

}
```
Enumerated Type
---------------

An enumerated type has the form as follows:

> <span id="OLE_LINK11" class="anchor"><span id="OLE_LINK12" class="anchor"></span></span>**EnumeratedType ::= ENUMERATED{**

> **enumName1,**

> **enumName2,**
>
> **enumName3,**
>
> **….**
>
> **}**

**Rule 1.** In the C\# definition, the defined class should be derived from class Asn1Enumerated.

**Rule 2.** In the defined class, there should be some public constant fields/properties corresponding to the enumerated elements. If the elements in the ASN.1 definition doesn’t have corresponding values, give them an arbitrary unique one.

TODO: Determine The case of the first letter.

**Rule 3.** The members mentioned in Rule 2 should have the Asn1EnumeratedeElements attribute.

**Rule 4.** <span id="OLE_LINK13" class="anchor"><span id="OLE_LINK14" class="anchor"></span></span>Rule 2 and Rule 3 in Referencing Type are applicable.

**Example:**

><span id="OLE_LINK15" class="anchor"><span id="OLE_LINK16" class="anchor"></span></span>**EnumeratedType1 ::= ENUMERATED{**

> **enumName1,**

> **enumName2,**
>
> **enumName3**
>
> **}**


>  **EnumeratedType2 ::= \[APPLICATION 1\] ENUMERATED{**

>    **enumName1 (10),**

> **enumName2 (20),**
>
> **enumName3 (30),**
>
> **enumName4 (40)**
>
> **}**

**C\# Class for Example i:**
```
    class EnumeratedType1 : Asn1Enumerated 
    {
        [Asn1EnumeratedElement]
        public const long enumName1 = 0;
        //Using properties or using int are all permitted.
        [Asn1EnumeratedElement]
        public const long enumName2 = 1;
        [Asn1EnumeratedElement]
        public const long enumName3 = 2;
    }

```
**C\# Class for Example ii:**
```
    [Asn1Tag(Asn1TagType.Application, 1)]
    class EnumeratedType2 : Asn1Enumerated 
    {
        [Asn1EnumeratedElement]
        public const long enumName1 = 10;
        [Asn1EnumeratedElement]
        public const long enumName2 = 20;
        [Asn1EnumeratedElement]
        public const long enumName3 = 30;
        [Asn1EnumeratedElement]
        public const long enumName4 = 40;
    }
```

SEQUENCE/SET Type
-----------------

A SEQUENCE or SET type has the form as follows:

> **Composition ::= SEQUENCE/SET {**

> **fieldName1 TypeName1,**

> **fieldName2 TypeName2,**
>
> **fieldName3 TypeName3,**
>
> **….**
>
> **}**

**Rule 1.** The C\# class should be derived from Asn1Sequence or Asn1Set, respectively.

**Rule 2.** In the defined class, there should be some public fields/properties corresponding to the fields in the ASN.1 definition. The types of the members should be the same as the types of the fields. The names can be arbitrary but are recommended to be set to the same names of the fields in the ASN.1 definition.

**Rule 3.** The members mentioned in Rule 2 should have the Asn1Field attributes. The positional parameter of the attribute should be set to the index of the corresponding field in ASN.1 definition. If the corresponding field in ASN.1 definition is OPTIONAL, set the named parameter “Optional” to true.

**Rule 4.** If the fields in the ASN.1 definition have Context-Specific Class Tags, add Asn1Tag attributes to the corresponding C\# members.

**Rule 5.** Rule 2 and Rule 3 in Referencing Type are applicable.

**Example: **

> <span id="OLE_LINK17" class="anchor"><span id="OLE_LINK18" class="anchor"></span></span>**SetType1 ::= SET{**

> **fieldName1 Int32, **
>
> **fieldName2 AppInt32, **
>
> **fieldName3 OCTET STRING,**
>
> **fieldName4 AS-REQ,**
>
> **fieldName5 EnumeratedType2 **
>
> **}**

>  **SeqType2 ::= \[APPLICATION 20\] SEQUENCE{**

> **fieldName1 \[0\] Int32, **
>
> **fieldName2 \[1\] AppInt32 OPTIONAL, **
>
> **fieldName3 \[2\] OCTET STRING OPTIONAL,**
>
> **fieldName4 \[3\] AS-REQ,**
>
> **fieldName5 \[4\] EnumeratedType2 OPTIONAL,**
>
> **fieldName6 \[5\] SetType1**
>
> **}**

**C\# Class for Example i:**
```
    class SetType1 : Asn1Set
    {
        [Asn1Field(0)]
        public Int32 fieldName1;
        //Using properties are also permitted.
        [Asn1Field(1)]
        public AppInt32 fieldName2;
        [Asn1Field(2)]
        public Asn1OctetString fieldName3;
        [Asn1Field(3)]
        public AS_REQ fieldName4;
        [Asn1Field(4)]
        public EnumeratedType2 fieldName5;
    }
```

**C\# Class for Example ii:**
```
    [Asn1Tag(Asn1TagType.Application, 20)]    
    class SeqType2 : Asn1Sequence
    {
        [Asn1Field(0), Asn1Tag(Asn1TagType.Context, 0)]
        public Int32 fieldName1;
        [Asn1Field(1, Optional = true), Asn1Tag(Asn1TagType.Context, 1)]
        public AppInt32 fieldName2;
        [Asn1Field(2, Optional = true), Asn1Tag(Asn1TagType.Context, 2)]
        public Asn1OctetString fieldName3;
        [Asn1Field(3), Asn1Tag(Asn1TagType.Context, 3)]
        public AS_REQ fieldName4;
        [Asn1Field(4, Optional = true), Asn1Tag(Asn1TagType.Context, 4)]
        public EnumeratedType2 fieldName5;
        [Asn1Field(5), Asn1Tag(Asn1TagType.Context, 5)]
        public SetType1 fieldName6;
    }
```

CHOICE Type
-----------

A Choice type has the form as follows:

**Choice ::= CHOICE {**

**choiceName1 TypeName1,**

> **choiceName2 TypeName2,**
>
> **choiceName3 TypeName3,**
>
> **….**
>
> **}**

**Rule 1.** The C\# class should be derived from Asn1Choice.

**Rule 2.** In the defined class, there should be a set of static public fields/properties, of which the type is long or int. The names of these members should be exactly the same as the names of the choices. The values of the members are the indices of the choices in the ASN.1 definition.

**Rule 3.** The members mentioned in Rule 2 should have the Asn1ChoiceIndex attribute.

**Rule 4.** In the defined class, there should be another set of non-static protected fields/properties corresponding to the choices in the ASN.1 definition. The type of the members should be the same as the type of the choices. The names of the member can be arbitrary.

**Rule 5.** The members mentioned in Rule 4 should have the Asn1ChoiceElement attributes. The positional parameter of the attribute should be set to value of the corresponding member mentioned in Rule 2.

**Rule 6.** If the choices in the ASN.1 definition have Context-Specific Class Tags, add Asn1Tag attributes to the members in Rule 4.

**Rule 7.** Rule 2 and Rule 3 in Referencing Type are applicable.

**Example: **

> **ChoiceType1::= CHOICE{**

> <span id="OLE_LINK19" class="anchor"><span id="OLE_LINK20" class="anchor"></span></span>**choiceName1 INTEGER, **
>
> **choiceName2 OCTET STRING, **
>
> **choiceName3 SetType1,**
>
> **choiceName4 EnumeratedType2,**
>
> **choiceName5 AS-REQ**
>
> **}**

> **ChoiceType2 ::= \[APPLICATION 2\] CHOICE{**

> **choiceName1 \[1\] INTEGER,**
>
> **choiceName2 \[2\] OCTET STRING,**
>
> **choiceName3 \[3\] SetType1,**
>
> **choiceName4 \[4\] EnumeratedType2,**
>
> **choiceName5 \[5\] AS-REQ**
>
> **choiceName6 \[6\] ChoiceType1**
>
> **}**

**C\# Class for Example i:**
```
    class ChoiceType1 : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long choiceName1 = 0;
        [Asn1ChoiceIndex]
        public const long choiceName2 = 1;
        [Asn1ChoiceIndex]
        public const long choiceName3 = 2;
        [Asn1ChoiceIndex]
        public const long choiceName4 = 3;
        [Asn1ChoiceIndex]
        public const long choiceName5 = 4;

        [Asn1ChoiceElement(choice1)]
        protected Asn1Integer placeHoder1 = null;
        [Asn1ChoiceElement(choice2)]
        protected Asn1OctetString placeHolder2 = null;
        [Asn1ChoiceElement(choice3)]
        protected SetType1 placeHolder3 = null;
        [Asn1ChoiceElement(choice4)]
        protected EnumeratedType2 placeHolder4 = null;
        [Asn1ChoiceElement(choice5)]
        protected AS_REQ placeHolder5 = null;
    }
```

**C\# Class for Example ii:**
```
    [Asn1Tag(Asn1TagType.Application, 2)]
    class ChoiceType2 : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long choiceName1 = 0;
        [Asn1ChoiceIndex]
        public const long choiceName2 = 1;
        [Asn1ChoiceIndex]
        public const long choiceName3 = 2;
        [Asn1ChoiceIndex]
        public const long choiceName4 = 3;
        [Asn1ChoiceIndex]
        public const long choiceName5 = 4;
        [Asn1ChoiceIndex]
        public const long choiceName6 = 5;

        [Asn1ChoiceElement(choice1), Asn1Tag(Asn1TagType.Context, 1)]
        protected Asn1Integer placeHoder1 = null;
        [Asn1ChoiceElement(choice2), Asn1Tag(Asn1TagType.Context, 2)]
        protected Asn1OctetString placeHolder2 = null;
        [Asn1ChoiceElement(choice3), Asn1Tag(Asn1TagType.Context, 3)]
        protected SetType1 placeHolder3 = null;
        [Asn1ChoiceElement(choice4), Asn1Tag(Asn1TagType.Context, 4)]
        protected EnumeratedType2 placeHolder4 = null;
        [Asn1ChoiceElement(choice5), Asn1Tag(Asn1TagType.Context, 5)]
        protected AS_REQ placeHolder5 = null;
        [Asn1ChoiceElement(choice6), Asn1Tag(Asn1TagType.Context, 6)]
        protected ChoiceType1 placeHolder6 = null;
    }
```

SEQUENCE/SET OF Type
--------------------

A SEQUENCE/SET OF type has the form as follows:

**SEQUENCE OF SomeType**

**SET OF SomeType**

It may appear in a Referencing Type as follows:

**NewType ::= SET OF SomeType**

*Note. SomeType may be a temporary type without an explicit name.*

Or it may appear in a SEQUENCE/SET/CHOICE Type as follows:

>**NewType ::= SET/SEQUENCE/CHOICE **

>**{**

> **fieldName1 SEQUENCE OF SomeType, **
>
> **fieldName2 SET OF SomeType,**
>
> **…. **
>
> **}**

**Rule 1.** For each occurrence of SEQUENCE/SET OF SomeType in ASN.1 definition, SequenceOfSameType&lt;SomeType&gt; or SetOfSameType&lt;SomeType&gt; should be used in the C\# definition, respectively.

**Example: **

1.  **SeqOfType1** <span id="OLE_LINK21" class="anchor"><span id="OLE_LINK22" class="anchor"></span></span>**::= SEQUENCE OF SetType1**

2.  <span id="OLE_LINK1" class="anchor"></span>**KDC-REQ ::= SEQUENCE OF SEQUENCE{**

    **fieldName1 AppInt32, **

    **fieldName2 OCTET STRING**

     **}**

1.  **SeqType3::= SEQUENCE {**

     **fieldName1 INTEGER, **

     **fieldName2 OCTET STRING, **

     **fieldName3 SEQUENCE OF SetType1,**

     **fieldName4 SET OF SeqType2**

     **}**

1.  **ChoiceType3 ::= \[APPLICATION 2\] CHOICE{**

     **choiceName1 \[1\] INTEGER,**
 
     **choiceName2 \[2\] SEQUENCE OF OCTET STRING,**
 
     **choiceName3 \[3\] SET OF SeqType2**
 
    **}**

**C\# Class for Example i:**
```
    class SeqOfType1 :Asn1SequenceOfSameType<SetType1> 
    {
    }
```

**C\# Class for Example ii:**
```
    class KDC_REQElement : Asn1Sequence 
    {
        [Asn1Field(0)]
        public AppInt32 fieldName1;
        [Asn1Field(1)]
        public Asn1OctetString fieldName2;
    }
    class KDC_REQ : Asn1SequenceOfSameType<KDC_REQElement> 
    {
    }
```

**C\# Class for Example iii:**
```
    class SeqType3 : Asn1Sequence
    {
        [Asn1Field(0)]
        public Asn1Integer fieldName1;
        [Asn1Field(1)]
        public Asn1OctetString fieldName2;
        [Asn1Field(2)]
        public Asn1SequenceOfSameType<SetType1> fieldName3;
        [Asn1Field(3)]
        public Asn1SetOfSameType<SeqType2> fieldName4;
    }
```

**C\# Class for Example iv:**
```
    [Asn1Tag(Asn1TagType.Application, 2)]
    class ChoiceType3 : Asn1Choice
    {
        [Asn1ChoiceIndex]
        public const long choiceName1 = 0;
        [Asn1ChoiceIndex]
        public const long choiceName2 = 1;
        [Asn1ChoiceIndex]
        public const long choiceName3 = 2;

        [Asn1ChoiceElement(choice1), Asn1Tag(Asn1TagType.Context, 1)]
        protected Asn1Integer placeHoder1 = null;
        [Asn1ChoiceElement(choice2), Asn1Tag(Asn1TagType.Context, 2)]
        protected Asn1SequenceOfSameType<Asn1OctetString> placeHolder2 = null;
        [Asn1ChoiceElement(choice3), Asn1Tag(Asn1TagType.Context, 3)]
        protected Asn1SetOfSameType<SeqType2> placeHolder3 = null;
    }
```

Usage of the defined classes
============================

Primitive Type and Types that reference to Primitive Types
----------------------------------------------------------

For INTEGER, BOOLEAN, access the data by “Value” property.

For Strings, access the data by “Value” property and access the length by “Length” property. Do not use “Value.Length” since it’s not accurate in some types, e.g., BIT STRING.

Enumerated Type
---------------

Enumerated Types are implemented by INTEGER. Therefore any long values could be stored in the instances. However, the value may not be a legal value based on the type’s definition. To make it more clear, use the constant values defined in the type other than a bare long value. Here is the example.

**Recommended:**
```
            EnumeratedType1 obj1 = new EnumeratedType1();
            obj1.Value = EnumeratedType1.enumName2;
            if(obj1.Value == EnumeratedType1.enumName3)
            {
                //do something
            }
```

**Not recommended (But it actually works):**
```
            EnumeratedType1 obj1 = new EnumeratedType1();
            obj1.Value = 1;
            if(obj1.Value == 2)
            {
                //do something
            }
```

SEQUENCE/SET Type
-----------------

Just access the data by the corresponding field.

CHOICE Type
-----------

Store the data to a CHOICE Type by the “SetData” method. The method has two parameters: 

*index*:

 Type: long?

 The corresponding index of the selected choice defined in the type for obj. Using of the constant values defined in the class as this parameter is recommended (Ref: 5.D.Rule 2). If the value is null, the stored data will be clear (the obj parameter will be ignored in this case).

*obj*:

Type: Asn1Object

The data to be stored in the CHOICE Type.

Get the data stored in a CHOICE by the “GetData” method.

**Example: **
```
            SetType1 st1 = new SetType1();
            //Store datas to st1
            ChoiceType1 ct1 = new ChoiceType1();
            ct1.SetData(ChoiceType1.choiceName3, st1); 
            //Type of choice3 is SetType1
	        //Directly using a long value also works but it’s not recommended
            Asn1Object obj = ct1.GetData();
```

SEQUENCE/SET OF Type
--------------------

Access the data by the “Elements” property. “Elements” is an array, of which the type is the corresponding type in the SETQUENCE/SET OF definition.

Encoding/Decoding Way for Tags
------------------------------

A tag will be encoded in two different ways: Primitive and Constructed. Specifying the encoding way by force is supported via the named parameter EncodingWay in Asn1Tag. Generally, the default value of “EncodingWay” works correctly. That means in most cases the “EncodingWay” parameter should not be used.

**Example:**
```
    [Asn1Tag(Asn1TagType.Application, 10, EncodingWay = EncodingWay.Primitive)]
    class Force : Asn1Sequence
    {
        [Asn1Tag(Asn1TagType.Context, 0, EncodingWay = EncodingWay.Constructed)]
        public AppInt32 fieldName1;
        //Other fields
    }
```

Customize the Encoding/Decoding Procedure
-----------------------------------------

The encoding/decoding methods for Tags, Lengths, Values and the method for the encoding framework could all be overrode. Override them only if it is really needed.
```
class SomeType : Asn1Sequence
    {
        //Fields are omitted

        protected override int LengthBerEncode(IAsn1ReversedEncodingBuffer buffer, Asn1Tag tag)
        {
            //Customized prodecure of encoding a length
        }
        protected override int LengthBerDecode(IAsn1DecodingBuffer buffer, out Asn1Tag tag)
        {
            //Customized prodecure of decoding a length
        }
        //Other customized methods
        public override int BerEncode(IAsn1ReversedEncodingBuffer buffer)
        {
            //Customized BER encoding framework 
        }
        public override int BerDecode(IAsn1DecodingBuffer buffer)
        {
            //Customized BER decoding framework 
        }
}

```
Appendix 1. The ASN.1 Module used in this UG
============================================

UserGuide DEFINITIONS ::= BEGIN

Flag ::= BOOLEAN

Int32 ::= INTEGER (-2147483648..2147483647)

AppInt32 ::= \[APPLICATION 10\] INTEGER (5)

AS-REQ ::= \[APPLICATION 11\] KDC-REQ

KerberosFlags ::= BIT STRING (SIZE (32..MAX))

EnumeratedType1 ::= ENUMERATED{

enumName1,

enumName2,

enumName3

}

EnumeratedType2 ::= \[APPLICATION 1\] ENUMERATED{

enumName1 (10),

enumName2 (20),

enumName3 (30),

enumName4 (40)

}

SetType1 ::= SET {

fieldName1 Int32,

fieldName2 AppInt32,

fieldName3 OCTET STRING,

fieldName4 AS-REQ,

fieldName5 EnumeratedType2

}

SeqType2 ::= \[APPLICATION 20\] SEQUENCE{

fieldName1 \[0\] Int32,

fieldName2 \[1\] AppInt32 OPTIONAL,

fieldName3 \[2\] OCTET STRING OPTIONAL,

fieldName4 \[3\] AS-REQ,

fieldName5 \[4\] EnumeratedType2 OPTIONAL,

fieldName6 \[5\] SetType1

}

ChoiceType1 ::= CHOICE{

choiceName1 INTEGER,

choiceName2 OCTET STRING,

choiceName3 SetType1,

choiceName4 EnumeratedType2,

choiceName5 AS-REQ

}

ChoiceType2 ::= \[APPLICATION 2\] CHOICE{

choiceName1 \[1\] INTEGER,

choiceName2 \[2\] OCTET STRING,

choiceName3 \[3\] SetType1,

choiceName4 \[4\] EnumeratedType2,

choiceName5 \[5\] AS-REQ,

choiceName6 \[6\] ChoiceType1

}

SeqOfType1 ::= SEQUENCE OF SetType1

KDC-REQ ::= SEQUENCE OF SEQUENCE{

fieldName1 AppInt32,

fieldName2 OCTET STRING

}

SeqType3::= SEQUENCE {

fieldName1 INTEGER,

fieldName2 OCTET STRING,

fieldName3 SEQUENCE OF SetType1,

fieldName4 SET OF SeqType2

}

ChoiceType3 ::= \[APPLICATION 2\] CHOICE{

choiceName1 \[1\] INTEGER,

choiceName2 \[2\] SEQUENCE OF OCTET STRING,

choiceName3 \[3\] SET OF SeqType2

}

END
