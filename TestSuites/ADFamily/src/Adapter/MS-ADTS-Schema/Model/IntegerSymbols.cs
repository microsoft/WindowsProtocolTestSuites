// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Provides a method to retrieve enumeration types for representing symbolic values
    /// of certain attributes.
    /// </summary>
    public static class IntegerSymbols
    {
        /// <summary>
        /// Gets an enumeration type which provide symbols 
        /// used in the TD to represent certain numeric values for attributes.
        /// </summary>
        /// <param name="attrName">The name of attribute.</param>
        /// <returns>Returns an enum type which provide symbols.</returns>
        public static Type GetSymbolEnumType(string attrName)
        {
            switch (attrName.ToLower())
            {
                case "schemaflagsex":
                    return typeof(SchemaFlagsEx);
                case "searchflags":
                    return typeof(SearchFlags);
                case "systemflags":
                    return typeof(SystemFlags);
                case "objectclasscategory":
                    return typeof(ObjectClassCategory);
                case "useraccountcontrol":
                    return typeof(UserAccountControl);
                default:
                    return null;
            }
        }
    }


    /// <summary>
    /// Symbols used in representing values of systemFlags attribute.
    /// </summary>
    [Flags]
    public enum SystemFlags : uint
    {
        /// <summary>
        /// FLAG_ATTR_NOT_REPLICATED
        /// </summary>
        FLAG_ATTR_NOT_REPLICATED = 0x1u,

        /// <summary>
        /// FLAG_ATTR_REQ_PARTIAL_SET_MEMBER
        /// </summary>
        FLAG_ATTR_REQ_PARTIAL_SET_MEMBER = 0x1u << 1,

        /// <summary>
        /// FLAG_ATTR_IS_CONSTRUCTED
        /// </summary>
        FLAG_ATTR_IS_CONSTRUCTED = 0x1u << 2,

        /// <summary>
        /// FLAG_ATTR_IS_OPERATIONAL
        /// </summary>
        FLAG_ATTR_IS_OPERATIONAL = 0x1u << 3,

        /// <summary>
        /// FLAG_SCHEMA_BASE_OBJECT
        /// </summary>
        FLAG_SCHEMA_BASE_OBJECT = 0x1u << 4,

        /// <summary>
        /// FLAG_ATTR_IS_RDN
        /// </summary>
        FLAG_ATTR_IS_RDN = 0x1u << 5,

        /// <summary>
        /// FLAG_DISALLOW_MOVE_ON_DELETE
        /// </summary>
        FLAG_DISALLOW_MOVE_ON_DELETE = 0x1u << 25,

        /// <summary>
        /// FLAG_DOMAIN_DISALLOW_MOVE
        /// </summary>
        FLAG_DOMAIN_DISALLOW_MOVE = 0x1u << 26,

        /// <summary>
        /// FLAG_DOMAIN_DISALLOW_RENAME
        /// </summary>
        FLAG_DOMAIN_DISALLOW_RENAME = 0x1u << 27,

        /// <summary>
        /// FLAG_CONFIG_ALLOW_LIMITED_MOVE
        /// </summary>
        FLAG_CONFIG_ALLOW_LIMITED_MOVE = 0x1u << 28,

        /// <summary>
        /// FLAG_CONFIG_ALLOW_MOVE
        /// </summary>
        FLAG_CONFIG_ALLOW_MOVE = 0x1u << 29,

        /// <summary>
        /// FLAG_CONFIG_ALLOW_RENAME
        /// </summary>
        FLAG_CONFIG_ALLOW_RENAME = 0x1u << 30,

        /// <summary>
        /// FLAG_DISALLOW_DELETE
        /// </summary>
        FLAG_DISALLOW_DELETE = 0x1u << 31,
    }


    /// <summary>
    /// Symbols used in representing values of schemaExFlags attribute.
    /// </summary>
    [Flags]
    public enum SchemaFlagsEx : uint
    {
        /// <summary>
        /// FLAG_ATTR_IS_CRITICAL
        /// </summary>
        FLAG_ATTR_IS_CRITICAL = 0x1u,
    }


    /// <summary>
    /// Symbols used in representing values of searchFlags attribute.
    /// </summary>
    [Flags]
    public enum SearchFlags : uint
    {
        /// <summary>
        /// fATTINDEX
        /// </summary>
        fATTINDEX = 0x1u,

        /// <summary>
        /// fPDNTATTINDEX
        /// </summary>
        fPDNTATTINDEX = 0x1u << 1,

        /// <summary>
        /// fANR
        /// </summary>
        fANR = 0x1u << 2,

        /// <summary>
        /// fPRESERVEONDELETE
        /// </summary>
        fPRESERVEONDELETE = 0x1u << 3,

        /// <summary>
        /// fCOPY
        /// </summary>
        fCOPY = 0x1u << 4,

        /// <summary>
        /// fTUPLEINDEX
        /// </summary>
        fTUPLEINDEX = 0x1u << 5,

        /// <summary>
        /// fSUBTREEATTINDEX
        /// </summary>
        fSUBTREEATTINDEX = 0x1u << 6,

        /// <summary>
        /// fCONFIDENTIAL
        /// </summary>
        fCONFIDENTIAL = 0x1u << 7,

        /// <summary>
        /// fNEVERVALUEAUDIT
        /// </summary>
        fNEVERVALUEAUDIT = 0x1u << 8,

        /// <summary>
        /// fRODCFILTEREDATTRIBUTE
        /// </summary>
        fRODCFILTEREDATTRIBUTE = 0x1u << 9,

        /// <summary>
        /// fEXTENDEDLINKTRACKING
        /// </summary>
        fEXTENDEDLINKTRACKING = 0x1u << 10,

        /// <summary>
        /// fEXTENDEDLINKTRACKING
        /// </summary>
        fBASEONLY = 0x1u << 11,

        /// <summary>
        /// fEXTENDEDLINKTRACKING
        /// </summary>
        fPARTITIONSECRET = 0x1u << 12
    }


    /// <summary>
    /// Symbols used in representing object class category.
    /// </summary>
    public enum ObjectClassCategory : uint
    {
        /// <summary>
        /// _88Class
        /// </summary>
        _88Class = 0x0,

        /// <summary>
        /// StructuralClass
        /// </summary>
        StructuralClass = 0x1,

        /// <summary>
        /// AbstractClass
        /// </summary>
        AbstractClass = 0x2,

        /// <summary>
        /// AuxiliaryClass
        /// </summary>
        AuxiliaryClass = 0x3
    }


    public enum UserAccountControl : uint
    {
        ADS_UF_ACCOUNT_DISABLE = 0x1u << 1,
        ADS_UF_HOMEDIR_REQUIRED = 0x1u << 3,
        ADS_UF_LOCKOUT = 0x1u << 4,
        ADS_UF_PASSWD_NOTREQD = 0x1u << 5,
        ADS_UF_PASSWD_CANT_CHANGE = 0x1u << 6,
        ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x1u << 7,
        ADS_UF_NORMAL_ACCOUNT = 0x1u << 9,
        ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 0x1u << 11,
        ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0x1u << 12,
        ADS_UF_SERVER_TRUST_ACCOUNT = 0x1u << 13,
        ADS_UF_DONT_EXPIRE_PASSWD = 0x1u << 16,
        ADS_UF_SMARTCARD_REQUIRED = 0x1u << 18,
        ADS_UF_TRUSTED_FOR_DELEGATION = 0x1u << 19,
        ADS_UF_NOT_DELEGATED = 0x1u << 20,
        ADS_UF_USE_DES_KEY_ONLY = 0x1u << 21,
        ADS_UF_DONT_REQUIRE_PREAUTH = 0x1u << 22,
        ADS_UF_PASSWORD_EXPIRED = 0x1u << 23,
        ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x1u << 24,
        ADS_UF_NO_AUTH_DATA_REQUIRED = 0x1u << 25,
        ADS_UF_PARTIAL_SECRETS_ACCOUNT = 0x1u << 26
    }
}
