// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory
{
    /// <summary>
    /// it's a simple solution before we implement [MS-CTA]. Now only supports "denyallexcept" option
    /// </summary>
    internal class SimpleCTAEngine
    {
        internal static List<CLAIMS_ARRAY> Run(CLAIMS_ARRAY[] input, string ruletext)
        {
            List<CLAIMS_ARRAY> ret = new List<CLAIMS_ARRAY>();
            string[] rules = ruletext.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, bool> supportedClaimIDs = new Dictionary<string, bool>();
            foreach (string r in rules)
            {
                if (!r.StartsWith("C1:[Type==\"") || !r.EndsWith("\"]=>Issue(claim=C1)"))
                {
                    throw new Exception("unsupported claim type from Simple CTA engine. Only valid kind is C1:[Type==\"%your claim id%\"]=>Issue(claim=C1)");
                }
                supportedClaimIDs.Add(r.Replace("C1:[Type==\"", "").Replace("\"]=>Issue(claim=C1)", ""), true);
            }
            foreach(CLAIMS_ARRAY array in input)
            {
                List<CLAIM_ENTRY> tmp = new List<CLAIM_ENTRY>();
                foreach (CLAIM_ENTRY entry in array.ClaimEntries)
                {
                    if (supportedClaimIDs.ContainsKey(entry.Id))
                    {
                        tmp.Add(entry);
                    }
                }
                if (tmp.Count > 0)
                {
                    CLAIMS_ARRAY tmpArray = new CLAIMS_ARRAY();
                    tmpArray.ClaimEntries = tmp.ToArray();
                    tmpArray.ulClaimsCount = (uint)tmpArray.ClaimEntries.Length;
                    tmpArray.usClaimsSourceType = array.usClaimsSourceType;
                    ret.Add(tmpArray);
                }
            }
            
            return ret;
        }
    }
}
