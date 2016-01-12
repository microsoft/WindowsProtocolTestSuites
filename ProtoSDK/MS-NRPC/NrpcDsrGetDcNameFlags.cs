// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// A set of bit flags that provide additional data that is used to process the request. 
    /// A flag is TRUE (or set) if its value is equal to 1. The value is constructed 
    /// from zero or more bit flags from the following table, with the exceptions that 
    /// bits D, E, and H cannot be combined; S and R cannot be combined; and N and O 
    /// cannot be combined.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NrpcDsrGetDcNameFlags : uint
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A:<para/>
        /// Forces cached DC data to be ignored.
        /// </summary>
        ForcesIgnoreCachedDcData = 0x1,

        /// <summary>
        /// B:<para/>
        /// Requires that the returned DC support directory service functions.
        /// </summary>
        RequiresDcSupportDirectorySerivce = 0x10,

        /// <summary>
        /// C:<para/>
        /// Indicates that the method MUST first attempt to find a DC that supports 
        /// directory service functions. 
        /// If a DC that supports directory services is not available, the method 
        /// returns the name of a non-directory service DC.
        /// </summary>
        AttemptsToFindDirectoryServiceDc = 0x20,

        /// <summary>
        /// D:<para/>
        /// Requires that the returned DC be a global catalog server for the forest of domains. 
        /// If this flag is set and the DomainName parameter is neither NULL nor empty, 
        /// DomainName MUST specify a forest name; otherwise, if DomainName is NULL or empty, 
        /// the forest of the server is assumed. This flag cannot be combined with the E or 
        /// H flags. (For information on domain controller discovery see [MS-ADTS] section 7.3).
        /// </summary>
        GlobalCatalogDc = 0x40,

        /// <summary>
        /// E:<para/>
        /// Requires that the returned DC be the PDC for the domain. 
        /// This flag cannot be combined with the D or H flags.
        /// </summary>
        Pdc = 0x80,

        /// <summary>
        /// F:<para/>
        /// If the A flag is not specified, this method uses cached DC data if available, 
        /// rather than attempting a DC locator call.
        /// </summary>
        UseCachedDcIfNotFlagA = 0x100,

        /// <summary>
        /// G:<para/>
        /// Indicates that the returned DC MUST have an IP (either IPv4 or IPv6) address.
        /// </summary>
        DcHasIpAddress = 0x200,

        /// <summary>
        /// H:<para/>
        /// Requires that the returned DC be currently running the 
        /// Kerberos Key Distribution Center service. This flag 
        /// cannot be combined with the D or E flags.
        /// </summary>
        KerberosKdc = 0x400,

        /// <summary>
        /// I:<para/>
        /// Requires that the returned DC be currently running the Windows Time Service.
        /// </summary>
        TimeService = 0x800,

        /// <summary>
        /// J:<para/>
        /// Requires that the returned DC be writable.
        /// </summary>
        WritableDc = 0x1000,

        /// <summary>
        /// K:<para/>
        /// Indicates that the method MUST first attempt to find a DC that 
        /// is a reliable time server. If a reliable time server is unavailable, 
        /// the method requires that the returned DC be currently running 
        /// the Windows Time Service. The Windows Time Service can be configured 
        /// to declare one or more DCs as a reliable time server. 
        /// For more information about the Windows Time Service, see [MS-SNTP].
        /// </summary>
        AttemptsToFindReliableTimeServerDc = 0x2000,

        /// <summary>
        /// L:<para/>
        /// This flag is ignored if the recipient is not running as a DC. On a DC, 
        /// if this flag is set, the receiving server MUST return a different DC 
        /// in the domain, if one exists.
        /// </summary>
        FindsDifferentDc = 0x4000,

        /// <summary>
        /// M:<para/>
        /// Specifies that the server returned is an LDAP server. The server returned 
        /// is not necessarily a DC. No other services are implied to be present 
        /// at the server. The server returned does not necessarily have a writable 
        /// config container or a writable schema container. If this flag is used with 
        /// the D flag, the server returned is an LDAP server that also hosts a global 
        /// catalog server. The returned global catalog server is not necessarily a DC. 
        /// No other services are implied to be present at the server. If this flag is 
        /// specified, the B, C, E, H, I, K, and T flags are ignored.
        /// </summary>
        LdapServer = 0x8000,

        /// <summary>
        /// N:<para/>
        /// Specifies that the DomainName parameter is a NetBIOS name. 
        /// This flag cannot be combined with the O flag. 
        /// If neither the N nor O flag is set, the method MUST first attempt 
        /// discovery assuming the DNS name. If that attempt fails, the method 
        /// MUST attempt discovery, assuming the NetBIOS name.
        /// </summary>
        DomainNameIsNetBiosName = 0x10000,

        /// <summary>
        /// O:<para/>
        /// Specifies that the DomainName parameter is a DNS name. 
        /// This flag cannot be combined with the N flag. 
        /// If neither the N nor O flag is set, the behavior is 
        /// the same as previously specified for flag N.
        /// </summary>
        DomainNameIsDnsName = 0x20000,

        /// <summary>
        /// P:<para/>
        /// Indicates that the method attempts to find a DC in the next closest site, 
        /// if a DC in the closest site is not available. 
        /// If a DC in the next closest site is also not available, 
        /// the method returns any available DC.
        /// </summary>
        AttemptsToFindDcInNextClosestSite = 0x40000,

        /// <summary>
        /// Q:<para/>
        /// Requires that the returned DC have a DC functional level of DS_BEHAVIOR_WIN2008 
        /// or greater, as specified in [MS-ADTS] section 7.1.4.2.
        /// </summary>
        Win2008FunctionalLevel = 0x80000,

        /// <summary>
        /// R:<para/>
        /// Specifies that the names returned in the DomainControllerName and 
        /// DomainName fields of DomainControllerInfo MUST be DNS names. 
        /// This flag cannot be specified with the S flag. 
        /// If neither the R nor S flag is specified, the method is free to 
        /// return either name type.
        /// </summary>
        ReturnedNamesAreDnsName = 0x40000000,

        /// <summary>
        /// S:<para/>
        /// Specifies that the names returned in the DomainControllerName and 
        /// DomainName fields of DomainControllerInfo MUST be NetBIOS names. 
        /// This flag cannot be specified with the R flag. 
        /// If neither the R nor S flag is specified, the method is free to 
        /// return either name type.
        /// </summary>
        ReturnedNamesAreNetBiosName = 0x80000000,

        /// <summary>
        /// T:<para/>
        /// Requires that the returned DC be currently running the Active Directory Web Service.
        /// </summary>
        ActiveDirectoryWebService = 0x100000,

        /// <summary>
        /// U:<para/>
        /// The server returns a DC that has a DC functional level of DS_BEHAVIOR_WIN2012 or
        /// or greater, as specified in [MS-ADTS] section 6.1.4.2.
        /// </summary>
        Win2012FunctionalLevel = 0x200000,

        /// <summary>
        /// U:<para/>
        /// The server returns a DC that has a DC functional level of DS_BEHAVIOR_WINBLUE or
        /// or greater, as specified in [MS-ADTS] section 6.1.4.2.
        /// </summary>
        WinblueFunctionalLevel = 0x400000,
    }
}
