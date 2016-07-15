// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
    public static class TestCategories
    {
        
        //feature
        public const string BVT = "BVT";
        public const string KilePac = "KilePac";
        public const string FAST = "FAST";
        public const string Claim = "Claim";
        public const string Silo = "Silo";
        public const string KerberosError = "Kerberos Error";
        public const string KKDCP = "KKDCP";
        public const string RC4 = "RC4";
        
        //SUT
        public const string Smb2Ap = "Smb2ApplicationServer"; 
        public const string HttpAp = "HttpApplicationServer";
        public const string LdapAp = "LdapApplicationServer";
        public const string KDC = "KDC";
        public const string KPS = "Kerberos Proxy Service";
       
        //topology
        public const string SingleRealm = "SingleRealm";
        public const string CrossRealm = "CrossRealm";
       
        //Domain Function Level
        public const string DFL2K8R2 = "DFL2K8R2";
        public const string DFL2K12 = "DFL2K12";
        public const string DFL2K12R2 = "DFL2K12R2";
    }
         
}
