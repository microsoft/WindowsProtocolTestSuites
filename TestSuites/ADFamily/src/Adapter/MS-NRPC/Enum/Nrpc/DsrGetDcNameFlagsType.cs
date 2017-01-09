// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;

    /// <summary>
    /// A set of bit flags that provide an additional data that is used to process the request. 
    /// A flag is TRUE (or set) if its value is equal to 1. 
    /// The value is constructed from zero or more bit flags from the following table, with the exceptions
    /// that bits D, E, and H cannot be combined; S and R cannot be combined; and N and O cannot be combined.
    /// These items are defined in section 3.5.5.2.1 in the TD of MS-NRPC.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32",
        Justification = "EnumStorageShouldBeInt32"), Flags()]
    public enum DsrGetDcNameFlagsType : uint
    {
        /// <summary>
        /// It forces cached DC data to be ignored.
        /// </summary>
        A = 0x00000001,

        /// <summary>
        /// It requires that the returned DC supports specific operating system versions.
        /// </summary>
        B = 0x00000010,

        /// <summary>
        /// It indicates that the method MUST first attempt to find a DC that supports directory service functions.
        /// If a DC that supports directory services is not available, the method returns the name of a non-directory
        /// service DC.
        /// </summary>
        C = 0x00000020,

        /// <summary>
        /// It requires that the returned DC should be a global catalog server for the forest of domains. 
        /// If this flag is set and the DomainName parameter is not NULL, DomainName MUST specify a forest name; 
        /// otherwise, if DomainName is NULL, the forest of the server is assumed.
        /// </summary>
        D = 0x00000040,

        /// <summary>
        /// It requires that the returned DC should be the PDC for the domain. This flag cannot be combined with the 
        /// D or H flags.
        /// </summary>
        E = 0x00000080,

        /// <summary>
        /// If the A flag is not specified, this method uses cached DC data if available, rather than attempt a DC
        /// locator call.
        /// </summary>
        F = 0x00000100,

        /// <summary>
        /// It indicates that the returned DC MUST have an IP (either IPv4 or IPv6) address.
        /// </summary>
        G = 0x00000200,

        /// <summary>
        /// It requires that the returned DC should be currently running the Kerberos Key Distribution Center service. 
        /// This flag cannot be combined with the D or E flags.
        /// </summary>
        H = 0x00000400,

        /// <summary>
        /// It requires that the returned DC should be currently running the Windows Time Service.
        /// </summary>
        I = 0x00000800,

        /// <summary>
        /// It requires that the returned DC should be writable.
        /// </summary>
        J = 0x00001000,

        /// <summary>
        /// It indicates that the method MUST first attempt to find a DC that is a reliable time server. 
        /// If a reliable time server is unavailable, the method requires that the returned DC should be 
        /// currently running the Windows Time Service.
        /// </summary>
        K = 0x00002000,

        /// <summary>
        /// This flag is ignored if the recipient is not running as a DC. On a DC, if this flag is set, 
        /// the receiving server MUST return a different server in the domain, if one exists.
        /// </summary>
        L = 0x00004000,

        /// <summary>
        /// It specifies that what the server returned is an LDAP server. 
        /// What the server returned is not necessarily a DC. 
        ///No other services is implied to be present at the server. 
        /// What the server returned does not necessarily have a writable config container or a writable schema 
        /// container.
        /// </summary>
        M = 0x00008000,

        /// <summary>
        /// It specifies that the DomainName parameter is a NetBIOS name. 
        /// This flag cannot be combined with the O flag. 
        /// If neither the N nor O flag is set, the method MUST first attempt discovery assuming the DNS name.
        /// If that attempt fails, the method MUST attempt discovery, assuming the NetBIOS name.
        /// </summary>
        N = 0x00010000,

        /// <summary>
        /// It specifies that the DomainName parameter is a DNS name. 
        /// This flag cannot be combined with the N flag. 
        /// If neither the N nor O flag is set, the behavior is the same as previously specified for flag N.
        /// </summary>
        O = 0x00020000,

        /// <summary>
        /// It indicates that the method attempts to find a DC in the next closest site, if a DC in the closest site is
        /// not available. 
        /// If a DC in the next closest site is also not available, the method returns any available DC.
        /// </summary>
        P = 0x00040000,

        /// <summary>
        /// It requires that the returned DC should be running a specific operating system.
        /// </summary>
        Q = 0x00080000,

        /// <summary>
        /// It specifies that the names returned in the DomainControllerName and DomainName fields of 
        /// DomainControllerInfo MUST be DNS names. 
        /// This flag cannot be specified with the S flag. If neither the R nor S flag is specified, 
        /// the method is free to return either name type.
        /// </summary>
        R = 0x40000000,

        /// <summary>
        /// It specifies that the names returned in the DomainControllerName and DomainName fields of 
        /// DomainControllerInfo MUST be NetBIOS names. This flag cannot be specified with the R flag. If neither the 
        /// R nor S flag is specified, the method is free to return either of the name type.
        /// </summary>
        S = 0x80000000,

        /// <summary>
        /// It requires that the returned DC should be currently running the Active Directory Web Service.
        /// </summary>
        T = 0x00100000
    }
}
