// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    #region Structures defined in Rpcdce.h

    /// <summary>
    /// The SEC_WINNT_AUTH_IDENTITY structure enables passing a particular username and password to 
    /// the run-time library for the purpose of authentication. The structure is defined in Rpcdce.h
    /// </summary>
    public struct SEC_WINNT_AUTH_IDENTITY
    {
        #region Variables

        /// <summary>
        /// String containing the user name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string User;

        /// <summary>
        /// Number of characters in User, excluding the terminating NULL.
        /// </summary>
        public uint UserLength;

        /// <summary>
        /// String containing the domain or workgroup name.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Domain;

        /// <summary>
        /// Number of characters in Domain, excluding the terminating NULL.
        /// </summary>
        public uint DomainLength;

        /// <summary>
        /// String containing the user's password in the domain or workgroup.
        /// </summary>
        [MarshalAs(UnmanagedType.LPTStr)]
        public string Password;

        /// <summary>
        /// Number of characters in Password, excluding the terminating NULL.
        /// </summary>
        public uint PasswordLength;

        /// <summary>
        /// Flags used to specify ANSI or UNICODE.
        /// </summary>
        public SEC_WINNT_AUTH_IDENTITY_FLAGS Flags;

        #endregion


        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domainName">the domain or workgroup name.</param>
        /// <param name="userName">the user name.</param>
        /// <param name="password">the user's password in the domain or workgroup.</param>
        /// <param name="flags">Flags used to specify ANSI or UNICODE.</param>
        public SEC_WINNT_AUTH_IDENTITY(
            String domainName,
            String userName,
            String password,
            SEC_WINNT_AUTH_IDENTITY_FLAGS flags)
        {
            this.Domain = domainName;
            this.User = userName;
            this.Password = password;
            this.DomainLength = (domainName == null) ? 0 : (uint)domainName.Length;
            this.UserLength = (userName == null) ? 0 : (uint)userName.Length;
            this.PasswordLength = (password == null) ? 0 : (uint)password.Length;
            this.Flags = flags;
        }

        #endregion
    }


    /// <summary>
    /// The RPC_IF_ID structure contains the interface UUID and major and minor version numbers of 
    /// an interface. The structure is defined in Rpcdce.h
    /// </summary>
    public struct RPC_IF_ID
    {
        /// <summary>
        /// Specifies the interface UUID.
        /// </summary>
        public Guid Uuid;

        /// <summary>
        /// Major version number, an integer from 0 to 65535, inclusive.
        /// </summary>
        public short VersMajor;

        /// <summary>
        /// Minor version number, an integer from 0 to 65535, inclusive.
        /// </summary>
        public short VersMinor;
    }

    #endregion 


    #region Enumerations defined in Rpcdce.h
    
    /// <summary>
    /// the types used by negotiable security packages.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SEC_WINNT_AUTH_IDENTITY_FLAGS : uint
    {
        /// <summary>
        /// Credentials are in ANSI form.
        /// </summary>
        ANSI = 1,

        /// <summary>
        /// Credentials are in Unicode form.
        /// </summary>
        UNICODE = 2,
    }


    /// <summary>
    /// specify authentication services by identifying the security package that provides the service, 
    /// such as NTLMSSP, Kerberos, or SChannel.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RPC_C_AUTHN : uint
    {
        /// <summary>
        /// No authentication
        /// </summary>
        NONE = 0,

        /// <summary>
        /// DCE private key authentication
        /// </summary>
        DCE_PRIVATE = 1,

        /// <summary>
        /// DCE public key authentication
        /// </summary>
        DCE_PUBLIC = 2,

        /// <summary>
        /// DEC public key authentication. Reserved for future use
        /// </summary>
        DEC_PUBLIC = 4,

        /// <summary>
        /// Snego security support provider
        /// </summary>
        GSS_NEGOTIATE = 9,

        /// <summary>
        /// NTLMSSP (Windows NT LAN Manager Security Support Provider). 
        /// </summary>
        WINNT = 10,

        /// <summary>
        /// SChannel security support provider. This authentication service supports SSL 2.0, SSL 3.0, TLS, and PCT. 
        /// </summary>
        GSS_SCHANNEL = 14,

        /// <summary>
        /// Kerberos security support provider. 
        /// </summary>
        GSS_KERBEROS = 16,

        /// <summary>
        /// Not currently supported.
        /// </summary>
        MSN = 17,

        /// <summary>
        /// Not currently supported.
        /// </summary>
        DPA = 18,

        /// <summary>
        /// Not currently supported.
        /// </summary>
        MQ = 100,

        /// <summary>
        /// The system default authentication service. 
        /// </summary>
        DEFAULT = 0xFFFFFFFF,
    }


    /// <summary>
    /// the amount of authentication provided to help protect the integrity of the data. 
    /// Each level includes the protection provided by the previous levels.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RPC_C_AUTHN_LEVEL : uint
    {
        /// <summary>
        /// Tells DCOM to choose the authentication level using its normal security blanket negotiation algorithm.
        /// </summary>
        DEFAULT = 0,

        /// <summary>
        /// Performs no authentication.
        /// </summary>
        NONE = 1,

        /// <summary>
        /// Authenticates the credentials of the client only when the client establishes a relationship
        /// with the server.
        /// </summary>
        CONNECT = 2,

        /// <summary>
        /// Authenticates only at the beginning of each remote procedure call when the server receives
        /// the request.
        /// </summary>
        CALL = 3,

        /// <summary>
        /// Authenticates that all data received is from the expected client.
        /// </summary>
        PKT = 4,

        /// <summary>
        /// Authenticates and verifies that none of the data transferred between client and server has
        /// been modified.
        /// </summary>
        PKT_INTEGRITY = 5,

        /// <summary>
        /// Authenticates all previous levels and encrypts the argument value of each remote procedure call.
        /// </summary>
        PKT_PRIVACY = 6,
    }


    /// <summary>
    /// type of inquiry to perform on the endpoint map.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RPC_C_EP : uint
    {
        /// <summary>
        /// Returns every element from the endpoint map. 
        /// The IfId, VersOption, and ObjectUuid parameters are ignored.
        /// </summary>
        ALL_ELTS = 0,

        /// <summary>
        /// Searches the endpoint map for elements that contain the interface identifier specified 
        /// by the IfId and VersOption values.
        /// </summary>
        MATCH_BY_IF = 1,

        /// <summary>
        /// Searches the endpoint map for elements that contain the object UUID specified by ObjectUuid.
        /// </summary>
        MATCH_BY_OBJ = 2,

        /// <summary>
        /// Searches the endpoint map for elements that contain the interface identifier 
        /// and object UUID specified by IfId, VersOption, and ObjectUuid.
        /// </summary>
        MATCH_BY_BOTH = 3
    }


    /// <summary>
    /// This parameter is only used when InquiryType is either RPC_C_EP_MATCH_BY_IF or RPC_C_EP_MATCH_BY_BOTH.
    /// Otherwise, it is ignored. The following are valid values for this parameter.  
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum RPC_C_VERS : uint
    {
        /// <summary>
        /// Returns endpoint-map elements that offer the specified interface UUID, regardless of the version numbers.
        /// </summary>
        ALL = 1,

        /// <summary>
        /// Returns endpoint-map elements that offer the same major version of the specified interface UUID and a
        /// minor version greater than or equal to the minor version of the specified interface UUID.
        /// </summary>
        COMPATIBLE = 2,

        /// <summary>
        /// Returns endpoint-map elements that offer the specified version of the specified interface UUID.
        /// </summary>
        EXACT = 3,

        /// <summary>
        /// Returns endpoint-map elements that offer the same major version of the specified interface UUID and 
        /// ignores the minor version.
        /// </summary>
        MAJOR_ONLY = 4,

        /// <summary>
        /// Returns endpoint-map elements that offer a version of the specified interface UUID less than or equal
        /// to the specified major and minor version.
        /// </summary>
        UPTO = 5
    }

    #endregion


    /// <summary>
    /// Pinvoke the windows API of RPC.
    /// </summary>
    public static class RpcNativeMethods
    {
        /// <summary>
        /// The RpcStringBindingCompose function creates a string binding handle.
        /// </summary>
        /// <param name="ObjUuid">Pointer to a null-terminated string representation of an object UUID. 
        /// For example, the string 6B29FC40-CA47-1067-B31D-00DD010662DA represents a valid UUID.</param>
        /// <param name="Protseq">Pointer to a null-terminated string representation of a protocol sequence.</param>
        /// <param name="NetworkAddr">Pointer to a null-terminated string representation of a network address. 
        /// The network-address format is associated with the protocol sequence.</param>
        /// <param name="Endpoint">Pointer to a null-terminated string representation of an endpoint. 
        /// The endpoint format and content are associated with the protocol sequence. For example, the endpoint
        /// associated with the protocol sequence ncacn_np is a pipe name in the format \pipe\pipename. </param>
        /// <param name="Options">Pointer to a null-terminated string representation of an endpoint. The endpoint
        /// format and content are associated with the protocol sequence. For example, the endpoint associated 
        /// with the protocol sequence ncacn_np is a pipe name in the format \pipe\pipename. </param>
        /// <param name="StringBinding">Returns a pointer to a pointer to a null-terminated string representation
        /// of a binding handle.</param>
        /// <returns> RPC Return Values</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcStringBindingCompose(
            string ObjUuid,
            string Protseq,
            string NetworkAddr,
            string Endpoint,
            string Options,
            out IntPtr StringBinding);


        /// <summary>
        /// The RpcBindingFromStringBinding function returns a binding handle from a string representation of
        /// a binding handle.
        /// </summary>
        /// <param name="StringBinding">Pointer to a string representation of a binding handle.</param>
        /// <param name="Binding">Returns a pointer to the server binding handle.</param>
        /// <returns>RPC Return Values</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcBindingFromStringBinding(
            IntPtr StringBinding,
            out IntPtr Binding);


        /// <summary>
        /// The RpcBindingSetAuthInfo function sets a binding handle's authentication and authorization information.
        /// </summary>
        /// <param name="Binding">Server binding handle to which authentication and authorization information is to
        /// be applied.</param>
        /// <param name="ServerPrincName">Pointer to the expected principal name of the server referenced by Binding.
        /// The content of the name and its syntax are defined by the authentication service in use.</param>
        /// <param name="AuthnLevel">Level of authentication to be performed on remote procedure calls made using
        /// Binding. For a list of the RPC-supported authentication levels</param>
        /// <param name="AuthnSvc">Authentication service to use</param>
        /// <param name="AuthIdentity">Handle to the structure containing the client's authentication and
        /// authorization credentials appropriate for the selected authentication and authorization service.</param>
        /// <param name="AuthzService">Authorization service implemented by the server for the interface of
        /// interest. </param>
        /// <returns>RPC Return Values</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcBindingSetAuthInfo(
            IntPtr Binding,
            string ServerPrincName,
            RPC_C_AUTHN_LEVEL AuthnLevel,
            RPC_C_AUTHN AuthnSvc,
            ref SEC_WINNT_AUTH_IDENTITY AuthIdentity,
            RPC_C_AUTHN AuthzService);


        /// <summary>
        /// The RpcMgmtEpEltInqBegin function creates an inquiry context 
        /// for viewing the elements in an endpoint map.
        /// </summary>
        /// <param name="EpBinding">Binding handle to a host whose endpoint-map elements is to be viewed. Specify
        /// NULL to view elements from the local host. If a binding handle is specified, the object UUID on the
        /// binding handle must be NULL. If present, the endpoint on the binding handle is ignored and the endpoint
        /// to the endpoint mapper database on the given host is used.</param>
        /// <param name="InquiryType">Integer value that indicates the type of inquiry to perform on the endpoint
        /// map. </param>
        /// <param name="IfId">Interface identifier of the endpoint-map elements to be returned by RpcMgmtEpEltInqNext.
        /// This parameter is only used when InquiryType is either RPC_C_EP_MATCH_BY_IF or RPC_C_EP_MATCH_BY_BOTH.
        /// Otherwise, it is ignored.</param>
        /// <param name="VersOption">Specifies how RpcMgmtEpEltInqNext uses the IfId parameter. </param>
        /// <param name="ObjectUuid">The object UUID that RpcMgmtEpEltInqNext looks for in endpoint-map elements.
        /// This parameter is used only when InquiryType is either RPC_C_EP_MATCH_BY_OBJ or RPC_C_EP_MATCH_BY_BOTH.
        /// </param>
        /// <param name="InquiryContext">Returns an inquiry context for use with RpcMgmtEpEltInqNext and
        /// RpcMgmtEpEltInqDone.</param>
        /// <returns>RPC Return Values.</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcMgmtEpEltInqBegin(
            IntPtr EpBinding,
            RPC_C_EP InquiryType,
            ref RPC_IF_ID IfId,
            RPC_C_VERS VersOption,
            ref Guid ObjectUuid,
            out IntPtr InquiryContext);


        /// <summary>
        ///The RpcMgmtEpEltInqNext function returns one element from an endpoint map. 
        /// </summary>
        /// <param name="InquiryContext">Specifies an inquiry context. The inquiry context is returned from 
        /// RpcMgmtEpEltInqBegin.</param>
        /// <param name="IfId">interface identifier of the endpoint-map element.</param>
        /// <param name="Binding">Optional. Returns the binding handle from the endpoint-map element</param>
        /// <param name="ObjectUuid">Optional. Returns the object UUID from the endpoint-map element</param>
        /// <param name="Annotation">Optional. Returns the annotation string for the endpoint-map element.
        /// When there is no annotation string in the endpoint-map element, the empty string ("") is returned.
        /// </param>
        /// <returns>RPC Return Values.</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcMgmtEpEltInqNext(
            IntPtr InquiryContext,
            ref RPC_IF_ID IfId,
            out IntPtr Binding,
            out Guid ObjectUuid,
            out IntPtr Annotation);


        /// <summary>
        /// The RpcMgmtEpEltInqDone function deletes the inquiry context for viewing the elements in an
        /// endpoint map.
        /// </summary>
        /// <param name="InquiryContext">Inquiry context to delete and returns the value NULL.</param>
        /// <returns>RPC Return Values.</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcMgmtEpEltInqDone(
             ref IntPtr InquiryContext);


        /// <summary>
        /// The RpcBindingToStringBinding function returns a string representation of a binding handle.
        /// </summary>
        /// <param name="Binding">Client or server binding handle to convert to a string representation of
        /// a binding handle.</param>
        /// <param name="StringBinding">Returns a pointer to a pointer to the string representation of the
        /// binding handle specified in the Binding parameter.</param>
        /// <returns>RPC Return Values.</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcBindingToStringBinding(
            IntPtr Binding,
            out IntPtr StringBinding);


        /// <summary>
        /// The RpcStringBindingParse function returns the object UUID part and the address parts of a string 
        /// binding as separate strings. An application calls RpcStringBindingParse to parse a string 
        /// representation of a binding handle into its component fields. The RpcStringBindingParse function
        /// returns the object UUID part and the address parts of a string binding as separate strings.
        /// </summary>
        /// <param name="StringBinding">Pointer to a null-terminated string representation of a binding.</param>
        /// <param name="ObjectUuid">Returns a pointer to a pointer to a null-terminated string representation 
        /// of an object UUID. </param>
        /// <param name="ProtSeq">Returns a pointer to a pointer to a null-terminated string representation of a
        /// protocol sequence. For a list of Microsoft RPC supported protocol sequences, see String Binding. </param>
        /// <param name="NetworkAddr">Returns a pointer to a pointer to a null-terminated string representation
        /// of a network address. Specify a NULL value to prevent RpcStringBindingParse from returning the
        /// NetworkAddr parameter. In this case, the application does not call RpcStringFree.</param>
        /// <param name="EndPoint">Returns a pointer to a pointer to a null-terminated string representation of
        /// an endpoint. Specify a NULL value to prevent RpcStringBindingParse from returning the EndPoint
        /// parameter. In this case, the application does not call RpcStringFree.</param>
        /// <param name="NetworkOptions">Returns a pointer to a pointer to a null-terminated string representation
        /// of network options. </param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcStringBindingParse(
            IntPtr StringBinding,
            out IntPtr ObjectUuid,
            out IntPtr ProtSeq,
            out IntPtr NetworkAddr,
            out IntPtr EndPoint,
            out IntPtr NetworkOptions);


        /// <summary>
        /// The RpcStringFree function frees a character string allocated by the RPC run-time library.
        /// </summary>
        /// <param name="String">Pointer to a pointer to the character string to free.</param>
        /// <returns> RPC Return Values</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcStringFree(
            ref IntPtr String);


        /// <summary>
        /// The RpcBindingFree function releases binding-handle resources.
        /// </summary>
        /// <param name="Binding">Pointer to the server binding to be freed.</param>
        /// <returns> RPC Return Values</returns>
        [SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
        [DllImport("rpcrt4.dll")]
        public static extern uint RpcBindingFree(
            ref IntPtr Binding);
    }
}
