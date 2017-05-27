// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Security.Samr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Samr
{
    #region enum

    /// <summary>
    /// The authentication-level constants represent authentication levels passed to various run-time functions. 
    /// These levels are listed in order of increasing authentication. 
    /// Each new level adds to the authentication provide by the previous level. 
    /// If the RPC run-time library does not support the specified level, 
    /// it will automatically upgrades to the next higher supported level.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028")]
    public enum RPC_C_AUTHN_LEVEL : uint
    {
        /// <summary>
        /// Uses the default authentication level for the specified authentication service.
        /// </summary>
        RPC_C_AUTHN_LEVEL_DEFAULT = 0,

        /// <summary>
        /// Performs no authentication.
        /// </summary>
        RPC_C_AUTHN_LEVEL_NONE = 1,

        /// <summary>
        /// Authenticate only when the client established a relationship with a server.
        /// </summary>
        RPC_C_AUTHN_LEVEL_CONNECT = 2,

        /// <summary>
        /// Authenticate only at the beginning of each remote procedure call when the server receives the request. 
        /// Does not apply to remote procedure calls made using the connection-based protocol sequences 
        /// (those that start with the prefix "ncacn"). 
        /// If the protocol sequence in a binding handle is a connection-based protocol sequence 
        /// and you specify this level, this routine instead uses the RPC_C_AUTHN_LEVEL_PKT constant.
        /// </summary>
        RPC_C_AUTHN_LEVEL_CALL = 3,

        /// <summary>
        /// Authenticate only that all data received is from the expected client. 
        /// Does not validate the data itself.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT = 4,

        /// <summary>
        /// Authenticate and verifies that none of the data transferred between client and server has been modified.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_INTEGRITY = 5,

        /// <summary>
        /// Includes all previous levels, and ensure clear text data can only be seen by the sender and the receiver. 
        /// In the local case, this involves using a secure channel. 
        /// In the remote case, this involves encrypting the argument value of each remote procedure call.
        /// </summary>
        RPC_C_AUTHN_LEVEL_PKT_PRIVACY = 6
    }


    /// <summary>
    /// The authentication service constants represent the authentication services passed to various run-time functions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028")]
    public enum RPC_C_AUTHN_SVC : uint
    {
        /// <summary>
        /// No authentication.
        /// </summary>
        RPC_C_AUTHN_NONE = 0,

        /// <summary>
        /// Use Distributed Computing Environment (DCE) private key authentication.
        /// </summary>
        RPC_C_AUTHN_DCE_PRIVATE = 1,

        /// <summary>
        /// DCE public key authentication (reserved for future use).
        /// </summary>
        RPC_C_AUTHN_DCE_PUBLIC = 2,

        /// <summary>
        /// DEC public key authentication (reserved for future use).
        /// </summary>
        RPC_C_AUTHN_DEC_PUBLIC = 4,

        /// <summary>
        /// Use the Microsoft Negotiate SSP.
        /// This SSP negotiates between the use of the NTLM and Kerberos protocol Security Support Providers (SSP).
        /// </summary>
        RPC_C_AUTHN_GSS_NEGOTIATE = 9,

        /// <summary>
        /// Use the Microsoft NT LAN Manager (NTLM) SSP.
        /// </summary>
        RPC_C_AUTHN_WINNT = 10,

        /// <summary>
        /// Use the Schannel SSP.
        /// This SSP supports Secure Socket Layer (SSL), private communication technology (PCT), 
        /// and transport level security (TLS).
        /// </summary>
        RPC_C_AUTHN_GSS_SCHANNEL = 14,

        /// <summary>
        /// Use the Microsoft Kerberos SSP.
        /// </summary>
        RPC_C_AUTHN_GSS_KERBEROS = 16,

        /// <summary>
        /// Use Distributed Password Authentication (DPA).
        /// </summary>
        RPC_C_AUTHN_PDA = 17,

        /// <summary>
        /// Authentication protocol SSP used for the Microsoft Network (MSN).
        /// </summary>
        RPC_C_AUTHN_MSN = 18,

        /// <summary>
        /// Windows XP or later: Use the Microsoft Digest SSP.
        /// </summary>
        RPC_C_AUTHN_DIGEST = 21,

        /// <summary>
        /// Windows 7 or later: Reserved.
        /// </summary>
        RPC_C_AUTHN_NEGO_EXTENDER = 30,

        /// <summary>
        /// This SSP provides an SSPI-compatible wrapper for the 
        /// Microsoft Message Queue (MSMQ) transport-level protocol.
        /// </summary>
        RPC_C_AUTHN_MQ = 100,

        /// <summary>
        /// Use the default authentication service.
        /// </summary>
        RPC_C_AUTHN_DEFAULT = 0xFFFFFFFF,
    }

    /// <summary>
    /// The authorization service constants represent the authorization services passed to various run-time functions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028")]
    public enum RPC_C_AUTHZ : uint
    {
        /// <summary>
        /// Server performs no authorization.
        /// </summary>
        RPC_C_AUTHZ_NONE = 0,

        /// <summary>
        /// Server performs authorization based on the client's principal name.
        /// </summary>
        RPC_C_AUTHZ_NAME = 1,

        /// <summary>
        /// Server performs authorization checking using the client's DCE privilege attribute certificate (PAC) information, 
        /// which is sent to the server with each remote procedure call made using the binding handle. 
        /// Generally, access is check against DCE access control lists (ACLs).
        /// </summary>
        RPC_C_AUTHZ_DCE = 2,

        /// <summary>
        /// Server use the default authorization service for the current SSP.
        /// </summary>
        RPC_C_AUTHZ_DEFAULT = 0xFFFFFFFF
    }

    #endregion

    /// <summary>
    /// Utilities tool for SAMR protocol.
    /// </summary>
    public partial class Utilities
    {
        #region RPC binding

        private const string rpcRuntimeDllName = "rpcrt4.dll";
        private const string samrRuntimeDllName = "Microsoft.Protocols.TestTools.StackSdk.Security.Samr.dll";

        /// <summary>
        /// The RpcStringBindingCompose function creates a string binding handle.
        /// </summary>
        /// <param name="ObjUuid">
        /// A string representation of an object UUID. 
        /// For example, the string 6B29FC40-CA47-1067-B31D-00DD010662DA represents a valid UUID.
        /// </param>
        /// <param name="ProtSeq">
        /// A string representation of a protocol sequence.
        /// </param>
        /// <param name="NetworkAddr">
        /// A string representation of a network address.
        /// The network-address format is associated with the protocol sequence.
        /// </param>
        /// <param name="EndPoint">
        /// A string representation of an endpoint. 
        /// The endpoint format and content are associated with the protocol sequence. 
        /// For example, the endpoint associated with the protocol sequence ncacn_np 
        /// is a pipe name in the format \pipe\pipename.
        /// </param>
        /// <param name="Options">
        /// A string representation of network options. 
        /// The option string is associated with the protocol sequence.
        /// </param>
        /// <param name="StringBinding">
        /// Returns a pointer to a string representation of a binding handle.
        /// </param>
        /// <returns>
        /// RPC_S_OK if the call succeeded.
        /// RPC_S_INVALID_STRING_UUID if the string representation of the UUID is not valid.
        /// </returns>
        /// Disable CA1060, because according to current test suite design, there is no need to move pinvoke to native.
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(rpcRuntimeDllName, CharSet = CharSet.Auto)]
        private extern static int RpcStringBindingCompose(
                                                        string ObjUuid,
                                                        string ProtSeq,
                                                        string NetworkAddr,
                                                        string EndPoint,
                                                        string Options,
                                                        out IntPtr StringBinding);


        /// <summary>
        /// The RpcBindingFromStringBinding function returns a binding handle from a string representation of a binding handle.
        /// </summary>
        /// <param name="StringBinding">
        /// A string representation of a binding handle.
        /// </param>
        /// <param name="Binding">
        /// Returns a pointer to the server binding handle.
        /// </param>
        /// <returns>
        /// RPC_S_OK if the call succeeded.
        /// RPC_S_INVALID_STRING_BINDING if the string binding is not valid.
        /// RPC_S_PROTSEQ_NOT_SUPPORTED if protocol sequence not supported on this host.
        /// RPC_S_INVALID_RPC_PROTSEQ if the protocol sequence is not valid.
        /// RPC_S_INVALID_ENDPOINT_FORMAT if the endpoint format is not valid.
        /// RPC_S_STRING_TOO_LONG if string too long.
        /// RPC_S_INVALID_NET_ADDR if the network address is not valid.
        /// RPC_S_INVALID_ARG if the argument was not valid.
        /// RPC_S_INVALID_NAF_ID if the network address family identifier is not valid.
        /// </returns>
        /// Disable CA1060, because according to current test suite design, there is no need to move pinvoke to native.
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(rpcRuntimeDllName, CharSet = CharSet.Auto)]
        private extern static int RpcBindingFromStringBinding(string StringBinding, out IntPtr Binding);


        /// <summary>
        /// The RpcBindingSetAuthInfo function sets a binding handle's authentication and authorization information.
        /// </summary>
        /// <param name="Binding">
        /// Server binding handle to which authentication and authorization information is to be applied.
        /// </param>
        /// <param name="ServerPrincName">
        /// The expected principal name of the server referenced by Binding. 
        /// The content of the name and its syntax are defined by the authentication service in use.
        /// </param>
        /// <param name="AuthnLevel">
        /// Level of authentication to be performed on remote procedure calls made using Binding. 
        /// </param>
        /// <param name="AuthnSvc">
        /// Authentication service to use.
        /// </param>
        /// <param name="AuthIdentity">
        /// Handle to the structure containing the client's authentication and authorization credentials appropriate 
        /// for the selected authentication and authorization service. 
        /// </param>
        /// <param name="AuthzService">
        /// Authorization service implemented by the server for the interface of interest.
        /// </param>
        /// <returns>
        /// RPC_S_OK if the call succeeded.
        /// RPC_S_INVALID_BINDING if the binding handle was invalid.
        /// RPC_S_WRONG_KIND_OF_BINDING if this was the wrong kind of binding for the operation.
        /// RPC_S_UNKNOWN_AUTHN_SERVICE if unknown authentication service.
        /// </returns>
        /// Disable CA1060, because according to current test suite design, there is no need to move pinvoke to native.
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(rpcRuntimeDllName, CharSet = CharSet.Auto)]
        private extern static int RpcBindingSetAuthInfo(IntPtr Binding,
                                                        string ServerPrincName,
                                                        uint AuthnLevel,
                                                        uint AuthnSvc,
                                                        IntPtr AuthIdentity,
                                                        uint AuthzService);

        /// <summary>
        /// The RpcBindingFree function releases binding-handle resources.
        /// </summary>
        /// <param name="Binding">
        /// Pointer to the server binding to be freed.
        /// </param>
        /// <returns>
        /// RPC_S_OK if the call succeeded.
        /// RPC_S_INVALID_BINDING if the binding handle was invalid.
        /// RPC_S_WRONG_KIND_OF_BINDING if this was the wrong kind of binding for the operation.
        /// </returns>
        /// Disable CA1060, because according to current test suite design, there is no need to move pinvoke to native.
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        [DllImport(rpcRuntimeDllName)]
        private extern static int RpcBindingFree(ref IntPtr Binding);

        /// <summary>
        /// GetRpcBindingHandle method is used to get the RPC binding handle with respect to
        /// the specified protocol sequence.
        /// </summary>
        /// <param name="serverName">Name of the server computer.</param>
        /// <param name="protocolSequence">There are two possible sequence which are TCP and Named Pipe.</param>
        /// <returns>Returns the RPC Binding handle.</returns>
        public static IntPtr GetRpcBindingHandle(string serverName, ProtocolSequence protocolSequence)
        {
            IntPtr handle = IntPtr.Zero;
            string bindingString = null;
            string protoSeq = null;
            string endPoint = null;
            string uuid = "12345778-1234-ABCD-EF00-0123456789AC";
            int status = 0;
            IntPtr bindingStringPtr = IntPtr.Zero;

            // Rpc over TCP Protocol Sequence.
            if (protocolSequence == ProtocolSequence.RpcOverTcp)
            {
                protoSeq = "ncacn_ip_tcp";
                endPoint = null;
            }
            else if (protocolSequence == ProtocolSequence.RpcOverNamedPipe)
            {
                protoSeq = "ncacn_np";
                endPoint = "\\PIPE\\samr";
            }
            else
            {
                throw new ArgumentException(
                    string.Format(null, "Unsupported protocol sequence: {0}.", protocolSequence),
                    "protocolSequence");
            }

            status = RpcStringBindingCompose(uuid,
                                            protoSeq,
                                            serverName,
                                            endPoint,
                                            null,
                                            out bindingStringPtr);

            bindingString = Marshal.PtrToStringAuto(bindingStringPtr);
            status = RpcBindingFromStringBinding(bindingString, out handle);
            status = RpcBindingSetAuthInfo(handle,
                                            serverName,
                                            (uint)RPC_C_AUTHN_LEVEL.RPC_C_AUTHN_LEVEL_PKT_INTEGRITY,
                                            (uint)RPC_C_AUTHN_SVC.RPC_C_AUTHN_GSS_NEGOTIATE,
                                            IntPtr.Zero,
                                            (uint)RPC_C_AUTHZ.RPC_C_AUTHZ_NAME);

            Console.WriteLine("Return status of RpcBindingSetAuthInfo call", status);
            return handle;
        }


        /// <summary>
        /// FreeRpcBindingHandle method is used to free the RPC binding handle.
        /// </summary>
        /// <param name="handle">The handle of server binding to be freed.</param>
        public static void FreeRpcBindingHandle(IntPtr handle)
        {
            int status = RpcBindingFree(ref handle);
            Console.WriteLine("Return status of RpcBindingTree call {0}", status);
        }

        #endregion

        #region string/etc operation

        /// <summary>
        /// Change byte array to short.
        /// </summary>
        /// <param name="input">Byte array to convert.</param>
        /// <returns>Short array.</returns>
        public ushort[] Byte2Ushort(byte[] input)
        {
            IntPtr ptr0 = Marshal.AllocHGlobal(input.Length);
            Marshal.Copy(input, 0, ptr0, input.Length);

            return IntPtrUtility.PtrToArray<ushort>(ptr0, (uint)input.Length / 2);
        }

        /// <summary>
        /// This method convert the given string into its equivalent unicode string.
        /// </summary>
        /// <param name="InputString">The input string.</param>
        /// <param name="unicodestring">The converts unicode string.</param>
        public void GetUnicodeString(string InputString, ref ushort[] unicodestring)
        {
            int iCharCount = 0;
            foreach (char c in InputString)
            {
                unicodestring[iCharCount] = Convert.ToUInt16(c);
                iCharCount++;
            }
        }

        /// <summary>
        /// ConvertStringToByteArray convert the given unicode string to a byte array.
        /// </summary>
        /// <param name="pswdString">Input string.</param>
        /// <param name="pswdInBytes">Converted byte array.</param>
        public void ConvertStringToByteArray(string pswdString, ref byte[] pswdInBytes)
        {
            char[] temp = new char[pswdString.Length];
            pswdInBytes = new byte[temp.Length];
            pswdString.CopyTo(0, temp, 0, pswdString.Length);
            for (int i = 0; i < pswdInBytes.Length; i++)
            {
                pswdInBytes[i] = (byte)temp[i];
            }
        }

        /// <summary>
        /// This method convert the given string into a structure RPC_UNICODE_STRING.
        /// </summary>
        /// <param name="inputName">Input string.</param>
        /// <param name="convertedString">Converted RPC_UNICODE_STRING.</param>
        public void convertStringToRPCUNICODESTRING(string inputName,
                                                    ref _RPC_UNICODE_STRING convertedString)
        {
            // convert the inputName to RPC Unicode string.
            convertedString = StringToRpcUnicodeString(inputName);
        }

        /// <summary>
        /// This method convert the given set of RPC_UNICODE_STRING param to string param.
        /// </summary>
        /// <param name="EnumBuf">Buffer having an array of RPC_UNICODE_STRING values.</param>
        /// <param name="countReturned">Number of array elements in the buffer.</param>
        /// <returns>String[].</returns>
        public string[] convertToString(_SAMPR_ENUMERATION_BUFFER EnumBuf, uint countReturned)
        {
            int i, j;
            string[] names = new string[(int)countReturned];
            for (i = 0; i < countReturned; i++)
            {
                char[] tempStr = new char[EnumBuf.Buffer[i].Name.Buffer.Length + 1];
                for (j = 0; j < (EnumBuf.Buffer[i].Name.Buffer.Length); j++)
                {
                    tempStr[j] = Convert.ToChar(EnumBuf.Buffer[i].Name.Buffer[j]);
                }
                string accountName = new string(tempStr, 0, tempStr.Length);
                accountName = accountName.Remove(accountName.Length - 1);
                names[i] = accountName;
            }
            return names;
        }

        /// <summary>
        /// This method convert the given ushort param to string param.
        /// </summary>
        /// <param name="ushortBuffer">Buffer having ushort values.</param>
        /// <returns>String.</returns>
        public string convertToString(ushort[] ushortBuffer)
        {
            if (ushortBuffer != null)
            {
                int j;
                string name = string.Empty;
                char[] tempStr = new char[ushortBuffer.Length + 1];
                for (j = 0; j < (ushortBuffer.Length); j++)
                {
                    tempStr[j] = Convert.ToChar(ushortBuffer[j]);
                }
                string accountName = new string(tempStr, 0, tempStr.Length);
                accountName = accountName.Remove(accountName.Length - 1);
                name = accountName;
                return name;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// This method splits the sid and fetches the Rid from it.
        /// </summary>
        /// <param name="sid">SID.</param>
        /// <returns>String.</returns>
        public string getRid(string sid)
        {
            string Rid;
            string[] sidArray = sid.Split('-');
            Rid = sidArray[sidArray.Length - 1];
            return Rid;
        }

        /// <summary>
        /// This method convert the given time value to assign into a _LARGE_INTEGER.
        /// </summary>
        /// <param name="time">Time value</param>
        /// <param name="lowPart">Low part of the Integer.</param>
        /// <param name="highPart">High part of the Integer.</param>
        /// <returns></returns>
        public void GetLargeInteger(long time, ref uint lowPart, ref int highPart)
        {
            long FiletimeValue = (long)0 - ((long)time * (long)60 * (long)Math.Pow(10, 7));

            byte[] tempBytes = BitConverter.GetBytes(FiletimeValue);
            byte[] templowBytes = new byte[4];
            byte[] tempHighBytes = new byte[4];

            Array.Copy(tempBytes, 0, templowBytes, 0, 4);
            Array.Copy(tempBytes, 4, tempHighBytes, 0, 4);

            highPart = (int)BitConverter.ToInt32(tempHighBytes, 0);
            lowPart = (uint)BitConverter.ToUInt32(templowBytes, 0);

        }

        /// <summary>
        /// Calculate the given period in days to delta format.
        /// </summary>
        /// <param name="periodInDays">Number of days.</param>
        /// <returns>Long.</returns>
        public long convertDaysToDeltaTime(long periodInDays)
        {
            long timeInDeltaTimeFormat;

            // Negative of FileTime syntax (Time expressed in intervals of 100 nano seconds.
            timeInDeltaTimeFormat = (long)0 - ((long)periodInDays * 24 * 3600 * (long)Math.Pow(10, 7));
            return timeInDeltaTimeFormat;
        }

        /// <summary>
        /// Calculate the given time in delta format.
        /// </summary>
        /// <param name="tempTime">Date time object to be converted.</param>
        /// <returns>Long.</returns>
        public long CalculateTimeinFITETIMESYNTAX(long tempTime)
        {
            long timeInFILETIMEFormat;
            timeInFILETIMEFormat = ~tempTime + 1;
            return timeInFILETIMEFormat;
        }

        /// <summary>
        /// The GetLongEquivalent method computes the Long equivalent of a _OLD_LARGE_INTEGER type.
        /// </summary>
        /// <param name="TimeInfo">_OLD_LARGE_INTEGER time format.</param>
        /// <returns>Long equivalent of the input time.</returns>
        public long GetLongEquivalent(_OLD_LARGE_INTEGER TimeInfo)
        {
            // Store the low part,High part of the large_integer into one single byte array.
            byte[] tempBytes = new byte[64];
            byte[] templowBytes = new byte[32];
            byte[] tempHighBytes = new byte[32];
            Int64 LongTime;
            templowBytes = BitConverter.GetBytes(TimeInfo.LowPart);
            tempHighBytes = BitConverter.GetBytes(TimeInfo.HighPart);
            templowBytes.CopyTo(tempBytes, 0);
            tempHighBytes.CopyTo(tempBytes, templowBytes.Length);
            // Compute the long value equivalent of the bytes in tempBytes.
            LongTime = BitConverter.ToInt64(tempBytes, 0);
            return LongTime;
        }

        /// <summary>
        /// Validate a password is complex.
        /// </summary>
        /// <param name="Password">The password string to validated.</param>
        /// <returns>The password is complex or not.</returns>
        public int isPasswordComplex(string Password)
        {
            char[] passwordArray = Password.ToCharArray();
            ushort[] SpecialCharacters = new ushort[]{0x0028,0x0060,0x007e,0x0021,0x0040,
                                                      0x0023,0x0024,0x0025,0x005e,0x0026,
                                                      0x002a,0x005f,0x002d,0x002b,0x003d,
                                                      0x007c,0x005c,0x007b,0x007d,0x005b,
                                                      0x003c,0x003e,0x002c,0x002e,0x003f,
                                                      0x0029,0x002f};

            int nLowerCasePresent = 0;
            int nUpperCasePresent = 0;
            int nArabicNumeralPresent = 0;
            int nSpecialCharPresent = 0;
            int nCount = 0;
            for (int i = 0; i < passwordArray.Length; i++)
            {
                if (passwordArray[i] >= 0x41 && passwordArray[i] <= 0x56)
                    nLowerCasePresent++;
                if (passwordArray[i] >= 0x62 && passwordArray[i] <= 0x7a)
                    nUpperCasePresent++;
                if (passwordArray[i] >= 0x30 && passwordArray[i] <= 0x39)
                    nArabicNumeralPresent++;
                for (int j = 0; j < SpecialCharacters.Length; j++)
                {
                    if (passwordArray[i] == (char)SpecialCharacters[j])
                        nSpecialCharPresent++;
                }
            }
            if (nLowerCasePresent > 0)
                nCount++;
            if (nUpperCasePresent > 0)
                nCount++;
            if (nArabicNumeralPresent > 0)
                nCount++;
            if (nSpecialCharPresent > 0)
                nCount++;

            return nCount;
        }

        /// <summary>
        /// Check if the index is unique.
        /// </summary>
        /// <param name="indices">A array to be checked.</param>
        /// <param name="key">The key to exam.</param>
        /// <returns>The check result.</returns>
        public bool isIndexUnique(uint[] indices, uint key)
        {
            uint count = 0;
            int i;
            for (i = 0; i < indices.Length; i++)
            {
                if (key == indices[i])
                {
                    count++;
                }
            }
            if (count == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method convert the string to _RPC_UNICODE_STRING.
        /// </summary>
        /// <param name="userName">A string to be convert.</param>
        /// <returns>A rpc unicode string.</returns>
        private _RPC_UNICODE_STRING StringToRpcUnicodeString(string userName)
        {
            if (userName == null)
            {
                string inputIsNull = "Input is null";
                throw new ArgumentNullException(inputIsNull);
            }
            _RPC_UNICODE_STRING _rpc_unicode_string = new _RPC_UNICODE_STRING();
            _rpc_unicode_string.Length = _rpc_unicode_string.MaximumLength = (ushort)(userName.Length * 2);
            _rpc_unicode_string.Buffer = new ushort[userName.Length];
            for (int i = 0; i < userName.Length; i++)
            {
                _rpc_unicode_string.Buffer[i] = Convert.ToUInt16(userName[i]);
            }
            return _rpc_unicode_string;
        }
        #endregion

        #region AD related operation

        public static LdapConnection LdapCon;

        /// <summary>
        /// Get security identifier of object indicated by DN.
        /// </summary>
        /// <param name="objectName">The RDN of the object.</param>
        /// <returns>The SID of the object.</returns>
        public SecurityIdentifier GetSid(string objectName)
        {
            ITestSite SamrTestSite = null;
            Object[] values;
            byte[][] objectSid = new byte[0][];
            SecurityIdentifier Sid = null;
            int j;
            SamrTestSite = TestClassBase.BaseTestSite;
            SearchRequest searchRequest = new SearchRequest();
            searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;

            // Set the DN of the object.
            searchRequest.DistinguishedName = SamrTestSite.Properties.Get("LDAPPathDN") + objectName + "," +
                                              SAMRProtocolAdapter.Instance(SamrTestSite).primaryDomainUserContainerDN;

            // Send the LDAP request to server.
            SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
            //LdapCon.Dispose();

            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                {
                    switch (attribute.Name)
                    {
                        case "objectSid":
                            values = attribute.GetValues(Type.GetType("System.Byte[]"));
                            for (j = 0; j < values.Length; j++)
                            {
                                objectSid = new byte[values.Length][];
                                objectSid[j] = (byte[])values[j];
                            }
                            Sid = new SecurityIdentifier(objectSid[j - 1], 0);
                            return Sid;
                    }
                }
            }

            return Sid;
        }

        /// <summary>
        /// This method compares the attributes like sAMAccountName with the one's that are obtained
        /// from Enumerate pattern.
        /// </summary>
        /// <param name="searchResponse">
        /// SearchResponse object.
        /// </param>
        /// <param name="entryName">
        /// The name of the group, alias or object for which the attributes need to be checked.
        /// </param>
        /// <param name="sid">
        /// Sid of the object.
        /// </param>
        /// <param name="entryRid">
        /// Rid of the object.
        /// </param>
        /// <returns>bool</returns>
        public bool enumerateAttributeCheck(SearchResponse searchResponse,
                                            string entryName,
                                            string sid,
                                            uint entryRid)
        {
            string rid, samAccName = "";
            rid = getRid(sid);
            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                {
                    if (attribute.Name == "sAMAccountName")
                    {
                        for (int i = 0; i < attribute.Count; i++)
                        {
                            samAccName = attribute[i].ToString();
                        }
                    }
                    else
                        continue;
                }
            }

            if (samAccName.Equals(entryName) && rid.Equals(entryRid.ToString()))
                return true;
            else
                return false;
        }

        /// <summary>
        /// CreateGroup method adds a specific Group Type object with the given GroupName.
        /// </summary>
        /// <param name="strDistinguishedName">Distinguished name of the group.</param>
        /// <param name="grpType">Type of the Group.</param>
        /// <param name="connectionObj">The already existing LDAP connection object.</param>
        /// <returns>True if group is successfully created.</returns>
        public bool CreateGroup(string strDistinguishedName, uint grpType, DirectoryConnection connectionObj)
        {
            AddRequest request = new AddRequest();
            request.DistinguishedName = strDistinguishedName;
            request.Attributes.Add(new DirectoryAttribute("objectClass", "group"));
            request.Attributes.Add(new DirectoryAttribute("groupType", Convert.ToString(grpType)));
            connectionObj.SendRequest(request);
            return true;
        }

        /// <summary>
        /// DeleteGroup method deletes a given account object on the domain.
        /// </summary>
        /// <param name="strDistinguishedName">DistinguishedName of the account.</param>
        /// <param name="connectionObj">LDAP connection Object.</param>
        public void DeleteGroup(string strDistinguishedName, DirectoryConnection connectionObj)
        {
            DeleteRequest request = new DeleteRequest();
            request.DistinguishedName = strDistinguishedName;
            connectionObj.SendRequest(request);
        }

        /// <summary>
        /// This method obtaines the names of the members from the member attribute.
        /// </summary>
        /// <param name="name">
        /// Name of the group.
        /// </param>
        /// <param name="count">
        /// Represent the number of members in member attribute.
        /// </param>
        /// <returns>Members names.</returns>
        public string[] getMembersFromAttribute(string name, ref int count)
        {
            ITestSite SamrTestSite = null;
            SamrTestSite = TestClassBase.BaseTestSite;

            bool bAllocateMemory = false;
            SearchRequest searchRequest = new SearchRequest();

            string DName = SamrTestSite.Properties.Get("LDAPPathDN") + name + "," +
                           SAMRProtocolAdapter.Instance(SamrTestSite).primaryDomainUserContainerDN;

            searchRequest.DistinguishedName = DName;
            searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;

            SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
            //LdapCon.Dispose();
            string[] tempMembers = { string.Empty };
            string[] tempArray = { string.Empty };
            string[] memval = { string.Empty };
            int i = 0, j = 0;

            // Finding entries in the member attribute of the group.
            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                {
                    switch (attribute.Name)
                    {
                        case "member":
                            for (i = 0; i < attribute.Count; i++)
                            {
                                if (!bAllocateMemory)
                                {
                                    memval = new string[attribute.Count];
                                    tempArray = new string[attribute.Count];
                                    bAllocateMemory = true;
                                }
                                memval[i] = attribute[i].ToString();
                                // Splitting member names.
                                tempMembers = memval[i].Split(',');

                                for (j = i; j <= i; j++)
                                    tempArray[j] = tempMembers[0];
                            }
                            count = attribute.Count;
                            break;
                        default:
                            break;
                    }
                }
            }

            return tempArray;
        }

        /// <summary>
        /// ValidateGroupAttributes method Queries attributes of an object given its GroupName.
        /// </summary>
        /// <param name="strDistinguishedName">DistinguishedName of the account.</param>
        /// <param name="ldapFilter">Whether Group or User.</param>
        /// <param name="connection">LDAP connection Object.</param>
        /// <returns>True if conditions checked are true.</returns>
        /// Disable warning CA1502 because according to Test Case design, excessive complexity is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public bool ValidateGroupAttributes(string strDistinguishedName, string ldapFilter, DirectoryConnection connection)
        {
            //Indicates if requirements pertaining to the attributes of the created Group have been verified.
            bool isRequirementValidated = false;
            UInt64 valueSamGrp = 0;
            string grpObjClsVal = string.Empty;
            UInt64 groupType = 0;
            UInt64 trstUserAccountControl = 0;
            UInt64 trstPrimaryGroupId = 0;

            #region Search Request
            // Since the filter we have given is objectClass = user so the objectClass attribute value is derived from user class
            SearchRequest request = new SearchRequest(
                strDistinguishedName, "(objectClass=" + ldapFilter + ")",
                System.DirectoryServices.Protocols.SearchScope.Subtree,
                new string[] { "objectClass", "groupType", "sAMAccountType", "userAccountControl", "primaryGroupID" });

            SearchResponse response = (SearchResponse)LdapCon.SendRequest(request);

            #region Record the Group Attribute values for validation
            foreach (SearchResultEntry entry in response.Entries)
            {
                SearchResultAttributeCollection attributes = entry.Attributes;
                foreach (DirectoryAttribute attribute in attributes.Values)
                {
                    switch (attribute.Name)
                    {
                        case "sAMAccountType":
                            for (int i = 0; i < attribute.Count; i++)
                            {
                                valueSamGrp = (uint)(int.Parse(attribute[i].
                                    ToString()));
                            }
                            break;

                        case "objectClass":

                            for (int i = 0; i < attribute.Count; i++)
                            {
                                grpObjClsVal = attribute[i].ToString();
                            }
                            break;

                        case "groupType":

                            for (int i = 0; i < attribute.Count; i++)
                            {
                                groupType = (uint)(int.Parse(attribute[i].
                                    ToString()));
                            }
                            break;
                        case "userAccountControl":

                            for (int i = 0; i < attribute.Count; i++)
                            {
                                trstUserAccountControl = (uint)(int.Parse(attribute[i].
                                    ToString()));
                            }
                            break;
                        case "primaryGroupID":

                            for (int i = 0; i < attribute.Count; i++)
                            {
                                trstPrimaryGroupId = (uint)(int.Parse(attribute[i].
                                    ToString()));
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            #endregion
            #endregion

            // Validation that checks for the necessary condition that once a
            // group of type "GROUP_TYPE_ACCOUNT_GROUP" is created, its SamAccountType
            // attribute MUST be set to "SAM_NON_SECURITY_GROUP_OBJECT".
            if (grpObjClsVal == "group" &&
                ((uint)groupType == Utilities.GROUP_TYPE_ACCOUNT_GROUP) &&
                (Utilities.SAM_NON_SECURITY_GROUP_OBJECT ==
                ((uint)valueSamGrp & Utilities.SAM_NON_SECURITY_GROUP_OBJECT)))
            {
                DeleteGroup(strDistinguishedName, connection);
                isRequirementValidated = true;
            }

            // Validation that checks for the necessary condition that once a group of type 
            // "GROUP_TYPE_UNIVERSAL_GROUP" is created, its SamAccountType attribute MUST be 
            // set to "SAM_NON_SECURITY_GROUP_OBJECT".
            if (grpObjClsVal == "group" &&
               ((uint)groupType == Utilities.GROUP_TYPE_RESOURCE_GROUP) &&
               (Utilities.SAM_NON_SECURITY_ALIAS_OBJECT ==
               ((uint)valueSamGrp & Utilities.SAM_NON_SECURITY_ALIAS_OBJECT)))
            {
                DeleteGroup(strDistinguishedName, connection);
                isRequirementValidated = true;
            }

            // Validation that checks for the necessary condition that once a
            // group of type "GROUP_TYPE_SECURITY_UNIVERSAL" is created, its SamAccountType
            // attribute MUST be set to "SAM_GROUP_OBJECT".
            if (grpObjClsVal == "group" &&
                ((uint)groupType == Utilities.GROUP_TYPE_SECURITY_UNIVERSAL) &&
                (Utilities.SAM_GROUP_OBJECT ==
                ((uint)valueSamGrp & Utilities.SAM_GROUP_OBJECT)))
            {
                DeleteGroup(strDistinguishedName, connection);
                isRequirementValidated = true;
            }

            // Validation that checks for the necessary condition that once a
            // group of type "GROUP_TYPE_UNIVERSAL_GROUP" is created, its SamAccountType
            // attribute MUST be set to "SAM_NON_SECURITY_GROUP_OBJECT".
            if (grpObjClsVal == "group" &&
                ((uint)groupType == Utilities.GROUP_TYPE_UNIVERSAL_GROUP) &&
                (Utilities.SAM_NON_SECURITY_GROUP_OBJECT ==
                ((uint)valueSamGrp & Utilities.SAM_NON_SECURITY_GROUP_OBJECT)))
            {
                isRequirementValidated = true;
                DeleteGroup(strDistinguishedName, connection);
            }

            // Validation that checks for the necessary condition that once an Account
            // of type "GROUP_TYPE_UNIVERSAL_GROUP" is created, its SamAccountType
            // attribute MUST be set to "SAM_TRUST_ACCOUNT", its 
            // UserAccountControl attribute to be set to UF_INTERDOMAIN_TRUST_ACCOUNT,
            // and PrimaryGroupId MUST be set to 513.
            if (("user" == grpObjClsVal) &&
                (Utilities.UF_INTERDOMAIN_TRUST_ACCOUNT ==
                ((uint)trstUserAccountControl & Utilities.UF_INTERDOMAIN_TRUST_ACCOUNT))
                && ((uint)valueSamGrp == Utilities.SAM_TRUST_ACCOUNT) && (trstPrimaryGroupId == 513))
            {
                // Requirement R494,R495,R497 are checked for necessary condition(above).
                // They will be covered IF AND ONLY IF isRequirementValidated is True.
                isRequirementValidated = true;
            }

            return isRequirementValidated;
        }

        /// <summary>
        /// This method obtains the attributes of an account
        /// from the inputSearch response and compares them with the
        /// given set of input attribute values.
        /// </summary>
        /// <param name="searchResponse">Search response object having attribute set.</param>
        /// <param name="entryRid">
        /// Attribute "entryRid" to be compared with the value in the search response.
        /// </param>
        /// <param name="entryName">
        /// Attribute "entryName" to be compared with the value in the search response.
        /// </param>
        /// <param name="adminComment">
        /// Attribute "adminComment" to be compared with the value in the search response.
        /// </param>
        /// <returns>Bool.</returns>
        public bool attributeCheck(SearchResponse searchResponse,
                                   string entryRid,
                                   ushort[] entryName,
                                   ushort[] adminComment)
        {
            SecurityIdentifier Sid;
            int j;
            string sid = string.Empty;
            string rid = string.Empty;
            string samAccName = string.Empty;
            Object[] values;
            byte[][] objectSid = new byte[0][];
            string aComment = null;
            char[] tempStr = new char[entryName.Length + 1];

            for (j = 0; j < (entryName.Length); j++)
            {
                tempStr[j] = Convert.ToChar(entryName[j]);
            }

            string accountName = new string(tempStr, 0, tempStr.Length);
            accountName = accountName.Remove(accountName.Length - 1);
            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                {

                    switch (attribute.Name)
                    {
                        case "sAMAccountName":

                            for (int i = 0; i < attribute.Count; i++)
                            {
                                samAccName = attribute[i].ToString();
                            }
                            break;
                        case "objectSid":
                            values = attribute.GetValues(Type.GetType("System.Byte[]"));
                            for (j = 0; j < values.Length; j++)
                            {
                                objectSid = new byte[values.Length][];
                                objectSid[j] = (byte[])values[j];
                            }
                            Sid = new SecurityIdentifier(objectSid[j - 1], 0);
                            sid = Sid.ToString();
                            rid = getRid(sid);
                            break;
                        case "description":
                            for (int i = 0; i < attribute.Count; i++)
                            {
                                aComment = attribute[i].ToString();

                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (adminComment != null)
            {
                char[] tempStr1 = new char[adminComment.Length + 1];
                for (j = 0; j < (adminComment.Length); j++)
                {
                    tempStr1[j] = Convert.ToChar(adminComment[j]);
                }
                string adComment = new string(tempStr1, 0, tempStr1.Length);
                adComment = adComment.Remove(adComment.Length - 1);
                if (rid.Equals(entryRid) && accountName.Equals(samAccName) && adComment.Equals(aComment))
                    return true;
                else
                    return false;
            }
            else
            {
                if (rid.Equals(entryRid) && accountName.Equals(samAccName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// This method compares the user account values of the given set
        /// of accounts and returns true if they match.
        /// </summary>
        /// <param name="names">
        /// Names for which User Account Values to be compared.</param>
        /// <param name="count">Count of names.</param>
        /// <returns>Bool.</returns>
        public bool isUACSame(string[] names, uint count)
        {
            int nCount1 = 0, i = 0, j = 0;
            uint uac = 0;
            SearchRequest searchRequest = new SearchRequest();
            ITestSite SamrTestSite = null;
            SamrTestSite = TestClassBase.BaseTestSite;

            for (i = 0; i < count; i++)
            {
                searchRequest.DistinguishedName = SamrTestSite.Properties.Get("LDAPPathDN") + names[i] + "," +
                                                     SAMRProtocolAdapter.Instance(SamrTestSite).primaryDomainUserContainerDN;

                searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;
                SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);

                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                    {
                        switch (attribute.Name)
                        {
                            case "userAccountControl":
                                for (j = 0; j < attribute.Count; j++)
                                {
                                    uac = (uint)(int.Parse(attribute[j].ToString()));
                                }
                                if (Utilities.UF_NORMAL_ACCOUNT == (uac & Utilities.UF_NORMAL_ACCOUNT))
                                {
                                    nCount1++;
                                }
                                break;
                        }
                    }
                }
            }

            //LdapCon.Dispose();
            if (nCount1 == count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// This method compares the Group Type values of the given set
        /// of Groups and returns true if they match.
        /// </summary>
        /// <param name="names">
        /// Names for which User Account Values to be compared.</param>
        /// <param name="count">Count of names.</param>
        /// <returns>Bool.</returns>
        public bool isGroupSame(string[] names, uint count)
        {
            int nCount1 = 0, i = 0, j = 0;
            uint GroupType = 0;
            SearchRequest searchRequest = new SearchRequest();
            ITestSite SamrTestSite = null;
            SamrTestSite = TestClassBase.BaseTestSite;
            for (i = 0; i < count; i++)
            {
                searchRequest.DistinguishedName = SamrTestSite.Properties.Get("LDAPPathDN") + names[i] + "," +
                                                     SAMRProtocolAdapter.Instance(SamrTestSite).primaryDomainUserContainerDN;

                searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;
                SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                    {
                        switch (attribute.Name)
                        {
                            case "groupType":
                                for (j = 0; j < attribute.Count; j++)
                                {
                                    GroupType = (uint)(int.Parse(attribute[j].ToString()));
                                }
                                if ((Utilities.GROUP_TYPE_SECURITY_ACCOUNT == (GroupType & Utilities.GROUP_TYPE_SECURITY_ACCOUNT)) ||
                                    (Utilities.GROUP_TYPE_SECURITY_UNIVERSAL == (GroupType & Utilities.GROUP_TYPE_SECURITY_UNIVERSAL)))
                                {
                                    nCount1++;
                                }
                                break;
                        }
                    }
                }
            }

            //LdapCon.Dispose();
            if (nCount1 == count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// IsMemberOfAdmin checks if the given member is present in the given group.
        /// </summary>
        /// <param name="AdminName">Represent the group in which member to be checked.</param>
        /// <returns>Bool.</returns>
        public bool IsMemberOfAdmin(string AdminName)
        {
            string[] tempMembers = { string.Empty };
            string[] tempArray = { string.Empty };
            string[] memval = { string.Empty };
            int i = 0, j = 0;
            bool bAllocateMemory = false;
            string memberToCheck = string.Empty;
            ITestSite SamrTestSite = null;
            SamrTestSite = TestClassBase.BaseTestSite;
            SearchRequest searchRequest = new SearchRequest();
            searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Base;

            if ("Administrators" == AdminName)
            {
                searchRequest.DistinguishedName = SamrTestSite.Properties.Get("LDAPPathDN") + AdminName + "," +
                                                 SamrTestSite.Properties.Get("BuiltinDomainObjectDN");
            }
            if ("Domain Admins" == AdminName)
            {
                searchRequest.DistinguishedName = SamrTestSite.Properties.Get("LDAPPathDN") + AdminName + "," +
                                                 SAMRProtocolAdapter.Instance(SamrTestSite).primaryDomainUserContainerDN;
            }
            memberToCheck = SamrTestSite.Properties.Get("LDAPPathDN") + AdminName;
            SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
            //LdapCon.Dispose();

            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                {
                    switch (attribute.Name)
                    {
                        case "member":
                            for (i = 0; i < attribute.Count; i++)
                            {
                                if (!bAllocateMemory)
                                {
                                    memval = new string[attribute.Count];
                                    tempArray = new string[attribute.Count];
                                    bAllocateMemory = true;
                                }
                                memval[i] = attribute[i].ToString();
                                // Splitting member names.
                                tempMembers = memval[i].Split(',');

                                for (j = i; j <= i; j++)
                                    tempArray[j] = tempMembers[0];
                            }
                            break;
                    }
                }
            }
            for (int nCount = 0; nCount < j; nCount++)
            {
                if (memberToCheck == tempArray[nCount])
                {
                    return true;
                }
            }
            return false;
        }

        public string GetSAMNameObject(string samName)
        {
            string accountName = string.Empty;
            ITestSite SamrTestSite = null;
            SamrTestSite = TestClassBase.BaseTestSite;

            SearchRequest searchRequest = new SearchRequest();
            searchRequest.DistinguishedName = SAMRProtocolAdapter.Instance(SamrTestSite).primaryDomainUserContainerDN;
            searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Subtree;
            searchRequest.Filter = "(sAMAccountName=" + samName + ")";
            SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
            //LdapCon.Dispose();

            if (searchResponse.Entries.Count == 0)
                return "";
            else
                return searchResponse.Entries[0].DistinguishedName;
        }

        /// <summary>
        /// This method searches if the give user exists in server's database.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Bool.</returns>
        public bool isObjectPresent(string userName)
        {
            bool isPresent = false;
            int i;
            string accountName = string.Empty;
            ITestSite SamrTestSite = null;
            SamrTestSite = TestClassBase.BaseTestSite;

            SearchRequest searchRequest = new SearchRequest();
            searchRequest.DistinguishedName = SAMRProtocolAdapter.Instance(SamrTestSite).primaryDomainUserContainerDN;
            searchRequest.Scope = System.DirectoryServices.Protocols.SearchScope.Subtree;
            SearchResponse searchResponse = (SearchResponse)LdapCon.SendRequest(searchRequest);
            //LdapCon.Dispose();

            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                foreach (DirectoryAttribute attribute in entry.Attributes.Values)
                {
                    switch (attribute.Name)
                    {
                        case "name":
                            for (i = 0; i < attribute.Count; i++)
                            {
                                accountName = attribute[i].ToString();
                            }
                            isPresent = string.Equals(userName, accountName,
                                                      StringComparison.CurrentCultureIgnoreCase);
                            break;
                    }
                }
                if (isPresent)
                    break;
            }
            return isPresent;
        }

        #region RemoveAccessRights
        /// <summary>
        /// RemoveAccessRights method removes the particular access right for a particular user on a
        /// particular AD Container/Object.
        /// </summary>
        /// <param name="dn">The distinguished name of the Container/Object.</param>
        /// <param name="credUser">The name of the user to whom the permissions to be set.</param>
        /// <param name="domain">The name of the domain to which user is belongs to.</param>
        /// <param name="accessRight">The name of the access right to be removed.</param>
        /// <param name="controlType">Allow/Deny particular ActiveDirecotyRights.</param>
        /// <param name="site">The site attribute.</param>
        public void RemoveAccessRights(string dn,
                                       string credUser,
                                       string domain,
                                       ActiveDirectoryRights accessRight,
                                       AccessControlType controlType,
                                       ITestSite site)
        {
            DirectoryEntry entry = new DirectoryEntry(
                "LDAP://" + SAMRProtocolAdapter.Instance(site).primaryDomainFqdn + "/" + dn,
                SAMRProtocolAdapter.Instance(site).DomainAdministratorName,
                SAMRProtocolAdapter.Instance(site).DomainUserPassword,
                AuthenticationTypes.Secure);
            ActiveDirectorySecurity sd = entry.ObjectSecurity;
            NTAccount accountName = new NTAccount(domain, credUser);
            IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);
            sd.RemoveAccessRule(myRule);
            entry.ObjectSecurity.RemoveAccessRule(myRule);
            entry.CommitChanges();
            entry.Close();
        }
        #endregion

        #region SetControlAccessRights
        /// <summary>
        /// Sets requested control access rights for a specified user.
        /// </summary>
        /// <param name="dn">Distinguished name of the object.</param>
        /// <param name="domainUser">User on which the right is to be set.</param>
        /// <param name="domain">Domain name.</param>
        /// <param name="controlAccessRightGuid">GUID of the control access right.</param>
        /// <param name="right">Active directory extended right.</param>
        /// <param name="accessControl">Allow or deny a particular access right.</param>
        /// <param name="site">The site attribute.</param>
        public void SetControlAcessRights(string dn,
                                          string domainUser,
                                          string domain,
                                          Guid controlAccessRightGuid,
                                          ActiveDirectoryRights right,
                                          AccessControlType accessControl,
                                          ITestSite site)
        {
            DirectoryEntry user = new DirectoryEntry(
                "LDAP://" + SAMRProtocolAdapter.Instance(site).primaryDomainFqdn + "/" + dn,
                SAMRProtocolAdapter.Instance(site).DomainAdministratorName,
                SAMRProtocolAdapter.Instance(site).DomainUserPassword,
                AuthenticationTypes.Secure);

            ActiveDirectorySecurity userSecurity = user.ObjectSecurity;
            Guid controlAccessRight = new Guid(controlAccessRightGuid.ToByteArray());
            NTAccount accountName = new NTAccount(domain, domainUser);
            IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), right, accessControl, controlAccessRight);
            userSecurity.AddAccessRule(myRule);
            user.ObjectSecurity.AddAccessRule(myRule);
            user.CommitChanges();
            user.Close();
        }

        #region SetAccessRights

        /// <summary>
        /// SetAccessRights method sets the particular access right for a particular user on a particular AD 
        /// Container/Object.
        /// </summary>
        /// <param name="dn">The distinguished name of the Container/Object.</param>
        /// <param name="credUser">The name of the user to whom the permissions to be set.</param>
        /// <param name="domain">The name of the domain to which user is belongs to.</param>
        /// <param name="accessRight">The name of the access right to be set.</param>
        /// <param name="controlType">Allow/Deny particular ActiveDirecotyRights.</param>
        /// <param name="site">The site attribute.</param>
        public void SetAccessRights(string dn,
                                    string credUser,
                                    string domain,
                                    ActiveDirectoryRights accessRight,
                                    AccessControlType controlType,
                                    ITestSite site)
        {
            DirectoryEntry entry = new DirectoryEntry(
                "LDAP://" + SAMRProtocolAdapter.Instance(site).primaryDomainFqdn + "/" + dn, SAMRProtocolAdapter.Instance(site).DomainAdministratorName, SAMRProtocolAdapter.Instance(site).DomainUserPassword, AuthenticationTypes.Secure);
            ActiveDirectorySecurity sd = entry.ObjectSecurity;
            NTAccount accountName = new NTAccount(domain, credUser);
            IdentityReference acctSID = accountName.Translate(typeof(SecurityIdentifier));
            ActiveDirectoryAccessRule myRule = new ActiveDirectoryAccessRule(new SecurityIdentifier(acctSID.Value), accessRight, controlType);
            sd.AddAccessRule(myRule);
            entry.ObjectSecurity.AddAccessRule(myRule);
            entry.CommitChanges();
            entry.Close();
        }

        #endregion

        #endregion

        #endregion

        #region Password setting operation

        /// <summary>
        /// SetPassword sets the password of a user given the password.
        /// </summary>
        /// <param name="NewPassword">Password to be set.</param>
        /// <param name="UserHandle">User Handle.</param>
        /// <param name="attributeForUpdate">Specify which attribute will to update.</param>
        /// <param name="SamrAdapterObj">Instance of the SamrRpcAdapter.</param>
        /// <returns>Bool.</returns>
        public bool SetPassword(string NewPassword,
                                IntPtr UserHandle,
                                AttributeType attributeForUpdate,
                                ISamrRpcAdapter SamrAdapterObj)
        {
            // UserAllInfo is filled with UserAllInformation class values and called initially.
            _SAMPR_USER_INFO_BUFFER userAllInfo = new _SAMPR_USER_INFO_BUFFER();

            // Compute NTOWF if LmStored is null.
            if (attributeForUpdate == AttributeType.Ntowf)
            {
                byte[] oldpswdbytes = new byte[NewPassword.Length];

                userAllInfo.All.WhichFields = 0x01000000;
                userAllInfo.All.NtPasswordPresent = 1;
                userAllInfo.All.NtOwfPassword.Length = 32;
                userAllInfo.All.NtOwfPassword.MaximumLength = 32;
                userAllInfo.All.NtOwfPassword.Buffer = new ushort[userAllInfo.All.NtOwfPassword.Length / 2];

                SamrCryptography crypt = new SamrCryptography(Encoding.UTF8.GetString(oldpswdbytes), NewPassword);
                _ENCRYPTED_LM_OWF_PASSWORD nt_owf = new _ENCRYPTED_LM_OWF_PASSWORD();
                nt_owf = crypt.GetOldNtOwfPasswordEncryptedWithSessionKey(SamrAdapterObj.SessionKey);
                Array.Copy(nt_owf.data, userAllInfo.All.NtOwfPassword.Buffer, 16);
            }

            // Compute LMOWF if LmStored is null.
            if (attributeForUpdate == AttributeType.Lmowf)
            {
                userAllInfo.All.WhichFields = 0x02000000;
                userAllInfo.All.LmPasswordPresent = 1;
                userAllInfo.All.LmOwfPassword.Length = 16;
                userAllInfo.All.LmOwfPassword.MaximumLength = 16;
                userAllInfo.All.LmOwfPassword.Buffer = new ushort[userAllInfo.All.LmOwfPassword.MaximumLength / 2];

                char[] temp = new char[NewPassword.Length];
                temp = NewPassword.ToCharArray();
                byte[] oldpswdbytes = new byte[temp.Length];

                for (int i = 0; i < temp.Length; i++)
                {
                    oldpswdbytes[i] = (byte)temp[i];
                }

                SamrCryptography crypt = new SamrCryptography(Encoding.UTF8.GetString(oldpswdbytes), NewPassword);
                _ENCRYPTED_LM_OWF_PASSWORD lm_owf = new _ENCRYPTED_LM_OWF_PASSWORD();
                lm_owf = crypt.GetOldLmOwfPasswordEncryptedWithSessionKey(SamrAdapterObj.SessionKey);

                for (int loopVal1 = 0, loopVal2 = 0;
                     loopVal1 < lm_owf.data.Length / 2;
                     loopVal1++, loopVal2 += 2)
                {
                    userAllInfo.All.LmOwfPassword.Buffer[loopVal1] = BitConverter.ToUInt16(lm_owf.data, loopVal2);
                }
            }

            // Compute the NTOWF and LMOWF respectively.
            if (attributeForUpdate == AttributeType.Both)
            {
                char[] temp = new char[NewPassword.Length];
                temp = NewPassword.ToCharArray();
                byte[] oldpswdbytes = new byte[temp.Length];

                for (int i = 0; i < temp.Length; i++)
                {
                    oldpswdbytes[i] = (byte)temp[i];
                }

                SamrCryptography crypt = new SamrCryptography(Encoding.UTF8.GetString(oldpswdbytes), NewPassword);
                _ENCRYPTED_LM_OWF_PASSWORD lm_owf = new _ENCRYPTED_LM_OWF_PASSWORD();
                _ENCRYPTED_LM_OWF_PASSWORD nt_owf = new _ENCRYPTED_LM_OWF_PASSWORD();
                lm_owf = crypt.GetOldLmOwfPasswordEncryptedWithSessionKey(SamrAdapterObj.SessionKey);
                nt_owf = crypt.GetOldNtOwfPasswordEncryptedWithSessionKey(SamrAdapterObj.SessionKey);

                userAllInfo.All.WhichFields = 0x01000000 | 0x02000000;
                // Setting the NtPasswordPresent and LmPasswordPresent whichfield to non zero values.
                userAllInfo.All.NtPasswordPresent = 1;
                userAllInfo.All.LmPasswordPresent = 1;

                // Populating the NtOwfPassword  and structures of UserAllInformation.
                userAllInfo.All.NtOwfPassword.Length = 16;
                userAllInfo.All.NtOwfPassword.MaximumLength = 16;
                userAllInfo.All.NtOwfPassword.Buffer = new ushort[userAllInfo.All.NtOwfPassword.MaximumLength / 2];

                userAllInfo.All.LmOwfPassword.Length = 16;
                userAllInfo.All.LmOwfPassword.MaximumLength = 16;
                userAllInfo.All.LmOwfPassword.Buffer = new ushort[userAllInfo.All.LmOwfPassword.MaximumLength / 2];

                for (int loopVal1 = 0, loopVal2 = 0;
                     loopVal1 < nt_owf.data.Length / 2;
                     loopVal1++, loopVal2 += 2)
                {
                    userAllInfo.All.NtOwfPassword.Buffer[loopVal1] = BitConverter.ToUInt16(nt_owf.data, loopVal2);
                    userAllInfo.All.LmOwfPassword.Buffer[loopVal1] = BitConverter.ToUInt16(lm_owf.data, loopVal2);
                }
            }

            int retval = 2;
            retval = SamrAdapterObj.SamrSetInformationUser2(UserHandle,
                                                            _USER_INFORMATION_CLASS.UserAllInformation,
                                                            userAllInfo);
            if (STATUS_SUCCESS == retval)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ChangePassword changes the password of a user given the old password.
        /// </summary>
        /// <param name="oldPassword">Old password string of the user.</param>
        /// <param name="newPassword">New password string of the user.</param>
        /// <param name="UserHandle">User Handle.</param>
        /// <param name="SamrAdapterObj">Instance of the SamrRpcAdapter.</param>
        /// <returns>Bool.</returns>
        public uint ChangePassword(string oldPassword,
                                   string newPassword,
                                   IntPtr UserHandle,
                                   ISamrRpcAdapter SamrAdapterObj)
        {
            uint status = 0;
            SamrCryptography samrCryptography = new SamrCryptography(oldPassword, newPassword);
            status = (uint)SamrAdapterObj.SamrChangePasswordUser(
                                            UserHandle,
                                            1,
                                            samrCryptography.GetOldLmEncryptedWithNewLm(),
                                            samrCryptography.GetNewLmEncryptedWithOldLm(),
                                            1,
                                            samrCryptography.GetOldNtEncryptedWithNewNt(),
                                            samrCryptography.GetNewNtEncryptedWithOldNt(),
                                            0,
                                            samrCryptography.GetNewNtEncryptedWithNewLm(),
                                            0,
                                            samrCryptography.GetNewLmEncryptedWithNewNt());

            return status;
        }

        #endregion

    }
}
