// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CRestriction_ulType_Values : uint
    {
        /// <summary>
        /// The node contains an empty Restriction (no value). It represents a node that would evaluate to no results.
        /// If under an RTNot node, the RTNot node would evaluate to the entire set of documents. If under an RTAnd node, the RTAnd node would also evaluate to no results.
        /// If under an RTOr node, the RTOr node would evaluate to whatever it would have evaluated to if the RTNone node was not there.
        /// </summary>
        RTNone = 0x00000000,

        /// <summary>
        /// The node contains a CNodeRestriction on which a logical AND operation is to be performed. The CNodeRestriction contains the other restrictions that this operation is performed on.
        /// </summary>
        RTAnd = 0x00000001,

        /// <summary>
        /// The node contains a CNodeRestriction on which a logical OR (disjunction) operation is to be performed. The CNodeRestriction contains the other restrictions that this operation is performed on.
        /// </summary>
        RTOr = 0x00000002,

        /// <summary>
        /// The node contains a CRestriction on which a NOT operation is to be performed.
        /// </summary>
        RTNot = 0x00000003,

        /// <summary>
        /// The node contains a CContentRestriction.
        /// </summary>
        RTContent = 0x00000004,

        /// <summary>
        /// The node contains a CPropertyRestriction.
        /// </summary>
        RTProperty = 0x00000005,

        /// <summary>
        /// The node contains a CNodeRestriction with an array of CContentRestriction structures.
        /// Any other kind of restriction is undefined.
        /// The restriction requires the words or phrases found in the CContentRestriction structures to be within the GSS defined range in order to be a match.
        /// The WSS implementation computes a rank based on how far apart the words or phrases are. Implementations of the GSS can choose to do the same.
        /// </summary>
        RTProximity = 0x00000006,

        /// <summary>
        /// The node contains a CVectorRestriction.
        /// </summary>
        RTVector = 0x00000007,

        /// <summary>
        /// The node contains a CNatLanguageRestriction.
        /// </summary>
        RTNatLanguage = 0x00000008,

        /// <summary>
        /// The node contains a CScopeRestriction.
        /// </summary>
        RTScope = 0x00000009,

        /// <summary>
        /// The node contains a CReuseWhere restriction.
        /// </summary>
        RTReuseWhere = 0x00000011,

        /// <summary>
        /// The node contains a CInternalPropertyRestriction.
        /// </summary>
        RTInternalProp = 0x00FFFFFA,

        /// <summary>
        /// The node contains a CNodeRestriction on which a phrase match is to be performed. The restrictions in the CNodeRestriction can only be a RTContent node. Otherwise, an error MUST be returned.
        /// </summary>
        RTPhrase = 0x00FFFFFD,

        /// <summary>
        /// The node contains a CCoercionRestriction structure with operation ADD, as specified in section 2.2.1.12.
        /// </summary>
        RTCoerce_Add = 0x0000000A,

        /// <summary>
        /// The node contains a CCoercionRestriction with structure operation MULTIPLY, as specified in section 2.2.1.12.
        /// </summary>
        RTCoerce_Multiply = 0x0000000B,

        /// <summary>
        /// The node contains a CCoercionRestriction structure with operation ABSOLUTE, as specified in section 2.2.1.12.
        /// </summary>
        RTCoerce_Absolute = 0x0000000C,

        /// <summary>
        /// The node contains a CProbRestriction structure.
        /// </summary>
        RTProb = 0x0000000D,

        /// <summary>
        /// The node contains a CFeedbackRestriction structure.
        /// </summary>
        RTFeedback = 0x0000000E,

        /// <summary>
        /// The node contains a CRelDocRestriction structure.
        /// </summary>
        RTReldoc = 0x0000000F,
    }

    /// <summary>
    /// The CRestriction structure contains a restriction node in a query command tree.
    /// </summary>
    public struct CRestriction : IWspRestriction
    {
        /// <summary>
        /// A 32-bit unsigned integer indicating the restriction type used for the command tree node. The type determines what is found in the Restriction field of the structure.
        /// </summary>
        public CRestriction_ulType_Values _ulType;

        /// <summary>
        /// A 32-bit unsigned integer representing the weight of the node.
        /// Weight indicates the node's importance relative to other nodes in the query command tree.
        /// This weight is used to calculate the rank of each node, although the exact effect of the weight is undefined.
        /// The guidance is that results from higher-weighted nodes usually return a higher rank than results from nodes that are the same but weighted lower.
        /// Implementers of the GSS can choose the exact algorithm. 
        /// </summary>
        public uint Weight;

        /// <summary>
        /// The restriction type for the command tree node. The syntax MUST be as indicated by the _ulType field.
        /// </summary>
        public IWspRestriction Restriction;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_ulType);

            buffer.Add(Weight);

            switch (_ulType)
            {
                case CRestriction_ulType_Values.RTNone:
                    {

                    }
                    break;

                case CRestriction_ulType_Values.RTAnd:
                    {
                        ((CNodeRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTOr:
                    {
                        ((CNodeRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTNot:
                    {
                        ((CRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTContent:
                    {
                        ((CContentRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTProperty:
                    {
                        ((CPropertyRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTProximity:
                    {
                        ((CNodeRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTVector:
                    {
                        ((CVectorRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTNatLanguage:
                    {
                        ((CNatLanguageRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTScope:
                    {
                        ((CScopeRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTReuseWhere:
                    {
                        ((CReuseWhere)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTInternalProp:
                    {
                        ((CInternalPropertyRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTPhrase:
                    {
                        ((CNodeRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTCoerce_Add:
                    {
                        ((CCoercionRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTCoerce_Multiply:
                    {
                        ((CCoercionRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTCoerce_Absolute:
                    {
                        ((CCoercionRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTProb:
                    {
                        ((CProbRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTFeedback:
                    {
                        ((CFeedbackRestriction)Restriction).ToBytes(buffer);
                    }
                    break;

                case CRestriction_ulType_Values.RTReldoc:
                    {
                        ((CRelDocRestriction)Restriction).ToBytes(buffer);
                    }
                    break;
            }
        }
    }
}
