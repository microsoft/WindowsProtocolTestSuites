// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// NET_API_STATUS<para/>
    /// defined at http://msdn.microsoft.com/en-us/library/aa370674(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NetApiStatus : uint
    {
        /// <summary>
        /// Success
        /// </summary>
        NERR_Success = 0,

        /// <summary>
        /// The workstation driver is not installed. 
        /// </summary>
        NERR_NetNotStarted = 2102,

        /// <summary>
        /// The server could not be located. 
        /// </summary>
        NERR_UnknownServer = 2103,

        /// <summary>
        /// An internal error occurred. The network cannot access a shared memory segment. 
        /// </summary>
        NERR_ShareMem = 2104,

        /// <summary>
        /// A network resource shortage occurred . 
        /// </summary>
        NERR_NoNetworkResource = 2105,

        /// <summary>
        /// This operation is not supported on workstations. 
        /// </summary>
        NERR_RemoteOnly = 2106,

        /// <summary>
        /// The device is not connected. 
        /// </summary>
        NERR_DevNotRedirected = 2107,

        /// <summary>
        /// The Server service is not started. 
        /// </summary>
        NERR_ServerNotStarted = 2114,

        /// <summary>
        /// The queue is empty. 
        /// </summary>
        NERR_ItemNotFound = 2115,

        /// <summary>
        /// The device or directory does not exist. 
        /// </summary>
        NERR_UnknownDevDir = 2116,

        /// <summary>
        /// The operation is invalid on a redirected resource. 
        /// </summary>
        NERR_RedirectedPath = 2117,

        /// <summary>
        /// The name has already been shared. 
        /// </summary>
        NERR_DuplicateShare = 2118,

        /// <summary>
        /// The server is currently out of the requested resource. 
        /// </summary>
        NERR_NoRoom = 2119,

        /// <summary>
        /// Requested addition of items exceeds the maximum allowed. 
        /// </summary>
        NERR_TooManyItems = 2121,

        /// <summary>
        /// The Peer service supports only two simultaneous users. 
        /// </summary>
        NERR_InvalidMaxUsers = 2122,

        /// <summary>
        /// The API return buffer is too small. 
        /// </summary>
        NERR_BufTooSmall = 2123,

        /// <summary>
        /// A remote API error occurred.  
        /// </summary>
        NERR_RemoteErr = 2127,

        /// <summary>
        /// An error occurred when opening or reading the configuration file. 
        /// </summary>
        NERR_LanmanIniError = 2131,

        /// <summary>
        /// A general network error occurred. 
        /// </summary>
        NERR_NetworkError = 2136,

        /// <summary>
        /// The Workstation service is in an inconsistent state. Restart the computer before restarting the Workstation service. 
        /// </summary>
        NERR_WkstaInconsistentState = 2137,

        /// <summary>
        /// The Workstation service has not been started. 
        /// </summary>
        NERR_WkstaNotStarted = 2138,

        /// <summary>
        /// The requested information is not available. 
        /// </summary>
        NERR_BrowserNotStarted = 2139,

        /// <summary>
        /// An internal Windows error occurred.
        /// </summary>
        NERR_InternalError = 2140,

        /// <summary>
        /// The server is not configured for transactions. 
        /// </summary>
        NERR_BadTransactConfig = 2141,

        /// <summary>
        /// The requested API is not supported on the remote server. 
        /// </summary>
        NERR_InvalidAPI = 2142,

        /// <summary>
        /// The event name is invalid. 
        /// </summary>
        NERR_BadEventName = 2143,

        /// <summary>
        /// The computer name already exists on the network. Change it and restart the computer. 
        /// </summary>
        NERR_DupNameReboot = 2144,

        /// <summary>
        /// The specified component could not be found in the configuration information. 
        /// </summary>
        NERR_CfgCompNotFound = 2146,

        /// <summary>
        /// The specified parameter could not be found in the configuration information. 
        /// </summary>
        NERR_CfgParamNotFound = 2147,

        /// <summary>
        /// A line in the configuration file is too long. 
        /// </summary>
        NERR_LineTooLong = 2149,

        /// <summary>
        /// The printer does not exist. 
        /// </summary>
        NERR_QNotFound = 2150,

        /// <summary>
        /// The print job does not exist. 
        /// </summary>
        NERR_JobNotFound = 2151,

        /// <summary>
        /// The printer destination cannot be found. 
        /// </summary>
        NERR_DestNotFound = 2152,

        /// <summary>
        /// The printer destination already exists. 
        /// </summary>
        NERR_DestExists = 2153,

        /// <summary>
        /// The printer queue already exists. 
        /// </summary>
        NERR_QExists = 2154,

        /// <summary>
        /// No more printers can be added. 
        /// </summary>
        NERR_QNoRoom = 2155,

        /// <summary>
        /// No more print jobs can be added.  
        /// </summary>
        NERR_JobNoRoom = 2156,

        /// <summary>
        /// No more printer destinations can be added. 
        /// </summary>
        NERR_DestNoRoom = 2157,

        /// <summary>
        /// This printer destination is idle and cannot accept control operations. 
        /// </summary>
        NERR_DestIdle = 2158,

        /// <summary>
        /// This printer destination request contains an invalid control function. 
        /// </summary>
        NERR_DestInvalidOp = 2159,

        /// <summary>
        /// The print processor is not responding. 
        /// </summary>
        NERR_ProcNoRespond = 2160,

        /// <summary>
        /// The spooler is not running. 
        /// </summary>
        NERR_SpoolerNotLoaded = 2161,

        /// <summary>
        /// This operation cannot be performed on the print destination in its current state. 
        /// </summary>
        NERR_DestInvalidState = 2162,

        /// <summary>
        /// This operation cannot be performed on the printer queue in its current state. 
        /// </summary>
        NERR_QInvalidState = 2163,

        /// <summary>
        /// This operation cannot be performed on the print job in its current state. 
        /// </summary>
        NERR_JobInvalidState = 2164,

        /// <summary>
        /// A spooler memory allocation failure occurred. 
        /// </summary>
        NERR_SpoolNoMemory = 2165,

        /// <summary>
        /// The device driver does not exist. 
        /// </summary>
        NERR_DriverNotFound = 2166,

        /// <summary>
        /// The data type is not supported by the print processor. 
        /// </summary>
        NERR_DataTypeInvalid = 2167,

        /// <summary>
        /// The print processor is not installed. 
        /// </summary>
        NERR_ProcNotFound = 2168,

        /// <summary>
        /// The service database is locked. 
        /// </summary>
        NERR_ServiceTableLocked = 2180,

        /// <summary>
        /// The service table is full. 
        /// </summary>
        NERR_ServiceTableFull = 2181,

        /// <summary>
        /// The requested service has already been started. 
        /// </summary>
        NERR_ServiceInstalled = 2182,

        /// <summary>
        /// The service does not respond to control actions. 
        /// </summary>
        NERR_ServiceEntryLocked = 2183,

        /// <summary>
        /// The service has not been started. 
        /// </summary>
        NERR_ServiceNotInstalled = 2184,

        /// <summary>
        /// The service name is invalid. 
        /// </summary>
        NERR_BadServiceName = 2185,

        /// <summary>
        /// The service is not responding to the control function. 
        /// </summary>
        NERR_ServiceCtlTimeout = 2186,

        /// <summary>
        /// The service control is busy. 
        /// </summary>
        NERR_ServiceCtlBusy = 2187,

        /// <summary>
        /// The configuration file contains an invalid service program name. 
        /// </summary>
        NERR_BadServiceProgName = 2188,

        /// <summary>
        /// The service could not be controlled in its present state. 
        /// </summary>
        NERR_ServiceNotCtrl = 2189,

        /// <summary>
        /// The service ended abnormally. 
        /// </summary>
        NERR_ServiceKillProc = 2190,

        /// <summary>
        /// The requested pause, continue, or stop is not valid for this service. 
        /// </summary>
        NERR_ServiceCtlNotValid = 2191,

        /// <summary>
        /// The service control dispatcher could not find the service name in the dispatch table. 
        /// </summary>
        NERR_NotInDispatchTbl = 2192,

        /// <summary>
        /// The service control dispatcher pipe read failed. 
        /// </summary>
        NERR_BadControlRecv = 2193,

        /// <summary>
        /// A thread for the new service could not be created. 
        /// </summary>
        NERR_ServiceNotStarting = 2194,

        /// <summary>
        /// This workstation is already logged on to the local-area network. 
        /// </summary>
        NERR_AlreadyLoggedOn = 2200,

        /// <summary>
        /// The workstation is not logged on to the local-area network. 
        /// </summary>
        NERR_NotLoggedOn = 2201,

        /// <summary>
        /// The user name or group name parameter is invalid.  
        /// </summary>
        NERR_BadUsername = 2202,

        /// <summary>
        /// The password parameter is invalid. 
        /// </summary>
        NERR_BadPassword = 2203,

        /// <summary>
        /// @W The logon processor did not add the message alias. 
        /// </summary>
        NERR_UnableToAddName_W = 2204,

        /// <summary>
        /// The logon processor did not add the message alias. 
        /// </summary>
        NERR_UnableToAddName_F = 2205,

        /// <summary>
        /// @W The logoff processor did not delete the message alias. 
        /// </summary>
        NERR_UnableToDelName_W = 2206,

        /// <summary>
        /// The logoff processor did not delete the message alias. 
        /// </summary>
        NERR_UnableToDelName_F = 2207,

        /// <summary>
        /// Network logons are paused. 
        /// </summary>
        NERR_LogonsPaused = 2209,

        /// <summary>
        /// A centralized logon-server conflict occurred.
        /// </summary>
        NERR_LogonServerConflict = 2210,
        
        /// <summary>
        /// The server is configured without a valid user path. 
        /// </summary>
        NERR_LogonNoUserPath = 2211,

        /// <summary>
        /// An error occurred while loading or running the logon script. 
        /// </summary>
        NERR_LogonScriptError = 2212,

        /// <summary>
        /// The logon server was not specified.  Your computer will be logged on as STANDALONE. 
        /// </summary>
        NERR_StandaloneLogon = 2214,

        /// <summary>
        /// The logon server could not be found.  
        /// </summary>
        NERR_LogonServerNotFound = 2215,

        /// <summary>
        /// There is already a logon domain for this computer.  
        /// </summary>
        NERR_LogonDomainExists = 2216,

        /// <summary>
        /// The logon server could not validate the logon. 
        /// </summary>
        NERR_NonValidatedLogon = 2217,

        /// <summary>
        /// The security database could not be found. 
        /// </summary>
        NERR_ACFNotFound = 2219,

        /// <summary>
        /// The group name could not be found. 
        /// </summary>
        NERR_GroupNotFound = 2220,

        /// <summary>
        /// The user name could not be found. 
        /// </summary>
        NERR_UserNotFound = 2221,

        /// <summary>
        /// The resource name could not be found.  
        /// </summary>
        NERR_ResourceNotFound = 2222,

        /// <summary>
        /// The group already exists. 
        /// </summary>
        NERR_GroupExists = 2223,

        /// <summary>
        /// The account already exists. 
        /// </summary>
        NERR_UserExists = 2224,

        /// <summary>
        /// The resource permission list already exists. 
        /// </summary>
        NERR_ResourceExists = 2225,

        /// <summary>
        /// This operation is only allowed on the primary domain controller of the domain. 
        /// </summary>
        NERR_NotPrimary = 2226,

        /// <summary>
        /// The security database has not been started. 
        /// </summary>
        NERR_ACFNotLoaded = 2227,

        /// <summary>
        /// There are too many names in the user accounts database. 
        /// </summary>
        NERR_ACFNoRoom = 2228,

        /// <summary>
        /// A disk I/O failure occurred.
        /// </summary>
        NERR_ACFFileIOFail = 2229,

        /// <summary>
        /// The limit of 64 entries per resource was exceeded. 
        /// </summary>
        NERR_ACFTooManyLists = 2230,

        /// <summary>
        /// Deleting a user with a session is not allowed. 
        /// </summary>
        NERR_UserLogon = 2231,

        /// <summary>
        /// The parent directory could not be located. 
        /// </summary>
        NERR_ACFNoParent = 2232,

        /// <summary>
        /// Unable to add to the security database session cache segment. 
        /// </summary>
        NERR_CanNotGrowSegment = 2233,

        /// <summary>
        /// This operation is not allowed on this special group. 
        /// </summary>
        NERR_SpeGroupOp = 2234,

        /// <summary>
        /// This user is not cached in user accounts database session cache. 
        /// </summary>
        NERR_NotInCache = 2235,

        /// <summary>
        /// The user already belongs to this group. 
        /// </summary>
        NERR_UserInGroup = 2236,

        /// <summary>
        /// The user does not belong to this group. 
        /// </summary>
        NERR_UserNotInGroup = 2237,

        /// <summary>
        /// This user account is undefined. 
        /// </summary>
        NERR_AccountUndefined = 2238,

        /// <summary>
        /// This user account has expired. 
        /// </summary>
        NERR_AccountExpired = 2239,

        /// <summary>
        /// The user is not allowed to log on from this workstation. 
        /// </summary>
        NERR_InvalidWorkstation = 2240,

        /// <summary>
        /// The user is not allowed to log on at this time.  
        /// </summary>
        NERR_InvalidLogonHours = 2241,

        /// <summary>
        /// The password of this user has expired. 
        /// </summary>
        NERR_PasswordExpired = 2242,

        /// <summary>
        /// The password of this user cannot change. 
        /// </summary>
        NERR_PasswordCantChange = 2243,

        /// <summary>
        /// This password cannot be used now. 
        /// </summary>
        NERR_PasswordHistConflict = 2244,

        /// <summary>
        /// The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements. 
        /// </summary>
        NERR_PasswordTooShort = 2245,

        /// <summary>
        /// The password of this user is too recent to change.  
        /// </summary>
        NERR_PasswordTooRecent = 2246,

        /// <summary>
        /// The security database is corrupted. 
        /// </summary>
        NERR_InvalidDatabase = 2247,

        /// <summary>
        /// No updates are necessary to this replicate network/local security database. 
        /// </summary>
        NERR_DatabaseUpToDate = 2248,

        /// <summary>
        /// This replicate database is outdated; synchronization is required. 
        /// </summary>
        NERR_SyncRequired = 2249,

        /// <summary>
        /// The network connection could not be found. 
        /// </summary>
        NERR_UseNotFound = 2250,

        /// <summary>
        /// This asg_type is invalid. 
        /// </summary>
        NERR_BadAsgType = 2251,

        /// <summary>
        /// This device is currently being shared. 
        /// </summary>
        NERR_DeviceIsShared = 2252,

        /// <summary>
        /// The user name may not be same as computer name. 
        /// </summary>
        NERR_SameAsComputerName = 2253,

        /// <summary>
        /// The computer name could not be added as a message alias. The name may already exist on the network. 
        /// </summary>
        NERR_NoComputerName = 2270,

        /// <summary>
        /// The Messenger service is already started. 
        /// </summary>
        NERR_MsgAlreadyStarted = 2271,

        /// <summary>
        /// The Messenger service failed to start.  
        /// </summary>
        NERR_MsgInitFailed = 2272,

        /// <summary>
        /// The message alias could not be found on the network. 
        /// </summary>
        NERR_NameNotFound = 2273,

        /// <summary>
        /// This message alias has already been forwarded. 
        /// </summary>
        NERR_AlreadyForwarded = 2274,

        /// <summary>
        /// This message alias has been added but is still forwarded. 
        /// </summary>
        NERR_AddForwarded = 2275,

        /// <summary>
        /// This message alias already exists locally. 
        /// </summary>
        NERR_AlreadyExists = 2276,

        /// <summary>
        /// The maximum number of added message aliases has been exceeded. 
        /// </summary>
        NERR_TooManyNames = 2277,

        /// <summary>
        /// The computer name could not be deleted.
        /// </summary>
        NERR_DelComputerName = 2278,

        /// <summary>
        /// Messages cannot be forwarded back to the same workstation. 
        /// </summary>
        NERR_LocalForward = 2279,

        /// <summary>
        /// An error occurred in the domain message processor. 
        /// </summary>
        NERR_GrpMsgProcessor = 2280,

        /// <summary>
        /// The message was sent, but the recipient has paused the Messenger service. 
        /// </summary>
        NERR_PausedRemote = 2281,

        /// <summary>
        /// The message was sent but not received. 
        /// </summary>
        NERR_BadReceive = 2282,

        /// <summary>
        /// The message alias is currently in use. Try again later. 
        /// </summary>
        NERR_NameInUse = 2283,

        /// <summary>
        /// The Messenger service has not been started. 
        /// </summary>
        NERR_MsgNotStarted = 2284,

        /// <summary>
        /// The name is not on the local computer. 
        /// </summary>
        NERR_NotLocalName = 2285,

        /// <summary>
        /// The forwarded message alias could not be found on the network. 
        /// </summary>
        NERR_NoForwardName = 2286,

        /// <summary>
        /// The message alias table on the remote station is full. 
        /// </summary>
        NERR_RemoteFull = 2287,

        /// <summary>
        /// Messages for this alias are not currently being forwarded. 
        /// </summary>
        NERR_NameNotForwarded = 2288,

        /// <summary>
        /// The broadcast message was truncated. 
        /// </summary>
        NERR_TruncatedBroadcast = 2289,

        /// <summary>
        /// This is an invalid device name. 
        /// </summary>
        NERR_InvalidDevice = 2294,

        /// <summary>
        /// A write fault occurred. 
        /// </summary>
        NERR_WriteFault = 2295,

        /// <summary>
        /// A duplicate message alias exists on the network. 
        /// </summary>
        NERR_DuplicateName = 2297,

        /// <summary>
        /// This message alias will be deleted later. 
        /// </summary>
        NERR_DeleteLater = 2298,

        /// <summary>
        /// The message alias was not successfully deleted from all networks. 
        /// </summary>
        NERR_IncompleteDel = 2299,

        /// <summary>
        /// This operation is not supported on computers with multiple networks. 
        /// </summary>
        NERR_MultipleNets = 2300,

        /// <summary>
        /// This shared resource does not exist.
        /// </summary>
        NERR_NetNameNotFound = 2310,

        /// <summary>
        /// This device is not shared. 
        /// </summary>
        NERR_DeviceNotShared = 2311,

        /// <summary>
        /// A session does not exist with that computer name. 
        /// </summary>
        NERR_ClientNameNotFound = 2312,

        /// <summary>
        /// There is not an open file with that identification number. 
        /// </summary>
        NERR_FileIdNotFound = 2314,

        /// <summary>
        /// A failure occurred when executing a remote administration command. 
        /// </summary>
        NERR_ExecFailure = 2315,

        /// <summary>
        /// A failure occurred when opening a remote temporary file. 
        /// </summary>
        NERR_TmpFile = 2316,

        /// <summary>
        /// The data returned from a remote administration command has been truncated to 64K. 
        /// </summary>
        NERR_TooMuchData = 2317,

        /// <summary>
        /// This device cannot be shared as both a spooled and a non-spooled resource. 
        /// </summary>
        NERR_DeviceShareConflict = 2318,

        /// <summary>
        /// The information in the list of servers may be incorrect. 
        /// </summary>
        NERR_BrowserTableIncomplete = 2319,

        /// <summary>
        /// The computer is not active in this domain. 
        /// </summary>
        NERR_NotLocalDomain = 2320,

        /// <summary>
        /// The share must be removed from the Distributed File System before it can be deleted. 
        /// </summary>
        NERR_IsDfsShare = 2321,

        /// <summary>
        /// The operation is invalid for this device. 
        /// </summary>
        NERR_DevInvalidOpCode = 2331,

        /// <summary>
        /// This device cannot be shared. 
        /// </summary>
        NERR_DevNotFound = 2332,

        /// <summary>
        /// This device was not open. 
        /// </summary>
        NERR_DevNotOpen = 2333,

        /// <summary>
        /// This device name list is invalid. 
        /// </summary>
        NERR_BadQueueDevString = 2334,

        /// <summary>
        /// The queue priority is invalid. 
        /// </summary>
        NERR_BadQueuePriority = 2335,

        /// <summary>
        /// There are no shared communication devices. 
        /// </summary>
        NERR_NoCommDevs = 2337,

        /// <summary>
        /// The queue you specified does not exist. 
        /// </summary>
        NERR_QueueNotFound = 2338,

        /// <summary>
        /// This list of devices is invalid. 
        /// </summary>
        NERR_BadDevString = 2340,

        /// <summary>
        /// The requested device is invalid. 
        /// </summary>
        NERR_BadDev = 2341,

        /// <summary>
        /// This device is already in use by the spooler. 
        /// </summary>
        NERR_InUseBySpooler = 2342,

        /// <summary>
        /// This device is already in use as a communication device. 
        /// </summary>
        NERR_CommDevInUse = 2343,

        /// <summary>
        /// This computer name is invalid. 
        /// </summary>
        NERR_InvalidComputer = 2351,

        /// <summary>
        /// The string and prefix specified are too long. 
        /// </summary>
        NERR_MaxLenExceeded = 2354,

        /// <summary>
        /// This path component is invalid. 
        /// </summary>
        NERR_BadComponent = 2356,

        /// <summary>
        /// Could not determine the type of input. 
        /// </summary>
        NERR_CantType = 2357,

        /// <summary>
        /// The buffer for types is not big enough. 
        /// </summary>
        NERR_TooManyEntries = 2362,

        /// <summary>
        /// Profile files cannot exceed 64K. 
        /// </summary>
        NERR_ProfileFileTooBig = 2370,

        /// <summary>
        /// The start offset is out of range. 
        /// </summary>
        NERR_ProfileOffset = 2371,

        /// <summary>
        /// The system cannot delete current connections to network resources. 
        /// </summary>
        NERR_ProfileCleanup = 2372,

        /// <summary>
        /// The system was unable to parse the command line in this file.
        /// </summary>
        NERR_ProfileUnknownCmd = 2373,

        /// <summary>
        /// An error occurred while loading the profile file. 
        /// </summary>
        NERR_ProfileLoadErr = 2374,

        /// <summary>
        /// Errors occurred while saving the profile file. The profile was partially saved. 
        /// </summary>
        NERR_ProfileSaveErr = 2375,

        /// <summary>
        /// Log file is full. 
        /// </summary>
        NERR_LogOverflow = 2377,

        /// <summary>
        /// This log file has changed between reads. 
        /// </summary>
        NERR_LogFileChanged = 2378,

        /// <summary>
        /// Log file %1 is corrupt. 
        /// </summary>
        NERR_LogFileCorrupt = 2379,

        /// <summary>
        /// The source path cannot be a directory. 
        /// </summary>
        NERR_SourceIsDir = 2380,

        /// <summary>
        /// The source path is illegal. 
        /// </summary>
        NERR_BadSource = 2381,

        /// <summary>
        /// The destination path is illegal. 
        /// </summary>
        NERR_BadDest = 2382,

        /// <summary>
        /// The source and destination paths are on different servers. 
        /// </summary>
        NERR_DifferentServers = 2383,

        /// <summary>
        /// The Run server you requested is paused. 
        /// </summary>
        NERR_RunSrvPaused = 2385,

        /// <summary>
        /// An error occurred when communicating with a Run server. 
        /// </summary>
        NERR_ErrCommRunSrv = 2389,

        /// <summary>
        /// An error occurred when starting a background process. 
        /// </summary>
        NERR_ErrorExecingGhost = 2391,

        /// <summary>
        /// The shared resource you are connected to could not be found.
        /// </summary>
        NERR_ShareNotFound = 2392,

        /// <summary>
        /// The LAN adapter number is invalid.  
        /// </summary>
        NERR_InvalidLana = 2400,

        /// <summary>
        /// There are open files on the connection.    
        /// </summary>
        NERR_OpenFiles = 2401,

        /// <summary>
        /// Active connections still exist. 
        /// </summary>
        NERR_ActiveConns = 2402,

        /// <summary>
        /// This share name or password is invalid. 
        /// </summary>
        NERR_BadPasswordCore = 2403,

        /// <summary>
        /// The device is being accessed by an active process. 
        /// </summary>
        NERR_DevInUse = 2404,

        /// <summary>
        /// The drive letter is in use locally. 
        /// </summary>
        NERR_LocalDrive = 2405,

        /// <summary>
        /// The specified client is already registered for the specified event. 
        /// </summary>
        NERR_AlertExists = 2430,

        /// <summary>
        /// The alert table is full. 
        /// </summary>
        NERR_TooManyAlerts = 2431,

        /// <summary>
        /// An invalid or nonexistent alert name was raised. 
        /// </summary>
        NERR_NoSuchAlert = 2432,

        /// <summary>
        /// The alert recipient is invalid.
        /// </summary>
        NERR_BadRecipient = 2433,

        /// <summary>
        /// A user's session with this server has been deleted because the user's logon hours are no longer valid. 
        /// </summary>
        NERR_AcctLimitExceeded = 2434,

        /// <summary>
        /// The log file does not contain the requested record number. 
        /// </summary>
        NERR_InvalidLogSeek = 2440,

        /// <summary>
        /// The user accounts database is not configured correctly. 
        /// </summary>
        NERR_BadUasConfig = 2450,

        /// <summary>
        /// This operation is not permitted when the Netlogon service is running. 
        /// </summary>
        NERR_InvalidUASOp = 2451,

        /// <summary>
        /// This operation is not allowed on the last administrative account. 
        /// </summary>
        NERR_LastAdmin = 2452,

        /// <summary>
        /// Could not find domain controller for this domain. 
        /// </summary>
        NERR_DCNotFound = 2453,

        /// <summary>
        /// Could not set logon information for this user. 
        /// </summary>
        NERR_LogonTrackingError = 2454,

        /// <summary>
        /// The Netlogon service has not been started. 
        /// </summary>
        NERR_NetlogonNotStarted = 2455,

        /// <summary>
        /// Unable to add to the user accounts database. 
        /// </summary>
        NERR_CanNotGrowUASFile = 2456,

        /// <summary>
        /// This server's clock is not synchronized with the primary domain controller's clock. 
        /// </summary>
        NERR_TimeDiffAtDC = 2457,

        /// <summary>
        /// A password mismatch has been detected. 
        /// </summary>
        NERR_PasswordMismatch = 2458,

        /// <summary>
        /// The server identification does not specify a valid server. 
        /// </summary>
        NERR_NoSuchServer = 2460,

        /// <summary>
        /// The session identification does not specify a valid session. 
        /// </summary>
        NERR_NoSuchSession = 2461,

        /// <summary>
        /// The connection identification does not specify a valid connection. 
        /// </summary>
        NERR_NoSuchConnection = 2462,

        /// <summary>
        /// There is no space for another entry in the table of available servers. 
        /// </summary>
        NERR_TooManyServers = 2463,

        /// <summary>
        /// The server has reached the maximum number of sessions it supports. 
        /// </summary>
        NERR_TooManySessions = 2464,

        /// <summary>
        /// The server has reached the maximum number of connections it supports. 
        /// </summary>
        NERR_TooManyConnections = 2465,

        /// <summary>
        /// The server cannot open more files because it has reached its maximum number. 
        /// </summary>
        NERR_TooManyFiles = 2466,

        /// <summary>
        /// There are no alternate servers registered on this server. 
        /// </summary>
        NERR_NoAlternateServers = 2467,

        /// <summary>
        /// Try down-level (remote admin protocol) version of API instead. 
        /// </summary>
        NERR_TryDownLevel = 2470,

        /// <summary>
        /// The UPS driver could not be accessed by the UPS service. 
        /// </summary>
        NERR_UPSDriverNotStarted = 2480,

        /// <summary>
        /// The UPS service is not configured correctly. 
        /// </summary>
        NERR_UPSInvalidConfig = 2481,

        /// <summary>
        /// The UPS service could not access the specified Comm Port. 
        /// </summary>
        NERR_UPSInvalidCommPort = 2482,

        /// <summary>
        /// The UPS indicated a line fail or low battery situation. Service not started. 
        /// </summary>
        NERR_UPSSignalAsserted = 2483,

        /// <summary>
        /// The UPS service failed to perform a system shut down. 
        /// </summary>
        NERR_UPSShutdownFailed = 2484,

        /// <summary>
        /// The program below returned an MS-DOS error code:
        /// </summary>
        NERR_BadDosRetCode = 2500,

        /// <summary>
        /// The program below needs more memory:
        /// </summary>
        NERR_ProgNeedsExtraMem = 2501,

        /// <summary>
        /// The program below called an unsupported MS-DOS function:
        /// </summary>
        NERR_BadDosFunction = 2502,

        /// <summary>
        /// The workstation failed to boot.
        /// </summary>
        NERR_RemoteBootFailed = 2503,

        /// <summary>
        /// The file below is corrupt.
        /// </summary>
        NERR_BadFileCheckSum = 2504,

        /// <summary>
        /// No loader is specified in the boot-block definition file.
        /// </summary>
        NERR_NoRplBootSystem = 2505,

        /// <summary>
        /// NetBIOS returned an error: The NCB and SMB are dumped above.
        /// </summary>
        NERR_RplLoadrNetBiosErr = 2506,

        /// <summary>
        /// A disk I/O error occurred.
        /// </summary>
        NERR_RplLoadrDiskErr = 2507,

        /// <summary>
        /// Image parameter substitution failed.
        /// </summary>
        NERR_ImageParamErr = 2508,

        /// <summary>
        /// Too many image parameters cross disk sector boundaries.
        /// </summary>
        NERR_TooManyImageParams = 2509,

        /// <summary>
        /// The image was not generated from an MS-DOS diskette formatted with /S.
        /// </summary>
        NERR_NonDosFloppyUsed = 2510,

        /// <summary>
        /// Remote boot will be restarted later.
        /// </summary>
        NERR_RplBootRestart = 2511,

        /// <summary>
        /// The call to the Remoteboot server failed.
        /// </summary>
        NERR_RplSrvrCallFailed = 2512,

        /// <summary>
        /// Cannot connect to the Remoteboot server.
        /// </summary>
        NERR_CantConnectRplSrvr = 2513,

        /// <summary>
        /// Cannot open image file on the Remoteboot server.
        /// </summary>
        NERR_CantOpenImageFile = 2514,

        /// <summary>
        /// Connecting to the Remoteboot server...
        /// </summary>
        NERR_CallingRplSrvr = 2515,

        /// <summary>
        /// Connecting to the Remoteboot server...
        /// </summary>
        NERR_StartingRplBoot = 2516,

        /// <summary>
        /// Remote boot service was stopped; check the error log for the cause of the problem.
        /// </summary>
        NERR_RplBootServiceTerm = 2517,

        /// <summary>
        /// Remote boot startup failed; check the error log for the cause of the problem.
        /// </summary>
        NERR_RplBootStartFailed = 2518,

        /// <summary>
        /// A second connection to a Remoteboot resource is not allowed.
        /// </summary>
        NERR_RPL_CONNECTED = 2519,

        /// <summary>
        /// The browser service was configured with MaintainServerList=No. 
        /// </summary>
        NERR_BrowserConfiguredToNotRun = 2550,

        /// <summary>
        /// Service failed to start since none of the network adapters started with this service.
        /// </summary>
        NERR_RplNoAdaptersStarted = 2610,

        /// <summary>
        /// Service failed to start due to bad startup information in the registry.
        /// </summary>
        NERR_RplBadRegistry = 2611,

        /// <summary>
        /// Service failed to start because its database is absent or corrupt.
        /// </summary>
        NERR_RplBadDatabase = 2612,

        /// <summary>
        /// Service failed to start because RPLFILES share is absent.
        /// </summary>
        NERR_RplRplfilesShare = 2613,

        /// <summary>
        ///Service failed to start because RPLUSER group is absent.
        /// </summary>
        NERR_RplNotRplServer = 2614,

        /// <summary>
        /// Cannot enumerate service records.
        /// </summary>
        NERR_RplCannotEnum = 2615,

        /// <summary>
        /// Workstation record information has been corrupted.
        /// </summary>
        NERR_RplWkstaInfoCorrupted = 2616,

        /// <summary>
        /// Workstation record was not found.
        /// </summary>
        NERR_RplWkstaNotFound = 2617,

        /// <summary>
        /// Workstation name is in use by some other workstation.
        /// </summary>
        NERR_RplWkstaNameUnavailable = 2618,

        /// <summary>
        /// Profile record information has been corrupted.
        /// </summary>
        NERR_RplProfileInfoCorrupted = 2619,

        /// <summary>
        /// Profile record was not found.
        /// </summary>
        NERR_RplProfileNotFound = 2620,

        /// <summary>
        /// Profile name is in use by some other profile.
        /// </summary>
        NERR_RplProfileNameUnavailable = 2621,

        /// <summary>
        /// There are workstations using this profile.
        /// </summary>
        NERR_RplProfileNotEmpty = 2622,

        /// <summary>
        /// Configuration record information has been corrupted.
        /// </summary>
        NERR_RplConfigInfoCorrupted = 2623,

        /// <summary>
        /// Configuration record was not found.
        /// </summary>
        NERR_RplConfigNotFound = 2624,

        /// <summary>
        /// Adapter id record information has been corrupted.
        /// </summary>
        NERR_RplAdapterInfoCorrupted = 2625,

        /// <summary>
        /// An internal service error has occurred.
        /// </summary>
        NERR_RplInternal = 2626,

        /// <summary>
        /// Vendor id record information has been corrupted.
        /// </summary>
        NERR_RplVendorInfoCorrupted = 2627,

        /// <summary>
        /// Boot block record information has been corrupted.
        /// </summary>
        NERR_RplBootInfoCorrupted = 2628,

        /// <summary>
        /// The user account for this workstation record is missing.
        /// </summary>
        NERR_RplWkstaNeedsUserAcct = 2629,

        /// <summary>
        /// The RPLUSER local group could not be found.
        /// </summary>
        NERR_RplNeedsRPLUSERAcct = 2630,

        /// <summary>
        /// Boot block record was not found.
        /// </summary>
        NERR_RplBootNotFound = 2631,

        /// <summary>
        /// Chosen profile is incompatible with this workstation.
        /// </summary>
        NERR_RplIncompatibleProfile = 2632,

        /// <summary>
        /// Chosen network adapter id is in use by some other workstation.
        /// </summary>
        NERR_RplAdapterNameUnavailable = 2633,

        /// <summary>
        /// There are profiles using this configuration.
        /// </summary>
        NERR_RplConfigNotEmpty = 2634,

        /// <summary>
        /// There are workstations, profiles or configurations using this boot block.
        /// </summary>
        NERR_RplBootInUse = 2635,

        /// <summary>
        /// Service failed to backup Remoteboot database.
        /// </summary>
        NERR_RplBackupDatabase = 2636,

        /// <summary>
        /// Adapter record was not found.
        /// </summary>
        NERR_RplAdapterNotFound = 2637,

        /// <summary>
        /// Vendor record was not found.
        /// </summary>
        NERR_RplVendorNotFound = 2638,

        /// <summary>
        /// Vendor name is in use by some other vendor record.
        /// </summary>
        NERR_RplVendorNameUnavailable = 2639,

        /// <summary>
        /// (boot name, vendor id) is in use by some other boot block record.
        /// </summary>
        NERR_RplBootNameUnavailable = 2640,

        /// <summary>
        /// Configuration name is in use by some other configuration.
        /// </summary>
        NERR_RplConfigNameUnavailable = 2641,

        /// <summary>
        /// The internal database maintained by the DFS service is corrupt
        /// </summary>
        NERR_DfsInternalCorruption = 2660,

        /// <summary>
        /// One of the records in the internal DFS database is corrupt
        /// </summary>
        NERR_DfsVolumeDataCorrupt = 2661,

        /// <summary>
        /// There is no DFS name whose entry path matches the input Entry Path
        /// </summary>
        NERR_DfsNoSuchVolume = 2662,

        /// <summary>
        /// A root or link with the given name already exists
        /// </summary>
        NERR_DfsVolumeAlreadyExists = 2663,

        /// <summary>
        /// The server share specified is already shared in the DFS
        /// </summary>
        NERR_DfsAlreadyShared = 2664,

        /// <summary>
        /// The indicated server share does not support the indicated DFS namespace
        /// </summary>
        NERR_DfsNoSuchShare = 2665,

        /// <summary>
        /// The operation is not valid on this portion of the namespace
        /// </summary>
        NERR_DfsNotALeafVolume = 2666,

        /// <summary>
        /// The operation is not valid on this portion of the namespace
        /// </summary>
        NERR_DfsLeafVolume = 2667,

        /// <summary>
        /// The operation is ambiguous because the link has multiple servers
        /// </summary>
        NERR_DfsVolumeHasMultipleServers = 2668,

        /// <summary>
        /// Unable to create a link
        /// </summary>
        NERR_DfsCantCreateJunctionPoint = 2669,

        /// <summary>
        /// The server is not DFS Aware
        /// </summary>
        NERR_DfsServerNotDfsAware = 2670,

        /// <summary>
        /// The specified rename target path is invalid
        /// </summary>
        NERR_DfsBadRenamePath = 2671,

        /// <summary>
        /// The specified DFS link is offline
        /// </summary>
        NERR_DfsVolumeIsOffline = 2672,

        /// <summary>
        /// The specified server is not a server for this link
        /// </summary>
        NERR_DfsNoSuchServer = 2673,

        /// <summary>
        /// A cycle in the DFS name was detected
        /// </summary>
        NERR_DfsCyclicalName = 2674,

        /// <summary>
        /// The operation is not supported on a server-based DFS
        /// </summary>
        NERR_DfsNotSupportedInServerDfs = 2675,

        /// <summary>
        /// This link is already supported by the specified server-share
        /// </summary>
        NERR_DfsDuplicateService = 2676,

        /// <summary>
        /// Can't remove the last server-share supporting this root or link
        /// </summary>
        NERR_DfsCantRemoveLastServerShare = 2677,

        /// <summary>
        /// The operation is not supported for an Inter-DFS link
        /// </summary>
        NERR_DfsVolumeIsInterDfs = 2678,

        /// <summary>
        /// The internal state of the DFS Service has become inconsistent
        /// </summary>
        NERR_DfsInconsistent = 2679,

        /// <summary>
        /// The DFS Service has been installed on the specified server
        /// </summary>
        NERR_DfsServerUpgraded = 2680,

        /// <summary>
        /// The DFS data being reconciled is identical
        /// </summary>
        NERR_DfsDataIsIdentical = 2681,

        /// <summary>
        /// The DFS root cannot be deleted - Uninstall DFS if required
        /// </summary>
        NERR_DfsCantRemoveDfsRoot = 2682,

        /// <summary>
        /// A child or parent directory of the share is already in a DFS
        /// </summary>
        NERR_DfsChildOrParentInDfs = 2683,

        /// <summary>
        /// DFS internal error
        /// </summary>
        NERR_DfsInternalError = 2690,

        /// <summary>
        /// This machine is already joined to a domain.
        /// </summary>
        NERR_SetupAlreadyJoined = 2691,

        /// <summary>
        /// This machine is not currently joined to a domain.
        /// </summary>
        NERR_SetupNotJoined = 2692,

        /// <summary>
        /// This machine is a domain controller and cannot be unjoined from a domain.
        /// </summary>
        NERR_SetupDomainController = 2693,

        /// <summary>
        /// The destination domain controller does not support creating machine accounts in OUs.
        /// </summary>
        NERR_DefaultJoinRequired = 2694,

        /// <summary>
        /// The specified workgroup name is invalid.
        /// </summary>
        NERR_InvalidWorkgroupName = 2695,

        /// <summary>
        /// The specified computer name is incompatible with the default language used on the domain controller.
        /// </summary>
        NERR_NameUsesIncompatibleCodePage = 2696,

        /// <summary>
        /// The specified computer account could not be found. Contact an administrator to verify the account is in the domain. If the account has been deleted unjoin, reboot, and rejoin the domain.
        /// </summary>
        NERR_ComputerAccountNotFound = 2697,

        /// <summary>
        /// This version of Windows cannot be joined to a domain.
        /// </summary>
        NERR_PersonalSku = 2698,

        /// <summary>
        /// An attempt to resolve the DNS name of a domain controller in the domain being joined has failed.  Please verify this client is configured to reach a DNS server that can resolve DNS names in the target domain. For information about network troubleshooting, see Windows Help.
        /// </summary>
        NERR_SetupCheckDNSConfig = 2699,

        /// <summary>
        /// Password must change at next logon 
        /// </summary>
        NERR_PasswordMustChange = 2701,

        /// <summary>
        /// Account is locked out 
        /// </summary>
        NERR_AccountLockedOut = 2702,

        /// <summary>
        /// Password is too long 
        /// </summary>
        NERR_PasswordTooLong = 2703,

        /// <summary>
        /// Password doesn't meet the complexity policy */#define NERR_PasswordFilterError           (NERR_BASE + 605)   /* Password doesn't meet the requirements of the filter dll's 
        /// </summary>
        NERR_PasswordNotComplexEnough = 2704,

        /// <summary>
        /// Offline join completion information was not found. 
        /// </summary>
        NERR_NoOfflineJoinInfo = 2709,

        /// <summary>
        /// The offline join completion information was bad. 
        /// </summary>
        NERR_BadOfflineJoinInfo = 2710,

        /// <summary>
        /// Unable to create offline join information. Please ensure you have access to the specified path location and permissions to modify its contents. Running as an elevated administrator may be required. */#define NERR_BadDomainJoinInfo             (NERR_BASE + 612)   /* The domain join info being saved was incomplete or bad. */#define NERR_JoinPerformedMustRestart      (NERR_BASE + 613)   /* Offline join operation successfully completed but a restart is needed. */#define NERR_NoJoinPending                 (NERR_BASE + 614)   /* There was no offline join operation pending. */#define NERR_ValuesNotSet                  (NERR_BASE + 615)   /* Unable to set one or more requested machine or domain name values on the local computer. 
        /// </summary>
        NERR_CantCreateJoinInfo = 2711,

        /// <summary>
        /// Could not verify the current machine's hostname against the saved value in the join completion information. 
        /// </summary>
        NERR_CantVerifyHostname = 2716,

        /// <summary>
        /// Unable to load the specified offline registry hive. Please ensure you have access to the specified path location and permissions to modify its contents. Running as an elevated administrator may be required. 
        /// </summary>
        NERR_CantLoadOfflineHive = 2717,

        /// <summary>
        /// The minimum session security requirements for this operation were not met. 
        /// </summary>
        NERR_ConnectionInsecure = 2718,

        /// <summary>
        /// Computer account provisioning blob version is not supported. 
        /// </summary>
        NERR_ProvisioningBlobUnsupported = 2719,
          
    }
}
