// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    /// <summary>
    /// Represents LDAP result codes.
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// Result code of Success.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Result code of Success.
        /// </summary>
        OperationsError = 1,

        /// <summary>
        /// Result code of ProtocolError.
        /// </summary>
        ProtocolError = 2,

        /// <summary>
        /// Result code of TimeLimitExceeded.
        /// </summary>
        TimeLimitExceeded = 3,

        /// <summary>
        /// Result code of SizeLimitExceeded.
        /// </summary>
        SizeLimitExceeded = 4,

        /// <summary>
        /// Result code of CompareFalse.
        /// </summary>
        CompareFalse = 5,

        /// <summary>
        /// Result code of CompareTrue.
        /// </summary>
        CompareTrue = 6,

        /// <summary>
        /// Result code of AuthMethodNotSupported.
        /// </summary>
        AuthMethodNotSupported = 7,

        /// <summary>
        /// Result code of StrongerAuthRequired.
        /// </summary>
        StrongerAuthRequired = 8,

        /// <summary>
        /// Result code of Reserved.
        /// </summary>
        Reserved = 9,

        /// <summary>
        /// Result code of Referral.
        /// </summary>
        Referral = 10,

        /// <summary>
        /// Result code of AdminLimitExceeded.
        /// </summary>
        AdminLimitExceeded = 11,

        /// <summary>
        /// Result code of UnavailableCriticialExtension.
        /// </summary>
        UnavailableCriticialExtension = 12,

        /// <summary>
        /// Result code of ConfidentialityRequired.
        /// </summary>
        ConfidentialityRequired = 13,

        /// <summary>
        /// Result code of SaslBindInProgress.
        /// </summary>
        SaslBindInProgress = 14,

        /// <summary>
        /// Result code of Reserved2.
        /// </summary>
        Reserved2 = 15,

        /// <summary>
        /// Result code of NoSuchAttribute.
        /// </summary>
        NoSuchAttribute = 16,

        /// <summary>
        /// Result code of UndefinedAttributeType.
        /// </summary>
        UndefinedAttributeType = 17,

        /// <summary>
        /// Result code of InappropriateMatching.
        /// </summary>
        InappropriateMatching = 18,

        /// <summary>
        /// Result code of ConstraintViolation.
        /// </summary>
        ConstraintViolation = 19,

        /// <summary>
        /// Result code of AttributeOrValueExists.
        /// </summary>
        AttributeOrValueExists = 20,

        /// <summary>
        /// Result code of InvalidAttributeSyntax.
        /// </summary>
        InvalidAttributeSyntax = 21,

        /// <summary>
        /// Result code of NoSuchObject.
        /// </summary>
        NoSuchObject = 32,

        /// <summary>
        /// Result code of AliasProblem.
        /// </summary>
        AliasProblem = 33,

        /// <summary>
        /// Result code of InvalidDNSyntax.
        /// </summary>
        InvalidDNSyntax = 34,

        /// <summary>
        /// Result code of AliasDereferencingProblem.
        /// </summary>
        AliasDereferencingProblem = 36,

        /// <summary>
        /// Result code of InappropriateAuthentication.
        /// </summary>
        InappropriateAuthentication = 48,

        /// <summary>
        /// Result code of InvalidCredentials.
        /// </summary>
        InvalidCredentials = 49,

        /// <summary>
        /// Result code of InsufficientAccessRights.
        /// </summary>
        InsufficientAccessRights = 50,

        /// <summary>
        /// Result code of Busy.
        /// </summary>
        Busy = 51,

        /// <summary>
        /// Result code of Unavailable.
        /// </summary>
        Unavailable = 52,

        /// <summary>
        /// Result code of UnwillingToPerform.
        /// </summary>
        UnwillingToPerform = 53,

        /// <summary>
        /// Result code of LoopDetect.
        /// </summary>
        LoopDetect = 54,

        /// <summary>
        /// Result code of NamingViolation.
        /// </summary>
        NamingViolation = 64,

        /// <summary>
        /// Result code of ObjectClassViolation.
        /// </summary>
        ObjectClassViolation = 65,

        /// <summary>
        /// Result code of NotAllowedOnLeaf.
        /// </summary>
        NotAllowedOnLeaf = 66,

        /// <summary>
        /// Result code of NotAllowedOnRDN.
        /// </summary>
        NotAllowedOnRDN = 67,

        /// <summary>
        /// Result code of EntryAlreadyExists.
        /// </summary>
        EntryAlreadyExists = 68,

        /// <summary>
        /// Result code of ObjectClassModsProhibited.
        /// </summary>
        ObjectClassModsProhibited = 69,

        /// <summary>
        /// Result code of AffectsMultipleDSAs.
        /// </summary>
        AffectsMultipleDSAs = 71,

        /// <summary>
        /// Result code of Other.
        /// </summary>
        Other = 80
    }

    /// <summary>
    /// Represents an LDAP result, given by a code and a potential diagnostic message.
    /// </summary>
    public struct ModelResult
    {
        /// <summary>
        /// ResultCode object.
        /// </summary>
        public ResultCode resultCode;

        /// <summary>
        /// matchedDN string.
        /// </summary>
        public string matchedDN;

        /// <summary>
        /// diagnosticMessage string.
        /// </summary>
        public string diagnosticMessage;

        /// <summary>
        /// String to store log messages.
        /// </summary>
        public string logMessage;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">Result code of the operation.</param>
        public ModelResult(ResultCode code)
        {
            this.resultCode = code;
            this.diagnosticMessage = String.Empty;
            this.logMessage = String.Empty;
            this.matchedDN = null;
        }


        /// <summary>
        /// Overloaded Constructor.
        /// </summary>
        /// <param name="code">Result code of the operation</param>
        /// <param name="message">Message.</param>
        /// <param name="parameters">Parameter to the log message.</param>
        public ModelResult(ResultCode code, string message, params string[] parameters)
        {
            this.resultCode = code;
            this.diagnosticMessage = String.Format(message, parameters);
            this.logMessage = String.Empty;
            this.matchedDN = null;
        }


        /// <summary>
        /// Overloaded Constructor.
        /// </summary>
        /// <param name="code">Result code of the operation</param>
        /// <param name="logMsg">Log message.</param>
        /// <param name="message">Message.</param>
        /// <param name="parameters">Parameter to the log message.</param>
        public ModelResult(ResultCode code, string logMsg, string message, params string[] parameters)
        {
            this.resultCode = code;
            this.diagnosticMessage = String.Format(message, parameters);
            this.logMessage = logMsg;
            this.matchedDN = null;
        }


        /// <summary>
        /// It represents whether this result is success or not.
        /// </summary>
        public bool IsSuccess
        {
            get { return resultCode == ResultCode.Success; }
        }


        /// <summary>
        /// This property is used to represent the result of the operation.
        /// </summary>
        public static ModelResult Success
        {
            get { return new ModelResult(ResultCode.Success); }
        }     
    }
}
