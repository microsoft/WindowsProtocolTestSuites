// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    /// <summary>
    /// MS-FSA model program
    /// </summary>
    public static partial class ModelProgram
    {
        #region 3.1.5.13    Application Requests a Query of Security Information
        /// <summary>
        ///3.1.5.13    Application Requests a Query of Security Information 
        /// </summary>
        /// <param name="securityInformation">This is abstracted to check the value of SecurityInformation</param>
        /// <param name="isByteCountGreater">True: if ByteCount is greater than OutputBufferSize</param>
        /// <param name="outputBuffer">OutputBuffer :to get the out value of outputBuffer</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus QuerySecurityInfo(
            SecurityInformation securityInformation,
            bool isByteCountGreater,
            out OutputBuffer outputBuffer
            )
        {
            outputBuffer = new OutputBuffer();
            //SecurityInformation contains any of OWNER_SECURITY_INFORMATION, GROUP_SECURITY_INFORMATION, LABEL_SECURITY_INFORMATION, or DACL_SECURITY_INFORMATION
            if ((securityInformation == SecurityInformation.OWNER_SECURITY_INFORMATION || 
                securityInformation == SecurityInformation.GROUP_SECURITY_INFORMATION || 
                securityInformation == SecurityInformation.LABEL_SECURITY_INFORMATION || 
                securityInformation == SecurityInformation.DACL_SECURITY_INFORMATION) && 
                ((gOpenGrantedAccess & FileAccess.READ_CONTROL) == 0))
            {
                Helper.CaptureRequirement(2776, @"[In Server Requests a Query of Security Information,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_ACCESS_DENIED under either of the following conditions:
                    SecurityInformation contains any of OWNER_SECURITY_INFORMATION, GROUP_SECURITY_INFORMATION, LABEL_SECURITY_INFORMATION, 
                    or DACL_SECURITY_INFORMATION, and Open.GrantedAccess does not contain READ_CONTROL.");
                return MessageStatus.ACCESS_DENIED;
            }
            //SecurityInformation contains SACL_SECURITY_INFORMATION
            if ((securityInformation == SecurityInformation.SACL_SECURITY_INFORMATION) && ((gOpenGrantedAccess & FileAccess.ACCESS_SYSTEM_SECURITY) == 0))
            {
                Helper.CaptureRequirement(2777, @"[In Server Requests a Query of Security Information,Pseudocode for the operation is as follows:]
                    The operation MUST be failed with STATUS_ACCESS_DENIED under either of the following conditions:
                    SecurityInformation contains SACL_SECURITY_INFORMATION and Open.GrantedAccess does not contain ACCESS_SYSTEM_SECURITY.");
                return MessageStatus.ACCESS_DENIED;
            }
            // If ByteCount is greater than OutputBufferSize
            if (isByteCountGreater)
            {
                Helper.CaptureRequirement(2799, @"[In Server Requests a Query of Security Information,Pseudocode for the operation is as follows:]If ByteCount is greater than OutputBufferSize, the operation MUST be failed with STATUS_BUFFER_OVERFLOW.");
                return MessageStatus.BUFFER_OVERFLOW;
            }

            //The object store MUST set OutputBuffer.Revision equal to 1;
            outputBuffer.Revision = 1;
            Helper.CaptureRequirement(2800, "[In Server Requests a Query of Security Information,Pseudocode for the operation is as follows:]The object store MUST set OutputBuffer.Revision equal to 1.");
            Helper.CaptureRequirement(2769, @"[In Server Requests a Query of Security Information]On completion, 
                the object store MUST return:[Status,OutputBuffer,ByteCount].");
            Helper.CaptureRequirement(2832, @"[In Server Requests a Query of Security Information] 
                Pseudocode for the operation is as follows:The operation returns STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.16    Application Requests Setting of Security Information

        ///<summary>
        ///3.1.5.16    Application Requests Setting of Security Information
        ///</summary>
        ///<param name="securityInformation">This is abstracted to check the value of SecurityInformation</param>
        ///<param name="ownerSidEnum">ownerSidEnum abstracted to check the ownersid status </param>
        ///<returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus SetSecurityInfo(
            SecurityInformation securityInformation,
            OwnerSid ownerSidEnum
            )
        {
            //If the object store does not implement security, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.
            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(3232, @"[In Server Requests Setting of Security Information] If the object store does not implement security, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }
            // SecurityInformation contains any of OWNER_SECURITY_INFORMATION, GROUP_SECURITY_INFORMATION, or LABEL_SECURITY_INFORMATION, and Open.GrantedAccess does not contain WRITE_OWNER.
            if ((securityInformation == SecurityInformation.OWNER_SECURITY_INFORMATION || securityInformation == SecurityInformation.GROUP_SECURITY_INFORMATION || securityInformation == SecurityInformation.LABEL_SECURITY_INFORMATION) && ((gOpenGrantedAccess & FileAccess.WRITE_OWNER) == 0))
            {
                Helper.CaptureRequirement(3258, @"[In Server Requests Setting of Security Information]  
                    The operation MUST be failed with STATUS_ACCESS_DENIED under any of the following conditions:SecurityInformation 
                    contains any of LABEL_SECURITY_INFORMATION, and Open.GrantedAccess does not contain WRITE_OWNER.");
                Helper.CaptureRequirement(3256, @"[In Server Requests Setting of Security Information]  
                    The operation MUST be failed with STATUS_ACCESS_DENIED under any of the following conditions: 
                    SecurityInformation contains any of OWNER_SECURITY_INFORMATION,  and Open.GrantedAccess does not contain WRITE_OWNER.");
                Helper.CaptureRequirement(3257, @"[In Server Requests Setting of Security Information]  
                     The operation MUST be failed with STATUS_ACCESS_DENIED under any of the following conditions:
                     SecurityInformation contains any of GROUP_SECURITY_INFORMATION,, and Open.GrantedAccess does not contain WRITE_OWNER.");
                Helper.CaptureRequirement(3239, @"[In Server Requests Setting of Security Information]On completion, 
                        the object store MUST return:[Status].");
                return MessageStatus.ACCESS_DENIED;
            }
            // SecurityInformation contains DACL_SECURITY_INFORMATION and Open.GrantedAccess does not contain WRITE_DAC
            if ((securityInformation == SecurityInformation.DACL_SECURITY_INFORMATION) && ((gOpenGrantedAccess & FileAccess.WRITE_DAC) == 0))
            {
                Helper.CaptureRequirement(3243, @"[In Server Requests Setting of Security Information]  
                    The operation MUST be failed with STATUS_ACCESS_DENIED under any of the following conditions:
                    SecurityInformation contains DACL_SECURITY_INFORMATION and Open.GrantedAccess does not contain WRITE_DAC.");
                Helper.CaptureRequirement(3239, @"[In Server Requests Setting of Security Information]On completion, 
                        the object store MUST return:[Status].");
                return MessageStatus.ACCESS_DENIED;
            }
            // ? SecurityInformation contains SACL_SECURITY_INFORMATION and Open.GrantedAccess does not contain ACCESS_SYSTEM_SECURITY.
            if ((securityInformation == SecurityInformation.SACL_SECURITY_INFORMATION) && ((gOpenGrantedAccess & FileAccess.ACCESS_SYSTEM_SECURITY) == 0))
            {
                Helper.CaptureRequirement(3244, @"[In Server Requests Setting of Security Information]  
                    The operation MUST be failed with STATUS_ACCESS_DENIED under any of the following conditions: SecurityInformation contains SACL_SECURITY_INFORMATION and Open.GrantedAccess does not contain ACCESS_SYSTEM_SECURITY.");
                Helper.CaptureRequirement(3239, @"[In Server Requests Setting of Security Information]On completion, 
                        the object store MUST return:[Status].");
                return MessageStatus.ACCESS_DENIED;
            }

            // If SecurityInformation contains OWNER_SECURITY_INFORMATION
            if (securityInformation == SecurityInformation.OWNER_SECURITY_INFORMATION)
            {
                // If InputBuffer.OwnerSid is not present,
                if (ownerSidEnum == OwnerSid.InputBufferOwnerSidNotPresent)
                {
                    Helper.CaptureRequirement(3251, @"[In Server Requests Setting of Security Information, 
                        Pseudocode for the operation is as follows:If SecurityInformation contains OWNER_SECURITY_INFORMATION:]
                        If InputBuffer.OwnerSid is not present, the operation MUST be failed with STATUS_INVALID_OWNER.");
                    Helper.CaptureRequirement(3239, @"[In Server Requests Setting of Security Information]On completion, 
                        the object store MUST return:[Status].");
                    return MessageStatus.INVALID_OWNER;
                }
                if (ownerSidEnum == OwnerSid.InputBufferOwnerSidNotValid)
                {
                    Helper.CaptureRequirement(4584, @"[In Server Requests Setting of Security Information, 
                        Pseudocode for the operation is as follows:If SecurityInformation contains OWNER_SECURITY_INFORMATION:]
                        If InputBuffer.OwnerSid is not a valid owner SID for a file in the object store, as determined in an implementation-specific manner, 
                        the object store MUST return STATUS_INVALID_OWNER.");
                    Helper.CaptureRequirement(3239, @"[In Server Requests Setting of Security Information]On completion, 
                        the object store MUST return:[Status].");
                    return MessageStatus.INVALID_OWNER;
                }
            }
            else
            {
                // If Open.File.SecurityDescriptor.Owner is NULL
                if (ownerSidEnum == OwnerSid.OpenFileSecDesOwnerIsNull)
                {
                    Helper.CaptureRequirement(3252, @"[In Server Requests Setting of Security Information,Pseudocode for the operation is as follows:
                        else If SecurityInformation doesn't contain OWNER_SECURITY_INFORMATION:]If Open.File.SecurityDescriptor.Owner is NULL, 
                        the operation MUST be failed with STATUS_INVALID_OWNER.");
                    Helper.CaptureRequirement(3239, @"[In Server Requests Setting of Security Information]On completion, 
                        the object store MUST return:[Status].");
                    return MessageStatus.INVALID_OWNER;
                }
            }
            Helper.CaptureRequirement(3255, @"[In Server Requests Setting of Security Information,Pseudocode for the operation is as follows:]
                 The operation returns STATUS_SUCCESS.");
            Helper.CaptureRequirement(3239, @"[In Server Requests Setting of Security Information]On completion, 
                        the object store MUST return:[Status].");
            return MessageStatus.SUCCESS;
        }

        #endregion

    }
}
