// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts.Asn1CodecV2
{
    /*
    LDAPResult_resultCode ::=    ENUMERATED {
                        success                      (0),
                        operationsError              (1),
                        protocolError                (2),
                        timeLimitExceeded            (3),
                        sizeLimitExceeded            (4),
                        compareFalse                 (5),
                        compareTrue                  (6),
                        authMethodNotSupported       (7),
                        strongAuthRequired           (8),
                        noSuchAttribute              (16),
                        undefinedAttributeType       (17),
                        inappropriateMatching        (18),
                        constraintViolation          (19),
                        attributeOrValueExists       (20),
                        invalidAttributeSyntax       (21),
                        noSuchObject                 (32),
                        aliasProblem                 (33),
                        invalidDNSyntax              (34),
                        isLeaf                       (35),
                        aliasDereferencingProblem    (36),
                        inappropriateAuthentication  (48),
                        invalidCredentials           (49),
                        insufficientAccessRights     (50),
                        busy                         (51),
unavailable                  (52),
                        unwillingToPerform           (53),
                        loopDetect                   (54),
                        namingViolation              (64),
                        objectClassViolation         (65),
                        notAllowedOnNonLeaf          (66),
                        notAllowedOnRDN              (67),
                        entryAlreadyExists           (68),
                        objectClassModsProhibited    (69),
                        other                        (80)
                      }
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
        public const long noSuchAttribute = 9;
        
        [Asn1EnumeratedElement]
        public const long undefinedAttributeType = 10;
        
        [Asn1EnumeratedElement]
        public const long inappropriateMatching = 11;
        
        [Asn1EnumeratedElement]
        public const long constraintViolation = 12;
        
        [Asn1EnumeratedElement]
        public const long attributeOrValueExists = 13;
        
        [Asn1EnumeratedElement]
        public const long invalidAttributeSyntax = 14;
        
        [Asn1EnumeratedElement]
        public const long noSuchObject = 15;
        
        [Asn1EnumeratedElement]
        public const long aliasProblem = 16;
        
        [Asn1EnumeratedElement]
        public const long invalidDNSyntax = 17;
        
        [Asn1EnumeratedElement]
        public const long isLeaf = 18;
        
        [Asn1EnumeratedElement]
        public const long aliasDereferencingProblem = 19;
        
        [Asn1EnumeratedElement]
        public const long inappropriateAuthentication = 20;
        
        [Asn1EnumeratedElement]
        public const long invalidCredentials = 21;
        
        [Asn1EnumeratedElement]
        public const long insufficientAccessRights = 22;
        
        [Asn1EnumeratedElement]
        public const long busy = 23;
        
        [Asn1EnumeratedElement]
        public const long unavailable = 24;
        
        [Asn1EnumeratedElement]
        public const long unwillingToPerform = 25;
        
        [Asn1EnumeratedElement]
        public const long loopDetect = 26;
        
        [Asn1EnumeratedElement]
        public const long namingViolation = 27;
        
        [Asn1EnumeratedElement]
        public const long objectClassViolation = 28;
        
        [Asn1EnumeratedElement]
        public const long notAllowedOnNonLeaf = 29;
        
        [Asn1EnumeratedElement]
        public const long notAllowedOnRDN = 30;
        
        [Asn1EnumeratedElement]
        public const long entryAlreadyExists = 31;
        
        [Asn1EnumeratedElement]
        public const long objectClassModsProhibited = 32;
        
        [Asn1EnumeratedElement]
        public const long other = 33;
        
        public LDAPResult_resultCode()
            : base()
        {
        }
        
        public LDAPResult_resultCode(long val)
            : base()
        {
        }
    }
}

