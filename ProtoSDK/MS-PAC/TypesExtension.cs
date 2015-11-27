// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{

    public partial struct PAC_SIGNATURE_DATA
    {
        /// <summary>
        /// Customized length calculator of Signature, called by Channel.
        /// This method's name is written in Signature's Size attribute. 
        /// </summary>
        /// <param name="context">Channel's context.</param>
        /// <returns>Signature's length.</returns>
        // this method is called by Channel.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static int CalculateSignatureLength(IEvaluationContext context)
        {
            PAC_SIGNATURE_DATA_SignatureType_Values signatureType = (PAC_SIGNATURE_DATA_SignatureType_Values)context.Variables["SignatureType"];
            return PacSignatureData.CalculateSignatureLength(signatureType);
        }
    }

  

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct PAC_CLIENT_CLAIMS_INFO
    {
        public CLAIMS_SET_METADATA Claims;
    }


    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct PAC_DEVICE_CLAIMS_INFO
    {
        public CLAIMS_SET_METADATA Claims;
    }





}
