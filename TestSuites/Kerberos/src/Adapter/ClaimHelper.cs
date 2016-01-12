// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public class ClaimHelper
    {
        static CLAIMS_ARRAY[] sourceClaims = new CLAIMS_ARRAY[0];

        /// <summary>
        /// Find same claim record from internal database
        /// </summary>
        /// <param name="principalDN">Distinguished Name of principal</param>
        /// <param name="principalClass">principal type</param>
        /// <param name="sourceType">claim source type, AD or certificate</param>
        /// <param name="claimID">ID of claim</param>
        /// <param name="valueType">claim value type</param>
        /// <param name="unobjected_values">values parsed into string and split with |ClaimUtilitySpliter|</param>
        /// <returns>true if found matched</returns>
        public static bool FoundMatchedClaim(string principalDN, ClaimsPrincipalClass principalClass, CLAIMS_SOURCE_TYPE sourceType, string claimID, CLAIM_TYPE valueType, string unobjected_values)
        {
            #region parse values from the string
            object[] values = ClaimUtility.ConvertStringToEntryUnion(valueType, unobjected_values);
            #endregion

            #region find same claim record
            for (int i = 0; i < sourceClaims.Length; i++)
            {
                if (sourceClaims[i].usClaimsSourceType == (short)sourceType)
                {
                    for (int j = 0; j < sourceClaims[i].ClaimEntries.Length; j++)
                    {
                        CLAIM_ENTRY entry = sourceClaims[i].ClaimEntries[j];
                        if (entry.Id == claimID && entry.Type == valueType)
                        {
                            //found claim with same ID and value type, need to check values
                            switch (valueType)
                            {
                                case CLAIM_TYPE.CLAIM_TYPE_BOOLEAN:
                                    {
                                        if (entry.Values.Struct4.BooleanValues.Length != values.Length)
                                            return false;

                                        for (int k = 0; k < entry.Values.Struct4.BooleanValues.Length; k++)
                                        {

                                            if ((bool)values[k] != entry.Values.Struct4.BooleanValues[k])
                                                return false;
                                        }
                                        return true;
                                    }
                                case CLAIM_TYPE.CLAIM_TYPE_INT64:
                                    {
                                        if (entry.Values.Struct1.Int64Values.Length != values.Length)
                                            return false;

                                        for (int k = 0; k < entry.Values.Struct1.Int64Values.Length; k++)
                                        {
                                            if ((int)values[k] != entry.Values.Struct1.Int64Values[k])
                                                return false;
                                        }
                                        return true;
                                    }
                                case CLAIM_TYPE.CLAIM_TYPE_STRING:
                                    {
                                        if (entry.Values.Struct3.StringValues.Length != values.Length)
                                            return false;

                                        for (int k = 0; k < entry.Values.Struct3.StringValues.Length; k++)
                                        {

                                            if ((string)values[k] != entry.Values.Struct3.StringValues[k])
                                                return false;
                                        }
                                        return true;
                                    }
                                case CLAIM_TYPE.CLAIM_TYPE_UINT64:
                                    {
                                        if (entry.Values.Struct2.Uint64Values.Length != values.Length)
                                            return false;

                                        for (int k = 0; k < entry.Values.Struct2.Uint64Values.Length; k++)
                                        {

                                            if ((uint)values[k] != entry.Values.Struct2.Uint64Values[k])
                                                return false;
                                        }
                                        return true;
                                    }
                            }
                        }
                    }
                }
            }
            #endregion
            return false;
        }

        /// <summary>
        /// load claims from dc
        /// </summary>
        /// <param name="principal">Distinguished Name of principal</param>
        /// <param name="principalClass">class type of principal</param>
        /// <param name="dc">Domain Controller name or address</param>
        /// <param name="domain">domain DNS name</param>
        /// <param name="user">admin user name</param>
        /// <param name="password">admin user password</param>
        public static void LoadClaims(string principal, ClaimsPrincipalClass principalClass, string dc, string domain, string user, string password)
        {
            ClaimsLoader cLoader = new ClaimsLoader(dc, domain, user, password);

            List<CLAIMS_ARRAY> adClaims = cLoader.GetClaimsForPrincipalWithoutEncode(principal, ClaimsPrincipalClass.User, ClaimsSource.AD | ClaimsSource.Certificate);

            sourceClaims = adClaims.ToArray();
        }
    }

    /// <summary>
    /// utility for claim
    /// </summary>
    public class ClaimUtility
    {
        /// <summary>
        /// spliter
        /// </summary>
        const string spliter = "|ClaimUtilitySpliter|";

        /// <summary>
        /// convert unions into a string
        /// </summary>
        /// <param name="type">claim value type</param>
        /// <param name="values">the union</param>
        /// <returns>result string</returns>
        public static string ConvertEntryUniontoString(CLAIM_TYPE type, CLAIM_ENTRY_VALUE_UNION values)
        {
            string ret = "";
            switch (type)
            {
                case CLAIM_TYPE.CLAIM_TYPE_BOOLEAN:
                    for (int i = 0; i < values.Struct4.BooleanValues.Length; i++)
                    {
                        ret += values.Struct4.BooleanValues[i].ToString();
                        ret += spliter;
                    }
                    break;
                case CLAIM_TYPE.CLAIM_TYPE_INT64:
                    for (int i = 0; i < values.Struct1.Int64Values.Length; i++)
                    {
                        ret += values.Struct1.Int64Values[i].ToString();
                        ret += spliter;
                    }
                    break;
                case CLAIM_TYPE.CLAIM_TYPE_UINT64:
                    for (int i = 0; i < values.Struct2.Uint64Values.Length; i++)
                    {
                        ret += values.Struct2.Uint64Values[i].ToString();
                        ret += spliter;
                    }
                    break;
                case CLAIM_TYPE.CLAIM_TYPE_STRING:
                    for (int i = 0; i < values.Struct3.StringValues.Length; i++)
                    {
                        ret += values.Struct3.StringValues[i].ToString();
                        ret += spliter;
                    }
                    break;
            }
            return ret;
        }

        /// <summary>
        /// parse string into claim union
        /// </summary>
        /// <param name="type">claim value type</param>
        /// <param name="unobjected_values">the string</param>
        /// <returns>union</returns>
        public static object[] ConvertStringToEntryUnion(CLAIM_TYPE type, string unobjected_values)
        {

            string[] unparsed = unobjected_values.Split(new string[] { spliter }, StringSplitOptions.RemoveEmptyEntries);
            object[] values = new object[unparsed.Length];
            for (int i = 0; i < values.Length; i++)
            {
                switch (type)
                {
                    case CLAIM_TYPE.CLAIM_TYPE_BOOLEAN:
                        values[i] = bool.Parse(unparsed[i]);
                        break;
                    case CLAIM_TYPE.CLAIM_TYPE_INT64:
                        values[i] = long.Parse(unparsed[i]);
                        break;
                    case CLAIM_TYPE.CLAIM_TYPE_STRING:
                        values[i] = unparsed[i];
                        break;
                    case CLAIM_TYPE.CLAIM_TYPE_UINT64:
                        values[i] = ulong.Parse(unparsed[i]);
                        break;
                }
            }

            return values;
        }
    }
}
