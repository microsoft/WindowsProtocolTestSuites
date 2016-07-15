// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// The DS_REPL_NEIGHBORW_BLOB structure is a representation of a tuple from the repsFrom or repsTo abstract 
    /// attribute of an NC replica. This structure, retrieved using an LDAP search method, is an alternative 
    /// representation of DS_REPL_NEIGHBORW, retrieved using the IDL_DRSGetReplInfo RPC method.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class DS_REPL_NEIGHBORW_BLOB
    {
        /// <summary>
        /// A 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string that 
        /// contains the naming context (NC) to which this replication state data pertains.
        /// </summary>
        public uint oszNamingContext;

        /// <summary>
        /// A 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string that 
        /// contains the distinguished name (DN) of the nTDSDSA object of the source server to which this replication
        /// state data pertains. Each source server has different associated neighbor data.
        /// </summary>
        public uint oszSourceDsaDN;

        /// <summary>
        /// A 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string that 
        /// contains the transport-specific network address of the source server—that is, a directory name service name
        /// for RPC/IP replication, or a Simple Mail Transfer Protocol (SMTP) address for an SMTP replication.
        /// </summary>
        public uint oszSourceDsaAddress;

        /// <summary>
        /// A 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string that 
        /// contains the DN of the interSiteTransport object that corresponds to the transport over which replication is
        /// performed. This member contains NULL for RPC/IP replication.
        /// </summary>
        public uint oszAsyncIntersiteTransportDN;

        /// <summary>
        /// A 32-bit bit field containing a set of flags that specify attributes and options for the replication data. 
        /// This can be zero or a combination of one or more of the following flags presented in big-endian byte order.
        /// </summary>
        public uint dwReplicaFlags;

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        public uint dwReserved;

        /// <summary>
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the NC that 
        /// corresponds to oszNamingContext.
        /// </summary>
        public Guid uuidNamingContextObjGuid;

        /// <summary>
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the nTDSDSA object
        /// that corresponds to oszSourceDsaDN.
        /// </summary>
        public Guid uuidSourceDsaObjGuid;

        /// <summary>
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the invocation ID used by the source
        /// server as of the last replication attempt.
        /// </summary>
        public Guid uuidSourceDsaInvocationID;

        /// <summary>
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the intersite 
        /// transport object that corresponds to oszAsyncIntersiteTransportDN.
        /// </summary>
        public Guid uuidAsyncIntersiteTransportObjGuid;

        /// <summary>
        /// An update sequence number (USN) value, as defined in section 3.1.1.1.9, containing the USN of the last
        /// object update received.
        /// </summary>
        public long usnLastObjChangeSynced;

        /// <summary>
        /// A USN value, as defined in section 3.1.1.1.9, containing the usnLastObjChangeSynced value at the end of the
        /// last complete, successful replication cycle, or 0 if none.
        /// </summary>
        public long usnAttributeFilter;

        /// <summary>
        /// A FILETIME structure that contains the date and time that the last successful replication cycle was
        /// completed from this source. All members of this structure are zero if the replication cycle has never been
        /// completed.
        /// </summary>
        public _FILETIME ftimeLastSyncSuccess;

        /// <summary>
        /// A FILETIME structure that contains the date and time of the last replication attempt from this source. All
        /// members of this structure are zero if the replication has never been attempted.
        /// </summary>
        public _FILETIME ftimeLastSyncAttempt;

        /// <summary>
        /// A 32-bit unsigned integer specifying a Windows error code associated with the last replication attempt from
        /// this source. Contains ERROR_SUCCESS if the last attempt was successful or replication was not attempted.
        /// </summary>
        public uint dwLastSyncResult;

        /// <summary>
        /// A 32-bit integer specifying the number of failed replication attempts that have been made from this source
        /// since the last successful replication attempt or since the source was added as a neighbor, if no previous
        /// attempt succeeded.
        /// </summary>
        public uint cNumConsecutiveSyncFailures;

        /// <summary>
        /// This field contains all the null-terminated strings that are pointed to by the offset fields in the 
        /// structure (oszNamingContext, oszSourceDsaDN, oszSourceDsaAddress, oszAsyncIntersiteTransportDN). The strings
        /// are packed into this field, and the offsets can be used to determine the start of each string.
        /// </summary>
        public byte[] data;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="szNamingContext">
        /// A null-terminated Unicode string that contains the naming context (NC) to which this replication state data
        /// pertains.
        /// </param>
        /// <param name="szSourceDsaDN">
        /// A null-terminated Unicode string that contains the distinguished name (DN) of the nTDSDSA object of the 
        /// source server to which this replication state data pertains. Each source server has different associated 
        /// neighbor data.
        /// </param>
        /// <param name="szSourceDsaAddress">
        /// A null-terminated Unicode string that contains the distinguished name (DN) of the nTDSDSA object of the 
        /// source server to which this replication state data pertains. Each source server has different associated 
        /// neighbor data.
        /// </param>
        /// <param name="szAsyncIntersiteTransportDN">
        /// A null-terminated Unicode string that contains the DN of the interSiteTransport object that corresponds to 
        /// the transport over which replication is performed. This member contains NULL for RPC/IP replication.
        /// </param>
        /// <param name="dwReplicaFlags">A 32-bit bit field containing a set of flags that specify attributes and 
        /// options for the replication data. This can be zero or a combination of one or more of the following flags 
        /// presented in big-endian byte order.
        /// </param>
        /// <param name="dwReserved">Reserved for future use.</param>
        /// <param name="uuidNamingContextObjGuid">Reserved for future use.</param>
        /// <param name="uuidSourceDsaObjGuid">
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the nTDSDSAobject that
        /// corresponds to oszSourceDsaDN.
        /// </param>
        /// <param name="uuidSourceDsaInvocationID">
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the invocation ID used by the source 
        /// server as of the last replication attempt.
        /// </param>
        /// <param name="uuidAsyncIntersiteTransportObjGuid">
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the intersite
        /// transport object that corresponds to oszAsyncIntersiteTransportDN.
        /// </param>
        /// <param name="usnLastObjChangeSynced">
        /// An update sequence number (USN) value, as defined in section 3.1.1.1.9, containing the USN of the last 
        /// object update received.
        /// </param>
        /// <param name="usnAttributeFilter">
        /// A USN value, as defined in section 3.1.1.1.9, containing the usnLastObjChangeSynced value at the end of the
        /// last complete, successful replication cycle, or 0 if none.
        /// </param>
        /// <param name="ftimeLastSyncSuccess">
        /// A FILETIME structure that contains the date and time that the last successful replication cycle was completed
        /// from this source. All members of this structure are zero if the replication cycle
        /// has never been completed.
        /// </param>
        /// <param name="ftimeLastSyncAttempt">
        /// A FILETIME structure that contains the date and time of the last replication attempt from this source. All
        /// members of this structure are zero if the replication has never been attempted.
        /// </param>
        /// <param name="dwLastSyncResult">
        /// A 32-bit unsigned integer specifying a Windows error code associated with the last replication attempt from 
        /// this source. Contains ERROR_SUCCESS if the last attempt was successful or replication was not attempted.
        /// </param>
        /// <param name="cNumConsecutiveSyncFailures">
        /// A 32-bit integer specifying the number of failed replication attempts that have been made from this source 
        /// since the last successful replication attempt or since the source was added as a neighbor, if no previous
        /// attempt succeeded.
        /// </param>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public DS_REPL_NEIGHBORW_BLOB(
            string szNamingContext,
            string szSourceDsaDN,
            string szSourceDsaAddress,
            string szAsyncIntersiteTransportDN,
            uint dwReplicaFlags,
            uint dwReserved,
            Guid uuidNamingContextObjGuid,
            Guid uuidSourceDsaObjGuid,
            Guid uuidSourceDsaInvocationID,
            Guid uuidAsyncIntersiteTransportObjGuid,
            long usnLastObjChangeSynced,
            long usnAttributeFilter,
            _FILETIME ftimeLastSyncSuccess,
            _FILETIME ftimeLastSyncAttempt,
            uint dwLastSyncResult,
            uint cNumConsecutiveSyncFailures
            )
        {
            this.data = ArrayUtility.ConcatenateArrays(
                Encoding.Unicode.GetBytes(szNamingContext), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szSourceDsaDN), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szSourceDsaAddress), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szAsyncIntersiteTransportDN), AdtsLdapUtility.nullTerminator);
            this.oszNamingContext = (uint)Marshal.OffsetOf(typeof(DS_REPL_NEIGHBORW_BLOB), AdtsLdapUtility.Data);
            this.oszSourceDsaDN = this.oszNamingContext + (uint)szNamingContext.Length +
                (uint)AdtsLdapUtility.nullTerminator.Length;
            this.oszSourceDsaAddress = this.oszSourceDsaDN + (uint)szSourceDsaDN.Length +
                (uint)AdtsLdapUtility.nullTerminator.Length;
            this.oszAsyncIntersiteTransportDN = this.oszSourceDsaAddress + (uint)szSourceDsaAddress.Length +
                (uint)AdtsLdapUtility.nullTerminator.Length;
            this.dwReplicaFlags = dwReplicaFlags;
            this.dwReserved = dwReserved;
            this.uuidNamingContextObjGuid = uuidNamingContextObjGuid;
            this.uuidSourceDsaObjGuid = uuidSourceDsaObjGuid;
            this.uuidSourceDsaInvocationID = uuidSourceDsaInvocationID;
            this.uuidAsyncIntersiteTransportObjGuid = uuidAsyncIntersiteTransportObjGuid;
            this.usnLastObjChangeSynced = usnLastObjChangeSynced;
            this.usnAttributeFilter = usnAttributeFilter;
            this.ftimeLastSyncSuccess = ftimeLastSyncSuccess;
            this.ftimeLastSyncAttempt = ftimeLastSyncAttempt;
            this.dwLastSyncResult = dwLastSyncResult;
            this.cNumConsecutiveSyncFailures = cNumConsecutiveSyncFailures;
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public DS_REPL_NEIGHBORW_BLOB()
        {
        }


        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="inputBuffer">A buffer that stored this object instance</param>
        /// <param name="offset">The offset to the start of this instance</param>
        /// <returns>A DS_REPL_NEIGHBORW_BLOB instance</returns>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public static DS_REPL_NEIGHBORW_BLOB FromBytes(byte[] inputBuffer, ref uint offset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            DS_REPL_NEIGHBORW_BLOB instance = new DS_REPL_NEIGHBORW_BLOB();

            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszNamingContext);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszSourceDsaDN);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszSourceDsaAddress);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszAsyncIntersiteTransportDN);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.dwReplicaFlags);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.dwReserved);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidNamingContextObjGuid);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidSourceDsaObjGuid);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidSourceDsaInvocationID);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidAsyncIntersiteTransportObjGuid);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.usnLastObjChangeSynced);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.usnAttributeFilter);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeLastSyncSuccess);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeLastSyncAttempt);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.dwLastSyncResult);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.cNumConsecutiveSyncFailures);
            instance.data = AdtsLdapUtility.SubArrayWithStrings(inputBuffer, ref offset, 4);//4 strings to get

            return instance;

        }


        /// <summary>
        /// Serializer
        /// </summary>
        /// <returns>Byte array containing the octet string </returns>
        public byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays<byte>(
                TypeMarshal.ToBytes(this.oszNamingContext),
                TypeMarshal.ToBytes(this.oszSourceDsaDN),
                TypeMarshal.ToBytes(this.oszSourceDsaAddress),
                TypeMarshal.ToBytes(this.oszAsyncIntersiteTransportDN),
                TypeMarshal.ToBytes(this.dwReplicaFlags),
                TypeMarshal.ToBytes(this.dwReserved),
                TypeMarshal.ToBytes(this.uuidNamingContextObjGuid),
                TypeMarshal.ToBytes(this.uuidSourceDsaObjGuid),
                TypeMarshal.ToBytes(this.uuidSourceDsaInvocationID),
                TypeMarshal.ToBytes(this.uuidAsyncIntersiteTransportObjGuid),
                TypeMarshal.ToBytes(this.usnLastObjChangeSynced),
                TypeMarshal.ToBytes(this.usnAttributeFilter),
                TypeMarshal.ToBytes(this.ftimeLastSyncSuccess),
                TypeMarshal.ToBytes(this.ftimeLastSyncAttempt),
                TypeMarshal.ToBytes(this.dwLastSyncResult),
                TypeMarshal.ToBytes(this.cNumConsecutiveSyncFailures),
                this.data);
        }
    }


    /// <summary>
    /// The DS_REPL_KCC_DSA_FAILUREW_BLOB structure is a representation of a tuple from the kCCFailedConnections or
    /// kCCFailedLinks variables of a DC. This structure, retrieved using an LDAP search method, is an alternative 
    /// representation of DS_REPL_KCC_DSA_FAILUREW, retrieved using the IDL_DRSGetReplInfo RPC method.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class DS_REPL_KCC_DSA_FAILUREW_BLOB
    {
        /// <summary>
        /// A 32-bit offset, in bytes, from the address of this structure to a null-terminated string that contains the
        /// DN of the nTDSDSA object of the source server.
        /// </summary>
        public uint oszDsaDN;

        /// <summary>
        /// A GUID structure, defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the object represented by
        /// the oszDsaDN member.
        /// </summary>
        public Guid uuidDsaObjGuid;

        /// <summary>
        /// A FILETIME structure, the content of which depends on the requested binary replication data.
        /// </summary>
        public _FILETIME ftimeFirstFailure;

        /// <summary>
        /// A 32-bit unsigned integer specifying the number of consecutive failures since the last successful
        /// replication.
        /// </summary>
        public uint cNumFailures;

        /// <summary>
        /// A 32-bit unsigned integer specifying the error code associated with the most recent failure, or
        /// ERROR_SUCCESS if the specific error is unavailable.
        /// </summary>
        public uint dwLastResult;

        /// <summary>
        /// The data field contains the null-terminated string that contains the DN of the nTDSDSAobject of the source
        /// server.
        /// </summary>
        public byte[] data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="szDsaDN">
        /// A 32-bit integer specifying the number of failed replication attempts that have been made from this
        /// source since the last successful replication attempt or since the source was added as a neighbor, if no 
        /// previous attempt succeeded.
        /// </param>
        /// <param name="uuidDsaObjGuid">
        /// A GUID structure, defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the object represented by
        /// the oszDsaDN member.
        /// </param>
        /// <param name="ftimeFirstFailure">
        /// A FILETIME structure, the content of which depends on the requested binary replication data.
        /// </param>
        /// <param name="cNumFailures">
        /// A FILETIME structure, the content of which depends on the requested binary replication data.
        /// </param>
        /// <param name="dwLastResult">
        /// A 32-bit unsigned integer specifying the error code associated with the most recent failure, orERROR_SUCCESS
        /// if the specific error is unavailable.
        /// </param>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public DS_REPL_KCC_DSA_FAILUREW_BLOB(
            string szDsaDN,
            Guid uuidDsaObjGuid,
            _FILETIME ftimeFirstFailure,
            uint cNumFailures,
            uint dwLastResult)
        {
            this.data = ArrayUtility.ConcatenateArrays(
                Encoding.Unicode.GetBytes(szDsaDN), AdtsLdapUtility.nullTerminator);
            this.oszDsaDN = (uint)Marshal.OffsetOf(typeof(DS_REPL_KCC_DSA_FAILUREW_BLOB), AdtsLdapUtility.Data);
            this.uuidDsaObjGuid = uuidDsaObjGuid;
            this.ftimeFirstFailure = ftimeFirstFailure;
            this.cNumFailures = cNumFailures;
            this.dwLastResult = dwLastResult;
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public DS_REPL_KCC_DSA_FAILUREW_BLOB()
        {
        }


        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="inputBuffer">A buffer that contains the DS_REPL_KCC_DSA_FAILUREW_BLOB</param>
        /// <param name="offset">The offset to the start of DS_REPL_KCC_DSA_FAILUREW_BLOB</param>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public static DS_REPL_KCC_DSA_FAILUREW_BLOB FromBytes(byte[] inputBuffer, ref uint offset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            DS_REPL_KCC_DSA_FAILUREW_BLOB instance = new DS_REPL_KCC_DSA_FAILUREW_BLOB();

            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszDsaDN);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidDsaObjGuid);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeFirstFailure);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.cNumFailures);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.dwLastResult);
            instance.data = AdtsLdapUtility.SubArrayWithStrings(inputBuffer, ref offset, 1);//1 string to get

            return instance;
        }


        /// <summary>
        /// Serializer
        /// </summary>
        /// <returns>Byte array containing the octet string </returns>
        public byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays<byte>(
                TypeMarshal.ToBytes(this.oszDsaDN),
                TypeMarshal.ToBytes(this.uuidDsaObjGuid),
                TypeMarshal.ToBytes(this.ftimeFirstFailure),
                TypeMarshal.ToBytes(this.cNumFailures),
                TypeMarshal.ToBytes(this.dwLastResult),
                this.data);
        }
    }


    /// <summary>
    /// The DS_REPL_OPW_BLOB structure is a representation of a tuple from the replicationQueue variable of a DC. This
    /// structure, retrieved using an LDAP search method, is an alternative representation of DS_REPL_OPW, retrieved 
    /// using the IDL_DRSGetReplInfo RPC method.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class DS_REPL_OPW_BLOB
    {
        /// <summary>
        /// A FILETIME structure that contains the date and time that this operation was added to the queue.
        /// </summary>
        public _FILETIME ftimeEnqueued;

        /// <summary>
        /// An unsigned integer specifying the identifier of the operation. The counter used to assign this identifier
        /// is volatile; it is reset during startup of a DC. Therefore, these identifiers are only unique between 
        /// restarts of a DC.
        /// </summary>
        public uint ulSerialNumber;

        /// <summary>
        /// An unsigned integer specifying the priority value of this operation. Tasks with a higher priority value are
        /// executed first. The priority is calculated by the server based on the type of operation and its parameters.
        /// </summary>
        public uint ulPriority;

        /// <summary>
        /// Indicate the type of operation that this structure represents.
        /// </summary>
        public AdtsOpType opType;

        /// <summary>
        /// Zero or more bits from the Directory Replication Service (DRS) options defined in [MS-DRDM] section 5.29,
        /// the interpretation of which depends on the OpType.
        /// </summary>
        public uint ulOptions;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated string that 
        /// contains the DN of the NC associated with this operation 
        /// (for example, the NC to be synchronized for DS_REPL_OP_TYPE_SYNC).
        /// </summary>
        public uint oszNamingContext;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated string that 
        /// contains the DN of the nTDSDSA object of the remote server corresponding to this operation. For example, the
        /// server from which to ask for changes for DS_REPL_OP_TYPE_SYNC. This can be NULL.
        /// </summary>
        public uint oszDsaDN;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated string that
        /// contains the transport-specific network address of the remote server associated with this operation. For 
        /// example, the DNS or SMTP address of the server from which to ask for changes for DS_REPL_OP_TYPE_SYNC. This
        /// can be NULL.
        /// </summary>
        public uint oszDsaAddress;

        /// <summary>
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the NC identified by
        /// oszNamingContext.
        /// </summary>
        public Guid uuidNamingContextObjGuid;

        /// <summary>
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the directory system
        /// agent object identified by oszDsaDN.
        /// </summary>
        public Guid uuidDsaObjGuid;

        /// <summary>
        /// This field contains all the null-terminated strings that are pointed to by the offset fields in the 
        /// structure (oszNamingContext, oszDsaDN, oszDsaAddress). The strings are packed into this field and the 
        /// offsets can be used to determine the start of each string.
        /// </summary>
        public byte[] data;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ftimeEnqueued">
        /// A FILETIME structure that contains the date and time that this operation was added to the queue.
        /// </param>
        /// <param name="ulSerialNumber">
        /// An unsigned integer specifying the identifier of the operation. The counter used to assign this identifier
        /// is volatile; it is reset during startup of a DC. Therefore, these identifiers are only 
        /// unique between restarts of a DC.
        /// </param>
        /// <param name="ulPriority">
        /// An unsigned integer specifying the priority value of this operation. Tasks with a higher priority value are
        /// executed first. The priority is calculated by the server based on the type of operation and its parameters.
        /// </param>
        /// <param name="opType">
        /// Contains one of the OpType Enum values that indicate the type of operation that this structure represents.
        /// </param>
        /// <param name="ulOptions">
        /// Zero or more bits from the Directory Replication Service (DRS) options defined in [MS-DRDM] section 5.29, 
        /// the interpretation of which depends on the OpType.
        /// </param>
        /// <param name="szNamingContext">
        /// A null-terminated string that contains the DN of the NC associated with this operation (for example, the NC 
        /// to be synchronized for DS_REPL_OP_TYPE_SYNC).
        /// </param>
        /// <param name="szDsaDN">
        /// A null-terminated string that contains the DN of the nTDSDSA object of the remote server corresponding to 
        /// this operation. For example, the server from which to ask for changes for DS_REPL_OP_TYPE_SYNC. This can be 
        /// NULL.
        /// </param>
        /// <param name="szDsaAddress">
        /// A null-terminated string that contains the transport-specific network address of the remote server 
        /// associated with this operation. For example, the DNS or SMTP address of the server from which to ask for 
        /// changes for DS_REPL_OP_TYPE_SYNC. This can be NULL.
        /// </param>
        /// <param name="uuidNamingContextObjGuid">
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the NC identified by
        /// oszNamingContext.
        /// </param>
        /// <param name="uuidDsaObjGuid">
        /// A GUID structure, as defined in [MS-DTYP] section 2.3.2, specifying the objectGUID of the directory system 
        /// agent object identified by oszDsaDN.
        /// </param>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public DS_REPL_OPW_BLOB(
            _FILETIME ftimeEnqueued,
            uint ulSerialNumber,
            uint ulPriority,
            AdtsOpType opType,
            uint ulOptions,
            string szNamingContext,
            string szDsaDN,
            string szDsaAddress,
            Guid uuidNamingContextObjGuid,
            Guid uuidDsaObjGuid)
        {
            this.data = ArrayUtility.ConcatenateArrays(
                Encoding.Unicode.GetBytes(szNamingContext), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szDsaDN), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szDsaAddress), AdtsLdapUtility.nullTerminator);
            this.oszNamingContext = (uint)Marshal.OffsetOf(typeof(DS_REPL_OPW_BLOB), AdtsLdapUtility.Data);
            this.oszDsaDN = this.oszNamingContext + (uint)szNamingContext.Length +
                (uint)AdtsLdapUtility.nullTerminator.Length;
            this.oszDsaAddress = this.oszDsaDN + (uint)szDsaDN.Length + (uint)AdtsLdapUtility.nullTerminator.Length;
            this.ftimeEnqueued = ftimeEnqueued;
            this.ulSerialNumber = ulSerialNumber;
            this.ulPriority = ulPriority;
            this.opType = opType;
            this.ulOptions = ulOptions;
            this.uuidNamingContextObjGuid = uuidNamingContextObjGuid;
            this.uuidDsaObjGuid = uuidDsaObjGuid;
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public DS_REPL_OPW_BLOB()
        {
        }


        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="inputBuffer">A buffer that contains the DS_REPL_OPW_BLOB</param>
        /// <param name="offset">The offset to the start of DS_REPL_OPW_BLOB</param>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public static DS_REPL_OPW_BLOB FromBytes(byte[] inputBuffer, ref uint offset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            DS_REPL_OPW_BLOB instance = new DS_REPL_OPW_BLOB();

            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeEnqueued);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ulSerialNumber);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ulPriority);
            uint temp;
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out temp);
            instance.opType = (AdtsOpType)temp;
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ulOptions);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszNamingContext);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszDsaDN);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszDsaAddress);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidNamingContextObjGuid);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidDsaObjGuid);
            instance.data = AdtsLdapUtility.SubArrayWithStrings(inputBuffer, ref offset, 3);//3 strings to get

            return instance;
        }


        /// <summary>
        /// Serializer
        /// </summary>
        /// <returns>Byte array containing the octet string </returns>
        public byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.ftimeEnqueued),
                TypeMarshal.ToBytes(this.ulSerialNumber),
                TypeMarshal.ToBytes(this.ulPriority),
                TypeMarshal.ToBytes((uint)this.opType),
                TypeMarshal.ToBytes(this.ulOptions),
                TypeMarshal.ToBytes(this.oszNamingContext),
                TypeMarshal.ToBytes(this.oszDsaDN),
                TypeMarshal.ToBytes(this.oszDsaAddress),
                TypeMarshal.ToBytes(this.uuidNamingContextObjGuid),
                TypeMarshal.ToBytes(this.uuidDsaObjGuid),
                this.data);
        }
    }


    /// <summary>
    /// The DS_REPL_QUEUE_STATISTICSW_BLOB structure contains the statistics related to the replicationQueue variable of
    /// a DC, returned by reading the msDS-ReplQueueStatistics rootDSE attribute (section 3.1.1.3.2.30).
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class DS_REPL_QUEUE_STATISTICSW_BLOB
    {
        /// <summary>
        /// A FILETIME structure that contains the date and time that the currently running operation started.
        /// </summary>
        public _FILETIME ftimeCurrentOpStarted;

        /// <summary>
        /// A unsigned integer specifying the number of currently pending operations.
        /// </summary>
        public uint cNumPendingOps;

        /// <summary>
        /// A FILETIME structure that contains the date and time of the oldest synchronization operation.
        /// </summary>
        public _FILETIME ftimeOldestSync;

        /// <summary>
        /// A FILETIME structure that contains the date and time of the oldest add operation.
        /// </summary>
        public _FILETIME ftimeOldestAdd;

        /// <summary>
        /// A FILETIME structure that contains the date and time of the oldest modification operation.
        /// </summary>
        public _FILETIME ftimeOldestMod;

        /// <summary>
        /// A FILETIME structure that contains the date and time of the oldest delete operation.
        /// </summary>
        public _FILETIME ftimeOldestDel;

        /// <summary>
        /// A FILETIME structure that contains the date and time of the oldest reference update operation.
        /// </summary>
        public _FILETIME ftimeOldestUpdRefs;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ftimeCurrentOpStarted">
        /// A FILETIME structure that contains the date and time that the currently running operation started.
        /// </param>
        /// <param name="cNumPendingOps">
        /// A unsigned integer specifying the number of currently pending operations.
        /// </param>
        /// <param name="ftimeOldestSync">
        /// A FILETIME structure that contains the date and time of the oldest synchronization operation.
        /// </param>
        /// <param name="ftimeOldestAdd">
        /// A FILETIME structure that contains the date and time of the oldest add operation.
        /// </param>
        /// <param name="ftimeOldestMod">
        /// A FILETIME structure that contains the date and time of the oldest modification operation.
        /// </param>
        /// <param name="ftimeOldestDel">
        /// A FILETIME structure that contains the date and time of the oldest delete operation.
        /// </param>
        /// <param name="ftimeOldestUpdRefs">
        /// A FILETIME structure that contains the date and time of the oldest reference update operation.
        /// </param>
        public DS_REPL_QUEUE_STATISTICSW_BLOB(
            _FILETIME ftimeCurrentOpStarted,
            uint cNumPendingOps,
            _FILETIME ftimeOldestSync,
            _FILETIME ftimeOldestAdd,
            _FILETIME ftimeOldestMod,
            _FILETIME ftimeOldestDel,
            _FILETIME ftimeOldestUpdRefs
            )
        {
            this.ftimeCurrentOpStarted = ftimeCurrentOpStarted;
            this.cNumPendingOps = cNumPendingOps;
            this.ftimeOldestSync = ftimeOldestSync;
            this.ftimeOldestAdd = ftimeOldestAdd;
            this.ftimeOldestMod = ftimeOldestMod;
            this.ftimeOldestDel = ftimeOldestDel;
            this.ftimeOldestUpdRefs = ftimeOldestUpdRefs;
        }


        /// <summary>
        /// Defalut constructor
        /// </summary>
        public DS_REPL_QUEUE_STATISTICSW_BLOB()
        {

        }


        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="inputBuffer">A buffer that contains the DS_REPL_QUEUE_STATISTICSW_BLOB</param>
        /// <param name="offset">The offset to the start of DS_REPL_QUEUE_STATISTICSW_BLOB</param>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public static DS_REPL_QUEUE_STATISTICSW_BLOB FromBytes(byte[] inputBuffer, ref uint offset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            DS_REPL_QUEUE_STATISTICSW_BLOB instance = new DS_REPL_QUEUE_STATISTICSW_BLOB();

            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeCurrentOpStarted);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.cNumPendingOps);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeOldestSync);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeOldestAdd);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeOldestMod);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeOldestDel);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeOldestUpdRefs);

            return instance;
        }


        /// <summary>
        /// Serializer
        /// </summary>
        /// <returns>Byte array containing the octet string </returns>
        public byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.ftimeCurrentOpStarted),
                TypeMarshal.ToBytes<uint>(this.cNumPendingOps),
                TypeMarshal.ToBytes(this.ftimeOldestSync),
                TypeMarshal.ToBytes(this.ftimeOldestAdd),
                TypeMarshal.ToBytes(this.ftimeOldestMod),
                TypeMarshal.ToBytes(this.ftimeOldestDel),
                TypeMarshal.ToBytes(this.ftimeOldestUpdRefs));
        }
    }


    /// <summary>
    /// The DS_REPL_CURSOR_BLOB is the packet representation of the ReplUpToDateVector type ([MS-DRDM] section 5.108) of
    /// an NC replica. This structure, retrieved using an LDAP search method, is an alternative representation of 
    /// DS_REPL_CURSOR_3W, retrieved using the IDL_DRSGetReplInfo RPC method.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class DS_REPL_CURSOR_BLOB
    {
        /// <summary>
        /// A GUID structure, defined in [MS-DTYP] section 2.3.2, specifying the Invocation ID of the originating server
        /// to which the usnAttributeFilter corresponds.
        /// </summary>
        public Guid uuidSourceDsaInvocationID;

        /// <summary>
        /// A USN value, as defined in section 3.1.1.1.9, containing the maximum USN to which the destination server can
        /// indicate that it has recorded all changes originated by the given server at USNs less than or equal to this
        /// USN. This is used to filter changes at replication source servers that the destination server has already
        /// applied.
        /// </summary>
        public long usnAttributeFilter;

        /// <summary>
        /// A FILETIME structure that contains the date and time of the last successful synchronization operation.
        /// </summary>
        public _FILETIME fTimeLastSyncSuccess;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string.
        /// The string contains the distinguished name of the directory service agent (DSA) that corresponds to the 
        /// source server to which this replication state data applies.
        /// </summary>
        public uint oszSourceDsaDN;

        public byte[] data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uuidSourceDsaInvocationID">
        /// A GUID structure, defined in [MS-DTYP] section 2.3.2, specifying the Invocation ID of the originating
        /// server to which the usnAttributeFilter corresponds.
        /// </param>
        /// <param name="usnAttributeFilter">
        /// A USN value, as defined in section 3.1.1.1.9, containing the maximum USN to which the destination server can
        /// indicate that it has recorded all changes originated by the given server at USNs less than or equal to this 
        /// USN. This is used to filter changes at replication source servers that the destination server has already 
        /// applied.
        /// </param>
        /// <param name="fTimeLastSyncSuccess">
        /// A USN value, as defined in section 3.1.1.1.9, containing the maximum USN to which the destination server can
        /// indicate that it has recorded all changes originated by the given server at USNs less than or equal to this 
        /// USN. This is used to filter changes at replication source servers that the destination server has already 
        /// applied.
        /// </param>
        /// <param name="szSourceDsaDN">
        /// A null-terminated Unicode string. The string contains the distinguished name of the directory service agent
        /// (DSA) that corresponds to the source server to which this replication state data applies.
        /// </param>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public DS_REPL_CURSOR_BLOB(
            Guid uuidSourceDsaInvocationID,
            long usnAttributeFilter,
            _FILETIME fTimeLastSyncSuccess,
            string szSourceDsaDN)
        {
            this.data = ArrayUtility.ConcatenateArrays(
                Encoding.Unicode.GetBytes(szSourceDsaDN),
                AdtsLdapUtility.nullTerminator);
            this.oszSourceDsaDN = (uint)Marshal.OffsetOf(typeof(DS_REPL_CURSOR_BLOB), AdtsLdapUtility.Data);
            this.uuidSourceDsaInvocationID = uuidSourceDsaInvocationID;
            this.usnAttributeFilter = usnAttributeFilter;
            this.fTimeLastSyncSuccess = fTimeLastSyncSuccess;
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public DS_REPL_CURSOR_BLOB()
        {

        }


        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="inputBuffer">A buffer that contains the DS_REPL_CURSOR_BLOB</param>
        /// <param name="offset">The offset to the start of DS_REPL_CURSOR_BLOB</param>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public static DS_REPL_CURSOR_BLOB FromBytes(byte[] inputBuffer, ref uint offset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            DS_REPL_CURSOR_BLOB instance = new DS_REPL_CURSOR_BLOB();

            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidSourceDsaInvocationID);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.usnAttributeFilter);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.fTimeLastSyncSuccess);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszSourceDsaDN);
            instance.data = AdtsLdapUtility.SubArrayWithStrings(inputBuffer, ref offset, 1);//1 string to get

            return instance;
        }


        /// <summary>
        /// Serializer
        /// </summary>
        /// <returns>Byte array containing the octet string </returns>
        public byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.uuidSourceDsaInvocationID),
                TypeMarshal.ToBytes(this.usnAttributeFilter),
                TypeMarshal.ToBytes(this.fTimeLastSyncSuccess),
                TypeMarshal.ToBytes(this.oszSourceDsaDN),
                this.data);
        }
    }


    /// <summary>
    /// The DS_REPL_ATTR_META_DATA_BLOB packet is a representation of a stamp variable (of type AttributeStamp) of an 
    /// attribute. This structure, retrieved using an LDAP search method, is an alternative representation of 
    /// DS_REPL_ATTR_META_DATA_2, retrieved using theIDL_DRSGetReplInfo RPC method.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class DS_REPL_ATTR_META_DATA_BLOB
    {
        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string
        /// that contains the LDAP display name of the attribute corresponding to this metadata.
        /// </summary>
        public uint oszAttributeName;

        /// <summary>
        /// Contains the dwVersion of this attribute's AttributeStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public uint dwVersion;

        /// <summary>
        /// Contains the timeChanged of this attribute's AttributeStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public _FILETIME ftimeLastOriginatingChange;

        /// <summary>
        /// Contains the uuidOriginating of this attribute's AttributeStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public Guid uuidLastOriginatingDsaInvocationID;

        /// <summary>
        /// Contains the usnOriginating of this attribute's AttributeStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public long usnOriginatingChange;

        /// <summary>
        /// A USN value, defined in section 3.1.1.1.9, specifying the USN on the destination server (the server from 
        /// which the metadata information is retrieved) at which the last change to this attribute was applied. This 
        /// value typically is different on all servers.
        /// </summary>
        public long usnLocalChange;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string 
        /// that contains the DN of the nTDSDSA object of the server that originated the last replication.
        /// </summary>
        public uint oszLastOriginatingDsaDN;

        /// <summary>
        /// This field contains all the null-terminated strings that are pointed to by the offset fields in the 
        /// structure (oszAttributeName, oszLastOriginatingDsaDN). The strings are packed into this field, and the 
        /// offsets can be used to determine the start of each string.
        /// </summary>
        public byte[] data;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="szAttributeName">
        /// A null-terminated Unicode string that contains the LDAP display name of the attribute corresponding 
        /// to this metadata.
        /// </param>
        /// <param name="dwVersion">
        /// Contains the dwVersion of this attribute's AttributeStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="ftimeLastOriginatingChange">
        /// Contains the timeChanged of this attribute's AttributeStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="uuidLastOriginatingDsaInvocationID">
        /// Contains the uuidOriginating of this attribute's AttributeStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="usnOriginatingChange">
        /// Contains the usnOriginating of this attribute's AttributeStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="usnLocalChange">
        /// A USN value, defined in section 3.1.1.1.9, specifying the USN on the destination server (the server from 
        /// which the metadata information is retrieved) at which the last change to this attribute was applied. This 
        /// value typically is different on all servers.
        /// </param>
        /// <param name="szLastOriginatingDsaDN">
        /// A null-terminated Unicode string that contains the DN of the nTDSDSA object of the server that originated 
        /// the last replication.
        /// </param>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public DS_REPL_ATTR_META_DATA_BLOB(
            string szAttributeName,
            uint dwVersion,
            _FILETIME ftimeLastOriginatingChange,
            Guid uuidLastOriginatingDsaInvocationID,
            long usnOriginatingChange,
            long usnLocalChange,
            string szLastOriginatingDsaDN)
        {
            this.data = ArrayUtility.ConcatenateArrays(
                Encoding.Unicode.GetBytes(szAttributeName), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szLastOriginatingDsaDN), AdtsLdapUtility.nullTerminator);
            this.oszAttributeName = (uint)Marshal.OffsetOf(typeof(DS_REPL_ATTR_META_DATA_BLOB), AdtsLdapUtility.Data);
            this.oszLastOriginatingDsaDN = this.oszAttributeName + (uint)szAttributeName.Length +
                (uint)AdtsLdapUtility.nullTerminator.Length;
            this.dwVersion = dwVersion;
            this.ftimeLastOriginatingChange = ftimeLastOriginatingChange;
            this.uuidLastOriginatingDsaInvocationID = uuidLastOriginatingDsaInvocationID;
            this.usnOriginatingChange = usnOriginatingChange;
            this.usnLocalChange = usnLocalChange;
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public DS_REPL_ATTR_META_DATA_BLOB()
        {

        }


        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="inputBuffer">A buffer that contains the DS_REPL_ATTR_META_DATA_BLOB</param>
        /// <param name="offset">The offset to the start of DS_REPL_ATTR_META_DATA_BLOB</param>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public static DS_REPL_ATTR_META_DATA_BLOB FromBytes(byte[] inputBuffer, ref uint offset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            DS_REPL_ATTR_META_DATA_BLOB instance = new DS_REPL_ATTR_META_DATA_BLOB();

            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszAttributeName);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.dwVersion);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeLastOriginatingChange);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidLastOriginatingDsaInvocationID);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.usnOriginatingChange);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.usnLocalChange);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszLastOriginatingDsaDN);
            instance.data = AdtsLdapUtility.SubArrayWithStrings(inputBuffer, ref offset, 2);//2 strings to get

            return instance;
        }


        /// <summary>
        /// Serializer
        /// </summary>
        /// <returns>Byte array containing the octet string </returns>
        public byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.oszAttributeName),
                TypeMarshal.ToBytes(this.dwVersion),
                TypeMarshal.ToBytes(this.ftimeLastOriginatingChange),
                TypeMarshal.ToBytes(this.uuidLastOriginatingDsaInvocationID),
                TypeMarshal.ToBytes(this.usnOriginatingChange),
                TypeMarshal.ToBytes(this.usnLocalChange),
                TypeMarshal.ToBytes(this.oszLastOriginatingDsaDN),
                this.data);
        }
    }


    /// <summary>
    /// The DS_REPL_VALUE_META_DATA_BLOB packet is a representation of a stamp variable (of type LinkValueStamp) of a 
    /// link value. This structure, retrieved using an LDAP search method, is an alternative representation of 
    /// DS_REPL_VALUE_META_DATA_2, retrieved using the IDL_DRSGetReplInfo RPC method.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class DS_REPL_VALUE_META_DATA_BLOB
    {
        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string
        /// that contains the LDAP display name of the attribute corresponding to this metadata.
        /// </summary>
        public uint oszAttributeName;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string
        /// that contains the DN of the object that this attribute belongs to.
        /// </summary>
        public uint oszObjectDn;

        /// <summary>
        /// Contains the number of bytes in the pbData array.
        /// </summary>
        public uint cbData;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to an octet that contains the
        /// attribute replication metadata. The cbData member contains the length, in bytes, of this octet.
        /// </summary>
        public uint pbData;

        /// <summary>
        /// Contains the timeDeleted of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </summary>
        public _FILETIME ftimeDeleted;

        /// <summary>
        /// Contains the timeCreated of this link value's LinkValueStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public _FILETIME ftimeCreated;

        /// <summary>
        /// Contains the dwVersion of this link value's LinkValueStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public uint dwVersion;

        /// <summary>
        /// Contains the timeChanged of this link value's LinkValueStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public _FILETIME ftimeLastOriginatingChange;

        /// <summary>
        /// Contains the uuidOriginating of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </summary>
        public Guid uuidLastOriginatingDsaInvocationID;

        /// <summary>
        /// Contains the usnOriginating of this link value's LinkValueStamp, as specified in section3.1.1.1.9.
        /// </summary>
        public long usnOriginatingChange;

        /// <summary>
        /// Specifies the USN, as found on the server from which the metadata information is being retrieved, at which 
        /// the last change to this attribute was applied. This value is typically different on all servers.
        /// </summary>
        public long usnLocalChange;

        /// <summary>
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode string 
        /// that contains the DN of the nTDSDSA object of the server that originated the last replication.
        /// </summary>
        public uint oszLastOriginatingDsaDN;

        /// <summary>
        /// This field contains all the null-terminated strings that are pointed to by the offset fields in the 
        /// structure (oszAttributeName, oszObjectDn, oszLastOriginatingDsaDN) and the octet pointed to by pbData. The
        /// strings and octets are packed into this field (aligned at 32-bit boundaries), and the offsets can be used
        /// to determine the start of each string.
        /// </summary>
        public byte[] data;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="szAttributeName">
        /// A null-terminated Unicode string that contains the LDAP display name of the attribute corresponding to this
        /// metadata.
        /// </param>
        /// <param name="szObjectDn">
        /// A null-terminated Unicode string that contains the DN of the object that this attribute belongs to.
        /// </param>
        /// <param name="cbData">Contains the number of bytes in the pbData array.</param>
        /// <param name="pbData">
        /// Contains a 32-bit offset, in bytes, from the address of this structure to an octet that contains the
        /// attribute replication metadata. The cbData member contains the length, in bytes, of this octet.
        /// </param>
        /// <param name="ftimeDeleted">
        /// Contains the timeDeleted of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="ftimeCreated">
        /// Contains the timeCreated of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="dwVersion">
        /// Contains the timeCreated of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="ftimeLastOriginatingChange">
        /// Contains the timeChanged of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="uuidLastOriginatingDsaInvocationID">
        /// Contains the uuidOriginating of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="usnOriginatingChange">
        /// Contains the usnOriginating of this link value's LinkValueStamp, as specified in section 3.1.1.1.9.
        /// </param>
        /// <param name="usnLocalChange">
        /// Specifies the USN, as found on the server from which the metadata information is being retrieved, at
        /// which the last change to this attribute was applied. This value is typically different on all servers.
        /// </param>
        /// <param name="szLastOriginatingDsaDN">
        /// Contains a 32-bit offset, in bytes, from the address of this structure to a null-terminated Unicode
        /// string that contains the DN of the nTDSDSA object of the server that originated the last replication.
        /// </param>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public DS_REPL_VALUE_META_DATA_BLOB(
            string szAttributeName,
            string szObjectDn,
            uint cbData,
            byte[] bData,
            _FILETIME ftimeDeleted,
            _FILETIME ftimeCreated,
            uint dwVersion,
            _FILETIME ftimeLastOriginatingChange,
            Guid uuidLastOriginatingDsaInvocationID,
            long usnOriginatingChange,
            long usnLocalChange,
            string szLastOriginatingDsaDN)
        {
            this.data = ArrayUtility.ConcatenateArrays(
                Encoding.Unicode.GetBytes(szAttributeName), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szObjectDn), AdtsLdapUtility.nullTerminator,
                Encoding.Unicode.GetBytes(szLastOriginatingDsaDN), AdtsLdapUtility.nullTerminator,
                bData);
            this.oszAttributeName = (uint)Marshal.OffsetOf(typeof(DS_REPL_VALUE_META_DATA_BLOB), AdtsLdapUtility.Data);
            this.oszObjectDn = this.oszAttributeName + (uint)szAttributeName.Length +
                (uint)AdtsLdapUtility.nullTerminator.Length;
            this.oszLastOriginatingDsaDN = this.oszObjectDn + (uint)szObjectDn.Length +
                (uint)AdtsLdapUtility.nullTerminator.Length;
            this.cbData = cbData;
            this.ftimeDeleted = ftimeDeleted;
            this.ftimeCreated = ftimeCreated;
            this.dwVersion = dwVersion;
            this.ftimeLastOriginatingChange = ftimeLastOriginatingChange;
            this.uuidLastOriginatingDsaInvocationID = uuidLastOriginatingDsaInvocationID;
            this.usnOriginatingChange = usnOriginatingChange;
            this.usnLocalChange = usnLocalChange;
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        public DS_REPL_VALUE_META_DATA_BLOB()
        {

        }


        /// <summary>
        /// Deserializer
        /// </summary>
        /// <param name="inputBuffer">A buffer that contains the DS_REPL_VALUE_META_DATA_BLOB</param>
        /// <param name="offset">The offset to the start of DS_REPL_VALUE_META_DATA_BLOB</param>
        /// <exception cref="System.ArgumentNullException">thrown if inputBuffer is null</exception>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        public static DS_REPL_VALUE_META_DATA_BLOB FromBytes(byte[] inputBuffer, ref uint offset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            DS_REPL_VALUE_META_DATA_BLOB instance = new DS_REPL_VALUE_META_DATA_BLOB();

            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszAttributeName);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszObjectDn);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.cbData);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.pbData);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeDeleted);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeCreated);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.dwVersion);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.ftimeLastOriginatingChange);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.uuidLastOriginatingDsaInvocationID);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.usnOriginatingChange);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.usnLocalChange);
            AdtsLdapUtility.BytesToObject(inputBuffer, ref offset, out instance.oszLastOriginatingDsaDN);
            instance.data = ArrayUtility.ConcatenateArrays(
                AdtsLdapUtility.SubArrayWithStrings(inputBuffer, ref offset, 3),
                ArrayUtility.SubArray(inputBuffer, (int)offset, (int)instance.cbData));
            offset += instance.cbData;

            return instance;
        }


        /// <summary>
        /// Serializer
        /// </summary>
        /// <returns>Byte array containing the octet string </returns>
        public byte[] ToBytes()
        {
            return ArrayUtility.ConcatenateArrays(
                TypeMarshal.ToBytes(this.oszAttributeName),
                TypeMarshal.ToBytes(this.oszObjectDn),
                TypeMarshal.ToBytes(this.cbData),
                TypeMarshal.ToBytes(this.pbData),
                TypeMarshal.ToBytes(this.ftimeDeleted),
                TypeMarshal.ToBytes(this.ftimeCreated),
                TypeMarshal.ToBytes(this.dwVersion),
                TypeMarshal.ToBytes(this.ftimeLastOriginatingChange),
                TypeMarshal.ToBytes(this.uuidLastOriginatingDsaInvocationID),
                TypeMarshal.ToBytes(this.usnOriginatingChange),
                TypeMarshal.ToBytes(this.usnLocalChange),
                TypeMarshal.ToBytes(this.oszLastOriginatingDsaDN),
                this.data);
        }
    }


    /// <summary>
    /// The well-known Guids
    /// </summary>
    public class AdtsWellKnownGuid
    {
        /// <summary>
        /// Computers object
        /// </summary>
        public static readonly Guid GUID_COMPUTERS_CONTAINER_W = new Guid(
            new byte[] { 0xAA, 0x31, 0x28, 0x25, 0x76, 0x88, 0x11, 0xD1, 0xAD, 0xED, 0x00, 0xC0, 0x4F, 0xD8, 0xD5, 0xCD });

        /// <summary>
        /// Deleted Objects object
        /// </summary>
        public static readonly Guid GUID_DELETED_OBJECTS_CONTAINER_W = new Guid(
            new byte[] { 0x18, 0xE2, 0xEA, 0x80, 0x68, 0x4F, 0x11, 0xD2, 0xB9, 0xAA, 0x00, 0xC0, 0x4F, 0x79, 0xF8, 0x05 });

        /// <summary>
        /// Domain Controllers object
        /// </summary>
        public static readonly Guid GUID_DOMAIN_CONTROLLERS_CONTAINER_W = new Guid(
            new byte[] { 0xA3, 0x61, 0xB2, 0xFF, 0xFF, 0xD2, 0x11, 0xD1, 0xAA, 0x4B, 0x00, 0xC0, 0x4F, 0xD7, 0xD8, 0x3A });

        /// <summary>
        /// ForeignSecurityPrincipals object
        /// </summary>
        public static readonly Guid GUID_FOREIGNSECURITYPRINCIPALS_CONTAINER_W = new Guid(
            new byte[] { 0x22, 0xB7, 0x0C, 0x67, 0xD5, 0x6E, 0x4E, 0xFB, 0x91, 0xE9, 0x30, 0x0F, 0xCA, 0x3D, 0xC1, 0xAA });

        /// <summary>
        /// Infrastructure object
        /// </summary>
        public static readonly Guid GUID_INFRASTRUCTURE_CONTAINER_W = new Guid(
            new byte[] { 0x2F, 0xBA, 0xC1, 0x87, 0x0A, 0xDE, 0x11, 0xD2, 0x97, 0xC4, 0x00, 0xC0, 0x4F, 0xD8, 0xD5, 0xCD });

        /// <summary>
        /// LostAndFound object
        /// </summary>
        public static readonly Guid GUID_LOSTANDFOUND_CONTAINER_W = new Guid(
            new byte[] { 0xAB, 0x81, 0x53, 0xB7, 0x76, 0x88, 0x11, 0xD1, 0xAD, 0xED, 0x00, 0xC0, 0x4F, 0xD8, 0xD5, 0xCD });

        /// <summary>
        /// MicrosoftNote 1 object
        /// </summary>
        public static readonly Guid GUID_MICROSOFT_PROGRAM_DATA_CONTAINER_W = new Guid(
            new byte[] { 0xF4, 0xBE, 0x92, 0xA4, 0xC7, 0x77, 0x48, 0x5E, 0x87, 0x8E, 0x94, 0x21, 0xD5, 0x30, 0x87, 0xDB });

        /// <summary>
        /// NTDS Quotas object
        /// </summary>
        public static readonly Guid GUID_NTDS_QUOTAS_CONTAINER_W = new Guid(
            new byte[] { 0x62, 0x27, 0xF0, 0xAF, 0x1F, 0xC2, 0x41, 0x0D, 0x8E, 0x3B, 0xB1, 0x06, 0x15, 0xBB, 0x5B, 0x0F });

        /// <summary>
        /// Program Data object
        /// </summary>
        public static readonly Guid GUID_PROGRAM_DATA_CONTAINER_W = new Guid(
            new byte[] { 0x09, 0x46, 0x0C, 0x08, 0xAE, 0x1E, 0x4A, 0x4E, 0xA0, 0xF6, 0x4A, 0xEE, 0x7D, 0xAA, 0x1E, 0x5A });

        /// <summary>
        /// System object
        /// </summary>
        public static readonly Guid GUID_SYSTEMS_CONTAINER_W = new Guid(
            new byte[] { 0xAB, 0x1D, 0x30, 0xF3, 0x76, 0x88, 0x11, 0xD1, 0xAD, 0xED, 0x00, 0xC0, 0x4F, 0xD8, 0xD5, 0xCD });

        /// <summary>
        /// Users object
        /// </summary>
        public static readonly Guid GUID_USERS_CONTAINER_W = new Guid(
            new byte[] { 0xA9, 0xD1, 0xCA, 0x15, 0x76, 0x88, 0x11, 0xD1, 0xAD, 0xED, 0x00, 0xC0, 0x4F, 0xD8, 0xD5, 0xCD });

        /// <summary>
        /// Managed Service Accounts object
        /// </summary>
        public static readonly Guid GUID_MANAGED_SERVICE_ACCOUNTS_CONTAINER_W = new Guid(
            new byte[] { 0x1E, 0xB9, 0x38, 0x89, 0xE4, 0x0C, 0x45, 0xDF, 0x9F, 0x0C, 0x64, 0xD2, 0x3B, 0xBB, 0x62, 0x37 });
    }


    /// <summary>
    /// An enum for the member opType of DS_REPL_OPW_BLOB
    /// </summary>
    public enum AdtsOpType
    {
        /// <summary>
        /// sync
        /// </summary>
        DS_REPL_OP_TYPE_SYNC = 0,

        /// <summary>
        /// add
        /// </summary>
        DS_REPL_OP_TYPE_ADD = 1,

        /// <summary>
        /// delete
        /// </summary>
        DS_REPL_OP_TYPE_DELETE = 2,

        /// <summary>
        /// modify
        /// </summary>
        DS_REPL_OP_TYPE_MODIFY = 3,

        /// <summary>
        /// update refs
        /// </summary>
        DS_REPL_OP_TYPE_UPDATE_REFS = 4
    }


    /// <summary>
    /// Specify the NtVersion
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NETLOGON_NT_VERSION : uint
    {
        /// <summary>
        /// No bit is set
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Unless overridden by V5, V5EX, or V5EP, this bit instructs the server to respond to LDAP ping (section 7.3.3)
        /// and mailslot ping (section 7.3.5) using either the NETLOGON_SAM_LOGON_RESPONSE_NT40 structure or the
        /// NETLOGON_PRIMARY_RESPONSE structure for the PDC.
        /// </summary>
        NETLOGON_NT_VERSION_1 = 0x00000001,

        /// <summary>
        /// Unless overridden by V5EX or V5EP, this bit instructs the server to respond to LDAP ping and mailslot ping
        /// using the NETLOGON_SAM_LOGON_RESPONSE structure.
        /// </summary>
        NETLOGON_NT_VERSION_5 = 0x00000002,

        /// <summary>
        /// Unless overridden by V5EP, this bit instructs the server to respond to LDAP ping and mailslot ping using the
        /// NETLOGON_SAM_LOGON_RESPONSE_EX structure.
        /// </summary>
        NETLOGON_NT_VERSION_5EX = 0x00000004,

        /// <summary>
        /// Instructs the server to respond to mailslot ping using the NETLOGON_SAM_LOGON_RESPONSE_EX structure and also
        /// to return the IP address of the server in the response.
        /// </summary>
        NETLOGON_NT_VERSION_5EX_WITH_IP = 0x00000008,

        /// <summary>
        /// Indicates that the client is querying for the closest site information.
        /// </summary>
        NETLOGON_NT_VERSION_WITH_CLOSEST_SITE = 0x00000010,

        /// <summary>
        /// Forces the server to respond to an LDAP ping and to honor all the NetLOGON_NT_VERSION options that the client
        /// specifies in the LDAP ping or mailslot ping.
        /// </summary>
        NETLOGON_NT_VERSION_AVOID_NT4EMUL = 0x01000000,

        /// <summary>
        /// Indicates that the client is querying for a PDC.
        /// </summary>
        NETLOGON_NT_VERSION_PDC = 0x10000000,

        /// <summary>
        /// Obsolete, ignored.
        /// </summary>
        NETLOGON_NT_VERSION_IP = 0x20000000,

        /// <summary>
        /// Indicates that the client is the local machine.
        /// </summary>
        NETLOGON_NT_VERSION_LOCAL = 0x40000000,

        /// <summary>
        /// Indicates that the client is querying for a GC.
        /// </summary>
        NETLOGON_NT_VERSION_GC = 0x80000000
    }


    /// <summary>
    /// DS_FLAG Options Bits
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum DS_FLAG : uint
    {
        /// <summary>
        /// No bit is set
        /// </summary>
        NONE = 0,

        /// <summary>
        /// The server holds the PDC FSMO role (PdcEmulationMasterRole).
        /// </summary>
        DS_PDC_FLAG = 0x00000001,

        /// <summary>
        /// The server is a GC server and will accept and process messages directed to it on the global catalog ports
        /// (see section 3.1.1.3.1.10).
        /// </summary>
        DS_GC_FLAG = 0x00000004,

        /// <summary>
        /// The server is an LDAP server.
        /// </summary>
        DS_LDAP_FLAG = 0x00000008,

        /// <summary>
        /// The server is a DC.
        /// </summary>
        DS_DS_FLAG = 0x00000010,

        /// <summary>
        /// The server is running the Kerberos Key Distribution Center service.
        /// </summary>
        DS_KDC_FLAG = 0x00000020,

        /// <summary>
        /// The Win32 Time Service, as specified in [MS-W32T], is present on the server.
        /// </summary>
        DS_TIMESERV_FLAG = 0x00000040,

        /// <summary>
        /// The server is in the same site as the client.
        /// </summary>
        DS_CLOSEST_FLAG = 0x00000080,

        /// <summary>
        /// Indicates that the server is not an RODC.
        /// </summary>
        DS_WRITABLE_FLAG = 0x00000100,

        /// <summary>
        /// The server is a reliable time server.
        /// </summary>
        DS_GOOD_TIMESERV_FLAG = 0x00000200,

        /// <summary>
        /// The NC is an application NC.
        /// </summary>
        DS_NDNC_FLAG = 0x00000400,

        /// <summary>
        /// The server is an RODC.
        /// </summary>
        DS_SELECT_SECRET_DOMAIN_6_FLAG = 0x00000800,

        /// <summary>
        /// The server is a writable DC, not running Microsoft Windows® 2000 Server operating system or Windows
        /// Server® 2003 operating system.
        /// </summary>
        DS_FULL_SECRET_DOMAIN_6_FLAG = 0x00001000,

        /// <summary>
        /// The Active Directory Web Service, as specified in [MS-ADDM], is present on the server.
        /// </summary>
        DS_WS_FLAG = 0x00002000,

        /// <summary>
        /// The server is not running Microsoft Windows 2000 operating system, Windows Server 2003,
        /// Windows Server 2008 operating system, Windows Server 2008 R2 operating system, or Windows Server 2012 operating system.
        /// </summary>
        DS_DS_8_FLAG = 0x00004000,

        /// <summary>
        /// The server has a DNS name.
        /// </summary>
        DS_DNS_CONTROLLER_FLAG = 0x20000000,

        /// <summary>
        /// The NC is a default NC.
        /// </summary>
        DS_DNS_DOMAIN_FLAG = 0x40000000,

        /// <summary>
        /// The NC is the forest root.
        /// </summary>
        DS_DNS_FOREST_FLAG = 0x80000000,
    }


    /// <summary>
    /// Operation code set in the request and response of an LDAP ping (section 7.3.3) or a mailslot ping (section 7.3.5)
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum AdtsOperationCode
    {
        /// <summary>
        /// NETLOGON_LOGON_QUERY
        /// </summary>
        LOGON_PRIMARY_QUERY = 7,

        /// <summary>
        /// NETLOGON_PRIMARY_RESPONSE
        /// </summary>
        LOGON_PRIMARY_RESPONSE = 12,

        /// <summary>
        /// NETLOGON_SAM_LOGON_REQUEST
        /// </summary>
        LOGON_SAM_LOGON_REQUEST = 18,

        /// <summary>
        /// Associated to NETLOGON_SAM_LOGON_RESPONSE
        /// </summary>
        LOGON_SAM_LOGON_RESPONSE = 19,

        /// <summary>
        /// Associated to NETLOGON_SAM_LOGON_RESPONSE
        /// </summary>
        LOGON_SAM_PAUSE_RESPONSE = 20,

        /// <summary>
        /// Associated to NETLOGON_SAM_LOGON_RESPONSE
        /// </summary>
        LOGON_SAM_USER_UNKNOWN = 21,

        /// <summary>
        /// Associated to NETLOGON_SAM_LOGON_RESPONSE_EX
        /// </summary>
        LOGON_SAM_LOGON_RESPONSE_EX = 23,

        /// <summary>
        /// Associated to NETLOGON_SAM_LOGON_RESPONSE
        /// </summary>
        LOGON_SAM_PAUSE_RESPONSE_EX = 24,

        /// <summary>
        /// Associated to NETLOGON_SAM_LOGON_RESPONSE
        /// </summary>
        LOGON_SAM_USER_UNKNOWN_EX = 25,
    }


    /// <summary>
    /// Bit flags describing various qualities of a security account.
    /// </summary>
    [Flags]
    public enum AdtsUserAccountControl
    {
        /// <summary>
        /// No bit is set
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Specifies that the account is not enabled for authentication.
        /// </summary>
        ADS_UF_ACCOUNT_DISABLE = 0x00000002,

        /// <summary>
        /// Specifies that the homeDirectory attribute is required.
        /// </summary>
        ADS_UF_HOMEDIR_REQUIRED = 0x00000008,

        /// <summary>
        /// Specifies that the account is temporarily locked out.
        /// </summary>
        ADS_UF_LOCKOUT = 0x00000010,

        /// <summary>
        /// Specifies that the password-length policy, as specified in [MS-SAMR] section 3.1.1.7.1, does not apply to this user.
        /// </summary>
        ADS_UF_PASSWD_NOTREQD = 0x00000020,

        /// <summary>
        /// Specifies that the user cannot change his or her password.
        /// </summary>
        ADS_UF_PASSWD_CANT_CHANGE = 0x00000040,

        /// <summary>
        /// Specifies that the clear text password is to be persisted.
        /// </summary>
        ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x00000080,

        /// <summary>
        /// Specifies that the account is the default account type that represents a typical user.
        /// </summary>
        ADS_UF_NORMAL_ACCOUNT = 0x00000200,

        /// <summary>
        /// Specifies that the account is for a domain-to-domain trust.
        /// </summary>
        ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 0x00000800,

        /// <summary>
        /// Specifies that the account is a computer account for a computer that is a member of this domain.
        /// </summary>
        ADS_UF_WORKSTATION_TRUST_ACCOUNT = 0x00001000,

        /// <summary>
        /// Specifies that the account is a computer account for a DC.
        /// </summary>
        ADS_UF_SERVER_TRUST_ACCOUNT = 0x00002000,

        /// <summary>
        /// Specifies that the password does not expire for the account.
        /// </summary>
        ADS_UF_DONT_EXPIRE_PASSWD = 0x00010000,

        /// <summary>
        /// Specifies that a smart card is required to log in to the account.
        /// </summary>
        ADS_UF_SMARTCARD_REQUIRED = 0x00040000,

        /// <summary>
        /// This bit indicates that the "OK as Delegate" ticket flag,
        /// </summary>
        ADS_UF_TRUSTED_FOR_DELEGATION = 0x00080000,

        /// <summary>
        /// This bit indicates that the ticket-granting tickets (TGTs) of this account and the service tickets obtained
        /// by this account are not marked as forwardable or proxiable when the forwardable or proxiable ticket flags
        /// are requested.
        /// </summary>
        ADS_UF_NOT_DELEGATED = 0x00100000,

        /// <summary>
        /// This bit indicates that only des-cbc-md5 or des-cbc-crc keys, as defined in [RFC3961], are used in the
        /// Kerberos protocols for this account.
        /// </summary>
        ADS_UF_USE_DES_KEY_ONLY = 0x00200000,

        /// <summary>
        /// This bit indicates that the account is not required to present valid preauthentication data, as described
        /// in [RFC4120] section 7.5.2.
        /// </summary>
        ADS_UF_DONT_REQUIRE_PREAUTH = 0x00400000,

        /// <summary>
        /// Specifies that the password age on the user has exceeded the maximum password age policy.
        /// </summary>
        ADS_UF_PASSWORD_EXPIRED = 0x00800000,

        /// <summary>
        /// this bit indicates that the account (when running as a service) obtains an S4U2self service ticket
        /// (as specified in [MS-SFU]) with the forwardable flag set.
        /// </summary>
        ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x01000000,

        /// <summary>
        /// This bit indicates that when the Key Distribution Center (KDC) is issuing a service ticket for this account,
        /// the Privilege Attribute Certificate (PAC) MUST NOT be included.
        /// </summary>
        ADS_UF_NO_AUTH_DATA_REQUIRED = 0x02000000,

        /// <summary>
        /// Specifies that the account is a computer account for a read-only domain controller (RODC).
        /// </summary>
        ADS_UF_PARTIAL_SECRETS_ACCOUNT = 0x04000000
    }


    /// <summary>
    /// The format of a mailslot ping as documented in section 7.3.5.
    /// </summary>
    public struct NETLOGON_LOGON_QUERY
    {
        /// <summary>
        /// Operation code (see section 7.3.1.3). Set to LOGON_PRIMARY_QUERY.
        /// </summary>
        public AdtsOperationCode Opcode;

        /// <summary>
        /// Null-terminated ASCII value of the NETBIOS name of the client.
        /// </summary>
        public string ComputerName;

        /// <summary>
        /// Null-terminated ASCII value of the name of the mailslot on which the client listens.
        /// </summary>
        public string MailslotName;

        /// <summary>
        /// Null-terminated Unicode value of the NETBIOS name of the client.
        /// </summary>
        public string UnicodeComputerName;

        /// <summary>
        /// NETLOGON_NT_VERSION Options
        /// </summary>
        public NETLOGON_NT_VERSION NtVersion;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort LmNtToken;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort Lm20Token;
    }


    /// <summary>
    /// The NETLOGON_PRIMARY_RESPONSE structure is the PDC server's response to a mailslot ping (section 7.3.5).
    /// </summary>
    public struct NETLOGON_PRIMARY_RESPONSE
    {
        /// <summary>
        /// Operation code (see section 7.3.1.3). Set to LOGON_PRIMARY_RESPONSE.
        /// </summary>
        public AdtsOperationCode Opcode;

        /// <summary>
        /// Null-terminated ASCII value of the NetBIOS name of the server.
        /// </summary>
        public string PrimaryDCName;

        /// <summary>
        /// Null-terminated Unicode value of the NetBIOS name of the server.
        /// </summary>
        public string UnicodePrimaryDCName;

        /// <summary>
        /// Null-terminated Unicode value of the NetBIOS name of the NC.
        /// </summary>
        public string UnicodeDomainName;

        /// <summary>
        /// NETLOGON_NT_VERSION Options
        /// </summary>
        public NETLOGON_NT_VERSION NtVersion;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort LmNtToken;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort Lm20Token;
    }


    /// <summary>
    /// The format of a mailslot ping as documented in section 7.3.5.
    /// </summary>
    public struct NETLOGON_SAM_LOGON_REQUEST
    {
        /// <summary>
        /// Operation code (see section 7.3.1.3). Set to LOGON_SAM_LOGON_REQUEST.
        /// </summary>
        public AdtsOperationCode Opcode;

        /// <summary>
        /// A USHORT that contains the number of times the user has repeated this request.
        /// </summary>
        public ushort RequestCount;

        /// <summary>
        /// Null-terminated Unicode value of the NETBIOS name of the client.
        /// </summary>
        public string UnicodeComputerName;

        /// <summary>
        /// Null-terminated Unicode value of the account name of the user being queried.
        /// </summary>
        public string UnicodeUserName;

        /// <summary>
        /// Null-terminated ASCII value of the name of the mailslot the client listens on.
        /// </summary>
        public string MailslotName;

        /// <summary>
        /// Represents the userAccountControl attribute of an account.
        /// </summary>
        public AdtsUserAccountControl AllowableAccountControlBits;

        /// <summary>
        /// A DWORD that contains the size of the DomainSid field.
        /// </summary>
        public uint DomainSidSize;

        /// <summary>
        /// The SID of the domain, specified as a SID structure, which is defined in [MS-DTYP] section 2.4.2.
        /// </summary>
        public SecurityIdentifier DomainSid;

        /// <summary>
        /// NETLOGON_NT_VERSION Options
        /// </summary>
        public NETLOGON_NT_VERSION NtVersion;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort LmNtToken;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort Lm20Token;
    }


    /// <summary>
    /// The NETLOGON_SAM_LOGON_RESPONSE_NT40 structure is the server's response to an LDAP ping (section 7.3.3) or a
    /// mailslot ping (section 7.3.5).
    /// </summary>
    public struct NETLOGON_SAM_LOGON_RESPONSE_NT40
    {
        /// <summary>
        /// Operation code
        /// </summary>
        public AdtsOperationCode Opcode;

        /// <summary>
        /// Null-terminated Unicode value of the NetBIOS name of the server.
        /// </summary>
        public string UnicodeLogonServer;

        /// <summary>
        /// Null-terminated Unicode value of the name of the user copied directly from the client's request.
        /// </summary>
        public string UnicodeUserName;

        /// <summary>
        /// Null-terminated Unicode value of the NetBIOS name of the NC.
        /// </summary>
        public string UnicodeDomainName;

        /// <summary>
        /// Set to NETLOGON_NT_VERSION_1.
        /// </summary>
        public NETLOGON_NT_VERSION NtVersion;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort LmNtToken;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort Lm20Token;
    }


    /// <summary>
    /// The NETLOGON_SAM_LOGON_RESPONSE structure is the first extended version of the server's response to an LDAP
    /// ping (section 7.3.3) or a mailslot ping (section 7.3.5).
    /// </summary>
    public struct NETLOGON_SAM_LOGON_RESPONSE
    {
        /// <summary>
        /// Operation code
        /// </summary>
        public AdtsOperationCode Opcode;

        /// <summary>
        /// Null-terminated Unicode value of the NetBIOS name of the server.
        /// </summary>
        public string UnicodeLogonServer;

        /// <summary>
        /// Null-terminated Unicode value of the name of the user copied directly from the client's request.
        /// </summary>
        public string UnicodeUserName;

        /// <summary>
        /// Null-terminated Unicode value of the NetBIOS name of the NC.
        /// </summary>
        public string UnicodeDomainName;

        /// <summary>
        /// The value of the NC's GUID attribute specified as a GUID structure,
        /// </summary>
        public Guid DomainGuid;

        /// <summary>
        /// A NULL GUID.
        /// </summary>
        public Guid NullGuid;

        /// <summary>
        /// DNS name of the forest
        /// </summary>
        public string DnsForestName;

        /// <summary>
        /// DNS name of the NC
        /// </summary>
        public string DnsDomainName;

        /// <summary>
        /// DNS name of the server
        /// </summary>
        public string DnsHostName;

        /// <summary>
        /// The domain controller IP address,
        /// </summary>
        public IPAddress DcIpAddress;

        /// <summary>
        /// DS_FLAG Options
        /// </summary>
        public DS_FLAG Flags;

        /// <summary>
        /// NETLOGON_NT_VERSION_1 | NETLOGON_NT_VERSION_5.
        /// </summary>
        public NETLOGON_NT_VERSION NtVersion;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort LmNtToken;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort Lm20Token;
    }


    /// <summary>
    /// This field is included only if the client specifies NETLOGON_NT_VERSION_5EX_WITH_IP in the request.
    /// </summary>
    public struct AdtsSocketAddress
    {
        /// <summary>
        /// The socket family, represented in little-endian byte order. The value SHOULD always be AF_INET(that is, 2).
        /// </summary>
        public ushort sin_family;

        /// <summary>
        /// The socket port, represented in little-endian byte order. The value SHOULD always be zero.
        /// </summary>
        public ushort sin_port;

        /// <summary>
        /// The socket address, represented in big-endian byte order. The value is an IPv4 address. If the domain
        /// controller does not have an IPv4 address, this value SHOULD be 127.0.0.1.
        /// </summary>
        public uint sin_addr;

        /// <summary>
        /// Reserved. MUST be set to zero when sending and ignored on receipt.
        /// </summary>
        public ulong sin_zero;
    }


    /// <summary>
    /// The NETLOGON_SAM_LOGON_RESPONSE_EX structure is the second extended version of the server's response to an LDAP
    /// ping (section 7.3.3) or a mailslot ping (section 7.3.5).
    /// </summary>
    public struct NETLOGON_SAM_LOGON_RESPONSE_EX
    {
        /// <summary>
        /// Operation code
        /// </summary>
        public AdtsOperationCode Opcode;

        /// <summary>
        /// This MUST be set to 0.
        /// </summary>
        public ushort Sbz;

        /// <summary>
        /// DS_FLAG Options
        /// </summary>
        public DS_FLAG Flags;

        /// <summary>
        /// The value of the NC's GUID attribute specified as a GUID structure
        /// </summary>
        public Guid DomainGuid;

        /// <summary>
        /// DNS name of the forest
        /// </summary>
        public string DnsForestName;

        /// <summary>
        /// DNS name of the NC
        /// </summary>
        public string DnsDomainName;

        /// <summary>
        /// DNS name of the server
        /// </summary>
        public string DnsHostName;

        /// <summary>
        /// NetBIOS name of the NC
        /// </summary>
        public string NetbiosDomainName;

        /// <summary>
        /// NetBIOS name of the server
        /// </summary>
        public string NetbiosComputerName;

        /// <summary>
        /// User name specified in the client's request
        /// </summary>
        public string UserName;

        /// <summary>
        /// Site name of the server
        /// </summary>
        public string DcSiteName;

        /// <summary>
        /// Site name of the client
        /// </summary>
        public string ClientSiteName;

        /// <summary>
        /// A CHAR that contains the size of the server's IP address. This field is included only if the client
        /// specifies NETLOGON_NT_VERSION_5EX_WITH_IP in the request.
        /// </summary>
        public byte? DcSockAddrSize;

        /// <summary>
        /// The domain controller IPv4 address, structured as shown in the following diagram. This field is included
        /// only if the client specifies NETLOGON_NT_VERSION_5EX_WITH_IP in the request.
        /// </summary>
        public AdtsSocketAddress? DcSockAddr;

        /// <summary>
        /// contains the name of the site that is closest by cost to ClientSiteName without being equal to it.
        /// </summary>
        public string NextClosestSiteName;

        /// <summary>
        /// NETLOGON_NT_VERSION_1 | NETLOGON_NT_VERSION_5EX.
        /// </summary>
        public NETLOGON_NT_VERSION NtVersion;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort LmNtToken;

        /// <summary>
        /// This MUST be set to 0xFFFF.
        /// </summary>
        public ushort Lm20Token;
    }


    /// <summary>
    /// instanceType attribute of an NC root
    /// </summary>
    [Flags]
    public enum AdtsInstanceType
    {
        /// <summary>
        /// No bit is set
        /// </summary>
        NONE = 0,

        /// <summary>
        /// This flag is set (value 1) on all NC roots.
        /// </summary>
        IT_NC_HEAD = 0x00000001,

        /// <summary>
        /// If this flag is set, the NC replica that this root represents does not exist locally.
        /// </summary>
        IT_UNINSTANT = 0x00000002,

        /// <summary>
        /// This flag is written locally based upon the desired NC replica type.
        /// </summary>
        IT_WRITE = 0x00000004,

        /// <summary>
        /// This flag indicates that the local DC holds an instantiated NC replica that is a parent of the NC replica
        /// represented by this NC root.
        /// </summary>
        IT_NC_ABOVE = 0x00000008,

        /// <summary>
        /// This flag indicates that the NC replica has not completed its initial replication into the local DC, and
        /// may not have a full set of objects in the NC represented by this NC root.
        /// </summary>
        IT_NC_COMING = 0x00000010,

        /// <summary>
        /// This flag indicates that the NC replica is being removed from the local DC, and may not have a full set of
        /// objects in the NC represented by this NC root.
        /// </summary>
        IT_NC_GOING = 0x00000020,
    }


    /// <summary>
    /// The enumeration of all Ldap Syntax.
    /// </summary>
    public enum AdtsLdapSyntax
    {
        /// <summary>
        /// Values in this syntax are encoded according to the following BNF: boolean = "TRUE" / "FALSE"
        /// Boolean values have an encoding of "TRUE" if they are logically true, and have an encoding of 
        /// "FALSE" otherwise
        /// </summary>
        Boolean,

        /// <summary>
        /// Values in this syntax are encoded as the decimal representation of their values, with each decimal 
        /// digit represented by the its character equivalent. So the number 1321 is represented by the character
        /// string "1321".
        /// </summary>
        Enumeration,

        /// <summary>
        /// Values in this syntax are encoded as the decimal representation of their values, with each decimal 
        /// digit represented by the its character equivalent. So the number 1321 is represented by the character
        /// string "1321".
        /// </summary>
        Integer,

        /// <summary>
        /// Values in this syntax are encoded as the decimal representation of their values, with each decimal 
        /// digit represented by the its character equivalent. So the number 1321 is represented by the character
        /// string "1321".
        /// </summary>
        LargeInteger,

        /// <summary>
        /// A value with this syntax is a UTF-8 string in the following format:
        /// presentation_address#X500:object_DN
        /// where presentation_address is a value encoded in the Object(Presentation-Address) syntax, object_DN is
        /// a DN in Object(DS-DN) form, and all remaining characters are string literals.
        /// </summary>
        ObjectForAccessPoint,

        /// <summary>
        /// A value with this syntax is a UTF-8 string in the following format:
        /// S:byte_count:string_value:object_DN
        /// where byte_count is the number (in decimal) of bytes in the string_value string, object_DN is a DN in
        /// Object(DS-DN) form, and all remaining characters are string literals. Since string_value is a UTF-8
        /// string, one character can require more than one byte to represent it.
        /// </summary>
        ObjectForDNString,

        /// <summary>
        /// A value with this syntax is a UTF-8 string in the following format:
        /// object_DN
        /// where object_DNis a DN in Object(DS-DN) form.
        /// </summary>
        ObjectForORName,

        /// <summary>
        /// A value with this syntax is a UTF-8 string in the following format:
        /// B:char_count:binary_value:object_DN
        /// where char_count is the number (in decimal) of hexadecimal digits in binary_value, binary_value is the
        /// hexadecimal representation of a binary value, object_DN is a DN in Object(DS-DN) form, and all remaining
        /// characters are string literals. Each byte is represented by a pair of hexadecimal characters in 
        /// binary_value, with the first character of each pair corresponding to the most-significant nibble of the
        /// byte. The first pair in binary_value corresponds to the first byte of the binary value, with subsequent 
        /// pairs corresponding to the remaining bytes in sequential order. Note that char_count is always even in a
        /// syntactically-valid Object(DN-Binary) value.
        /// </summary>
        ObjectForDNBinary,

        /// <summary>
        /// Values in the Distinguished Name syntax are encoded to have the representation defined in RFC2252 [5].
        /// Note that this representation is not reversible to an ASN.1 encoding used in X.500 for Distinguished Names,
        /// as the CHOICE of any DirectoryString element in an RDN is no longer known
        /// Examples (from [5]):
        /// CN=Steve Kille,O=Isode Limited,C=GB
        /// OU=Sales+CN=J. Smith,O=Widget Inc.,C=US
        /// CN=L. Eagle,O=Sue\, Grabbit and Runn,C=GB
        /// CN=Before\0DAfter,O=Test,C=GB
        /// 1.3.6.1.4.1.1466.0=#04024869,O=Test,C=GB
        /// SN=Lu\C4\8Di\C4\87
        /// </summary>
        ObjectForDSDN,

        /// <summary>
        /// Values in this syntax are encoded with the representation described in RFC 1278 [6].
        /// </summary>
        ObjectForPresentationAddress,

        /// <summary>
        /// This encoding format is used if the binary encoding is requested by the client for an attribute, or if
        /// the attribute syntax name is "1.3.6.1.4.1.1466.115.121.1.5". The contents of the LDAP AttributeValue
        /// or AssertionValue field is a BER-encoded instance of the attribute value or a matching rule assertion
        /// value ASN.1 data type as defined for use with X.500. (The first byte inside the OCTET STRING wrapper is
        /// a tag octet.  However, the OCTET STRING is still encoded in primitive form.)
        /// All servers MUST implement this form for both generating attribute values in search responses, and
        /// parsing attribute values in add, compare and modify requests, if the attribute type is recognized and
        /// the attribute syntax name is that of Binary.  Clients which request that all attributes be returned 
        /// from entries MUST be prepared to receive values in binary (e.g. userCertificate;binary), and SHOULD NOT
        /// simply display binary or unrecognized values to users.
        /// </summary>
        ObjectForReplicaLink,

        /// <summary>
        /// A value with this syntax is a case-sensitive UTF-8 string, but the server does not enforce that a value
        /// of this syntax must be a valid UTF-8 string.
        /// </summary>
        StringForCase,

        /// <summary>
        /// The encoding of a value in this syntax is the string value itself.
        /// </summary>
        StringForIA5,

        /// <summary>
        /// A value with this syntax contains a Windows security descriptor in binary form. The binary form is that
        /// of a SECURITY_DESCRIPTOR structure and is specified in [MS-DTYP] section 2.4.6. It is otherwise encoded
        /// the same as the String(Octet) syntax.
        /// </summary>
        StringForNTSecDesc,

        /// <summary>
        /// The encoding of a string in this syntax is the string value itself.
        /// Example:
        /// 1997
        /// </summary>
        StringForNumeric,

        /// <summary>
        /// Values in the Object Identifier syntax are encoded according to the BNF in section RFC2252 4.1 for "oid".
        /// Example:
        /// 1.2.3.4
        /// cn
        /// </summary>
        StringForObjectIdentifier,

        /// <summary>
        /// This encoding format is used if the binary encoding is requested by the client for an attribute, or if
        /// the attribute syntax name is "1.3.6.1.4.1.1466.115.121.1.5". The contents of the LDAP AttributeValue
        /// or AssertionValue field is a BER-encoded instance of the attribute value or a matching rule assertion
        /// value ASN.1 data type as defined for use with X.500. (The first byte inside the OCTET STRING wrapper is
        /// a tag octet.  However, the OCTET STRING is still encoded in primitive form.)
        /// All servers MUST implement this form for both generating attribute values in search responses, and
        /// parsing attribute values in add, compare and modify requests, if the attribute type is recognized and
        /// the attribute syntax name is that of Binary.  Clients which request that all attributes be returned 
        /// from entries MUST be prepared to receive values in binary (e.g. userCertificate;binary), and SHOULD NOT
        /// simply display binary or unrecognized values to users.
        /// </summary>
        StringForOctet,

        /// <summary>
        /// The encoding of a value in this syntax is the string value itself.
        /// PrintableString is limited to the characters in production p of RFC2252 section 4.1.
        /// Example:
        /// This is a PrintableString
        /// </summary>
        StringForPrintable,

        /// <summary>
        /// A value with this syntax contains a SID in binary form. The binary form is that of a SID structure 
        /// (the SID structure is specified in [MS-DTYP] section 2.4.2). It is otherwise encoded the same as the 
        /// String(Octet) syntax.
        /// </summary>
        StringForSid,

        /// <summary>
        /// A value with this syntax is a UTF-8 string restricted to characters with values between 0x20 and 0x7E,
        /// inclusive.
        /// </summary>
        StringForTeletex,

        /// <summary>
        /// A string in this syntax is encoded in the UTF-8 form of ISO 10646 (a superset of Unicode).  Servers and
        /// clients MUST be prepared to receive encodings of arbitrary Unicode characters, including characters not
        /// presently assigned to any character set.
        /// For characters in the PrintableString form, the value is encoded as the string value itself.
        /// If it is of the TeletexString form, then the characters are transliterated to their equivalents in
        /// UniversalString, and encoded in UTF-8
        /// </summary>
        StringForUnicode,

        /// <summary>
        /// Values in this syntax are encoded as if they were printable strings with the strings containing a 
        /// UTCTime value. This is historical; new attribute definitions SHOULD use GeneralizedTime instead.
        /// </summary>
        StringForUTCTime,

        /// <summary>
        /// Values in this syntax are encoded as printable strings, represented as specified in X.208.
        /// Note that the time zone must be specified. It is strongly recommended that GMT time be used.  For example,
        ///        199412161032Z
        /// </summary>
        StringForGeneralizedTime
    }
}
