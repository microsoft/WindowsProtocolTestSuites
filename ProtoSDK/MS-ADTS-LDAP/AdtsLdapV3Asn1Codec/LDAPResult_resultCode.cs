// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV3
{
    /*
    	LDAPResult_resultCode      ENUMERATED {
                             success                      (0),
                             operationsError              (1),
                             protocolError                (2),
                             timeLimitExceeded            (3),
                             sizeLimitExceeded            (4),
                             compareFalse                 (5),
                             compareTrue                  (6),
                             authMethodNotSupported       (7),
                             strongAuthRequired           (8),
                                        -- 9 reserved --
                             referral                     (10),  -- new
                             adminLimitExceeded           (11),  -- new
                             unavailableCriticalExtension (12),  -- new
                             confidentialityRequired      (13),  -- new
                             saslBindInProgress           (14),  -- new
                             noSuchAttribute              (16),
                             undefinedAttributeType       (17),
                             inappropriateMatching        (18),
                             constraintViolation          (19),
                             attributeOrValueExists       (20),
                             invalidAttributeSyntax       (21),
                                        -- 22-31 unused --
                             noSuchObject                 (32),
                             aliasProblem                 (33),
                             invalidDNSyntax              (34),
                             -- 35 reserved for undefined isLeaf --
                             aliasDereferencingProblem    (36),
                                        -- 37-47 unused --
                             inappropriateAuthentication  (48),
			     invalidCredentials           (49),
                             insufficientAccessRights     (50),
                             busy                         (51),
                             unavailable                  (52),
                             unwillingToPerform           (53),
                             loopDetect                   (54),
                                        -- 55-63 unused --
                             namingViolation              (64),
                             objectClassViolation         (65),
                             notAllowedOnNonLeaf          (66),
                             notAllowedOnRDN              (67),
                             entryAlreadyExists           (68),
                             objectClassModsProhibited    (69),
                                        -- 70 reserved for CLDAP --
                             affectsMultipleDSAs          (71), -- new
                                        -- 72-79 unused --
                             other                        (80) }
                             -- 81-90 reserved for APIs --
    */
    public class LDAPResult_resultCode : Asn1Enumerated
    {
        [Asn1EnumeratedElement]
        public const long success = 0;
        
        [Asn1EnumeratedElement]
        public const long operationsError = 1;
        
        [Asn1EnumeratedElement]
        public const long protocolError = 2;
        
        [Asn1EnumeratedElement]
        public const long timeLimitExceeded = 3;
        
        [Asn1EnumeratedElement]
        public const long sizeLimitExceeded = 4;
        
        [Asn1EnumeratedElement]
        public const long compareFalse = 5;
        
        [Asn1EnumeratedElement]
        public const long compareTrue = 6;
        
        [Asn1EnumeratedElement]
        public const long authMethodNotSupported = 7;
        
        [Asn1EnumeratedElement]
        public const long strongAuthRequired = 8;
        
        [Asn1EnumeratedElement]
        public const long referral = 10;
        
        [Asn1EnumeratedElement]
        public const long adminLimitExceeded = 11;
        
        [Asn1EnumeratedElement]
        public const long unavailableCriticalExtension = 12;
        
        [Asn1EnumeratedElement]
        public const long confidentialityRequired = 13;
        
        [Asn1EnumeratedElement]
        public const long saslBindInProgress = 14;
        
        [Asn1EnumeratedElement]
        public const long noSuchAttribute = 16;
        
        [Asn1EnumeratedElement]
        public const long undefinedAttributeType = 17;
        
        [Asn1EnumeratedElement]
        public const long inappropriateMatching = 18;
        
        [Asn1EnumeratedElement]
        public const long constraintViolation = 19;
        
        [Asn1EnumeratedElement]
        public const long attributeOrValueExists = 20;
        
        [Asn1EnumeratedElement]
        public const long invalidAttributeSyntax = 21;
        
        [Asn1EnumeratedElement]
        public const long noSuchObject = 32;
        
        [Asn1EnumeratedElement]
        public const long aliasProblem = 33;
        
        [Asn1EnumeratedElement]
        public const long invalidDNSyntax = 34;
        
        [Asn1EnumeratedElement]
        public const long aliasDereferencingProblem = 36;
        
        [Asn1EnumeratedElement]
        public const long inappropriateAuthentication = 48;
        
        [Asn1EnumeratedElement]
        public const long invalidCredentials = 49;
        
        [Asn1EnumeratedElement]
        public const long insufficientAccessRights = 50;
        
        [Asn1EnumeratedElement]
        public const long busy = 51;
        
        [Asn1EnumeratedElement]
        public const long unavailable = 52;
        
        [Asn1EnumeratedElement]
        public const long unwillingToPerform = 53;
        
        [Asn1EnumeratedElement]
        public const long loopDetect = 54;
        
        [Asn1EnumeratedElement]
        public const long namingViolation = 64;
        
        [Asn1EnumeratedElement]
        public const long objectClassViolation = 65;
        
        [Asn1EnumeratedElement]
        public const long notAllowedOnNonLeaf = 66;
        
        [Asn1EnumeratedElement]
        public const long notAllowedOnRDN = 67;
        
        [Asn1EnumeratedElement]
        public const long entryAlreadyExists = 68;
        
        [Asn1EnumeratedElement]
        public const long objectClassModsProhibited = 69;
        
        [Asn1EnumeratedElement]
        public const long affectsMultipleDSAs = 71;
        
        [Asn1EnumeratedElement]
        public const long other = 80;
        
        public LDAPResult_resultCode()
            : base()
        {
        }
        
        public LDAPResult_resultCode(long val)
            : base(val)
        {
        }
    }
}

