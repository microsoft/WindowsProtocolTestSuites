// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    #region Abstract SMB Request

    /// <summary>
    /// SMB request is a general abstract SMB request, it is a base class. 
    /// Other SMB request will derive from this and add members for their self.
    /// </summary>
    public abstract class SmbRequest
    {
        /// <summary>
        /// This is used to associate a response with a request.
        /// </summary>
        public int messageId;

        /// <summary>
        /// The operation code that this SMB is requesting or responding to. 
        /// Including Command and the subcommands of SMB_COM_TRANSACTION, SMB_COM_TRANSACTION2 and SMB_NT_TRANSACTION.
        /// </summary>
        public Command command;

        /// <summary>
        /// SMBRequest constructor.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="command">
        /// The operation code that this SMB is requesting or responding to. 
        /// Including Command and the subcommands of SMB_COM_TRANSACTION, SMB_COM_TRANSACTION2 and SMB_NT_TRANSACTION.
        /// </param>
        protected SmbRequest(int messageId, Command command)
        {
            this.messageId = messageId;
            this.command = command;
        }
    }

    #endregion

    #region SMB Requests

    /// <summary>
    /// SMB_COM_NEGOTIATE request.
    /// </summary>
    public class NegotiateRequest : SmbRequest
    {
        /// <summary>
        /// It indicates whether the System Under Test (the SUT) supports the extended security.
        /// </summary>
        public bool isSupportExtSecurity;

        /// <summary>
        /// It indicates the client message signing policy.
        /// </summary>
        public SignState signState;

        /// <summary>
        /// The input array of dialects.
        /// </summary>
        public Sequence<Dialect> dialectNames;

        /// <summary>
        /// SMB_COM_NEGOTIATE request.
        /// </summary>
        /// <param name="dialectNames">The input array of dialects.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="isSupportExtSecurity">It indicates whether the SUT supports the extended security.</param>
        /// <param name="signState">State of the SUT has message signing enabled or required.</param>
        public NegotiateRequest(
            int messageId,
            bool isSupportExtSecurity,
            SignState signState,
            Sequence<Dialect> dialectNames)
            : base(messageId, Command.SmbComNegotiate)
        {
            this.isSupportExtSecurity = isSupportExtSecurity;
            this.signState = signState;
            this.dialectNames = dialectNames;
        }
    }


    /// <summary>
    /// SMB_COM_SESSION_SETUP request.
    /// </summary>
    public class SessionSetupRequest : SmbRequest
    {
        /// <summary>
        /// It indicates the account type to establish the session.
        /// </summary>
        public AccountType accountType;

        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// It indicates the security signature used in session setup response header. 
        /// </summary>
        public int securitySignature;

        /// <summary>
        /// It indicates whether the message signing is required.
        /// </summary>
        public bool isRequireSign;

        /// <summary>
        /// Used by the SUT to notify the client as to which features are supported by the SUT.
        /// </summary>
        public Set<Capabilities> capabilities;

        /// <summary>
        /// SMB_COM_SESSION_SETUP request
        /// </summary>
        /// <param name="accountType">It indicates the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// /// </param>
        /// <param name="securitySignature">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        /// <param name="capabilities">
        /// It is used by the SUT to notify the client as to which features are supported by the SUT.
        /// </param>
        public SessionSetupRequest(
            AccountType accountType,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            Set<Capabilities> capabilities)
            : base(messageId, Command.SmbComSessionSetup)
        {
            this.accountType = accountType;
            this.sessionId = sessionId;
            this.isRequireSign = isRequireSign;
            this.securitySignature = securitySignature;
            this.capabilities = capabilities;
        }
    }


    /// <summary>
    /// SMB_COM_SESSION_SETUP request.
    /// </summary>
    public class SessionSetupRequestAdditional : SmbRequest
    {
        /// <summary>
        /// It indicates the account type to establish the session.
        /// </summary>
        public AccountType accountType;

        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// It indicates the security signature used in session setup response header. 
        /// </summary>
        public int securitySignature;

        /// <summary>
        /// It indicates whether the message signing is required.
        /// </summary>
        public bool isRequireSign;

        /// <summary>
        /// It is used by the SUT to notify the client as to which features are supported by the SUT.
        /// </summary>
        public Set<Capabilities> capabilities;

        /// <summary>
        /// It indicates whether Generic Security Services (GSS) is valid or not.
        /// </summary>
        public bool isGssValid;

        /// <summary>
        /// It indicates whether the user ID is valid or not.
        /// </summary>
        public bool isUserIdValid;


        /// <summary>
        /// SMB_COM_SESSION_SETUP request
        /// </summary>
        /// <param name="accountType">It indicates the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.</param>
        /// <param name="securitySignature">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        /// <param name="capabilities">
        /// It is used by the SUT to notify the client as to which features are supported by the SUT.
        /// </param>
        /// <param name="isGssValid">It indicates whether GSS is valid or not.</param>
        /// <param name="isUserIdValid">It indicates whether the user ID is valid or not.</param>
        public SessionSetupRequestAdditional(
            AccountType accountType,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            Set<Capabilities> capabilities,
            bool isGssValid,
            bool isUserIdValid)
            : base(messageId, Command.SmbComSessionSetupAdditional)
        {
            this.accountType = accountType;
            this.sessionId = sessionId;
            this.isRequireSign = isRequireSign;
            this.securitySignature = securitySignature;
            this.capabilities = capabilities;
            this.isGssValid = isGssValid;
            this.isUserIdValid = isUserIdValid;
        }
    }


    /// <summary>
    /// SMB_COM_SESSION_SETUP request for non-extended security mode.
    /// </summary>
    public class NonExtendedSessionSetupRequest : SmbRequest
    {
        /// <summary>
        /// It indicates the account type to establish the session.
        /// </summary>
        public AccountType accountType;

        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// It indicates the security signature used in session setup response header. 
        /// </summary>
        public int securitySignature;

        /// <summary>
        /// It indicates whether the message signing is required.
        /// </summary>
        public bool isRequireSign;

        /// <summary>
        /// Used by the SUT to notify the client as to which features are supported by the SUT.
        /// </summary>
        public Set<Capabilities> capabilities;


        /// <summary>
        /// SMB_COM_SESSION_SETUP request for non-extended security mode.
        /// </summary>
        /// <param name="accountType">It indicates the account type to establish the session.</param>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="securitySignature">
        /// It indicates the security signature used in session setup response header.
        /// </param>
        /// <param name="isRequireSign">It indicates whether the message signing is required.</param>
        public NonExtendedSessionSetupRequest(
            AccountType accountType,
            int messageId,
            int sessionId,
            int securitySignature,
            bool isRequireSign,
            Set<Capabilities> capabilities)
            : base(messageId, Command.SmbComSessionSetup)
        {
            this.accountType = accountType;
            this.sessionId = sessionId;
            this.isRequireSign = isRequireSign;
            this.securitySignature = securitySignature;
            this.capabilities = capabilities;
        }
    }

    /// <summary>
    /// SMB_COM_TREE_CONNECT_ANDX request.
    /// </summary>
    public class TreeConnectRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// It indicates whether TREE_CONNECT_ANDX_DISCONNECT_TID is set.
        /// </summary>
        public bool isTidDisconnectionSet;

        /// <summary>
        /// It indicates whether the client is requesting signing key protection.
        /// </summary>
        public bool isRequestExtSignature;

        /// <summary>
        /// It indicates whether the client is requesting extended information on the SMB_COM_TREE_CONNECT_ANDX 
        /// response or not.
        /// </summary>
        public bool isRequestExtResponse;

        /// <summary>
        /// This is used to indicate which share the client wants to access.
        /// </summary>
        public string shareName;

        /// <summary>
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// SMB_COM_TREE_CONNECT_ANDX request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="shareName">This is used to indicate which share the client wants to access.</param>
        /// <param name="isTidDisconnectionSet">It indicates whether TREE_CONNECT_ANDX_DISCONNECT_TID is set.</param>
        /// <param name="isRequestExtSignature">
        /// It indicates whether the client is requesting signing key protection.
        /// </param>
        /// <param name="isRequestExtResponse">
        /// It indicates whether the client is requesting extended information on the SMB_COM_TREE_CONNECT_ANDX 
        /// response or not.
        /// </param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        public TreeConnectRequest(
            int messageId,
            int sessionId,
            bool isTidDisconnectionSet,
            bool isRequestExtSignature,
            bool isRequestExtResponse,
            string shareName,
            ShareType shareType)
            : base(messageId, Command.SmbComTreeConnect)
        {
            this.sessionId = sessionId;
            this.isTidDisconnectionSet = isTidDisconnectionSet;
            this.isRequestExtSignature = isRequestExtSignature;
            this.isRequestExtResponse = isRequestExtResponse;
            this.shareName = shareName;
            this.shareType = shareType;
        }
    }
    /// <summary>
    /// SMB_COM_NT_CREATE_ANDX request.
    /// </summary>
    public class CreateRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// Access wanted. 
        /// This value must be specified in the ACCESS_MASK format.
        /// </summary>
        public int desiredAccess;

        /// <summary>
        /// Type of shared access requested for this file or directory.
        /// </summary>
        public int shareAccess;

        /// <summary>
        /// The action to take if a file does or does not exist.
        /// </summary>
        public CreateDisposition createDisposition;

        /// <summary>
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </summary>
        public int impersonationLevel;

        /// <summary>
        /// The name of the file that will be created.
        /// </summary>
        public string name;

        /// <summary>
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// It indicates whether FILE_OPEN_BY_FILE_ID is set in CreateOptions field.
        /// </summary>
        public bool isOpenByFileId;

        /// <summary>
        /// It indicates whether the FILE_DIRECTORY_FILE and FILE_NON_DIRECTORY_FILE are set. 
        /// If true,FILE_DIRECTORY_FILE is set; else FILE_NON_DIRECTORY_FILE is set.
        /// </summary>
        public bool isDirectoryFile;


        /// <summary>
        /// SMB_COM_NT_CREATE_ANDX request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="name">This is used to represent the name of the resource.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="desiredAccess">Access wanted. This value must be specified in the ACCESS_MASK format.</param>
        /// <param name="createDisposition">The action to take if a file does or does not exist.</param>
        /// <param name="impersonationLevel">
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </param>        
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="isOpenByFileId">
        /// It indicates whether FILE_OPEN_BY_FILE_ID is set in CreateOptions field.
        /// </param>
        /// <param name="isDirectoryFile">
        /// It indicates whether the FILE_DIRECTORY_FILE and FILE_NON_DIRECTORY_FILE are set. 
        /// If true,FILE_DIRECTORY_FILE is set; else FILE_NON_DIRECTORY_FILE is set.
        /// </param>
        public CreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            int desiredAccess,
            CreateDisposition createDisposition,
            int impersonationLevel,
            string name,
            ShareType shareType,
            bool isSigned,
            bool isOpenByFileId,
            bool isDirectoryFile)
            : base(messageId, Command.NtCreateRequest)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.desiredAccess = desiredAccess;
            this.createDisposition = createDisposition;
            this.impersonationLevel = impersonationLevel;
            this.name = name;
            this.shareType = shareType;
            this.isSigned = isSigned;
            this.isOpenByFileId = isOpenByFileId;
            this.isDirectoryFile = isDirectoryFile;
        }
    }


    /// <summary>
    /// SMB_COM_OPEN_ANDX Request.
    /// </summary>
    public class OpenRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// Access wanted. This value must be specified in the ACCESS_MASK format.
        /// </summary>
        public int desiredAccess;

        /// <summary>
        /// The name of the file that will be created.
        /// </summary>
        public string name;

        /// <summary>
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </summary>
        public int impersonationLevel;

        /// <summary>
        /// SMB_COM_OPEN_ANDX request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="desiredAccess">Access wanted. This value must be specified in the ACCESS_MASK format.</param>
        /// <param name="impersonationLevel">
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </param>
        /// <param name="name">This is used to represent the name of the resource.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        public OpenRequest(
            int messageId,
            int sessionId,
            int treeId,
            int desiredAccess,
            int impersonationLevel,
            string name,
            ShareType shareType,
            bool isSigned)
            : base(messageId, Command.NtOpenrequest)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.desiredAccess = desiredAccess;
            this.name = name;
            this.shareType = shareType;
            this.isSigned = isSigned;
            this.impersonationLevel = impersonationLevel;
        }
    }


    /// <summary>
    /// SMB_COM_READ_ANDX Request.
    /// </summary>
    public class ReadRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;


        /// <summary>
        /// SMB_COM_READ_ANDX request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="desiredAccess">Access wanted. This value must be specified in the ACCESS_MASK format.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        public ReadRequest(int messageId, int sessionId, int treeId, int fId, ShareType shareType, bool isSigned)
            : base(messageId, Command.NtReadRequest)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.shareType = shareType;
            this.isSigned = isSigned;
        }
    }


    /// <summary>
    /// SMB_COM_WRITE_ANDX Request.
    /// </summary>
    public class WriteRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// SMB_COM_WRITE_ANDX request
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId"
        /// >Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        public WriteRequest(int messageId, int sessionId, int treeId, ShareType shareType, bool isSigned)
            : base(messageId, Command.NtWriteRequest)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.shareType = shareType;
            this.isSigned = isSigned;
        }
    }


    /// <summary>
    /// Invalid command request.
    /// </summary>
    public class InvalidCommandRequest : SmbRequest
    {
        /// <summary>
        /// Valid command.
        /// </summary>
        public Command validCommand;


        /// <summary>
        /// Invalid command request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="validCommand">Vaild command.</param>
        public InvalidCommandRequest(int messageId, Command validCommand)
            : base(messageId, Command.InvalidCommand)
        {
            this.validCommand = validCommand;
        }
    }


    /// <summary>
    /// TRANS2_QUERY_FILE_INFORMATION Request
    /// </summary>
    public class Trans2QueryFileInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </summary>
        public bool isUsePassthrough;

        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        public Trans2QueryFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fId,
            bool isUsePassthrough,
            InformationLevel informationLevel)
            : base(messageId, Command.Trans2QueryFileInformation)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isSigned = isSigned;
            this.fId = fId;
            this.isUsePassthrough = isUsePassthrough;
            this.informationLevel = informationLevel;
        }
    }


    /// <summary>
    /// TRANS2_QUERY_PATH_INFORMATION Request.
    /// </summary>
    public class Trans2QueryPathInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// isSigned
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// qmtTokenIndex
        /// The index of the GMT token configured by CheckPreviousVersion action. 
        /// </summary>
        public int gmtTokenIndex;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </summary>
        public bool isUsePassthrough;

        /// <summary>
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </summary>
        public bool isReparse;

        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        public Trans2QueryPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int gmtTokenIndex,
            bool isUsePassthrough,
            bool isReparse,
            InformationLevel informationLevel)
            : base(messageId, Command.Trans2QueryPathInformation)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isSigned = isSigned;
            this.gmtTokenIndex = gmtTokenIndex;
            this.isUsePassthrough = isUsePassthrough;
            this.isReparse = isReparse;
            this.informationLevel = informationLevel;
        }
    }


    /// <summary>
    /// TRANS2_SET_FILE_INFORMATION Request.
    /// </summary>
    public class Trans2SetFileInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// This is used to represent the name of the resource.
        /// </summary>
        public string fileName;

        /// <summary>
        /// It indicates whether the new name or link will replace the original one that exists already.
        /// </summary>
        public bool replaceEnable;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </summary>
        public bool isUsePassthrough;

        /// <summary>
        /// Whether RootDirectory is null
        /// </summary>
        public bool isRootDirectory;

        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="replaceEnable">
        /// It indicates whether the new name or link will replace the original one that exists already.
        /// </param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="fileName">The This is used to represent the name of the resource.</param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        /// <param name="isRootDirectory">Whether RootDirectory is null</param>
        public Trans2SetFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool replaceEnable,
            bool isUsePassthrough,
            int fId,
            string fileName,
            InformationLevel informationLevel,
            bool isRootDirectory)
            : base(messageId, Command.Trans2SetFileInformation)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isSigned = isSigned;
            this.replaceEnable = replaceEnable;
            this.isUsePassthrough = isUsePassthrough;
            this.fId = fId;
            this.fileName = fileName;
            this.informationLevel = informationLevel;
            this.isRootDirectory = isRootDirectory;
        }
    }


    /// <summary>
    /// TRANS2_SET_PATH_INFORMATION Request.
    /// </summary>
    public class Trans2SetPathInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// The index of the GMT token configured by CheckPreviousVersion action. 
        /// </summary>
        public int gmtTokenIndex;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </summary>
        public bool isUsePassthrough;

        /// <summary>
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </summary>
        public bool isReparse;

        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action</param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        public Trans2SetPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isReparse,
            int gmtTokenIndex,
            InformationLevel informationLevel)
            : base(messageId, Command.Trans2SetPathInforamtion)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isSigned = isSigned;
            this.isUsePassthrough = isUsePassthrough;
            this.isReparse = isReparse;
            this.gmtTokenIndex = gmtTokenIndex;
            this.informationLevel = informationLevel;
        }
    }


    /// <summary>
    /// TRANS2_QUERY_FS_INFORMATION Request.
    /// </summary>
    public class Trans2QueryFSInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </summary>
        public bool isUsePassthrough;

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        public Trans2QueryFSInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            InformationLevel informationLevel)
            : base(messageId, Command.Trans2QueryFsInformation)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isSigned = isSigned;
            this.isUsePassthrough = isUsePassthrough;
            this.informationLevel = informationLevel;
        }
    }


    /// <summary>
    /// TRANS2_SET_FS_INFORMATION Request.
    /// </summary>
    public class Trans2SetFSInfoRequestAdditional : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </summary>
        public bool isUsePassthrough;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION request parameter.
        /// </summary>
        public Trans2SetFsInfoResponseParameter requestPara;
        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">Set this value to 0 to request a new session setup, or set this value to a previously established session identifier to request the re-authentication of an existing session.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="fId"></param>
        /// <param name="isUsePassthrough">isUsePassthrough.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        /// <param name="requestPara">TRANS2_SET_FS_INFORMATION request parameter.</param>
        public Trans2SetFSInfoRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            InformationLevel informationLevel,
            Trans2SetFsInfoResponseParameter requestPara)
            : base(messageId, Command.Trans2SetFsInforamtionAdditional)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.isSigned = isSigned;
            this.requestPara = requestPara;
            this.informationLevel = informationLevel;
        }
    }


    /// <summary>
    /// TRANS2_SET_FS_INFORMATION Request.
    /// </summary>
    public class Trans2SetFSInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether flags set to Disconnect_TID.
        /// </summary>
        public bool requireDisconnectTreeFlags;

        /// <summary>
        /// It indicates whether flags set to NO_RESPONSE.
        /// </summary>
        public bool requireNoResponseFlags;

        /// <summary>
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </summary>
        public bool isUsePassthrough;

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isUsePassthrough">isUsePassthrough.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        /// <param name="requireDisconnectTreeFlags">It indicates whether flags set to Disconnect_TID.</param>
        /// <param name="requireNoResponseFlags">It indicates whether flags set to NO_RESPONSE.</param>
        public Trans2SetFSInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isUsePassthrough,
            bool requireDisconnectTreeFlags,
            bool requireNoResponseFlags,
            bool isSigned,
            InformationLevel informationLevel)
            : base(messageId, Command.Trans2SetFsInformaiton)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isUsePassthrough = isUsePassthrough;
            this.requireDisconnectTreeFlags = requireDisconnectTreeFlags;
            this.requireNoResponseFlags = requireNoResponseFlags;
            this.isSigned = isSigned;
            this.informationLevel = informationLevel;
        }
    }


    /// <summary>
    /// TRANS2_FIND_FIRST2 Request.
    /// </summary>
    public class Trans2FindFirst2Request : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The index of the GMT token configured by CheckPreviousVersion action. 
        /// </summary>
        public int gmtTokenIndex;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </summary>
        public bool isReparse;

        /// <summary>
        /// It indicates the adapter to set SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header or not.
        /// </summary>
        public bool isFlagsKnowsLongNameSet;

        /// <summary>
        /// It indicates whether the find in GMT pattern.
        /// </summary>
        public bool isGmtPatten;

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// It indicates the adapter to set SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header or not.
        /// </param>
        /// <param name="isGmtPatten">It indicates whether the find in GMT pattern.</param>
        public Trans2FindFirst2Request(
            int messageId,
            int sessionId,
            int treeId,
            int gmtTokenIndex,
            bool isReparse,
            InformationLevel informationLevel,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten)
            : base(messageId, Command.TransFindFirst2)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.gmtTokenIndex = gmtTokenIndex;
            this.isReparse = isReparse;
            this.informationLevel = informationLevel;
            this.isFlagsKnowsLongNameSet = isFlagsKnowsLongNameSet;
            this.isGmtPatten = isGmtPatten;
        }
    }


    /// <summary>
    /// TRANS2_FIND_NEXT2 Request.
    /// </summary>
    public class Trans2FindNext2Request : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The index of the GMT token configured by CheckPreviousVersion action. 
        /// </summary>
        public int gmtTokenIndex;

        /// <summary>
        /// This can be used to query information from the SUT.
        /// </summary>
        public InformationLevel informationLevel;

        /// <summary>
        /// Search Handler. 
        /// </summary>
        public int searchHandlerId;

        /// <summary>
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </summary>
        public bool isReparse;

        /// <summary>
        /// It indicates the adapter to set SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header or not
        /// </summary>
        public bool isFlagsKnowsLongNameSet;


        /// <summary>
        /// TRANS2_FIND_NEXT2 Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action</param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the SUT.</param>
        /// <param name="searchHandlerId">Search Handler.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// It indicates the adapter to set SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header or not.
        /// </param>
        public Trans2FindNext2Request(
            int messageId,
            int sessionId,
            int treeId,
            int searchHandlerId,
            int gmtTokenIndex,
            bool isReparse,
            InformationLevel informationLevel,
            bool isFlagsKnowsLongNameSet)
            : base(messageId, Command.TransFindNext2)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.searchHandlerId = searchHandlerId;
            this.gmtTokenIndex = gmtTokenIndex;
            this.isReparse = isReparse;
            this.informationLevel = informationLevel;
            this.isFlagsKnowsLongNameSet = isFlagsKnowsLongNameSet;
        }
    }


    /// <summary>
    /// TRANS2_GET_DFS_REFERRAL Request.
    /// </summary>
    public class Trans2GetDfsReferralRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// TRANS2_GET_DFS_REFERRAL Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        public Trans2GetDfsReferralRequest(int messageId, int sessionId, int treeId)
            : base(messageId, Command.TransGetDfsReferral)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
        }
    }

    #region Message Structures for SMB_COM_NT_TRANSACTION

    /// <summary>
    /// NT_TRANSACT_RENAME Client Request.
    /// </summary>
    public class NtTransactRenameRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// It indicates whether the new name or link will replace the original one that exists already.
        /// </summary>
        public bool replaceEnable;

        /// <summary>
        /// The name that the file is being renamed to.
        /// </summary>
        public string newName;

        /// <summary>
        /// NT_TRANSACT_RENAME Client Request.
        /// </summary>
        /// <param name="messageId">Message ID is used to identify the message.</param>
        /// <param name="sessionId">Session ID is used to identify the session.</param>
        /// <param name="treeId">Tree ID is used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the file to rename.</param>
        /// <param name="replaceEnable">
        /// It indicates whether the rename operation must replace the traget file if the target name exists.
        /// </param>
        /// <param name="newName">The name that the file is being renamed to.</param>
        public NtTransactRenameRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool replaceEnable,
            string newName, bool isSigned)
            : base(messageId, Command.NtTransactRename)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.replaceEnable = replaceEnable;
            this.newName = newName;
            this.isSigned = isSigned;
        }
    }


    /// <summary>
    /// NT_TRANSACT_QUERY_QUOTA Client Request.
    /// </summary>
    public class NtTransactQueryQuotaRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// isSigned
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// A bool variable.
        /// if set, indicates only a single entry is to be returned instead of filling the entire buffer.
        /// </summary>
        public bool returnSingle;

        /// <summary>
        /// A bool variable.
        /// if set, indicates that the scan of the quota information is to be restarted.
        /// </summary>
        public bool restartScan;

        /// <summary>
        /// Supply the length in bytes of the SidList, or 0 if there is no SidList.
        /// </summary>
        public int sidListLength;

        /// <summary>
        /// Supply the length in bytes of the StartSid, or 0 if there is no StartSid.
        /// </summary>
        public int startSidLength;

        /// <summary>
        /// NT_TRANSACT_QUERY_QUOTA Client Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="returnSingle">
        /// A bool variable. 
        /// if set, indicates only a single entry is to be returned instead of filling the entire buffer.
        /// </param>
        /// <param name="restartScan">
        /// A bool variable.
        /// if set, indicates that the scan of the quota information is to be restarted.
        /// </param>
        public NtTransactQueryQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            bool returnSingle,
            bool restartScan,
            int sidListLength,
            int startSidLength)
            : base(messageId, Command.NtTransactQueryQuota)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.isSigned = isSigned;
            this.returnSingle = returnSingle;
            this.restartScan = restartScan;
            this.sidListLength = sidListLength;
            this.startSidLength = startSidLength;
        }
    }


    /// <summary>
    /// NT_TRANSACT_SET_QUOTA Client Request.
    /// </summary>
    public class NtTransactSetQuotaRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// The amount of quota, in bytes, used by this user
        /// </summary>
        public int quotaUsed;

        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Client Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="quotaUsed">The amount of quota, in bytes, used by this user.</param>
        public NtTransactSetQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            int quotaUsed)
            : base(messageId, Command.NtTransactSetQuota)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.isSigned = isSigned;
            this.quotaUsed = quotaUsed;
        }
    }


    /// <summary>
    /// NT_TRANSACT_SET_QUOTA Client Request.
    /// </summary>
    public class NTTransactSetQuotaRequestAdditional : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// NT_TRANSACT_SET_QUOTA request parameter.
        /// </summary>
        public NtTransSetQuotaRequestParameter requestPara;

        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Client Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="requestPara">NT_TRANSACT_SET_QUOTA request parameter.</param>
        public NTTransactSetQuotaRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            NtTransSetQuotaRequestParameter requestPara)
            : base(messageId, Command.NtTransactSetQuotaAdditional)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.isSigned = isSigned;
            this.requestPara = requestPara;
        }
    }


    /// <summary>
    /// FSCTL_SRV_ENUMERATE_SNAPSHOTS Request.
    /// </summary>
    public class FsctlSrvEnumSnapshotsRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// Control the MaxDataCount in FSCTL_SRV_ENUMERATE_SNAPSHOTS.
        /// </summary>
        public MaxDataCount maxDataCount;

        /// <summary>
        /// FSCTL_SRV_ENUMERATE_SNAPSHOTS Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="maxDataCount">Control the MaxDataCount in FSCTL_SRV_ENUMERATE_SNAPSHOTS.</param>
        public FsctlSrvEnumSnapshotsRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            MaxDataCount maxDataCount)
            : base(messageId, Command.FsctlSrvEnumerateSnapshots)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.isSigned = isSigned;
            this.maxDataCount = maxDataCount;
        }
    }


    /// <summary>
    /// FSCTL_SRV_REQUEST_RESUME_KEY Request.
    /// </summary>
    public class FsctlSrvResumeKeyRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// FSCTL_SRV_REQUEST_RESUME_KEY Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        public FsctlSrvResumeKeyRequest(int messageId, int sessionId, int treeId, int fId, bool isSigned)
            : base(messageId, Command.FsctlSrvRequestResumeKey)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.isSigned = isSigned;
        }
    }


    /// <summary>
    /// NT_TRANSACT_IOCTL Client Request.
    /// </summary>
    public class FsctlSrvCopyChunkRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The SMB file identifier of the target directory.
        /// </summary>
        public int fId;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// The number of bytes to copy from the source file to the target file.
        /// </summary>
        public int length;

        /// <summary>
        /// The the SUT resume key for a source file.
        /// </summary>
        public string copychunkResumeKey;

        /// <summary>
        /// NT_TRANSACT_IOCTL Client Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fId">The target file identifier.</param>
        /// <param name="copychunkResumeKey">The the SUT resume key for a source file.</param>
        /// <param name="length">The number of bytes to copy from the source file to the target file.</param>
        public FsctlSrvCopyChunkRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            int length,
            string copychunkResumeKey)
            : base(messageId, Command.FsctlSrvCupychunk)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fId = fId;
            this.isSigned = isSigned;
            this.length = length;
            this.copychunkResumeKey = copychunkResumeKey;
        }
    }


    /// <summary>
    /// Invalid FSCTL command request.
    /// </summary>
    public class FSCTLBadCommandRequest : SmbRequest
    {
        /// <summary>
        /// Valid command.
        /// </summary>
        public FsctlInvalidCommand invalidCommand;

        /// <summary>
        /// Invalid FSCTL command request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="invalidCommand">Invaild command.</param>
        public FSCTLBadCommandRequest(int messageId, FsctlInvalidCommand invalidCommand)
            : base(messageId, Command.InvalidFsctlCommand)
        {
            this.messageId = messageId;
            this.invalidCommand = invalidCommand;
        }
    }

    /// <summary>
    /// NT_TRANSACT_CREATE request.
    /// </summary>
    public class NtTransactCreateRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </summary>
        public int impersonationLevel;

        /// <summary>
        /// This is used to represent the name of the resource.
        /// </summary>
        public string fileName;

        /// <summary>
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;

        /// <summary>
        /// It indicates whether the SUT has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// NT_TRANSACT_CREATE request.
        /// </summary>
        /// <param name="messageId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        /// <param name="sessionId"></param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="impersonationLevel">
        /// This field specifies the information given to the SUT about the client and how the SUT MUST represent,
        /// or impersonate the client.
        /// </param>
        /// <param name="fileName">This is used to represent the name of the resource.</param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        /// <param name="isSigned">It indicates whether the SUT has message signing enabled or required.</param>
        public NtTransactCreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            int impersonationLevel,
            string fileName,
            ShareType shareType,
            bool isSigned)
            : base(messageId, Command.NtTransactCreate)
        {
            this.messageId = messageId;
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.impersonationLevel = impersonationLevel;
            this.fileName = fileName;
            this.shareType = shareType;
            this.isSigned = isSigned;
        }
    }

    /// <summary>
    /// NT_TRANSACT_IOCTL Client Request
    /// </summary>
    public class FSCTLNameRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session identifier to request reauthenticate a existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This field identifies the subdirectory (or tree) (also referred to as a share in this document) on the server that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// The file identifier.
        /// </summary>
        public int fid;

        /// <summary>
        /// Indicates whether the server has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// Indicates the FSCTL name
        /// </summary>
        public FSCCFSCTLName fsctlName;

        /// <summary>
        /// NT_TRANSACT_IOCTL Client Request
        /// </summary>
        /// <param name="messageId"> Message ID used to identify the message.</param>
        /// <param name="sessionId"> Session ID used to identify the session.</param>
        /// <param name="treeId"> Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
        /// <param name="fid"> The target file identifier. </param>
        /// <param name="copychunkResumeKey">The server resume key for a source file.</param>
        /// <param name="length"> The number of bytes to copy from the source file to the target file.</param>
        public FSCTLNameRequest(int messageId, int sessionId, int treeId, int fid, bool isSigned, FSCCFSCTLName fsctlName)
            : base(messageId, Command.FSCC_FSCTL_NAME)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.fid = fid;
            this.isSigned = isSigned;
            this.fsctlName = fsctlName;
        }
    }

    /// <summary>
    /// FSCC TRANS2_QUERY_PATH_INFORMATION Request
    /// </summary>
    public class FSCCTrans2QueryPathInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session identifier to request reauthenticate a existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This field identifies the subdirectory (or tree) (also referred to as a share in this document) on the server that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// isSigned
        /// Indicates whether the server has message signing enabled or required.
        /// </summary>
        public bool isSigned;


        /// <summary>
        /// This can be used to query information from the server.
        /// </summary>
        public FSCCTransaction2QueryPathInforLevel informationLevel;


        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION Request
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> Set this value to 0 to request a new session setup, or set this value to a 
        /// previously established session identifier to reauthenticate to an existing session. </param>
        /// <param name="treeId"> This field identifies the subdirectory (or tree) (also referred to as a share 
        /// in this document) on the server that the client is accessing. </param>
        /// <param name="isSigned"> Indicates whether the server has message signing enabled or required.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action</param>
        /// <param name="isUsePassthrough"> Indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of 
        /// the request.</param>
        /// <param name="isReparse">Indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the 
        /// SMB header.</param>
        /// <param name="informationLevel"> This can be used to query information from the server.</param>
        public FSCCTrans2QueryPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            FSCCTransaction2QueryPathInforLevel informationLevel)
            : base(messageId, Command.FSCCTRANS2_QUERY_PATH_INFORMATION)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isSigned = isSigned;
            this.informationLevel = informationLevel;
        }
    }

    /// <summary>
    /// TRANS2_QUERY_FS_INFORMATION Request
    /// </summary>
    public class FSCCTrans2QueryFSInfoRequest : SmbRequest
    {
        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session identifier to request reauthenticate a existing session.
        /// </summary>
        public int sessionId;

        /// <summary>
        /// This field identifies the subdirectory (or tree) (also referred to as a share in this document) on the server that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// Indicates whether the server has message signing enabled or required.
        /// </summary>
        public bool isSigned;

        /// <summary>
        /// This can be used to query information from the server.
        /// </summary>
        public FSCCTransaction2QueryFSInforLevel informationLevel;

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Request
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> Set this value to 0 to request a new session setup, or set this value to a previously established session identifier to reauthenticate to an existing session. </param>
        /// <param name="treeId"> This field identifies the subdirectory (or tree) (also referred to as a share in this document) on the server that the client is accessing. </param>
        /// <param name="isSigned"> Indicates whether the server has message signing enabled or required.</param>
        /// <param name="isUsePassthrough"> isUsePassthrough </param>
        /// <param name="informationLevel"> This can be used to query information from the server.</param>
        public FSCCTrans2QueryFSInfoRequest(int messageId, int sessionId, int treeId, bool isSigned, FSCCTransaction2QueryFSInforLevel informationLevel)
            : base(messageId, Command.FSCCTRANS2_QUERY_FS_INFORMATION)
        {
            this.sessionId = sessionId;
            this.treeId = treeId;
            this.isSigned = isSigned;
            this.informationLevel = informationLevel;
        }
    }

    #endregion

    #endregion
}
