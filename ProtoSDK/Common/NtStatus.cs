// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// NTSTATUS
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NtStatus : uint
    {
        /// <summary>
        /// The success status codes 0 - 63 are reserved for wait completion status.
        /// FacilityCodes 0x5 - 0xF have been allocated by various drivers.<para/>
        /// STATUS_WAIT_0 is re-using this value.
        /// </summary>
        STATUS_SUCCESS = 0x00000000,

        /// <summary>
        /// MessageId: STATUS_WAIT_1
        /// MessageText:
        ///  STATUS_WAIT_1
        /// </summary>
        STATUS_WAIT_1 = 0x00000001,

        /// <summary>
        /// MessageId: STATUS_WAIT_2
        /// MessageText:
        ///  STATUS_WAIT_2
        /// </summary>
        STATUS_WAIT_2 = 0x00000002,

        /// <summary>
        /// MessageId: STATUS_WAIT_3
        /// MessageText:
        ///  STATUS_WAIT_3
        /// </summary>
        STATUS_WAIT_3 = 0x00000003,

        /// <summary>
        /// MessageId: STATUS_WAIT_63
        /// MessageText:
        ///  STATUS_WAIT_63
        /// </summary>
        STATUS_WAIT_63 = 0x0000003F,

        /// <summary>
        /// The success status codes 128 - 191 are reserved for wait completion
        /// status with an abandoned mutant object.
        /// </summary>
        STATUS_ABANDONED = 0x00000080,

        /// <summary>
        /// MessageId: STATUS_ABANDONED_WAIT_0
        /// MessageText:
        ///  STATUS_ABANDONED_WAIT_0
        /// </summary>
        STATUS_ABANDONED_WAIT_0 = 0x00000080,

        /// <summary>
        /// MessageId: STATUS_ABANDONED_WAIT_63
        /// MessageText:
        ///  STATUS_ABANDONED_WAIT_63
        /// </summary>
        STATUS_ABANDONED_WAIT_63 = 0x000000BF,

        /// <summary>
        /// The success status codes 256, 257, 258, and 258 are reserved for
        /// User APC, Kernel APC, Alerted, and Timeout.
        /// MessageId: STATUS_USER_APC
        /// MessageText:
        ///  STATUS_USER_APC
        /// </summary>
        STATUS_USER_APC = 0x000000C0,

        /// <summary>
        /// MessageId: STATUS_KERNEL_APC
        /// MessageText:
        ///  STATUS_KERNEL_APC
        /// </summary>
        STATUS_KERNEL_APC = 0x00000100,

        /// <summary>
        /// MessageId: STATUS_ALERTED
        /// MessageText:
        ///  STATUS_ALERTED
        /// </summary>
        STATUS_ALERTED = 0x00000101,

        /// <summary>
        /// MessageId: STATUS_TIMEOUT
        /// MessageText:
        ///  STATUS_TIMEOUT
        /// </summary>
        STATUS_TIMEOUT = 0x00000102,

        /// <summary>
        /// MessageId: STATUS_PENDING
        /// MessageText:
        /// The operation that was requested is pending completion.
        /// </summary>
        STATUS_PENDING = 0x00000103,

        /// <summary>
        /// MessageId: STATUS_REPARSE
        /// MessageText:
        /// A reparse should be performed by the Object Manager since the name of the file resulted in a symbolic link.
        /// </summary>
        STATUS_REPARSE = 0x00000104,

        /// <summary>
        /// MessageId: STATUS_MORE_ENTRIES
        /// MessageText:
        /// Returned by enumeration APIs to indicate more information is available to successive calls.
        /// </summary>
        STATUS_MORE_ENTRIES = 0x00000105,

        /// <summary>
        /// MessageId: STATUS_NOT_ALL_ASSIGNED
        /// MessageText:
        /// Indicates not all privileges or groups referenced are assigned to the caller.
        /// This allows, for example, all privileges to be disabled without having to know exactly which privileges are assigned.
        /// </summary>
        STATUS_NOT_ALL_ASSIGNED = 0x00000106,

        /// <summary>
        /// MessageId: STATUS_SOME_NOT_MAPPED
        /// MessageText:
        /// Some of the information to be translated has not been translated.
        /// </summary>
        STATUS_SOME_NOT_MAPPED = 0x00000107,

        /// <summary>
        /// MessageId: STATUS_OPLOCK_BREAK_IN_PROGRESS
        /// MessageText:
        /// An open/create operation completed while an oplock break is underway.
        /// </summary>
        STATUS_OPLOCK_BREAK_IN_PROGRESS = 0x00000108,

        /// <summary>
        /// MessageId: STATUS_VOLUME_MOUNTED
        /// MessageText:
        /// A new volume has been mounted by a file system.
        /// </summary>
        STATUS_VOLUME_MOUNTED = 0x00000109,

        /// <summary>
        /// MessageId: STATUS_RXACT_COMMITTED
        /// MessageText:
        /// This success level status indicates that the transaction state already exists for the registry sub-tree, but that a transaction commit was previously aborted.
        /// The commit has now been completed.
        /// </summary>
        STATUS_RXACT_COMMITTED = 0x0000010A,

        /// <summary>
        /// MessageId: STATUS_NOTIFY_CLEANUP
        /// MessageText:
        /// This indicates that a notify change request has been completed due to closing the handle which made the notify change request.
        /// </summary>
        STATUS_NOTIFY_CLEANUP = 0x0000010B,

        /// <summary>
        /// MessageId: STATUS_NOTIFY_ENUM_DIR
        /// MessageText:
        /// This indicates that a notify change request is being completed and that the information is not being returned in the caller's buffer.
        /// The caller now needs to enumerate the files to find the changes.
        /// </summary>
        STATUS_NOTIFY_ENUM_DIR = 0x0000010C,

        /// <summary>
        /// MessageId: STATUS_NO_QUOTAS_FOR_ACCOUNT
        /// MessageText:
        /// {No Quotas}
        /// No system quota limits are specifically set for this account.
        /// </summary>
        STATUS_NO_QUOTAS_FOR_ACCOUNT = 0x0000010D,

        /// <summary>
        /// MessageId: STATUS_PRIMARY_TRANSPORT_CONNECT_FAILED
        /// MessageText:
        /// {Connect Failure on Primary Transport}
        /// An attempt was made to connect to the remote server %hs on the primary transport, but the connection failed.
        /// The computer WAS able to connect on a secondary transport.
        /// </summary>
        STATUS_PRIMARY_TRANSPORT_CONNECT_FAILED = 0x0000010E,

        /// <summary>
        /// MessageId: STATUS_PAGE_FAULT_TRANSITION
        /// MessageText:
        /// Page fault was a transition fault.
        /// </summary>
        STATUS_PAGE_FAULT_TRANSITION = 0x00000110,

        /// <summary>
        /// MessageId: STATUS_PAGE_FAULT_DEMAND_ZERO
        /// MessageText:
        /// Page fault was a demand zero fault.
        /// </summary>
        STATUS_PAGE_FAULT_DEMAND_ZERO = 0x00000111,

        /// <summary>
        /// MessageId: STATUS_PAGE_FAULT_COPY_ON_WRITE
        /// MessageText:
        /// Page fault was a demand zero fault.
        /// </summary>
        STATUS_PAGE_FAULT_COPY_ON_WRITE = 0x00000112,

        /// <summary>
        /// MessageId: STATUS_PAGE_FAULT_GUARD_PAGE
        /// MessageText:
        /// Page fault was a demand zero fault.
        /// </summary>
        STATUS_PAGE_FAULT_GUARD_PAGE = 0x00000113,

        /// <summary>
        /// MessageId: STATUS_PAGE_FAULT_PAGING_FILE
        /// MessageText:
        /// Page fault was satisfied by reading from a secondary storage device.
        /// </summary>
        STATUS_PAGE_FAULT_PAGING_FILE = 0x00000114,

        /// <summary>
        /// MessageId: STATUS_CACHE_PAGE_LOCKED
        /// MessageText:
        /// Cached page was locked during operation.
        /// </summary>
        STATUS_CACHE_PAGE_LOCKED = 0x00000115,

        /// <summary>
        /// MessageId: STATUS_CRASH_DUMP
        /// MessageText:
        /// Crash dump exists in paging file.
        /// </summary>
        STATUS_CRASH_DUMP = 0x00000116,

        /// <summary>
        /// MessageId: STATUS_BUFFER_ALL_ZEROS
        /// MessageText:
        /// Specified buffer contains all zeros.
        /// </summary>
        STATUS_BUFFER_ALL_ZEROS = 0x00000117,

        /// <summary>
        /// MessageId: STATUS_REPARSE_OBJECT
        /// MessageText:
        /// A reparse should be performed by the Object Manager since the name of the file resulted in a symbolic link.
        /// </summary>
        STATUS_REPARSE_OBJECT = 0x00000118,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_REQUIREMENTS_CHANGED
        /// MessageText:
        /// The device has succeeded a query-stop and its resource requirements have changed.
        /// </summary>
        STATUS_RESOURCE_REQUIREMENTS_CHANGED = 0x00000119,

        /// <summary>
        /// MessageId: STATUS_TRANSLATION_COMPLETE
        /// MessageText:
        /// The translator has translated these resources into the global space and no further translations should be performed.
        /// </summary>
        STATUS_TRANSLATION_COMPLETE = 0x00000120,

        /// <summary>
        /// MessageId: STATUS_DS_MEMBERSHIP_EVALUATED_LOCALLY
        /// MessageText:
        /// The directory service evaluated group memberships locally, as it was unable to contact a global catalog server.
        /// </summary>
        STATUS_DS_MEMBERSHIP_EVALUATED_LOCALLY = 0x00000121,

        /// <summary>
        /// MessageId: STATUS_NOTHING_TO_TERMINATE
        /// MessageText:
        /// A process being terminated has no threads to terminate.
        /// </summary>
        STATUS_NOTHING_TO_TERMINATE = 0x00000122,

        /// <summary>
        /// MessageId: STATUS_PROCESS_NOT_IN_JOB
        /// MessageText:
        /// The specified process is not part of a job.
        /// </summary>
        STATUS_PROCESS_NOT_IN_JOB = 0x00000123,

        /// <summary>
        /// MessageId: STATUS_PROCESS_IN_JOB
        /// MessageText:
        /// The specified process is part of a job.
        /// </summary>
        STATUS_PROCESS_IN_JOB = 0x00000124,

        /// <summary>
        /// MessageId: STATUS_VOLSNAP_HIBERNATE_READY
        /// MessageText:
        /// {Volume Shadow Copy Service}
        /// The system is now ready for hibernation.
        /// </summary>
        STATUS_VOLSNAP_HIBERNATE_READY = 0x00000125,

        /// <summary>
        /// MessageId: STATUS_FSFILTER_OP_COMPLETED_SUCCESSFULLY
        /// MessageText:
        /// A file system or file system filter driver has successfully completed an FsFilter operation.
        /// </summary>
        STATUS_FSFILTER_OP_COMPLETED_SUCCESSFULLY = 0x00000126,

        /// <summary>
        /// MessageId: STATUS_INTERRUPT_VECTOR_ALREADY_CONNECTED
        /// MessageText:
        /// The specified interrupt vector was already connected.
        /// </summary>
        STATUS_INTERRUPT_VECTOR_ALREADY_CONNECTED = 0x00000127,

        /// <summary>
        /// MessageId: STATUS_INTERRUPT_STILL_CONNECTED
        /// MessageText:
        /// The specified interrupt vector is still connected.
        /// </summary>
        STATUS_INTERRUPT_STILL_CONNECTED = 0x00000128,

        /// <summary>
        /// MessageId: STATUS_PROCESS_CLONED
        /// MessageText:
        /// The current process is a cloned process.
        /// </summary>
        STATUS_PROCESS_CLONED = 0x00000129,

        /// <summary>
        /// MessageId: STATUS_FILE_LOCKED_WITH_ONLY_READERS
        /// MessageText:
        /// The file was locked and all users of the file can only read.
        /// </summary>
        STATUS_FILE_LOCKED_WITH_ONLY_READERS = 0x0000012A,

        /// <summary>
        /// MessageId: STATUS_FILE_LOCKED_WITH_WRITERS
        /// MessageText:
        /// The file was locked and at least one user of the file can write.
        /// </summary>
        STATUS_FILE_LOCKED_WITH_WRITERS = 0x0000012B,

        /// <summary>
        /// MessageId: STATUS_RESOURCEMANAGER_READ_ONLY
        /// MessageText:
        /// The specified ResourceManager made no changes or updates to the resource under this transaction.
        /// </summary>
        STATUS_RESOURCEMANAGER_READ_ONLY = 0x00000202,

        /// <summary>
        /// MessageId: DBG_EXCEPTION_HANDLED
        /// MessageText:
        /// Debugger handled exception
        /// </summary>
        DBG_EXCEPTION_HANDLED = 0x00010001,

        /// <summary>
        /// MessageId: DBG_CONTINUE
        /// MessageText:
        /// Debugger continued
        /// </summary>
        DBG_CONTINUE = 0x00010002,

        /// <summary>
        /// MessageId: STATUS_FLT_IO_COMPLETE
        /// MessageText:
        /// The IO was completed by a filter.
        /// </summary>
        STATUS_FLT_IO_COMPLETE = 0x001C0001,

        /// <summary>
        /// MessageId: STATUS_OBJECT_NAME_EXISTS
        /// MessageText:
        /// {Object Exists}
        /// An attempt was made to create an object and the object name already existed.
        /// </summary>
        STATUS_OBJECT_NAME_EXISTS = 0x40000000,

        /// <summary>
        /// MessageId: STATUS_THREAD_WAS_SUSPENDED
        /// MessageText:
        /// {Thread Suspended}
        /// A thread termination occurred while the thread was suspended. The thread was resumed, and termination proceeded.
        /// </summary>
        STATUS_THREAD_WAS_SUSPENDED = 0x40000001,

        /// <summary>
        /// MessageId: STATUS_WORKING_SET_LIMIT_RANGE
        /// MessageText:
        /// {Working Set Range Error}
        /// An attempt was made to set the working set minimum or maximum to values which are outside of the allowable range.
        /// </summary>
        STATUS_WORKING_SET_LIMIT_RANGE = 0x40000002,

        /// <summary>
        /// MessageId: STATUS_IMAGE_NOT_AT_BASE
        /// MessageText:
        /// {Image Relocated}
        /// An image file could not be mapped at the address specified in the image file. Local fixups must be performed on this image.
        /// </summary>
        STATUS_IMAGE_NOT_AT_BASE = 0x40000003,

        /// <summary>
        /// MessageId: STATUS_RXACT_STATE_CREATED
        /// MessageText:
        /// This informational level status indicates that a specified registry sub-tree transaction state did not yet exist and had to be created.
        /// </summary>
        STATUS_RXACT_STATE_CREATED = 0x40000004,

        /// <summary>
        /// MessageId: STATUS_SEGMENT_NOTIFICATION
        /// MessageText:
        /// {Segment Load}
        /// A virtual DOS machine (VDM) is loading, unloading, or moving an MS-DOS or Win16 program segment image.
        /// An exception is raised so a debugger can load, unload or track symbols and breakpoints within these 16-bit segments.
        /// </summary>
        STATUS_SEGMENT_NOTIFICATION = 0x40000005,

        /// <summary>
        /// MessageId: STATUS_LOCAL_USER_SESSION_KEY
        /// MessageText:
        /// {Local Session Key}
        /// A user session key was requested for a local RPC connection. The session key returned is a constant value and not unique to this connection.
        /// </summary>
        STATUS_LOCAL_USER_SESSION_KEY = 0x40000006,

        /// <summary>
        /// MessageId: STATUS_BAD_CURRENT_DIRECTORY
        /// MessageText:
        /// {Invalid Current Directory}
        /// The process cannot switch to the startup current directory %hs.
        /// Select OK to set current directory to %hs, or select CANCEL to exit.
        /// </summary>
        STATUS_BAD_CURRENT_DIRECTORY = 0x40000007,

        /// <summary>
        /// MessageId: STATUS_SERIAL_MORE_WRITES
        /// MessageText:
        /// {Serial IOCTL Complete}
        /// A serial I/O operation was completed by another write to a serial port.
        /// (The IOCTL_SERIAL_XOFF_COUNTER reached zero.)
        /// </summary>
        STATUS_SERIAL_MORE_WRITES = 0x40000008,

        /// <summary>
        /// MessageId: STATUS_REGISTRY_RECOVERED
        /// MessageText:
        /// {Registry Recovery}
        /// One of the files containing the system's Registry data had to be recovered by use of a log or alternate copy.
        /// The recovery was successful.
        /// </summary>
        STATUS_REGISTRY_RECOVERED = 0x40000009,

        /// <summary>
        /// MessageId: STATUS_FT_READ_RECOVERY_FROM_BACKUP
        /// MessageText:
        /// {Redundant Read}
        /// To satisfy a read request, the NT fault-tolerant file system successfully read the requested data from a redundant copy.
        /// This was done because the file system encountered a failure on a member of the fault-tolerant volume, but was unable to reassign the failing area of the device.
        /// </summary>
        STATUS_FT_READ_RECOVERY_FROM_BACKUP = 0x4000000A,

        /// <summary>
        /// MessageId: STATUS_FT_WRITE_RECOVERY
        /// MessageText:
        /// {Redundant Write}
        /// To satisfy a write request, the NT fault-tolerant file system successfully wrote a redundant copy of the information.
        /// This was done because the file system encountered a failure on a member of the fault-tolerant volume, but was not able to reassign the failing area of the device.
        /// </summary>
        STATUS_FT_WRITE_RECOVERY = 0x4000000B,

        /// <summary>
        /// MessageId: STATUS_SERIAL_COUNTER_TIMEOUT
        /// MessageText:
        /// {Serial IOCTL Timeout}
        /// A serial I/O operation completed because the time-out period expired.
        /// (The IOCTL_SERIAL_XOFF_COUNTER had not reached zero.)
        /// </summary>
        STATUS_SERIAL_COUNTER_TIMEOUT = 0x4000000C,

        /// <summary>
        /// MessageId: STATUS_NULL_LM_PASSWORD
        /// MessageText:
        /// {Password Too Complex}
        /// The Windows password is too complex to be converted to a LAN Manager password.
        /// The LAN Manager password returned is a NULL string.
        /// </summary>
        STATUS_NULL_LM_PASSWORD = 0x4000000D,

        /// <summary>
        /// MessageId: STATUS_IMAGE_MACHINE_TYPE_MISMATCH
        /// MessageText:
        /// {Machine Type Mismatch}
        /// The image file %hs is valid, but is for a machine type other than the current machine. Select OK to continue, or CANCEL to fail the DLL load.
        /// </summary>
        STATUS_IMAGE_MACHINE_TYPE_MISMATCH = 0x4000000E,

        /// <summary>
        /// MessageId: STATUS_RECEIVE_PARTIAL
        /// MessageText:
        /// {Partial Data Received}
        /// The network transport returned partial data to its client. The remaining data will be sent later.
        /// </summary>
        STATUS_RECEIVE_PARTIAL = 0x4000000F,

        /// <summary>
        /// MessageId: STATUS_RECEIVE_EXPEDITED
        /// MessageText:
        /// {Expedited Data Received}
        /// The network transport returned data to its client that was marked as expedited by the remote system.
        /// </summary>
        STATUS_RECEIVE_EXPEDITED = 0x40000010,

        /// <summary>
        /// MessageId: STATUS_RECEIVE_PARTIAL_EXPEDITED
        /// MessageText:
        /// {Partial Expedited Data Received}
        /// The network transport returned partial data to its client and this data was marked as expedited by the remote system. The remaining data will be sent later.
        /// </summary>
        STATUS_RECEIVE_PARTIAL_EXPEDITED = 0x40000011,

        /// <summary>
        /// MessageId: STATUS_EVENT_DONE
        /// MessageText:
        /// {TDI Event Done}
        /// The TDI indication has completed successfully.
        /// </summary>
        STATUS_EVENT_DONE = 0x40000012,

        /// <summary>
        /// MessageId: STATUS_EVENT_PENDING
        /// MessageText:
        /// {TDI Event Pending}
        /// The TDI indication has entered the pending state.
        /// </summary>
        STATUS_EVENT_PENDING = 0x40000013,

        /// <summary>
        /// MessageId: STATUS_CHECKING_FILE_SYSTEM
        /// MessageText:
        /// Checking file system on %wZ
        /// </summary>
        STATUS_CHECKING_FILE_SYSTEM = 0x40000014,

        /// <summary>
        /// MessageId: STATUS_FATAL_APP_EXIT
        /// MessageText:
        /// {Fatal Application Exit}
        /// %hs
        /// </summary>
        STATUS_FATAL_APP_EXIT = 0x40000015,

        /// <summary>
        /// MessageId: STATUS_PREDEFINED_HANDLE
        /// MessageText:
        /// The specified registry key is referenced by a predefined handle.
        /// </summary>
        STATUS_PREDEFINED_HANDLE = 0x40000016,

        /// <summary>
        /// MessageId: STATUS_WAS_UNLOCKED
        /// MessageText:
        /// {Page Unlocked}
        /// The page protection of a locked page was changed to 'No Access' and the page was unlocked from memory and from the process.
        /// </summary>
        STATUS_WAS_UNLOCKED = 0x40000017,

        /// <summary>
        /// MessageId: STATUS_SERVICE_NOTIFICATION
        /// MessageText:
        /// %hs
        /// </summary>
        STATUS_SERVICE_NOTIFICATION = 0x40000018,

        /// <summary>
        /// MessageId: STATUS_WAS_LOCKED
        /// MessageText:
        /// {Page Locked}
        /// One of the pages to lock was already locked.
        /// </summary>
        STATUS_WAS_LOCKED = 0x40000019,

        /// <summary>
        /// MessageId: STATUS_LOG_HARD_ERROR
        /// MessageText:
        /// Application popup: %1 : %2
        /// </summary>
        STATUS_LOG_HARD_ERROR = 0x4000001A,

        /// <summary>
        /// MessageId: STATUS_ALREADY_WIN32
        /// MessageText:
        ///  STATUS_ALREADY_WIN32
        /// </summary>
        STATUS_ALREADY_WIN32 = 0x4000001B,

        /// <summary>
        /// MessageId: STATUS_WX86_UNSIMULATE
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_UNSIMULATE = 0x4000001C,

        /// <summary>
        /// MessageId: STATUS_WX86_CONTINUE
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_CONTINUE = 0x4000001D,

        /// <summary>
        /// MessageId: STATUS_WX86_SINGLE_STEP
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_SINGLE_STEP = 0x4000001E,

        /// <summary>
        /// MessageId: STATUS_WX86_BREAKPOINT
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_BREAKPOINT = 0x4000001F,

        /// <summary>
        /// MessageId: STATUS_WX86_EXCEPTION_CONTINUE
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_EXCEPTION_CONTINUE = 0x40000020,

        /// <summary>
        /// MessageId: STATUS_WX86_EXCEPTION_LASTCHANCE
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_EXCEPTION_LASTCHANCE = 0x40000021,

        /// <summary>
        /// MessageId: STATUS_WX86_EXCEPTION_CHAIN
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_EXCEPTION_CHAIN = 0x40000022,

        /// <summary>
        /// MessageId: STATUS_IMAGE_MACHINE_TYPE_MISMATCH_EXE
        /// MessageText:
        /// {Machine Type Mismatch}
        /// The image file %hs is valid, but is for a machine type other than the current machine.
        /// </summary>
        STATUS_IMAGE_MACHINE_TYPE_MISMATCH_EXE = 0x40000023,

        /// <summary>
        /// MessageId: STATUS_NO_YIELD_PERFORMED
        /// MessageText:
        /// A yield execution was performed and no thread was available to run.
        /// </summary>
        STATUS_NO_YIELD_PERFORMED = 0x40000024,

        /// <summary>
        /// MessageId: STATUS_TIMER_RESUME_IGNORED
        /// MessageText:
        /// The resumable flag to a timer API was ignored.
        /// </summary>
        STATUS_TIMER_RESUME_IGNORED = 0x40000025,

        /// <summary>
        /// MessageId: STATUS_ARBITRATION_UNHANDLED
        /// MessageText:
        /// The arbiter has deferred arbitration of these resources to its parent
        /// </summary>
        STATUS_ARBITRATION_UNHANDLED = 0x40000026,

        /// <summary>
        /// MessageId: STATUS_CARDBUS_NOT_SUPPORTED
        /// MessageText:
        /// The device "%hs" has detected a CardBus card in its slot, but the firmware on this system is not configured to allow the CardBus controller to be run in CardBus mode.
        /// The operating system will currently accept only 16-bit (R2) pc-cards on this controller.
        /// </summary>
        STATUS_CARDBUS_NOT_SUPPORTED = 0x40000027,

        /// <summary>
        /// MessageId: STATUS_WX86_CREATEWX86TIB
        /// MessageText:
        /// Exception status code used by Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_CREATEWX86TIB = 0x40000028,

        /// <summary>
        /// MessageId: STATUS_MP_PROCESSOR_MISMATCH
        /// MessageText:
        /// The CPUs in this multiprocessor system are not all the same revision level.  To use all processors the operating system restricts itself to the features of the least capable processor in the system.  Should problems occur with this system, contact
        /// the CPU manufacturer to see if this mix of processors is supported.
        /// </summary>
        STATUS_MP_PROCESSOR_MISMATCH = 0x40000029,

        /// <summary>
        /// MessageId: STATUS_HIBERNATED
        /// MessageText:
        /// The system was put into hibernation.
        /// </summary>
        STATUS_HIBERNATED = 0x4000002A,

        /// <summary>
        /// MessageId: STATUS_RESUME_HIBERNATION
        /// MessageText:
        /// The system was resumed from hibernation.
        /// </summary>
        STATUS_RESUME_HIBERNATION = 0x4000002B,

        /// <summary>
        /// MessageId: STATUS_FIRMWARE_UPDATED
        /// MessageText:
        /// Windows has detected that the system firmware (BIOS) was updated [previous firmware date = %2, current firmware date %3].
        /// </summary>
        STATUS_FIRMWARE_UPDATED = 0x4000002C,

        /// <summary>
        /// MessageId: STATUS_DRIVERS_LEAKING_LOCKED_PAGES
        /// MessageText:
        /// A device driver is leaking locked I/O pages causing system degradation. The system has automatically enabled tracking code in order to try and catch the culprit.
        /// </summary>
        STATUS_DRIVERS_LEAKING_LOCKED_PAGES = 0x4000002D,

        /// <summary>
        /// MessageId: STATUS_MESSAGE_RETRIEVED
        /// MessageText:
        /// The ALPC message being canceled has already been retrieved from the queue on the other side.
        /// </summary>
        STATUS_MESSAGE_RETRIEVED = 0x4000002E,

        /// <summary>
        /// MessageId: STATUS_SYSTEM_POWERSTATE_TRANSITION
        /// MessageText:
        /// The system powerstate is transitioning from %2 to %3.
        /// </summary>
        STATUS_SYSTEM_POWERSTATE_TRANSITION = 0x4000002F,

        /// <summary>
        /// MessageId: STATUS_ALPC_CHECK_COMPLETION_LIST
        /// MessageText:
        /// The receive operation was successful. Check the ALPC completion list for the received message.
        /// </summary>
        STATUS_ALPC_CHECK_COMPLETION_LIST = 0x40000030,

        /// <summary>
        /// MessageId: STATUS_SYSTEM_POWERSTATE_COMPLEX_TRANSITION
        /// MessageText:
        /// The system powerstate is transitioning from %2 to %3 but could enter %4.
        /// </summary>
        STATUS_SYSTEM_POWERSTATE_COMPLEX_TRANSITION = 0x40000031,

        /// <summary>
        /// MessageId: STATUS_ACCESS_AUDIT_BY_POLICY
        /// MessageText:
        /// Access to %1 is monitored by policy rule %2.
        /// </summary>
        STATUS_ACCESS_AUDIT_BY_POLICY = 0x40000032,

        /// <summary>
        /// MessageId: STATUS_ABANDON_HIBERFILE
        /// MessageText:
        /// A valid hibernation file has been invalidated and should be abandoned.
        /// </summary>
        STATUS_ABANDON_HIBERFILE = 0x40000033,

        /// <summary>
        /// MessageId: STATUS_BIZRULES_NOT_ENABLED
        /// MessageText:
        /// Business rule scripts are disabled for the calling application.
        /// </summary>
        STATUS_BIZRULES_NOT_ENABLED = 0x40000034,

        /// <summary>
        /// MessageId: DBG_REPLY_LATER
        /// MessageText:
        /// Debugger will reply later.
        /// </summary>
        DBG_REPLY_LATER = 0x40010001,

        /// <summary>
        /// MessageId: DBG_UNABLE_TO_PROVIDE_HANDLE
        /// MessageText:
        /// Debugger cannot provide handle.
        /// </summary>
        DBG_UNABLE_TO_PROVIDE_HANDLE = 0x40010002,

        /// <summary>
        /// MessageId: DBG_TERMINATE_THREAD
        /// MessageText:
        /// Debugger terminated thread.
        /// </summary>
        DBG_TERMINATE_THREAD = 0x40010003,

        /// <summary>
        /// MessageId: DBG_TERMINATE_PROCESS
        /// MessageText:
        /// Debugger terminated process.
        /// </summary>
        DBG_TERMINATE_PROCESS = 0x40010004,

        /// <summary>
        /// MessageId: DBG_CONTROL_C
        /// MessageText:
        /// Debugger got control C.
        /// </summary>
        DBG_CONTROL_C = 0x40010005,

        /// <summary>
        /// MessageId: DBG_PRINTEXCEPTION_C
        /// MessageText:
        /// Debugger printed exception on control C.
        /// </summary>
        DBG_PRINTEXCEPTION_C = 0x40010006,

        /// <summary>
        /// MessageId: DBG_RIPEXCEPTION
        /// MessageText:
        /// Debugger received RIP exception.
        /// </summary>
        DBG_RIPEXCEPTION = 0x40010007,

        /// <summary>
        /// MessageId: DBG_CONTROL_BREAK
        /// MessageText:
        /// Debugger received control break.
        /// </summary>
        DBG_CONTROL_BREAK = 0x40010008,

        /// <summary>
        /// MessageId: DBG_COMMAND_EXCEPTION
        /// MessageText:
        /// Debugger command communication exception.
        /// </summary>
        DBG_COMMAND_EXCEPTION = 0x40010009,

        /// <summary>
        /// MessageId: STATUS_FLT_BUFFER_TOO_SMALL
        /// MessageText:
        /// {Buffer too small}
        /// The buffer is too small to contain the entry. No information has been written to the buffer.
        /// </summary>
        STATUS_FLT_BUFFER_TOO_SMALL = 0x801C0001,

        /// <summary>
        /// MessageId: STATUS_GUARD_PAGE_VIOLATION
        /// MessageText:
        /// {EXCEPTION}
        /// Guard Page Exception
        /// A page of memory that marks the end of a data structure, such as a stack or an array, has been accessed.
        /// </summary>
        STATUS_GUARD_PAGE_VIOLATION = 0x80000001,

        /// <summary>
        /// MessageId: STATUS_DATATYPE_MISALIGNMENT
        /// MessageText:
        /// {EXCEPTION}
        /// Alignment Fault
        /// A datatype misalignment was detected in a load or store instruction.
        /// </summary>
        STATUS_DATATYPE_MISALIGNMENT = 0x80000002,

        /// <summary>
        /// MessageId: STATUS_BREAKPOINT
        /// MessageText:
        /// {EXCEPTION}
        /// Breakpoint
        /// A breakpoint has been reached.
        /// </summary>
        STATUS_BREAKPOINT = 0x80000003,

        /// <summary>
        /// MessageId: STATUS_SINGLE_STEP
        /// MessageText:
        /// {EXCEPTION}
        /// Single Step
        /// A single step or trace operation has just been completed.
        /// </summary>
        STATUS_SINGLE_STEP = 0x80000004,

        /// <summary>
        /// MessageId: STATUS_BUFFER_OVERFLOW
        /// MessageText:
        /// {Buffer Overflow}
        /// The data was too large to fit into the specified buffer.
        /// </summary>
        STATUS_BUFFER_OVERFLOW = 0x80000005,

        /// <summary>
        /// MessageId: STATUS_NO_MORE_FILES
        /// MessageText:
        /// {No More Files}
        /// No more files were found which match the file specification.
        /// </summary>
        STATUS_NO_MORE_FILES = 0x80000006,

        /// <summary>
        /// MessageId: STATUS_WAKE_SYSTEM_DEBUGGER
        /// MessageText:
        /// {Kernel Debugger Awakened}
        /// the system debugger was awakened by an interrupt.
        /// </summary>
        STATUS_WAKE_SYSTEM_DEBUGGER = 0x80000007,

        /// <summary>
        /// MessageId: STATUS_HANDLES_CLOSED
        /// MessageText:
        /// {Handles Closed}
        /// Handles to objects have been automatically closed as a result of the requested operation.
        /// </summary>
        STATUS_HANDLES_CLOSED = 0x8000000A,

        /// <summary>
        /// MessageId: STATUS_NO_INHERITANCE
        /// MessageText:
        /// {Non-Inheritable ACL}
        /// An access control list (ACL) contains no components that can be inherited.
        /// </summary>
        STATUS_NO_INHERITANCE = 0x8000000B,

        /// <summary>
        /// MessageId: STATUS_GUID_SUBSTITUTION_MADE
        /// MessageText:
        /// {GUID Substitution}
        /// During the translation of a global identifier (GUID) to a Windows security ID (SID), no administratively-defined GUID prefix was found.
        /// A substitute prefix was used, which will not compromise system security.
        /// However, this may provide a more restrictive access than intended.
        /// </summary>
        STATUS_GUID_SUBSTITUTION_MADE = 0x8000000C,

        /// <summary>
        /// MessageId: STATUS_PARTIAL_COPY
        /// MessageText:
        /// {Partial Copy}
        /// Due to protection conflicts not all the requested bytes could be copied.
        /// </summary>
        STATUS_PARTIAL_COPY = 0x8000000D,

        /// <summary>
        /// MessageId: STATUS_DEVICE_PAPER_EMPTY
        /// MessageText:
        /// {Out of Paper}
        /// The printer is out of paper.
        /// </summary>
        STATUS_DEVICE_PAPER_EMPTY = 0x8000000E,

        /// <summary>
        /// MessageId: STATUS_DEVICE_POWERED_OFF
        /// MessageText:
        /// {Device Power Is Off}
        /// The printer power has been turned off.
        /// </summary>
        STATUS_DEVICE_POWERED_OFF = 0x8000000F,

        /// <summary>
        /// MessageId: STATUS_DEVICE_OFF_LINE
        /// MessageText:
        /// {Device Offline}
        /// The printer has been taken offline.
        /// </summary>
        STATUS_DEVICE_OFF_LINE = 0x80000010,

        /// <summary>
        /// MessageId: STATUS_DEVICE_BUSY
        /// MessageText:
        /// {Device Busy}
        /// The device is currently busy.
        /// </summary>
        STATUS_DEVICE_BUSY = 0x80000011,

        /// <summary>
        /// MessageId: STATUS_NO_MORE_EAS
        /// MessageText:
        /// {No More EAs}
        /// No more extended attributes (EAs) were found for the file.
        /// </summary>
        STATUS_NO_MORE_EAS = 0x80000012,

        /// <summary>
        /// MessageId: STATUS_INVALID_EA_NAME
        /// MessageText:
        /// {Illegal EA}
        /// The specified extended attribute (EA) name contains at least one illegal character.
        /// </summary>
        STATUS_INVALID_EA_NAME = 0x80000013,

        /// <summary>
        /// MessageId: STATUS_EA_LIST_INCONSISTENT
        /// MessageText:
        /// {Inconsistent EA List}
        /// The extended attribute (EA) list is inconsistent.
        /// </summary>
        STATUS_EA_LIST_INCONSISTENT = 0x80000014,

        /// <summary>
        /// MessageId: STATUS_INVALID_EA_FLAG
        /// MessageText:
        /// {Invalid EA Flag}
        /// An invalid extended attribute (EA) flag was set.
        /// </summary>
        STATUS_INVALID_EA_FLAG = 0x80000015,

        /// <summary>
        /// MessageId: STATUS_VERIFY_REQUIRED
        /// MessageText:
        /// {Verifying Disk}
        /// The media has changed and a verify operation is in progress so no reads or writes may be performed to the device, except those used in the verify operation.
        /// </summary>
        STATUS_VERIFY_REQUIRED = 0x80000016,

        /// <summary>
        /// MessageId: STATUS_EXTRANEOUS_INFORMATION
        /// MessageText:
        /// {Too Much Information}
        /// The specified access control list (ACL) contained more information than was expected.
        /// </summary>
        STATUS_EXTRANEOUS_INFORMATION = 0x80000017,

        /// <summary>
        /// MessageId: STATUS_RXACT_COMMIT_NECESSARY
        /// MessageText:
        /// This warning level status indicates that the transaction state already exists for the registry sub-tree, but that a transaction commit was previously aborted.
        /// The commit has NOT been completed, but has not been rolled back either (so it may still be committed if desired).
        /// </summary>
        STATUS_RXACT_COMMIT_NECESSARY = 0x80000018,

        /// <summary>
        /// MessageId: STATUS_NO_MORE_ENTRIES
        /// MessageText:
        /// {No More Entries}
        /// No more entries are available from an enumeration operation.
        /// </summary>
        STATUS_NO_MORE_ENTRIES = 0x8000001A,

        /// <summary>
        /// MessageId: STATUS_FILEMARK_DETECTED
        /// MessageText:
        /// {Filemark Found}
        /// A filemark was detected.
        /// </summary>
        STATUS_FILEMARK_DETECTED = 0x8000001B,

        /// <summary>
        /// MessageId: STATUS_MEDIA_CHANGED
        /// MessageText:
        /// {Media Changed}
        /// The media may have changed.
        /// </summary>
        STATUS_MEDIA_CHANGED = 0x8000001C,

        /// <summary>
        /// MessageId: STATUS_BUS_RESET
        /// MessageText:
        /// {I/O Bus Reset}
        /// An I/O bus reset was detected.
        /// </summary>
        STATUS_BUS_RESET = 0x8000001D,

        /// <summary>
        /// MessageId: STATUS_END_OF_MEDIA
        /// MessageText:
        /// {End of Media}
        /// The end of the media was encountered.
        /// </summary>
        STATUS_END_OF_MEDIA = 0x8000001E,

        /// <summary>
        /// MessageId: STATUS_BEGINNING_OF_MEDIA
        /// MessageText:
        /// Beginning of tape or partition has been detected.
        /// </summary>
        STATUS_BEGINNING_OF_MEDIA = 0x8000001F,

        /// <summary>
        /// MessageId: STATUS_MEDIA_CHECK
        /// MessageText:
        /// {Media Changed}
        /// The media may have changed.
        /// </summary>
        STATUS_MEDIA_CHECK = 0x80000020,

        /// <summary>
        /// MessageId: STATUS_SETMARK_DETECTED
        /// MessageText:
        /// A tape access reached a setmark.
        /// </summary>
        STATUS_SETMARK_DETECTED = 0x80000021,

        /// <summary>
        /// MessageId: STATUS_NO_DATA_DETECTED
        /// MessageText:
        /// During a tape access, the end of the data written is reached.
        /// </summary>
        STATUS_NO_DATA_DETECTED = 0x80000022,

        /// <summary>
        /// MessageId: STATUS_REDIRECTOR_HAS_OPEN_HANDLES
        /// MessageText:
        /// The redirector is in use and cannot be unloaded.
        /// </summary>
        STATUS_REDIRECTOR_HAS_OPEN_HANDLES = 0x80000023,

        /// <summary>
        /// MessageId: STATUS_SERVER_HAS_OPEN_HANDLES
        /// MessageText:
        /// The server is in use and cannot be unloaded.
        /// </summary>
        STATUS_SERVER_HAS_OPEN_HANDLES = 0x80000024,

        /// <summary>
        /// MessageId: STATUS_ALREADY_DISCONNECTED
        /// MessageText:
        /// The specified connection has already been disconnected.
        /// </summary>
        STATUS_ALREADY_DISCONNECTED = 0x80000025,

        /// <summary>
        /// MessageId: STATUS_LONGJUMP
        /// MessageText:
        /// A long jump has been executed.
        /// </summary>
        STATUS_LONGJUMP = 0x80000026,

        /// <summary>
        /// MessageId: STATUS_CLEANER_CARTRIDGE_INSTALLED
        /// MessageText:
        /// A cleaner cartridge is present in the tape library.
        /// </summary>
        STATUS_CLEANER_CARTRIDGE_INSTALLED = 0x80000027,

        /// <summary>
        /// MessageId: STATUS_PLUGPLAY_QUERY_VETOED
        /// MessageText:
        /// The Plug and Play query operation was not successful.
        /// </summary>
        STATUS_PLUGPLAY_QUERY_VETOED = 0x80000028,

        /// <summary>
        /// MessageId: STATUS_UNWIND_CONSOLIDATE
        /// MessageText:
        /// A frame consolidation has been executed.
        /// </summary>
        STATUS_UNWIND_CONSOLIDATE = 0x80000029,

        /// <summary>
        /// MessageId: STATUS_REGISTRY_HIVE_RECOVERED
        /// MessageText:
        /// {Registry Hive Recovered}
        /// Registry hive (file):
        /// %hs
        /// was corrupted and it has been recovered. Some data might have been lost.
        /// </summary>
        STATUS_REGISTRY_HIVE_RECOVERED = 0x8000002A,

        /// <summary>
        /// MessageId: STATUS_DLL_MIGHT_BE_INSECURE
        /// MessageText:
        /// The application is attempting to run executable code from the module %hs. This may be insecure.  An alternative, %hs, is available.  Should the application use the secure module %hs?
        /// </summary>
        STATUS_DLL_MIGHT_BE_INSECURE = 0x8000002B,

        /// <summary>
        /// MessageId: STATUS_DLL_MIGHT_BE_INCOMPATIBLE
        /// MessageText:
        /// The application is loading executable code from the module %hs. This is secure, but may be incompatible with previous releases of the operating system.  An alternative, %hs, is available.  Should the application use the secure module %hs?
        /// </summary>
        STATUS_DLL_MIGHT_BE_INCOMPATIBLE = 0x8000002C,

        /// <summary>
        /// MessageId: STATUS_STOPPED_ON_SYMLINK
        /// MessageText:
        /// The create operation stopped after reaching a symbolic link.
        /// </summary>
        STATUS_STOPPED_ON_SYMLINK = 0x8000002D,

        /// <summary>
        /// MessageId: DBG_EXCEPTION_NOT_HANDLED
        /// MessageText:
        /// Debugger did not handle the exception.
        /// </summary>
        DBG_EXCEPTION_NOT_HANDLED = 0x80010001,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_ALREADY_UP
        /// MessageText:
        /// The cluster node is already up.
        /// </summary>
        STATUS_CLUSTER_NODE_ALREADY_UP = 0x80130001,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_ALREADY_DOWN
        /// MessageText:
        /// The cluster node is already down.
        /// </summary>
        STATUS_CLUSTER_NODE_ALREADY_DOWN = 0x80130002,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NETWORK_ALREADY_ONLINE
        /// MessageText:
        /// The cluster network is already online.
        /// </summary>
        STATUS_CLUSTER_NETWORK_ALREADY_ONLINE = 0x80130003,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NETWORK_ALREADY_OFFLINE
        /// MessageText:
        /// The cluster network is already offline.
        /// </summary>
        STATUS_CLUSTER_NETWORK_ALREADY_OFFLINE = 0x80130004,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_ALREADY_MEMBER
        /// MessageText:
        /// The cluster node is already a member of the cluster.
        /// </summary>
        STATUS_CLUSTER_NODE_ALREADY_MEMBER = 0x80130005,

        /// <summary>
        /// MessageId: STATUS_FVE_PARTIAL_METADATA
        /// MessageText:
        /// Volume Metadata read or write is incomplete.
        /// </summary>
        STATUS_FVE_PARTIAL_METADATA = 0x80210001,

        /// <summary>
        /// MessageId: STATUS_UNSUCCESSFUL
        /// MessageText:
        /// {Operation Failed}
        /// The requested operation was unsuccessful.
        /// </summary>
        STATUS_UNSUCCESSFUL = 0xC0000001,

        /// <summary>
        /// MessageId: STATUS_NOT_IMPLEMENTED
        /// MessageText:
        /// {Not Implemented}
        /// The requested operation is not implemented.
        /// </summary>
        STATUS_NOT_IMPLEMENTED = 0xC0000002,

        /// <summary>
        /// MessageId: STATUS_INVALID_INFO_CLASS
        /// MessageText:
        /// {Invalid Parameter}
        /// The specified information class is not a valid information class for the specified object.
        /// </summary>
        STATUS_INVALID_INFO_CLASS = 0xC0000003,

        /// <summary>
        /// MessageId: STATUS_INFO_LENGTH_MISMATCH
        /// MessageText:
        /// The specified information record length does not match the length required for the specified information class.
        /// </summary>
        STATUS_INFO_LENGTH_MISMATCH = 0xC0000004,

        /// <summary>
        /// MessageId: STATUS_ACCESS_VIOLATION
        /// MessageText:
        /// The instruction at 0x%08lx referenced memory at 0x%08lx. The memory could not be %s.
        /// </summary>
        STATUS_ACCESS_VIOLATION = 0xC0000005,

        /// <summary>
        /// MessageId: STATUS_IN_PAGE_ERROR
        /// MessageText:
        /// The instruction at 0x%08lx referenced memory at 0x%08lx. The required data was not placed into memory because of an I/O error status of 0x%08lx.
        /// </summary>
        STATUS_IN_PAGE_ERROR = 0xC0000006,

        /// <summary>
        /// MessageId: STATUS_PAGEFILE_QUOTA
        /// MessageText:
        /// The pagefile quota for the process has been exhausted.
        /// </summary>
        STATUS_PAGEFILE_QUOTA = 0xC0000007,

        /// <summary>
        /// MessageId: STATUS_INVALID_HANDLE
        /// MessageText:
        /// An invalid HANDLE was specified.
        /// </summary>
        STATUS_INVALID_HANDLE = 0xC0000008,

        /// <summary>
        /// MessageId: STATUS_BAD_INITIAL_STACK
        /// MessageText:
        /// An invalid initial stack was specified in a call to NtCreateThread.
        /// </summary>
        STATUS_BAD_INITIAL_STACK = 0xC0000009,

        /// <summary>
        /// MessageId: STATUS_BAD_INITIAL_PC
        /// MessageText:
        /// An invalid initial start address was specified in a call to NtCreateThread.
        /// </summary>
        STATUS_BAD_INITIAL_PC = 0xC000000A,

        /// <summary>
        /// MessageId: STATUS_INVALID_CID
        /// MessageText:
        /// An invalid Client ID was specified.
        /// </summary>
        STATUS_INVALID_CID = 0xC000000B,

        /// <summary>
        /// MessageId: STATUS_TIMER_NOT_CANCELED
        /// MessageText:
        /// An attempt was made to cancel or set a timer that has an associated APC and the subject thread is not the thread that originally set the timer with an associated APC routine.
        /// </summary>
        STATUS_TIMER_NOT_CANCELED = 0xC000000C,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER
        /// MessageText:
        /// An invalid parameter was passed to a service or function.
        /// </summary>
        STATUS_INVALID_PARAMETER = 0xC000000D,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_DEVICE
        /// MessageText:
        /// A device which does not exist was specified.
        /// </summary>
        STATUS_NO_SUCH_DEVICE = 0xC000000E,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_FILE
        /// MessageText:
        /// {File Not Found}
        /// The file %hs does not exist.
        /// </summary>
        STATUS_NO_SUCH_FILE = 0xC000000F,

        /// <summary>
        /// MessageId: STATUS_INVALID_DEVICE_REQUEST
        /// MessageText:
        /// The specified request is not a valid operation for the target device.
        /// </summary>
        STATUS_INVALID_DEVICE_REQUEST = 0xC0000010,

        /// <summary>
        /// MessageId: STATUS_END_OF_FILE
        /// MessageText:
        /// The end-of-file marker has been reached. There is no valid data in the file beyond this marker.
        /// </summary>
        STATUS_END_OF_FILE = 0xC0000011,

        /// <summary>
        /// MessageId: STATUS_WRONG_VOLUME
        /// MessageText:
        /// {Wrong Volume}
        /// The wrong volume is in the drive.
        /// Please insert volume %hs into drive %hs.
        /// </summary>
        STATUS_WRONG_VOLUME = 0xC0000012,

        /// <summary>
        /// MessageId: STATUS_NO_MEDIA_IN_DEVICE
        /// MessageText:
        /// {No Disk}
        /// There is no disk in the drive.
        /// Please insert a disk into drive %hs.
        /// </summary>
        STATUS_NO_MEDIA_IN_DEVICE = 0xC0000013,

        /// <summary>
        /// MessageId: STATUS_UNRECOGNIZED_MEDIA
        /// MessageText:
        /// {Unknown Disk Format}
        /// The disk in drive %hs is not formatted properly.
        /// Please check the disk, and reformat if necessary.
        /// </summary>
        STATUS_UNRECOGNIZED_MEDIA = 0xC0000014,

        /// <summary>
        /// MessageId: STATUS_NONEXISTENT_SECTOR
        /// MessageText:
        /// {Sector Not Found}
        /// The specified sector does not exist.
        /// </summary>
        STATUS_NONEXISTENT_SECTOR = 0xC0000015,

        /// <summary>
        /// MessageId: STATUS_MORE_PROCESSING_REQUIRED
        /// MessageText:
        /// {Still Busy}
        /// The specified I/O request packet (IRP) cannot be disposed of because the I/O operation is not complete.
        /// </summary>
        STATUS_MORE_PROCESSING_REQUIRED = 0xC0000016,

        /// <summary>
        /// MessageId: STATUS_NO_MEMORY
        /// MessageText:
        /// {Not Enough Quota}
        /// Not enough virtual memory or paging file quota is available to complete the specified operation.
        /// </summary>
        STATUS_NO_MEMORY = 0xC0000017,

        /// <summary>
        /// MessageId: STATUS_CONFLICTING_ADDRESSES
        /// MessageText:
        /// {Conflicting Address Range}
        /// The specified address range conflicts with the address space.
        /// </summary>
        STATUS_CONFLICTING_ADDRESSES = 0xC0000018,

        /// <summary>
        /// MessageId: STATUS_NOT_MAPPED_VIEW
        /// MessageText:
        /// Address range to unmap is not a mapped view.
        /// </summary>
        STATUS_NOT_MAPPED_VIEW = 0xC0000019,

        /// <summary>
        /// MessageId: STATUS_UNABLE_TO_FREE_VM
        /// MessageText:
        /// Virtual memory cannot be freed.
        /// </summary>
        STATUS_UNABLE_TO_FREE_VM = 0xC000001A,

        /// <summary>
        /// MessageId: STATUS_UNABLE_TO_DELETE_SECTION
        /// MessageText:
        /// Specified section cannot be deleted.
        /// </summary>
        STATUS_UNABLE_TO_DELETE_SECTION = 0xC000001B,

        /// <summary>
        /// MessageId: STATUS_INVALID_SYSTEM_SERVICE
        /// MessageText:
        /// An invalid system service was specified in a system service call.
        /// </summary>
        STATUS_INVALID_SYSTEM_SERVICE = 0xC000001C,

        /// <summary>
        /// MessageId: STATUS_ILLEGAL_INSTRUCTION
        /// MessageText:
        /// {EXCEPTION}
        /// Illegal Instruction
        /// An attempt was made to execute an illegal instruction.
        /// </summary>
        STATUS_ILLEGAL_INSTRUCTION = 0xC000001D,

        /// <summary>
        /// MessageId: STATUS_INVALID_LOCK_SEQUENCE
        /// MessageText:
        /// {Invalid Lock Sequence}
        /// An attempt was made to execute an invalid lock sequence.
        /// </summary>
        STATUS_INVALID_LOCK_SEQUENCE = 0xC000001E,

        /// <summary>
        /// MessageId: STATUS_INVALID_VIEW_SIZE
        /// MessageText:
        /// {Invalid Mapping}
        /// An attempt was made to create a view for a section which is bigger than the section.
        /// </summary>
        STATUS_INVALID_VIEW_SIZE = 0xC000001F,

        /// <summary>
        /// MessageId: STATUS_INVALID_FILE_FOR_SECTION
        /// MessageText:
        /// {Bad File}
        /// The attributes of the specified mapping file for a section of memory cannot be read.
        /// </summary>
        STATUS_INVALID_FILE_FOR_SECTION = 0xC0000020,

        /// <summary>
        /// MessageId: STATUS_ALREADY_COMMITTED
        /// MessageText:
        /// {Already Committed}
        /// The specified address range is already committed.
        /// </summary>
        STATUS_ALREADY_COMMITTED = 0xC0000021,

        /// <summary>
        /// MessageId: STATUS_ACCESS_DENIED
        /// MessageText:
        /// {Access Denied}
        /// A process has requested access to an object, but has not been granted those access rights.
        /// </summary>
        STATUS_ACCESS_DENIED = 0xC0000022,

        /// <summary>
        /// MessageId: STATUS_BUFFER_TOO_SMALL
        /// MessageText:
        /// {Buffer Too Small}
        /// The buffer is too small to contain the entry. No information has been written to the buffer.
        /// </summary>
        STATUS_BUFFER_TOO_SMALL = 0xC0000023,

        /// <summary>
        /// MessageId: STATUS_OBJECT_TYPE_MISMATCH
        /// MessageText:
        /// {Wrong Type}
        /// There is a mismatch between the type of object required by the requested operation and the type of object that is specified in the request.
        /// </summary>
        STATUS_OBJECT_TYPE_MISMATCH = 0xC0000024,

        /// <summary>
        /// MessageId: STATUS_NONCONTINUABLE_EXCEPTION
        /// MessageText:
        /// {EXCEPTION}
        /// Cannot Continue
        /// Windows cannot continue from this exception.
        /// </summary>
        STATUS_NONCONTINUABLE_EXCEPTION = 0xC0000025,

        /// <summary>
        /// MessageId: STATUS_INVALID_DISPOSITION
        /// MessageText:
        /// An invalid exception disposition was returned by an exception handler.
        /// </summary>
        STATUS_INVALID_DISPOSITION = 0xC0000026,

        /// <summary>
        /// MessageId: STATUS_UNWIND
        /// MessageText:
        /// Unwind exception code.
        /// </summary>
        STATUS_UNWIND = 0xC0000027,

        /// <summary>
        /// MessageId: STATUS_BAD_STACK
        /// MessageText:
        /// An invalid or unaligned stack was encountered during an unwind operation.
        /// </summary>
        STATUS_BAD_STACK = 0xC0000028,

        /// <summary>
        /// MessageId: STATUS_INVALID_UNWIND_TARGET
        /// MessageText:
        /// An invalid unwind target was encountered during an unwind operation.
        /// </summary>
        STATUS_INVALID_UNWIND_TARGET = 0xC0000029,

        /// <summary>
        /// MessageId: STATUS_NOT_LOCKED
        /// MessageText:
        /// An attempt was made to unlock a page of memory which was not locked.
        /// </summary>
        STATUS_NOT_LOCKED = 0xC000002A,

        /// <summary>
        /// MessageId: STATUS_PARITY_ERROR
        /// MessageText:
        /// Device parity error on I/O operation.
        /// </summary>
        STATUS_PARITY_ERROR = 0xC000002B,

        /// <summary>
        /// MessageId: STATUS_UNABLE_TO_DECOMMIT_VM
        /// MessageText:
        /// An attempt was made to decommit uncommitted virtual memory.
        /// </summary>
        STATUS_UNABLE_TO_DECOMMIT_VM = 0xC000002C,

        /// <summary>
        /// MessageId: STATUS_NOT_COMMITTED
        /// MessageText:
        /// An attempt was made to change the attributes on memory that has not been committed.
        /// </summary>
        STATUS_NOT_COMMITTED = 0xC000002D,

        /// <summary>
        /// MessageId: STATUS_INVALID_PORT_ATTRIBUTES
        /// MessageText:
        /// Invalid Object Attributes specified to NtCreatePort or invalid Port Attributes specified to NtConnectPort
        /// </summary>
        STATUS_INVALID_PORT_ATTRIBUTES = 0xC000002E,

        /// <summary>
        /// MessageId: STATUS_PORT_MESSAGE_TOO_LONG
        /// MessageText:
        /// Length of message passed to NtRequestPort or NtRequestWaitReplyPort was longer than the maximum message allowed by the port.
        /// </summary>
        STATUS_PORT_MESSAGE_TOO_LONG = 0xC000002F,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_MIX
        /// MessageText:
        /// An invalid combination of parameters was specified.
        /// </summary>
        STATUS_INVALID_PARAMETER_MIX = 0xC0000030,

        /// <summary>
        /// MessageId: STATUS_INVALID_QUOTA_LOWER
        /// MessageText:
        /// An attempt was made to lower a quota limit below the current usage.
        /// </summary>
        STATUS_INVALID_QUOTA_LOWER = 0xC0000031,

        /// <summary>
        /// MessageId: STATUS_DISK_CORRUPT_ERROR
        /// MessageText:
        /// {Corrupt Disk}
        /// The file system structure on the disk is corrupt and unusable.
        /// Please run the Chkdsk utility on the volume %hs.
        /// </summary>
        STATUS_DISK_CORRUPT_ERROR = 0xC0000032,

        /// <summary>
        /// MessageId: STATUS_OBJECT_NAME_INVALID
        /// MessageText:
        /// Object Name invalid.
        /// </summary>
        STATUS_OBJECT_NAME_INVALID = 0xC0000033,

        /// <summary>
        /// MessageId: STATUS_OBJECT_NAME_NOT_FOUND
        /// MessageText:
        /// Object Name not found.
        /// </summary>
        STATUS_OBJECT_NAME_NOT_FOUND = 0xC0000034,

        /// <summary>
        /// MessageId: STATUS_OBJECT_NAME_COLLISION
        /// MessageText:
        /// Object Name already exists.
        /// </summary>
        STATUS_OBJECT_NAME_COLLISION = 0xC0000035,

        /// <summary>
        /// MessageId: STATUS_PORT_DISCONNECTED
        /// MessageText:
        /// Attempt to send a message to a disconnected communication port.
        /// </summary>
        STATUS_PORT_DISCONNECTED = 0xC0000037,

        /// <summary>
        /// MessageId: STATUS_DEVICE_ALREADY_ATTACHED
        /// MessageText:
        /// An attempt was made to attach to a device that was already attached to another device.
        /// </summary>
        STATUS_DEVICE_ALREADY_ATTACHED = 0xC0000038,

        /// <summary>
        /// MessageId: STATUS_OBJECT_PATH_INVALID
        /// MessageText:
        /// Object Path Component was not a directory object.
        /// </summary>
        STATUS_OBJECT_PATH_INVALID = 0xC0000039,

        /// <summary>
        /// MessageId: STATUS_OBJECT_PATH_NOT_FOUND
        /// MessageText:
        /// {Path Not Found}
        /// The path %hs does not exist.
        /// </summary>
        STATUS_OBJECT_PATH_NOT_FOUND = 0xC000003A,

        /// <summary>
        /// MessageId: STATUS_OBJECT_PATH_SYNTAX_BAD
        /// MessageText:
        /// Object Path Component was not a directory object.
        /// </summary>
        STATUS_OBJECT_PATH_SYNTAX_BAD = 0xC000003B,

        /// <summary>
        /// MessageId: STATUS_DATA_OVERRUN
        /// MessageText:
        /// {Data Overrun}
        /// A data overrun error occurred.
        /// </summary>
        STATUS_DATA_OVERRUN = 0xC000003C,

        /// <summary>
        /// MessageId: STATUS_DATA_LATE_ERROR
        /// MessageText:
        /// {Data Late}
        /// A data late error occurred.
        /// </summary>
        STATUS_DATA_LATE_ERROR = 0xC000003D,

        /// <summary>
        /// MessageId: STATUS_DATA_ERROR
        /// MessageText:
        /// {Data Error}
        /// An error in reading or writing data occurred.
        /// </summary>
        STATUS_DATA_ERROR = 0xC000003E,

        /// <summary>
        /// MessageId: STATUS_CRC_ERROR
        /// MessageText:
        /// {Bad CRC}
        /// A cyclic redundancy check (CRC) checksum error occurred.
        /// </summary>
        STATUS_CRC_ERROR = 0xC000003F,

        /// <summary>
        /// MessageId: STATUS_SECTION_TOO_BIG
        /// MessageText:
        /// {Section Too Large}
        /// The specified section is too big to map the file.
        /// </summary>
        STATUS_SECTION_TOO_BIG = 0xC0000040,

        /// <summary>
        /// MessageId: STATUS_PORT_CONNECTION_REFUSED
        /// MessageText:
        /// The NtConnectPort request is refused.
        /// </summary>
        STATUS_PORT_CONNECTION_REFUSED = 0xC0000041,

        /// <summary>
        /// MessageId: STATUS_INVALID_PORT_HANDLE
        /// MessageText:
        /// The type of port handle is invalid for the operation requested.
        /// </summary>
        STATUS_INVALID_PORT_HANDLE = 0xC0000042,

        /// <summary>
        /// MessageId: STATUS_SHARING_VIOLATION
        /// MessageText:
        /// A file cannot be opened because the share access flags are incompatible.
        /// </summary>
        STATUS_SHARING_VIOLATION = 0xC0000043,

        /// <summary>
        /// MessageId: STATUS_QUOTA_EXCEEDED
        /// MessageText:
        /// Insufficient quota exists to complete the operation
        /// </summary>
        STATUS_QUOTA_EXCEEDED = 0xC0000044,

        /// <summary>
        /// MessageId: STATUS_INVALID_PAGE_PROTECTION
        /// MessageText:
        /// The specified page protection was not valid.
        /// </summary>
        STATUS_INVALID_PAGE_PROTECTION = 0xC0000045,

        /// <summary>
        /// MessageId: STATUS_MUTANT_NOT_OWNED
        /// MessageText:
        /// An attempt to release a mutant object was made by a thread that was not the owner of the mutant object.
        /// </summary>
        STATUS_MUTANT_NOT_OWNED = 0xC0000046,

        /// <summary>
        /// MessageId: STATUS_SEMAPHORE_LIMIT_EXCEEDED
        /// MessageText:
        /// An attempt was made to release a semaphore such that its maximum count would have been exceeded.
        /// </summary>
        STATUS_SEMAPHORE_LIMIT_EXCEEDED = 0xC0000047,

        /// <summary>
        /// MessageId: STATUS_PORT_ALREADY_SET
        /// MessageText:
        /// An attempt to set a processes DebugPort or ExceptionPort was made, but a port already exists in the process or
        /// an attempt to set a file's CompletionPort made, but a port was already set in the file or
        /// an attempt to set an alpc port's associated completion port was made, but it is already set.
        /// </summary>
        STATUS_PORT_ALREADY_SET = 0xC0000048,

        /// <summary>
        /// MessageId: STATUS_SECTION_NOT_IMAGE
        /// MessageText:
        /// An attempt was made to query image information on a section which does not map an image.
        /// </summary>
        STATUS_SECTION_NOT_IMAGE = 0xC0000049,

        /// <summary>
        /// MessageId: STATUS_SUSPEND_COUNT_EXCEEDED
        /// MessageText:
        /// An attempt was made to suspend a thread whose suspend count was at its maximum.
        /// </summary>
        STATUS_SUSPEND_COUNT_EXCEEDED = 0xC000004A,

        /// <summary>
        /// MessageId: STATUS_THREAD_IS_TERMINATING
        /// MessageText:
        /// An attempt was made to access a thread that has begun termination.
        /// </summary>
        STATUS_THREAD_IS_TERMINATING = 0xC000004B,

        /// <summary>
        /// MessageId: STATUS_BAD_WORKING_SET_LIMIT
        /// MessageText:
        /// An attempt was made to set the working set limit to an invalid value (minimum greater than maximum, etc.).
        /// </summary>
        STATUS_BAD_WORKING_SET_LIMIT = 0xC000004C,

        /// <summary>
        /// MessageId: STATUS_INCOMPATIBLE_FILE_MAP
        /// MessageText:
        /// A section was created to map a file which is not compatible to an already existing section which maps the same file.
        /// </summary>
        STATUS_INCOMPATIBLE_FILE_MAP = 0xC000004D,

        /// <summary>
        /// MessageId: STATUS_SECTION_PROTECTION
        /// MessageText:
        /// A view to a section specifies a protection which is incompatible with the initial view's protection.
        /// </summary>
        STATUS_SECTION_PROTECTION = 0xC000004E,

        /// <summary>
        /// MessageId: STATUS_EAS_NOT_SUPPORTED
        /// MessageText:
        /// An operation involving EAs failed because the file system does not support EAs.
        /// </summary>
        STATUS_EAS_NOT_SUPPORTED = 0xC000004F,

        /// <summary>
        /// MessageId: STATUS_EA_TOO_LARGE
        /// MessageText:
        /// An EA operation failed because EA set is too large.
        /// </summary>
        STATUS_EA_TOO_LARGE = 0xC0000050,

        /// <summary>
        /// MessageId: STATUS_NONEXISTENT_EA_ENTRY
        /// MessageText:
        /// An EA operation failed because the name or EA index is invalid.
        /// </summary>
        STATUS_NONEXISTENT_EA_ENTRY = 0xC0000051,

        /// <summary>
        /// MessageId: STATUS_NO_EAS_ON_FILE
        /// MessageText:
        /// The file for which EAs were requested has no EAs.
        /// </summary>
        STATUS_NO_EAS_ON_FILE = 0xC0000052,

        /// <summary>
        /// MessageId: STATUS_EA_CORRUPT_ERROR
        /// MessageText:
        /// The EA is corrupt and non-readable.
        /// </summary>
        STATUS_EA_CORRUPT_ERROR = 0xC0000053,

        /// <summary>
        /// MessageId: STATUS_FILE_LOCK_CONFLICT
        /// MessageText:
        /// A requested read/write cannot be granted due to a conflicting file lock.
        /// </summary>
        STATUS_FILE_LOCK_CONFLICT = 0xC0000054,

        /// <summary>
        /// MessageId: STATUS_LOCK_NOT_GRANTED
        /// MessageText:
        /// A requested file lock cannot be granted due to other existing locks.
        /// </summary>
        STATUS_LOCK_NOT_GRANTED = 0xC0000055,

        /// <summary>
        /// MessageId: STATUS_DELETE_PENDING
        /// MessageText:
        /// A non close operation has been requested of a file object with a delete pending.
        /// </summary>
        STATUS_DELETE_PENDING = 0xC0000056,

        /// <summary>
        /// MessageId: STATUS_CTL_FILE_NOT_SUPPORTED
        /// MessageText:
        /// An attempt was made to set the control attribute on a file. This attribute is not supported in the target file system.
        /// </summary>
        STATUS_CTL_FILE_NOT_SUPPORTED = 0xC0000057,

        /// <summary>
        /// MessageId: STATUS_UNKNOWN_REVISION
        /// MessageText:
        /// Indicates a revision number encountered or specified is not one known by the service. It may be a more recent revision than the service is aware of.
        /// </summary>
        STATUS_UNKNOWN_REVISION = 0xC0000058,

        /// <summary>
        /// MessageId: STATUS_REVISION_MISMATCH
        /// MessageText:
        /// Indicates two revision levels are incompatible.
        /// </summary>
        STATUS_REVISION_MISMATCH = 0xC0000059,

        /// <summary>
        /// MessageId: STATUS_INVALID_OWNER
        /// MessageText:
        /// Indicates a particular Security ID may not be assigned as the owner of an object.
        /// </summary>
        STATUS_INVALID_OWNER = 0xC000005A,

        /// <summary>
        /// MessageId: STATUS_INVALID_PRIMARY_GROUP
        /// MessageText:
        /// Indicates a particular Security ID may not be assigned as the primary group of an object.
        /// </summary>
        STATUS_INVALID_PRIMARY_GROUP = 0xC000005B,

        /// <summary>
        /// MessageId: STATUS_NO_IMPERSONATION_TOKEN
        /// MessageText:
        /// An attempt has been made to operate on an impersonation token by a thread that is not currently impersonating a client.
        /// </summary>
        STATUS_NO_IMPERSONATION_TOKEN = 0xC000005C,

        /// <summary>
        /// MessageId: STATUS_CANT_DISABLE_MANDATORY
        /// MessageText:
        /// A mandatory group may not be disabled.
        /// </summary>
        STATUS_CANT_DISABLE_MANDATORY = 0xC000005D,

        /// <summary>
        /// MessageId: STATUS_NO_LOGON_SERVERS
        /// MessageText:
        /// There are currently no logon servers available to service the logon request.
        /// </summary>
        STATUS_NO_LOGON_SERVERS = 0xC000005E,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_LOGON_SESSION
        /// MessageText:
        /// A specified logon session does not exist. It may already have been terminated.
        /// </summary>
        STATUS_NO_SUCH_LOGON_SESSION = 0xC000005F,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_PRIVILEGE
        /// MessageText:
        /// A specified privilege does not exist.
        /// </summary>
        STATUS_NO_SUCH_PRIVILEGE = 0xC0000060,

        /// <summary>
        /// MessageId: STATUS_PRIVILEGE_NOT_HELD
        /// MessageText:
        /// A required privilege is not held by the client.
        /// </summary>
        STATUS_PRIVILEGE_NOT_HELD = 0xC0000061,

        /// <summary>
        /// MessageId: STATUS_INVALID_ACCOUNT_NAME
        /// MessageText:
        /// The name provided is not a properly formed account name.
        /// </summary>
        STATUS_INVALID_ACCOUNT_NAME = 0xC0000062,

        /// <summary>
        /// MessageId: STATUS_USER_EXISTS
        /// MessageText:
        /// The specified account already exists.
        /// </summary>
        STATUS_USER_EXISTS = 0xC0000063,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_USER
        /// MessageText:
        /// The specified account does not exist.
        /// </summary>
        STATUS_NO_SUCH_USER = 0xC0000064,

        /// <summary>
        /// MessageId: STATUS_GROUP_EXISTS
        /// MessageText:
        /// The specified group already exists.
        /// </summary>
        STATUS_GROUP_EXISTS = 0xC0000065,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_GROUP
        /// MessageText:
        /// The specified group does not exist.
        /// </summary>
        STATUS_NO_SUCH_GROUP = 0xC0000066,

        /// <summary>
        /// MessageId: STATUS_MEMBER_IN_GROUP
        /// MessageText:
        /// The specified user account is already in the specified group account.
        /// Also used to indicate a group cannot be deleted because it contains a member.
        /// </summary>
        STATUS_MEMBER_IN_GROUP = 0xC0000067,

        /// <summary>
        /// MessageId: STATUS_MEMBER_NOT_IN_GROUP
        /// MessageText:
        /// The specified user account is not a member of the specified group account.
        /// </summary>
        STATUS_MEMBER_NOT_IN_GROUP = 0xC0000068,

        /// <summary>
        /// MessageId: STATUS_LAST_ADMIN
        /// MessageText:
        /// Indicates the requested operation would disable or delete the last remaining administration account.
        /// This is not allowed to prevent creating a situation in which the system cannot be administrated.
        /// </summary>
        STATUS_LAST_ADMIN = 0xC0000069,

        /// <summary>
        /// MessageId: STATUS_WRONG_PASSWORD
        /// MessageText:
        /// When trying to update a password, this return status indicates that the value provided as the current password is not correct.
        /// </summary>
        STATUS_WRONG_PASSWORD = 0xC000006A,

        /// <summary>
        /// MessageId: STATUS_ILL_FORMED_PASSWORD
        /// MessageText:
        /// When trying to update a password, this return status indicates that the value provided for the new password contains values that are not allowed in passwords.
        /// </summary>
        STATUS_ILL_FORMED_PASSWORD = 0xC000006B,

        /// <summary>
        /// MessageId: STATUS_PASSWORD_RESTRICTION
        /// MessageText:
        /// When trying to update a password, this status indicates that some password update rule has been violated. For example, the password may not meet length criteria.
        /// </summary>
        STATUS_PASSWORD_RESTRICTION = 0xC000006C,

        /// <summary>
        /// MessageId: STATUS_LOGON_FAILURE
        /// MessageText:
        /// The attempted logon is invalid. This is either due to a bad username or authentication information.
        /// </summary>
        STATUS_LOGON_FAILURE = 0xC000006D,

        /// <summary>
        /// MessageId: STATUS_ACCOUNT_RESTRICTION
        /// MessageText:
        /// Indicates a referenced user name and authentication information are valid, but some user account restriction has prevented successful authentication (such as time-of-day restrictions).
        /// </summary>
        STATUS_ACCOUNT_RESTRICTION = 0xC000006E,

        /// <summary>
        /// MessageId: STATUS_INVALID_LOGON_HOURS
        /// MessageText:
        /// The user account has time restrictions and may not be logged onto at this time.
        /// </summary>
        STATUS_INVALID_LOGON_HOURS = 0xC000006F,

        /// <summary>
        /// MessageId: STATUS_INVALID_WORKSTATION
        /// MessageText:
        /// The user account is restricted such that it may not be used to log on from the source workstation.
        /// </summary>
        STATUS_INVALID_WORKSTATION = 0xC0000070,

        /// <summary>
        /// MessageId: STATUS_PASSWORD_EXPIRED
        /// MessageText:
        /// The user account's password has expired.
        /// </summary>
        STATUS_PASSWORD_EXPIRED = 0xC0000071,

        /// <summary>
        /// MessageId: STATUS_ACCOUNT_DISABLED
        /// MessageText:
        /// The referenced account is currently disabled and may not be logged on to.
        /// </summary>
        STATUS_ACCOUNT_DISABLED = 0xC0000072,

        /// <summary>
        /// MessageId: STATUS_NONE_MAPPED
        /// MessageText:
        /// None of the information to be translated has been translated.
        /// </summary>
        STATUS_NONE_MAPPED = 0xC0000073,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_LUIDS_REQUESTED
        /// MessageText:
        /// The number of LUIDs requested may not be allocated with a single allocation.
        /// </summary>
        STATUS_TOO_MANY_LUIDS_REQUESTED = 0xC0000074,

        /// <summary>
        /// MessageId: STATUS_LUIDS_EXHAUSTED
        /// MessageText:
        /// Indicates there are no more LUIDs to allocate.
        /// </summary>
        STATUS_LUIDS_EXHAUSTED = 0xC0000075,

        /// <summary>
        /// MessageId: STATUS_INVALID_SUB_AUTHORITY
        /// MessageText:
        /// Indicates the sub-authority value is invalid for the particular use.
        /// </summary>
        STATUS_INVALID_SUB_AUTHORITY = 0xC0000076,

        /// <summary>
        /// MessageId: STATUS_INVALID_ACL
        /// MessageText:
        /// Indicates the ACL structure is not valid.
        /// </summary>
        STATUS_INVALID_ACL = 0xC0000077,

        /// <summary>
        /// MessageId: STATUS_INVALID_SID
        /// MessageText:
        /// Indicates the SID structure is not valid.
        /// </summary>
        STATUS_INVALID_SID = 0xC0000078,

        /// <summary>
        /// MessageId: STATUS_INVALID_SECURITY_DESCR
        /// MessageText:
        /// Indicates the SECURITY_DESCRIPTOR structure is not valid.
        /// </summary>
        STATUS_INVALID_SECURITY_DESCR = 0xC0000079,

        /// <summary>
        /// MessageId: STATUS_PROCEDURE_NOT_FOUND
        /// MessageText:
        /// Indicates the specified procedure address cannot be found in the DLL.
        /// </summary>
        STATUS_PROCEDURE_NOT_FOUND = 0xC000007A,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_FORMAT
        /// MessageText:
        /// {Bad Image}
        /// %hs is either not designed to run on Windows or it contains an error. Try installing the program again using the original installation media or contact your system administrator or the software vendor for support.
        /// </summary>
        STATUS_INVALID_IMAGE_FORMAT = 0xC000007B,

        /// <summary>
        /// MessageId: STATUS_NO_TOKEN
        /// MessageText:
        /// An attempt was made to reference a token that doesn't exist.
        /// This is typically done by referencing the token associated with a thread when the thread is not impersonating a client.
        /// </summary>
        STATUS_NO_TOKEN = 0xC000007C,

        /// <summary>
        /// MessageId: STATUS_BAD_INHERITANCE_ACL
        /// MessageText:
        /// Indicates that an attempt to build either an inherited ACL or ACE was not successful.
        /// This can be caused by a number of things. One of the more probable causes is the replacement of a CreatorId with an SID that didn't fit into the ACE or ACL.
        /// </summary>
        STATUS_BAD_INHERITANCE_ACL = 0xC000007D,

        /// <summary>
        /// MessageId: STATUS_RANGE_NOT_LOCKED
        /// MessageText:
        /// The range specified in NtUnlockFile was not locked.
        /// </summary>
        STATUS_RANGE_NOT_LOCKED = 0xC000007E,

        /// <summary>
        /// MessageId: STATUS_DISK_FULL
        /// MessageText:
        /// An operation failed because the disk was full.
        /// </summary>
        STATUS_DISK_FULL = 0xC000007F,

        /// <summary>
        /// MessageId: STATUS_SERVER_DISABLED
        /// MessageText:
        /// The GUID allocation server is [already] disabled at the moment.
        /// </summary>
        STATUS_SERVER_DISABLED = 0xC0000080,

        /// <summary>
        /// MessageId: STATUS_SERVER_NOT_DISABLED
        /// MessageText:
        /// The GUID allocation server is [already] enabled at the moment.
        /// </summary>
        STATUS_SERVER_NOT_DISABLED = 0xC0000081,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_GUIDS_REQUESTED
        /// MessageText:
        /// Too many GUIDs were requested from the allocation server at once.
        /// </summary>
        STATUS_TOO_MANY_GUIDS_REQUESTED = 0xC0000082,

        /// <summary>
        /// MessageId: STATUS_GUIDS_EXHAUSTED
        /// MessageText:
        /// The GUIDs could not be allocated because the Authority Agent was exhausted.
        /// </summary>
        STATUS_GUIDS_EXHAUSTED = 0xC0000083,

        /// <summary>
        /// MessageId: STATUS_INVALID_ID_AUTHORITY
        /// MessageText:
        /// The value provided was an invalid value for an identifier authority.
        /// </summary>
        STATUS_INVALID_ID_AUTHORITY = 0xC0000084,

        /// <summary>
        /// MessageId: STATUS_AGENTS_EXHAUSTED
        /// MessageText:
        /// There are no more authority agent values available for the given identifier authority value.
        /// </summary>
        STATUS_AGENTS_EXHAUSTED = 0xC0000085,

        /// <summary>
        /// MessageId: STATUS_INVALID_VOLUME_LABEL
        /// MessageText:
        /// An invalid volume label has been specified.
        /// </summary>
        STATUS_INVALID_VOLUME_LABEL = 0xC0000086,

        /// <summary>
        /// MessageId: STATUS_SECTION_NOT_EXTENDED
        /// MessageText:
        /// A mapped section could not be extended.
        /// </summary>
        STATUS_SECTION_NOT_EXTENDED = 0xC0000087,

        /// <summary>
        /// MessageId: STATUS_NOT_MAPPED_DATA
        /// MessageText:
        /// Specified section to flush does not map a data file.
        /// </summary>
        STATUS_NOT_MAPPED_DATA = 0xC0000088,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_DATA_NOT_FOUND
        /// MessageText:
        /// Indicates the specified image file did not contain a resource section.
        /// </summary>
        STATUS_RESOURCE_DATA_NOT_FOUND = 0xC0000089,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_TYPE_NOT_FOUND
        /// MessageText:
        /// Indicates the specified resource type cannot be found in the image file.
        /// </summary>
        STATUS_RESOURCE_TYPE_NOT_FOUND = 0xC000008A,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_NAME_NOT_FOUND
        /// MessageText:
        /// Indicates the specified resource name cannot be found in the image file.
        /// </summary>
        STATUS_RESOURCE_NAME_NOT_FOUND = 0xC000008B,

        /// <summary>
        /// MessageId: STATUS_ARRAY_BOUNDS_EXCEEDED
        /// MessageText:
        /// {EXCEPTION}
        /// Array bounds exceeded.
        /// </summary>
        STATUS_ARRAY_BOUNDS_EXCEEDED = 0xC000008C,

        /// <summary>
        /// MessageId: STATUS_FLOAT_DENORMAL_OPERAND
        /// MessageText:
        /// {EXCEPTION}
        /// Floating-point denormal operand.
        /// </summary>
        STATUS_FLOAT_DENORMAL_OPERAND = 0xC000008D,

        /// <summary>
        /// MessageId: STATUS_FLOAT_DIVIDE_BY_ZERO
        /// MessageText:
        /// {EXCEPTION}
        /// Floating-point division by zero.
        /// </summary>
        STATUS_FLOAT_DIVIDE_BY_ZERO = 0xC000008E,

        /// <summary>
        /// MessageId: STATUS_FLOAT_INEXACT_RESULT
        /// MessageText:
        /// {EXCEPTION}
        /// Floating-point inexact result.
        /// </summary>
        STATUS_FLOAT_INEXACT_RESULT = 0xC000008F,

        /// <summary>
        /// MessageId: STATUS_FLOAT_INVALID_OPERATION
        /// MessageText:
        /// {EXCEPTION}
        /// Floating-point invalid operation.
        /// </summary>
        STATUS_FLOAT_INVALID_OPERATION = 0xC0000090,

        /// <summary>
        /// MessageId: STATUS_FLOAT_OVERFLOW
        /// MessageText:
        /// {EXCEPTION}
        /// Floating-point overflow.
        /// </summary>
        STATUS_FLOAT_OVERFLOW = 0xC0000091,

        /// <summary>
        /// MessageId: STATUS_FLOAT_STACK_CHECK
        /// MessageText:
        /// {EXCEPTION}
        /// Floating-point stack check.
        /// </summary>
        STATUS_FLOAT_STACK_CHECK = 0xC0000092,

        /// <summary>
        /// MessageId: STATUS_FLOAT_UNDERFLOW
        /// MessageText:
        /// {EXCEPTION}
        /// Floating-point underflow.
        /// </summary>
        STATUS_FLOAT_UNDERFLOW = 0xC0000093,

        /// <summary>
        /// MessageId: STATUS_INTEGER_DIVIDE_BY_ZERO
        /// MessageText:
        /// {EXCEPTION}
        /// Integer division by zero.
        /// </summary>
        STATUS_INTEGER_DIVIDE_BY_ZERO = 0xC0000094,

        /// <summary>
        /// MessageId: STATUS_INTEGER_OVERFLOW
        /// MessageText:
        /// {EXCEPTION}
        /// Integer overflow.
        /// </summary>
        STATUS_INTEGER_OVERFLOW = 0xC0000095,

        /// <summary>
        /// MessageId: STATUS_PRIVILEGED_INSTRUCTION
        /// MessageText:
        /// {EXCEPTION}
        /// Privileged instruction.
        /// </summary>
        STATUS_PRIVILEGED_INSTRUCTION = 0xC0000096,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_PAGING_FILES
        /// MessageText:
        /// An attempt was made to install more paging files than the system supports.
        /// </summary>
        STATUS_TOO_MANY_PAGING_FILES = 0xC0000097,

        /// <summary>
        /// MessageId: STATUS_FILE_INVALID
        /// MessageText:
        /// The volume for a file has been externally altered such that the opened file is no longer valid.
        /// </summary>
        STATUS_FILE_INVALID = 0xC0000098,

        /// <summary>
        /// MessageId: STATUS_ALLOTTED_SPACE_EXCEEDED
        /// MessageText:
        /// When a block of memory is allotted for future updates, such as the memory allocated to hold discretionary access control and primary group information, successive updates may exceed the amount of memory originally allotted.
        /// Since quota may already have been charged to several processes which have handles to the object, it is not reasonable to alter the size of the allocated memory.
        /// Instead, a request that requires more memory than has been allotted must fail and the STATUS_ALLOTED_SPACE_EXCEEDED error returned.
        /// </summary>
        STATUS_ALLOTTED_SPACE_EXCEEDED = 0xC0000099,

        /// <summary>
        /// MessageId: STATUS_INSUFFICIENT_RESOURCES
        /// MessageText:
        /// Insufficient system resources exist to complete the API.
        /// </summary>
        STATUS_INSUFFICIENT_RESOURCES = 0xC000009A,

        /// <summary>
        /// MessageId: STATUS_DFS_EXIT_PATH_FOUND
        /// MessageText:
        /// An attempt has been made to open a DFS exit path control file.
        /// </summary>
        STATUS_DFS_EXIT_PATH_FOUND = 0xC000009B,

        /// <summary>
        /// MessageId: STATUS_DEVICE_DATA_ERROR
        /// MessageText:
        ///  STATUS_DEVICE_DATA_ERROR
        /// </summary>
        STATUS_DEVICE_DATA_ERROR = 0xC000009C,

        /// <summary>
        /// MessageId: STATUS_DEVICE_NOT_CONNECTED
        /// MessageText:
        ///  STATUS_DEVICE_NOT_CONNECTED
        /// </summary>
        STATUS_DEVICE_NOT_CONNECTED = 0xC000009D,

        /// <summary>
        /// MessageId: STATUS_DEVICE_POWER_FAILURE
        /// MessageText:
        ///  STATUS_DEVICE_POWER_FAILURE
        /// </summary>
        STATUS_DEVICE_POWER_FAILURE = 0xC000009E,

        /// <summary>
        /// MessageId: STATUS_FREE_VM_NOT_AT_BASE
        /// MessageText:
        /// Virtual memory cannot be freed as base address is not the base of the region and a region size of zero was specified.
        /// </summary>
        STATUS_FREE_VM_NOT_AT_BASE = 0xC000009F,

        /// <summary>
        /// MessageId: STATUS_MEMORY_NOT_ALLOCATED
        /// MessageText:
        /// An attempt was made to free virtual memory which is not allocated.
        /// </summary>
        STATUS_MEMORY_NOT_ALLOCATED = 0xC00000A0,

        /// <summary>
        /// MessageId: STATUS_WORKING_SET_QUOTA
        /// MessageText:
        /// The working set is not big enough to allow the requested pages to be locked.
        /// </summary>
        STATUS_WORKING_SET_QUOTA = 0xC00000A1,

        /// <summary>
        /// MessageId: STATUS_MEDIA_WRITE_PROTECTED
        /// MessageText:
        /// {Write Protect Error}
        /// The disk cannot be written to because it is write protected.
        /// Please remove the write protection from the volume %hs in drive %hs.
        /// </summary>
        STATUS_MEDIA_WRITE_PROTECTED = 0xC00000A2,

        /// <summary>
        /// MessageId: STATUS_DEVICE_NOT_READY
        /// MessageText:
        /// {Drive Not Ready}
        /// The drive is not ready for use; its door may be open.
        /// Please check drive %hs and make sure that a disk is inserted and that the drive door is closed.
        /// </summary>
        STATUS_DEVICE_NOT_READY = 0xC00000A3,

        /// <summary>
        /// MessageId: STATUS_INVALID_GROUP_ATTRIBUTES
        /// MessageText:
        /// The specified attributes are invalid, or incompatible with the attributes for the group as a whole.
        /// </summary>
        STATUS_INVALID_GROUP_ATTRIBUTES = 0xC00000A4,

        /// <summary>
        /// MessageId: STATUS_BAD_IMPERSONATION_LEVEL
        /// MessageText:
        /// A specified impersonation level is invalid.
        /// Also used to indicate a required impersonation level was not provided.
        /// </summary>
        STATUS_BAD_IMPERSONATION_LEVEL = 0xC00000A5,

        /// <summary>
        /// MessageId: STATUS_CANT_OPEN_ANONYMOUS
        /// MessageText:
        /// An attempt was made to open an Anonymous level token.
        /// Anonymous tokens may not be opened.
        /// </summary>
        STATUS_CANT_OPEN_ANONYMOUS = 0xC00000A6,

        /// <summary>
        /// MessageId: STATUS_BAD_VALIDATION_CLASS
        /// MessageText:
        /// The validation information class requested was invalid.
        /// </summary>
        STATUS_BAD_VALIDATION_CLASS = 0xC00000A7,

        /// <summary>
        /// MessageId: STATUS_BAD_TOKEN_TYPE
        /// MessageText:
        /// The type of a token object is inappropriate for its attempted use.
        /// </summary>
        STATUS_BAD_TOKEN_TYPE = 0xC00000A8,

        /// <summary>
        /// MessageId: STATUS_BAD_MASTER_BOOT_RECORD
        /// MessageText:
        /// The type of a token object is inappropriate for its attempted use.
        /// </summary>
        STATUS_BAD_MASTER_BOOT_RECORD = 0xC00000A9,

        /// <summary>
        /// MessageId: STATUS_INSTRUCTION_MISALIGNMENT
        /// MessageText:
        /// An attempt was made to execute an instruction at an unaligned address and the host system does not support unaligned instruction references.
        /// </summary>
        STATUS_INSTRUCTION_MISALIGNMENT = 0xC00000AA,

        /// <summary>
        /// MessageId: STATUS_INSTANCE_NOT_AVAILABLE
        /// MessageText:
        /// The maximum named pipe instance count has been reached.
        /// </summary>
        STATUS_INSTANCE_NOT_AVAILABLE = 0xC00000AB,

        /// <summary>
        /// MessageId: STATUS_PIPE_NOT_AVAILABLE
        /// MessageText:
        /// An instance of a named pipe cannot be found in the listening state.
        /// </summary>
        STATUS_PIPE_NOT_AVAILABLE = 0xC00000AC,

        /// <summary>
        /// MessageId: STATUS_INVALID_PIPE_STATE
        /// MessageText:
        /// The named pipe is not in the connected or closing state.
        /// </summary>
        STATUS_INVALID_PIPE_STATE = 0xC00000AD,

        /// <summary>
        /// MessageId: STATUS_PIPE_BUSY
        /// MessageText:
        /// The specified pipe is set to complete operations and there are current I/O operations queued so it cannot be changed to queue operations.
        /// </summary>
        STATUS_PIPE_BUSY = 0xC00000AE,

        /// <summary>
        /// MessageId: STATUS_ILLEGAL_FUNCTION
        /// MessageText:
        /// The specified handle is not open to the server end of the named pipe.
        /// </summary>
        STATUS_ILLEGAL_FUNCTION = 0xC00000AF,

        /// <summary>
        /// MessageId: STATUS_PIPE_DISCONNECTED
        /// MessageText:
        /// The specified named pipe is in the disconnected state.
        /// </summary>
        STATUS_PIPE_DISCONNECTED = 0xC00000B0,

        /// <summary>
        /// MessageId: STATUS_PIPE_CLOSING
        /// MessageText:
        /// The specified named pipe is in the closing state.
        /// </summary>
        STATUS_PIPE_CLOSING = 0xC00000B1,

        /// <summary>
        /// MessageId: STATUS_PIPE_CONNECTED
        /// MessageText:
        /// The specified named pipe is in the connected state.
        /// </summary>
        STATUS_PIPE_CONNECTED = 0xC00000B2,

        /// <summary>
        /// MessageId: STATUS_PIPE_LISTENING
        /// MessageText:
        /// The specified named pipe is in the listening state.
        /// </summary>
        STATUS_PIPE_LISTENING = 0xC00000B3,

        /// <summary>
        /// MessageId: STATUS_INVALID_READ_MODE
        /// MessageText:
        /// The specified named pipe is not in message mode.
        /// </summary>
        STATUS_INVALID_READ_MODE = 0xC00000B4,

        /// <summary>
        /// MessageId: STATUS_IO_TIMEOUT
        /// MessageText:
        /// {Device Timeout}
        /// The specified I/O operation on %hs was not completed before the time-out period expired.
        /// </summary>
        STATUS_IO_TIMEOUT = 0xC00000B5,

        /// <summary>
        /// MessageId: STATUS_FILE_FORCED_CLOSED
        /// MessageText:
        /// The specified file has been closed by another process.
        /// </summary>
        STATUS_FILE_FORCED_CLOSED = 0xC00000B6,

        /// <summary>
        /// MessageId: STATUS_PROFILING_NOT_STARTED
        /// MessageText:
        /// Profiling not started.
        /// </summary>
        STATUS_PROFILING_NOT_STARTED = 0xC00000B7,

        /// <summary>
        /// MessageId: STATUS_PROFILING_NOT_STOPPED
        /// MessageText:
        /// Profiling not stopped.
        /// </summary>
        STATUS_PROFILING_NOT_STOPPED = 0xC00000B8,

        /// <summary>
        /// MessageId: STATUS_COULD_NOT_INTERPRET
        /// MessageText:
        /// The passed ACL did not contain the minimum required information.
        /// </summary>
        STATUS_COULD_NOT_INTERPRET = 0xC00000B9,

        /// <summary>
        /// MessageId: STATUS_FILE_IS_A_DIRECTORY
        /// MessageText:
        /// The file that was specified as a target is a directory and the caller specified that it could be anything but a directory.
        /// </summary>
        STATUS_FILE_IS_A_DIRECTORY = 0xC00000BA,

        /// <summary>
        /// Network specific errors.
        /// MessageId: STATUS_NOT_SUPPORTED
        /// MessageText:
        /// The request is not supported.
        /// </summary>
        STATUS_NOT_SUPPORTED = 0xC00000BB,

        /// <summary>
        /// MessageId: STATUS_REMOTE_NOT_LISTENING
        /// MessageText:
        /// This remote computer is not listening.
        /// </summary>
        STATUS_REMOTE_NOT_LISTENING = 0xC00000BC,

        /// <summary>
        /// MessageId: STATUS_DUPLICATE_NAME
        /// MessageText:
        /// A duplicate name exists on the network.
        /// </summary>
        STATUS_DUPLICATE_NAME = 0xC00000BD,

        /// <summary>
        /// MessageId: STATUS_BAD_NETWORK_PATH
        /// MessageText:
        /// The network path cannot be located.
        /// </summary>
        STATUS_BAD_NETWORK_PATH = 0xC00000BE,

        /// <summary>
        /// MessageId: STATUS_NETWORK_BUSY
        /// MessageText:
        /// The network is busy.
        /// </summary>
        STATUS_NETWORK_BUSY = 0xC00000BF,

        /// <summary>
        /// MessageId: STATUS_DEVICE_DOES_NOT_EXIST
        /// MessageText:
        /// This device does not exist.
        /// </summary>
        STATUS_DEVICE_DOES_NOT_EXIST = 0xC00000C0,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_COMMANDS
        /// MessageText:
        /// The network BIOS command limit has been reached.
        /// </summary>
        STATUS_TOO_MANY_COMMANDS = 0xC00000C1,

        /// <summary>
        /// MessageId: STATUS_ADAPTER_HARDWARE_ERROR
        /// MessageText:
        /// An I/O adapter hardware error has occurred.
        /// </summary>
        STATUS_ADAPTER_HARDWARE_ERROR = 0xC00000C2,

        /// <summary>
        /// MessageId: STATUS_INVALID_NETWORK_RESPONSE
        /// MessageText:
        /// The network responded incorrectly.
        /// </summary>
        STATUS_INVALID_NETWORK_RESPONSE = 0xC00000C3,

        /// <summary>
        /// MessageId: STATUS_UNEXPECTED_NETWORK_ERROR
        /// MessageText:
        /// An unexpected network error occurred.
        /// </summary>
        STATUS_UNEXPECTED_NETWORK_ERROR = 0xC00000C4,

        /// <summary>
        /// MessageId: STATUS_BAD_REMOTE_ADAPTER
        /// MessageText:
        /// The remote adapter is not compatible.
        /// </summary>
        STATUS_BAD_REMOTE_ADAPTER = 0xC00000C5,

        /// <summary>
        /// MessageId: STATUS_PRINT_QUEUE_FULL
        /// MessageText:
        /// The printer queue is full.
        /// </summary>
        STATUS_PRINT_QUEUE_FULL = 0xC00000C6,

        /// <summary>
        /// MessageId: STATUS_NO_SPOOL_SPACE
        /// MessageText:
        /// Space to store the file waiting to be printed is not available on the server.
        /// </summary>
        STATUS_NO_SPOOL_SPACE = 0xC00000C7,

        /// <summary>
        /// MessageId: STATUS_PRINT_CANCELLED
        /// MessageText:
        /// The requested print file has been canceled.
        /// </summary>
        STATUS_PRINT_CANCELLED = 0xC00000C8,

        /// <summary>
        /// MessageId: STATUS_NETWORK_NAME_DELETED
        /// MessageText:
        /// The network name was deleted.
        /// </summary>
        STATUS_NETWORK_NAME_DELETED = 0xC00000C9,

        /// <summary>
        /// MessageId: STATUS_NETWORK_ACCESS_DENIED
        /// MessageText:
        /// Network access is denied.
        /// </summary>
        STATUS_NETWORK_ACCESS_DENIED = 0xC00000CA,

        /// <summary>
        /// MessageId: STATUS_BAD_DEVICE_TYPE
        /// MessageText:
        /// {Incorrect Network Resource Type}
        /// The specified device type (LPT, for example) conflicts with the actual device type on the remote resource.
        /// </summary>
        STATUS_BAD_DEVICE_TYPE = 0xC00000CB,

        /// <summary>
        /// MessageId: STATUS_BAD_NETWORK_NAME
        /// MessageText:
        /// {Network Name Not Found}
        /// The specified share name cannot be found on the remote server.
        /// </summary>
        STATUS_BAD_NETWORK_NAME = 0xC00000CC,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_NAMES
        /// MessageText:
        /// The name limit for the local computer network adapter card was exceeded.
        /// </summary>
        STATUS_TOO_MANY_NAMES = 0xC00000CD,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_SESSIONS
        /// MessageText:
        /// The network BIOS session limit was exceeded.
        /// </summary>
        STATUS_TOO_MANY_SESSIONS = 0xC00000CE,

        /// <summary>
        /// MessageId: STATUS_SHARING_PAUSED
        /// MessageText:
        /// File sharing has been temporarily paused.
        /// </summary>
        STATUS_SHARING_PAUSED = 0xC00000CF,

        /// <summary>
        /// MessageId: STATUS_REQUEST_NOT_ACCEPTED
        /// MessageText:
        /// No more connections can be made to this remote computer at this time because there are already as many connections as the computer can accept.
        /// </summary>
        STATUS_REQUEST_NOT_ACCEPTED = 0xC00000D0,

        /// <summary>
        /// MessageId: STATUS_REDIRECTOR_PAUSED
        /// MessageText:
        /// Print or disk redirection is temporarily paused.
        /// </summary>
        STATUS_REDIRECTOR_PAUSED = 0xC00000D1,

        /// <summary>
        /// MessageId: STATUS_NET_WRITE_FAULT
        /// MessageText:
        /// A network data fault occurred.
        /// </summary>
        STATUS_NET_WRITE_FAULT = 0xC00000D2,

        /// <summary>
        /// MessageId: STATUS_PROFILING_AT_LIMIT
        /// MessageText:
        /// The number of active profiling objects is at the maximum and no more may be started.
        /// </summary>
        STATUS_PROFILING_AT_LIMIT = 0xC00000D3,

        /// <summary>
        /// MessageId: STATUS_NOT_SAME_DEVICE
        /// MessageText:
        /// {Incorrect Volume}
        /// The target file of a rename request is located on a different device than the source of the rename request.
        /// </summary>
        STATUS_NOT_SAME_DEVICE = 0xC00000D4,

        /// <summary>
        /// MessageId: STATUS_FILE_RENAMED
        /// MessageText:
        /// The file specified has been renamed and thus cannot be modified.
        /// </summary>
        STATUS_FILE_RENAMED = 0xC00000D5,

        /// <summary>
        /// MessageId: STATUS_VIRTUAL_CIRCUIT_CLOSED
        /// MessageText:
        /// {Network Request Timeout}
        /// The session with a remote server has been disconnected because the time-out interval for a request has expired.
        /// </summary>
        STATUS_VIRTUAL_CIRCUIT_CLOSED = 0xC00000D6,

        /// <summary>
        /// MessageId: STATUS_NO_SECURITY_ON_OBJECT
        /// MessageText:
        /// Indicates an attempt was made to operate on the security of an object that does not have security associated with it.
        /// </summary>
        STATUS_NO_SECURITY_ON_OBJECT = 0xC00000D7,

        /// <summary>
        /// MessageId: STATUS_CANT_WAIT
        /// MessageText:
        /// Used to indicate that an operation cannot continue without blocking for I/O.
        /// </summary>
        STATUS_CANT_WAIT = 0xC00000D8,

        /// <summary>
        /// MessageId: STATUS_PIPE_EMPTY
        /// MessageText:
        /// Used to indicate that a read operation was done on an empty pipe.
        /// </summary>
        STATUS_PIPE_EMPTY = 0xC00000D9,

        /// <summary>
        /// MessageId: STATUS_CANT_ACCESS_DOMAIN_INFO
        /// MessageText:
        /// Configuration information could not be read from the domain controller, either because the machine is unavailable, or access has been denied.
        /// </summary>
        STATUS_CANT_ACCESS_DOMAIN_INFO = 0xC00000DA,

        /// <summary>
        /// MessageId: STATUS_CANT_TERMINATE_SELF
        /// MessageText:
        /// Indicates that a thread attempted to terminate itself by default (called NtTerminateThread with NULL) and it was the last thread in the current process.
        /// </summary>
        STATUS_CANT_TERMINATE_SELF = 0xC00000DB,

        /// <summary>
        /// MessageId: STATUS_INVALID_SERVER_STATE
        /// MessageText:
        /// Indicates the Sam Server was in the wrong state to perform the desired operation.
        /// </summary>
        STATUS_INVALID_SERVER_STATE = 0xC00000DC,

        /// <summary>
        /// MessageId: STATUS_INVALID_DOMAIN_STATE
        /// MessageText:
        /// Indicates the Domain was in the wrong state to perform the desired operation.
        /// </summary>
        STATUS_INVALID_DOMAIN_STATE = 0xC00000DD,

        /// <summary>
        /// MessageId: STATUS_INVALID_DOMAIN_ROLE
        /// MessageText:
        /// This operation is only allowed for the Primary Domain Controller of the domain.
        /// </summary>
        STATUS_INVALID_DOMAIN_ROLE = 0xC00000DE,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_DOMAIN
        /// MessageText:
        /// The specified Domain did not exist.
        /// </summary>
        STATUS_NO_SUCH_DOMAIN = 0xC00000DF,

        /// <summary>
        /// MessageId: STATUS_DOMAIN_EXISTS
        /// MessageText:
        /// The specified Domain already exists.
        /// </summary>
        STATUS_DOMAIN_EXISTS = 0xC00000E0,

        /// <summary>
        /// MessageId: STATUS_DOMAIN_LIMIT_EXCEEDED
        /// MessageText:
        /// An attempt was made to exceed the limit on the number of domains per server for this release.
        /// </summary>
        STATUS_DOMAIN_LIMIT_EXCEEDED = 0xC00000E1,

        /// <summary>
        /// MessageId: STATUS_OPLOCK_NOT_GRANTED
        /// MessageText:
        /// Error status returned when oplock request is denied.
        /// </summary>
        STATUS_OPLOCK_NOT_GRANTED = 0xC00000E2,

        /// <summary>
        /// MessageId: STATUS_INVALID_OPLOCK_PROTOCOL
        /// MessageText:
        /// Error status returned when an invalid oplock acknowledgment is received by a file system.
        /// </summary>
        STATUS_INVALID_OPLOCK_PROTOCOL = 0xC00000E3,

        /// <summary>
        /// MessageId: STATUS_INTERNAL_DB_CORRUPTION
        /// MessageText:
        /// This error indicates that the requested operation cannot be completed due to a catastrophic media failure or on-disk data structure corruption.
        /// </summary>
        STATUS_INTERNAL_DB_CORRUPTION = 0xC00000E4,

        /// <summary>
        /// MessageId: STATUS_INTERNAL_ERROR
        /// MessageText:
        /// An internal error occurred.
        /// </summary>
        STATUS_INTERNAL_ERROR = 0xC00000E5,

        /// <summary>
        /// MessageId: STATUS_GENERIC_NOT_MAPPED
        /// MessageText:
        /// Indicates generic access types were contained in an access mask which should already be mapped to non-generic access types.
        /// </summary>
        STATUS_GENERIC_NOT_MAPPED = 0xC00000E6,

        /// <summary>
        /// MessageId: STATUS_BAD_DESCRIPTOR_FORMAT
        /// MessageText:
        /// Indicates a security descriptor is not in the necessary format (absolute or self-relative).
        /// </summary>
        STATUS_BAD_DESCRIPTOR_FORMAT = 0xC00000E7,

        /// <summary>
        /// Status codes raised by the Cache Manager which must be considered as
        /// "expected" by its callers.
        /// MessageId: STATUS_INVALID_USER_BUFFER
        /// MessageText:
        /// An access to a user buffer failed at an "expected" point in time.
        /// This code is defined since the caller does not want to accept STATUS_ACCESS_VIOLATION in its filter.
        /// </summary>
        STATUS_INVALID_USER_BUFFER = 0xC00000E8,

        /// <summary>
        /// MessageId: STATUS_UNEXPECTED_IO_ERROR
        /// MessageText:
        /// If an I/O error is returned which is not defined in the standard FsRtl filter, it is converted to the following error which is guaranteed to be in the filter.
        /// In this case information is lost, however, the filter correctly handles the exception.
        /// </summary>
        STATUS_UNEXPECTED_IO_ERROR = 0xC00000E9,

        /// <summary>
        /// MessageId: STATUS_UNEXPECTED_MM_CREATE_ERR
        /// MessageText:
        /// If an MM error is returned which is not defined in the standard FsRtl filter, it is converted to one of the following errors which is guaranteed to be in the filter.
        /// In this case information is lost, however, the filter correctly handles the exception.
        /// </summary>
        STATUS_UNEXPECTED_MM_CREATE_ERR = 0xC00000EA,

        /// <summary>
        /// MessageId: STATUS_UNEXPECTED_MM_MAP_ERROR
        /// MessageText:
        /// If an MM error is returned which is not defined in the standard FsRtl filter, it is converted to one of the following errors which is guaranteed to be in the filter.
        /// In this case information is lost, however, the filter correctly handles the exception.
        /// </summary>
        STATUS_UNEXPECTED_MM_MAP_ERROR = 0xC00000EB,

        /// <summary>
        /// MessageId: STATUS_UNEXPECTED_MM_EXTEND_ERR
        /// MessageText:
        /// If an MM error is returned which is not defined in the standard FsRtl filter, it is converted to one of the following errors which is guaranteed to be in the filter.
        /// In this case information is lost, however, the filter correctly handles the exception.
        /// </summary>
        STATUS_UNEXPECTED_MM_EXTEND_ERR = 0xC00000EC,

        /// <summary>
        /// MessageId: STATUS_NOT_LOGON_PROCESS
        /// MessageText:
        /// The requested action is restricted for use by logon processes only. The calling process has not registered as a logon process.
        /// </summary>
        STATUS_NOT_LOGON_PROCESS = 0xC00000ED,

        /// <summary>
        /// MessageId: STATUS_LOGON_SESSION_EXISTS
        /// MessageText:
        /// An attempt has been made to start a new session manager or LSA logon session with an ID that is already in use.
        /// </summary>
        STATUS_LOGON_SESSION_EXISTS = 0xC00000EE,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_1
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the first argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_1 = 0xC00000EF,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_2
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the second argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_2 = 0xC00000F0,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_3
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the third argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_3 = 0xC00000F1,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_4
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the fourth argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_4 = 0xC00000F2,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_5
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the fifth argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_5 = 0xC00000F3,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_6
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the sixth argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_6 = 0xC00000F4,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_7
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the seventh argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_7 = 0xC00000F5,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_8
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the eighth argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_8 = 0xC00000F6,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_9
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the ninth argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_9 = 0xC00000F7,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_10
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the tenth argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_10 = 0xC00000F8,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_11
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the eleventh argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_11 = 0xC00000F9,

        /// <summary>
        /// MessageId: STATUS_INVALID_PARAMETER_12
        /// MessageText:
        /// An invalid parameter was passed to a service or function as the twelfth argument.
        /// </summary>
        STATUS_INVALID_PARAMETER_12 = 0xC00000FA,

        /// <summary>
        /// MessageId: STATUS_REDIRECTOR_NOT_STARTED
        /// MessageText:
        /// An attempt was made to access a network file, but the network software was not yet started.
        /// </summary>
        STATUS_REDIRECTOR_NOT_STARTED = 0xC00000FB,

        /// <summary>
        /// MessageId: STATUS_REDIRECTOR_STARTED
        /// MessageText:
        /// An attempt was made to start the redirector, but the redirector has already been started.
        /// </summary>
        STATUS_REDIRECTOR_STARTED = 0xC00000FC,

        /// <summary>
        /// MessageId: STATUS_STACK_OVERFLOW
        /// MessageText:
        /// A new guard page for the stack cannot be created.
        /// </summary>
        STATUS_STACK_OVERFLOW = 0xC00000FD,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_PACKAGE
        /// MessageText:
        /// A specified authentication package is unknown.
        /// </summary>
        STATUS_NO_SUCH_PACKAGE = 0xC00000FE,

        /// <summary>
        /// MessageId: STATUS_BAD_FUNCTION_TABLE
        /// MessageText:
        /// A malformed function table was encountered during an unwind operation.
        /// </summary>
        STATUS_BAD_FUNCTION_TABLE = 0xC00000FF,

        /// <summary>
        /// MessageId: STATUS_VARIABLE_NOT_FOUND
        /// MessageText:
        /// Indicates the specified environment variable name was not found in the specified environment block.
        /// </summary>
        STATUS_VARIABLE_NOT_FOUND = 0xC0000100,

        /// <summary>
        /// MessageId: STATUS_DIRECTORY_NOT_EMPTY
        /// MessageText:
        /// Indicates that the directory trying to be deleted is not empty.
        /// </summary>
        STATUS_DIRECTORY_NOT_EMPTY = 0xC0000101,

        /// <summary>
        /// MessageId: STATUS_FILE_CORRUPT_ERROR
        /// MessageText:
        /// {Corrupt File}
        /// The file or directory %hs is corrupt and unreadable.
        /// Please run the Chkdsk utility.
        /// </summary>
        STATUS_FILE_CORRUPT_ERROR = 0xC0000102,

        /// <summary>
        /// MessageId: STATUS_NOT_A_DIRECTORY
        /// MessageText:
        /// A requested opened file is not a directory.
        /// </summary>
        STATUS_NOT_A_DIRECTORY = 0xC0000103,

        /// <summary>
        /// MessageId: STATUS_BAD_LOGON_SESSION_STATE
        /// MessageText:
        /// The logon session is not in a state that is consistent with the requested operation.
        /// </summary>
        STATUS_BAD_LOGON_SESSION_STATE = 0xC0000104,

        /// <summary>
        /// MessageId: STATUS_LOGON_SESSION_COLLISION
        /// MessageText:
        /// An internal LSA error has occurred. An authentication package has requested the creation of a Logon Session but the ID of an already existing Logon Session has been specified.
        /// </summary>
        STATUS_LOGON_SESSION_COLLISION = 0xC0000105,

        /// <summary>
        /// MessageId: STATUS_NAME_TOO_LONG
        /// MessageText:
        /// A specified name string is too long for its intended use.
        /// </summary>
        STATUS_NAME_TOO_LONG = 0xC0000106,

        /// <summary>
        /// MessageId: STATUS_FILES_OPEN
        /// MessageText:
        /// The user attempted to force close the files on a redirected drive, but there were opened files on the drive, and the user did not specify a sufficient level of force.
        /// </summary>
        STATUS_FILES_OPEN = 0xC0000107,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_IN_USE
        /// MessageText:
        /// The user attempted to force close the files on a redirected drive, but there were opened directories on the drive, and the user did not specify a sufficient level of force.
        /// </summary>
        STATUS_CONNECTION_IN_USE = 0xC0000108,

        /// <summary>
        /// MessageId: STATUS_MESSAGE_NOT_FOUND
        /// MessageText:
        /// RtlFindMessage could not locate the requested message ID in the message table resource.
        /// </summary>
        STATUS_MESSAGE_NOT_FOUND = 0xC0000109,

        /// <summary>
        /// MessageId: STATUS_PROCESS_IS_TERMINATING
        /// MessageText:
        /// An attempt was made to access an exiting process.
        /// </summary>
        STATUS_PROCESS_IS_TERMINATING = 0xC000010A,

        /// <summary>
        /// MessageId: STATUS_INVALID_LOGON_TYPE
        /// MessageText:
        /// Indicates an invalid value has been provided for the LogonType requested.
        /// </summary>
        STATUS_INVALID_LOGON_TYPE = 0xC000010B,

        /// <summary>
        /// MessageId: STATUS_NO_GUID_TRANSLATION
        /// MessageText:
        /// Indicates that an attempt was made to assign protection to a file system file or directory and one of the SIDs in the security descriptor could not be translated into a GUID that could be stored by the file system.
        /// This causes the protection attempt to fail, which may cause a file creation attempt to fail.
        /// </summary>
        STATUS_NO_GUID_TRANSLATION = 0xC000010C,

        /// <summary>
        /// MessageId: STATUS_CANNOT_IMPERSONATE
        /// MessageText:
        /// Indicates that an attempt has been made to impersonate via a named pipe that has not yet been read from.
        /// </summary>
        STATUS_CANNOT_IMPERSONATE = 0xC000010D,

        /// <summary>
        /// MessageId: STATUS_IMAGE_ALREADY_LOADED
        /// MessageText:
        /// Indicates that the specified image is already loaded.
        /// </summary>
        STATUS_IMAGE_ALREADY_LOADED = 0xC000010E,

        /// <summary>
        /// MessageId: STATUS_ABIOS_NOT_PRESENT
        /// MessageText:
        ///  STATUS_ABIOS_NOT_PRESENT
        /// </summary>
        STATUS_ABIOS_NOT_PRESENT = 0xC000010F,

        /// <summary>
        /// MessageId: STATUS_ABIOS_LID_NOT_EXIST
        /// MessageText:
        ///  STATUS_ABIOS_LID_NOT_EXIST
        /// </summary>
        STATUS_ABIOS_LID_NOT_EXIST = 0xC0000110,

        /// <summary>
        /// MessageId: STATUS_ABIOS_LID_ALREADY_OWNED
        /// MessageText:
        ///  STATUS_ABIOS_LID_ALREADY_OWNED
        /// </summary>
        STATUS_ABIOS_LID_ALREADY_OWNED = 0xC0000111,

        /// <summary>
        /// MessageId: STATUS_ABIOS_NOT_LID_OWNER
        /// MessageText:
        ///  STATUS_ABIOS_NOT_LID_OWNER
        /// </summary>
        STATUS_ABIOS_NOT_LID_OWNER = 0xC0000112,

        /// <summary>
        /// MessageId: STATUS_ABIOS_INVALID_COMMAND
        /// MessageText:
        ///  STATUS_ABIOS_INVALID_COMMAND
        /// </summary>
        STATUS_ABIOS_INVALID_COMMAND = 0xC0000113,

        /// <summary>
        /// MessageId: STATUS_ABIOS_INVALID_LID
        /// MessageText:
        ///  STATUS_ABIOS_INVALID_LID
        /// </summary>
        STATUS_ABIOS_INVALID_LID = 0xC0000114,

        /// <summary>
        /// MessageId: STATUS_ABIOS_SELECTOR_NOT_AVAILABLE
        /// MessageText:
        ///  STATUS_ABIOS_SELECTOR_NOT_AVAILABLE
        /// </summary>
        STATUS_ABIOS_SELECTOR_NOT_AVAILABLE = 0xC0000115,

        /// <summary>
        /// MessageId: STATUS_ABIOS_INVALID_SELECTOR
        /// MessageText:
        ///  STATUS_ABIOS_INVALID_SELECTOR
        /// </summary>
        STATUS_ABIOS_INVALID_SELECTOR = 0xC0000116,

        /// <summary>
        /// MessageId: STATUS_NO_LDT
        /// MessageText:
        /// Indicates that an attempt was made to change the size of the LDT for a process that has no LDT.
        /// </summary>
        STATUS_NO_LDT = 0xC0000117,

        /// <summary>
        /// MessageId: STATUS_INVALID_LDT_SIZE
        /// MessageText:
        /// Indicates that an attempt was made to grow an LDT by setting its size, or that the size was not an even number of selectors.
        /// </summary>
        STATUS_INVALID_LDT_SIZE = 0xC0000118,

        /// <summary>
        /// MessageId: STATUS_INVALID_LDT_OFFSET
        /// MessageText:
        /// Indicates that the starting value for the LDT information was not an integral multiple of the selector size.
        /// </summary>
        STATUS_INVALID_LDT_OFFSET = 0xC0000119,

        /// <summary>
        /// MessageId: STATUS_INVALID_LDT_DESCRIPTOR
        /// MessageText:
        /// Indicates that the user supplied an invalid descriptor when trying to set up Ldt descriptors.
        /// </summary>
        STATUS_INVALID_LDT_DESCRIPTOR = 0xC000011A,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_NE_FORMAT
        /// MessageText:
        /// The specified image file did not have the correct format. It appears to be NE format.
        /// </summary>
        STATUS_INVALID_IMAGE_NE_FORMAT = 0xC000011B,

        /// <summary>
        /// MessageId: STATUS_RXACT_INVALID_STATE
        /// MessageText:
        /// Indicates that the transaction state of a registry sub-tree is incompatible with the requested operation.
        /// For example, a request has been made to start a new transaction with one already in progress,
        /// or a request has been made to apply a transaction when one is not currently in progress.
        /// </summary>
        STATUS_RXACT_INVALID_STATE = 0xC000011C,

        /// <summary>
        /// MessageId: STATUS_RXACT_COMMIT_FAILURE
        /// MessageText:
        /// Indicates an error has occurred during a registry transaction commit.
        /// The database has been left in an unknown, but probably inconsistent, state.
        /// The state of the registry transaction is left as COMMITTING.
        /// </summary>
        STATUS_RXACT_COMMIT_FAILURE = 0xC000011D,

        /// <summary>
        /// MessageId: STATUS_MAPPED_FILE_SIZE_ZERO
        /// MessageText:
        /// An attempt was made to map a file of size zero with the maximum size specified as zero.
        /// </summary>
        STATUS_MAPPED_FILE_SIZE_ZERO = 0xC000011E,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_OPENED_FILES
        /// MessageText:
        /// Too many files are opened on a remote server.
        /// This error should only be returned by the Windows redirector on a remote drive.
        /// </summary>
        STATUS_TOO_MANY_OPENED_FILES = 0xC000011F,

        /// <summary>
        /// MessageId: STATUS_CANCELLED
        /// MessageText:
        /// The I/O request was canceled.
        /// </summary>
        STATUS_CANCELLED = 0xC0000120,

        /// <summary>
        /// MessageId: STATUS_CANNOT_DELETE
        /// MessageText:
        /// An attempt has been made to remove a file or directory that cannot be deleted.
        /// </summary>
        STATUS_CANNOT_DELETE = 0xC0000121,

        /// <summary>
        /// MessageId: STATUS_INVALID_COMPUTER_NAME
        /// MessageText:
        /// Indicates a name specified as a remote computer name is syntactically invalid.
        /// </summary>
        STATUS_INVALID_COMPUTER_NAME = 0xC0000122,

        /// <summary>
        /// MessageId: STATUS_FILE_DELETED
        /// MessageText:
        /// An I/O request other than close was performed on a file after it has been deleted,
        /// which can only happen to a request which did not complete before the last handle was closed via NtClose.
        /// </summary>
        STATUS_FILE_DELETED = 0xC0000123,

        /// <summary>
        /// MessageId: STATUS_SPECIAL_ACCOUNT
        /// MessageText:
        /// Indicates an operation has been attempted on a built-in (special) SAM account which is incompatible with built-in accounts.
        /// For example, built-in accounts cannot be deleted.
        /// </summary>
        STATUS_SPECIAL_ACCOUNT = 0xC0000124,

        /// <summary>
        /// MessageId: STATUS_SPECIAL_GROUP
        /// MessageText:
        /// The operation requested may not be performed on the specified group because it is a built-in special group.
        /// </summary>
        STATUS_SPECIAL_GROUP = 0xC0000125,

        /// <summary>
        /// MessageId: STATUS_SPECIAL_USER
        /// MessageText:
        /// The operation requested may not be performed on the specified user because it is a built-in special user.
        /// </summary>
        STATUS_SPECIAL_USER = 0xC0000126,

        /// <summary>
        /// MessageId: STATUS_MEMBERS_PRIMARY_GROUP
        /// MessageText:
        /// Indicates a member cannot be removed from a group because the group is currently the member's primary group.
        /// </summary>
        STATUS_MEMBERS_PRIMARY_GROUP = 0xC0000127,

        /// <summary>
        /// MessageId: STATUS_FILE_CLOSED
        /// MessageText:
        /// An I/O request other than close and several other special case operations was attempted using a file object that had already been closed.
        /// </summary>
        STATUS_FILE_CLOSED = 0xC0000128,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_THREADS
        /// MessageText:
        /// Indicates a process has too many threads to perform the requested action. For example, assignment of a primary token may only be performed when a process has zero or one threads.
        /// </summary>
        STATUS_TOO_MANY_THREADS = 0xC0000129,

        /// <summary>
        /// MessageId: STATUS_THREAD_NOT_IN_PROCESS
        /// MessageText:
        /// An attempt was made to operate on a thread within a specific process, but the thread specified is not in the process specified.
        /// </summary>
        STATUS_THREAD_NOT_IN_PROCESS = 0xC000012A,

        /// <summary>
        /// MessageId: STATUS_TOKEN_ALREADY_IN_USE
        /// MessageText:
        /// An attempt was made to establish a token for use as a primary token but the token is already in use. A token can only be the primary token of one process at a time.
        /// </summary>
        STATUS_TOKEN_ALREADY_IN_USE = 0xC000012B,

        /// <summary>
        /// MessageId: STATUS_PAGEFILE_QUOTA_EXCEEDED
        /// MessageText:
        /// Page file quota was exceeded.
        /// </summary>
        STATUS_PAGEFILE_QUOTA_EXCEEDED = 0xC000012C,

        /// <summary>
        /// MessageId: STATUS_COMMITMENT_LIMIT
        /// MessageText:
        /// {Out of Virtual Memory}
        /// Your system is low on virtual memory. To ensure that Windows runs properly, increase the size of your virtual memory paging file. For more information, see Help.
        /// </summary>
        STATUS_COMMITMENT_LIMIT = 0xC000012D,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_LE_FORMAT
        /// MessageText:
        /// The specified image file did not have the correct format, it appears to be LE format.
        /// </summary>
        STATUS_INVALID_IMAGE_LE_FORMAT = 0xC000012E,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_NOT_MZ
        /// MessageText:
        /// The specified image file did not have the correct format, it did not have an initial MZ.
        /// </summary>
        STATUS_INVALID_IMAGE_NOT_MZ = 0xC000012F,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_PROTECT
        /// MessageText:
        /// The specified image file did not have the correct format, it did not have a proper e_lfarlc in the MZ header.
        /// </summary>
        STATUS_INVALID_IMAGE_PROTECT = 0xC0000130,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_WIN_16
        /// MessageText:
        /// The specified image file did not have the correct format, it appears to be a 16-bit Windows image.
        /// </summary>
        STATUS_INVALID_IMAGE_WIN_16 = 0xC0000131,

        /// <summary>
        /// MessageId: STATUS_LOGON_SERVER_CONFLICT
        /// MessageText:
        /// The Netlogon service cannot start because another Netlogon service running in the domain conflicts with the specified role.
        /// </summary>
        STATUS_LOGON_SERVER_CONFLICT = 0xC0000132,

        /// <summary>
        /// MessageId: STATUS_TIME_DIFFERENCE_AT_DC
        /// MessageText:
        /// The time at the Primary Domain Controller is different than the time at the Backup Domain Controller or member server by too large an amount.
        /// </summary>
        STATUS_TIME_DIFFERENCE_AT_DC = 0xC0000133,

        /// <summary>
        /// MessageId: STATUS_SYNCHRONIZATION_REQUIRED
        /// MessageText:
        /// The SAM database on a Windows Server is significantly out of synchronization with the copy on the Domain Controller. A complete synchronization is required.
        /// </summary>
        STATUS_SYNCHRONIZATION_REQUIRED = 0xC0000134,

        /// <summary>
        /// MessageId: STATUS_DLL_NOT_FOUND
        /// MessageText:
        /// {Unable To Locate Component}
        /// This application has failed to start because %hs was not found. Re-installing the application may fix this problem.
        /// </summary>
        STATUS_DLL_NOT_FOUND = 0xC0000135,

        /// <summary>
        /// MessageId: STATUS_OPEN_FAILED
        /// MessageText:
        /// The NtCreateFile API failed. This error should never be returned to an application, it is a place holder for the Windows Lan Manager Redirector to use in its internal error mapping routines.
        /// </summary>
        STATUS_OPEN_FAILED = 0xC0000136,

        /// <summary>
        /// MessageId: STATUS_IO_PRIVILEGE_FAILED
        /// MessageText:
        /// {Privilege Failed}
        /// The I/O permissions for the process could not be changed.
        /// </summary>
        STATUS_IO_PRIVILEGE_FAILED = 0xC0000137,

        /// <summary>
        /// MessageId: STATUS_ORDINAL_NOT_FOUND
        /// MessageText:
        /// {Ordinal Not Found}
        /// The ordinal %ld could not be located in the dynamic link library %hs.
        /// </summary>
        STATUS_ORDINAL_NOT_FOUND = 0xC0000138,

        /// <summary>
        /// MessageId: STATUS_ENTRYPOINT_NOT_FOUND
        /// MessageText:
        /// {Entry Point Not Found}
        /// The procedure entry point %hs could not be located in the dynamic link library %hs.
        /// </summary>
        STATUS_ENTRYPOINT_NOT_FOUND = 0xC0000139,

        /// <summary>
        /// MessageId: STATUS_CONTROL_C_EXIT
        /// MessageText:
        /// {Application Exit by CTRL+C}
        /// The application terminated as a result of a CTRL+C.
        /// </summary>
        STATUS_CONTROL_C_EXIT = 0xC000013A,

        /// <summary>
        /// MessageId: STATUS_LOCAL_DISCONNECT
        /// MessageText:
        /// {Virtual Circuit Closed}
        /// The network transport on your computer has closed a network connection. There may or may not be I/O requests outstanding.
        /// </summary>
        STATUS_LOCAL_DISCONNECT = 0xC000013B,

        /// <summary>
        /// MessageId: STATUS_REMOTE_DISCONNECT
        /// MessageText:
        /// {Virtual Circuit Closed}
        /// The network transport on a remote computer has closed a network connection. There may or may not be I/O requests outstanding.
        /// </summary>
        STATUS_REMOTE_DISCONNECT = 0xC000013C,

        /// <summary>
        /// MessageId: STATUS_REMOTE_RESOURCES
        /// MessageText:
        /// {Insufficient Resources on Remote Computer}
        /// The remote computer has insufficient resources to complete the network request. For instance, there may not be enough memory available on the remote computer to carry out the request at this time.
        /// </summary>
        STATUS_REMOTE_RESOURCES = 0xC000013D,

        /// <summary>
        /// MessageId: STATUS_LINK_FAILED
        /// MessageText:
        /// {Virtual Circuit Closed}
        /// An existing connection (virtual circuit) has been broken at the remote computer. There is probably something wrong with the network software protocol or the network hardware on the remote computer.
        /// </summary>
        STATUS_LINK_FAILED = 0xC000013E,

        /// <summary>
        /// MessageId: STATUS_LINK_TIMEOUT
        /// MessageText:
        /// {Virtual Circuit Closed}
        /// The network transport on your computer has closed a network connection because it had to wait too long for a response from the remote computer.
        /// </summary>
        STATUS_LINK_TIMEOUT = 0xC000013F,

        /// <summary>
        /// MessageId: STATUS_INVALID_CONNECTION
        /// MessageText:
        /// The connection handle given to the transport was invalid.
        /// </summary>
        STATUS_INVALID_CONNECTION = 0xC0000140,

        /// <summary>
        /// MessageId: STATUS_INVALID_ADDRESS
        /// MessageText:
        /// The address handle given to the transport was invalid.
        /// </summary>
        STATUS_INVALID_ADDRESS = 0xC0000141,

        /// <summary>
        /// MessageId: STATUS_DLL_INIT_FAILED
        /// MessageText:
        /// {DLL Initialization Failed}
        /// Initialization of the dynamic link library %hs failed. The process is terminating abnormally.
        /// </summary>
        STATUS_DLL_INIT_FAILED = 0xC0000142,

        /// <summary>
        /// MessageId: STATUS_MISSING_SYSTEMFILE
        /// MessageText:
        /// {Missing System File}
        /// The required system file %hs is bad or missing.
        /// </summary>
        STATUS_MISSING_SYSTEMFILE = 0xC0000143,

        /// <summary>
        /// MessageId: STATUS_UNHANDLED_EXCEPTION
        /// MessageText:
        /// {Application Error}
        /// The exception %s (0x%08lx) occurred in the application at location 0x%08lx.
        /// </summary>
        STATUS_UNHANDLED_EXCEPTION = 0xC0000144,

        /// <summary>
        /// MessageId: STATUS_APP_INIT_FAILURE
        /// MessageText:
        /// {Application Error}
        /// The application failed to initialize properly (0x%lx). Click OK to terminate the application.
        /// </summary>
        STATUS_APP_INIT_FAILURE = 0xC0000145,

        /// <summary>
        /// MessageId: STATUS_PAGEFILE_CREATE_FAILED
        /// MessageText:
        /// {Unable to Create Paging File}
        /// The creation of the paging file %hs failed (%lx). The requested size was %ld.
        /// </summary>
        STATUS_PAGEFILE_CREATE_FAILED = 0xC0000146,

        /// <summary>
        /// MessageId: STATUS_NO_PAGEFILE
        /// MessageText:
        /// {No Paging File Specified}
        /// No paging file was specified in the system configuration.
        /// </summary>
        STATUS_NO_PAGEFILE = 0xC0000147,

        /// <summary>
        /// MessageId: STATUS_INVALID_LEVEL
        /// MessageText:
        /// {Incorrect System Call Level}
        /// An invalid level was passed into the specified system call.
        /// </summary>
        STATUS_INVALID_LEVEL = 0xC0000148,

        /// <summary>
        /// MessageId: STATUS_WRONG_PASSWORD_CORE
        /// MessageText:
        /// {Incorrect Password to LAN Manager Server}
        /// You specified an incorrect password to a LAN Manager 2.x or MS-NET server.
        /// </summary>
        STATUS_WRONG_PASSWORD_CORE = 0xC0000149,

        /// <summary>
        /// MessageId: STATUS_ILLEGAL_FLOAT_CONTEXT
        /// MessageText:
        /// {EXCEPTION}
        /// A real-mode application issued a floating-point instruction and floating-point hardware is not present.
        /// </summary>
        STATUS_ILLEGAL_FLOAT_CONTEXT = 0xC000014A,

        /// <summary>
        /// MessageId: STATUS_PIPE_BROKEN
        /// MessageText:
        /// The pipe operation has failed because the other end of the pipe has been closed.
        /// </summary>
        STATUS_PIPE_BROKEN = 0xC000014B,

        /// <summary>
        /// MessageId: STATUS_REGISTRY_CORRUPT
        /// MessageText:
        /// {The Registry Is Corrupt}
        /// The structure of one of the files that contains Registry data is corrupt, or the image of the file in memory is corrupt, or the file could not be recovered because the alternate copy or log was absent or corrupt.
        /// </summary>
        STATUS_REGISTRY_CORRUPT = 0xC000014C,

        /// <summary>
        /// MessageId: STATUS_REGISTRY_IO_FAILED
        /// MessageText:
        /// An I/O operation initiated by the Registry failed unrecoverably.
        /// The Registry could not read in, or write out, or flush, one of the files that contain the system's image of the Registry.
        /// </summary>
        STATUS_REGISTRY_IO_FAILED = 0xC000014D,

        /// <summary>
        /// MessageId: STATUS_NO_EVENT_PAIR
        /// MessageText:
        /// An event pair synchronization operation was performed using the thread specific client/server event pair object, but no event pair object was associated with the thread.
        /// </summary>
        STATUS_NO_EVENT_PAIR = 0xC000014E,

        /// <summary>
        /// MessageId: STATUS_UNRECOGNIZED_VOLUME
        /// MessageText:
        /// The volume does not contain a recognized file system.
        /// Please make sure that all required file system drivers are loaded and that the volume is not corrupt.
        /// </summary>
        STATUS_UNRECOGNIZED_VOLUME = 0xC000014F,

        /// <summary>
        /// MessageId: STATUS_SERIAL_NO_DEVICE_INITED
        /// MessageText:
        /// No serial device was successfully initialized. The serial driver will unload.
        /// </summary>
        STATUS_SERIAL_NO_DEVICE_INITED = 0xC0000150,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_ALIAS
        /// MessageText:
        /// The specified local group does not exist.
        /// </summary>
        STATUS_NO_SUCH_ALIAS = 0xC0000151,

        /// <summary>
        /// MessageId: STATUS_MEMBER_NOT_IN_ALIAS
        /// MessageText:
        /// The specified account name is not a member of the group.
        /// </summary>
        STATUS_MEMBER_NOT_IN_ALIAS = 0xC0000152,

        /// <summary>
        /// MessageId: STATUS_MEMBER_IN_ALIAS
        /// MessageText:
        /// The specified account name is already a member of the group.
        /// </summary>
        STATUS_MEMBER_IN_ALIAS = 0xC0000153,

        /// <summary>
        /// MessageId: STATUS_ALIAS_EXISTS
        /// MessageText:
        /// The specified local group already exists.
        /// </summary>
        STATUS_ALIAS_EXISTS = 0xC0000154,

        /// <summary>
        /// MessageId: STATUS_LOGON_NOT_GRANTED
        /// MessageText:
        /// A requested type of logon (e.g., Interactive, Network, Service) is not granted by the target system's local security policy.
        /// Please ask the system administrator to grant the necessary form of logon.
        /// </summary>
        STATUS_LOGON_NOT_GRANTED = 0xC0000155,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_SECRETS
        /// MessageText:
        /// The maximum number of secrets that may be stored in a single system has been exceeded. The length and number of secrets is limited to satisfy United States State Department export restrictions.
        /// </summary>
        STATUS_TOO_MANY_SECRETS = 0xC0000156,

        /// <summary>
        /// MessageId: STATUS_SECRET_TOO_LONG
        /// MessageText:
        /// The length of a secret exceeds the maximum length allowed. The length and number of secrets is limited to satisfy United States State Department export restrictions.
        /// </summary>
        STATUS_SECRET_TOO_LONG = 0xC0000157,

        /// <summary>
        /// MessageId: STATUS_INTERNAL_DB_ERROR
        /// MessageText:
        /// The Local Security Authority (LSA) database contains an internal inconsistency.
        /// </summary>
        STATUS_INTERNAL_DB_ERROR = 0xC0000158,

        /// <summary>
        /// MessageId: STATUS_FULLSCREEN_MODE
        /// MessageText:
        /// The requested operation cannot be performed in fullscreen mode.
        /// </summary>
        STATUS_FULLSCREEN_MODE = 0xC0000159,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_CONTEXT_IDS
        /// MessageText:
        /// During a logon attempt, the user's security context accumulated too many security IDs. This is a very unusual situation.
        /// Remove the user from some global or local groups to reduce the number of security ids to incorporate into the security context.
        /// </summary>
        STATUS_TOO_MANY_CONTEXT_IDS = 0xC000015A,

        /// <summary>
        /// MessageId: STATUS_LOGON_TYPE_NOT_GRANTED
        /// MessageText:
        /// A user has requested a type of logon (e.g., interactive or network) that has not been granted. An administrator has control over who may logon interactively and through the network.
        /// </summary>
        STATUS_LOGON_TYPE_NOT_GRANTED = 0xC000015B,

        /// <summary>
        /// MessageId: STATUS_NOT_REGISTRY_FILE
        /// MessageText:
        /// The system has attempted to load or restore a file into the registry, and the specified file is not in the format of a registry file.
        /// </summary>
        STATUS_NOT_REGISTRY_FILE = 0xC000015C,

        /// <summary>
        /// MessageId: STATUS_NT_CROSS_ENCRYPTION_REQUIRED
        /// MessageText:
        /// An attempt was made to change a user password in the security account manager without providing the necessary Windows cross-encrypted password.
        /// </summary>
        STATUS_NT_CROSS_ENCRYPTION_REQUIRED = 0xC000015D,

        /// <summary>
        /// MessageId: STATUS_DOMAIN_CTRLR_CONFIG_ERROR
        /// MessageText:
        /// A Windows Server has an incorrect configuration.
        /// </summary>
        STATUS_DOMAIN_CTRLR_CONFIG_ERROR = 0xC000015E,

        /// <summary>
        /// MessageId: STATUS_FT_MISSING_MEMBER
        /// MessageText:
        /// An attempt was made to explicitly access the secondary copy of information via a device control to the Fault Tolerance driver and the secondary copy is not present in the system.
        /// </summary>
        STATUS_FT_MISSING_MEMBER = 0xC000015F,

        /// <summary>
        /// MessageId: STATUS_ILL_FORMED_SERVICE_ENTRY
        /// MessageText:
        /// A configuration registry node representing a driver service entry was ill-formed and did not contain required value entries.
        /// </summary>
        STATUS_ILL_FORMED_SERVICE_ENTRY = 0xC0000160,

        /// <summary>
        /// MessageId: STATUS_ILLEGAL_CHARACTER
        /// MessageText:
        /// An illegal character was encountered. For a multi-byte character set this includes a lead byte without a succeeding trail byte. For the Unicode character set this includes the characters 0xFFFF and 0xFFFE.
        /// </summary>
        STATUS_ILLEGAL_CHARACTER = 0xC0000161,

        /// <summary>
        /// MessageId: STATUS_UNMAPPABLE_CHARACTER
        /// MessageText:
        /// No mapping for the Unicode character exists in the target multi-byte code page.
        /// </summary>
        STATUS_UNMAPPABLE_CHARACTER = 0xC0000162,

        /// <summary>
        /// MessageId: STATUS_UNDEFINED_CHARACTER
        /// MessageText:
        /// The Unicode character is not defined in the Unicode character set installed on the system.
        /// </summary>
        STATUS_UNDEFINED_CHARACTER = 0xC0000163,

        /// <summary>
        /// MessageId: STATUS_FLOPPY_VOLUME
        /// MessageText:
        /// The paging file cannot be created on a floppy diskette.
        /// </summary>
        STATUS_FLOPPY_VOLUME = 0xC0000164,

        /// <summary>
        /// MessageId: STATUS_FLOPPY_ID_MARK_NOT_FOUND
        /// MessageText:
        /// {Floppy Disk Error}
        /// While accessing a floppy disk, an ID address mark was not found.
        /// </summary>
        STATUS_FLOPPY_ID_MARK_NOT_FOUND = 0xC0000165,

        /// <summary>
        /// MessageId: STATUS_FLOPPY_WRONG_CYLINDER
        /// MessageText:
        /// {Floppy Disk Error}
        /// While accessing a floppy disk, the track address from the sector ID field was found to be different than the track address maintained by the controller.
        /// </summary>
        STATUS_FLOPPY_WRONG_CYLINDER = 0xC0000166,

        /// <summary>
        /// MessageId: STATUS_FLOPPY_UNKNOWN_ERROR
        /// MessageText:
        /// {Floppy Disk Error}
        /// The floppy disk controller reported an error that is not recognized by the floppy disk driver.
        /// </summary>
        STATUS_FLOPPY_UNKNOWN_ERROR = 0xC0000167,

        /// <summary>
        /// MessageId: STATUS_FLOPPY_BAD_REGISTERS
        /// MessageText:
        /// {Floppy Disk Error}
        /// While accessing a floppy-disk, the controller returned inconsistent results via its registers.
        /// </summary>
        STATUS_FLOPPY_BAD_REGISTERS = 0xC0000168,

        /// <summary>
        /// MessageId: STATUS_DISK_RECALIBRATE_FAILED
        /// MessageText:
        /// {Hard Disk Error}
        /// While accessing the hard disk, a recalibrate operation failed, even after retries.
        /// </summary>
        STATUS_DISK_RECALIBRATE_FAILED = 0xC0000169,

        /// <summary>
        /// MessageId: STATUS_DISK_OPERATION_FAILED
        /// MessageText:
        /// {Hard Disk Error}
        /// While accessing the hard disk, a disk operation failed even after retries.
        /// </summary>
        STATUS_DISK_OPERATION_FAILED = 0xC000016A,

        /// <summary>
        /// MessageId: STATUS_DISK_RESET_FAILED
        /// MessageText:
        /// {Hard Disk Error}
        /// While accessing the hard disk, a disk controller reset was needed, but even that failed.
        /// </summary>
        STATUS_DISK_RESET_FAILED = 0xC000016B,

        /// <summary>
        /// MessageId: STATUS_SHARED_IRQ_BUSY
        /// MessageText:
        /// An attempt was made to open a device that was sharing an IRQ with other devices.
        /// At least one other device that uses that IRQ was already opened.
        /// Two concurrent opens of devices that share an IRQ and only work via interrupts is not supported for the particular bus type that the devices use.
        /// </summary>
        STATUS_SHARED_IRQ_BUSY = 0xC000016C,

        /// <summary>
        /// MessageId: STATUS_FT_ORPHANING
        /// MessageText:
        /// {FT Orphaning}
        /// A disk that is part of a fault-tolerant volume can no longer be accessed.
        /// </summary>
        STATUS_FT_ORPHANING = 0xC000016D,

        /// <summary>
        /// MessageId: STATUS_BIOS_FAILED_TO_CONNECT_INTERRUPT
        /// MessageText:
        /// The system bios failed to connect a system interrupt to the device or bus for
        /// which the device is connected.
        /// </summary>
        STATUS_BIOS_FAILED_TO_CONNECT_INTERRUPT = 0xC000016E,

        /// <summary>
        /// MessageId: STATUS_PARTITION_FAILURE
        /// MessageText:
        /// Tape could not be partitioned.
        /// </summary>
        STATUS_PARTITION_FAILURE = 0xC0000172,

        /// <summary>
        /// MessageId: STATUS_INVALID_BLOCK_LENGTH
        /// MessageText:
        /// When accessing a new tape of a multivolume partition, the current blocksize is incorrect.
        /// </summary>
        STATUS_INVALID_BLOCK_LENGTH = 0xC0000173,

        /// <summary>
        /// MessageId: STATUS_DEVICE_NOT_PARTITIONED
        /// MessageText:
        /// Tape partition information could not be found when loading a tape.
        /// </summary>
        STATUS_DEVICE_NOT_PARTITIONED = 0xC0000174,

        /// <summary>
        /// MessageId: STATUS_UNABLE_TO_LOCK_MEDIA
        /// MessageText:
        /// Attempt to lock the eject media mechanism fails.
        /// </summary>
        STATUS_UNABLE_TO_LOCK_MEDIA = 0xC0000175,

        /// <summary>
        /// MessageId: STATUS_UNABLE_TO_UNLOAD_MEDIA
        /// MessageText:
        /// Unload media fails.
        /// </summary>
        STATUS_UNABLE_TO_UNLOAD_MEDIA = 0xC0000176,

        /// <summary>
        /// MessageId: STATUS_EOM_OVERFLOW
        /// MessageText:
        /// Physical end of tape was detected.
        /// </summary>
        STATUS_EOM_OVERFLOW = 0xC0000177,

        /// <summary>
        /// MessageId: STATUS_NO_MEDIA
        /// MessageText:
        /// {No Media}
        /// There is no media in the drive.
        /// Please insert media into drive %hs.
        /// </summary>
        STATUS_NO_MEDIA = 0xC0000178,

        /// <summary>
        /// MessageId: STATUS_NO_SUCH_MEMBER
        /// MessageText:
        /// A member could not be added to or removed from the local group because the member does not exist.
        /// </summary>
        STATUS_NO_SUCH_MEMBER = 0xC000017A,

        /// <summary>
        /// MessageId: STATUS_INVALID_MEMBER
        /// MessageText:
        /// A new member could not be added to a local group because the member has the wrong account type.
        /// </summary>
        STATUS_INVALID_MEMBER = 0xC000017B,

        /// <summary>
        /// MessageId: STATUS_KEY_DELETED
        /// MessageText:
        /// Illegal operation attempted on a registry key which has been marked for deletion.
        /// </summary>
        STATUS_KEY_DELETED = 0xC000017C,

        /// <summary>
        /// MessageId: STATUS_NO_LOG_SPACE
        /// MessageText:
        /// System could not allocate required space in a registry log.
        /// </summary>
        STATUS_NO_LOG_SPACE = 0xC000017D,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_SIDS
        /// MessageText:
        /// Too many Sids have been specified.
        /// </summary>
        STATUS_TOO_MANY_SIDS = 0xC000017E,

        /// <summary>
        /// MessageId: STATUS_LM_CROSS_ENCRYPTION_REQUIRED
        /// MessageText:
        /// An attempt was made to change a user password in the security account manager without providing the necessary LM cross-encrypted password.
        /// </summary>
        STATUS_LM_CROSS_ENCRYPTION_REQUIRED = 0xC000017F,

        /// <summary>
        /// MessageId: STATUS_KEY_HAS_CHILDREN
        /// MessageText:
        /// An attempt was made to create a symbolic link in a registry key that already has subkeys or values.
        /// </summary>
        STATUS_KEY_HAS_CHILDREN = 0xC0000180,

        /// <summary>
        /// MessageId: STATUS_CHILD_MUST_BE_VOLATILE
        /// MessageText:
        /// An attempt was made to create a Stable subkey under a Volatile parent key.
        /// </summary>
        STATUS_CHILD_MUST_BE_VOLATILE = 0xC0000181,

        /// <summary>
        /// MessageId: STATUS_DEVICE_CONFIGURATION_ERROR
        /// MessageText:
        /// The I/O device is configured incorrectly or the configuration parameters to the driver are incorrect.
        /// </summary>
        STATUS_DEVICE_CONFIGURATION_ERROR = 0xC0000182,

        /// <summary>
        /// MessageId: STATUS_DRIVER_INTERNAL_ERROR
        /// MessageText:
        /// An error was detected between two drivers or within an I/O driver.
        /// </summary>
        STATUS_DRIVER_INTERNAL_ERROR = 0xC0000183,

        /// <summary>
        /// MessageId: STATUS_INVALID_DEVICE_STATE
        /// MessageText:
        /// The device is not in a valid state to perform this request.
        /// </summary>
        STATUS_INVALID_DEVICE_STATE = 0xC0000184,

        /// <summary>
        /// MessageId: STATUS_IO_DEVICE_ERROR
        /// MessageText:
        /// The I/O device reported an I/O error.
        /// </summary>
        STATUS_IO_DEVICE_ERROR = 0xC0000185,

        /// <summary>
        /// MessageId: STATUS_DEVICE_PROTOCOL_ERROR
        /// MessageText:
        /// A protocol error was detected between the driver and the device.
        /// </summary>
        STATUS_DEVICE_PROTOCOL_ERROR = 0xC0000186,

        /// <summary>
        /// MessageId: STATUS_BACKUP_CONTROLLER
        /// MessageText:
        /// This operation is only allowed for the Primary Domain Controller of the domain.
        /// </summary>
        STATUS_BACKUP_CONTROLLER = 0xC0000187,

        /// <summary>
        /// MessageId: STATUS_LOG_FILE_FULL
        /// MessageText:
        /// Log file space is insufficient to support this operation.
        /// </summary>
        STATUS_LOG_FILE_FULL = 0xC0000188,

        /// <summary>
        /// MessageId: STATUS_TOO_LATE
        /// MessageText:
        /// A write operation was attempted to a volume after it was dismounted.
        /// </summary>
        STATUS_TOO_LATE = 0xC0000189,

        /// <summary>
        /// MessageId: STATUS_NO_TRUST_LSA_SECRET
        /// MessageText:
        /// The workstation does not have a trust secret for the primary domain in the local LSA database.
        /// </summary>
        STATUS_NO_TRUST_LSA_SECRET = 0xC000018A,

        /// <summary>
        /// MessageId: STATUS_NO_TRUST_SAM_ACCOUNT
        /// MessageText:
        /// The SAM database on the Windows Server does not have a computer account for this workstation trust relationship.
        /// </summary>
        STATUS_NO_TRUST_SAM_ACCOUNT = 0xC000018B,

        /// <summary>
        /// MessageId: STATUS_TRUSTED_DOMAIN_FAILURE
        /// MessageText:
        /// The logon request failed because the trust relationship between the primary domain and the trusted domain failed.
        /// </summary>
        STATUS_TRUSTED_DOMAIN_FAILURE = 0xC000018C,

        /// <summary>
        /// MessageId: STATUS_TRUSTED_RELATIONSHIP_FAILURE
        /// MessageText:
        /// The logon request failed because the trust relationship between this workstation and the primary domain failed.
        /// </summary>
        STATUS_TRUSTED_RELATIONSHIP_FAILURE = 0xC000018D,

        /// <summary>
        /// MessageId: STATUS_EVENTLOG_FILE_CORRUPT
        /// MessageText:
        /// The Eventlog log file is corrupt.
        /// </summary>
        STATUS_EVENTLOG_FILE_CORRUPT = 0xC000018E,

        /// <summary>
        /// MessageId: STATUS_EVENTLOG_CANT_START
        /// MessageText:
        /// No Eventlog log file could be opened. The Eventlog service did not start.
        /// </summary>
        STATUS_EVENTLOG_CANT_START = 0xC000018F,

        /// <summary>
        /// MessageId: STATUS_TRUST_FAILURE
        /// MessageText:
        /// The network logon failed. This may be because the validation authority can't be reached.
        /// </summary>
        STATUS_TRUST_FAILURE = 0xC0000190,

        /// <summary>
        /// MessageId: STATUS_MUTANT_LIMIT_EXCEEDED
        /// MessageText:
        /// An attempt was made to acquire a mutant such that its maximum count would have been exceeded.
        /// </summary>
        STATUS_MUTANT_LIMIT_EXCEEDED = 0xC0000191,

        /// <summary>
        /// MessageId: STATUS_NETLOGON_NOT_STARTED
        /// MessageText:
        /// An attempt was made to logon, but the netlogon service was not started.
        /// </summary>
        STATUS_NETLOGON_NOT_STARTED = 0xC0000192,

        /// <summary>
        /// MessageId: STATUS_ACCOUNT_EXPIRED
        /// MessageText:
        /// The user's account has expired.
        /// </summary>
        STATUS_ACCOUNT_EXPIRED = 0xC0000193,

        /// <summary>
        /// MessageId: STATUS_POSSIBLE_DEADLOCK
        /// MessageText:
        /// {EXCEPTION}
        /// Possible deadlock condition.
        /// </summary>
        STATUS_POSSIBLE_DEADLOCK = 0xC0000194,

        /// <summary>
        /// MessageId: STATUS_NETWORK_CREDENTIAL_CONFLICT
        /// MessageText:
        /// Multiple connections to a server or shared resource by the same user, using more than one user name, are not allowed. Disconnect all previous connections to the server or shared resource and try again.
        /// </summary>
        STATUS_NETWORK_CREDENTIAL_CONFLICT = 0xC0000195,

        /// <summary>
        /// MessageId: STATUS_REMOTE_SESSION_LIMIT
        /// MessageText:
        /// An attempt was made to establish a session to a network server, but there are already too many sessions established to that server.
        /// </summary>
        STATUS_REMOTE_SESSION_LIMIT = 0xC0000196,

        /// <summary>
        /// MessageId: STATUS_EVENTLOG_FILE_CHANGED
        /// MessageText:
        /// The log file has changed between reads.
        /// </summary>
        STATUS_EVENTLOG_FILE_CHANGED = 0xC0000197,

        /// <summary>
        /// MessageId: STATUS_NOLOGON_INTERDOMAIN_TRUST_ACCOUNT
        /// MessageText:
        /// The account used is an Interdomain Trust account. Use your global user account or local user account to access this server.
        /// </summary>
        STATUS_NOLOGON_INTERDOMAIN_TRUST_ACCOUNT = 0xC0000198,

        /// <summary>
        /// MessageId: STATUS_NOLOGON_WORKSTATION_TRUST_ACCOUNT
        /// MessageText:
        /// The account used is a Computer Account. Use your global user account or local user account to access this server.
        /// </summary>
        STATUS_NOLOGON_WORKSTATION_TRUST_ACCOUNT = 0xC0000199,

        /// <summary>
        /// MessageId: STATUS_NOLOGON_SERVER_TRUST_ACCOUNT
        /// MessageText:
        /// The account used is an Server Trust account. Use your global user account or local user account to access this server.
        /// </summary>
        STATUS_NOLOGON_SERVER_TRUST_ACCOUNT = 0xC000019A,

        /// <summary>
        /// MessageId: STATUS_DOMAIN_TRUST_INCONSISTENT
        /// MessageText:
        /// The name or SID of the domain specified is inconsistent with the trust information for that domain.
        /// </summary>
        STATUS_DOMAIN_TRUST_INCONSISTENT = 0xC000019B,

        /// <summary>
        /// MessageId: STATUS_FS_DRIVER_REQUIRED
        /// MessageText:
        /// A volume has been accessed for which a file system driver is required that has not yet been loaded.
        /// </summary>
        STATUS_FS_DRIVER_REQUIRED = 0xC000019C,

        /// <summary>
        /// MessageId: STATUS_IMAGE_ALREADY_LOADED_AS_DLL
        /// MessageText:
        /// Indicates that the specified image is already loaded as a DLL.
        /// </summary>
        STATUS_IMAGE_ALREADY_LOADED_AS_DLL = 0xC000019D,

        /// <summary>
        /// MessageId: STATUS_NETWORK_OPEN_RESTRICTION
        /// MessageText:
        /// A remote open failed because the network open restrictions were not satisfied.
        /// </summary>
        STATUS_NETWORK_OPEN_RESTRICTION = 0xC0000201,

        /// <summary>
        /// MessageId: STATUS_NO_USER_SESSION_KEY
        /// MessageText:
        /// There is no user session key for the specified logon session.
        /// </summary>
        STATUS_NO_USER_SESSION_KEY = 0xC0000202,

        /// <summary>
        /// MessageId: STATUS_USER_SESSION_DELETED
        /// MessageText:
        /// The remote user session has been deleted.
        /// </summary>
        STATUS_USER_SESSION_DELETED = 0xC0000203,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_LANG_NOT_FOUND
        /// MessageText:
        /// Indicates the specified resource language ID cannot be found in the
        /// image file.
        /// </summary>
        STATUS_RESOURCE_LANG_NOT_FOUND = 0xC0000204,

        /// <summary>
        /// MessageId: STATUS_INSUFF_SERVER_RESOURCES
        /// MessageText:
        /// Insufficient server resources exist to complete the request.
        /// </summary>
        STATUS_INSUFF_SERVER_RESOURCES = 0xC0000205,

        /// <summary>
        /// MessageId: STATUS_INVALID_BUFFER_SIZE
        /// MessageText:
        /// The size of the buffer is invalid for the specified operation.
        /// </summary>
        STATUS_INVALID_BUFFER_SIZE = 0xC0000206,

        /// <summary>
        /// MessageId: STATUS_INVALID_ADDRESS_COMPONENT
        /// MessageText:
        /// The transport rejected the network address specified as invalid.
        /// </summary>
        STATUS_INVALID_ADDRESS_COMPONENT = 0xC0000207,

        /// <summary>
        /// MessageId: STATUS_INVALID_ADDRESS_WILDCARD
        /// MessageText:
        /// The transport rejected the network address specified due to an invalid use of a wildcard.
        /// </summary>
        STATUS_INVALID_ADDRESS_WILDCARD = 0xC0000208,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_ADDRESSES
        /// MessageText:
        /// The transport address could not be opened because all the available addresses are in use.
        /// </summary>
        STATUS_TOO_MANY_ADDRESSES = 0xC0000209,

        /// <summary>
        /// MessageId: STATUS_ADDRESS_ALREADY_EXISTS
        /// MessageText:
        /// The transport address could not be opened because it already exists.
        /// </summary>
        STATUS_ADDRESS_ALREADY_EXISTS = 0xC000020A,

        /// <summary>
        /// MessageId: STATUS_ADDRESS_CLOSED
        /// MessageText:
        /// The transport address is now closed.
        /// </summary>
        STATUS_ADDRESS_CLOSED = 0xC000020B,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_DISCONNECTED
        /// MessageText:
        /// The transport connection is now disconnected.
        /// </summary>
        STATUS_CONNECTION_DISCONNECTED = 0xC000020C,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_RESET
        /// MessageText:
        /// The transport connection has been reset.
        /// </summary>
        STATUS_CONNECTION_RESET = 0xC000020D,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_NODES
        /// MessageText:
        /// The transport cannot dynamically acquire any more nodes.
        /// </summary>
        STATUS_TOO_MANY_NODES = 0xC000020E,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_ABORTED
        /// MessageText:
        /// The transport aborted a pending transaction.
        /// </summary>
        STATUS_TRANSACTION_ABORTED = 0xC000020F,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_TIMED_OUT
        /// MessageText:
        /// The transport timed out a request waiting for a response.
        /// </summary>
        STATUS_TRANSACTION_TIMED_OUT = 0xC0000210,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_NO_RELEASE
        /// MessageText:
        /// The transport did not receive a release for a pending response.
        /// </summary>
        STATUS_TRANSACTION_NO_RELEASE = 0xC0000211,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_NO_MATCH
        /// MessageText:
        /// The transport did not find a transaction matching the specific
        /// token.
        /// </summary>
        STATUS_TRANSACTION_NO_MATCH = 0xC0000212,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_RESPONDED
        /// MessageText:
        /// The transport had previously responded to a transaction request.
        /// </summary>
        STATUS_TRANSACTION_RESPONDED = 0xC0000213,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_INVALID_ID
        /// MessageText:
        /// The transport does not recognized the transaction request identifier specified.
        /// </summary>
        STATUS_TRANSACTION_INVALID_ID = 0xC0000214,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_INVALID_TYPE
        /// MessageText:
        /// The transport does not recognize the transaction request type specified.
        /// </summary>
        STATUS_TRANSACTION_INVALID_TYPE = 0xC0000215,

        /// <summary>
        /// MessageId: STATUS_NOT_SERVER_SESSION
        /// MessageText:
        /// The transport can only process the specified request on the server side of a session.
        /// </summary>
        STATUS_NOT_SERVER_SESSION = 0xC0000216,

        /// <summary>
        /// MessageId: STATUS_NOT_CLIENT_SESSION
        /// MessageText:
        /// The transport can only process the specified request on the client side of a session.
        /// </summary>
        STATUS_NOT_CLIENT_SESSION = 0xC0000217,

        /// <summary>
        /// MessageId: STATUS_CANNOT_LOAD_REGISTRY_FILE
        /// MessageText:
        /// {Registry File Failure}
        /// The registry cannot load the hive (file):
        /// %hs
        /// or its log or alternate.
        /// It is corrupt, absent, or not writable.
        /// </summary>
        STATUS_CANNOT_LOAD_REGISTRY_FILE = 0xC0000218,

        /// <summary>
        /// MessageId: STATUS_DEBUG_ATTACH_FAILED
        /// MessageText:
        /// {Unexpected Failure in DebugActiveProcess}
        /// An unexpected failure occurred while processing a DebugActiveProcess API request. You may choose OK to terminate the process, or Cancel to ignore the error.
        /// </summary>
        STATUS_DEBUG_ATTACH_FAILED = 0xC0000219,

        /// <summary>
        /// MessageId: STATUS_SYSTEM_PROCESS_TERMINATED
        /// MessageText:
        /// {Fatal System Error}
        /// The %hs system process terminated unexpectedly with a status of 0x%08x (0x%08x 0x%08x).
        /// The system has been shut down.
        /// </summary>
        STATUS_SYSTEM_PROCESS_TERMINATED = 0xC000021A,

        /// <summary>
        /// MessageId: STATUS_DATA_NOT_ACCEPTED
        /// MessageText:
        /// {Data Not Accepted}
        /// The TDI client could not handle the data received during an indication.
        /// </summary>
        STATUS_DATA_NOT_ACCEPTED = 0xC000021B,

        /// <summary>
        /// MessageId: STATUS_NO_BROWSER_SERVERS_FOUND
        /// MessageText:
        /// {Unable to Retrieve Browser Server List}
        /// The list of servers for this workgroup is not currently available.
        /// </summary>
        STATUS_NO_BROWSER_SERVERS_FOUND = 0xC000021C,

        /// <summary>
        /// MessageId: STATUS_VDM_HARD_ERROR
        /// MessageText:
        /// NTVDM encountered a hard error.
        /// </summary>
        STATUS_VDM_HARD_ERROR = 0xC000021D,

        /// <summary>
        /// MessageId: STATUS_DRIVER_CANCEL_TIMEOUT
        /// MessageText:
        /// {Cancel Timeout}
        /// The driver %hs failed to complete a cancelled I/O request in the allotted time.
        /// </summary>
        STATUS_DRIVER_CANCEL_TIMEOUT = 0xC000021E,

        /// <summary>
        /// MessageId: STATUS_REPLY_MESSAGE_MISMATCH
        /// MessageText:
        /// {Reply Message Mismatch}
        /// An attempt was made to reply to an LPC message, but the thread specified by the client ID in the message was not waiting on that message.
        /// </summary>
        STATUS_REPLY_MESSAGE_MISMATCH = 0xC000021F,

        /// <summary>
        /// MessageId: STATUS_MAPPED_ALIGNMENT
        /// MessageText:
        /// {Mapped View Alignment Incorrect}
        /// An attempt was made to map a view of a file, but either the specified base address or the offset into the file were not aligned on the proper allocation granularity.
        /// </summary>
        STATUS_MAPPED_ALIGNMENT = 0xC0000220,

        /// <summary>
        /// MessageId: STATUS_IMAGE_CHECKSUM_MISMATCH
        /// MessageText:
        /// {Bad Image Checksum}
        /// The image %hs is possibly corrupt. The header checksum does not match the computed checksum.
        /// </summary>
        STATUS_IMAGE_CHECKSUM_MISMATCH = 0xC0000221,

        /// <summary>
        /// MessageId: STATUS_LOST_WRITEBEHIND_DATA
        /// MessageText:
        /// {Delayed Write Failed}
        /// Windows was unable to save all the data for the file %hs. The data has been lost.
        /// This error may be caused by a failure of your computer hardware or network connection. Please try to save this file elsewhere.
        /// </summary>
        STATUS_LOST_WRITEBEHIND_DATA = 0xC0000222,

        /// <summary>
        /// MessageId: STATUS_CLIENT_SERVER_PARAMETERS_INVALID
        /// MessageText:
        /// The parameter(s) passed to the server in the client/server shared memory window were invalid. Too much data may have been put in the shared memory window.
        /// </summary>
        STATUS_CLIENT_SERVER_PARAMETERS_INVALID = 0xC0000223,

        /// <summary>
        /// MessageId: STATUS_PASSWORD_MUST_CHANGE
        /// MessageText:
        /// The user's password must be changed before logging on the first time.
        /// </summary>
        STATUS_PASSWORD_MUST_CHANGE = 0xC0000224,

        /// <summary>
        /// MessageId: STATUS_NOT_FOUND
        /// MessageText:
        /// The object was not found.
        /// </summary>
        STATUS_NOT_FOUND = 0xC0000225,

        /// <summary>
        /// MessageId: STATUS_NOT_TINY_STREAM
        /// MessageText:
        /// The stream is not a tiny stream.
        /// </summary>
        STATUS_NOT_TINY_STREAM = 0xC0000226,

        /// <summary>
        /// MessageId: STATUS_RECOVERY_FAILURE
        /// MessageText:
        /// A transaction recover failed.
        /// </summary>
        STATUS_RECOVERY_FAILURE = 0xC0000227,

        /// <summary>
        /// MessageId: STATUS_STACK_OVERFLOW_READ
        /// MessageText:
        /// The request must be handled by the stack overflow code.
        /// </summary>
        STATUS_STACK_OVERFLOW_READ = 0xC0000228,

        /// <summary>
        /// MessageId: STATUS_FAIL_CHECK
        /// MessageText:
        /// A consistency check failed.
        /// </summary>
        STATUS_FAIL_CHECK = 0xC0000229,

        /// <summary>
        /// MessageId: STATUS_DUPLICATE_OBJECTID
        /// MessageText:
        /// The attempt to insert the ID in the index failed because the ID is already in the index.
        /// </summary>
        STATUS_DUPLICATE_OBJECTID = 0xC000022A,

        /// <summary>
        /// MessageId: STATUS_OBJECTID_EXISTS
        /// MessageText:
        /// The attempt to set the object's ID failed because the object already has an ID.
        /// </summary>
        STATUS_OBJECTID_EXISTS = 0xC000022B,

        /// <summary>
        /// MessageId: STATUS_CONVERT_TO_LARGE
        /// MessageText:
        /// Internal OFS status codes indicating how an allocation operation is handled. Either it is retried after the containing onode is moved or the extent stream is converted to a large stream.
        /// </summary>
        STATUS_CONVERT_TO_LARGE = 0xC000022C,

        /// <summary>
        /// MessageId: STATUS_RETRY
        /// MessageText:
        /// The request needs to be retried.
        /// </summary>
        STATUS_RETRY = 0xC000022D,

        /// <summary>
        /// MessageId: STATUS_FOUND_OUT_OF_SCOPE
        /// MessageText:
        /// The attempt to find the object found an object matching by ID on the volume but it is out of the scope of the handle used for the operation.
        /// </summary>
        STATUS_FOUND_OUT_OF_SCOPE = 0xC000022E,

        /// <summary>
        /// MessageId: STATUS_ALLOCATE_BUCKET
        /// MessageText:
        /// The bucket array must be grown. Retry transaction after doing so.
        /// </summary>
        STATUS_ALLOCATE_BUCKET = 0xC000022F,

        /// <summary>
        /// MessageId: STATUS_PROPSET_NOT_FOUND
        /// MessageText:
        /// The property set specified does not exist on the object.
        /// </summary>
        STATUS_PROPSET_NOT_FOUND = 0xC0000230,

        /// <summary>
        /// MessageId: STATUS_MARSHALL_OVERFLOW
        /// MessageText:
        /// The user/kernel marshalling buffer has overflowed.
        /// </summary>
        STATUS_MARSHALL_OVERFLOW = 0xC0000231,

        /// <summary>
        /// MessageId: STATUS_INVALID_VARIANT
        /// MessageText:
        /// The supplied variant structure contains invalid data.
        /// </summary>
        STATUS_INVALID_VARIANT = 0xC0000232,

        /// <summary>
        /// MessageId: STATUS_DOMAIN_CONTROLLER_NOT_FOUND
        /// MessageText:
        /// Could not find a domain controller for this domain.
        /// </summary>
        STATUS_DOMAIN_CONTROLLER_NOT_FOUND = 0xC0000233,

        /// <summary>
        /// MessageId: STATUS_ACCOUNT_LOCKED_OUT
        /// MessageText:
        /// The user account has been automatically locked because too many invalid logon attempts or password change attempts have been requested.
        /// </summary>
        STATUS_ACCOUNT_LOCKED_OUT = 0xC0000234,

        /// <summary>
        /// MessageId: STATUS_HANDLE_NOT_CLOSABLE
        /// MessageText:
        /// NtClose was called on a handle that was protected from close via NtSetInformationObject.
        /// </summary>
        STATUS_HANDLE_NOT_CLOSABLE = 0xC0000235,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_REFUSED
        /// MessageText:
        /// The transport connection attempt was refused by the remote system.
        /// </summary>
        STATUS_CONNECTION_REFUSED = 0xC0000236,

        /// <summary>
        /// MessageId: STATUS_GRACEFUL_DISCONNECT
        /// MessageText:
        /// The transport connection was gracefully closed.
        /// </summary>
        STATUS_GRACEFUL_DISCONNECT = 0xC0000237,

        /// <summary>
        /// MessageId: STATUS_ADDRESS_ALREADY_ASSOCIATED
        /// MessageText:
        /// The transport endpoint already has an address associated with it.
        /// </summary>
        STATUS_ADDRESS_ALREADY_ASSOCIATED = 0xC0000238,

        /// <summary>
        /// MessageId: STATUS_ADDRESS_NOT_ASSOCIATED
        /// MessageText:
        /// An address has not yet been associated with the transport endpoint.
        /// </summary>
        STATUS_ADDRESS_NOT_ASSOCIATED = 0xC0000239,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_INVALID
        /// MessageText:
        /// An operation was attempted on a nonexistent transport connection.
        /// </summary>
        STATUS_CONNECTION_INVALID = 0xC000023A,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_ACTIVE
        /// MessageText:
        /// An invalid operation was attempted on an active transport connection.
        /// </summary>
        STATUS_CONNECTION_ACTIVE = 0xC000023B,

        /// <summary>
        /// MessageId: STATUS_NETWORK_UNREACHABLE
        /// MessageText:
        /// The remote network is not reachable by the transport.
        /// </summary>
        STATUS_NETWORK_UNREACHABLE = 0xC000023C,

        /// <summary>
        /// MessageId: STATUS_HOST_UNREACHABLE
        /// MessageText:
        /// The remote system is not reachable by the transport.
        /// </summary>
        STATUS_HOST_UNREACHABLE = 0xC000023D,

        /// <summary>
        /// MessageId: STATUS_PROTOCOL_UNREACHABLE
        /// MessageText:
        /// The remote system does not support the transport protocol.
        /// </summary>
        STATUS_PROTOCOL_UNREACHABLE = 0xC000023E,

        /// <summary>
        /// MessageId: STATUS_PORT_UNREACHABLE
        /// MessageText:
        /// No service is operating at the destination port of the transport on the remote system.
        /// </summary>
        STATUS_PORT_UNREACHABLE = 0xC000023F,

        /// <summary>
        /// MessageId: STATUS_REQUEST_ABORTED
        /// MessageText:
        /// The request was aborted.
        /// </summary>
        STATUS_REQUEST_ABORTED = 0xC0000240,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_ABORTED
        /// MessageText:
        /// The transport connection was aborted by the local system.
        /// </summary>
        STATUS_CONNECTION_ABORTED = 0xC0000241,

        /// <summary>
        /// MessageId: STATUS_BAD_COMPRESSION_BUFFER
        /// MessageText:
        /// The specified buffer contains ill-formed data.
        /// </summary>
        STATUS_BAD_COMPRESSION_BUFFER = 0xC0000242,

        /// <summary>
        /// MessageId: STATUS_USER_MAPPED_FILE
        /// MessageText:
        /// The requested operation cannot be performed on a file with a user mapped section open.
        /// </summary>
        STATUS_USER_MAPPED_FILE = 0xC0000243,

        /// <summary>
        /// MessageId: STATUS_AUDIT_FAILED
        /// MessageText:
        /// {Audit Failed}
        /// An attempt to generate a security audit failed.
        /// </summary>
        STATUS_AUDIT_FAILED = 0xC0000244,

        /// <summary>
        /// MessageId: STATUS_TIMER_RESOLUTION_NOT_SET
        /// MessageText:
        /// The timer resolution was not previously set by the current process.
        /// </summary>
        STATUS_TIMER_RESOLUTION_NOT_SET = 0xC0000245,

        /// <summary>
        /// MessageId: STATUS_CONNECTION_COUNT_LIMIT
        /// MessageText:
        /// A connection to the server could not be made because the limit on the number of concurrent connections for this account has been reached.
        /// </summary>
        STATUS_CONNECTION_COUNT_LIMIT = 0xC0000246,

        /// <summary>
        /// MessageId: STATUS_LOGIN_TIME_RESTRICTION
        /// MessageText:
        /// Attempting to login during an unauthorized time of day for this account.
        /// </summary>
        STATUS_LOGIN_TIME_RESTRICTION = 0xC0000247,

        /// <summary>
        /// MessageId: STATUS_LOGIN_WKSTA_RESTRICTION
        /// MessageText:
        /// The account is not authorized to login from this station.
        /// </summary>
        STATUS_LOGIN_WKSTA_RESTRICTION = 0xC0000248,

        /// <summary>
        /// MessageId: STATUS_IMAGE_MP_UP_MISMATCH
        /// MessageText:
        /// {UP/MP Image Mismatch}
        /// The image %hs has been modified for use on a uniprocessor system, but you are running it on a multiprocessor machine.
        /// Please reinstall the image file.
        /// </summary>
        STATUS_IMAGE_MP_UP_MISMATCH = 0xC0000249,

        /// <summary>
        /// MessageId: STATUS_INSUFFICIENT_LOGON_INFO
        /// MessageText:
        /// There is insufficient account information to log you on.
        /// </summary>
        STATUS_INSUFFICIENT_LOGON_INFO = 0xC0000250,

        /// <summary>
        /// MessageId: STATUS_BAD_DLL_ENTRYPOINT
        /// MessageText:
        /// {Invalid DLL Entrypoint}
        /// The dynamic link library %hs is not written correctly. The stack pointer has been left in an inconsistent state.
        /// The entrypoint should be declared as WINAPI or STDCALL. Select YES to fail the DLL load. Select NO to continue execution. Selecting NO may cause the application to operate incorrectly.
        /// </summary>
        STATUS_BAD_DLL_ENTRYPOINT = 0xC0000251,

        /// <summary>
        /// MessageId: STATUS_BAD_SERVICE_ENTRYPOINT
        /// MessageText:
        /// {Invalid Service Callback Entrypoint}
        /// The %hs service is not written correctly. The stack pointer has been left in an inconsistent state.
        /// The callback entrypoint should be declared as WINAPI or STDCALL. Selecting OK will cause the service to continue operation. However, the service process may operate incorrectly.
        /// </summary>
        STATUS_BAD_SERVICE_ENTRYPOINT = 0xC0000252,

        /// <summary>
        /// MessageId: STATUS_LPC_REPLY_LOST
        /// MessageText:
        /// The server received the messages but did not send a reply.
        /// </summary>
        STATUS_LPC_REPLY_LOST = 0xC0000253,

        /// <summary>
        /// MessageId: STATUS_IP_ADDRESS_CONFLICT1
        /// MessageText:
        /// There is an IP address conflict with another system on the network
        /// </summary>
        STATUS_IP_ADDRESS_CONFLICT1 = 0xC0000254,

        /// <summary>
        /// MessageId: STATUS_IP_ADDRESS_CONFLICT2
        /// MessageText:
        /// There is an IP address conflict with another system on the network
        /// </summary>
        STATUS_IP_ADDRESS_CONFLICT2 = 0xC0000255,

        /// <summary>
        /// MessageId: STATUS_REGISTRY_QUOTA_LIMIT
        /// MessageText:
        /// {Low On Registry Space}
        /// The system has reached the maximum size allowed for the system part of the registry.  Additional storage requests will be ignored.
        /// </summary>
        STATUS_REGISTRY_QUOTA_LIMIT = 0xC0000256,

        /// <summary>
        /// MessageId: STATUS_PATH_NOT_COVERED
        /// MessageText:
        /// The contacted server does not support the indicated part of the DFS namespace.
        /// </summary>
        STATUS_PATH_NOT_COVERED = 0xC0000257,

        /// <summary>
        /// MessageId: STATUS_NO_CALLBACK_ACTIVE
        /// MessageText:
        /// A callback return system service cannot be executed when no callback is active.
        /// </summary>
        STATUS_NO_CALLBACK_ACTIVE = 0xC0000258,

        /// <summary>
        /// MessageId: STATUS_LICENSE_QUOTA_EXCEEDED
        /// MessageText:
        /// The service being accessed is licensed for a particular number of connections.
        /// No more connections can be made to the service at this time because there are already as many connections as the service can accept.
        /// </summary>
        STATUS_LICENSE_QUOTA_EXCEEDED = 0xC0000259,

        /// <summary>
        /// MessageId: STATUS_PWD_TOO_SHORT
        /// MessageText:
        /// The password provided is too short to meet the policy of your user account.
        /// Please choose a longer password.
        /// </summary>
        STATUS_PWD_TOO_SHORT = 0xC000025A,

        /// <summary>
        /// MessageId: STATUS_PWD_TOO_RECENT
        /// MessageText:
        /// The policy of your user account does not allow you to change passwords too frequently.
        /// This is done to prevent users from changing back to a familiar, but potentially discovered, password.
        /// If you feel your password has been compromised then please contact your administrator immediately to have a new one assigned.
        /// </summary>
        STATUS_PWD_TOO_RECENT = 0xC000025B,

        /// <summary>
        /// MessageId: STATUS_PWD_HISTORY_CONFLICT
        /// MessageText:
        /// You have attempted to change your password to one that you have used in the past.
        /// The policy of your user account does not allow this. Please select a password that you have not previously used.
        /// </summary>
        STATUS_PWD_HISTORY_CONFLICT = 0xC000025C,

        /// <summary>
        /// MessageId: STATUS_PLUGPLAY_NO_DEVICE
        /// MessageText:
        /// You have attempted to load a legacy device driver while its device instance had been disabled.
        /// </summary>
        STATUS_PLUGPLAY_NO_DEVICE = 0xC000025E,

        /// <summary>
        /// MessageId: STATUS_UNSUPPORTED_COMPRESSION
        /// MessageText:
        /// The specified compression format is unsupported.
        /// </summary>
        STATUS_UNSUPPORTED_COMPRESSION = 0xC000025F,

        /// <summary>
        /// MessageId: STATUS_INVALID_HW_PROFILE
        /// MessageText:
        /// The specified hardware profile configuration is invalid.
        /// </summary>
        STATUS_INVALID_HW_PROFILE = 0xC0000260,

        /// <summary>
        /// MessageId: STATUS_INVALID_PLUGPLAY_DEVICE_PATH
        /// MessageText:
        /// The specified Plug and Play registry device path is invalid.
        /// </summary>
        STATUS_INVALID_PLUGPLAY_DEVICE_PATH = 0xC0000261,

        /// <summary>
        /// MessageId: STATUS_DRIVER_ORDINAL_NOT_FOUND
        /// MessageText:
        /// {Driver Entry Point Not Found}
        /// The %hs device driver could not locate the ordinal %ld in driver %hs.
        /// </summary>
        STATUS_DRIVER_ORDINAL_NOT_FOUND = 0xC0000262,

        /// <summary>
        /// MessageId: STATUS_DRIVER_ENTRYPOINT_NOT_FOUND
        /// MessageText:
        /// {Driver Entry Point Not Found}
        /// The %hs device driver could not locate the entry point %hs in driver %hs.
        /// </summary>
        STATUS_DRIVER_ENTRYPOINT_NOT_FOUND = 0xC0000263,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_NOT_OWNED
        /// MessageText:
        /// {Application Error}
        /// The application attempted to release a resource it did not own. Click OK to terminate the application.
        /// </summary>
        STATUS_RESOURCE_NOT_OWNED = 0xC0000264,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_LINKS
        /// MessageText:
        /// An attempt was made to create more links on a file than the file system supports.
        /// </summary>
        STATUS_TOO_MANY_LINKS = 0xC0000265,

        /// <summary>
        /// MessageId: STATUS_QUOTA_LIST_INCONSISTENT
        /// MessageText:
        /// The specified quota list is internally inconsistent with its descriptor.
        /// </summary>
        STATUS_QUOTA_LIST_INCONSISTENT = 0xC0000266,

        /// <summary>
        /// MessageId: STATUS_FILE_IS_OFFLINE
        /// MessageText:
        /// The specified file has been relocated to offline storage.
        /// </summary>
        STATUS_FILE_IS_OFFLINE = 0xC0000267,

        /// <summary>
        /// MessageId: STATUS_EVALUATION_EXPIRATION
        /// MessageText:
        /// {Windows Evaluation Notification}
        /// The evaluation period for this installation of Windows has expired. This system will shutdown in 1 hour. To restore access to this installation of Windows, please upgrade this installation using a licensed distribution of this product.
        /// </summary>
        STATUS_EVALUATION_EXPIRATION = 0xC0000268,

        /// <summary>
        /// MessageId: STATUS_ILLEGAL_DLL_RELOCATION
        /// MessageText:
        /// {Illegal System DLL Relocation}
        /// The system DLL %hs was relocated in memory. The application will not run properly.
        /// The relocation occurred because the DLL %hs used an address range reserved for Windows system DLLs. The vendor supplying the DLL should be contacted for a new DLL.
        /// </summary>
        STATUS_ILLEGAL_DLL_RELOCATION = 0xC0000269,

        /// <summary>
        /// MessageId: STATUS_LICENSE_VIOLATION
        /// MessageText:
        /// {License Violation}
        /// The system has detected tampering with your registered product type. This is a violation of your software license. Tampering with product type is not permitted.
        /// </summary>
        STATUS_LICENSE_VIOLATION = 0xC000026A,

        /// <summary>
        /// MessageId: STATUS_DLL_INIT_FAILED_LOGOFF
        /// MessageText:
        /// {DLL Initialization Failed}
        /// The application failed to initialize because the window station is shutting down.
        /// </summary>
        STATUS_DLL_INIT_FAILED_LOGOFF = 0xC000026B,

        /// <summary>
        /// MessageId: STATUS_DRIVER_UNABLE_TO_LOAD
        /// MessageText:
        /// {Unable to Load Device Driver}
        /// %hs device driver could not be loaded.
        /// Error Status was 0x%x
        /// </summary>
        STATUS_DRIVER_UNABLE_TO_LOAD = 0xC000026C,

        /// <summary>
        /// MessageId: STATUS_DFS_UNAVAILABLE
        /// MessageText:
        /// DFS is unavailable on the contacted server.
        /// </summary>
        STATUS_DFS_UNAVAILABLE = 0xC000026D,

        /// <summary>
        /// MessageId: STATUS_VOLUME_DISMOUNTED
        /// MessageText:
        /// An operation was attempted to a volume after it was dismounted.
        /// </summary>
        STATUS_VOLUME_DISMOUNTED = 0xC000026E,

        /// <summary>
        /// MessageId: STATUS_WX86_INTERNAL_ERROR
        /// MessageText:
        /// An internal error occurred in the Win32 x86 emulation subsystem.
        /// </summary>
        STATUS_WX86_INTERNAL_ERROR = 0xC000026F,

        /// <summary>
        /// MessageId: STATUS_WX86_FLOAT_STACK_CHECK
        /// MessageText:
        /// Win32 x86 emulation subsystem Floating-point stack check.
        /// </summary>
        STATUS_WX86_FLOAT_STACK_CHECK = 0xC0000270,

        /// <summary>
        /// MessageId: STATUS_VALIDATE_CONTINUE
        /// MessageText:
        /// The validation process needs to continue on to the next step.
        /// </summary>
        STATUS_VALIDATE_CONTINUE = 0xC0000271,

        /// <summary>
        /// MessageId: STATUS_NO_MATCH
        /// MessageText:
        /// There was no match for the specified key in the index.
        /// </summary>
        STATUS_NO_MATCH = 0xC0000272,

        /// <summary>
        /// MessageId: STATUS_NO_MORE_MATCHES
        /// MessageText:
        /// There are no more matches for the current index enumeration.
        /// </summary>
        STATUS_NO_MORE_MATCHES = 0xC0000273,

        /// <summary>
        /// MessageId: STATUS_NOT_A_REPARSE_POINT
        /// MessageText:
        /// The NTFS file or directory is not a reparse point.
        /// </summary>
        STATUS_NOT_A_REPARSE_POINT = 0xC0000275,

        /// <summary>
        /// MessageId: STATUS_IO_REPARSE_TAG_INVALID
        /// MessageText:
        /// The Windows I/O reparse tag passed for the NTFS reparse point is invalid.
        /// </summary>
        STATUS_IO_REPARSE_TAG_INVALID = 0xC0000276,

        /// <summary>
        /// MessageId: STATUS_IO_REPARSE_TAG_MISMATCH
        /// MessageText:
        /// The Windows I/O reparse tag does not match the one present in the NTFS reparse point.
        /// </summary>
        STATUS_IO_REPARSE_TAG_MISMATCH = 0xC0000277,

        /// <summary>
        /// MessageId: STATUS_IO_REPARSE_DATA_INVALID
        /// MessageText:
        /// The user data passed for the NTFS reparse point is invalid.
        /// </summary>
        STATUS_IO_REPARSE_DATA_INVALID = 0xC0000278,

        /// <summary>
        /// MessageId: STATUS_IO_REPARSE_TAG_NOT_HANDLED
        /// MessageText:
        /// The layered file system driver for this IO tag did not handle it when needed.
        /// </summary>
        STATUS_IO_REPARSE_TAG_NOT_HANDLED = 0xC0000279,

        /// <summary>
        /// MessageId: STATUS_REPARSE_POINT_NOT_RESOLVED
        /// MessageText:
        /// The NTFS symbolic link could not be resolved even though the initial file name is valid.
        /// </summary>
        STATUS_REPARSE_POINT_NOT_RESOLVED = 0xC0000280,

        /// <summary>
        /// MessageId: STATUS_DIRECTORY_IS_A_REPARSE_POINT
        /// MessageText:
        /// The NTFS directory is a reparse point.
        /// </summary>
        STATUS_DIRECTORY_IS_A_REPARSE_POINT = 0xC0000281,

        /// <summary>
        /// MessageId: STATUS_RANGE_LIST_CONFLICT
        /// MessageText:
        /// The range could not be added to the range list because of a conflict.
        /// </summary>
        STATUS_RANGE_LIST_CONFLICT = 0xC0000282,

        /// <summary>
        /// MessageId: STATUS_SOURCE_ELEMENT_EMPTY
        /// MessageText:
        /// The specified medium changer source element contains no media.
        /// </summary>
        STATUS_SOURCE_ELEMENT_EMPTY = 0xC0000283,

        /// <summary>
        /// MessageId: STATUS_DESTINATION_ELEMENT_FULL
        /// MessageText:
        /// The specified medium changer destination element already contains media.
        /// </summary>
        STATUS_DESTINATION_ELEMENT_FULL = 0xC0000284,

        /// <summary>
        /// MessageId: STATUS_ILLEGAL_ELEMENT_ADDRESS
        /// MessageText:
        /// The specified medium changer element does not exist.
        /// </summary>
        STATUS_ILLEGAL_ELEMENT_ADDRESS = 0xC0000285,

        /// <summary>
        /// MessageId: STATUS_MAGAZINE_NOT_PRESENT
        /// MessageText:
        /// The specified element is contained within a magazine that is no longer present.
        /// </summary>
        STATUS_MAGAZINE_NOT_PRESENT = 0xC0000286,

        /// <summary>
        /// MessageId: STATUS_REINITIALIZATION_NEEDED
        /// MessageText:
        /// The device requires reinitialization due to hardware errors.
        /// </summary>
        STATUS_REINITIALIZATION_NEEDED = 0xC0000287,

        /// <summary>
        /// MessageId: STATUS_DEVICE_REQUIRES_CLEANING
        /// MessageText:
        /// The device has indicated that cleaning is necessary.
        /// </summary>
        STATUS_DEVICE_REQUIRES_CLEANING = 0x80000288,

        /// <summary>
        /// MessageId: STATUS_DEVICE_DOOR_OPEN
        /// MessageText:
        /// The device has indicated that it's door is open. Further operations require it closed and secured.
        /// </summary>
        STATUS_DEVICE_DOOR_OPEN = 0x80000289,

        /// <summary>
        /// MessageId: STATUS_ENCRYPTION_FAILED
        /// MessageText:
        /// The file encryption attempt failed.
        /// </summary>
        STATUS_ENCRYPTION_FAILED = 0xC000028A,

        /// <summary>
        /// MessageId: STATUS_DECRYPTION_FAILED
        /// MessageText:
        /// The file decryption attempt failed.
        /// </summary>
        STATUS_DECRYPTION_FAILED = 0xC000028B,

        /// <summary>
        /// MessageId: STATUS_RANGE_NOT_FOUND
        /// MessageText:
        /// The specified range could not be found in the range list.
        /// </summary>
        STATUS_RANGE_NOT_FOUND = 0xC000028C,

        /// <summary>
        /// MessageId: STATUS_NO_RECOVERY_POLICY
        /// MessageText:
        /// There is no encryption recovery policy configured for this system.
        /// </summary>
        STATUS_NO_RECOVERY_POLICY = 0xC000028D,

        /// <summary>
        /// MessageId: STATUS_NO_EFS
        /// MessageText:
        /// The required encryption driver is not loaded for this system.
        /// </summary>
        STATUS_NO_EFS = 0xC000028E,

        /// <summary>
        /// MessageId: STATUS_WRONG_EFS
        /// MessageText:
        /// The file was encrypted with a different encryption driver than is currently loaded.
        /// </summary>
        STATUS_WRONG_EFS = 0xC000028F,

        /// <summary>
        /// MessageId: STATUS_NO_USER_KEYS
        /// MessageText:
        /// There are no EFS keys defined for the user.
        /// </summary>
        STATUS_NO_USER_KEYS = 0xC0000290,

        /// <summary>
        /// MessageId: STATUS_FILE_NOT_ENCRYPTED
        /// MessageText:
        /// The specified file is not encrypted.
        /// </summary>
        STATUS_FILE_NOT_ENCRYPTED = 0xC0000291,

        /// <summary>
        /// MessageId: STATUS_NOT_EXPORT_FORMAT
        /// MessageText:
        /// The specified file is not in the defined EFS export format.
        /// </summary>
        STATUS_NOT_EXPORT_FORMAT = 0xC0000292,

        /// <summary>
        /// MessageId: STATUS_FILE_ENCRYPTED
        /// MessageText:
        /// The specified file is encrypted and the user does not have the ability to decrypt it.
        /// </summary>
        STATUS_FILE_ENCRYPTED = 0xC0000293,

        /// <summary>
        /// MessageId: STATUS_WAKE_SYSTEM
        /// MessageText:
        /// The system has awoken
        /// </summary>
        STATUS_WAKE_SYSTEM = 0x40000294,

        /// <summary>
        /// MessageId: STATUS_WMI_GUID_NOT_FOUND
        /// MessageText:
        /// The guid passed was not recognized as valid by a WMI data provider.
        /// </summary>
        STATUS_WMI_GUID_NOT_FOUND = 0xC0000295,

        /// <summary>
        /// MessageId: STATUS_WMI_INSTANCE_NOT_FOUND
        /// MessageText:
        /// The instance name passed was not recognized as valid by a WMI data provider.
        /// </summary>
        STATUS_WMI_INSTANCE_NOT_FOUND = 0xC0000296,

        /// <summary>
        /// MessageId: STATUS_WMI_ITEMID_NOT_FOUND
        /// MessageText:
        /// The data item id passed was not recognized as valid by a WMI data provider.
        /// </summary>
        STATUS_WMI_ITEMID_NOT_FOUND = 0xC0000297,

        /// <summary>
        /// MessageId: STATUS_WMI_TRY_AGAIN
        /// MessageText:
        /// The WMI request could not be completed and should be retried.
        /// </summary>
        STATUS_WMI_TRY_AGAIN = 0xC0000298,

        /// <summary>
        /// MessageId: STATUS_SHARED_POLICY
        /// MessageText:
        /// The policy object is shared and can only be modified at the root
        /// </summary>
        STATUS_SHARED_POLICY = 0xC0000299,

        /// <summary>
        /// MessageId: STATUS_POLICY_OBJECT_NOT_FOUND
        /// MessageText:
        /// The policy object does not exist when it should
        /// </summary>
        STATUS_POLICY_OBJECT_NOT_FOUND = 0xC000029A,

        /// <summary>
        /// MessageId: STATUS_POLICY_ONLY_IN_DS
        /// MessageText:
        /// The requested policy information only lives in the Ds
        /// </summary>
        STATUS_POLICY_ONLY_IN_DS = 0xC000029B,

        /// <summary>
        /// MessageId: STATUS_VOLUME_NOT_UPGRADED
        /// MessageText:
        /// The volume must be upgraded to enable this feature
        /// </summary>
        STATUS_VOLUME_NOT_UPGRADED = 0xC000029C,

        /// <summary>
        /// MessageId: STATUS_REMOTE_STORAGE_NOT_ACTIVE
        /// MessageText:
        /// The remote storage service is not operational at this time.
        /// </summary>
        STATUS_REMOTE_STORAGE_NOT_ACTIVE = 0xC000029D,

        /// <summary>
        /// MessageId: STATUS_REMOTE_STORAGE_MEDIA_ERROR
        /// MessageText:
        /// The remote storage service encountered a media error.
        /// </summary>
        STATUS_REMOTE_STORAGE_MEDIA_ERROR = 0xC000029E,

        /// <summary>
        /// MessageId: STATUS_NO_TRACKING_SERVICE
        /// MessageText:
        /// The tracking (workstation) service is not running.
        /// </summary>
        STATUS_NO_TRACKING_SERVICE = 0xC000029F,

        /// <summary>
        /// MessageId: STATUS_SERVER_SID_MISMATCH
        /// MessageText:
        /// The server process is running under a SID different than that required by client.
        /// </summary>
        STATUS_SERVER_SID_MISMATCH = 0xC00002A0,

        /// <summary>
        /// Directory Service specific Errors
        /// MessageId: STATUS_DS_NO_ATTRIBUTE_OR_VALUE
        /// MessageText:
        /// The specified directory service attribute or value does not exist.
        /// </summary>
        STATUS_DS_NO_ATTRIBUTE_OR_VALUE = 0xC00002A1,

        /// <summary>
        /// MessageId: STATUS_DS_INVALID_ATTRIBUTE_SYNTAX
        /// MessageText:
        /// The attribute syntax specified to the directory service is invalid.
        /// </summary>
        STATUS_DS_INVALID_ATTRIBUTE_SYNTAX = 0xC00002A2,

        /// <summary>
        /// MessageId: STATUS_DS_ATTRIBUTE_TYPE_UNDEFINED
        /// MessageText:
        /// The attribute type specified to the directory service is not defined.
        /// </summary>
        STATUS_DS_ATTRIBUTE_TYPE_UNDEFINED = 0xC00002A3,

        /// <summary>
        /// MessageId: STATUS_DS_ATTRIBUTE_OR_VALUE_EXISTS
        /// MessageText:
        /// The specified directory service attribute or value already exists.
        /// </summary>
        STATUS_DS_ATTRIBUTE_OR_VALUE_EXISTS = 0xC00002A4,

        /// <summary>
        /// MessageId: STATUS_DS_BUSY
        /// MessageText:
        /// The directory service is busy.
        /// </summary>
        STATUS_DS_BUSY = 0xC00002A5,

        /// <summary>
        /// MessageId: STATUS_DS_UNAVAILABLE
        /// MessageText:
        /// The directory service is not available.
        /// </summary>
        STATUS_DS_UNAVAILABLE = 0xC00002A6,

        /// <summary>
        /// MessageId: STATUS_DS_NO_RIDS_ALLOCATED
        /// MessageText:
        /// The directory service was unable to allocate a relative identifier.
        /// </summary>
        STATUS_DS_NO_RIDS_ALLOCATED = 0xC00002A7,

        /// <summary>
        /// MessageId: STATUS_DS_NO_MORE_RIDS
        /// MessageText:
        /// The directory service has exhausted the pool of relative identifiers.
        /// </summary>
        STATUS_DS_NO_MORE_RIDS = 0xC00002A8,

        /// <summary>
        /// MessageId: STATUS_DS_INCORRECT_ROLE_OWNER
        /// MessageText:
        /// The requested operation could not be performed because the directory service is not the master for that type of operation.
        /// </summary>
        STATUS_DS_INCORRECT_ROLE_OWNER = 0xC00002A9,

        /// <summary>
        /// MessageId: STATUS_DS_RIDMGR_INIT_ERROR
        /// MessageText:
        /// The directory service was unable to initialize the subsystem that allocates relative identifiers.
        /// </summary>
        STATUS_DS_RIDMGR_INIT_ERROR = 0xC00002AA,

        /// <summary>
        /// MessageId: STATUS_DS_OBJ_CLASS_VIOLATION
        /// MessageText:
        /// The requested operation did not satisfy one or more constraints associated with the class of the object.
        /// </summary>
        STATUS_DS_OBJ_CLASS_VIOLATION = 0xC00002AB,

        /// <summary>
        /// MessageId: STATUS_DS_CANT_ON_NON_LEAF
        /// MessageText:
        /// The directory service can perform the requested operation only on a leaf object.
        /// </summary>
        STATUS_DS_CANT_ON_NON_LEAF = 0xC00002AC,

        /// <summary>
        /// MessageId: STATUS_DS_CANT_ON_RDN
        /// MessageText:
        /// The directory service cannot perform the requested operation on the Relatively Defined Name (RDN) attribute of an object.
        /// </summary>
        STATUS_DS_CANT_ON_RDN = 0xC00002AD,

        /// <summary>
        /// MessageId: STATUS_DS_CANT_MOD_OBJ_CLASS
        /// MessageText:
        /// The directory service detected an attempt to modify the object class of an object.
        /// </summary>
        STATUS_DS_CANT_MOD_OBJ_CLASS = 0xC00002AE,

        /// <summary>
        /// MessageId: STATUS_DS_CROSS_DOM_MOVE_FAILED
        /// MessageText:
        /// An error occurred while performing a cross domain move operation.
        /// </summary>
        STATUS_DS_CROSS_DOM_MOVE_FAILED = 0xC00002AF,

        /// <summary>
        /// MessageId: STATUS_DS_GC_NOT_AVAILABLE
        /// MessageText:
        /// Unable to Contact the Global Catalog Server.
        /// </summary>
        STATUS_DS_GC_NOT_AVAILABLE = 0xC00002B0,

        /// <summary>
        /// MessageId: STATUS_DIRECTORY_SERVICE_REQUIRED
        /// MessageText:
        /// The requested operation requires a directory service, and none was available.
        /// </summary>
        STATUS_DIRECTORY_SERVICE_REQUIRED = 0xC00002B1,

        /// <summary>
        /// MessageId: STATUS_REPARSE_ATTRIBUTE_CONFLICT
        /// MessageText:
        /// The reparse attribute cannot be set as it is incompatible with an existing attribute.
        /// </summary>
        STATUS_REPARSE_ATTRIBUTE_CONFLICT = 0xC00002B2,

        /// <summary>
        /// MessageId: STATUS_CANT_ENABLE_DENY_ONLY
        /// MessageText:
        /// A group marked use for deny only cannot be enabled.
        /// </summary>
        STATUS_CANT_ENABLE_DENY_ONLY = 0xC00002B3,

        /// <summary>
        /// MessageId: STATUS_FLOAT_MULTIPLE_FAULTS
        /// MessageText:
        /// {EXCEPTION}
        /// Multiple floating point faults.
        /// </summary>
        STATUS_FLOAT_MULTIPLE_FAULTS = 0xC00002B4,

        /// <summary>
        /// MessageId: STATUS_FLOAT_MULTIPLE_TRAPS
        /// MessageText:
        /// {EXCEPTION}
        /// Multiple floating point traps.
        /// </summary>
        STATUS_FLOAT_MULTIPLE_TRAPS = 0xC00002B5,

        /// <summary>
        /// MessageId: STATUS_DEVICE_REMOVED
        /// MessageText:
        /// The device has been removed.
        /// </summary>
        STATUS_DEVICE_REMOVED = 0xC00002B6,

        /// <summary>
        /// MessageId: STATUS_JOURNAL_DELETE_IN_PROGRESS
        /// MessageText:
        /// The volume change journal is being deleted.
        /// </summary>
        STATUS_JOURNAL_DELETE_IN_PROGRESS = 0xC00002B7,

        /// <summary>
        /// MessageId: STATUS_JOURNAL_NOT_ACTIVE
        /// MessageText:
        /// The volume change journal is not active.
        /// </summary>
        STATUS_JOURNAL_NOT_ACTIVE = 0xC00002B8,

        /// <summary>
        /// MessageId: STATUS_NOINTERFACE
        /// MessageText:
        /// The requested interface is not supported.
        /// </summary>
        STATUS_NOINTERFACE = 0xC00002B9,

        /// <summary>
        /// MessageId: STATUS_DS_ADMIN_LIMIT_EXCEEDED
        /// MessageText:
        /// A directory service resource limit has been exceeded.
        /// </summary>
        STATUS_DS_ADMIN_LIMIT_EXCEEDED = 0xC00002C1,

        /// <summary>
        /// MessageId: STATUS_DRIVER_FAILED_SLEEP
        /// MessageText:
        /// {System Standby Failed}
        /// The driver %hs does not support standby mode. Updating this driver may allow the system to go to standby mode.
        /// </summary>
        STATUS_DRIVER_FAILED_SLEEP = 0xC00002C2,

        /// <summary>
        /// MessageId: STATUS_MUTUAL_AUTHENTICATION_FAILED
        /// MessageText:
        /// Mutual Authentication failed. The server's password is out of date at the domain controller.
        /// </summary>
        STATUS_MUTUAL_AUTHENTICATION_FAILED = 0xC00002C3,

        /// <summary>
        /// MessageId: STATUS_CORRUPT_SYSTEM_FILE
        /// MessageText:
        /// The system file %1 has become corrupt and has been replaced.
        /// </summary>
        STATUS_CORRUPT_SYSTEM_FILE = 0xC00002C4,

        /// <summary>
        /// MessageId: STATUS_DATATYPE_MISALIGNMENT_ERROR
        /// MessageText:
        /// {EXCEPTION}
        /// Alignment Error
        /// A datatype misalignment error was detected in a load or store instruction.
        /// </summary>
        STATUS_DATATYPE_MISALIGNMENT_ERROR = 0xC00002C5,

        /// <summary>
        /// MessageId: STATUS_WMI_READ_ONLY
        /// MessageText:
        /// The WMI data item or data block is read only.
        /// </summary>
        STATUS_WMI_READ_ONLY = 0xC00002C6,

        /// <summary>
        /// MessageId: STATUS_WMI_SET_FAILURE
        /// MessageText:
        /// The WMI data item or data block could not be changed.
        /// </summary>
        STATUS_WMI_SET_FAILURE = 0xC00002C7,

        /// <summary>
        /// MessageId: STATUS_COMMITMENT_MINIMUM
        /// MessageText:
        /// {Virtual Memory Minimum Too Low}
        /// Your system is low on virtual memory. Windows is increasing the size of your virtual memory paging file.
        /// During this process, memory requests for some applications may be denied. For more information, see Help.
        /// </summary>
        STATUS_COMMITMENT_MINIMUM = 0xC00002C8,

        /// <summary>
        /// MessageId: STATUS_REG_NAT_CONSUMPTION
        /// MessageText:
        /// {EXCEPTION}
        /// Register NaT consumption faults.
        /// A NaT value is consumed on a non speculative instruction.
        /// </summary>
        STATUS_REG_NAT_CONSUMPTION = 0xC00002C9,

        /// <summary>
        /// MessageId: STATUS_TRANSPORT_FULL
        /// MessageText:
        /// The medium changer's transport element contains media, which is causing the operation to fail.
        /// </summary>
        STATUS_TRANSPORT_FULL = 0xC00002CA,

        /// <summary>
        /// MessageId: STATUS_DS_SAM_INIT_FAILURE
        /// MessageText:
        /// Security Accounts Manager initialization failed because of the following error:
        /// %hs
        /// Error Status: 0x%x.
        /// Please click OK to shutdown this system and reboot into Directory Services Restore Mode, check the event log for more detailed information.
        /// </summary>
        STATUS_DS_SAM_INIT_FAILURE = 0xC00002CB,

        /// <summary>
        /// MessageId: STATUS_ONLY_IF_CONNECTED
        /// MessageText:
        /// This operation is supported only when you are connected to the server.
        /// </summary>
        STATUS_ONLY_IF_CONNECTED = 0xC00002CC,

        /// <summary>
        /// MessageId: STATUS_DS_SENSITIVE_GROUP_VIOLATION
        /// MessageText:
        /// Only an administrator can modify the membership list of an administrative group.
        /// </summary>
        STATUS_DS_SENSITIVE_GROUP_VIOLATION = 0xC00002CD,

        /// <summary>
        /// MessageId: STATUS_PNP_RESTART_ENUMERATION
        /// MessageText:
        /// A device was removed so enumeration must be restarted.
        /// </summary>
        STATUS_PNP_RESTART_ENUMERATION = 0xC00002CE,

        /// <summary>
        /// MessageId: STATUS_JOURNAL_ENTRY_DELETED
        /// MessageText:
        /// The journal entry has been deleted from the journal.
        /// </summary>
        STATUS_JOURNAL_ENTRY_DELETED = 0xC00002CF,

        /// <summary>
        /// MessageId: STATUS_DS_CANT_MOD_PRIMARYGROUPID
        /// MessageText:
        /// Cannot change the primary group ID of a domain controller account.
        /// </summary>
        STATUS_DS_CANT_MOD_PRIMARYGROUPID = 0xC00002D0,

        /// <summary>
        /// MessageId: STATUS_SYSTEM_IMAGE_BAD_SIGNATURE
        /// MessageText:
        /// {Fatal System Error}
        /// The system image %s is not properly signed.
        /// The file has been replaced with the signed file.
        /// The system has been shut down.
        /// </summary>
        STATUS_SYSTEM_IMAGE_BAD_SIGNATURE = 0xC00002D1,

        /// <summary>
        /// MessageId: STATUS_PNP_REBOOT_REQUIRED
        /// MessageText:
        /// Device will not start without a reboot.
        /// </summary>
        STATUS_PNP_REBOOT_REQUIRED = 0xC00002D2,

        /// <summary>
        /// MessageId: STATUS_POWER_STATE_INVALID
        /// MessageText:
        /// Current device power state cannot support this request.
        /// </summary>
        STATUS_POWER_STATE_INVALID = 0xC00002D3,

        /// <summary>
        /// MessageId: STATUS_DS_INVALID_GROUP_TYPE
        /// MessageText:
        /// The specified group type is invalid.
        /// </summary>
        STATUS_DS_INVALID_GROUP_TYPE = 0xC00002D4,

        /// <summary>
        /// MessageId: STATUS_DS_NO_NEST_GLOBALGROUP_IN_MIXEDDOMAIN
        /// MessageText:
        /// In mixed domain no nesting of global group if group is security enabled.
        /// </summary>
        STATUS_DS_NO_NEST_GLOBALGROUP_IN_MIXEDDOMAIN = 0xC00002D5,

        /// <summary>
        /// MessageId: STATUS_DS_NO_NEST_LOCALGROUP_IN_MIXEDDOMAIN
        /// MessageText:
        /// In mixed domain, cannot nest local groups with other local groups, if the group is security enabled.
        /// </summary>
        STATUS_DS_NO_NEST_LOCALGROUP_IN_MIXEDDOMAIN = 0xC00002D6,

        /// <summary>
        /// MessageId: STATUS_DS_GLOBAL_CANT_HAVE_LOCAL_MEMBER
        /// MessageText:
        /// A global group cannot have a local group as a member.
        /// </summary>
        STATUS_DS_GLOBAL_CANT_HAVE_LOCAL_MEMBER = 0xC00002D7,

        /// <summary>
        /// MessageId: STATUS_DS_GLOBAL_CANT_HAVE_UNIVERSAL_MEMBER
        /// MessageText:
        /// A global group cannot have a universal group as a member.
        /// </summary>
        STATUS_DS_GLOBAL_CANT_HAVE_UNIVERSAL_MEMBER = 0xC00002D8,

        /// <summary>
        /// MessageId: STATUS_DS_UNIVERSAL_CANT_HAVE_LOCAL_MEMBER
        /// MessageText:
        /// A universal group cannot have a local group as a member.
        /// </summary>
        STATUS_DS_UNIVERSAL_CANT_HAVE_LOCAL_MEMBER = 0xC00002D9,

        /// <summary>
        /// MessageId: STATUS_DS_GLOBAL_CANT_HAVE_CROSSDOMAIN_MEMBER
        /// MessageText:
        /// A global group cannot have a cross domain member.
        /// </summary>
        STATUS_DS_GLOBAL_CANT_HAVE_CROSSDOMAIN_MEMBER = 0xC00002DA,

        /// <summary>
        /// MessageId: STATUS_DS_LOCAL_CANT_HAVE_CROSSDOMAIN_LOCAL_MEMBER
        /// MessageText:
        /// A local group cannot have another cross domain local group as a member.
        /// </summary>
        STATUS_DS_LOCAL_CANT_HAVE_CROSSDOMAIN_LOCAL_MEMBER = 0xC00002DB,

        /// <summary>
        /// MessageId: STATUS_DS_HAVE_PRIMARY_MEMBERS
        /// MessageText:
        /// Cannot change to security disabled group because of having primary members in this group.
        /// </summary>
        STATUS_DS_HAVE_PRIMARY_MEMBERS = 0xC00002DC,

        /// <summary>
        /// MessageId: STATUS_WMI_NOT_SUPPORTED
        /// MessageText:
        /// The WMI operation is not supported by the data block or method.
        /// </summary>
        STATUS_WMI_NOT_SUPPORTED = 0xC00002DD,

        /// <summary>
        /// MessageId: STATUS_INSUFFICIENT_POWER
        /// MessageText:
        /// There is not enough power to complete the requested operation.
        /// </summary>
        STATUS_INSUFFICIENT_POWER = 0xC00002DE,

        /// <summary>
        /// MessageId: STATUS_SAM_NEED_BOOTKEY_PASSWORD
        /// MessageText:
        /// Security Account Manager needs to get the boot password.
        /// </summary>
        STATUS_SAM_NEED_BOOTKEY_PASSWORD = 0xC00002DF,

        /// <summary>
        /// MessageId: STATUS_SAM_NEED_BOOTKEY_FLOPPY
        /// MessageText:
        /// Security Account Manager needs to get the boot key from floppy disk.
        /// </summary>
        STATUS_SAM_NEED_BOOTKEY_FLOPPY = 0xC00002E0,

        /// <summary>
        /// MessageId: STATUS_DS_CANT_START
        /// MessageText:
        /// Directory Service cannot start.
        /// </summary>
        STATUS_DS_CANT_START = 0xC00002E1,

        /// <summary>
        /// MessageId: STATUS_DS_INIT_FAILURE
        /// MessageText:
        /// Directory Services could not start because of the following error:
        /// %hs
        /// Error Status: 0x%x.
        /// Please click OK to shutdown this system and reboot into Directory Services Restore Mode, check the event log for more detailed information.
        /// </summary>
        STATUS_DS_INIT_FAILURE = 0xC00002E2,

        /// <summary>
        /// MessageId: STATUS_SAM_INIT_FAILURE
        /// MessageText:
        /// Security Accounts Manager initialization failed because of the following error:
        /// %hs
        /// Error Status: 0x%x.
        /// Please click OK to shutdown this system and reboot into Safe Mode, check the event log for more detailed information.
        /// </summary>
        STATUS_SAM_INIT_FAILURE = 0xC00002E3,

        /// <summary>
        /// MessageId: STATUS_DS_GC_REQUIRED
        /// MessageText:
        /// The requested operation can be performed only on a global catalog server.
        /// </summary>
        STATUS_DS_GC_REQUIRED = 0xC00002E4,

        /// <summary>
        /// MessageId: STATUS_DS_LOCAL_MEMBER_OF_LOCAL_ONLY
        /// MessageText:
        /// A local group can only be a member of other local groups in the same domain.
        /// </summary>
        STATUS_DS_LOCAL_MEMBER_OF_LOCAL_ONLY = 0xC00002E5,

        /// <summary>
        /// MessageId: STATUS_DS_NO_FPO_IN_UNIVERSAL_GROUPS
        /// MessageText:
        /// Foreign security principals cannot be members of universal groups.
        /// </summary>
        STATUS_DS_NO_FPO_IN_UNIVERSAL_GROUPS = 0xC00002E6,

        /// <summary>
        /// MessageId: STATUS_DS_MACHINE_ACCOUNT_QUOTA_EXCEEDED
        /// MessageText:
        /// Your computer could not be joined to the domain. You have exceeded the maximum number of computer accounts you are allowed to create in this domain. Contact your system administrator to have this limit reset or increased.
        /// </summary>
        STATUS_DS_MACHINE_ACCOUNT_QUOTA_EXCEEDED = 0xC00002E7,

        /// <summary>
        /// MessageId: STATUS_MULTIPLE_FAULT_VIOLATION
        /// MessageText:
        ///  STATUS_MULTIPLE_FAULT_VIOLATION
        /// </summary>
        STATUS_MULTIPLE_FAULT_VIOLATION = 0xC00002E8,

        /// <summary>
        /// MessageId: STATUS_CURRENT_DOMAIN_NOT_ALLOWED
        /// MessageText:
        /// This operation cannot be performed on the current domain.
        /// </summary>
        STATUS_CURRENT_DOMAIN_NOT_ALLOWED = 0xC00002E9,

        /// <summary>
        /// MessageId: STATUS_CANNOT_MAKE
        /// MessageText:
        /// The directory or file cannot be created.
        /// </summary>
        STATUS_CANNOT_MAKE = 0xC00002EA,

        /// <summary>
        /// MessageId: STATUS_SYSTEM_SHUTDOWN
        /// MessageText:
        /// The system is in the process of shutting down.
        /// </summary>
        STATUS_SYSTEM_SHUTDOWN = 0xC00002EB,

        /// <summary>
        /// MessageId: STATUS_DS_INIT_FAILURE_CONSOLE
        /// MessageText:
        /// Directory Services could not start because of the following error:
        /// %hs
        /// Error Status: 0x%x.
        /// Please click OK to shutdown the system. You can use the recovery console to diagnose the system further.
        /// </summary>
        STATUS_DS_INIT_FAILURE_CONSOLE = 0xC00002EC,

        /// <summary>
        /// MessageId: STATUS_DS_SAM_INIT_FAILURE_CONSOLE
        /// MessageText:
        /// Security Accounts Manager initialization failed because of the following error:
        /// %hs
        /// Error Status: 0x%x.
        /// Please click OK to shutdown the system. You can use the recovery console to diagnose the system further.
        /// </summary>
        STATUS_DS_SAM_INIT_FAILURE_CONSOLE = 0xC00002ED,

        /// <summary>
        /// MessageId: STATUS_UNFINISHED_CONTEXT_DELETED
        /// MessageText:
        /// A security context was deleted before the context was completed. This is considered a logon failure.
        /// </summary>
        STATUS_UNFINISHED_CONTEXT_DELETED = 0xC00002EE,

        /// <summary>
        /// MessageId: STATUS_NO_TGT_REPLY
        /// MessageText:
        /// The client is trying to negotiate a context and the server requires user-to-user but didn't send a TGT reply.
        /// </summary>
        STATUS_NO_TGT_REPLY = 0xC00002EF,

        /// <summary>
        /// MessageId: STATUS_OBJECTID_NOT_FOUND
        /// MessageText:
        /// No object ID was found in the file.
        /// </summary>
        STATUS_OBJECTID_NOT_FOUND = 0xC00002F0,

        /// <summary>
        /// MessageId: STATUS_NO_IP_ADDRESSES
        /// MessageText:
        /// Unable to accomplish the requested task because the local machine does not have any IP addresses.
        /// </summary>
        STATUS_NO_IP_ADDRESSES = 0xC00002F1,

        /// <summary>
        /// MessageId: STATUS_WRONG_CREDENTIAL_HANDLE
        /// MessageText:
        /// The supplied credential handle does not match the credential associated with the security context.
        /// </summary>
        STATUS_WRONG_CREDENTIAL_HANDLE = 0xC00002F2,

        /// <summary>
        /// MessageId: STATUS_CRYPTO_SYSTEM_INVALID
        /// MessageText:
        /// The crypto system or checksum function is invalid because a required function is unavailable.
        /// </summary>
        STATUS_CRYPTO_SYSTEM_INVALID = 0xC00002F3,

        /// <summary>
        /// MessageId: STATUS_MAX_REFERRALS_EXCEEDED
        /// MessageText:
        /// The number of maximum ticket referrals has been exceeded.
        /// </summary>
        STATUS_MAX_REFERRALS_EXCEEDED = 0xC00002F4,

        /// <summary>
        /// MessageId: STATUS_MUST_BE_KDC
        /// MessageText:
        /// The local machine must be a Kerberos KDC (domain controller) and it is not.
        /// </summary>
        STATUS_MUST_BE_KDC = 0xC00002F5,

        /// <summary>
        /// MessageId: STATUS_STRONG_CRYPTO_NOT_SUPPORTED
        /// MessageText:
        /// The other end of the security negotiation is requires strong crypto but it is not supported on the local machine.
        /// </summary>
        STATUS_STRONG_CRYPTO_NOT_SUPPORTED = 0xC00002F6,

        /// <summary>
        /// MessageId: STATUS_TOO_MANY_PRINCIPALS
        /// MessageText:
        /// The KDC reply contained more than one principal name.
        /// </summary>
        STATUS_TOO_MANY_PRINCIPALS = 0xC00002F7,

        /// <summary>
        /// MessageId: STATUS_NO_PA_DATA
        /// MessageText:
        /// Expected to find PA data for a hint of what etype to use, but it was not found.
        /// </summary>
        STATUS_NO_PA_DATA = 0xC00002F8,

        /// <summary>
        /// MessageId: STATUS_PKINIT_NAME_MISMATCH
        /// MessageText:
        /// The client certificate does not contain a valid UPN, or does not match the client name
        /// in the logon request.  Please contact your administrator.
        /// </summary>
        STATUS_PKINIT_NAME_MISMATCH = 0xC00002F9,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_LOGON_REQUIRED
        /// MessageText:
        /// Smartcard logon is required and was not used.
        /// </summary>
        STATUS_SMARTCARD_LOGON_REQUIRED = 0xC00002FA,

        /// <summary>
        /// MessageId: STATUS_KDC_INVALID_REQUEST
        /// MessageText:
        /// An invalid request was sent to the KDC.
        /// </summary>
        STATUS_KDC_INVALID_REQUEST = 0xC00002FB,

        /// <summary>
        /// MessageId: STATUS_KDC_UNABLE_TO_REFER
        /// MessageText:
        /// The KDC was unable to generate a referral for the service requested.
        /// </summary>
        STATUS_KDC_UNABLE_TO_REFER = 0xC00002FC,

        /// <summary>
        /// MessageId: STATUS_KDC_UNKNOWN_ETYPE
        /// MessageText:
        /// The encryption type requested is not supported by the KDC.
        /// </summary>
        STATUS_KDC_UNKNOWN_ETYPE = 0xC00002FD,

        /// <summary>
        /// MessageId: STATUS_SHUTDOWN_IN_PROGRESS
        /// MessageText:
        /// A system shutdown is in progress.
        /// </summary>
        STATUS_SHUTDOWN_IN_PROGRESS = 0xC00002FE,

        /// <summary>
        /// MessageId: STATUS_SERVER_SHUTDOWN_IN_PROGRESS
        /// MessageText:
        /// The server machine is shutting down.
        /// </summary>
        STATUS_SERVER_SHUTDOWN_IN_PROGRESS = 0xC00002FF,

        /// <summary>
        /// MessageId: STATUS_NOT_SUPPORTED_ON_SBS
        /// MessageText:
        /// This operation is not supported on a computer running Windows Server 2003 for Small Business Server
        /// </summary>
        STATUS_NOT_SUPPORTED_ON_SBS = 0xC0000300,

        /// <summary>
        /// MessageId: STATUS_WMI_GUID_DISCONNECTED
        /// MessageText:
        /// The WMI GUID is no longer available
        /// </summary>
        STATUS_WMI_GUID_DISCONNECTED = 0xC0000301,

        /// <summary>
        /// MessageId: STATUS_WMI_ALREADY_DISABLED
        /// MessageText:
        /// Collection or events for the WMI GUID is already disabled.
        /// </summary>
        STATUS_WMI_ALREADY_DISABLED = 0xC0000302,

        /// <summary>
        /// MessageId: STATUS_WMI_ALREADY_ENABLED
        /// MessageText:
        /// Collection or events for the WMI GUID is already enabled.
        /// </summary>
        STATUS_WMI_ALREADY_ENABLED = 0xC0000303,

        /// <summary>
        /// MessageId: STATUS_MFT_TOO_FRAGMENTED
        /// MessageText:
        /// The Master File Table on the volume is too fragmented to complete this operation.
        /// </summary>
        STATUS_MFT_TOO_FRAGMENTED = 0xC0000304,

        /// <summary>
        /// MessageId: STATUS_COPY_PROTECTION_FAILURE
        /// MessageText:
        /// Copy protection failure.
        /// </summary>
        STATUS_COPY_PROTECTION_FAILURE = 0xC0000305,

        /// <summary>
        /// MessageId: STATUS_CSS_AUTHENTICATION_FAILURE
        /// MessageText:
        /// Copy protection error - DVD CSS Authentication failed.
        /// </summary>
        STATUS_CSS_AUTHENTICATION_FAILURE = 0xC0000306,

        /// <summary>
        /// MessageId: STATUS_CSS_KEY_NOT_PRESENT
        /// MessageText:
        /// Copy protection error - The given sector does not contain a valid key.
        /// </summary>
        STATUS_CSS_KEY_NOT_PRESENT = 0xC0000307,

        /// <summary>
        /// MessageId: STATUS_CSS_KEY_NOT_ESTABLISHED
        /// MessageText:
        /// Copy protection error - DVD session key not established.
        /// </summary>
        STATUS_CSS_KEY_NOT_ESTABLISHED = 0xC0000308,

        /// <summary>
        /// MessageId: STATUS_CSS_SCRAMBLED_SECTOR
        /// MessageText:
        /// Copy protection error - The read failed because the sector is encrypted.
        /// </summary>
        STATUS_CSS_SCRAMBLED_SECTOR = 0xC0000309,

        /// <summary>
        /// MessageId: STATUS_CSS_REGION_MISMATCH
        /// MessageText:
        /// Copy protection error - The given DVD's region does not correspond to the
        /// region setting of the drive.
        /// </summary>
        STATUS_CSS_REGION_MISMATCH = 0xC000030A,

        /// <summary>
        /// MessageId: STATUS_CSS_RESETS_EXHAUSTED
        /// MessageText:
        /// Copy protection error - The drive's region setting may be permanent.
        /// </summary>
        STATUS_CSS_RESETS_EXHAUSTED = 0xC000030B,

        /// <summary>
        /// MessageId: STATUS_PKINIT_FAILURE
        /// MessageText:
        /// The Kerberos protocol encountered an error while validating the KDC certificate during smartcard Logon.  There
        /// is more information in the system event log.
        /// </summary>
        STATUS_PKINIT_FAILURE = 0xC0000320,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_SUBSYSTEM_FAILURE
        /// MessageText:
        /// The Kerberos protocol encountered an error while attempting to utilize the smartcard subsystem.
        /// </summary>
        STATUS_SMARTCARD_SUBSYSTEM_FAILURE = 0xC0000321,

        /// <summary>
        /// MessageId: STATUS_NO_KERB_KEY
        /// MessageText:
        /// The target server does not have acceptable Kerberos credentials.
        /// </summary>
        STATUS_NO_KERB_KEY = 0xC0000322,

        /// <summary>
        /// MessageId: STATUS_HOST_DOWN
        /// MessageText:
        /// The transport determined that the remote system is down.
        /// </summary>
        STATUS_HOST_DOWN = 0xC0000350,

        /// <summary>
        /// MessageId: STATUS_UNSUPPORTED_PREAUTH
        /// MessageText:
        /// An unsupported preauthentication mechanism was presented to the Kerberos package.
        /// </summary>
        STATUS_UNSUPPORTED_PREAUTH = 0xC0000351,

        /// <summary>
        /// MessageId: STATUS_EFS_ALG_BLOB_TOO_BIG
        /// MessageText:
        /// The encryption algorithm used on the source file needs a bigger key buffer than the one used on the destination file.
        /// </summary>
        STATUS_EFS_ALG_BLOB_TOO_BIG = 0xC0000352,

        /// <summary>
        /// MessageId: STATUS_PORT_NOT_SET
        /// MessageText:
        /// An attempt to remove a processes DebugPort was made, but a port was not already associated with the process.
        /// </summary>
        STATUS_PORT_NOT_SET = 0xC0000353,

        /// <summary>
        /// MessageId: STATUS_DEBUGGER_INACTIVE
        /// MessageText:
        /// An attempt to do an operation on a debug port failed because the port is in the process of being deleted.
        /// </summary>
        STATUS_DEBUGGER_INACTIVE = 0xC0000354,

        /// <summary>
        /// MessageId: STATUS_DS_VERSION_CHECK_FAILURE
        /// MessageText:
        /// This version of Windows is not compatible with the behavior version of directory forest, domain or domain controller.
        /// </summary>
        STATUS_DS_VERSION_CHECK_FAILURE = 0xC0000355,

        /// <summary>
        /// MessageId: STATUS_AUDITING_DISABLED
        /// MessageText:
        /// The specified event is currently not being audited.
        /// </summary>
        STATUS_AUDITING_DISABLED = 0xC0000356,

        /// <summary>
        /// MessageId: STATUS_PRENT4_MACHINE_ACCOUNT
        /// MessageText:
        /// The machine account was created pre-NT4. The account needs to be recreated.
        /// </summary>
        STATUS_PRENT4_MACHINE_ACCOUNT = 0xC0000357,

        /// <summary>
        /// MessageId: STATUS_DS_AG_CANT_HAVE_UNIVERSAL_MEMBER
        /// MessageText:
        /// An account group cannot have a universal group as a member.
        /// </summary>
        STATUS_DS_AG_CANT_HAVE_UNIVERSAL_MEMBER = 0xC0000358,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_WIN_32
        /// MessageText:
        /// The specified image file did not have the correct format, it appears to be a 32-bit Windows image.
        /// </summary>
        STATUS_INVALID_IMAGE_WIN_32 = 0xC0000359,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_WIN_64
        /// MessageText:
        /// The specified image file did not have the correct format, it appears to be a 64-bit Windows image.
        /// </summary>
        STATUS_INVALID_IMAGE_WIN_64 = 0xC000035A,

        /// <summary>
        /// MessageId: STATUS_BAD_BINDINGS
        /// MessageText:
        /// Client's supplied SSPI channel bindings were incorrect.
        /// </summary>
        STATUS_BAD_BINDINGS = 0xC000035B,

        /// <summary>
        /// MessageId: STATUS_NETWORK_SESSION_EXPIRED
        /// MessageText:
        /// The client's session has expired, so the client must reauthenticate to continue accessing the remote resources.
        /// </summary>
        STATUS_NETWORK_SESSION_EXPIRED = 0xC000035C,

        /// <summary>
        /// MessageId: STATUS_APPHELP_BLOCK
        /// MessageText:
        /// AppHelp dialog canceled thus preventing the application from starting.
        /// </summary>
        STATUS_APPHELP_BLOCK = 0xC000035D,

        /// <summary>
        /// MessageId: STATUS_ALL_SIDS_FILTERED
        /// MessageText:
        /// The SID filtering operation removed all SIDs.
        /// </summary>
        STATUS_ALL_SIDS_FILTERED = 0xC000035E,

        /// <summary>
        /// MessageId: STATUS_NOT_SAFE_MODE_DRIVER
        /// MessageText:
        /// The driver was not loaded because the system is booting into safe mode.
        /// </summary>
        STATUS_NOT_SAFE_MODE_DRIVER = 0xC000035F,

        /// <summary>
        /// MessageId: STATUS_ACCESS_DISABLED_BY_POLICY_DEFAULT
        /// MessageText:
        /// Access has been restricted by your Administrator by the default software restriction policy level.
        /// </summary>
        STATUS_ACCESS_DISABLED_BY_POLICY_DEFAULT = 0xC0000361,

        /// <summary>
        /// MessageId: STATUS_ACCESS_DISABLED_BY_POLICY_PATH
        /// MessageText:
        /// Access to %1 has been restricted by your Administrator by location with policy rule %2 placed on path %3
        /// </summary>
        STATUS_ACCESS_DISABLED_BY_POLICY_PATH = 0xC0000362,

        /// <summary>
        /// MessageId: STATUS_ACCESS_DISABLED_BY_POLICY_PUBLISHER
        /// MessageText:
        /// Access to %1 has been restricted by your Administrator by software publisher policy.
        /// </summary>
        STATUS_ACCESS_DISABLED_BY_POLICY_PUBLISHER = 0xC0000363,

        /// <summary>
        /// MessageId: STATUS_ACCESS_DISABLED_BY_POLICY_OTHER
        /// MessageText:
        /// Access to %1 has been restricted by your Administrator by policy rule %2.
        /// </summary>
        STATUS_ACCESS_DISABLED_BY_POLICY_OTHER = 0xC0000364,

        /// <summary>
        /// MessageId: STATUS_FAILED_DRIVER_ENTRY
        /// MessageText:
        /// The driver was not loaded because it failed it's initialization call.
        /// </summary>
        STATUS_FAILED_DRIVER_ENTRY = 0xC0000365,

        /// <summary>
        /// MessageId: STATUS_DEVICE_ENUMERATION_ERROR
        /// MessageText:
        /// The "%hs" encountered an error while applying power or reading the device configuration.
        /// This may be caused by a failure of your hardware or by a poor connection.
        /// </summary>
        STATUS_DEVICE_ENUMERATION_ERROR = 0xC0000366,

        /// <summary>
        /// MessageId: STATUS_WAIT_FOR_OPLOCK
        /// MessageText:
        /// An operation is blocked waiting for an oplock.
        /// </summary>
        STATUS_WAIT_FOR_OPLOCK = 0x00000367,

        /// <summary>
        /// MessageId: STATUS_MOUNT_POINT_NOT_RESOLVED
        /// MessageText:
        /// The create operation failed because the name contained at least one mount point which resolves to a volume to which the specified device object is not attached.
        /// </summary>
        STATUS_MOUNT_POINT_NOT_RESOLVED = 0xC0000368,

        /// <summary>
        /// MessageId: STATUS_INVALID_DEVICE_OBJECT_PARAMETER
        /// MessageText:
        /// The device object parameter is either not a valid device object or is not attached to the volume specified by the file name.
        /// </summary>
        STATUS_INVALID_DEVICE_OBJECT_PARAMETER = 0xC0000369,

        /// <summary>
        /// MessageId: STATUS_MCA_OCCURED
        /// MessageText:
        /// A Machine Check Error has occurred. Please check the system eventlog for additional information.
        /// </summary>
        STATUS_MCA_OCCURED = 0xC000036A,

        /// <summary>
        /// MessageId: STATUS_DRIVER_BLOCKED_CRITICAL
        /// MessageText:
        /// Driver %2 has been blocked from loading.
        /// </summary>
        STATUS_DRIVER_BLOCKED_CRITICAL = 0xC000036B,

        /// <summary>
        /// MessageId: STATUS_DRIVER_BLOCKED
        /// MessageText:
        /// Driver %2 has been blocked from loading.
        /// </summary>
        STATUS_DRIVER_BLOCKED = 0xC000036C,

        /// <summary>
        /// MessageId: STATUS_DRIVER_DATABASE_ERROR
        /// MessageText:
        /// There was error [%2] processing the driver database.
        /// </summary>
        STATUS_DRIVER_DATABASE_ERROR = 0xC000036D,

        /// <summary>
        /// MessageId: STATUS_SYSTEM_HIVE_TOO_LARGE
        /// MessageText:
        /// System hive size has exceeded its limit.
        /// </summary>
        STATUS_SYSTEM_HIVE_TOO_LARGE = 0xC000036E,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMPORT_OF_NON_DLL
        /// MessageText:
        /// A dynamic link library (DLL) referenced a module that was neither a DLL nor the process's executable image.
        /// </summary>
        STATUS_INVALID_IMPORT_OF_NON_DLL = 0xC000036F,

        /// <summary>
        /// MessageId: STATUS_DS_SHUTTING_DOWN
        /// MessageText:
        /// The Directory Service is shutting down.
        /// </summary>
        STATUS_DS_SHUTTING_DOWN = 0x40000370,

        /// <summary>
        /// MessageId: STATUS_NO_SECRETS
        /// MessageText:
        /// The local account store does not contain secret material for the specified account.
        /// </summary>
        STATUS_NO_SECRETS = 0xC0000371,

        /// <summary>
        /// MessageId: STATUS_ACCESS_DISABLED_NO_SAFER_UI_BY_POLICY
        /// MessageText:
        /// Access to %1 has been restricted by your Administrator by policy rule %2.
        /// </summary>
        STATUS_ACCESS_DISABLED_NO_SAFER_UI_BY_POLICY = 0xC0000372,

        /// <summary>
        /// MessageId: STATUS_FAILED_STACK_SWITCH
        /// MessageText:
        /// The system was not able to allocate enough memory to perform a stack switch.
        /// </summary>
        STATUS_FAILED_STACK_SWITCH = 0xC0000373,

        /// <summary>
        /// MessageId: STATUS_HEAP_CORRUPTION
        /// MessageText:
        /// A heap has been corrupted.
        /// </summary>
        STATUS_HEAP_CORRUPTION = 0xC0000374,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_WRONG_PIN
        /// MessageText:
        /// An incorrect PIN was presented to the smart card
        /// </summary>
        STATUS_SMARTCARD_WRONG_PIN = 0xC0000380,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_CARD_BLOCKED
        /// MessageText:
        /// The smart card is blocked
        /// </summary>
        STATUS_SMARTCARD_CARD_BLOCKED = 0xC0000381,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_CARD_NOT_AUTHENTICATED
        /// MessageText:
        /// No PIN was presented to the smart card
        /// </summary>
        STATUS_SMARTCARD_CARD_NOT_AUTHENTICATED = 0xC0000382,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_NO_CARD
        /// MessageText:
        /// No smart card available
        /// </summary>
        STATUS_SMARTCARD_NO_CARD = 0xC0000383,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_NO_KEY_CONTAINER
        /// MessageText:
        /// The requested key container does not exist on the smart card
        /// </summary>
        STATUS_SMARTCARD_NO_KEY_CONTAINER = 0xC0000384,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_NO_CERTIFICATE
        /// MessageText:
        /// The requested certificate does not exist on the smart card
        /// </summary>
        STATUS_SMARTCARD_NO_CERTIFICATE = 0xC0000385,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_NO_KEYSET
        /// MessageText:
        /// The requested keyset does not exist
        /// </summary>
        STATUS_SMARTCARD_NO_KEYSET = 0xC0000386,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_IO_ERROR
        /// MessageText:
        /// A communication error with the smart card has been detected.
        /// </summary>
        STATUS_SMARTCARD_IO_ERROR = 0xC0000387,

        /// <summary>
        /// MessageId: STATUS_DOWNGRADE_DETECTED
        /// MessageText:
        /// The system detected a possible attempt to compromise security. Please ensure that you can contact the server that authenticated you.
        /// </summary>
        STATUS_DOWNGRADE_DETECTED = 0xC0000388,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_CERT_REVOKED
        /// MessageText:
        /// The smartcard certificate used for authentication has been revoked.
        /// Please contact your system administrator.  There may be additional information in the
        /// event log.
        /// </summary>
        STATUS_SMARTCARD_CERT_REVOKED = 0xC0000389,

        /// <summary>
        /// MessageId: STATUS_ISSUING_CA_UNTRUSTED
        /// MessageText:
        /// An untrusted certificate authority was detected While processing the
        /// smartcard certificate used for authentication.  Please contact your system
        /// administrator.
        /// </summary>
        STATUS_ISSUING_CA_UNTRUSTED = 0xC000038A,

        /// <summary>
        /// MessageId: STATUS_REVOCATION_OFFLINE_C
        /// MessageText:
        /// The revocation status of the smartcard certificate used for
        /// authentication could not be determined. Please contact your system administrator.
        /// </summary>
        STATUS_REVOCATION_OFFLINE_C = 0xC000038B,

        /// <summary>
        /// MessageId: STATUS_PKINIT_CLIENT_FAILURE
        /// MessageText:
        /// The smartcard certificate used for authentication was not trusted.  Please
        /// contact your system administrator.
        /// </summary>
        STATUS_PKINIT_CLIENT_FAILURE = 0xC000038C,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_CERT_EXPIRED
        /// MessageText:
        /// The smartcard certificate used for authentication has expired.  Please
        /// contact your system administrator.
        /// </summary>
        STATUS_SMARTCARD_CERT_EXPIRED = 0xC000038D,

        /// <summary>
        /// MessageId: STATUS_DRIVER_FAILED_PRIOR_UNLOAD
        /// MessageText:
        /// The driver could not be loaded because a previous version of the driver is still in memory.
        /// </summary>
        STATUS_DRIVER_FAILED_PRIOR_UNLOAD = 0xC000038E,

        /// <summary>
        /// MessageId: STATUS_SMARTCARD_SILENT_CONTEXT
        /// MessageText:
        /// The smartcard provider could not perform the action since the context was acquired as silent.
        /// </summary>
        STATUS_SMARTCARD_SILENT_CONTEXT = 0xC000038F,

        /// <summary>
        /// MessageId: STATUS_PER_USER_TRUST_QUOTA_EXCEEDED
        /// MessageText:
        /// The current user's delegated trust creation quota has been exceeded.
        /// </summary>
        STATUS_PER_USER_TRUST_QUOTA_EXCEEDED = 0xC0000401,

        /// <summary>
        /// MessageId: STATUS_ALL_USER_TRUST_QUOTA_EXCEEDED
        /// MessageText:
        /// The total delegated trust creation quota has been exceeded.
        /// </summary>
        STATUS_ALL_USER_TRUST_QUOTA_EXCEEDED = 0xC0000402,

        /// <summary>
        /// MessageId: STATUS_USER_DELETE_TRUST_QUOTA_EXCEEDED
        /// MessageText:
        /// The current user's delegated trust deletion quota has been exceeded.
        /// </summary>
        STATUS_USER_DELETE_TRUST_QUOTA_EXCEEDED = 0xC0000403,

        /// <summary>
        /// MessageId: STATUS_DS_NAME_NOT_UNIQUE
        /// MessageText:
        /// The requested name already exists as a unique identifier.
        /// </summary>
        STATUS_DS_NAME_NOT_UNIQUE = 0xC0000404,

        /// <summary>
        /// MessageId: STATUS_DS_DUPLICATE_ID_FOUND
        /// MessageText:
        /// The requested object has a non-unique identifier and cannot be retrieved.
        /// </summary>
        STATUS_DS_DUPLICATE_ID_FOUND = 0xC0000405,

        /// <summary>
        /// MessageId: STATUS_DS_GROUP_CONVERSION_ERROR
        /// MessageText:
        /// The group cannot be converted due to attribute restrictions on the requested group type.
        /// </summary>
        STATUS_DS_GROUP_CONVERSION_ERROR = 0xC0000406,

        /// <summary>
        /// MessageId: STATUS_VOLSNAP_PREPARE_HIBERNATE
        /// MessageText:
        /// {Volume Shadow Copy Service}
        /// Please wait while the Volume Shadow Copy Service prepares volume %hs for hibernation.
        /// </summary>
        STATUS_VOLSNAP_PREPARE_HIBERNATE = 0xC0000407,

        /// <summary>
        /// MessageId: STATUS_USER2USER_REQUIRED
        /// MessageText:
        /// Kerberos sub-protocol User2User is required.
        /// </summary>
        STATUS_USER2USER_REQUIRED = 0xC0000408,

        /// <summary>
        /// MessageId: STATUS_STACK_BUFFER_OVERRUN
        /// MessageText:
        /// The system detected an overrun of a stack-based buffer in this application. This
        /// overrun could potentially allow a malicious user to gain control of this application.
        /// </summary>
        STATUS_STACK_BUFFER_OVERRUN = 0xC0000409,

        /// <summary>
        /// MessageId: STATUS_NO_S4U_PROT_SUPPORT
        /// MessageText:
        /// The Kerberos subsystem encountered an error. A service for user protocol request was made
        /// against a domain controller which does not support service for user.
        /// </summary>
        STATUS_NO_S4U_PROT_SUPPORT = 0xC000040A,

        /// <summary>
        /// MessageId: STATUS_CROSSREALM_DELEGATION_FAILURE
        /// MessageText:
        /// An attempt was made by this server to make a Kerberos constrained delegation request for a target
        /// outside of the server's realm. This is not supported, and indicates a misconfiguration on this
        /// server's allowed to delegate to list.  Please contact your administrator.
        /// </summary>
        STATUS_CROSSREALM_DELEGATION_FAILURE = 0xC000040B,

        /// <summary>
        /// MessageId: STATUS_REVOCATION_OFFLINE_KDC
        /// MessageText:
        /// The revocation status of the domain controller certificate used for smartcard
        /// authentication could not be determined.  There is additional information in the system event
        /// log. Please contact your system administrator.
        /// </summary>
        STATUS_REVOCATION_OFFLINE_KDC = 0xC000040C,

        /// <summary>
        /// MessageId: STATUS_ISSUING_CA_UNTRUSTED_KDC
        /// MessageText:
        /// An untrusted certificate authority was detected while processing the
        /// domain controller certificate used for authentication.  There is additional information in
        /// the system event log.  Please contact your system administrator.
        /// </summary>
        STATUS_ISSUING_CA_UNTRUSTED_KDC = 0xC000040D,

        /// <summary>
        /// MessageId: STATUS_KDC_CERT_EXPIRED
        /// MessageText:
        /// The domain controller certificate used for smartcard logon has expired.
        /// Please contact your system administrator with the contents of your system event log.
        /// </summary>
        STATUS_KDC_CERT_EXPIRED = 0xC000040E,

        /// <summary>
        /// MessageId: STATUS_KDC_CERT_REVOKED
        /// MessageText:
        /// The domain controller certificate used for smartcard logon has been revoked.
        /// Please contact your system administrator with the contents of your system event log.
        /// </summary>
        STATUS_KDC_CERT_REVOKED = 0xC000040F,

        /// <summary>
        /// MessageId: STATUS_PARAMETER_QUOTA_EXCEEDED
        /// MessageText:
        /// Data present in one of the parameters is more than the function can operate on.
        /// </summary>
        STATUS_PARAMETER_QUOTA_EXCEEDED = 0xC0000410,

        /// <summary>
        /// MessageId: STATUS_HIBERNATION_FAILURE
        /// MessageText:
        /// The system has failed to hibernate (The error code is %hs).  Hibernation will be disabled until the system is restarted.
        /// </summary>
        STATUS_HIBERNATION_FAILURE = 0xC0000411,

        /// <summary>
        /// MessageId: STATUS_DELAY_LOAD_FAILED
        /// MessageText:
        /// An attempt to delay-load a .dll or get a function address in a delay-loaded .dll failed.
        /// </summary>
        STATUS_DELAY_LOAD_FAILED = 0xC0000412,

        /// <summary>
        /// MessageId: STATUS_AUTHENTICATION_FIREWALL_FAILED
        /// MessageText:
        /// Logon Failure: The machine you are logging onto is protected by an authentication firewall. The specified account is not allowed to authenticate to the machine.
        /// </summary>
        STATUS_AUTHENTICATION_FIREWALL_FAILED = 0xC0000413,

        /// <summary>
        /// MessageId: STATUS_VDM_DISALLOWED
        /// MessageText:
        /// %hs is a 16-bit application. You do not have permissions to execute 16-bit applications. Check your permissions with your system administrator.
        /// </summary>
        STATUS_VDM_DISALLOWED = 0xC0000414,

        /// <summary>
        /// MessageId: STATUS_HUNG_DISPLAY_DRIVER_THREAD
        /// MessageText:
        /// {Display Driver Stopped Responding}
        /// The %hs display driver has stopped working normally.  Save your work and reboot the system to restore full display functionality.
        /// The next time you reboot the machine a dialog will be displayed giving you a chance to report this failure to Microsoft.
        /// </summary>
        STATUS_HUNG_DISPLAY_DRIVER_THREAD = 0xC0000415,

        /// <summary>
        /// MessageId: STATUS_INSUFFICIENT_RESOURCE_FOR_SPECIFIED_SHARED_SECTION_SIZE
        /// MessageText:
        /// The Desktop heap encountered an error while allocating session memory.  There is more information in the system event log.
        /// </summary>
        STATUS_INSUFFICIENT_RESOURCE_FOR_SPECIFIED_SHARED_SECTION_SIZE = 0xC0000416,

        /// <summary>
        /// MessageId: STATUS_INVALID_CRUNTIME_PARAMETER
        /// MessageText:
        /// An invalid parameter was passed to a C runtime function.
        /// </summary>
        STATUS_INVALID_CRUNTIME_PARAMETER = 0xC0000417,

        /// <summary>
        /// MessageId: STATUS_NTLM_BLOCKED
        /// MessageText:
        /// The authentication failed since NTLM was blocked.
        /// </summary>
        STATUS_NTLM_BLOCKED = 0xC0000418,

        /// <summary>
        /// An assertion failure has occurred.
        /// </summary>
        STATUS_ASSERTION_FAILURE = 0xC0000420,

        /// <summary>
        /// MessageId: STATUS_VERIFIER_STOP
        /// MessageText:
        /// Application verifier has found an error in the current process.
        /// </summary>
        STATUS_VERIFIER_STOP = 0xC0000421,

        /// <summary>
        /// An exception has occurred in a user mode callback and the kernel callback frame should be removed.
        /// </summary>
        STATUS_CALLBACK_POP_STACK = 0xC0000423,

        /// <summary>
        /// MessageId: STATUS_INCOMPATIBLE_DRIVER_BLOCKED
        /// MessageText:
        /// %2 has been blocked from loading due to incompatibility with this system. Please contact your software
        /// vendor for a compatible version of the driver.
        /// </summary>
        STATUS_INCOMPATIBLE_DRIVER_BLOCKED = 0xC0000424,

        /// <summary>
        /// MessageId: STATUS_HIVE_UNLOADED
        /// MessageText:
        /// Illegal operation attempted on a registry key which has already been unloaded.
        /// </summary>
        STATUS_HIVE_UNLOADED = 0xC0000425,

        /// <summary>
        /// MessageId: STATUS_COMPRESSION_DISABLED
        /// MessageText:
        /// Compression is disabled for this volume.
        /// </summary>
        STATUS_COMPRESSION_DISABLED = 0xC0000426,

        /// <summary>
        /// MessageId: STATUS_FILE_SYSTEM_LIMITATION
        /// MessageText:
        /// The requested operation could not be completed due to a file system limitation
        /// </summary>
        STATUS_FILE_SYSTEM_LIMITATION = 0xC0000427,

        /// <summary>
        /// MessageId: STATUS_INVALID_IMAGE_HASH
        /// MessageText:
        /// Windows cannot verify the digital signature for this file. A recent hardware or software change might have installed a file that is signed incorrectly or damaged, or that might be malicious software from an unknown source.
        /// </summary>
        STATUS_INVALID_IMAGE_HASH = 0xC0000428,

        /// <summary>
        /// MessageId: STATUS_NOT_CAPABLE
        /// MessageText:
        /// The implementation is not capable of performing the request.
        /// </summary>
        STATUS_NOT_CAPABLE = 0xC0000429,

        /// <summary>
        /// MessageId: STATUS_REQUEST_OUT_OF_SEQUENCE
        /// MessageText:
        /// The requested operation is out of order with respect to other operations.
        /// </summary>
        STATUS_REQUEST_OUT_OF_SEQUENCE = 0xC000042A,

        /// <summary>
        /// MessageId: STATUS_IMPLEMENTATION_LIMIT
        /// MessageText:
        /// An operation attempted to exceed an implementation-defined limit.
        /// </summary>
        STATUS_IMPLEMENTATION_LIMIT = 0xC000042B,

        /// <summary>
        /// MessageId: STATUS_ELEVATION_REQUIRED
        /// MessageText:
        /// The requested operation requires elevation.
        /// </summary>
        STATUS_ELEVATION_REQUIRED = 0xC000042C,

        /// <summary>
        /// MessageId: STATUS_BEYOND_VDL
        /// MessageText:
        /// The operation was attempted beyond the valid data length of the file.
        /// </summary>
        STATUS_BEYOND_VDL = 0xC0000432,

        /// <summary>
        /// MessageId: STATUS_ENCOUNTERED_WRITE_IN_PROGRESS
        /// MessageText:
        /// The attempted write operation encountered a write already in progress for some portion of the range.
        /// </summary>
        STATUS_ENCOUNTERED_WRITE_IN_PROGRESS = 0xC0000433,

        /// <summary>
        /// MessageId: STATUS_PTE_CHANGED
        /// MessageText:
        /// The page fault mappings changed in the middle of processing a fault so the operation must be retried.
        /// </summary>
        STATUS_PTE_CHANGED = 0xC0000434,

        /// <summary>
        /// MessageId: STATUS_PURGE_FAILED
        /// MessageText:
        /// The attempt to purge this file from memory failed to purge some or all the data from memory.
        /// </summary>
        STATUS_PURGE_FAILED = 0xC0000435,

        /// <summary>
        /// MessageId: STATUS_CRED_REQUIRES_CONFIRMATION
        /// MessageText:
        /// The requested credential requires confirmation.
        /// </summary>
        STATUS_CRED_REQUIRES_CONFIRMATION = 0xC0000440,

        /// <summary>
        /// MessageId: STATUS_CS_ENCRYPTION_INVALID_SERVER_RESPONSE
        /// MessageText:
        /// The remote server sent an invalid response for a file being opened with Client Side Encryption.
        /// </summary>
        STATUS_CS_ENCRYPTION_INVALID_SERVER_RESPONSE = 0xC0000441,

        /// <summary>
        /// MessageId: STATUS_CS_ENCRYPTION_UNSUPPORTED_SERVER
        /// MessageText:
        /// Client Side Encryption is not supported by the remote server even though it claims to support it.
        /// </summary>
        STATUS_CS_ENCRYPTION_UNSUPPORTED_SERVER = 0xC0000442,

        /// <summary>
        /// MessageId: STATUS_CS_ENCRYPTION_EXISTING_ENCRYPTED_FILE
        /// MessageText:
        /// File is encrypted and should be opened in Client Side Encryption mode.
        /// </summary>
        STATUS_CS_ENCRYPTION_EXISTING_ENCRYPTED_FILE = 0xC0000443,

        /// <summary>
        /// MessageId: STATUS_CS_ENCRYPTION_NEW_ENCRYPTED_FILE
        /// MessageText:
        /// A new encrypted file is being created and a $EFS needs to be provided.
        /// </summary>
        STATUS_CS_ENCRYPTION_NEW_ENCRYPTED_FILE = 0xC0000444,

        /// <summary>
        /// MessageId: STATUS_CS_ENCRYPTION_FILE_NOT_CSE
        /// MessageText:
        /// The SMB client requested a CSE FSCTL on a non-CSE file.
        /// </summary>
        STATUS_CS_ENCRYPTION_FILE_NOT_CSE = 0xC0000445,

        /// <summary>
        /// MessageId: STATUS_INVALID_LABEL
        /// MessageText:
        /// Indicates a particular Security ID may not be assigned as the label of an object.
        /// </summary>
        STATUS_INVALID_LABEL = 0xC0000446,

        /// <summary>
        /// MessageId: STATUS_DRIVER_PROCESS_TERMINATED
        /// MessageText:
        /// The process hosting the driver for this device has terminated.
        /// </summary>
        STATUS_DRIVER_PROCESS_TERMINATED = 0xC0000450,

        /// <summary>
        /// MessageId: STATUS_AMBIGUOUS_SYSTEM_DEVICE
        /// MessageText:
        /// The requested system device cannot be identified due to multiple indistinguishable devices potentially matching the identification criteria.
        /// </summary>
        STATUS_AMBIGUOUS_SYSTEM_DEVICE = 0xC0000451,

        /// <summary>
        /// MessageId: STATUS_SYSTEM_DEVICE_NOT_FOUND
        /// MessageText:
        /// The requested system device cannot be found.
        /// </summary>
        STATUS_SYSTEM_DEVICE_NOT_FOUND = 0xC0000452,

        /// <summary>
        /// MessageId: STATUS_RESTART_BOOT_APPLICATION
        /// MessageText:
        /// This boot application must be restarted.
        /// </summary>
        STATUS_RESTART_BOOT_APPLICATION = 0xC0000453,

        /// <summary>
        /// MessageId: STATUS_INVALID_TASK_NAME
        /// MessageText:
        /// The specified task name is invalid.
        /// </summary>
        STATUS_INVALID_TASK_NAME = 0xC0000500,

        /// <summary>
        /// MessageId: STATUS_INVALID_TASK_INDEX
        /// MessageText:
        /// The specified task index is invalid.
        /// </summary>
        STATUS_INVALID_TASK_INDEX = 0xC0000501,

        /// <summary>
        /// MessageId: STATUS_THREAD_ALREADY_IN_TASK
        /// MessageText:
        /// The specified thread is already joining a task.
        /// </summary>
        STATUS_THREAD_ALREADY_IN_TASK = 0xC0000502,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_BYPASS
        /// MessageText:
        /// A callback has requested to bypass native code.
        /// </summary>
        STATUS_CALLBACK_BYPASS = 0xC0000503,

        /// <summary>
        /// MessageId: STATUS_PORT_CLOSED
        /// MessageText:
        /// The ALPC port is closed.
        /// </summary>
        STATUS_PORT_CLOSED = 0xC0000700,

        /// <summary>
        /// MessageId: STATUS_MESSAGE_LOST
        /// MessageText:
        /// The ALPC message requested is no longer available.
        /// </summary>
        STATUS_MESSAGE_LOST = 0xC0000701,

        /// <summary>
        /// MessageId: STATUS_INVALID_MESSAGE
        /// MessageText:
        /// The ALPC message supplied is invalid.
        /// </summary>
        STATUS_INVALID_MESSAGE = 0xC0000702,

        /// <summary>
        /// MessageId: STATUS_REQUEST_CANCELED
        /// MessageText:
        /// The ALPC message has been canceled.
        /// </summary>
        STATUS_REQUEST_CANCELED = 0xC0000703,

        /// <summary>
        /// MessageId: STATUS_RECURSIVE_DISPATCH
        /// MessageText:
        /// Invalid recursive dispatch attempt.
        /// </summary>
        STATUS_RECURSIVE_DISPATCH = 0xC0000704,

        /// <summary>
        /// MessageId: STATUS_LPC_RECEIVE_BUFFER_EXPECTED
        /// MessageText:
        /// No receive buffer has been supplied in a synchronous request.
        /// </summary>
        STATUS_LPC_RECEIVE_BUFFER_EXPECTED = 0xC0000705,

        /// <summary>
        /// MessageId: STATUS_LPC_INVALID_CONNECTION_USAGE
        /// MessageText:
        /// The connection port is used in an invalid context.
        /// </summary>
        STATUS_LPC_INVALID_CONNECTION_USAGE = 0xC0000706,

        /// <summary>
        /// MessageId: STATUS_LPC_REQUESTS_NOT_ALLOWED
        /// MessageText:
        /// The ALPC port does not accept new request messages.
        /// </summary>
        STATUS_LPC_REQUESTS_NOT_ALLOWED = 0xC0000707,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_IN_USE
        /// MessageText:
        /// The resource requested is already in use.
        /// </summary>
        STATUS_RESOURCE_IN_USE = 0xC0000708,

        /// <summary>
        /// MessageId: STATUS_HARDWARE_MEMORY_ERROR
        /// MessageText:
        /// The hardware has reported an uncorrectable memory error.
        /// </summary>
        STATUS_HARDWARE_MEMORY_ERROR = 0xC0000709,

        /// <summary>
        /// MessageId: STATUS_THREADPOOL_HANDLE_EXCEPTION
        /// MessageText:
        /// Status 0x%08x was returned, waiting on handle 0x%x for wait 0x%p, in waiter 0x%p.
        /// </summary>
        STATUS_THREADPOOL_HANDLE_EXCEPTION = 0xC000070A,

        /// <summary>
        /// MessageId: STATUS_THREADPOOL_SET_EVENT_ON_COMPLETION_FAILED
        /// MessageText:
        /// After a callback to 0x%p(0x%p), a completion call to SetEvent(0x%p) failed with status 0x%08x.
        /// </summary>
        STATUS_THREADPOOL_SET_EVENT_ON_COMPLETION_FAILED = 0xC000070B,

        /// <summary>
        /// MessageId: STATUS_THREADPOOL_RELEASE_SEMAPHORE_ON_COMPLETION_FAILED
        /// MessageText:
        /// After a callback to 0x%p(0x%p), a completion call to ReleaseSemaphore(0x%p, %d) failed with status 0x%08x.
        /// </summary>
        STATUS_THREADPOOL_RELEASE_SEMAPHORE_ON_COMPLETION_FAILED = 0xC000070C,

        /// <summary>
        /// MessageId: STATUS_THREADPOOL_RELEASE_MUTEX_ON_COMPLETION_FAILED
        /// MessageText:
        /// After a callback to 0x%p(0x%p), a completion call to ReleaseMutex(%p) failed with status 0x%08x.
        /// </summary>
        STATUS_THREADPOOL_RELEASE_MUTEX_ON_COMPLETION_FAILED = 0xC000070D,

        /// <summary>
        /// MessageId: STATUS_THREADPOOL_FREE_LIBRARY_ON_COMPLETION_FAILED
        /// MessageText:
        /// After a callback to 0x%p(0x%p), an completion call to FreeLibrary(%p) failed with status 0x%08x.
        /// </summary>
        STATUS_THREADPOOL_FREE_LIBRARY_ON_COMPLETION_FAILED = 0xC000070E,

        /// <summary>
        /// MessageId: STATUS_THREADPOOL_RELEASED_DURING_OPERATION
        /// MessageText:
        /// The threadpool 0x%p was released while a thread was posting a callback to 0x%p(0x%p) to it.
        /// </summary>
        STATUS_THREADPOOL_RELEASED_DURING_OPERATION = 0xC000070F,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_RETURNED_WHILE_IMPERSONATING
        /// MessageText:
        /// A threadpool worker thread is impersonating a client, after a callback to 0x%p(0x%p).
        /// This is unexpected, indicating that the callback is missing a call to revert the impersonation.
        /// </summary>
        STATUS_CALLBACK_RETURNED_WHILE_IMPERSONATING = 0xC0000710,

        /// <summary>
        /// MessageId: STATUS_APC_RETURNED_WHILE_IMPERSONATING
        /// MessageText:
        /// A threadpool worker thread is impersonating a client, after executing an APC.
        /// This is unexpected, indicating that the APC is missing a call to revert the impersonation.
        /// </summary>
        STATUS_APC_RETURNED_WHILE_IMPERSONATING = 0xC0000711,

        /// <summary>
        /// MessageId: STATUS_PROCESS_IS_PROTECTED
        /// MessageText:
        /// Either the target process, or the target thread's containing process, is a protected process.
        /// </summary>
        STATUS_PROCESS_IS_PROTECTED = 0xC0000712,

        /// <summary>
        /// MessageId: STATUS_MCA_EXCEPTION
        /// MessageText:
        /// A Thread is getting dispatched with MCA EXCEPTION because of MCA.
        /// </summary>
        STATUS_MCA_EXCEPTION = 0xC0000713,

        /// <summary>
        /// MessageId: STATUS_CERTIFICATE_MAPPING_NOT_UNIQUE
        /// MessageText:
        /// The client certificate account mapping is not unique.
        /// </summary>
        STATUS_CERTIFICATE_MAPPING_NOT_UNIQUE = 0xC0000714,

        /// <summary>
        /// MessageId: STATUS_SYMLINK_CLASS_DISABLED
        /// MessageText:
        /// The symbolic link cannot be followed because its type is disabled.
        /// </summary>
        STATUS_SYMLINK_CLASS_DISABLED = 0xC0000715,

        /// <summary>
        /// MessageId: STATUS_INVALID_IDN_NORMALIZATION
        /// MessageText:
        /// Indicates that the specified string is not valid for IDN normalization.
        /// </summary>
        STATUS_INVALID_IDN_NORMALIZATION = 0xC0000716,

        /// <summary>
        /// MessageId: STATUS_NO_UNICODE_TRANSLATION
        /// MessageText:
        /// No mapping for the Unicode character exists in the target multi-byte code page.
        /// </summary>
        STATUS_NO_UNICODE_TRANSLATION = 0xC0000717,

        /// <summary>
        /// MessageId: STATUS_ALREADY_REGISTERED
        /// MessageText:
        /// The provided callback is already registered.
        /// </summary>
        STATUS_ALREADY_REGISTERED = 0xC0000718,

        /// <summary>
        /// MessageId: STATUS_CONTEXT_MISMATCH
        /// MessageText:
        /// The provided context did not match the target.
        /// </summary>
        STATUS_CONTEXT_MISMATCH = 0xC0000719,

        /// <summary>
        /// MessageId: STATUS_PORT_ALREADY_HAS_COMPLETION_LIST
        /// MessageText:
        /// The specified port already has a completion list.
        /// </summary>
        STATUS_PORT_ALREADY_HAS_COMPLETION_LIST = 0xC000071A,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_RETURNED_THREAD_PRIORITY
        /// MessageText:
        /// A threadpool worker thread entered a callback at thread base priority 0x%x and exited at priority 0x%x.
        /// This is unexpected, indicating that the callback missed restoring the priority.
        /// </summary>
        STATUS_CALLBACK_RETURNED_THREAD_PRIORITY = 0xC000071B,

        /// <summary>
        /// MessageId: STATUS_INVALID_THREAD
        /// MessageText:
        /// An invalid thread, handle %p, is specified for this operation.  Possibly, a threadpool worker thread was specified.
        /// </summary>
        STATUS_INVALID_THREAD = 0xC000071C,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_RETURNED_TRANSACTION
        /// MessageText:
        /// A threadpool worker thread enter a callback, which left transaction state.
        /// This is unexpected, indicating that the callback missed clearing the transaction.
        /// </summary>
        STATUS_CALLBACK_RETURNED_TRANSACTION = 0xC000071D,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_RETURNED_LDR_LOCK
        /// MessageText:
        /// A threadpool worker thread enter a callback, which left the loader lock held.
        /// This is unexpected, indicating that the callback missed releasing the lock.
        /// </summary>
        STATUS_CALLBACK_RETURNED_LDR_LOCK = 0xC000071E,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_RETURNED_LANG
        /// MessageText:
        /// A threadpool worker thread enter a callback, which left with preferred languages set.
        /// This is unexpected, indicating that the callback missed clearing them.
        /// </summary>
        STATUS_CALLBACK_RETURNED_LANG = 0xC000071F,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_RETURNED_PRI_BACK
        /// MessageText:
        /// A threadpool worker thread enter a callback, which left with background priorities set.
        /// This is unexpected, indicating that the callback missed restoring the original priorities.
        /// </summary>
        STATUS_CALLBACK_RETURNED_PRI_BACK = 0xC0000720,

        /// <summary>
        /// MessageId: STATUS_CALLBACK_RETURNED_THREAD_AFFINITY
        /// MessageText:
        /// A threadpool worker thread enter a callback at thread affinity %p and exited at affinity %p.
        /// This is unexpected, indicating that the callback missed restoring the priority.
        /// </summary>
        STATUS_CALLBACK_RETURNED_THREAD_AFFINITY = 0xC0000721,

        /// <summary>
        /// MessageId: STATUS_DISK_REPAIR_DISABLED
        /// MessageText:
        /// The attempted operation required self healing to be enabled.
        /// </summary>
        STATUS_DISK_REPAIR_DISABLED = 0xC0000800,

        /// <summary>
        /// MessageId: STATUS_DS_DOMAIN_RENAME_IN_PROGRESS
        /// MessageText:
        /// The Directory Service cannot perform the requested operation because a domain rename operation is in progress.
        /// </summary>
        STATUS_DS_DOMAIN_RENAME_IN_PROGRESS = 0xC0000801,

        /// <summary>
        /// MessageId: STATUS_DISK_QUOTA_EXCEEDED
        /// MessageText:
        /// The requested file operation failed because the storage quota was exceeded.
        /// To free up disk space, move files to a different location or delete unnecessary files. For more information, contact your system administrator.
        /// </summary>
        STATUS_DISK_QUOTA_EXCEEDED = 0xC0000802,

        /// <summary>
        /// MessageId: STATUS_DATA_LOST_REPAIR
        /// MessageText:
        /// Windows discovered a corruption in the file %hs. This file has now been repaired.
        /// Please check if any data in the file was lost because of the corruption.
        /// </summary>
        STATUS_DATA_LOST_REPAIR = 0x80000803,

        /// <summary>
        /// MessageId: STATUS_CONTENT_BLOCKED
        /// MessageText:
        /// The requested file operation failed because the storage policy blocks that type of file. For more information, contact your system administrator.
        /// </summary>
        STATUS_CONTENT_BLOCKED = 0xC0000804,

        /// <summary>
        /// MessageId: STATUS_BAD_CLUSTERS
        /// MessageText:
        /// The operation could not be completed due to bad clusters on disk.
        /// </summary>
        STATUS_BAD_CLUSTERS = 0xC0000805,

        /// <summary>
        /// MessageId: STATUS_VOLUME_DIRTY
        /// MessageText:
        /// The operation could not be completed because the volume is dirty.  Please run chkdsk and try again.
        /// </summary>
        STATUS_VOLUME_DIRTY = 0xC0000806,

        /// <summary>
        /// MessageId: STATUS_FILE_CHECKED_OUT
        /// MessageText:
        /// This file is checked out or locked for editing by another user.
        /// </summary>
        STATUS_FILE_CHECKED_OUT = 0xC0000901,

        /// <summary>
        /// MessageId: STATUS_CHECKOUT_REQUIRED
        /// MessageText:
        /// The file must be checked out before saving changes.
        /// </summary>
        STATUS_CHECKOUT_REQUIRED = 0xC0000902,

        /// <summary>
        /// MessageId: STATUS_BAD_FILE_TYPE
        /// MessageText:
        /// The file type being saved or retrieved has been blocked.
        /// </summary>
        STATUS_BAD_FILE_TYPE = 0xC0000903,

        /// <summary>
        /// MessageId: STATUS_FILE_TOO_LARGE
        /// MessageText:
        /// The file size exceeds the limit allowed and cannot be saved.
        /// </summary>
        STATUS_FILE_TOO_LARGE = 0xC0000904,

        /// <summary>
        /// MessageId: STATUS_FORMS_AUTH_REQUIRED
        /// MessageText:
        /// Access Denied.  Before opening files in this location, you must first browse to the web site and select the option to login automatically.
        /// </summary>
        STATUS_FORMS_AUTH_REQUIRED = 0xC0000905,

        /// <summary>
        /// MessageId: STATUS_VIRUS_INFECTED
        /// MessageText:
        /// Operation did not complete successfully because the file contains a virus.
        /// </summary>
        STATUS_VIRUS_INFECTED = 0xC0000906,

        /// <summary>
        /// MessageId: STATUS_VIRUS_DELETED
        /// MessageText:
        /// This file contains a virus and cannot be opened. Due to the nature of this virus, the file has been removed from this location.
        /// </summary>
        STATUS_VIRUS_DELETED = 0xC0000907,

        /// <summary>
        /// MessageId: STATUS_BAD_MCFG_TABLE
        /// MessageText:
        /// The resources required for this device conflict with the MCFG table.
        /// </summary>
        STATUS_BAD_MCFG_TABLE = 0xC0000908,

        /// <summary>
        /// MessageId: STATUS_WOW_ASSERTION
        /// MessageText:
        /// WOW Assertion Error.
        /// </summary>
        STATUS_WOW_ASSERTION = 0xC0009898,

        /// <summary>
        /// MessageId: STATUS_INVALID_SIGNATURE
        /// MessageText:
        /// The cryptographic signature is invalid.
        /// </summary>
        STATUS_INVALID_SIGNATURE = 0xC000A000,

        /// <summary>
        /// MessageId: STATUS_HMAC_NOT_SUPPORTED
        /// MessageText:
        /// The cryptographic provider does not support HMAC.
        /// </summary>
        STATUS_HMAC_NOT_SUPPORTED = 0xC000A001,

        /// <summary>
        /// MessageId: STATUS_IPSEC_QUEUE_OVERFLOW
        /// MessageText:
        /// The IPSEC queue overflowed.
        /// </summary>
        STATUS_IPSEC_QUEUE_OVERFLOW = 0xC000A010,

        /// <summary>
        /// MessageId: STATUS_ND_QUEUE_OVERFLOW
        /// MessageText:
        /// The neighbor discovery queue overflowed.
        /// </summary>
        STATUS_ND_QUEUE_OVERFLOW = 0xC000A011,

        /// <summary>
        /// MessageId: STATUS_HOPLIMIT_EXCEEDED
        /// MessageText:
        /// An ICMP hop limit exceeded error was received.
        /// </summary>
        STATUS_HOPLIMIT_EXCEEDED = 0xC000A012,

        /// <summary>
        /// MessageId: STATUS_PROTOCOL_NOT_SUPPORTED
        /// MessageText:
        /// The protocol is not installed on the local machine.
        /// </summary>
        STATUS_PROTOCOL_NOT_SUPPORTED = 0xC000A013,

        /// <summary>
        /// MessageId: STATUS_LOST_WRITEBEHIND_DATA_NETWORK_DISCONNECTED
        /// MessageText:
        /// {Delayed Write Failed}
        /// Windows was unable to save all the data for the file %hs; the data has been lost.
        /// This error may be caused by network connectivity issues. Please try to save this file elsewhere.
        /// </summary>
        STATUS_LOST_WRITEBEHIND_DATA_NETWORK_DISCONNECTED = 0xC000A080,

        /// <summary>
        /// MessageId: STATUS_LOST_WRITEBEHIND_DATA_NETWORK_SERVER_ERROR
        /// MessageText:
        /// {Delayed Write Failed}
        /// Windows was unable to save all the data for the file %hs; the data has been lost.
        /// This error was returned by the server on which the file exists. Please try to save this file elsewhere.
        /// </summary>
        STATUS_LOST_WRITEBEHIND_DATA_NETWORK_SERVER_ERROR = 0xC000A081,

        /// <summary>
        /// MessageId: STATUS_LOST_WRITEBEHIND_DATA_LOCAL_DISK_ERROR
        /// MessageText:
        /// {Delayed Write Failed}
        /// Windows was unable to save all the data for the file %hs; the data has been lost.
        /// This error may be caused if the device has been removed or the media is write-protected.
        /// </summary>
        STATUS_LOST_WRITEBEHIND_DATA_LOCAL_DISK_ERROR = 0xC000A082,

        /// <summary>
        /// MessageId: STATUS_XML_PARSE_ERROR
        /// MessageText:
        /// Windows was unable to parse the requested XML data.
        /// </summary>
        STATUS_XML_PARSE_ERROR = 0xC000A083,

        /// <summary>
        /// MessageId: STATUS_XMLDSIG_ERROR
        /// MessageText:
        /// An error was encountered while processing an XML digital signature.
        /// </summary>
        STATUS_XMLDSIG_ERROR = 0xC000A084,

        /// <summary>
        /// MessageId: STATUS_WRONG_COMPARTMENT
        /// MessageText:
        /// Indicates that the caller made the connection request in the wrong routing compartment.
        /// </summary>
        STATUS_WRONG_COMPARTMENT = 0xC000A085,

        /// <summary>
        /// MessageId: STATUS_AUTHIP_FAILURE
        /// MessageText:
        /// Indicates that there was an AuthIP failure when attempting to connect to the remote host.
        /// </summary>
        STATUS_AUTHIP_FAILURE = 0xC000A086,

        /// <summary>
        ///  Debugger error values
        /// MessageId: DBG_NO_STATE_CHANGE
        /// MessageText:
        /// Debugger did not perform a state change.
        /// </summary>
        DBG_NO_STATE_CHANGE = 0xC0010001,

        /// <summary>
        /// MessageId: DBG_APP_NOT_IDLE
        /// MessageText:
        /// Debugger has found the application is not idle.
        /// </summary>
        DBG_APP_NOT_IDLE = 0xC0010002,

        /// <summary>
        ///  RPC error values
        /// MessageId: RPC_NT_INVALID_STRING_BINDING
        /// MessageText:
        /// The string binding is invalid.
        /// </summary>
        RPC_NT_INVALID_STRING_BINDING = 0xC0020001,

        /// <summary>
        /// MessageId: RPC_NT_WRONG_KIND_OF_BINDING
        /// MessageText:
        /// The binding handle is not the correct type.
        /// </summary>
        RPC_NT_WRONG_KIND_OF_BINDING = 0xC0020002,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_BINDING
        /// MessageText:
        /// The binding handle is invalid.
        /// </summary>
        RPC_NT_INVALID_BINDING = 0xC0020003,

        /// <summary>
        /// MessageId: RPC_NT_PROTSEQ_NOT_SUPPORTED
        /// MessageText:
        /// The RPC protocol sequence is not supported.
        /// </summary>
        RPC_NT_PROTSEQ_NOT_SUPPORTED = 0xC0020004,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_RPC_PROTSEQ
        /// MessageText:
        /// The RPC protocol sequence is invalid.
        /// </summary>
        RPC_NT_INVALID_RPC_PROTSEQ = 0xC0020005,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_STRING_UUID
        /// MessageText:
        /// The string UUID is invalid.
        /// </summary>
        RPC_NT_INVALID_STRING_UUID = 0xC0020006,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_ENDPOINT_FORMAT
        /// MessageText:
        /// The endpoint format is invalid.
        /// </summary>
        RPC_NT_INVALID_ENDPOINT_FORMAT = 0xC0020007,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_NET_ADDR
        /// MessageText:
        /// The network address is invalid.
        /// </summary>
        RPC_NT_INVALID_NET_ADDR = 0xC0020008,

        /// <summary>
        /// MessageId: RPC_NT_NO_ENDPOINT_FOUND
        /// MessageText:
        /// No endpoint was found.
        /// </summary>
        RPC_NT_NO_ENDPOINT_FOUND = 0xC0020009,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_TIMEOUT
        /// MessageText:
        /// The timeout value is invalid.
        /// </summary>
        RPC_NT_INVALID_TIMEOUT = 0xC002000A,

        /// <summary>
        /// MessageId: RPC_NT_OBJECT_NOT_FOUND
        /// MessageText:
        /// The object UUID was not found.
        /// </summary>
        RPC_NT_OBJECT_NOT_FOUND = 0xC002000B,

        /// <summary>
        /// MessageId: RPC_NT_ALREADY_REGISTERED
        /// MessageText:
        /// The object UUID has already been registered.
        /// </summary>
        RPC_NT_ALREADY_REGISTERED = 0xC002000C,

        /// <summary>
        /// MessageId: RPC_NT_TYPE_ALREADY_REGISTERED
        /// MessageText:
        /// The type UUID has already been registered.
        /// </summary>
        RPC_NT_TYPE_ALREADY_REGISTERED = 0xC002000D,

        /// <summary>
        /// MessageId: RPC_NT_ALREADY_LISTENING
        /// MessageText:
        /// The RPC server is already listening.
        /// </summary>
        RPC_NT_ALREADY_LISTENING = 0xC002000E,

        /// <summary>
        /// MessageId: RPC_NT_NO_PROTSEQS_REGISTERED
        /// MessageText:
        /// No protocol sequences have been registered.
        /// </summary>
        RPC_NT_NO_PROTSEQS_REGISTERED = 0xC002000F,

        /// <summary>
        /// MessageId: RPC_NT_NOT_LISTENING
        /// MessageText:
        /// The RPC server is not listening.
        /// </summary>
        RPC_NT_NOT_LISTENING = 0xC0020010,

        /// <summary>
        /// MessageId: RPC_NT_UNKNOWN_MGR_TYPE
        /// MessageText:
        /// The manager type is unknown.
        /// </summary>
        RPC_NT_UNKNOWN_MGR_TYPE = 0xC0020011,

        /// <summary>
        /// MessageId: RPC_NT_UNKNOWN_IF
        /// MessageText:
        /// The interface is unknown.
        /// </summary>
        RPC_NT_UNKNOWN_IF = 0xC0020012,

        /// <summary>
        /// MessageId: RPC_NT_NO_BINDINGS
        /// MessageText:
        /// There are no bindings.
        /// </summary>
        RPC_NT_NO_BINDINGS = 0xC0020013,

        /// <summary>
        /// MessageId: RPC_NT_NO_PROTSEQS
        /// MessageText:
        /// There are no protocol sequences.
        /// </summary>
        RPC_NT_NO_PROTSEQS = 0xC0020014,

        /// <summary>
        /// MessageId: RPC_NT_CANT_CREATE_ENDPOINT
        /// MessageText:
        /// The endpoint cannot be created.
        /// </summary>
        RPC_NT_CANT_CREATE_ENDPOINT = 0xC0020015,

        /// <summary>
        /// MessageId: RPC_NT_OUT_OF_RESOURCES
        /// MessageText:
        /// Not enough resources are available to complete this operation.
        /// </summary>
        RPC_NT_OUT_OF_RESOURCES = 0xC0020016,

        /// <summary>
        /// MessageId: RPC_NT_SERVER_UNAVAILABLE
        /// MessageText:
        /// The RPC server is unavailable.
        /// </summary>
        RPC_NT_SERVER_UNAVAILABLE = 0xC0020017,

        /// <summary>
        /// MessageId: RPC_NT_SERVER_TOO_BUSY
        /// MessageText:
        /// The RPC server is too busy to complete this operation.
        /// </summary>
        RPC_NT_SERVER_TOO_BUSY = 0xC0020018,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_NETWORK_OPTIONS
        /// MessageText:
        /// The network options are invalid.
        /// </summary>
        RPC_NT_INVALID_NETWORK_OPTIONS = 0xC0020019,

        /// <summary>
        /// MessageId: RPC_NT_NO_CALL_ACTIVE
        /// MessageText:
        /// There are no remote procedure calls active on this thread.
        /// </summary>
        RPC_NT_NO_CALL_ACTIVE = 0xC002001A,

        /// <summary>
        /// MessageId: RPC_NT_CALL_FAILED
        /// MessageText:
        /// The remote procedure call failed.
        /// </summary>
        RPC_NT_CALL_FAILED = 0xC002001B,

        /// <summary>
        /// MessageId: RPC_NT_CALL_FAILED_DNE
        /// MessageText:
        /// The remote procedure call failed and did not execute.
        /// </summary>
        RPC_NT_CALL_FAILED_DNE = 0xC002001C,

        /// <summary>
        /// MessageId: RPC_NT_PROTOCOL_ERROR
        /// MessageText:
        /// An RPC protocol error occurred.
        /// </summary>
        RPC_NT_PROTOCOL_ERROR = 0xC002001D,

        /// <summary>
        /// MessageId: RPC_NT_UNSUPPORTED_TRANS_SYN
        /// MessageText:
        /// The transfer syntax is not supported by the RPC server.
        /// </summary>
        RPC_NT_UNSUPPORTED_TRANS_SYN = 0xC002001F,

        /// <summary>
        /// MessageId: RPC_NT_UNSUPPORTED_TYPE
        /// MessageText:
        /// The type UUID is not supported.
        /// </summary>
        RPC_NT_UNSUPPORTED_TYPE = 0xC0020021,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_TAG
        /// MessageText:
        /// The tag is invalid.
        /// </summary>
        RPC_NT_INVALID_TAG = 0xC0020022,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_BOUND
        /// MessageText:
        /// The array bounds are invalid.
        /// </summary>
        RPC_NT_INVALID_BOUND = 0xC0020023,

        /// <summary>
        /// MessageId: RPC_NT_NO_ENTRY_NAME
        /// MessageText:
        /// The binding does not contain an entry name.
        /// </summary>
        RPC_NT_NO_ENTRY_NAME = 0xC0020024,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_NAME_SYNTAX
        /// MessageText:
        /// The name syntax is invalid.
        /// </summary>
        RPC_NT_INVALID_NAME_SYNTAX = 0xC0020025,

        /// <summary>
        /// MessageId: RPC_NT_UNSUPPORTED_NAME_SYNTAX
        /// MessageText:
        /// The name syntax is not supported.
        /// </summary>
        RPC_NT_UNSUPPORTED_NAME_SYNTAX = 0xC0020026,

        /// <summary>
        /// MessageId: RPC_NT_UUID_NO_ADDRESS
        /// MessageText:
        /// No network address is available to use to construct a UUID.
        /// </summary>
        RPC_NT_UUID_NO_ADDRESS = 0xC0020028,

        /// <summary>
        /// MessageId: RPC_NT_DUPLICATE_ENDPOINT
        /// MessageText:
        /// The endpoint is a duplicate.
        /// </summary>
        RPC_NT_DUPLICATE_ENDPOINT = 0xC0020029,

        /// <summary>
        /// MessageId: RPC_NT_UNKNOWN_AUTHN_TYPE
        /// MessageText:
        /// The authentication type is unknown.
        /// </summary>
        RPC_NT_UNKNOWN_AUTHN_TYPE = 0xC002002A,

        /// <summary>
        /// MessageId: RPC_NT_MAX_CALLS_TOO_SMALL
        /// MessageText:
        /// The maximum number of calls is too small.
        /// </summary>
        RPC_NT_MAX_CALLS_TOO_SMALL = 0xC002002B,

        /// <summary>
        /// MessageId: RPC_NT_STRING_TOO_LONG
        /// MessageText:
        /// The string is too long.
        /// </summary>
        RPC_NT_STRING_TOO_LONG = 0xC002002C,

        /// <summary>
        /// MessageId: RPC_NT_PROTSEQ_NOT_FOUND
        /// MessageText:
        /// The RPC protocol sequence was not found.
        /// </summary>
        RPC_NT_PROTSEQ_NOT_FOUND = 0xC002002D,

        /// <summary>
        /// MessageId: RPC_NT_PROCNUM_OUT_OF_RANGE
        /// MessageText:
        /// The procedure number is out of range.
        /// </summary>
        RPC_NT_PROCNUM_OUT_OF_RANGE = 0xC002002E,

        /// <summary>
        /// MessageId: RPC_NT_BINDING_HAS_NO_AUTH
        /// MessageText:
        /// The binding does not contain any authentication information.
        /// </summary>
        RPC_NT_BINDING_HAS_NO_AUTH = 0xC002002F,

        /// <summary>
        /// MessageId: RPC_NT_UNKNOWN_AUTHN_SERVICE
        /// MessageText:
        /// The authentication service is unknown.
        /// </summary>
        RPC_NT_UNKNOWN_AUTHN_SERVICE = 0xC0020030,

        /// <summary>
        /// MessageId: RPC_NT_UNKNOWN_AUTHN_LEVEL
        /// MessageText:
        /// The authentication level is unknown.
        /// </summary>
        RPC_NT_UNKNOWN_AUTHN_LEVEL = 0xC0020031,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_AUTH_IDENTITY
        /// MessageText:
        /// The security context is invalid.
        /// </summary>
        RPC_NT_INVALID_AUTH_IDENTITY = 0xC0020032,

        /// <summary>
        /// MessageId: RPC_NT_UNKNOWN_AUTHZ_SERVICE
        /// MessageText:
        /// The authorization service is unknown.
        /// </summary>
        RPC_NT_UNKNOWN_AUTHZ_SERVICE = 0xC0020033,

        /// <summary>
        /// MessageId: EPT_NT_INVALID_ENTRY
        /// MessageText:
        /// The entry is invalid.
        /// </summary>
        EPT_NT_INVALID_ENTRY = 0xC0020034,

        /// <summary>
        /// MessageId: EPT_NT_CANT_PERFORM_OP
        /// MessageText:
        /// The operation cannot be performed.
        /// </summary>
        EPT_NT_CANT_PERFORM_OP = 0xC0020035,

        /// <summary>
        /// MessageId: EPT_NT_NOT_REGISTERED
        /// MessageText:
        /// There are no more endpoints available from the endpoint mapper.
        /// </summary>
        EPT_NT_NOT_REGISTERED = 0xC0020036,

        /// <summary>
        /// MessageId: RPC_NT_NOTHING_TO_EXPORT
        /// MessageText:
        /// No interfaces have been exported.
        /// </summary>
        RPC_NT_NOTHING_TO_EXPORT = 0xC0020037,

        /// <summary>
        /// MessageId: RPC_NT_INCOMPLETE_NAME
        /// MessageText:
        /// The entry name is incomplete.
        /// </summary>
        RPC_NT_INCOMPLETE_NAME = 0xC0020038,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_VERS_OPTION
        /// MessageText:
        /// The version option is invalid.
        /// </summary>
        RPC_NT_INVALID_VERS_OPTION = 0xC0020039,

        /// <summary>
        /// MessageId: RPC_NT_NO_MORE_MEMBERS
        /// MessageText:
        /// There are no more members.
        /// </summary>
        RPC_NT_NO_MORE_MEMBERS = 0xC002003A,

        /// <summary>
        /// MessageId: RPC_NT_NOT_ALL_OBJS_UNEXPORTED
        /// MessageText:
        /// There is nothing to unexport.
        /// </summary>
        RPC_NT_NOT_ALL_OBJS_UNEXPORTED = 0xC002003B,

        /// <summary>
        /// MessageId: RPC_NT_INTERFACE_NOT_FOUND
        /// MessageText:
        /// The interface was not found.
        /// </summary>
        RPC_NT_INTERFACE_NOT_FOUND = 0xC002003C,

        /// <summary>
        /// MessageId: RPC_NT_ENTRY_ALREADY_EXISTS
        /// MessageText:
        /// The entry already exists.
        /// </summary>
        RPC_NT_ENTRY_ALREADY_EXISTS = 0xC002003D,

        /// <summary>
        /// MessageId: RPC_NT_ENTRY_NOT_FOUND
        /// MessageText:
        /// The entry is not found.
        /// </summary>
        RPC_NT_ENTRY_NOT_FOUND = 0xC002003E,

        /// <summary>
        /// MessageId: RPC_NT_NAME_SERVICE_UNAVAILABLE
        /// MessageText:
        /// The name service is unavailable.
        /// </summary>
        RPC_NT_NAME_SERVICE_UNAVAILABLE = 0xC002003F,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_NAF_ID
        /// MessageText:
        /// The network address family is invalid.
        /// </summary>
        RPC_NT_INVALID_NAF_ID = 0xC0020040,

        /// <summary>
        /// MessageId: RPC_NT_CANNOT_SUPPORT
        /// MessageText:
        /// The requested operation is not supported.
        /// </summary>
        RPC_NT_CANNOT_SUPPORT = 0xC0020041,

        /// <summary>
        /// MessageId: RPC_NT_NO_CONTEXT_AVAILABLE
        /// MessageText:
        /// No security context is available to allow impersonation.
        /// </summary>
        RPC_NT_NO_CONTEXT_AVAILABLE = 0xC0020042,

        /// <summary>
        /// MessageId: RPC_NT_INTERNAL_ERROR
        /// MessageText:
        /// An internal error occurred in RPC.
        /// </summary>
        RPC_NT_INTERNAL_ERROR = 0xC0020043,

        /// <summary>
        /// MessageId: RPC_NT_ZERO_DIVIDE
        /// MessageText:
        /// The RPC server attempted an integer divide by zero.
        /// </summary>
        RPC_NT_ZERO_DIVIDE = 0xC0020044,

        /// <summary>
        /// MessageId: RPC_NT_ADDRESS_ERROR
        /// MessageText:
        /// An addressing error occurred in the RPC server.
        /// </summary>
        RPC_NT_ADDRESS_ERROR = 0xC0020045,

        /// <summary>
        /// MessageId: RPC_NT_FP_DIV_ZERO
        /// MessageText:
        /// A floating point operation at the RPC server caused a divide by zero.
        /// </summary>
        RPC_NT_FP_DIV_ZERO = 0xC0020046,

        /// <summary>
        /// MessageId: RPC_NT_FP_UNDERFLOW
        /// MessageText:
        /// A floating point underflow occurred at the RPC server.
        /// </summary>
        RPC_NT_FP_UNDERFLOW = 0xC0020047,

        /// <summary>
        /// MessageId: RPC_NT_FP_OVERFLOW
        /// MessageText:
        /// A floating point overflow occurred at the RPC server.
        /// </summary>
        RPC_NT_FP_OVERFLOW = 0xC0020048,

        /// <summary>
        /// MessageId: RPC_NT_NO_MORE_ENTRIES
        /// MessageText:
        /// The list of RPC servers available for auto-handle binding has been exhausted.
        /// </summary>
        RPC_NT_NO_MORE_ENTRIES = 0xC0030001,

        /// <summary>
        /// MessageId: RPC_NT_SS_CHAR_TRANS_OPEN_FAIL
        /// MessageText:
        /// The file designated by DCERPCCHARTRANS cannot be opened.
        /// </summary>
        RPC_NT_SS_CHAR_TRANS_OPEN_FAIL = 0xC0030002,

        /// <summary>
        /// MessageId: RPC_NT_SS_CHAR_TRANS_SHORT_FILE
        /// MessageText:
        /// The file containing the character translation table has fewer than 512 bytes.
        /// </summary>
        RPC_NT_SS_CHAR_TRANS_SHORT_FILE = 0xC0030003,

        /// <summary>
        /// MessageId: RPC_NT_SS_IN_NULL_CONTEXT
        /// MessageText:
        /// A null context handle is passed as an [in] parameter.
        /// </summary>
        RPC_NT_SS_IN_NULL_CONTEXT = 0xC0030004,

        /// <summary>
        /// MessageId: RPC_NT_SS_CONTEXT_MISMATCH
        /// MessageText:
        /// The context handle does not match any known context handles.
        /// </summary>
        RPC_NT_SS_CONTEXT_MISMATCH = 0xC0030005,

        /// <summary>
        /// MessageId: RPC_NT_SS_CONTEXT_DAMAGED
        /// MessageText:
        /// The context handle changed during a call.
        /// </summary>
        RPC_NT_SS_CONTEXT_DAMAGED = 0xC0030006,

        /// <summary>
        /// MessageId: RPC_NT_SS_HANDLES_MISMATCH
        /// MessageText:
        /// The binding handles passed to a remote procedure call do not match.
        /// </summary>
        RPC_NT_SS_HANDLES_MISMATCH = 0xC0030007,

        /// <summary>
        /// MessageId: RPC_NT_SS_CANNOT_GET_CALL_HANDLE
        /// MessageText:
        /// The stub is unable to get the call handle.
        /// </summary>
        RPC_NT_SS_CANNOT_GET_CALL_HANDLE = 0xC0030008,

        /// <summary>
        /// MessageId: RPC_NT_NULL_REF_POINTER
        /// MessageText:
        /// A null reference pointer was passed to the stub.
        /// </summary>
        RPC_NT_NULL_REF_POINTER = 0xC0030009,

        /// <summary>
        /// MessageId: RPC_NT_ENUM_VALUE_OUT_OF_RANGE
        /// MessageText:
        /// The enumeration value is out of range.
        /// </summary>
        RPC_NT_ENUM_VALUE_OUT_OF_RANGE = 0xC003000A,

        /// <summary>
        /// MessageId: RPC_NT_BYTE_COUNT_TOO_SMALL
        /// MessageText:
        /// The byte count is too small.
        /// </summary>
        RPC_NT_BYTE_COUNT_TOO_SMALL = 0xC003000B,

        /// <summary>
        /// MessageId: RPC_NT_BAD_STUB_DATA
        /// MessageText:
        /// The stub received bad data.
        /// </summary>
        RPC_NT_BAD_STUB_DATA = 0xC003000C,

        /// <summary>
        /// MessageId: RPC_NT_CALL_IN_PROGRESS
        /// MessageText:
        /// A remote procedure call is already in progress for this thread.
        /// </summary>
        RPC_NT_CALL_IN_PROGRESS = 0xC0020049,

        /// <summary>
        /// MessageId: RPC_NT_NO_MORE_BINDINGS
        /// MessageText:
        /// There are no more bindings.
        /// </summary>
        RPC_NT_NO_MORE_BINDINGS = 0xC002004A,

        /// <summary>
        /// MessageId: RPC_NT_GROUP_MEMBER_NOT_FOUND
        /// MessageText:
        /// The group member was not found.
        /// </summary>
        RPC_NT_GROUP_MEMBER_NOT_FOUND = 0xC002004B,

        /// <summary>
        /// MessageId: EPT_NT_CANT_CREATE
        /// MessageText:
        /// The endpoint mapper database entry could not be created.
        /// </summary>
        EPT_NT_CANT_CREATE = 0xC002004C,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_OBJECT
        /// MessageText:
        /// The object UUID is the nil UUID.
        /// </summary>
        RPC_NT_INVALID_OBJECT = 0xC002004D,

        /// <summary>
        /// MessageId: RPC_NT_NO_INTERFACES
        /// MessageText:
        /// No interfaces have been registered.
        /// </summary>
        RPC_NT_NO_INTERFACES = 0xC002004F,

        /// <summary>
        /// MessageId: RPC_NT_CALL_CANCELLED
        /// MessageText:
        /// The remote procedure call was cancelled.
        /// </summary>
        RPC_NT_CALL_CANCELLED = 0xC0020050,

        /// <summary>
        /// MessageId: RPC_NT_BINDING_INCOMPLETE
        /// MessageText:
        /// The binding handle does not contain all required information.
        /// </summary>
        RPC_NT_BINDING_INCOMPLETE = 0xC0020051,

        /// <summary>
        /// MessageId: RPC_NT_COMM_FAILURE
        /// MessageText:
        /// A communications failure occurred during a remote procedure call.
        /// </summary>
        RPC_NT_COMM_FAILURE = 0xC0020052,

        /// <summary>
        /// MessageId: RPC_NT_UNSUPPORTED_AUTHN_LEVEL
        /// MessageText:
        /// The requested authentication level is not supported.
        /// </summary>
        RPC_NT_UNSUPPORTED_AUTHN_LEVEL = 0xC0020053,

        /// <summary>
        /// MessageId: RPC_NT_NO_PRINC_NAME
        /// MessageText:
        /// No principal name registered.
        /// </summary>
        RPC_NT_NO_PRINC_NAME = 0xC0020054,

        /// <summary>
        /// MessageId: RPC_NT_NOT_RPC_ERROR
        /// MessageText:
        /// The error specified is not a valid Windows RPC error code.
        /// </summary>
        RPC_NT_NOT_RPC_ERROR = 0xC0020055,

        /// <summary>
        /// MessageId: RPC_NT_UUID_LOCAL_ONLY
        /// MessageText:
        /// A UUID that is valid only on this computer has been allocated.
        /// </summary>
        RPC_NT_UUID_LOCAL_ONLY = 0x40020056,

        /// <summary>
        /// MessageId: RPC_NT_SEC_PKG_ERROR
        /// MessageText:
        /// A security package specific error occurred.
        /// </summary>
        RPC_NT_SEC_PKG_ERROR = 0xC0020057,

        /// <summary>
        /// MessageId: RPC_NT_NOT_CANCELLED
        /// MessageText:
        /// Thread is not cancelled.
        /// </summary>
        RPC_NT_NOT_CANCELLED = 0xC0020058,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_ES_ACTION
        /// MessageText:
        /// Invalid operation on the encoding/decoding handle.
        /// </summary>
        RPC_NT_INVALID_ES_ACTION = 0xC0030059,

        /// <summary>
        /// MessageId: RPC_NT_WRONG_ES_VERSION
        /// MessageText:
        /// Incompatible version of the serializing package.
        /// </summary>
        RPC_NT_WRONG_ES_VERSION = 0xC003005A,

        /// <summary>
        /// MessageId: RPC_NT_WRONG_STUB_VERSION
        /// MessageText:
        /// Incompatible version of the RPC stub.
        /// </summary>
        RPC_NT_WRONG_STUB_VERSION = 0xC003005B,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_PIPE_OBJECT
        /// MessageText:
        /// The RPC pipe object is invalid or corrupted.
        /// </summary>
        RPC_NT_INVALID_PIPE_OBJECT = 0xC003005C,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_PIPE_OPERATION
        /// MessageText:
        /// An invalid operation was attempted on an RPC pipe object.
        /// </summary>
        RPC_NT_INVALID_PIPE_OPERATION = 0xC003005D,

        /// <summary>
        /// MessageId: RPC_NT_WRONG_PIPE_VERSION
        /// MessageText:
        /// Unsupported RPC pipe version.
        /// </summary>
        RPC_NT_WRONG_PIPE_VERSION = 0xC003005E,

        /// <summary>
        /// MessageId: RPC_NT_PIPE_CLOSED
        /// MessageText:
        /// The RPC pipe object has already been closed.
        /// </summary>
        RPC_NT_PIPE_CLOSED = 0xC003005F,

        /// <summary>
        /// MessageId: RPC_NT_PIPE_DISCIPLINE_ERROR
        /// MessageText:
        /// The RPC call completed before all pipes were processed.
        /// </summary>
        RPC_NT_PIPE_DISCIPLINE_ERROR = 0xC0030060,

        /// <summary>
        /// MessageId: RPC_NT_PIPE_EMPTY
        /// MessageText:
        /// No more data is available from the RPC pipe.
        /// </summary>
        RPC_NT_PIPE_EMPTY = 0xC0030061,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_ASYNC_HANDLE
        /// MessageText:
        /// Invalid asynchronous remote procedure call handle.
        /// </summary>
        RPC_NT_INVALID_ASYNC_HANDLE = 0xC0020062,

        /// <summary>
        /// MessageId: RPC_NT_INVALID_ASYNC_CALL
        /// MessageText:
        /// Invalid asynchronous RPC call handle for this operation.
        /// </summary>
        RPC_NT_INVALID_ASYNC_CALL = 0xC0020063,

        /// <summary>
        /// MessageId: RPC_NT_PROXY_ACCESS_DENIED
        /// MessageText:
        /// Access to the HTTP proxy is denied.
        /// </summary>
        RPC_NT_PROXY_ACCESS_DENIED = 0xC0020064,

        /// <summary>
        /// MessageId: RPC_NT_SEND_INCOMPLETE
        /// MessageText:
        /// Some data remains to be sent in the request buffer.
        /// </summary>
        RPC_NT_SEND_INCOMPLETE = 0x400200AF,

        /// <summary>
        ///  ACPI error values
        /// MessageId: STATUS_ACPI_INVALID_OPCODE
        /// MessageText:
        /// An attempt was made to run an invalid AML opcode
        /// </summary>
        STATUS_ACPI_INVALID_OPCODE = 0xC0140001,

        /// <summary>
        /// MessageId: STATUS_ACPI_STACK_OVERFLOW
        /// MessageText:
        /// The AML Interpreter Stack has overflowed
        /// </summary>
        STATUS_ACPI_STACK_OVERFLOW = 0xC0140002,

        /// <summary>
        /// MessageId: STATUS_ACPI_ASSERT_FAILED
        /// MessageText:
        /// An inconsistent state has occurred
        /// </summary>
        STATUS_ACPI_ASSERT_FAILED = 0xC0140003,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_INDEX
        /// MessageText:
        /// An attempt was made to access an array outside of its bounds
        /// </summary>
        STATUS_ACPI_INVALID_INDEX = 0xC0140004,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_ARGUMENT
        /// MessageText:
        /// A required argument was not specified
        /// </summary>
        STATUS_ACPI_INVALID_ARGUMENT = 0xC0140005,

        /// <summary>
        /// MessageId: STATUS_ACPI_FATAL
        /// MessageText:
        /// A fatal error has occurred
        /// </summary>
        STATUS_ACPI_FATAL = 0xC0140006,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_SUPERNAME
        /// MessageText:
        /// An invalid SuperName was specified
        /// </summary>
        STATUS_ACPI_INVALID_SUPERNAME = 0xC0140007,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_ARGTYPE
        /// MessageText:
        /// An argument with an incorrect type was specified
        /// </summary>
        STATUS_ACPI_INVALID_ARGTYPE = 0xC0140008,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_OBJTYPE
        /// MessageText:
        /// An object with an incorrect type was specified
        /// </summary>
        STATUS_ACPI_INVALID_OBJTYPE = 0xC0140009,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_TARGETTYPE
        /// MessageText:
        /// A target with an incorrect type was specified
        /// </summary>
        STATUS_ACPI_INVALID_TARGETTYPE = 0xC014000A,

        /// <summary>
        /// MessageId: STATUS_ACPI_INCORRECT_ARGUMENT_COUNT
        /// MessageText:
        /// An incorrect number of arguments were specified
        /// </summary>
        STATUS_ACPI_INCORRECT_ARGUMENT_COUNT = 0xC014000B,

        /// <summary>
        /// MessageId: STATUS_ACPI_ADDRESS_NOT_MAPPED
        /// MessageText:
        /// An address failed to translate
        /// </summary>
        STATUS_ACPI_ADDRESS_NOT_MAPPED = 0xC014000C,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_EVENTTYPE
        /// MessageText:
        /// An incorrect event type was specified
        /// </summary>
        STATUS_ACPI_INVALID_EVENTTYPE = 0xC014000D,

        /// <summary>
        /// MessageId: STATUS_ACPI_HANDLER_COLLISION
        /// MessageText:
        /// A handler for the target already exists
        /// </summary>
        STATUS_ACPI_HANDLER_COLLISION = 0xC014000E,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_DATA
        /// MessageText:
        /// Invalid data for the target was specified
        /// </summary>
        STATUS_ACPI_INVALID_DATA = 0xC014000F,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_REGION
        /// MessageText:
        /// An invalid region for the target was specified
        /// </summary>
        STATUS_ACPI_INVALID_REGION = 0xC0140010,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_ACCESS_SIZE
        /// MessageText:
        /// An attempt was made to access a field outside of the defined range
        /// </summary>
        STATUS_ACPI_INVALID_ACCESS_SIZE = 0xC0140011,

        /// <summary>
        /// MessageId: STATUS_ACPI_ACQUIRE_GLOBAL_LOCK
        /// MessageText:
        /// The Global system lock could not be acquired
        /// </summary>
        STATUS_ACPI_ACQUIRE_GLOBAL_LOCK = 0xC0140012,

        /// <summary>
        /// MessageId: STATUS_ACPI_ALREADY_INITIALIZED
        /// MessageText:
        /// An attempt was made to reinitialize the ACPI subsystem
        /// </summary>
        STATUS_ACPI_ALREADY_INITIALIZED = 0xC0140013,

        /// <summary>
        /// MessageId: STATUS_ACPI_NOT_INITIALIZED
        /// MessageText:
        /// The ACPI subsystem has not been initialized
        /// </summary>
        STATUS_ACPI_NOT_INITIALIZED = 0xC0140014,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_MUTEX_LEVEL
        /// MessageText:
        /// An incorrect mutex was specified
        /// </summary>
        STATUS_ACPI_INVALID_MUTEX_LEVEL = 0xC0140015,

        /// <summary>
        /// MessageId: STATUS_ACPI_MUTEX_NOT_OWNED
        /// MessageText:
        /// The mutex is not currently owned
        /// </summary>
        STATUS_ACPI_MUTEX_NOT_OWNED = 0xC0140016,

        /// <summary>
        /// MessageId: STATUS_ACPI_MUTEX_NOT_OWNER
        /// MessageText:
        /// An attempt was made to access the mutex by a process that was not the owner
        /// </summary>
        STATUS_ACPI_MUTEX_NOT_OWNER = 0xC0140017,

        /// <summary>
        /// MessageId: STATUS_ACPI_RS_ACCESS
        /// MessageText:
        /// An error occurred during an access to Region Space
        /// </summary>
        STATUS_ACPI_RS_ACCESS = 0xC0140018,

        /// <summary>
        /// MessageId: STATUS_ACPI_INVALID_TABLE
        /// MessageText:
        /// An attempt was made to use an incorrect table
        /// </summary>
        STATUS_ACPI_INVALID_TABLE = 0xC0140019,

        /// <summary>
        /// MessageId: STATUS_ACPI_REG_HANDLER_FAILED
        /// MessageText:
        /// The registration of an ACPI event failed
        /// </summary>
        STATUS_ACPI_REG_HANDLER_FAILED = 0xC0140020,

        /// <summary>
        /// MessageId: STATUS_ACPI_POWER_REQUEST_FAILED
        /// MessageText:
        /// An ACPI Power Object failed to transition state
        /// </summary>
        STATUS_ACPI_POWER_REQUEST_FAILED = 0xC0140021,

        /// <summary>
        /// Terminal Server specific Errors
        /// MessageId: STATUS_CTX_WINSTATION_NAME_INVALID
        /// MessageText:
        /// Session name %1 is invalid.
        /// </summary>
        STATUS_CTX_WINSTATION_NAME_INVALID = 0xC00A0001,

        /// <summary>
        /// MessageId: STATUS_CTX_INVALID_PD
        /// MessageText:
        /// The protocol driver %1 is invalid.
        /// </summary>
        STATUS_CTX_INVALID_PD = 0xC00A0002,

        /// <summary>
        /// MessageId: STATUS_CTX_PD_NOT_FOUND
        /// MessageText:
        /// The protocol driver %1 was not found in the system path.
        /// </summary>
        STATUS_CTX_PD_NOT_FOUND = 0xC00A0003,

        /// <summary>
        /// MessageId: STATUS_CTX_CDM_CONNECT
        /// MessageText:
        /// The Client Drive Mapping Service Has Connected on Terminal Connection.
        /// </summary>
        STATUS_CTX_CDM_CONNECT = 0x400A0004,

        /// <summary>
        /// MessageId: STATUS_CTX_CDM_DISCONNECT
        /// MessageText:
        /// The Client Drive Mapping Service Has Disconnected on Terminal Connection.
        /// </summary>
        STATUS_CTX_CDM_DISCONNECT = 0x400A0005,

        /// <summary>
        /// MessageId: STATUS_CTX_CLOSE_PENDING
        /// MessageText:
        /// A close operation is pending on the Terminal Connection.
        /// </summary>
        STATUS_CTX_CLOSE_PENDING = 0xC00A0006,

        /// <summary>
        /// MessageId: STATUS_CTX_NO_OUTBUF
        /// MessageText:
        /// There are no free output buffers available.
        /// </summary>
        STATUS_CTX_NO_OUTBUF = 0xC00A0007,

        /// <summary>
        /// MessageId: STATUS_CTX_MODEM_INF_NOT_FOUND
        /// MessageText:
        /// The MODEM.INF file was not found.
        /// </summary>
        STATUS_CTX_MODEM_INF_NOT_FOUND = 0xC00A0008,

        /// <summary>
        /// MessageId: STATUS_CTX_INVALID_MODEMNAME
        /// MessageText:
        /// The modem (%1) was not found in MODEM.INF.
        /// </summary>
        STATUS_CTX_INVALID_MODEMNAME = 0xC00A0009,

        /// <summary>
        /// MessageId: STATUS_CTX_RESPONSE_ERROR
        /// MessageText:
        /// The modem did not accept the command sent to it.
        /// Verify the configured modem name matches the attached modem.
        /// </summary>
        STATUS_CTX_RESPONSE_ERROR = 0xC00A000A,

        /// <summary>
        /// MessageId: STATUS_CTX_MODEM_RESPONSE_TIMEOUT
        /// MessageText:
        /// The modem did not respond to the command sent to it.
        /// Verify the modem is properly cabled and powered on.
        /// </summary>
        STATUS_CTX_MODEM_RESPONSE_TIMEOUT = 0xC00A000B,

        /// <summary>
        /// MessageId: STATUS_CTX_MODEM_RESPONSE_NO_CARRIER
        /// MessageText:
        /// Carrier detect has failed or carrier has been dropped due to disconnect.
        /// </summary>
        STATUS_CTX_MODEM_RESPONSE_NO_CARRIER = 0xC00A000C,

        /// <summary>
        /// MessageId: STATUS_CTX_MODEM_RESPONSE_NO_DIALTONE
        /// MessageText:
        /// Dial tone was not detected within required time.
        /// Verify phone cable is properly attached and functional.
        /// </summary>
        STATUS_CTX_MODEM_RESPONSE_NO_DIALTONE = 0xC00A000D,

        /// <summary>
        /// MessageId: STATUS_CTX_MODEM_RESPONSE_BUSY
        /// MessageText:
        /// Busy signal detected at remote site on callback.
        /// </summary>
        STATUS_CTX_MODEM_RESPONSE_BUSY = 0xC00A000E,

        /// <summary>
        /// MessageId: STATUS_CTX_MODEM_RESPONSE_VOICE
        /// MessageText:
        /// Voice detected at remote site on callback.
        /// </summary>
        STATUS_CTX_MODEM_RESPONSE_VOICE = 0xC00A000F,

        /// <summary>
        /// MessageId: STATUS_CTX_TD_ERROR
        /// MessageText:
        /// Transport driver error
        /// </summary>
        STATUS_CTX_TD_ERROR = 0xC00A0010,

        /// <summary>
        /// MessageId: STATUS_CTX_LICENSE_CLIENT_INVALID
        /// MessageText:
        /// The client you are using is not licensed to use this system. Your logon request is denied.
        /// </summary>
        STATUS_CTX_LICENSE_CLIENT_INVALID = 0xC00A0012,

        /// <summary>
        /// MessageId: STATUS_CTX_LICENSE_NOT_AVAILABLE
        /// MessageText:
        /// The system has reached its licensed logon limit.
        /// Please try again later.
        /// </summary>
        STATUS_CTX_LICENSE_NOT_AVAILABLE = 0xC00A0013,

        /// <summary>
        /// MessageId: STATUS_CTX_LICENSE_EXPIRED
        /// MessageText:
        /// The system license has expired. Your logon request is denied.
        /// </summary>
        STATUS_CTX_LICENSE_EXPIRED = 0xC00A0014,

        /// <summary>
        /// MessageId: STATUS_CTX_WINSTATION_NOT_FOUND
        /// MessageText:
        /// The specified session cannot be found.
        /// </summary>
        STATUS_CTX_WINSTATION_NOT_FOUND = 0xC00A0015,

        /// <summary>
        /// MessageId: STATUS_CTX_WINSTATION_NAME_COLLISION
        /// MessageText:
        /// The specified session name is already in use.
        /// </summary>
        STATUS_CTX_WINSTATION_NAME_COLLISION = 0xC00A0016,

        /// <summary>
        /// MessageId: STATUS_CTX_WINSTATION_BUSY
        /// MessageText:
        /// The requested operation cannot be completed because the Terminal Connection is currently busy processing a connect, disconnect, reset, or delete operation.
        /// </summary>
        STATUS_CTX_WINSTATION_BUSY = 0xC00A0017,

        /// <summary>
        /// MessageId: STATUS_CTX_BAD_VIDEO_MODE
        /// MessageText:
        /// An attempt has been made to connect to a session whose video mode is not supported by the current client.
        /// </summary>
        STATUS_CTX_BAD_VIDEO_MODE = 0xC00A0018,

        /// <summary>
        /// MessageId: STATUS_CTX_GRAPHICS_INVALID
        /// MessageText:
        /// The application attempted to enable DOS graphics mode.
        /// DOS graphics mode is not supported.
        /// </summary>
        STATUS_CTX_GRAPHICS_INVALID = 0xC00A0022,

        /// <summary>
        /// MessageId: STATUS_CTX_NOT_CONSOLE
        /// MessageText:
        /// The requested operation can be performed only on the system console.
        /// This is most often the result of a driver or system DLL requiring direct console access.
        /// </summary>
        STATUS_CTX_NOT_CONSOLE = 0xC00A0024,

        /// <summary>
        /// MessageId: STATUS_CTX_CLIENT_QUERY_TIMEOUT
        /// MessageText:
        /// The client failed to respond to the server connect message.
        /// </summary>
        STATUS_CTX_CLIENT_QUERY_TIMEOUT = 0xC00A0026,

        /// <summary>
        /// MessageId: STATUS_CTX_CONSOLE_DISCONNECT
        /// MessageText:
        /// Disconnecting the console session is not supported.
        /// </summary>
        STATUS_CTX_CONSOLE_DISCONNECT = 0xC00A0027,

        /// <summary>
        /// MessageId: STATUS_CTX_CONSOLE_CONNECT
        /// MessageText:
        /// Reconnecting a disconnected session to the console is not supported.
        /// </summary>
        STATUS_CTX_CONSOLE_CONNECT = 0xC00A0028,

        /// <summary>
        /// MessageId: STATUS_CTX_SHADOW_DENIED
        /// MessageText:
        /// The request to control another session remotely was denied.
        /// </summary>
        STATUS_CTX_SHADOW_DENIED = 0xC00A002A,

        /// <summary>
        /// MessageId: STATUS_CTX_WINSTATION_ACCESS_DENIED
        /// MessageText:
        /// A process has requested access to a session, but has not been granted those access rights.
        /// </summary>
        STATUS_CTX_WINSTATION_ACCESS_DENIED = 0xC00A002B,

        /// <summary>
        /// MessageId: STATUS_CTX_INVALID_WD
        /// MessageText:
        /// The Terminal Connection driver %1 is invalid.
        /// </summary>
        STATUS_CTX_INVALID_WD = 0xC00A002E,

        /// <summary>
        /// MessageId: STATUS_CTX_WD_NOT_FOUND
        /// MessageText:
        /// The Terminal Connection driver %1 was not found in the system path.
        /// </summary>
        STATUS_CTX_WD_NOT_FOUND = 0xC00A002F,

        /// <summary>
        /// MessageId: STATUS_CTX_SHADOW_INVALID
        /// MessageText:
        /// The requested session cannot be controlled remotely.
        /// You cannot control your own session, a session that is trying to control your session,
        /// a session that has no user logged on, nor control other sessions from the console.
        /// </summary>
        STATUS_CTX_SHADOW_INVALID = 0xC00A0030,

        /// <summary>
        /// MessageId: STATUS_CTX_SHADOW_DISABLED
        /// MessageText:
        /// The requested session is not configured to allow remote control.
        /// </summary>
        STATUS_CTX_SHADOW_DISABLED = 0xC00A0031,

        /// <summary>
        /// MessageId: STATUS_RDP_PROTOCOL_ERROR
        /// MessageText:
        /// The RDP protocol component %2 detected an error in the protocol stream and has disconnected the client.
        /// </summary>
        STATUS_RDP_PROTOCOL_ERROR = 0xC00A0032,

        /// <summary>
        /// MessageId: STATUS_CTX_CLIENT_LICENSE_NOT_SET
        /// MessageText:
        /// Your request to connect to this Terminal Server has been rejected.
        /// Your Terminal Server Client license number has not been entered for this copy of the Terminal Client.
        /// Please call your system administrator for help in entering a valid, unique license number for this Terminal Server Client.
        /// Click OK to continue.
        /// </summary>
        STATUS_CTX_CLIENT_LICENSE_NOT_SET = 0xC00A0033,

        /// <summary>
        /// MessageId: STATUS_CTX_CLIENT_LICENSE_IN_USE
        /// MessageText:
        /// Your request to connect to this Terminal server has been rejected.
        /// Your Terminal Server Client license number is currently being used by another user.
        /// Please call your system administrator to obtain a new copy of the Terminal Server Client with a valid, unique license number.
        /// Click OK to continue.
        /// </summary>
        STATUS_CTX_CLIENT_LICENSE_IN_USE = 0xC00A0034,

        /// <summary>
        /// MessageId: STATUS_CTX_SHADOW_ENDED_BY_MODE_CHANGE
        /// MessageText:
        /// The remote control of the console was terminated because the display mode was changed. Changing the display mode in a remote control session is not supported.
        /// </summary>
        STATUS_CTX_SHADOW_ENDED_BY_MODE_CHANGE = 0xC00A0035,

        /// <summary>
        /// MessageId: STATUS_CTX_SHADOW_NOT_RUNNING
        /// MessageText:
        /// Remote control could not be terminated because the specified session is not currently being remotely controlled.
        /// </summary>
        STATUS_CTX_SHADOW_NOT_RUNNING = 0xC00A0036,

        /// <summary>
        /// MessageId: STATUS_CTX_LOGON_DISABLED
        /// MessageText:
        /// Your interactive logon privilege has been disabled.
        /// Please contact your system administrator.
        /// </summary>
        STATUS_CTX_LOGON_DISABLED = 0xC00A0037,

        /// <summary>
        /// MessageId: STATUS_CTX_SECURITY_LAYER_ERROR
        /// MessageText:
        /// The Terminal Server security layer detected an error in the protocol stream and has disconnected the client.
        /// </summary>
        STATUS_CTX_SECURITY_LAYER_ERROR = 0xC00A0038,

        /// <summary>
        /// MessageId: STATUS_TS_INCOMPATIBLE_SESSIONS
        /// MessageText:
        /// The target session is incompatible with the current session.
        /// </summary>
        STATUS_TS_INCOMPATIBLE_SESSIONS = 0xC00A0039,

        /// <summary>
        /// IO error values
        /// MessageId: STATUS_PNP_BAD_MPS_TABLE
        /// MessageText:
        /// A device is missing in the system BIOS MPS table. This device will not be used.
        /// Please contact your system vendor for system BIOS update.
        /// </summary>
        STATUS_PNP_BAD_MPS_TABLE = 0xC0040035,

        /// <summary>
        /// MessageId: STATUS_PNP_TRANSLATION_FAILED
        /// MessageText:
        /// A translator failed to translate resources.
        /// </summary>
        STATUS_PNP_TRANSLATION_FAILED = 0xC0040036,

        /// <summary>
        /// MessageId: STATUS_PNP_IRQ_TRANSLATION_FAILED
        /// MessageText:
        /// An IRQ translator failed to translate resources.
        /// </summary>
        STATUS_PNP_IRQ_TRANSLATION_FAILED = 0xC0040037,

        /// <summary>
        /// MessageId: STATUS_PNP_INVALID_ID
        /// MessageText:
        /// Driver %2 returned invalid ID for a child device (%3).
        /// </summary>
        STATUS_PNP_INVALID_ID = 0xC0040038,

        /// <summary>
        /// MessageId: STATUS_IO_REISSUE_AS_CACHED
        /// MessageText:
        /// Reissue the given operation as a cached IO operation
        /// </summary>
        STATUS_IO_REISSUE_AS_CACHED = 0xC0040039,

        /// <summary>
        /// MUI error values
        /// MessageId: STATUS_MUI_FILE_NOT_FOUND
        /// MessageText:
        /// The resource loader failed to find MUI file.
        /// </summary>
        STATUS_MUI_FILE_NOT_FOUND = 0xC00B0001,

        /// <summary>
        /// MessageId: STATUS_MUI_INVALID_FILE
        /// MessageText:
        /// The resource loader failed to load MUI file because the file fail to pass validation.
        /// </summary>
        STATUS_MUI_INVALID_FILE = 0xC00B0002,

        /// <summary>
        /// MessageId: STATUS_MUI_INVALID_RC_CONFIG
        /// MessageText:
        /// The RC Manifest is corrupted with garbage data or unsupported version or missing required item.
        /// </summary>
        STATUS_MUI_INVALID_RC_CONFIG = 0xC00B0003,

        /// <summary>
        /// MessageId: STATUS_MUI_INVALID_LOCALE_NAME
        /// MessageText:
        /// The RC Manifest has invalid culture name.
        /// </summary>
        STATUS_MUI_INVALID_LOCALE_NAME = 0xC00B0004,

        /// <summary>
        /// MessageId: STATUS_MUI_INVALID_ULTIMATEFALLBACK_NAME
        /// MessageText:
        /// The RC Manifest has invalid ultimatefallback name.
        /// </summary>
        STATUS_MUI_INVALID_ULTIMATEFALLBACK_NAME = 0xC00B0005,

        /// <summary>
        /// MessageId: STATUS_MUI_FILE_NOT_LOADED
        /// MessageText:
        /// The resource loader cache doesn't have loaded MUI entry.
        /// </summary>
        STATUS_MUI_FILE_NOT_LOADED = 0xC00B0006,

        /// <summary>
        /// MessageId: STATUS_RESOURCE_ENUM_USER_STOP
        /// MessageText:
        /// User stopped resource enumeration.
        /// </summary>
        STATUS_RESOURCE_ENUM_USER_STOP = 0xC00B0007,

        /// <summary>
        /// MessageId: STATUS_FLT_NO_HANDLER_DEFINED
        /// MessageText:
        /// A handler was not defined by the filter for this operation.
        /// </summary>
        STATUS_FLT_NO_HANDLER_DEFINED = 0xC01C0001,

        /// <summary>
        /// MessageId: STATUS_FLT_CONTEXT_ALREADY_DEFINED
        /// MessageText:
        /// A context is already defined for this object.
        /// </summary>
        STATUS_FLT_CONTEXT_ALREADY_DEFINED = 0xC01C0002,

        /// <summary>
        /// MessageId: STATUS_FLT_INVALID_ASYNCHRONOUS_REQUEST
        /// MessageText:
        /// Asynchronous requests are not valid for this operation.
        /// </summary>
        STATUS_FLT_INVALID_ASYNCHRONOUS_REQUEST = 0xC01C0003,

        /// <summary>
        /// MessageId: STATUS_FLT_DISALLOW_FAST_IO
        /// MessageText:
        /// Internal error code used by the filter manager to determine if a fastio operation
        /// should be forced down the IRP path.  Mini-filters should never return this value.
        /// </summary>
        STATUS_FLT_DISALLOW_FAST_IO = 0xC01C0004,

        /// <summary>
        /// MessageId: STATUS_FLT_INVALID_NAME_REQUEST
        /// MessageText:
        /// An invalid name request was made. The name requested cannot be retrieved at this time.
        /// </summary>
        STATUS_FLT_INVALID_NAME_REQUEST = 0xC01C0005,

        /// <summary>
        /// MessageId: STATUS_FLT_NOT_SAFE_TO_POST_OPERATION
        /// MessageText:
        /// Posting this operation to a worker thread for further processing is not safe
        /// at this time because it could lead to a system deadlock.
        /// </summary>
        STATUS_FLT_NOT_SAFE_TO_POST_OPERATION = 0xC01C0006,

        /// <summary>
        /// MessageId: STATUS_FLT_NOT_INITIALIZED
        /// MessageText:
        /// The Filter Manager was not initialized when a filter tried to register.  Make
        /// sure that the Filter Manager is getting loaded as a driver.
        /// </summary>
        STATUS_FLT_NOT_INITIALIZED = 0xC01C0007,

        /// <summary>
        /// MessageId: STATUS_FLT_FILTER_NOT_READY
        /// MessageText:
        /// The filter is not ready for attachment to volumes because it has not finished
        /// initializing (FltStartFiltering has not been called).
        /// </summary>
        STATUS_FLT_FILTER_NOT_READY = 0xC01C0008,

        /// <summary>
        /// MessageId: STATUS_FLT_POST_OPERATION_CLEANUP
        /// MessageText:
        /// The filter must cleanup any operation specific context at this time because
        /// it is being removed from the system before the operation is completed by
        /// the lower drivers.
        /// </summary>
        STATUS_FLT_POST_OPERATION_CLEANUP = 0xC01C0009,

        /// <summary>
        /// MessageId: STATUS_FLT_INTERNAL_ERROR
        /// MessageText:
        /// The Filter Manager had an internal error from which it cannot recover,
        /// therefore the operation has been failed. This is usually the result
        /// of a filter returning an invalid value from a pre-operation callback.
        /// </summary>
        STATUS_FLT_INTERNAL_ERROR = 0xC01C000A,

        /// <summary>
        /// MessageId: STATUS_FLT_DELETING_OBJECT
        /// MessageText:
        /// The object specified for this action is in the process of being
        /// deleted, therefore the action requested cannot be completed at
        /// this time.
        /// </summary>
        STATUS_FLT_DELETING_OBJECT = 0xC01C000B,

        /// <summary>
        /// MessageId: STATUS_FLT_MUST_BE_NONPAGED_POOL
        /// MessageText:
        /// Non-paged pool must be used for this type of context.
        /// </summary>
        STATUS_FLT_MUST_BE_NONPAGED_POOL = 0xC01C000C,

        /// <summary>
        /// MessageId: STATUS_FLT_DUPLICATE_ENTRY
        /// MessageText:
        /// A duplicate handler definition has been provided for an operation.
        /// </summary>
        STATUS_FLT_DUPLICATE_ENTRY = 0xC01C000D,

        /// <summary>
        /// MessageId: STATUS_FLT_CBDQ_DISABLED
        /// MessageText:
        /// The callback data queue has been disabled.
        /// </summary>
        STATUS_FLT_CBDQ_DISABLED = 0xC01C000E,

        /// <summary>
        /// MessageId: STATUS_FLT_DO_NOT_ATTACH
        /// MessageText:
        /// Do not attach the filter to the volume at this time.
        /// </summary>
        STATUS_FLT_DO_NOT_ATTACH = 0xC01C000F,

        /// <summary>
        /// MessageId: STATUS_FLT_DO_NOT_DETACH
        /// MessageText:
        /// Do not detach the filter from the volume at this time.
        /// </summary>
        STATUS_FLT_DO_NOT_DETACH = 0xC01C0010,

        /// <summary>
        /// MessageId: STATUS_FLT_INSTANCE_ALTITUDE_COLLISION
        /// MessageText:
        /// An instance already exists at this altitude on the volume specified.
        /// </summary>
        STATUS_FLT_INSTANCE_ALTITUDE_COLLISION = 0xC01C0011,

        /// <summary>
        /// MessageId: STATUS_FLT_INSTANCE_NAME_COLLISION
        /// MessageText:
        /// An instance already exists with this name on the volume specified.
        /// </summary>
        STATUS_FLT_INSTANCE_NAME_COLLISION = 0xC01C0012,

        /// <summary>
        /// MessageId: STATUS_FLT_FILTER_NOT_FOUND
        /// MessageText:
        /// The system could not find the filter specified.
        /// </summary>
        STATUS_FLT_FILTER_NOT_FOUND = 0xC01C0013,

        /// <summary>
        /// MessageId: STATUS_FLT_VOLUME_NOT_FOUND
        /// MessageText:
        /// The system could not find the volume specified.
        /// </summary>
        STATUS_FLT_VOLUME_NOT_FOUND = 0xC01C0014,

        /// <summary>
        /// MessageId: STATUS_FLT_INSTANCE_NOT_FOUND
        /// MessageText:
        /// The system could not find the instance specified.
        /// </summary>
        STATUS_FLT_INSTANCE_NOT_FOUND = 0xC01C0015,

        /// <summary>
        /// MessageId: STATUS_FLT_CONTEXT_ALLOCATION_NOT_FOUND
        /// MessageText:
        /// No registered context allocation definition was found for the given request.
        /// </summary>
        STATUS_FLT_CONTEXT_ALLOCATION_NOT_FOUND = 0xC01C0016,

        /// <summary>
        /// MessageId: STATUS_FLT_INVALID_CONTEXT_REGISTRATION
        /// MessageText:
        /// An invalid parameter was specified during context registration.
        /// </summary>
        STATUS_FLT_INVALID_CONTEXT_REGISTRATION = 0xC01C0017,

        /// <summary>
        /// MessageId: STATUS_FLT_NAME_CACHE_MISS
        /// MessageText:
        /// The name requested was not found in Filter Manager's name cache and could not be retrieved from the file system.
        /// </summary>
        STATUS_FLT_NAME_CACHE_MISS = 0xC01C0018,

        /// <summary>
        /// MessageId: STATUS_FLT_NO_DEVICE_OBJECT
        /// MessageText:
        /// The requested device object does not exist for the given volume.
        /// </summary>
        STATUS_FLT_NO_DEVICE_OBJECT = 0xC01C0019,

        /// <summary>
        /// MessageId: STATUS_FLT_VOLUME_ALREADY_MOUNTED
        /// MessageText:
        /// The specified volume is already mounted.
        /// </summary>
        STATUS_FLT_VOLUME_ALREADY_MOUNTED = 0xC01C001A,

        /// <summary>
        /// MessageId: STATUS_FLT_ALREADY_ENLISTED
        /// MessageText:
        /// The specified Transaction Context is already enlisted in a transaction
        /// </summary>
        STATUS_FLT_ALREADY_ENLISTED = 0xC01C001B,

        /// <summary>
        /// MessageId: STATUS_FLT_CONTEXT_ALREADY_LINKED
        /// MessageText:
        /// The specific context is already attached to another object
        /// </summary>
        STATUS_FLT_CONTEXT_ALREADY_LINKED = 0xC01C001C,

        /// <summary>
        /// MessageId: STATUS_FLT_NO_WAITER_FOR_REPLY
        /// MessageText:
        /// No waiter is present for the filter's reply to this message.
        /// </summary>
        STATUS_FLT_NO_WAITER_FOR_REPLY = 0xC01C0020,

        /// <summary>
        /// Side-by-side (SXS) error values
        /// MessageId: STATUS_SXS_SECTION_NOT_FOUND
        /// MessageText:
        /// The requested section is not present in the activation context.
        /// </summary>
        STATUS_SXS_SECTION_NOT_FOUND = 0xC0150001,

        /// <summary>
        /// MessageId: STATUS_SXS_CANT_GEN_ACTCTX
        /// MessageText:
        /// Windows was not able to process the application binding information.
        /// Please refer to your System Event Log for further information.
        /// </summary>
        STATUS_SXS_CANT_GEN_ACTCTX = 0xC0150002,

        /// <summary>
        /// MessageId: STATUS_SXS_INVALID_ACTCTXDATA_FORMAT
        /// MessageText:
        /// The application binding data format is invalid.
        /// </summary>
        STATUS_SXS_INVALID_ACTCTXDATA_FORMAT = 0xC0150003,

        /// <summary>
        /// MessageId: STATUS_SXS_ASSEMBLY_NOT_FOUND
        /// MessageText:
        /// The referenced assembly is not installed on your system.
        /// </summary>
        STATUS_SXS_ASSEMBLY_NOT_FOUND = 0xC0150004,

        /// <summary>
        /// MessageId: STATUS_SXS_MANIFEST_FORMAT_ERROR
        /// MessageText:
        /// The manifest file does not begin with the required tag and format information.
        /// </summary>
        STATUS_SXS_MANIFEST_FORMAT_ERROR = 0xC0150005,

        /// <summary>
        /// MessageId: STATUS_SXS_MANIFEST_PARSE_ERROR
        /// MessageText:
        /// The manifest file contains one or more syntax errors.
        /// </summary>
        STATUS_SXS_MANIFEST_PARSE_ERROR = 0xC0150006,

        /// <summary>
        /// MessageId: STATUS_SXS_ACTIVATION_CONTEXT_DISABLED
        /// MessageText:
        /// The application attempted to activate a disabled activation context.
        /// </summary>
        STATUS_SXS_ACTIVATION_CONTEXT_DISABLED = 0xC0150007,

        /// <summary>
        /// MessageId: STATUS_SXS_KEY_NOT_FOUND
        /// MessageText:
        /// The requested lookup key was not found in any active activation context.
        /// </summary>
        STATUS_SXS_KEY_NOT_FOUND = 0xC0150008,

        /// <summary>
        /// MessageId: STATUS_SXS_VERSION_CONFLICT
        /// MessageText:
        /// A component version required by the application conflicts with another component version already active.
        /// </summary>
        STATUS_SXS_VERSION_CONFLICT = 0xC0150009,

        /// <summary>
        /// MessageId: STATUS_SXS_WRONG_SECTION_TYPE
        /// MessageText:
        /// The type requested activation context section does not match the query API used.
        /// </summary>
        STATUS_SXS_WRONG_SECTION_TYPE = 0xC015000A,

        /// <summary>
        /// MessageId: STATUS_SXS_THREAD_QUERIES_DISABLED
        /// MessageText:
        /// Lack of system resources has required isolated activation to be disabled for the current thread of execution.
        /// </summary>
        STATUS_SXS_THREAD_QUERIES_DISABLED = 0xC015000B,

        /// <summary>
        /// MessageId: STATUS_SXS_ASSEMBLY_MISSING
        /// MessageText:
        /// The referenced assembly could not be found.
        /// </summary>
        STATUS_SXS_ASSEMBLY_MISSING = 0xC015000C,

        /// <summary>
        /// MessageId: STATUS_SXS_RELEASE_ACTIVATION_CONTEXT
        /// MessageText:
        /// A kernel mode component is releasing a reference on an activation context.
        /// </summary>
        STATUS_SXS_RELEASE_ACTIVATION_CONTEXT = 0x4015000D,

        /// <summary>
        /// MessageId: STATUS_SXS_PROCESS_DEFAULT_ALREADY_SET
        /// MessageText:
        /// An attempt to set the process default activation context failed because the process default activation context was already set.
        /// </summary>
        STATUS_SXS_PROCESS_DEFAULT_ALREADY_SET = 0xC015000E,

        /// <summary>
        /// MessageId: STATUS_SXS_EARLY_DEACTIVATION
        /// MessageText:
        /// The activation context being deactivated is not the most recently activated one.
        /// </summary>
        STATUS_SXS_EARLY_DEACTIVATION = 0xC015000F,

        /// <summary>
        /// MessageId: STATUS_SXS_INVALID_DEACTIVATION
        /// MessageText:
        /// The activation context being deactivated is not active for the current thread of execution.
        /// </summary>
        STATUS_SXS_INVALID_DEACTIVATION = 0xC0150010,

        /// <summary>
        /// MessageId: STATUS_SXS_MULTIPLE_DEACTIVATION
        /// MessageText:
        /// The activation context being deactivated has already been deactivated.
        /// </summary>
        STATUS_SXS_MULTIPLE_DEACTIVATION = 0xC0150011,

        /// <summary>
        /// MessageId: STATUS_SXS_SYSTEM_DEFAULT_ACTIVATION_CONTEXT_EMPTY
        /// MessageText:
        /// The activation context of system default assembly could not be generated.
        /// </summary>
        STATUS_SXS_SYSTEM_DEFAULT_ACTIVATION_CONTEXT_EMPTY = 0xC0150012,

        /// <summary>
        /// MessageId: STATUS_SXS_PROCESS_TERMINATION_REQUESTED
        /// MessageText:
        /// A component used by the isolation facility has requested to terminate the process.
        /// </summary>
        STATUS_SXS_PROCESS_TERMINATION_REQUESTED = 0xC0150013,

        /// <summary>
        /// MessageId: STATUS_SXS_CORRUPT_ACTIVATION_STACK
        /// MessageText:
        /// The activation context activation stack for the running thread of execution is corrupt.
        /// </summary>
        STATUS_SXS_CORRUPT_ACTIVATION_STACK = 0xC0150014,

        /// <summary>
        /// MessageId: STATUS_SXS_CORRUPTION
        /// MessageText:
        /// The application isolation metadata for this process or thread has become corrupt.
        /// </summary>
        STATUS_SXS_CORRUPTION = 0xC0150015,

        /// <summary>
        /// MessageId: STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_VALUE
        /// MessageText:
        /// The value of an attribute in an identity is not within the legal range.
        /// </summary>
        STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_VALUE = 0xC0150016,

        /// <summary>
        /// MessageId: STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_NAME
        /// MessageText:
        /// The name of an attribute in an identity is not within the legal range.
        /// </summary>
        STATUS_SXS_INVALID_IDENTITY_ATTRIBUTE_NAME = 0xC0150017,

        /// <summary>
        /// MessageId: STATUS_SXS_IDENTITY_DUPLICATE_ATTRIBUTE
        /// MessageText:
        /// An identity contains two definitions for the same attribute.
        /// </summary>
        STATUS_SXS_IDENTITY_DUPLICATE_ATTRIBUTE = 0xC0150018,

        /// <summary>
        /// MessageId: STATUS_SXS_IDENTITY_PARSE_ERROR
        /// MessageText:
        /// The identity string is malformed. This may be due to a trailing comma, more than two unnamed attributes, missing attribute name or missing attribute value.
        /// </summary>
        STATUS_SXS_IDENTITY_PARSE_ERROR = 0xC0150019,

        /// <summary>
        /// MessageId: STATUS_SXS_COMPONENT_STORE_CORRUPT
        /// MessageText:
        /// The component store has been corrupted.
        /// </summary>
        STATUS_SXS_COMPONENT_STORE_CORRUPT = 0xC015001A,

        /// <summary>
        /// MessageId: STATUS_SXS_FILE_HASH_MISMATCH
        /// MessageText:
        /// A component's file does not match the verification information present in the component manifest.
        /// </summary>
        STATUS_SXS_FILE_HASH_MISMATCH = 0xC015001B,

        /// <summary>
        /// MessageId: STATUS_SXS_MANIFEST_IDENTITY_SAME_BUT_CONTENTS_DIFFERENT
        /// MessageText:
        /// The identities of the manifests are identical but their contents are different.
        /// </summary>
        STATUS_SXS_MANIFEST_IDENTITY_SAME_BUT_CONTENTS_DIFFERENT = 0xC015001C,

        /// <summary>
        /// MessageId: STATUS_SXS_IDENTITIES_DIFFERENT
        /// MessageText:
        /// The component identities are different.
        /// </summary>
        STATUS_SXS_IDENTITIES_DIFFERENT = 0xC015001D,

        /// <summary>
        /// MessageId: STATUS_SXS_ASSEMBLY_IS_NOT_A_DEPLOYMENT
        /// MessageText:
        /// The assembly is not a deployment.
        /// </summary>
        STATUS_SXS_ASSEMBLY_IS_NOT_A_DEPLOYMENT = 0xC015001E,

        /// <summary>
        /// MessageId: STATUS_SXS_FILE_NOT_PART_OF_ASSEMBLY
        /// MessageText:
        /// The file is not a part of the assembly.
        /// </summary>
        STATUS_SXS_FILE_NOT_PART_OF_ASSEMBLY = 0xC015001F,

        /// <summary>
        /// MessageId: STATUS_ADVANCED_INSTALLER_FAILED
        /// MessageText:
        /// An advanced installer failed during setup or servicing.
        /// </summary>
        STATUS_ADVANCED_INSTALLER_FAILED = 0xC0150020,

        /// <summary>
        /// MessageId: STATUS_XML_ENCODING_MISMATCH
        /// MessageText:
        /// The character encoding in the XML declaration did not match the encoding used in the document.
        /// </summary>
        STATUS_XML_ENCODING_MISMATCH = 0xC0150021,

        /// <summary>
        /// MessageId: STATUS_SXS_MANIFEST_TOO_BIG
        /// MessageText:
        /// The size of the manifest exceeds the maximum allowed.
        /// </summary>
        STATUS_SXS_MANIFEST_TOO_BIG = 0xC0150022,

        /// <summary>
        /// MessageId: STATUS_SXS_SETTING_NOT_REGISTERED
        /// MessageText:
        /// The setting is not registered.
        /// </summary>
        STATUS_SXS_SETTING_NOT_REGISTERED = 0xC0150023,

        /// <summary>
        /// MessageId: STATUS_SXS_TRANSACTION_CLOSURE_INCOMPLETE
        /// MessageText:
        /// One or more required members of the transaction are not present.
        /// </summary>
        STATUS_SXS_TRANSACTION_CLOSURE_INCOMPLETE = 0xC0150024,

        /// <summary>
        /// MessageId: STATUS_SMI_PRIMITIVE_INSTALLER_FAILED
        /// MessageText:
        /// The SMI primitive installer failed during setup or servicing.
        /// </summary>
        STATUS_SMI_PRIMITIVE_INSTALLER_FAILED = 0xC0150025,

        /// <summary>
        /// MessageId: STATUS_GENERIC_COMMAND_FAILED
        /// MessageText:
        /// A generic command executable returned a result that indicates failure.
        /// </summary>
        STATUS_GENERIC_COMMAND_FAILED = 0xC0150026,

        /// <summary>
        /// MessageId: STATUS_SXS_FILE_HASH_MISSING
        /// MessageText:
        /// A component is missing file verification information in its manifest.
        /// </summary>
        STATUS_SXS_FILE_HASH_MISSING = 0xC0150027,

        /// <summary>
        ///  Cluster error values
        /// MessageId: STATUS_CLUSTER_INVALID_NODE
        /// MessageText:
        /// The cluster node is not valid.
        /// </summary>
        STATUS_CLUSTER_INVALID_NODE = 0xC0130001,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_EXISTS
        /// MessageText:
        /// The cluster node already exists.
        /// </summary>
        STATUS_CLUSTER_NODE_EXISTS = 0xC0130002,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_JOIN_IN_PROGRESS
        /// MessageText:
        /// A node is in the process of joining the cluster.
        /// </summary>
        STATUS_CLUSTER_JOIN_IN_PROGRESS = 0xC0130003,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_NOT_FOUND
        /// MessageText:
        /// The cluster node was not found.
        /// </summary>
        STATUS_CLUSTER_NODE_NOT_FOUND = 0xC0130004,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_LOCAL_NODE_NOT_FOUND
        /// MessageText:
        /// The cluster local node information was not found.
        /// </summary>
        STATUS_CLUSTER_LOCAL_NODE_NOT_FOUND = 0xC0130005,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NETWORK_EXISTS
        /// MessageText:
        /// The cluster network already exists.
        /// </summary>
        STATUS_CLUSTER_NETWORK_EXISTS = 0xC0130006,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NETWORK_NOT_FOUND
        /// MessageText:
        /// The cluster network was not found.
        /// </summary>
        STATUS_CLUSTER_NETWORK_NOT_FOUND = 0xC0130007,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NETINTERFACE_EXISTS
        /// MessageText:
        /// The cluster network interface already exists.
        /// </summary>
        STATUS_CLUSTER_NETINTERFACE_EXISTS = 0xC0130008,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NETINTERFACE_NOT_FOUND
        /// MessageText:
        /// The cluster network interface was not found.
        /// </summary>
        STATUS_CLUSTER_NETINTERFACE_NOT_FOUND = 0xC0130009,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_INVALID_REQUEST
        /// MessageText:
        /// The cluster request is not valid for this object.
        /// </summary>
        STATUS_CLUSTER_INVALID_REQUEST = 0xC013000A,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_INVALID_NETWORK_PROVIDER
        /// MessageText:
        /// The cluster network provider is not valid.
        /// </summary>
        STATUS_CLUSTER_INVALID_NETWORK_PROVIDER = 0xC013000B,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_DOWN
        /// MessageText:
        /// The cluster node is down.
        /// </summary>
        STATUS_CLUSTER_NODE_DOWN = 0xC013000C,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_UNREACHABLE
        /// MessageText:
        /// The cluster node is not reachable.
        /// </summary>
        STATUS_CLUSTER_NODE_UNREACHABLE = 0xC013000D,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_NOT_MEMBER
        /// MessageText:
        /// The cluster node is not a member of the cluster.
        /// </summary>
        STATUS_CLUSTER_NODE_NOT_MEMBER = 0xC013000E,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_JOIN_NOT_IN_PROGRESS
        /// MessageText:
        /// A cluster join operation is not in progress.
        /// </summary>
        STATUS_CLUSTER_JOIN_NOT_IN_PROGRESS = 0xC013000F,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_INVALID_NETWORK
        /// MessageText:
        /// The cluster network is not valid.
        /// </summary>
        STATUS_CLUSTER_INVALID_NETWORK = 0xC0130010,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NO_NET_ADAPTERS
        /// MessageText:
        /// No network adapters are available.
        /// </summary>
        STATUS_CLUSTER_NO_NET_ADAPTERS = 0xC0130011,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_UP
        /// MessageText:
        /// The cluster node is up.
        /// </summary>
        STATUS_CLUSTER_NODE_UP = 0xC0130012,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_PAUSED
        /// MessageText:
        /// The cluster node is paused.
        /// </summary>
        STATUS_CLUSTER_NODE_PAUSED = 0xC0130013,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NODE_NOT_PAUSED
        /// MessageText:
        /// The cluster node is not paused.
        /// </summary>
        STATUS_CLUSTER_NODE_NOT_PAUSED = 0xC0130014,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NO_SECURITY_CONTEXT
        /// MessageText:
        /// No cluster security context is available.
        /// </summary>
        STATUS_CLUSTER_NO_SECURITY_CONTEXT = 0xC0130015,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_NETWORK_NOT_INTERNAL
        /// MessageText:
        /// The cluster network is not configured for internal cluster communication.
        /// </summary>
        STATUS_CLUSTER_NETWORK_NOT_INTERNAL = 0xC0130016,

        /// <summary>
        /// MessageId: STATUS_CLUSTER_POISONED
        /// MessageText:
        /// The cluster node has been poisoned.
        /// </summary>
        STATUS_CLUSTER_POISONED = 0xC0130017,

        /// <summary>
        ///  Transaction Manager error values
        /// MessageId: STATUS_TRANSACTIONAL_CONFLICT
        /// MessageText:
        /// The function attempted to use a name that is reserved for use by another transaction.
        /// </summary>
        STATUS_TRANSACTIONAL_CONFLICT = 0xC0190001,

        /// <summary>
        /// MessageId: STATUS_INVALID_TRANSACTION
        /// MessageText:
        /// The transaction handle associated with this operation is not valid.
        /// </summary>
        STATUS_INVALID_TRANSACTION = 0xC0190002,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_NOT_ACTIVE
        /// MessageText:
        /// The requested operation was made in the context of a transaction that is no longer active.
        /// </summary>
        STATUS_TRANSACTION_NOT_ACTIVE = 0xC0190003,

        /// <summary>
        /// MessageId: STATUS_TM_INITIALIZATION_FAILED
        /// MessageText:
        /// The Transaction Manager was unable to be successfully initialized.  Transacted operations are not supported.
        /// </summary>
        STATUS_TM_INITIALIZATION_FAILED = 0xC0190004,

        /// <summary>
        /// MessageId: STATUS_RM_NOT_ACTIVE
        /// MessageText:
        /// Transaction support within the specified file system resource manager is not started or was shutdown due to an error.
        /// </summary>
        STATUS_RM_NOT_ACTIVE = 0xC0190005,

        /// <summary>
        /// MessageId: STATUS_RM_METADATA_CORRUPT
        /// MessageText:
        /// The metadata of the RM has been corrupted. The RM will not function.
        /// </summary>
        STATUS_RM_METADATA_CORRUPT = 0xC0190006,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_NOT_JOINED
        /// MessageText:
        /// The resource manager has attempted to prepare a transaction that it has not successfully joined.
        /// </summary>
        STATUS_TRANSACTION_NOT_JOINED = 0xC0190007,

        /// <summary>
        /// MessageId: STATUS_DIRECTORY_NOT_RM
        /// MessageText:
        /// The specified directory does not contain a file system resource manager.
        /// </summary>
        STATUS_DIRECTORY_NOT_RM = 0xC0190008,

        /// <summary>
        /// MessageId: STATUS_COULD_NOT_RESIZE_LOG
        /// MessageText:
        /// The log could not be set to the requested size.
        /// </summary>
        STATUS_COULD_NOT_RESIZE_LOG = 0x80190009,

        /// <summary>
        /// MessageId: STATUS_TRANSACTIONS_UNSUPPORTED_REMOTE
        /// MessageText:
        /// The remote server or share does not support transacted file operations.
        /// </summary>
        STATUS_TRANSACTIONS_UNSUPPORTED_REMOTE = 0xC019000A,

        /// <summary>
        /// MessageId: STATUS_LOG_RESIZE_INVALID_SIZE
        /// MessageText:
        /// The requested log size for the file system resource manager is invalid.
        /// </summary>
        STATUS_LOG_RESIZE_INVALID_SIZE = 0xC019000B,

        /// <summary>
        /// MessageId: STATUS_REMOTE_FILE_VERSION_MISMATCH
        /// MessageText:
        /// The remote server sent mismatching version number or Fid for a file opened with transactions.
        /// </summary>
        STATUS_REMOTE_FILE_VERSION_MISMATCH = 0xC019000C,

        /// <summary>
        /// MessageId: STATUS_CRM_PROTOCOL_ALREADY_EXISTS
        /// MessageText:
        /// The RM tried to register a protocol that already exists.
        /// </summary>
        STATUS_CRM_PROTOCOL_ALREADY_EXISTS = 0xC019000F,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_PROPAGATION_FAILED
        /// MessageText:
        /// The attempt to propagate the Transaction failed.
        /// </summary>
        STATUS_TRANSACTION_PROPAGATION_FAILED = 0xC0190010,

        /// <summary>
        /// MessageId: STATUS_CRM_PROTOCOL_NOT_FOUND
        /// MessageText:
        /// The requested propagation protocol was not registered as a CRM.
        /// </summary>
        STATUS_CRM_PROTOCOL_NOT_FOUND = 0xC0190011,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_SUPERIOR_EXISTS
        /// MessageText:
        /// The Transaction object already has a superior enlistment, and the caller attempted an operation that would have created a new superior.  Only a single superior enlistment is allowed.
        /// </summary>
        STATUS_TRANSACTION_SUPERIOR_EXISTS = 0xC0190012,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_REQUEST_NOT_VALID
        /// MessageText:
        /// The requested operation is not valid on the Transaction object in its current state.
        /// </summary>
        STATUS_TRANSACTION_REQUEST_NOT_VALID = 0xC0190013,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_NOT_REQUESTED
        /// MessageText:
        /// The caller has called a response API, but the response is not expected because the TM did not issue the corresponding request to the caller.
        /// </summary>
        STATUS_TRANSACTION_NOT_REQUESTED = 0xC0190014,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_ALREADY_ABORTED
        /// MessageText:
        /// It is too late to perform the requested operation, since the Transaction has already been aborted.
        /// </summary>
        STATUS_TRANSACTION_ALREADY_ABORTED = 0xC0190015,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_ALREADY_COMMITTED
        /// MessageText:
        /// It is too late to perform the requested operation, since the Transaction has already been committed.
        /// </summary>
        STATUS_TRANSACTION_ALREADY_COMMITTED = 0xC0190016,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_INVALID_MARSHALL_BUFFER
        /// MessageText:
        /// The buffer passed in to NtPushTransaction or NtPullTransaction is not in a valid format.
        /// </summary>
        STATUS_TRANSACTION_INVALID_MARSHALL_BUFFER = 0xC0190017,

        /// <summary>
        /// MessageId: STATUS_CURRENT_TRANSACTION_NOT_VALID
        /// MessageText:
        /// The current transaction context associated with the thread is not a valid handle to a transaction object.
        /// </summary>
        STATUS_CURRENT_TRANSACTION_NOT_VALID = 0xC0190018,

        /// <summary>
        /// MessageId: STATUS_LOG_GROWTH_FAILED
        /// MessageText:
        /// An attempt to create space in the transactional resource manager's log failed. The failure status has been recorded in the event log.
        /// </summary>
        STATUS_LOG_GROWTH_FAILED = 0xC0190019,

        /// <summary>
        /// MessageId: STATUS_OBJECT_NO_LONGER_EXISTS
        /// MessageText:
        /// The object (file, stream, link) corresponding to the handle has been deleted by a transaction savepoint rollback.
        /// </summary>
        STATUS_OBJECT_NO_LONGER_EXISTS = 0xC0190021,

        /// <summary>
        /// MessageId: STATUS_STREAM_MINIVERSION_NOT_FOUND
        /// MessageText:
        /// The specified file miniversion was not found for this transacted file open.
        /// </summary>
        STATUS_STREAM_MINIVERSION_NOT_FOUND = 0xC0190022,

        /// <summary>
        /// MessageId: STATUS_STREAM_MINIVERSION_NOT_VALID
        /// MessageText:
        /// The specified file miniversion was found but has been invalidated. Most likely cause is a transaction savepoint rollback.
        /// </summary>
        STATUS_STREAM_MINIVERSION_NOT_VALID = 0xC0190023,

        /// <summary>
        /// MessageId: STATUS_MINIVERSION_INACCESSIBLE_FROM_SPECIFIED_TRANSACTION
        /// MessageText:
        /// A miniversion may only be opened in the context of the transaction that created it.
        /// </summary>
        STATUS_MINIVERSION_INACCESSIBLE_FROM_SPECIFIED_TRANSACTION = 0xC0190024,

        /// <summary>
        /// MessageId: STATUS_CANT_OPEN_MINIVERSION_WITH_MODIFY_INTENT
        /// MessageText:
        /// It is not possible to open a miniversion with modify access.
        /// </summary>
        STATUS_CANT_OPEN_MINIVERSION_WITH_MODIFY_INTENT = 0xC0190025,

        /// <summary>
        /// MessageId: STATUS_CANT_CREATE_MORE_STREAM_MINIVERSIONS
        /// MessageText:
        /// It is not possible to create any more miniversions for this stream.
        /// </summary>
        STATUS_CANT_CREATE_MORE_STREAM_MINIVERSIONS = 0xC0190026,

        /// <summary>
        /// MessageId: STATUS_HANDLE_NO_LONGER_VALID
        /// MessageText:
        /// The handle has been invalidated by a transaction. The most likely cause is the presence of memory mapping on a file or an open handle when the transaction ended or rolled back to savepoint.
        /// </summary>
        STATUS_HANDLE_NO_LONGER_VALID = 0xC0190028,

        /// <summary>
        /// MessageId: STATUS_NO_TXF_METADATA
        /// MessageText:
        /// There is no transaction metadata on the file.
        /// </summary>
        STATUS_NO_TXF_METADATA = 0x80190029,

        /// <summary>
        /// MessageId: STATUS_LOG_CORRUPTION_DETECTED
        /// MessageText:
        /// The log data is corrupt.
        /// </summary>
        STATUS_LOG_CORRUPTION_DETECTED = 0xC0190030,

        /// <summary>
        /// MessageId: STATUS_CANT_RECOVER_WITH_HANDLE_OPEN
        /// MessageText:
        /// The file can't be recovered because there is a handle still open on it.
        /// </summary>
        STATUS_CANT_RECOVER_WITH_HANDLE_OPEN = 0x80190031,

        /// <summary>
        /// MessageId: STATUS_RM_DISCONNECTED
        /// MessageText:
        /// The transaction outcome is unavailable because the resource manager responsible for it has disconnected.
        /// </summary>
        STATUS_RM_DISCONNECTED = 0xC0190032,

        /// <summary>
        /// MessageId: STATUS_ENLISTMENT_NOT_SUPERIOR
        /// MessageText:
        /// The request was rejected because the enlistment in question is not a superior enlistment.
        /// </summary>
        STATUS_ENLISTMENT_NOT_SUPERIOR = 0xC0190033,

        /// <summary>
        /// MessageId: STATUS_RECOVERY_NOT_NEEDED
        /// MessageText:
        /// The transactional resource manager is already consistent.  Recovery is not needed.
        /// </summary>
        STATUS_RECOVERY_NOT_NEEDED = 0x40190034,

        /// <summary>
        /// MessageId: STATUS_RM_ALREADY_STARTED
        /// MessageText:
        /// The transactional resource manager has already been started.
        /// </summary>
        STATUS_RM_ALREADY_STARTED = 0x40190035,

        /// <summary>
        /// MessageId: STATUS_FILE_IDENTITY_NOT_PERSISTENT
        /// MessageText:
        /// The file cannot be opened transactionally, because its identity depends on the outcome of an unresolved transaction.
        /// </summary>
        STATUS_FILE_IDENTITY_NOT_PERSISTENT = 0xC0190036,

        /// <summary>
        /// MessageId: STATUS_CANT_BREAK_TRANSACTIONAL_DEPENDENCY
        /// MessageText:
        /// The operation cannot be performed because another transaction is depending on the fact that this property will not change.
        /// </summary>
        STATUS_CANT_BREAK_TRANSACTIONAL_DEPENDENCY = 0xC0190037,

        /// <summary>
        /// MessageId: STATUS_CANT_CROSS_RM_BOUNDARY
        /// MessageText:
        /// The operation would involve a single file with two transactional resource managers and is therefore not allowed.
        /// </summary>
        STATUS_CANT_CROSS_RM_BOUNDARY = 0xC0190038,

        /// <summary>
        /// MessageId: STATUS_TXF_DIR_NOT_EMPTY
        /// MessageText:
        /// The $Txf directory must be empty for this operation to succeed.
        /// </summary>
        STATUS_TXF_DIR_NOT_EMPTY = 0xC0190039,

        /// <summary>
        /// MessageId: STATUS_INDOUBT_TRANSACTIONS_EXIST
        /// MessageText:
        /// The operation would leave a transactional resource manager in an inconsistent state and is therefore not allowed.
        /// </summary>
        STATUS_INDOUBT_TRANSACTIONS_EXIST = 0xC019003A,

        /// <summary>
        /// MessageId: STATUS_TM_VOLATILE
        /// MessageText:
        /// The operation could not be completed because the transaction manager does not have a log.
        /// </summary>
        STATUS_TM_VOLATILE = 0xC019003B,

        /// <summary>
        /// MessageId: STATUS_ROLLBACK_TIMER_EXPIRED
        /// MessageText:
        /// A rollback could not be scheduled because a previously scheduled rollback has already executed or been queued for execution.
        /// </summary>
        STATUS_ROLLBACK_TIMER_EXPIRED = 0xC019003C,

        /// <summary>
        /// MessageId: STATUS_TXF_ATTRIBUTE_CORRUPT
        /// MessageText:
        /// The transactional metadata attribute on the file or directory %hs is corrupt and unreadable.
        /// </summary>
        STATUS_TXF_ATTRIBUTE_CORRUPT = 0xC019003D,

        /// <summary>
        /// MessageId: STATUS_EFS_NOT_ALLOWED_IN_TRANSACTION
        /// MessageText:
        /// The encryption operation could not be completed because a transaction is active.
        /// </summary>
        STATUS_EFS_NOT_ALLOWED_IN_TRANSACTION = 0xC019003E,

        /// <summary>
        /// MessageId: STATUS_TRANSACTIONAL_OPEN_NOT_ALLOWED
        /// MessageText:
        /// This object is not allowed to be opened in a transaction.
        /// </summary>
        STATUS_TRANSACTIONAL_OPEN_NOT_ALLOWED = 0xC019003F,

        /// <summary>
        /// MessageId: STATUS_TRANSACTED_MAPPING_UNSUPPORTED_REMOTE
        /// MessageText:
        /// Memory mapping (creating a mapped section) a remote file under a transaction is not supported.
        /// </summary>
        STATUS_TRANSACTED_MAPPING_UNSUPPORTED_REMOTE = 0xC0190040,

        /// <summary>
        /// MessageId: STATUS_TXF_METADATA_ALREADY_PRESENT
        /// MessageText:
        /// Transaction metadata is already present on this file and cannot be superseded.
        /// </summary>
        STATUS_TXF_METADATA_ALREADY_PRESENT = 0x80190041,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_SCOPE_CALLBACKS_NOT_SET
        /// MessageText:
        /// A transaction scope could not be entered because the scope handler has not been initialized.
        /// </summary>
        STATUS_TRANSACTION_SCOPE_CALLBACKS_NOT_SET = 0x80190042,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_REQUIRED_PROMOTION
        /// MessageText:
        /// Promotion was required in order to allow the resource manager to enlist, but the transaction was set to disallow it.
        /// </summary>
        STATUS_TRANSACTION_REQUIRED_PROMOTION = 0xC0190043,

        /// <summary>
        /// MessageId: STATUS_CANNOT_EXECUTE_FILE_IN_TRANSACTION
        /// MessageText:
        /// This file is open for modification in an unresolved transaction and may be opened for execute only by a transacted reader.
        /// </summary>
        STATUS_CANNOT_EXECUTE_FILE_IN_TRANSACTION = 0xC0190044,

        /// <summary>
        /// MessageId: STATUS_TRANSACTIONS_NOT_FROZEN
        /// MessageText:
        /// The request to thaw frozen transactions was ignored because transactions had not previously been frozen.
        /// </summary>
        STATUS_TRANSACTIONS_NOT_FROZEN = 0xC0190045,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_FREEZE_IN_PROGRESS
        /// MessageText:
        /// Transactions cannot be frozen because a freeze is already in progress.
        /// </summary>
        STATUS_TRANSACTION_FREEZE_IN_PROGRESS = 0xC0190046,

        /// <summary>
        /// MessageId: STATUS_NOT_SNAPSHOT_VOLUME
        /// MessageText:
        /// The target volume is not a snapshot volume. This operation is only valid on a volume mounted as a snapshot.
        /// </summary>
        STATUS_NOT_SNAPSHOT_VOLUME = 0xC0190047,

        /// <summary>
        /// MessageId: STATUS_NO_SAVEPOINT_WITH_OPEN_FILES
        /// MessageText:
        /// The savepoint operation failed because files are open on the transaction. This is not permitted.
        /// </summary>
        STATUS_NO_SAVEPOINT_WITH_OPEN_FILES = 0xC0190048,

        /// <summary>
        /// MessageId: STATUS_SPARSE_NOT_ALLOWED_IN_TRANSACTION
        /// MessageText:
        /// The sparse operation could not be completed because a transaction is active on the file.
        /// </summary>
        STATUS_SPARSE_NOT_ALLOWED_IN_TRANSACTION = 0xC0190049,

        /// <summary>
        /// MessageId: STATUS_TM_IDENTITY_MISMATCH
        /// MessageText:
        /// The call to create a TransactionManager object failed because the Tm Identity stored in the logfile does not match the Tm Identity that was passed in as an argument.
        /// </summary>
        STATUS_TM_IDENTITY_MISMATCH = 0xC019004A,

        /// <summary>
        /// MessageId: STATUS_FLOATED_SECTION
        /// MessageText:
        /// I/O was attempted on a section object that has been floated as a result of a transaction ending.  There is no valid data.
        /// </summary>
        STATUS_FLOATED_SECTION = 0xC019004B,

        /// <summary>
        /// MessageId: STATUS_CANNOT_ACCEPT_TRANSACTED_WORK
        /// MessageText:
        /// The transactional resource manager cannot currently accept transacted work due to a transient condition such as low resources.
        /// </summary>
        STATUS_CANNOT_ACCEPT_TRANSACTED_WORK = 0xC019004C,

        /// <summary>
        /// MessageId: STATUS_CANNOT_ABORT_TRANSACTIONS
        /// MessageText:
        /// The transactional resource manager had too many tranactions outstanding that could not be aborted. The transactional resource manger has been shut down.
        /// </summary>
        STATUS_CANNOT_ABORT_TRANSACTIONS = 0xC019004D,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_NOT_FOUND
        /// MessageText:
        /// The specified Transaction was unable to be opened, because it was not found.
        /// </summary>
        STATUS_TRANSACTION_NOT_FOUND = 0xC019004E,

        /// <summary>
        /// MessageId: STATUS_RESOURCEMANAGER_NOT_FOUND
        /// MessageText:
        /// The specified ResourceManager was unable to be opened, because it was not found.
        /// </summary>
        STATUS_RESOURCEMANAGER_NOT_FOUND = 0xC019004F,

        /// <summary>
        /// MessageId: STATUS_ENLISTMENT_NOT_FOUND
        /// MessageText:
        /// The specified Enlistment was unable to be opened, because it was not found.
        /// </summary>
        STATUS_ENLISTMENT_NOT_FOUND = 0xC0190050,

        /// <summary>
        /// MessageId: STATUS_TRANSACTIONMANAGER_NOT_FOUND
        /// MessageText:
        /// The specified TransactionManager was unable to be opened, because it was not found.
        /// </summary>
        STATUS_TRANSACTIONMANAGER_NOT_FOUND = 0xC0190051,

        /// <summary>
        /// MessageId: STATUS_TRANSACTIONMANAGER_NOT_ONLINE
        /// MessageText:
        /// The specified ResourceManager was unable to create an enlistment, because its associated TransactionManager is not online.
        /// </summary>
        STATUS_TRANSACTIONMANAGER_NOT_ONLINE = 0xC0190052,

        /// <summary>
        /// MessageId: STATUS_TRANSACTIONMANAGER_RECOVERY_NAME_COLLISION
        /// MessageText:
        /// The specified TransactionManager was unable to create the objects contained in its logfile in the Ob namespace.  Therefore, the TransactionManager was unable to recover.
        /// </summary>
        STATUS_TRANSACTIONMANAGER_RECOVERY_NAME_COLLISION = 0xC0190053,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_NOT_ROOT
        /// MessageText:
        /// The call to create a superior Enlistment on this Transaction object could not be completed, because the Transaction object specified for the enlistment is a subordinate branch of the Transaction.  Only the root of the Transaction can be enlisted on as a superior.
        /// </summary>
        STATUS_TRANSACTION_NOT_ROOT = 0xC0190054,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_OBJECT_EXPIRED
        /// MessageText:
        /// Because the associated transaction manager or resource manager has been closed, the handle is no longer valid.
        /// </summary>
        STATUS_TRANSACTION_OBJECT_EXPIRED = 0xC0190055,

        /// <summary>
        /// MessageId: STATUS_COMPRESSION_NOT_ALLOWED_IN_TRANSACTION
        /// MessageText:
        /// The compression operation could not be completed because a transaction is active on the file.
        /// </summary>
        STATUS_COMPRESSION_NOT_ALLOWED_IN_TRANSACTION = 0xC0190056,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_RESPONSE_NOT_ENLISTED
        /// MessageText:
        /// The specified operation could not be performed on this Superior enlistment, because the enlistment was not created with the corresponding completion response in the NotificationMask.
        /// </summary>
        STATUS_TRANSACTION_RESPONSE_NOT_ENLISTED = 0xC0190057,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_RECORD_TOO_LONG
        /// MessageText:
        /// The specified operation could not be performed, because the record that would be logged was too long. This can occur because of two conditions:  either there are too many Enlistments on this Transaction, or the combined RecoveryInformation being logged on behalf of those Enlistments is too long.
        /// </summary>
        STATUS_TRANSACTION_RECORD_TOO_LONG = 0xC0190058,

        /// <summary>
        /// MessageId: STATUS_NO_LINK_TRACKING_IN_TRANSACTION
        /// MessageText:
        /// The link tracking operation could not be completed because a transaction is active.
        /// </summary>
        STATUS_NO_LINK_TRACKING_IN_TRANSACTION = 0xC0190059,

        /// <summary>
        /// MessageId: STATUS_OPERATION_NOT_SUPPORTED_IN_TRANSACTION
        /// MessageText:
        /// This operation cannot be performed in a transaction.
        /// </summary>
        STATUS_OPERATION_NOT_SUPPORTED_IN_TRANSACTION = 0xC019005A,

        /// <summary>
        /// MessageId: STATUS_TRANSACTION_INTEGRITY_VIOLATED
        /// MessageText:
        /// The kernel transaction manager had to abort or forget the transaction because it blocked forward progress.
        /// </summary>
        STATUS_TRANSACTION_INTEGRITY_VIOLATED = 0xC019005B,

        /// <summary>
        ///  CLFS (common log file system) error values
        /// MessageId: STATUS_LOG_SECTOR_INVALID
        /// MessageText:
        /// Log service found an invalid log sector.
        /// </summary>
        STATUS_LOG_SECTOR_INVALID = 0xC01A0001,

        /// <summary>
        /// MessageId: STATUS_LOG_SECTOR_PARITY_INVALID
        /// MessageText:
        /// Log service encountered a log sector with invalid block parity.
        /// </summary>
        STATUS_LOG_SECTOR_PARITY_INVALID = 0xC01A0002,

        /// <summary>
        /// MessageId: STATUS_LOG_SECTOR_REMAPPED
        /// MessageText:
        /// Log service encountered a remapped log sector.
        /// </summary>
        STATUS_LOG_SECTOR_REMAPPED = 0xC01A0003,

        /// <summary>
        /// MessageId: STATUS_LOG_BLOCK_INCOMPLETE
        /// MessageText:
        /// Log service encountered a partial or incomplete log block.
        /// </summary>
        STATUS_LOG_BLOCK_INCOMPLETE = 0xC01A0004,

        /// <summary>
        /// MessageId: STATUS_LOG_INVALID_RANGE
        /// MessageText:
        /// Log service encountered an attempt access data outside the active log range.
        /// </summary>
        STATUS_LOG_INVALID_RANGE = 0xC01A0005,

        /// <summary>
        /// MessageId: STATUS_LOG_BLOCKS_EXHAUSTED
        /// MessageText:
        /// Log service user log marshalling buffers are exhausted.
        /// </summary>
        STATUS_LOG_BLOCKS_EXHAUSTED = 0xC01A0006,

        /// <summary>
        /// MessageId: STATUS_LOG_READ_CONTEXT_INVALID
        /// MessageText:
        /// Log service encountered an attempt read from a marshalling area with an invalid read context.
        /// </summary>
        STATUS_LOG_READ_CONTEXT_INVALID = 0xC01A0007,

        /// <summary>
        /// MessageId: STATUS_LOG_RESTART_INVALID
        /// MessageText:
        /// Log service encountered an invalid log restart area.
        /// </summary>
        STATUS_LOG_RESTART_INVALID = 0xC01A0008,

        /// <summary>
        /// MessageId: STATUS_LOG_BLOCK_VERSION
        /// MessageText:
        /// Log service encountered an invalid log block version.
        /// </summary>
        STATUS_LOG_BLOCK_VERSION = 0xC01A0009,

        /// <summary>
        /// MessageId: STATUS_LOG_BLOCK_INVALID
        /// MessageText:
        /// Log service encountered an invalid log block.
        /// </summary>
        STATUS_LOG_BLOCK_INVALID = 0xC01A000A,

        /// <summary>
        /// MessageId: STATUS_LOG_READ_MODE_INVALID
        /// MessageText:
        /// Log service encountered an attempt to read the log with an invalid read mode.
        /// </summary>
        STATUS_LOG_READ_MODE_INVALID = 0xC01A000B,

        /// <summary>
        /// MessageId: STATUS_LOG_NO_RESTART
        /// MessageText:
        /// Log service encountered a log stream with no restart area.
        /// </summary>
        STATUS_LOG_NO_RESTART = 0x401A000C,

        /// <summary>
        /// MessageId: STATUS_LOG_METADATA_CORRUPT
        /// MessageText:
        /// Log service encountered a corrupted metadata file.
        /// </summary>
        STATUS_LOG_METADATA_CORRUPT = 0xC01A000D,

        /// <summary>
        /// MessageId: STATUS_LOG_METADATA_INVALID
        /// MessageText:
        /// Log service encountered a metadata file that could not be created by the log file system.
        /// </summary>
        STATUS_LOG_METADATA_INVALID = 0xC01A000E,

        /// <summary>
        /// MessageId: STATUS_LOG_METADATA_INCONSISTENT
        /// MessageText:
        /// Log service encountered a metadata file with inconsistent data.
        /// </summary>
        STATUS_LOG_METADATA_INCONSISTENT = 0xC01A000F,

        /// <summary>
        /// MessageId: STATUS_LOG_RESERVATION_INVALID
        /// MessageText:
        /// Log service encountered an attempt to erroneously allocate or dispose reservation space.
        /// </summary>
        STATUS_LOG_RESERVATION_INVALID = 0xC01A0010,

        /// <summary>
        /// MessageId: STATUS_LOG_CANT_DELETE
        /// MessageText:
        /// Log service cannot delete log file or file system container.
        /// </summary>
        STATUS_LOG_CANT_DELETE = 0xC01A0011,

        /// <summary>
        /// MessageId: STATUS_LOG_CONTAINER_LIMIT_EXCEEDED
        /// MessageText:
        /// Log service has reached the maximum allowable containers allocated to a log file.
        /// </summary>
        STATUS_LOG_CONTAINER_LIMIT_EXCEEDED = 0xC01A0012,

        /// <summary>
        /// MessageId: STATUS_LOG_START_OF_LOG
        /// MessageText:
        /// Log service has attempted to read or write backwards past the start of the log.
        /// </summary>
        STATUS_LOG_START_OF_LOG = 0xC01A0013,

        /// <summary>
        /// MessageId: STATUS_LOG_POLICY_ALREADY_INSTALLED
        /// MessageText:
        /// Log policy could not be installed because a policy of the same type is already present.
        /// </summary>
        STATUS_LOG_POLICY_ALREADY_INSTALLED = 0xC01A0014,

        /// <summary>
        /// MessageId: STATUS_LOG_POLICY_NOT_INSTALLED
        /// MessageText:
        /// Log policy in question was not installed at the time of the request.
        /// </summary>
        STATUS_LOG_POLICY_NOT_INSTALLED = 0xC01A0015,

        /// <summary>
        /// MessageId: STATUS_LOG_POLICY_INVALID
        /// MessageText:
        /// The installed set of policies on the log is invalid.
        /// </summary>
        STATUS_LOG_POLICY_INVALID = 0xC01A0016,

        /// <summary>
        /// MessageId: STATUS_LOG_POLICY_CONFLICT
        /// MessageText:
        /// A policy on the log in question prevented the operation from completing.
        /// </summary>
        STATUS_LOG_POLICY_CONFLICT = 0xC01A0017,

        /// <summary>
        /// MessageId: STATUS_LOG_PINNED_ARCHIVE_TAIL
        /// MessageText:
        /// Log space cannot be reclaimed because the log is pinned by the archive tail.
        /// </summary>
        STATUS_LOG_PINNED_ARCHIVE_TAIL = 0xC01A0018,

        /// <summary>
        /// MessageId: STATUS_LOG_RECORD_NONEXISTENT
        /// MessageText:
        /// Log record is not a record in the log file.
        /// </summary>
        STATUS_LOG_RECORD_NONEXISTENT = 0xC01A0019,

        /// <summary>
        /// MessageId: STATUS_LOG_RECORDS_RESERVED_INVALID
        /// MessageText:
        /// Number of reserved log records or the adjustment of the number of reserved log records is invalid.
        /// </summary>
        STATUS_LOG_RECORDS_RESERVED_INVALID = 0xC01A001A,

        /// <summary>
        /// MessageId: STATUS_LOG_SPACE_RESERVED_INVALID
        /// MessageText:
        /// Reserved log space or the adjustment of the log space is invalid.
        /// </summary>
        STATUS_LOG_SPACE_RESERVED_INVALID = 0xC01A001B,

        /// <summary>
        /// MessageId: STATUS_LOG_TAIL_INVALID
        /// MessageText:
        /// A new or existing archive tail or base of the active log is invalid.
        /// </summary>
        STATUS_LOG_TAIL_INVALID = 0xC01A001C,

        /// <summary>
        /// MessageId: STATUS_LOG_FULL
        /// MessageText:
        /// Log space is exhausted.
        /// </summary>
        STATUS_LOG_FULL = 0xC01A001D,

        /// <summary>
        /// MessageId: STATUS_LOG_MULTIPLEXED
        /// MessageText:
        /// Log is multiplexed, no direct writes to the physical log is allowed.
        /// </summary>
        STATUS_LOG_MULTIPLEXED = 0xC01A001E,

        /// <summary>
        /// MessageId: STATUS_LOG_DEDICATED
        /// MessageText:
        /// The operation failed because the log is a dedicated log.
        /// </summary>
        STATUS_LOG_DEDICATED = 0xC01A001F,

        /// <summary>
        /// MessageId: STATUS_LOG_ARCHIVE_NOT_IN_PROGRESS
        /// MessageText:
        /// The operation requires an archive context.
        /// </summary>
        STATUS_LOG_ARCHIVE_NOT_IN_PROGRESS = 0xC01A0020,

        /// <summary>
        /// MessageId: STATUS_LOG_ARCHIVE_IN_PROGRESS
        /// MessageText:
        /// Log archival is in progress.
        /// </summary>
        STATUS_LOG_ARCHIVE_IN_PROGRESS = 0xC01A0021,

        /// <summary>
        /// MessageId: STATUS_LOG_EPHEMERAL
        /// MessageText:
        /// The operation requires a non-ephemeral log, but the log is ephemeral.
        /// </summary>
        STATUS_LOG_EPHEMERAL = 0xC01A0022,

        /// <summary>
        /// MessageId: STATUS_LOG_NOT_ENOUGH_CONTAINERS
        /// MessageText:
        /// The log must have at least two containers before it can be read from or written to.
        /// </summary>
        STATUS_LOG_NOT_ENOUGH_CONTAINERS = 0xC01A0023,

        /// <summary>
        /// MessageId: STATUS_LOG_CLIENT_ALREADY_REGISTERED
        /// MessageText:
        /// A log client has already registered on the stream.
        /// </summary>
        STATUS_LOG_CLIENT_ALREADY_REGISTERED = 0xC01A0024,

        /// <summary>
        /// MessageId: STATUS_LOG_CLIENT_NOT_REGISTERED
        /// MessageText:
        /// A log client has not been registered on the stream.
        /// </summary>
        STATUS_LOG_CLIENT_NOT_REGISTERED = 0xC01A0025,

        /// <summary>
        /// MessageId: STATUS_LOG_FULL_HANDLER_IN_PROGRESS
        /// MessageText:
        /// A request has already been made to handle the log full condition.
        /// </summary>
        STATUS_LOG_FULL_HANDLER_IN_PROGRESS = 0xC01A0026,

        /// <summary>
        /// MessageId: STATUS_LOG_CONTAINER_READ_FAILED
        /// MessageText:
        /// Log service encountered an error when attempting to read from a log container.
        /// </summary>
        STATUS_LOG_CONTAINER_READ_FAILED = 0xC01A0027,

        /// <summary>
        /// MessageId: STATUS_LOG_CONTAINER_WRITE_FAILED
        /// MessageText:
        /// Log service encountered an error when attempting to write to a log container.
        /// </summary>
        STATUS_LOG_CONTAINER_WRITE_FAILED = 0xC01A0028,

        /// <summary>
        /// MessageId: STATUS_LOG_CONTAINER_OPEN_FAILED
        /// MessageText:
        /// Log service encountered an error when attempting open a log container.
        /// </summary>
        STATUS_LOG_CONTAINER_OPEN_FAILED = 0xC01A0029,

        /// <summary>
        /// MessageId: STATUS_LOG_CONTAINER_STATE_INVALID
        /// MessageText:
        /// Log service encountered an invalid container state when attempting a requested action.
        /// </summary>
        STATUS_LOG_CONTAINER_STATE_INVALID = 0xC01A002A,

        /// <summary>
        /// MessageId: STATUS_LOG_STATE_INVALID
        /// MessageText:
        /// Log service is not in the correct state to perform a requested action.
        /// </summary>
        STATUS_LOG_STATE_INVALID = 0xC01A002B,

        /// <summary>
        /// MessageId: STATUS_LOG_PINNED
        /// MessageText:
        /// Log space cannot be reclaimed because the log is pinned.
        /// </summary>
        STATUS_LOG_PINNED = 0xC01A002C,

        /// <summary>
        /// MessageId: STATUS_LOG_METADATA_FLUSH_FAILED
        /// MessageText:
        /// Log metadata flush failed.
        /// </summary>
        STATUS_LOG_METADATA_FLUSH_FAILED = 0xC01A002D,

        /// <summary>
        /// MessageId: STATUS_LOG_INCONSISTENT_SECURITY
        /// MessageText:
        /// Security on the log and its containers is inconsistent.
        /// </summary>
        STATUS_LOG_INCONSISTENT_SECURITY = 0xC01A002E,

        /// <summary>
        /// MessageId: STATUS_LOG_APPENDED_FLUSH_FAILED
        /// MessageText:
        /// Records were appended to the log or reservation changes were made, but the log could not be flushed.
        /// </summary>
        STATUS_LOG_APPENDED_FLUSH_FAILED = 0xC01A002F,

        /// <summary>
        /// MessageId: STATUS_LOG_PINNED_RESERVATION
        /// MessageText:
        /// The log is pinned due to reservation consuming most of the log space.  Free some reserved records to make space available.
        /// </summary>
        STATUS_LOG_PINNED_RESERVATION = 0xC01A0030,

        /// <summary>
        /// XDDM Video Facility Error codes (videoprt.sys)
        /// MessageId: STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD
        /// MessageText:
        /// {Display Driver Stopped Responding}
        /// The %hs display driver has stopped working normally. Save your work and reboot the system to restore full display functionality. The next time you reboot the machine a dialog will be displayed giving you a chance to upload data about this failure to Microsoft.
        /// </summary>
        STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD = 0xC01B00EA,

        /// <summary>
        /// MessageId: STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD_RECOVERED
        /// MessageText:
        /// {Display Driver Stopped Responding and recovered}
        /// The %hs display driver has stopped working normally. The recovery had been performed.
        /// </summary>
        STATUS_VIDEO_HUNG_DISPLAY_DRIVER_THREAD_RECOVERED = 0x801B00EB,

        /// <summary>
        /// MessageId: STATUS_VIDEO_DRIVER_DEBUG_REPORT_REQUEST
        /// MessageText:
        /// {Display Driver Recovered From Failure}
        /// The %hs display driver has detected and recovered from a failure. Some graphical operations may have failed. The next time you reboot the machine a dialog will be displayed giving you a chance to upload data about this failure to Microsoft.
        /// </summary>
        STATUS_VIDEO_DRIVER_DEBUG_REPORT_REQUEST = 0x401B00EC,

        /// <summary>
        /// Monitor Facility Error codes (monitor.sys)
        /// MessageId: STATUS_MONITOR_NO_DESCRIPTOR
        /// MessageText:
        /// Monitor descriptor could not be obtained.
        /// </summary>
        STATUS_MONITOR_NO_DESCRIPTOR = 0xC01D0001,

        /// <summary>
        /// MessageId: STATUS_MONITOR_UNKNOWN_DESCRIPTOR_FORMAT
        /// MessageText:
        /// Format of the obtained monitor descriptor is not supported by this release.
        /// </summary>
        STATUS_MONITOR_UNKNOWN_DESCRIPTOR_FORMAT = 0xC01D0002,

        /// <summary>
        /// MessageId: STATUS_MONITOR_INVALID_DESCRIPTOR_CHECKSUM
        /// MessageText:
        /// Checksum of the obtained monitor descriptor is invalid.
        /// </summary>
        STATUS_MONITOR_INVALID_DESCRIPTOR_CHECKSUM = 0xC01D0003,

        /// <summary>
        /// MessageId: STATUS_MONITOR_INVALID_STANDARD_TIMING_BLOCK
        /// MessageText:
        /// Monitor descriptor contains an invalid standard timing block.
        /// </summary>
        STATUS_MONITOR_INVALID_STANDARD_TIMING_BLOCK = 0xC01D0004,

        /// <summary>
        /// MessageId: STATUS_MONITOR_WMI_DATABLOCK_REGISTRATION_FAILED
        /// MessageText:
        /// WMI data block registration failed for one of the MSMonitorClass WMI subclasses.
        /// </summary>
        STATUS_MONITOR_WMI_DATABLOCK_REGISTRATION_FAILED = 0xC01D0005,

        /// <summary>
        /// MessageId: STATUS_MONITOR_INVALID_SERIAL_NUMBER_MONDSC_BLOCK
        /// MessageText:
        /// Provided monitor descriptor block is either corrupted or does not contain monitor's detailed serial number.
        /// </summary>
        STATUS_MONITOR_INVALID_SERIAL_NUMBER_MONDSC_BLOCK = 0xC01D0006,

        /// <summary>
        /// MessageId: STATUS_MONITOR_INVALID_USER_FRIENDLY_MONDSC_BLOCK
        /// MessageText:
        /// Provided monitor descriptor block is either corrupted or does not contain monitor's user friendly name.
        /// </summary>
        STATUS_MONITOR_INVALID_USER_FRIENDLY_MONDSC_BLOCK = 0xC01D0007,

        /// <summary>
        /// MessageId: STATUS_MONITOR_NO_MORE_DESCRIPTOR_DATA
        /// MessageText:
        /// There is no monitor descriptor data at the specified (offset, size) region.
        /// </summary>
        STATUS_MONITOR_NO_MORE_DESCRIPTOR_DATA = 0xC01D0008,

        /// <summary>
        /// MessageId: STATUS_MONITOR_INVALID_DETAILED_TIMING_BLOCK
        /// MessageText:
        /// Monitor descriptor contains an invalid detailed timing block.
        /// </summary>
        STATUS_MONITOR_INVALID_DETAILED_TIMING_BLOCK = 0xC01D0009,

        /// <summary>
        /// Graphics Facility Error codes (dxg.sys, dxgkrnl.sys)
        ///   Common Windows Graphics Kernel Subsystem status codes {0x0000..0x00ff}
        /// MessageId: STATUS_GRAPHICS_NOT_EXCLUSIVE_MODE_OWNER
        /// MessageText:
        /// Exclusive mode ownership is needed to create unmanaged primary allocation.
        /// </summary>
        STATUS_GRAPHICS_NOT_EXCLUSIVE_MODE_OWNER = 0xC01E0000,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INSUFFICIENT_DMA_BUFFER
        /// MessageText:
        /// The driver needs more DMA buffer space in order to complete the requested operation.
        /// </summary>
        STATUS_GRAPHICS_INSUFFICIENT_DMA_BUFFER = 0xC01E0001,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_DISPLAY_ADAPTER
        /// MessageText:
        /// Specified display adapter handle is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_DISPLAY_ADAPTER = 0xC01E0002,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ADAPTER_WAS_RESET
        /// MessageText:
        /// Specified display adapter and all of its state has been reset.
        /// </summary>
        STATUS_GRAPHICS_ADAPTER_WAS_RESET = 0xC01E0003,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_DRIVER_MODEL
        /// MessageText:
        /// The driver stack doesn't match the expected driver model.
        /// </summary>
        STATUS_GRAPHICS_INVALID_DRIVER_MODEL = 0xC01E0004,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PRESENT_MODE_CHANGED
        /// MessageText:
        /// Present happened but ended up into the changed desktop mode
        /// </summary>
        STATUS_GRAPHICS_PRESENT_MODE_CHANGED = 0xC01E0005,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PRESENT_OCCLUDED
        /// MessageText:
        /// Nothing to present due to desktop occlusion
        /// </summary>
        STATUS_GRAPHICS_PRESENT_OCCLUDED = 0xC01E0006,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PRESENT_DENIED
        /// MessageText:
        /// Not able to present due to denial of desktop access
        /// </summary>
        STATUS_GRAPHICS_PRESENT_DENIED = 0xC01E0007,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CANNOTCOLORCONVERT
        /// MessageText:
        /// Not able to present with color conversion
        /// </summary>
        STATUS_GRAPHICS_CANNOTCOLORCONVERT = 0xC01E0008,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DRIVER_MISMATCH
        /// MessageText:
        /// The kernel driver detected a version mismatch between it and the user mode driver.
        /// </summary>
        STATUS_GRAPHICS_DRIVER_MISMATCH = 0xC01E0009,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PARTIAL_DATA_POPULATED
        /// MessageText:
        /// Specified buffer is not big enough to contain entire requested dataset. Partial data populated up to the size of the buffer.
        /// Caller needs to provide buffer of size as specified in the partially populated buffer's content (interface specific).
        /// </summary>
        STATUS_GRAPHICS_PARTIAL_DATA_POPULATED = 0x401E000A,

        /// <summary>
        /// Video Memory Manager (VidMM) specific status codes {0x0100..0x01ff}
        /// MessageId: STATUS_GRAPHICS_NO_VIDEO_MEMORY
        /// MessageText:
        /// Not enough video memory available to complete the operation.
        /// </summary>
        STATUS_GRAPHICS_NO_VIDEO_MEMORY = 0xC01E0100,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CANT_LOCK_MEMORY
        /// MessageText:
        /// Couldn't probe and lock the underlying memory of an allocation.
        /// </summary>
        STATUS_GRAPHICS_CANT_LOCK_MEMORY = 0xC01E0101,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ALLOCATION_BUSY
        /// MessageText:
        /// The allocation is currently busy.
        /// </summary>
        STATUS_GRAPHICS_ALLOCATION_BUSY = 0xC01E0102,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_TOO_MANY_REFERENCES
        /// MessageText:
        /// An object being referenced has already reached the maximum reference count and can't be referenced any further.
        /// </summary>
        STATUS_GRAPHICS_TOO_MANY_REFERENCES = 0xC01E0103,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_TRY_AGAIN_LATER
        /// MessageText:
        /// A problem couldn't be solved due to some currently existing condition. The problem should be tried again later.
        /// </summary>
        STATUS_GRAPHICS_TRY_AGAIN_LATER = 0xC01E0104,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_TRY_AGAIN_NOW
        /// MessageText:
        /// A problem couldn't be solved due to some currently existing condition. The problem should be tried again immediately.
        /// </summary>
        STATUS_GRAPHICS_TRY_AGAIN_NOW = 0xC01E0105,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ALLOCATION_INVALID
        /// MessageText:
        /// The allocation is invalid.
        /// </summary>
        STATUS_GRAPHICS_ALLOCATION_INVALID = 0xC01E0106,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNAVAILABLE
        /// MessageText:
        /// No more unswizzling aperture are currently available.
        /// </summary>
        STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNAVAILABLE = 0xC01E0107,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNSUPPORTED
        /// MessageText:
        /// The current allocation can't be unswizzled by an aperture.
        /// </summary>
        STATUS_GRAPHICS_UNSWIZZLING_APERTURE_UNSUPPORTED = 0xC01E0108,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CANT_EVICT_PINNED_ALLOCATION
        /// MessageText:
        /// The request failed because a pinned allocation can't be evicted.
        /// </summary>
        STATUS_GRAPHICS_CANT_EVICT_PINNED_ALLOCATION = 0xC01E0109,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_ALLOCATION_USAGE
        /// MessageText:
        /// The allocation can't be used from it's current segment location for the specified operation.
        /// </summary>
        STATUS_GRAPHICS_INVALID_ALLOCATION_USAGE = 0xC01E0110,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CANT_RENDER_LOCKED_ALLOCATION
        /// MessageText:
        /// A locked allocation can't be used in the current command buffer.
        /// </summary>
        STATUS_GRAPHICS_CANT_RENDER_LOCKED_ALLOCATION = 0xC01E0111,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ALLOCATION_CLOSED
        /// MessageText:
        /// The allocation being referenced has been closed permanently.
        /// </summary>
        STATUS_GRAPHICS_ALLOCATION_CLOSED = 0xC01E0112,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_ALLOCATION_INSTANCE
        /// MessageText:
        /// An invalid allocation instance is being referenced.
        /// </summary>
        STATUS_GRAPHICS_INVALID_ALLOCATION_INSTANCE = 0xC01E0113,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_ALLOCATION_HANDLE
        /// MessageText:
        /// An invalid allocation handle is being referenced.
        /// </summary>
        STATUS_GRAPHICS_INVALID_ALLOCATION_HANDLE = 0xC01E0114,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_WRONG_ALLOCATION_DEVICE
        /// MessageText:
        /// The allocation being referenced doesn't belong to the current device.
        /// </summary>
        STATUS_GRAPHICS_WRONG_ALLOCATION_DEVICE = 0xC01E0115,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ALLOCATION_CONTENT_LOST
        /// MessageText:
        /// The specified allocation lost its content.
        /// </summary>
        STATUS_GRAPHICS_ALLOCATION_CONTENT_LOST = 0xC01E0116,

        /// <summary>
        /// Video GPU Scheduler (VidSch) specific status codes {0x0200..0x02ff}
        /// MessageId: STATUS_GRAPHICS_GPU_EXCEPTION_ON_DEVICE
        /// MessageText:
        /// GPU exception is detected on the given device. The device is not able to be scheduled.
        /// </summary>
        STATUS_GRAPHICS_GPU_EXCEPTION_ON_DEVICE = 0xC01E0200,

        /// <summary>
        ///   Video Present Network Management (VidPNMgr) specific status codes {0x0300..0x03ff}
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY
        /// MessageText:
        /// Specified VidPN topology is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY = 0xC01E0300,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_VIDPN_TOPOLOGY_NOT_SUPPORTED
        /// MessageText:
        /// Specified VidPN topology is valid but is not supported by this model of the display adapter.
        /// </summary>
        STATUS_GRAPHICS_VIDPN_TOPOLOGY_NOT_SUPPORTED = 0xC01E0301,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_VIDPN_TOPOLOGY_CURRENTLY_NOT_SUPPORTED
        /// MessageText:
        /// Specified VidPN topology is valid but is not supported by the display adapter at this time, due to current allocation of its resources.
        /// </summary>
        STATUS_GRAPHICS_VIDPN_TOPOLOGY_CURRENTLY_NOT_SUPPORTED = 0xC01E0302,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDPN
        /// MessageText:
        /// Specified VidPN handle is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDPN = 0xC01E0303,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE
        /// MessageText:
        /// Specified video present source is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE = 0xC01E0304,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET
        /// MessageText:
        /// Specified video present target is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET = 0xC01E0305,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_VIDPN_MODALITY_NOT_SUPPORTED
        /// MessageText:
        /// Specified VidPN modality is not supported (e.g. at least two of the pinned modes are not cofunctional).
        /// </summary>
        STATUS_GRAPHICS_VIDPN_MODALITY_NOT_SUPPORTED = 0xC01E0306,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MODE_NOT_PINNED
        /// MessageText:
        /// No mode is pinned on the specified VidPN source/target.
        /// </summary>
        STATUS_GRAPHICS_MODE_NOT_PINNED = 0x401E0307,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDPN_SOURCEMODESET
        /// MessageText:
        /// Specified VidPN source mode set is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDPN_SOURCEMODESET = 0xC01E0308,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDPN_TARGETMODESET
        /// MessageText:
        /// Specified VidPN target mode set is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDPN_TARGETMODESET = 0xC01E0309,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_FREQUENCY
        /// MessageText:
        /// Specified video signal frequency is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_FREQUENCY = 0xC01E030A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_ACTIVE_REGION
        /// MessageText:
        /// Specified video signal active region is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_ACTIVE_REGION = 0xC01E030B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_TOTAL_REGION
        /// MessageText:
        /// Specified video signal total region is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_TOTAL_REGION = 0xC01E030C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE_MODE
        /// MessageText:
        /// Specified video present source mode is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_SOURCE_MODE = 0xC01E0310,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET_MODE
        /// MessageText:
        /// Specified video present target mode is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDEO_PRESENT_TARGET_MODE = 0xC01E0311,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PINNED_MODE_MUST_REMAIN_IN_SET
        /// MessageText:
        /// Pinned mode must remain in the set on VidPN's cofunctional modality enumeration.
        /// </summary>
        STATUS_GRAPHICS_PINNED_MODE_MUST_REMAIN_IN_SET = 0xC01E0312,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PATH_ALREADY_IN_TOPOLOGY
        /// MessageText:
        /// Specified video present path is already in VidPN's topology.
        /// </summary>
        STATUS_GRAPHICS_PATH_ALREADY_IN_TOPOLOGY = 0xC01E0313,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MODE_ALREADY_IN_MODESET
        /// MessageText:
        /// Specified mode is already in the mode set.
        /// </summary>
        STATUS_GRAPHICS_MODE_ALREADY_IN_MODESET = 0xC01E0314,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDEOPRESENTSOURCESET
        /// MessageText:
        /// Specified video present source set is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDEOPRESENTSOURCESET = 0xC01E0315,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDEOPRESENTTARGETSET
        /// MessageText:
        /// Specified video present target set is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDEOPRESENTTARGETSET = 0xC01E0316,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_SOURCE_ALREADY_IN_SET
        /// MessageText:
        /// Specified video present source is already in the video present source set.
        /// </summary>
        STATUS_GRAPHICS_SOURCE_ALREADY_IN_SET = 0xC01E0317,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_TARGET_ALREADY_IN_SET
        /// MessageText:
        /// Specified video present target is already in the video present target set.
        /// </summary>
        STATUS_GRAPHICS_TARGET_ALREADY_IN_SET = 0xC01E0318,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDPN_PRESENT_PATH
        /// MessageText:
        /// Specified VidPN present path is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDPN_PRESENT_PATH = 0xC01E0319,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_RECOMMENDED_VIDPN_TOPOLOGY
        /// MessageText:
        /// Miniport has no recommendation for augmentation of the specified VidPN's topology.
        /// </summary>
        STATUS_GRAPHICS_NO_RECOMMENDED_VIDPN_TOPOLOGY = 0xC01E031A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGESET
        /// MessageText:
        /// Specified monitor frequency range set is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGESET = 0xC01E031B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE
        /// MessageText:
        /// Specified monitor frequency range is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE = 0xC01E031C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_FREQUENCYRANGE_NOT_IN_SET
        /// MessageText:
        /// Specified frequency range is not in the specified monitor frequency range set.
        /// </summary>
        STATUS_GRAPHICS_FREQUENCYRANGE_NOT_IN_SET = 0xC01E031D,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_PREFERRED_MODE
        /// MessageText:
        /// Specified mode set does not specify preference for one of its modes.
        /// </summary>
        STATUS_GRAPHICS_NO_PREFERRED_MODE = 0x401E031E,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_FREQUENCYRANGE_ALREADY_IN_SET
        /// MessageText:
        /// Specified frequency range is already in the specified monitor frequency range set.
        /// </summary>
        STATUS_GRAPHICS_FREQUENCYRANGE_ALREADY_IN_SET = 0xC01E031F,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_STALE_MODESET
        /// MessageText:
        /// Specified mode set is stale. Please reacquire the new mode set.
        /// </summary>
        STATUS_GRAPHICS_STALE_MODESET = 0xC01E0320,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITOR_SOURCEMODESET
        /// MessageText:
        /// Specified monitor source mode set is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITOR_SOURCEMODESET = 0xC01E0321,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITOR_SOURCE_MODE
        /// MessageText:
        /// Specified monitor source mode is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITOR_SOURCE_MODE = 0xC01E0322,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_RECOMMENDED_FUNCTIONAL_VIDPN
        /// MessageText:
        /// Miniport does not have any recommendation regarding the request to provide a functional VidPN given the current display adapter configuration.
        /// </summary>
        STATUS_GRAPHICS_NO_RECOMMENDED_FUNCTIONAL_VIDPN = 0xC01E0323,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MODE_ID_MUST_BE_UNIQUE
        /// MessageText:
        /// ID of the specified mode is already used by another mode in the set.
        /// </summary>
        STATUS_GRAPHICS_MODE_ID_MUST_BE_UNIQUE = 0xC01E0324,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_EMPTY_ADAPTER_MONITOR_MODE_SUPPORT_INTERSECTION
        /// MessageText:
        /// System failed to determine a mode that is supported by both the display adapter and the monitor connected to it.
        /// </summary>
        STATUS_GRAPHICS_EMPTY_ADAPTER_MONITOR_MODE_SUPPORT_INTERSECTION = 0xC01E0325,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_VIDEO_PRESENT_TARGETS_LESS_THAN_SOURCES
        /// MessageText:
        /// Number of video present targets must be greater than or equal to the number of video present sources.
        /// </summary>
        STATUS_GRAPHICS_VIDEO_PRESENT_TARGETS_LESS_THAN_SOURCES = 0xC01E0326,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PATH_NOT_IN_TOPOLOGY
        /// MessageText:
        /// Specified present path is not in VidPN's topology.
        /// </summary>
        STATUS_GRAPHICS_PATH_NOT_IN_TOPOLOGY = 0xC01E0327,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_SOURCE
        /// MessageText:
        /// Display adapter must have at least one video present source.
        /// </summary>
        STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_SOURCE = 0xC01E0328,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_TARGET
        /// MessageText:
        /// Display adapter must have at least one video present target.
        /// </summary>
        STATUS_GRAPHICS_ADAPTER_MUST_HAVE_AT_LEAST_ONE_TARGET = 0xC01E0329,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITORDESCRIPTORSET
        /// MessageText:
        /// Specified monitor descriptor set is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITORDESCRIPTORSET = 0xC01E032A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITORDESCRIPTOR
        /// MessageText:
        /// Specified monitor descriptor is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITORDESCRIPTOR = 0xC01E032B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MONITORDESCRIPTOR_NOT_IN_SET
        /// MessageText:
        /// Specified descriptor is not in the specified monitor descriptor set.
        /// </summary>
        STATUS_GRAPHICS_MONITORDESCRIPTOR_NOT_IN_SET = 0xC01E032C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MONITORDESCRIPTOR_ALREADY_IN_SET
        /// MessageText:
        /// Specified descriptor is already in the specified monitor descriptor set.
        /// </summary>
        STATUS_GRAPHICS_MONITORDESCRIPTOR_ALREADY_IN_SET = 0xC01E032D,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MONITORDESCRIPTOR_ID_MUST_BE_UNIQUE
        /// MessageText:
        /// ID of the specified monitor descriptor is already used by another descriptor in the set.
        /// </summary>
        STATUS_GRAPHICS_MONITORDESCRIPTOR_ID_MUST_BE_UNIQUE = 0xC01E032E,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDPN_TARGET_SUBSET_TYPE
        /// MessageText:
        /// Specified video present target subset type is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDPN_TARGET_SUBSET_TYPE = 0xC01E032F,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_RESOURCES_NOT_RELATED
        /// MessageText:
        /// Two or more of the specified resources are not related to each other, as defined by the interface semantics.
        /// </summary>
        STATUS_GRAPHICS_RESOURCES_NOT_RELATED = 0xC01E0330,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_SOURCE_ID_MUST_BE_UNIQUE
        /// MessageText:
        /// ID of the specified video present source is already used by another source in the set.
        /// </summary>
        STATUS_GRAPHICS_SOURCE_ID_MUST_BE_UNIQUE = 0xC01E0331,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_TARGET_ID_MUST_BE_UNIQUE
        /// MessageText:
        /// ID of the specified video present target is already used by another target in the set.
        /// </summary>
        STATUS_GRAPHICS_TARGET_ID_MUST_BE_UNIQUE = 0xC01E0332,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_AVAILABLE_VIDPN_TARGET
        /// MessageText:
        /// Specified VidPN source cannot be used because there is no available VidPN target to connect it to.
        /// </summary>
        STATUS_GRAPHICS_NO_AVAILABLE_VIDPN_TARGET = 0xC01E0333,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MONITOR_COULD_NOT_BE_ASSOCIATED_WITH_ADAPTER
        /// MessageText:
        /// Newly arrived monitor could not be associated with a display adapter.
        /// </summary>
        STATUS_GRAPHICS_MONITOR_COULD_NOT_BE_ASSOCIATED_WITH_ADAPTER = 0xC01E0334,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_VIDPNMGR
        /// MessageText:
        /// Display adapter in question does not have an associated VidPN manager.
        /// </summary>
        STATUS_GRAPHICS_NO_VIDPNMGR = 0xC01E0335,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_ACTIVE_VIDPN
        /// MessageText:
        /// VidPN manager of the display adapter in question does not have an active VidPN.
        /// </summary>
        STATUS_GRAPHICS_NO_ACTIVE_VIDPN = 0xC01E0336,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_STALE_VIDPN_TOPOLOGY
        /// MessageText:
        /// Specified VidPN topology is stale. Please reacquire the new topology.
        /// </summary>
        STATUS_GRAPHICS_STALE_VIDPN_TOPOLOGY = 0xC01E0337,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MONITOR_NOT_CONNECTED
        /// MessageText:
        /// There is no monitor connected on the specified video present target.
        /// </summary>
        STATUS_GRAPHICS_MONITOR_NOT_CONNECTED = 0xC01E0338,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_SOURCE_NOT_IN_TOPOLOGY
        /// MessageText:
        /// Specified source is not part of the specified VidPN's topology.
        /// </summary>
        STATUS_GRAPHICS_SOURCE_NOT_IN_TOPOLOGY = 0xC01E0339,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_PRIMARYSURFACE_SIZE
        /// MessageText:
        /// Specified primary surface size is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_PRIMARYSURFACE_SIZE = 0xC01E033A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VISIBLEREGION_SIZE
        /// MessageText:
        /// Specified visible region size is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VISIBLEREGION_SIZE = 0xC01E033B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_STRIDE
        /// MessageText:
        /// Specified stride is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_STRIDE = 0xC01E033C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_PIXELFORMAT
        /// MessageText:
        /// Specified pixel format is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_PIXELFORMAT = 0xC01E033D,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_COLORBASIS
        /// MessageText:
        /// Specified color basis is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_COLORBASIS = 0xC01E033E,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_PIXELVALUEACCESSMODE
        /// MessageText:
        /// Specified pixel value access mode is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_PIXELVALUEACCESSMODE = 0xC01E033F,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_TARGET_NOT_IN_TOPOLOGY
        /// MessageText:
        /// Specified target is not part of the specified VidPN's topology.
        /// </summary>
        STATUS_GRAPHICS_TARGET_NOT_IN_TOPOLOGY = 0xC01E0340,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_DISPLAY_MODE_MANAGEMENT_SUPPORT
        /// MessageText:
        /// Failed to acquire display mode management interface.
        /// </summary>
        STATUS_GRAPHICS_NO_DISPLAY_MODE_MANAGEMENT_SUPPORT = 0xC01E0341,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_VIDPN_SOURCE_IN_USE
        /// MessageText:
        /// Specified VidPN source is already owned by a DMM client and cannot be used until that client releases it.
        /// </summary>
        STATUS_GRAPHICS_VIDPN_SOURCE_IN_USE = 0xC01E0342,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CANT_ACCESS_ACTIVE_VIDPN
        /// MessageText:
        /// Specified VidPN is active and cannot be accessed.
        /// </summary>
        STATUS_GRAPHICS_CANT_ACCESS_ACTIVE_VIDPN = 0xC01E0343,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_PATH_IMPORTANCE_ORDINAL
        /// MessageText:
        /// Specified VidPN present path importance ordinal is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_PATH_IMPORTANCE_ORDINAL = 0xC01E0344,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_PATH_CONTENT_GEOMETRY_TRANSFORMATION
        /// MessageText:
        /// Specified VidPN present path content geometry transformation is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_PATH_CONTENT_GEOMETRY_TRANSFORMATION = 0xC01E0345,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_SUPPORTED
        /// MessageText:
        /// Specified content geometry transformation is not supported on the respective VidPN present path.
        /// </summary>
        STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_SUPPORTED = 0xC01E0346,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_GAMMA_RAMP
        /// MessageText:
        /// Specified gamma ramp is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_GAMMA_RAMP = 0xC01E0347,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_GAMMA_RAMP_NOT_SUPPORTED
        /// MessageText:
        /// Specified gamma ramp is not supported on the respective VidPN present path.
        /// </summary>
        STATUS_GRAPHICS_GAMMA_RAMP_NOT_SUPPORTED = 0xC01E0348,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MULTISAMPLING_NOT_SUPPORTED
        /// MessageText:
        /// Multi-sampling is not supported on the respective VidPN present path.
        /// </summary>
        STATUS_GRAPHICS_MULTISAMPLING_NOT_SUPPORTED = 0xC01E0349,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MODE_NOT_IN_MODESET
        /// MessageText:
        /// Specified mode is not in the specified mode set.
        /// </summary>
        STATUS_GRAPHICS_MODE_NOT_IN_MODESET = 0xC01E034A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DATASET_IS_EMPTY
        /// MessageText:
        /// Specified data set (e.g. mode set, frequency range set, descriptor set, topology, etc.) is empty.
        /// </summary>
        STATUS_GRAPHICS_DATASET_IS_EMPTY = 0x401E034B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_MORE_ELEMENTS_IN_DATASET
        /// MessageText:
        /// Specified data set (e.g. mode set, frequency range set, descriptor set, topology, etc.) does not contain any more elements.
        /// </summary>
        STATUS_GRAPHICS_NO_MORE_ELEMENTS_IN_DATASET = 0x401E034C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY_RECOMMENDATION_REASON
        /// MessageText:
        /// Specified VidPN topology recommendation reason is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_VIDPN_TOPOLOGY_RECOMMENDATION_REASON = 0xC01E034D,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_PATH_CONTENT_TYPE
        /// MessageText:
        /// Specified VidPN present path content type is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_PATH_CONTENT_TYPE = 0xC01E034E,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_COPYPROTECTION_TYPE
        /// MessageText:
        /// Specified VidPN present path copy protection type is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_COPYPROTECTION_TYPE = 0xC01E034F,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_UNASSIGNED_MODESET_ALREADY_EXISTS
        /// MessageText:
        /// No more than one unassigned mode set can exist at any given time for a given VidPN source/target.
        /// </summary>
        STATUS_GRAPHICS_UNASSIGNED_MODESET_ALREADY_EXISTS = 0xC01E0350,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_PINNED
        /// MessageText:
        /// Specified content transformation is not pinned on the specified VidPN present path.
        /// </summary>
        STATUS_GRAPHICS_PATH_CONTENT_GEOMETRY_TRANSFORMATION_NOT_PINNED = 0x401E0351,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_SCANLINE_ORDERING
        /// MessageText:
        /// Specified scanline ordering type is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_SCANLINE_ORDERING = 0xC01E0352,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_TOPOLOGY_CHANGES_NOT_ALLOWED
        /// MessageText:
        /// Topology changes are not allowed for the specified VidPN.
        /// </summary>
        STATUS_GRAPHICS_TOPOLOGY_CHANGES_NOT_ALLOWED = 0xC01E0353,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_AVAILABLE_IMPORTANCE_ORDINALS
        /// MessageText:
        /// All available importance ordinals are already used in specified topology.
        /// </summary>
        STATUS_GRAPHICS_NO_AVAILABLE_IMPORTANCE_ORDINALS = 0xC01E0354,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INCOMPATIBLE_PRIVATE_FORMAT
        /// MessageText:
        /// Specified primary surface has a different private format attribute than the current primary surface
        /// </summary>
        STATUS_GRAPHICS_INCOMPATIBLE_PRIVATE_FORMAT = 0xC01E0355,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MODE_PRUNING_ALGORITHM
        /// MessageText:
        /// Specified mode pruning algorithm is invalid
        /// </summary>
        STATUS_GRAPHICS_INVALID_MODE_PRUNING_ALGORITHM = 0xC01E0356,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITOR_CAPABILITY_ORIGIN
        /// MessageText:
        /// Specified monitor capability origin is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITOR_CAPABILITY_ORIGIN = 0xC01E0357,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE_CONSTRAINT
        /// MessageText:
        /// Specified monitor frequency range constraint is invalid.
        /// </summary>
        STATUS_GRAPHICS_INVALID_MONITOR_FREQUENCYRANGE_CONSTRAINT = 0xC01E0358,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MAX_NUM_PATHS_REACHED
        /// MessageText:
        /// Maximum supported number of present paths has been reached.
        /// </summary>
        STATUS_GRAPHICS_MAX_NUM_PATHS_REACHED = 0xC01E0359,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CANCEL_VIDPN_TOPOLOGY_AUGMENTATION
        /// MessageText:
        /// Miniport requested that augmentation be cancelled for the specified source of the specified VidPN's topology.
        /// </summary>
        STATUS_GRAPHICS_CANCEL_VIDPN_TOPOLOGY_AUGMENTATION = 0xC01E035A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_CLIENT_TYPE
        /// MessageText:
        /// Specified client type was not recognized.
        /// </summary>
        STATUS_GRAPHICS_INVALID_CLIENT_TYPE = 0xC01E035B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CLIENTVIDPN_NOT_SET
        /// MessageText:
        /// Client VidPN is not set on this adapter (e.g. no user mode initiated mode changes took place on this adapter yet).
        /// </summary>
        STATUS_GRAPHICS_CLIENTVIDPN_NOT_SET = 0xC01E035C,

        /// <summary>
        ///   Port specific status codes {0x0400..0x04ff}
        /// MessageId: STATUS_GRAPHICS_SPECIFIED_CHILD_ALREADY_CONNECTED
        /// MessageText:
        /// Specified display adapter child device already has an external device connected to it.
        /// </summary>
        STATUS_GRAPHICS_SPECIFIED_CHILD_ALREADY_CONNECTED = 0xC01E0400,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CHILD_DESCRIPTOR_NOT_SUPPORTED
        /// MessageText:
        /// Specified display adapter child device does not support descriptor exposure.
        /// </summary>
        STATUS_GRAPHICS_CHILD_DESCRIPTOR_NOT_SUPPORTED = 0xC01E0401,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_UNKNOWN_CHILD_STATUS
        /// MessageText:
        /// Child device presence was not reliably detected.
        /// </summary>
        STATUS_GRAPHICS_UNKNOWN_CHILD_STATUS = 0x401E042F,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NOT_A_LINKED_ADAPTER
        /// MessageText:
        /// The display adapter is not linked to any other adapters.
        /// </summary>
        STATUS_GRAPHICS_NOT_A_LINKED_ADAPTER = 0xC01E0430,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_LEADLINK_NOT_ENUMERATED
        /// MessageText:
        /// Lead adapter in a linked configuration was not enumerated yet.
        /// </summary>
        STATUS_GRAPHICS_LEADLINK_NOT_ENUMERATED = 0xC01E0431,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CHAINLINKS_NOT_ENUMERATED
        /// MessageText:
        /// Some chain adapters in a linked configuration were not enumerated yet.
        /// </summary>
        STATUS_GRAPHICS_CHAINLINKS_NOT_ENUMERATED = 0xC01E0432,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ADAPTER_CHAIN_NOT_READY
        /// MessageText:
        /// The chain of linked adapters is not ready to start because of an unknown failure.
        /// </summary>
        STATUS_GRAPHICS_ADAPTER_CHAIN_NOT_READY = 0xC01E0433,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CHAINLINKS_NOT_STARTED
        /// MessageText:
        /// An attempt was made to start a lead link display adapter when the chain links were not started yet.
        /// </summary>
        STATUS_GRAPHICS_CHAINLINKS_NOT_STARTED = 0xC01E0434,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_CHAINLINKS_NOT_POWERED_ON
        /// MessageText:
        /// An attempt was made to power up a lead link display adapter when the chain links were powered down.
        /// </summary>
        STATUS_GRAPHICS_CHAINLINKS_NOT_POWERED_ON = 0xC01E0435,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INCONSISTENT_DEVICE_LINK_STATE
        /// MessageText:
        /// The adapter link was found to be in an inconsistent state. Not all adapters are in an expected PNP/Power state.
        /// </summary>
        STATUS_GRAPHICS_INCONSISTENT_DEVICE_LINK_STATE = 0xC01E0436,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_LEADLINK_START_DEFERRED
        /// MessageText:
        /// Starting the leadlink adapter has been deferred temporarily.
        /// </summary>
        STATUS_GRAPHICS_LEADLINK_START_DEFERRED = 0x401E0437,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NOT_POST_DEVICE_DRIVER
        /// MessageText:
        /// The driver trying to start is not the same as the driver for the POSTed display adapter.
        /// </summary>
        STATUS_GRAPHICS_NOT_POST_DEVICE_DRIVER = 0xC01E0438,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_POLLING_TOO_FREQUENTLY
        /// MessageText:
        /// The display adapter is being polled for children too frequently at the same polling level.
        /// </summary>
        STATUS_GRAPHICS_POLLING_TOO_FREQUENTLY = 0x401E0439,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_START_DEFERRED
        /// MessageText:
        /// Starting the adapter has been deferred temporarily.
        /// </summary>
        STATUS_GRAPHICS_START_DEFERRED = 0x401E043A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_ADAPTER_ACCESS_NOT_EXCLUDED
        /// MessageText:
        /// An operation is being attempted that requires the display adapter to be in a quiescent state.
        /// </summary>
        STATUS_GRAPHICS_ADAPTER_ACCESS_NOT_EXCLUDED = 0xC01E043B,

        /// <summary>
        ///   OPM, PVP and UAB status codes {0x0500..0x057F}
        /// MessageId: STATUS_GRAPHICS_OPM_NOT_SUPPORTED
        /// MessageText:
        /// The driver does not support OPM.
        /// </summary>
        STATUS_GRAPHICS_OPM_NOT_SUPPORTED = 0xC01E0500,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_COPP_NOT_SUPPORTED
        /// MessageText:
        /// The driver does not support COPP.
        /// </summary>
        STATUS_GRAPHICS_COPP_NOT_SUPPORTED = 0xC01E0501,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_UAB_NOT_SUPPORTED
        /// MessageText:
        /// The driver does not support UAB.
        /// </summary>
        STATUS_GRAPHICS_UAB_NOT_SUPPORTED = 0xC01E0502,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_INVALID_ENCRYPTED_PARAMETERS
        /// MessageText:
        /// The specified encrypted parameters are invalid.
        /// </summary>
        STATUS_GRAPHICS_OPM_INVALID_ENCRYPTED_PARAMETERS = 0xC01E0503,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_NO_PROTECTED_OUTPUTS_EXIST
        /// MessageText:
        /// The GDI display device passed to this function does not have any active protected outputs.
        /// </summary>
        STATUS_GRAPHICS_OPM_NO_PROTECTED_OUTPUTS_EXIST = 0xC01E0505,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_INTERNAL_ERROR
        /// MessageText:
        /// An internal error caused an operation to fail.
        /// </summary>
        STATUS_GRAPHICS_OPM_INTERNAL_ERROR = 0xC01E050B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_INVALID_HANDLE
        /// MessageText:
        /// The function failed because the caller passed in an invalid OPM user mode handle.
        /// </summary>
        STATUS_GRAPHICS_OPM_INVALID_HANDLE = 0xC01E050C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PVP_INVALID_CERTIFICATE_LENGTH
        /// MessageText:
        /// A certificate could not be returned because the certificate buffer passed to the function was too small.
        /// </summary>
        STATUS_GRAPHICS_PVP_INVALID_CERTIFICATE_LENGTH = 0xC01E050E,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_SPANNING_MODE_ENABLED
        /// MessageText:
        /// The DxgkDdiOpmCreateProtectedOutput function could not create a protected output because the Video Present Target is in spanning mode.
        /// </summary>
        STATUS_GRAPHICS_OPM_SPANNING_MODE_ENABLED = 0xC01E050F,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_THEATER_MODE_ENABLED
        /// MessageText:
        /// The DxgkDdiOpmCreateProtectedOutput function could not create a protected output because the Video Present Target is in theater mode.
        /// </summary>
        STATUS_GRAPHICS_OPM_THEATER_MODE_ENABLED = 0xC01E0510,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PVP_HFS_FAILED
        /// MessageText:
        /// The function failed because the display adapter's Hardware Functionality Scan failed to validate the graphics hardware.  
        /// </summary>
        STATUS_GRAPHICS_PVP_HFS_FAILED = 0xC01E0511,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_INVALID_SRM
        /// MessageText:
        /// The HDCP System Renewability Message passed to this function did not comply with section 5 of the HDCP 1.1 specification.
        /// </summary>
        STATUS_GRAPHICS_OPM_INVALID_SRM = 0xC01E0512,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_HDCP
        /// MessageText:
        /// The protected output cannot enable the High-bandwidth Digital Content Protection (HDCP) System because it does not support HDCP.
        /// </summary>
        STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_HDCP = 0xC01E0513,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_ACP
        /// MessageText:
        /// The protected output cannot enable Analogue Copy Protection (ACP) because it does not support ACP.
        /// </summary>
        STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_ACP = 0xC01E0514,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_CGMSA
        /// MessageText:
        /// The protected output cannot enable the Content Generation Management System Analogue (CGMS-A) protection technology because it does not support CGMS-A.
        /// </summary>
        STATUS_GRAPHICS_OPM_OUTPUT_DOES_NOT_SUPPORT_CGMSA = 0xC01E0515,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_HDCP_SRM_NEVER_SET
        /// MessageText:
        /// The DxgkDdiOPMGetInformation function cannot return the version of the SRM being used because the application never successfully passed an SRM to the protected output.  
        /// </summary>
        STATUS_GRAPHICS_OPM_HDCP_SRM_NEVER_SET = 0xC01E0516,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_RESOLUTION_TOO_HIGH
        /// MessageText:
        /// The DxgkDdiOPMConfigureProtectedOutput function cannot enable the specified output protection technology because the output's screen resolution is too high.  
        /// </summary>
        STATUS_GRAPHICS_OPM_RESOLUTION_TOO_HIGH = 0xC01E0517,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_ALL_HDCP_HARDWARE_ALREADY_IN_USE
        /// MessageText:
        /// The DxgkDdiOPMConfigureProtectedOutput function cannot enable HDCP because the display adapter's HDCP hardware is already being used by other physical outputs.
        /// </summary>
        STATUS_GRAPHICS_OPM_ALL_HDCP_HARDWARE_ALREADY_IN_USE = 0xC01E0518,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_NO_LONGER_EXISTS
        /// MessageText:
        /// The operating system asynchronously destroyed this OPM protected output because the operating system's state changed. This error typically occurs because the monitor PDO associated with this protected output was removed, the monitor PDO associated with this protected output was stopped, or the protected output's session became a non-console session.
        /// </summary>
        STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_NO_LONGER_EXISTS = 0xC01E051A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_COPP_SEMANTICS
        /// MessageText:
        /// Either the DxgkDdiOPMGetCOPPCompatibleInformation, DxgkDdiOPMGetInformation, or DxgkDdiOPMConfigureProtectedOutput function failed. This error is only returned if a protected output has OPM semantics.  DxgkDdiOPMGetCOPPCompatibleInformation always returns this error if a protected output has OPM semantics.  DxgkDdiOPMGetInformation returns this error code if the caller requested COPP specific information.  DxgkDdiOPMConfigureProtectedOutput returns this error when the caller tries to use a COPP specific command.  
        /// </summary>
        STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_COPP_SEMANTICS = 0xC01E051C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_INVALID_INFORMATION_REQUEST
        /// MessageText:
        /// The DxgkDdiOPMGetInformation and DxgkDdiOPMGetCOPPCompatibleInformation functions return this error code if the passed in sequence number is not the expected sequence number or the passed in OMAC value is invalid.
        /// </summary>
        STATUS_GRAPHICS_OPM_INVALID_INFORMATION_REQUEST = 0xC01E051D,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_DRIVER_INTERNAL_ERROR
        /// MessageText:
        /// The function failed because an unexpected error occurred inside of a display driver.
        /// </summary>
        STATUS_GRAPHICS_OPM_DRIVER_INTERNAL_ERROR = 0xC01E051E,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_OPM_SEMANTICS
        /// MessageText:
        /// Either the DxgkDdiOPMGetCOPPCompatibleInformation, DxgkDdiOPMGetInformation, or DxgkDdiOPMConfigureProtectedOutput function failed. This error is only returned if a protected output has COPP semantics.  DxgkDdiOPMGetCOPPCompatibleInformation returns this error code if the caller requested OPM specific information.  DxgkDdiOPMGetInformation always returns this error if a protected output has COPP semantics.  DxgkDdiOPMConfigureProtectedOutput returns this error when the caller tries to use an OPM specific command.  
        /// </summary>
        STATUS_GRAPHICS_OPM_PROTECTED_OUTPUT_DOES_NOT_HAVE_OPM_SEMANTICS = 0xC01E051F,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_SIGNALING_NOT_SUPPORTED
        /// MessageText:
        /// The DxgkDdiOPMGetCOPPCompatibleInformation and DxgkDdiOPMConfigureProtectedOutput functions return this error if the display driver does not support the DXGKMDT_OPM_GET_ACP_AND_CGMSA_SIGNALING and DXGKMDT_OPM_SET_ACP_AND_CGMSA_SIGNALING GUIDs.
        /// </summary>
        STATUS_GRAPHICS_OPM_SIGNALING_NOT_SUPPORTED = 0xC01E0520,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_OPM_INVALID_CONFIGURATION_REQUEST
        /// MessageText:
        /// The DxgkDdiOPMConfigureProtectedOutput function returns this error code if the passed in sequence number is not the expected sequence number or the passed in OMAC value is invalid.
        /// </summary>
        STATUS_GRAPHICS_OPM_INVALID_CONFIGURATION_REQUEST = 0xC01E0521,

        /// <summary>
        ///   Monitor Configuration API status codes {0x0580..0x05DF}
        /// MessageId: STATUS_GRAPHICS_I2C_NOT_SUPPORTED
        /// MessageText:
        /// The monitor connected to the specified video output does not have an I2C bus.
        /// </summary>
        STATUS_GRAPHICS_I2C_NOT_SUPPORTED = 0xC01E0580,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_I2C_DEVICE_DOES_NOT_EXIST
        /// MessageText:
        /// No device on the I2C bus has the specified address.
        /// </summary>
        STATUS_GRAPHICS_I2C_DEVICE_DOES_NOT_EXIST = 0xC01E0581,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_I2C_ERROR_TRANSMITTING_DATA
        /// MessageText:
        /// An error occurred while transmitting data to the device on the I2C bus.
        /// </summary>
        STATUS_GRAPHICS_I2C_ERROR_TRANSMITTING_DATA = 0xC01E0582,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_I2C_ERROR_RECEIVING_DATA
        /// MessageText:
        /// An error occurred while receiving data from the device on the I2C bus.
        /// </summary>
        STATUS_GRAPHICS_I2C_ERROR_RECEIVING_DATA = 0xC01E0583,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED
        /// MessageText:
        /// The monitor does not support the specified VCP code.
        /// </summary>
        STATUS_GRAPHICS_DDCCI_VCP_NOT_SUPPORTED = 0xC01E0584,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DDCCI_INVALID_DATA
        /// MessageText:
        /// The data received from the monitor is invalid.
        /// </summary>
        STATUS_GRAPHICS_DDCCI_INVALID_DATA = 0xC01E0585,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DDCCI_MONITOR_RETURNED_INVALID_TIMING_STATUS_BYTE
        /// MessageText:
        /// The function failed because a monitor returned an invalid Timing Status byte when the operating system used the DDC/CI Get Timing Report &amp; Timing Message command to get a timing report from a monitor.
        /// </summary>
        STATUS_GRAPHICS_DDCCI_MONITOR_RETURNED_INVALID_TIMING_STATUS_BYTE = 0xC01E0586,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DDCCI_INVALID_CAPABILITIES_STRING
        /// MessageText:
        /// A monitor returned a DDC/CI capabilities string which did not comply with the ACCESS.bus 3.0, DDC/CI 1.1, or MCCS 2 Revision 1 specification.
        /// </summary>
        STATUS_GRAPHICS_DDCCI_INVALID_CAPABILITIES_STRING = 0xC01E0587,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MCA_INTERNAL_ERROR
        /// MessageText:
        /// An internal error caused an operation to fail.
        /// </summary>
        STATUS_GRAPHICS_MCA_INTERNAL_ERROR = 0xC01E0588,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_COMMAND
        /// MessageText:
        /// An operation failed because a DDC/CI message had an invalid value in its command field.
        /// </summary>
        STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_COMMAND = 0xC01E0589,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH
        /// MessageText:
        /// An error occurred because the field length of a DDC/CI message contained an invalid value.  
        /// </summary>
        STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_LENGTH = 0xC01E058A,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_CHECKSUM
        /// MessageText:
        /// An error occurred because the checksum field in a DDC/CI message did not match the message's computed checksum value. This error implies that the data was corrupted while it was being transmitted from a monitor to a computer.
        /// </summary>
        STATUS_GRAPHICS_DDCCI_INVALID_MESSAGE_CHECKSUM = 0xC01E058B,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE
        /// MessageText:
        /// This function failed because an invalid monitor handle was passed to it.
        /// </summary>
        STATUS_GRAPHICS_INVALID_PHYSICAL_MONITOR_HANDLE = 0xC01E058C,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MONITOR_NO_LONGER_EXISTS
        /// MessageText:
        /// The operating system asynchronously destroyed the monitor which corresponds to this handle because the operating system's state changed. This error typically occurs because the monitor PDO associated with this handle was removed, the monitor PDO associated with this handle was stopped, or a display mode change occurred. A display mode change occurs when windows sends a WM_DISPLAYCHANGE windows message to applications.
        /// </summary>
        STATUS_GRAPHICS_MONITOR_NO_LONGER_EXISTS = 0xC01E058D,

        /// <summary>
        ///   OPM, UAB, PVP and DDC/CI shared status codes {0x25E0..0x25FF}
        /// MessageId: STATUS_GRAPHICS_ONLY_CONSOLE_SESSION_SUPPORTED
        /// MessageText:
        /// This function can only be used if a program is running in the local console session. It cannot be used if a program is running on a remote desktop session or on a terminal server session.
        /// </summary>
        STATUS_GRAPHICS_ONLY_CONSOLE_SESSION_SUPPORTED = 0xC01E05E0,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_DISPLAY_DEVICE_CORRESPONDS_TO_NAME
        /// MessageText:
        /// This function cannot find an actual GDI display device which corresponds to the specified GDI display device name.
        /// </summary>
        STATUS_GRAPHICS_NO_DISPLAY_DEVICE_CORRESPONDS_TO_NAME = 0xC01E05E1,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_DISPLAY_DEVICE_NOT_ATTACHED_TO_DESKTOP
        /// MessageText:
        /// The function failed because the specified GDI display device was not attached to the Windows desktop.
        /// </summary>
        STATUS_GRAPHICS_DISPLAY_DEVICE_NOT_ATTACHED_TO_DESKTOP = 0xC01E05E2,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_MIRRORING_DEVICES_NOT_SUPPORTED
        /// MessageText:
        /// This function does not support GDI mirroring display devices because GDI mirroring display devices do not have any physical monitors associated with them.
        /// </summary>
        STATUS_GRAPHICS_MIRRORING_DEVICES_NOT_SUPPORTED = 0xC01E05E3,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INVALID_POINTER
        /// MessageText:
        /// The function failed because an invalid pointer parameter was passed to it. A pointer parameter is invalid if it is NULL, it points to an invalid address, it points to a kernel mode address or it is not correctly aligned.
        /// </summary>
        STATUS_GRAPHICS_INVALID_POINTER = 0xC01E05E4,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_NO_MONITORS_CORRESPOND_TO_DISPLAY_DEVICE
        /// MessageText:
        /// This function failed because the GDI device passed to it did not have any monitors associated with it.
        /// </summary>
        STATUS_GRAPHICS_NO_MONITORS_CORRESPOND_TO_DISPLAY_DEVICE = 0xC01E05E5,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_PARAMETER_ARRAY_TOO_SMALL
        /// MessageText:
        /// An array passed to the function cannot hold all of the data that the function must copy into the array.
        /// </summary>
        STATUS_GRAPHICS_PARAMETER_ARRAY_TOO_SMALL = 0xC01E05E6,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_INTERNAL_ERROR
        /// MessageText:
        /// An internal error caused an operation to fail.
        /// </summary>
        STATUS_GRAPHICS_INTERNAL_ERROR = 0xC01E05E7,

        /// <summary>
        /// MessageId: STATUS_GRAPHICS_SESSION_TYPE_CHANGE_IN_PROGRESS
        /// MessageText:
        /// The function failed because the current session is changing its type. This function cannot be called when the current session is changing its type.  There are currently three types of sessions: console, disconnected and remote (RDP or ICA).
        /// </summary>
        STATUS_GRAPHICS_SESSION_TYPE_CHANGE_IN_PROGRESS = 0xC01E05E8,

        /// <summary>
        /// Full Volume Encryption Error codes (fvevol.sys)
        /// MessageId: STATUS_FVE_LOCKED_VOLUME
        /// MessageText:
        /// This volume is locked by BitLocker Drive Encryption.
        /// </summary>
        STATUS_FVE_LOCKED_VOLUME = 0xC0210000,

        /// <summary>
        /// MessageId: STATUS_FVE_NOT_ENCRYPTED
        /// MessageText:
        /// The volume is not encrypted, no key is available.
        /// </summary>
        STATUS_FVE_NOT_ENCRYPTED = 0xC0210001,

        /// <summary>
        /// MessageId: STATUS_FVE_BAD_INFORMATION
        /// MessageText:
        /// The control block for the encrypted volume is not valid.
        /// </summary>
        STATUS_FVE_BAD_INFORMATION = 0xC0210002,

        /// <summary>
        /// MessageId: STATUS_FVE_TOO_SMALL
        /// MessageText:
        /// The volume cannot be encrypted because it does not have enough free space.
        /// </summary>
        STATUS_FVE_TOO_SMALL = 0xC0210003,

        /// <summary>
        /// MessageId: STATUS_FVE_FAILED_WRONG_FS
        /// MessageText:
        /// The volume cannot be encrypted because the file system is not supported.
        /// </summary>
        STATUS_FVE_FAILED_WRONG_FS = 0xC0210004,

        /// <summary>
        /// MessageId: STATUS_FVE_FAILED_BAD_FS
        /// MessageText:
        /// The file system is corrupt. Run CHKDSK.
        /// </summary>
        STATUS_FVE_FAILED_BAD_FS = 0xC0210005,

        /// <summary>
        /// MessageId: STATUS_FVE_FS_NOT_EXTENDED
        /// MessageText:
        /// The file system does not extend to the end of the volume.
        /// </summary>
        STATUS_FVE_FS_NOT_EXTENDED = 0xC0210006,

        /// <summary>
        /// MessageId: STATUS_FVE_FS_MOUNTED
        /// MessageText:
        /// This operation cannot be performed while a file system is mounted on the volume.
        /// </summary>
        STATUS_FVE_FS_MOUNTED = 0xC0210007,

        /// <summary>
        /// MessageId: STATUS_FVE_NO_LICENSE
        /// MessageText:
        /// BitLocker Drive Encryption is not included with this version of Windows.
        /// </summary>
        STATUS_FVE_NO_LICENSE = 0xC0210008,

        /// <summary>
        /// MessageId: STATUS_FVE_ACTION_NOT_ALLOWED
        /// MessageText:
        /// Requested action not allowed in the current volume state.
        /// </summary>
        STATUS_FVE_ACTION_NOT_ALLOWED = 0xC0210009,

        /// <summary>
        /// MessageId: STATUS_FVE_BAD_DATA
        /// MessageText:
        /// Data supplied is malformed.
        /// </summary>
        STATUS_FVE_BAD_DATA = 0xC021000A,

        /// <summary>
        /// MessageId: STATUS_FVE_VOLUME_NOT_BOUND
        /// MessageText:
        /// The volume is not bound to the system.
        /// </summary>
        STATUS_FVE_VOLUME_NOT_BOUND = 0xC021000B,

        /// <summary>
        /// MessageId: STATUS_FVE_NOT_DATA_VOLUME
        /// MessageText:
        /// That volume is not a data volume.
        /// </summary>
        STATUS_FVE_NOT_DATA_VOLUME = 0xC021000C,

        /// <summary>
        /// MessageId: STATUS_FVE_CONV_READ_ERROR
        /// MessageText:
        /// A read operation failed while converting the volume.
        /// </summary>
        STATUS_FVE_CONV_READ_ERROR = 0xC021000D,

        /// <summary>
        /// MessageId: STATUS_FVE_CONV_WRITE_ERROR
        /// MessageText:
        /// A write operation failed while converting the volume.
        /// </summary>
        STATUS_FVE_CONV_WRITE_ERROR = 0xC021000E,

        /// <summary>
        /// MessageId: STATUS_FVE_OVERLAPPED_UPDATE
        /// MessageText:
        /// The control block for the encrypted volume was updated by another thread. Try again.
        /// </summary>
        STATUS_FVE_OVERLAPPED_UPDATE = 0xC021000F,

        /// <summary>
        /// MessageId: STATUS_FVE_FAILED_SECTOR_SIZE
        /// MessageText:
        /// The encryption algorithm does not support the sector size of that volume.
        /// </summary>
        STATUS_FVE_FAILED_SECTOR_SIZE = 0xC0210010,

        /// <summary>
        /// MessageId: STATUS_FVE_FAILED_AUTHENTICATION
        /// MessageText:
        /// BitLocker recovery authentication failed.
        /// </summary>
        STATUS_FVE_FAILED_AUTHENTICATION = 0xC0210011,

        /// <summary>
        /// MessageId: STATUS_FVE_NOT_OS_VOLUME
        /// MessageText:
        /// That volume is not the OS volume.
        /// </summary>
        STATUS_FVE_NOT_OS_VOLUME = 0xC0210012,

        /// <summary>
        /// MessageId: STATUS_FVE_KEYFILE_NOT_FOUND
        /// MessageText:
        /// The BitLocker startup key or recovery password could not be read from external media.
        /// </summary>
        STATUS_FVE_KEYFILE_NOT_FOUND = 0xC0210013,

        /// <summary>
        /// MessageId: STATUS_FVE_KEYFILE_INVALID
        /// MessageText:
        /// The BitLocker startup key or recovery password file is corrupt or invalid.
        /// </summary>
        STATUS_FVE_KEYFILE_INVALID = 0xC0210014,

        /// <summary>
        /// MessageId: STATUS_FVE_KEYFILE_NO_VMK
        /// MessageText:
        /// The BitLocker encryption key could not be obtained from the startup key or recovery password.
        /// </summary>
        STATUS_FVE_KEYFILE_NO_VMK = 0xC0210015,

        /// <summary>
        /// MessageId: STATUS_FVE_TPM_DISABLED
        /// MessageText:
        /// The Trusted Platform Module (TPM) is disabled.
        /// </summary>
        STATUS_FVE_TPM_DISABLED = 0xC0210016,

        /// <summary>
        /// MessageId: STATUS_FVE_TPM_SRK_AUTH_NOT_ZERO
        /// MessageText:
        /// The authorization data for the Storage Root Key (SRK) of the Trusted Platform Module (TPM) is not zero.
        /// </summary>
        STATUS_FVE_TPM_SRK_AUTH_NOT_ZERO = 0xC0210017,

        /// <summary>
        /// MessageId: STATUS_FVE_TPM_INVALID_PCR
        /// MessageText:
        /// The system boot information changed or the Trusted Platform Module (TPM) locked out access to BitLocker encryption keys until the computer is restarted.
        /// </summary>
        STATUS_FVE_TPM_INVALID_PCR = 0xC0210018,

        /// <summary>
        /// MessageId: STATUS_FVE_TPM_NO_VMK
        /// MessageText:
        /// The BitLocker encryption key could not be obtained from the Trusted Platform Module (TPM).
        /// </summary>
        STATUS_FVE_TPM_NO_VMK = 0xC0210019,

        /// <summary>
        /// MessageId: STATUS_FVE_PIN_INVALID
        /// MessageText:
        /// The BitLocker encryption key could not be obtained from the Trusted Platform Module (TPM) and PIN.
        /// </summary>
        STATUS_FVE_PIN_INVALID = 0xC021001A,

        /// <summary>
        /// MessageId: STATUS_FVE_AUTH_INVALID_APPLICATION
        /// MessageText:
        /// A boot application hash does not match the hash computed when BitLocker was turned on.
        /// </summary>
        STATUS_FVE_AUTH_INVALID_APPLICATION = 0xC021001B,

        /// <summary>
        /// MessageId: STATUS_FVE_AUTH_INVALID_CONFIG
        /// MessageText:
        /// The Boot Configuration Data (BCD) settings are not supported or have changed since BitLocker was enabled.
        /// </summary>
        STATUS_FVE_AUTH_INVALID_CONFIG = 0xC021001C,

        /// <summary>
        /// MessageId: STATUS_FVE_DEBUGGER_ENABLED
        /// MessageText:
        /// Boot debugging is enabled.  Run bcdedit to turn it off.
        /// </summary>
        STATUS_FVE_DEBUGGER_ENABLED = 0xC021001D,

        /// <summary>
        /// MessageId: STATUS_FVE_DRY_RUN_FAILED
        /// MessageText:
        /// The BitLocker encryption key could not be obtained.
        /// </summary>
        STATUS_FVE_DRY_RUN_FAILED = 0xC021001E,

        /// <summary>
        /// MessageId: STATUS_FVE_BAD_METADATA_POINTER
        /// MessageText:
        /// The metadata disk region pointer is incorrect.
        /// </summary>
        STATUS_FVE_BAD_METADATA_POINTER = 0xC021001F,

        /// <summary>
        /// MessageId: STATUS_FVE_OLD_METADATA_COPY
        /// MessageText:
        /// The backup copy of the metadata is out of date.
        /// </summary>
        STATUS_FVE_OLD_METADATA_COPY = 0xC0210020,

        /// <summary>
        /// MessageId: STATUS_FVE_REBOOT_REQUIRED
        /// MessageText:
        /// No action was taken as a system reboot is required.
        /// </summary>
        STATUS_FVE_REBOOT_REQUIRED = 0xC0210021,

        /// <summary>
        /// MessageId: STATUS_FVE_RAW_ACCESS
        /// MessageText:
        /// No action was taken as BitLocker Drive Encryption is in RAW access mode.
        /// </summary>
        STATUS_FVE_RAW_ACCESS = 0xC0210022,

        /// <summary>
        /// MessageId: STATUS_FVE_RAW_BLOCKED
        /// MessageText:
        /// BitLocker Drive Encryption cannot enter raw access mode for this volume.
        /// </summary>
        STATUS_FVE_RAW_BLOCKED = 0xC0210023,

        /// <summary>
        /// FWP error codes (fwpkclnt.sys)
        /// MessageId: STATUS_FWP_CALLOUT_NOT_FOUND
        /// MessageText:
        /// The callout does not exist.
        /// </summary>
        STATUS_FWP_CALLOUT_NOT_FOUND = 0xC0220001,

        /// <summary>
        /// MessageId: STATUS_FWP_CONDITION_NOT_FOUND
        /// MessageText:
        /// The filter condition does not exist.
        /// </summary>
        STATUS_FWP_CONDITION_NOT_FOUND = 0xC0220002,

        /// <summary>
        /// MessageId: STATUS_FWP_FILTER_NOT_FOUND
        /// MessageText:
        /// The filter does not exist.
        /// </summary>
        STATUS_FWP_FILTER_NOT_FOUND = 0xC0220003,

        /// <summary>
        /// MessageId: STATUS_FWP_LAYER_NOT_FOUND
        /// MessageText:
        /// The layer does not exist.
        /// </summary>
        STATUS_FWP_LAYER_NOT_FOUND = 0xC0220004,

        /// <summary>
        /// MessageId: STATUS_FWP_PROVIDER_NOT_FOUND
        /// MessageText:
        /// The provider does not exist.
        /// </summary>
        STATUS_FWP_PROVIDER_NOT_FOUND = 0xC0220005,

        /// <summary>
        /// MessageId: STATUS_FWP_PROVIDER_CONTEXT_NOT_FOUND
        /// MessageText:
        /// The provider context does not exist.
        /// </summary>
        STATUS_FWP_PROVIDER_CONTEXT_NOT_FOUND = 0xC0220006,

        /// <summary>
        /// MessageId: STATUS_FWP_SUBLAYER_NOT_FOUND
        /// MessageText:
        /// The sublayer does not exist.
        /// </summary>
        STATUS_FWP_SUBLAYER_NOT_FOUND = 0xC0220007,

        /// <summary>
        /// MessageId: STATUS_FWP_NOT_FOUND
        /// MessageText:
        /// The object does not exist.
        /// </summary>
        STATUS_FWP_NOT_FOUND = 0xC0220008,

        /// <summary>
        /// MessageId: STATUS_FWP_ALREADY_EXISTS
        /// MessageText:
        /// An object with that GUID or LUID already exists.
        /// </summary>
        STATUS_FWP_ALREADY_EXISTS = 0xC0220009,

        /// <summary>
        /// MessageId: STATUS_FWP_IN_USE
        /// MessageText:
        /// The object is referenced by other objects so cannot be deleted.
        /// </summary>
        STATUS_FWP_IN_USE = 0xC022000A,

        /// <summary>
        /// MessageId: STATUS_FWP_DYNAMIC_SESSION_IN_PROGRESS
        /// MessageText:
        /// The call is not allowed from within a dynamic session.
        /// </summary>
        STATUS_FWP_DYNAMIC_SESSION_IN_PROGRESS = 0xC022000B,

        /// <summary>
        /// MessageId: STATUS_FWP_WRONG_SESSION
        /// MessageText:
        /// The call was made from the wrong session so cannot be completed.
        /// </summary>
        STATUS_FWP_WRONG_SESSION = 0xC022000C,

        /// <summary>
        /// MessageId: STATUS_FWP_NO_TXN_IN_PROGRESS
        /// MessageText:
        /// The call must be made from within an explicit transaction.
        /// </summary>
        STATUS_FWP_NO_TXN_IN_PROGRESS = 0xC022000D,

        /// <summary>
        /// MessageId: STATUS_FWP_TXN_IN_PROGRESS
        /// MessageText:
        /// The call is not allowed from within an explicit transaction.
        /// </summary>
        STATUS_FWP_TXN_IN_PROGRESS = 0xC022000E,

        /// <summary>
        /// MessageId: STATUS_FWP_TXN_ABORTED
        /// MessageText:
        /// The explicit transaction has been forcibly cancelled.
        /// </summary>
        STATUS_FWP_TXN_ABORTED = 0xC022000F,

        /// <summary>
        /// MessageId: STATUS_FWP_SESSION_ABORTED
        /// MessageText:
        /// The session has been cancelled.
        /// </summary>
        STATUS_FWP_SESSION_ABORTED = 0xC0220010,

        /// <summary>
        /// MessageId: STATUS_FWP_INCOMPATIBLE_TXN
        /// MessageText:
        /// The call is not allowed from within a read-only transaction.
        /// </summary>
        STATUS_FWP_INCOMPATIBLE_TXN = 0xC0220011,

        /// <summary>
        /// MessageId: STATUS_FWP_TIMEOUT
        /// MessageText:
        /// The call timed out while waiting to acquire the transaction lock.
        /// </summary>
        STATUS_FWP_TIMEOUT = 0xC0220012,

        /// <summary>
        /// MessageId: STATUS_FWP_NET_EVENTS_DISABLED
        /// MessageText:
        /// Collection of network diagnostic events is disabled.
        /// </summary>
        STATUS_FWP_NET_EVENTS_DISABLED = 0xC0220013,

        /// <summary>
        /// MessageId: STATUS_FWP_INCOMPATIBLE_LAYER
        /// MessageText:
        /// The operation is not supported by the specified layer.
        /// </summary>
        STATUS_FWP_INCOMPATIBLE_LAYER = 0xC0220014,

        /// <summary>
        /// MessageId: STATUS_FWP_KM_CLIENTS_ONLY
        /// MessageText:
        /// The call is allowed for kernel-mode callers only.
        /// </summary>
        STATUS_FWP_KM_CLIENTS_ONLY = 0xC0220015,

        /// <summary>
        /// MessageId: STATUS_FWP_LIFETIME_MISMATCH
        /// MessageText:
        /// The call tried to associate two objects with incompatible lifetimes.
        /// </summary>
        STATUS_FWP_LIFETIME_MISMATCH = 0xC0220016,

        /// <summary>
        /// MessageId: STATUS_FWP_BUILTIN_OBJECT
        /// MessageText:
        /// The object is built in so cannot be deleted.
        /// </summary>
        STATUS_FWP_BUILTIN_OBJECT = 0xC0220017,

        /// <summary>
        /// MessageId: STATUS_FWP_TOO_MANY_CALLOUTS
        /// MessageText:
        /// The maximum number of callouts has been reached.
        /// </summary>
        STATUS_FWP_TOO_MANY_CALLOUTS = 0xC0220018,

        /// <summary>
        /// MessageId: STATUS_FWP_NOTIFICATION_DROPPED
        /// MessageText:
        /// A notification could not be delivered because a message queue is at its maximum capacity.
        /// </summary>
        STATUS_FWP_NOTIFICATION_DROPPED = 0xC0220019,

        /// <summary>
        /// MessageId: STATUS_FWP_TRAFFIC_MISMATCH
        /// MessageText:
        /// The traffic parameters do not match those for the security association context.
        /// </summary>
        STATUS_FWP_TRAFFIC_MISMATCH = 0xC022001A,

        /// <summary>
        /// MessageId: STATUS_FWP_INCOMPATIBLE_SA_STATE
        /// MessageText:
        /// The call is not allowed for the current security association state.
        /// </summary>
        STATUS_FWP_INCOMPATIBLE_SA_STATE = 0xC022001B,

        /// <summary>
        /// MessageId: STATUS_FWP_NULL_POINTER
        /// MessageText:
        /// A required pointer is null.
        /// </summary>
        STATUS_FWP_NULL_POINTER = 0xC022001C,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_ENUMERATOR
        /// MessageText:
        /// An enumerator is not valid.
        /// </summary>
        STATUS_FWP_INVALID_ENUMERATOR = 0xC022001D,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_FLAGS
        /// MessageText:
        /// The flags field contains an invalid value.
        /// </summary>
        STATUS_FWP_INVALID_FLAGS = 0xC022001E,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_NET_MASK
        /// MessageText:
        /// A network mask is not valid.
        /// </summary>
        STATUS_FWP_INVALID_NET_MASK = 0xC022001F,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_RANGE
        /// MessageText:
        /// An FWP_RANGE is not valid.
        /// </summary>
        STATUS_FWP_INVALID_RANGE = 0xC0220020,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_INTERVAL
        /// MessageText:
        /// The time interval is not valid.
        /// </summary>
        STATUS_FWP_INVALID_INTERVAL = 0xC0220021,

        /// <summary>
        /// MessageId: STATUS_FWP_ZERO_LENGTH_ARRAY
        /// MessageText:
        /// An array that must contain at least one element is zero length.
        /// </summary>
        STATUS_FWP_ZERO_LENGTH_ARRAY = 0xC0220022,

        /// <summary>
        /// MessageId: STATUS_FWP_NULL_DISPLAY_NAME
        /// MessageText:
        /// The displayData.name field cannot be null.
        /// </summary>
        STATUS_FWP_NULL_DISPLAY_NAME = 0xC0220023,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_ACTION_TYPE
        /// MessageText:
        /// The action type is not one of the allowed action types for a filter.
        /// </summary>
        STATUS_FWP_INVALID_ACTION_TYPE = 0xC0220024,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_WEIGHT
        /// MessageText:
        /// The filter weight is not valid.
        /// </summary>
        STATUS_FWP_INVALID_WEIGHT = 0xC0220025,

        /// <summary>
        /// MessageId: STATUS_FWP_MATCH_TYPE_MISMATCH
        /// MessageText:
        /// A filter condition contains a match type that is not compatible with the operands.
        /// </summary>
        STATUS_FWP_MATCH_TYPE_MISMATCH = 0xC0220026,

        /// <summary>
        /// MessageId: STATUS_FWP_TYPE_MISMATCH
        /// MessageText:
        /// An FWP_VALUE or FWPM_CONDITION_VALUE is of the wrong type.
        /// </summary>
        STATUS_FWP_TYPE_MISMATCH = 0xC0220027,

        /// <summary>
        /// MessageId: STATUS_FWP_OUT_OF_BOUNDS
        /// MessageText:
        /// An integer value is outside the allowed range.
        /// </summary>
        STATUS_FWP_OUT_OF_BOUNDS = 0xC0220028,

        /// <summary>
        /// MessageId: STATUS_FWP_RESERVED
        /// MessageText:
        /// A reserved field is non-zero.
        /// </summary>
        STATUS_FWP_RESERVED = 0xC0220029,

        /// <summary>
        /// MessageId: STATUS_FWP_DUPLICATE_CONDITION
        /// MessageText:
        /// A filter cannot contain multiple conditions operating on a single field.
        /// </summary>
        STATUS_FWP_DUPLICATE_CONDITION = 0xC022002A,

        /// <summary>
        /// MessageId: STATUS_FWP_DUPLICATE_KEYMOD
        /// MessageText:
        /// A policy cannot contain the same keying module more than once.
        /// </summary>
        STATUS_FWP_DUPLICATE_KEYMOD = 0xC022002B,

        /// <summary>
        /// MessageId: STATUS_FWP_ACTION_INCOMPATIBLE_WITH_LAYER
        /// MessageText:
        /// The action type is not compatible with the layer.
        /// </summary>
        STATUS_FWP_ACTION_INCOMPATIBLE_WITH_LAYER = 0xC022002C,

        /// <summary>
        /// MessageId: STATUS_FWP_ACTION_INCOMPATIBLE_WITH_SUBLAYER
        /// MessageText:
        /// The action type is not compatible with the sublayer.
        /// </summary>
        STATUS_FWP_ACTION_INCOMPATIBLE_WITH_SUBLAYER = 0xC022002D,

        /// <summary>
        /// MessageId: STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_LAYER
        /// MessageText:
        /// The raw context or the provider context is not compatible with the layer.
        /// </summary>
        STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_LAYER = 0xC022002E,

        /// <summary>
        /// MessageId: STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_CALLOUT
        /// MessageText:
        /// The raw context or the provider context is not compatible with the callout.
        /// </summary>
        STATUS_FWP_CONTEXT_INCOMPATIBLE_WITH_CALLOUT = 0xC022002F,

        /// <summary>
        /// MessageId: STATUS_FWP_INCOMPATIBLE_AUTH_METHOD
        /// MessageText:
        /// The authentication method is not compatible with the policy type.
        /// </summary>
        STATUS_FWP_INCOMPATIBLE_AUTH_METHOD = 0xC0220030,

        /// <summary>
        /// MessageId: STATUS_FWP_INCOMPATIBLE_DH_GROUP
        /// MessageText:
        /// The Diffie-Hellman group is not compatible with the policy type.
        /// </summary>
        STATUS_FWP_INCOMPATIBLE_DH_GROUP = 0xC0220031,

        /// <summary>
        /// MessageId: STATUS_FWP_EM_NOT_SUPPORTED
        /// MessageText:
        /// An IKE policy cannot contain an Extended Mode policy.
        /// </summary>
        STATUS_FWP_EM_NOT_SUPPORTED = 0xC0220032,

        /// <summary>
        /// MessageId: STATUS_FWP_NEVER_MATCH
        /// MessageText:
        /// The enumeration template or subscription will never match any objects.
        /// </summary>
        STATUS_FWP_NEVER_MATCH = 0xC0220033,

        /// <summary>
        /// MessageId: STATUS_FWP_PROVIDER_CONTEXT_MISMATCH
        /// MessageText:
        /// The provider context is of the wrong type.
        /// </summary>
        STATUS_FWP_PROVIDER_CONTEXT_MISMATCH = 0xC0220034,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_PARAMETER
        /// MessageText:
        /// The parameter is incorrect.
        /// </summary>
        STATUS_FWP_INVALID_PARAMETER = 0xC0220035,

        /// <summary>
        /// MessageId: STATUS_FWP_TOO_MANY_SUBLAYERS
        /// MessageText:
        /// The maximum number of sublayers has been reached.
        /// </summary>
        STATUS_FWP_TOO_MANY_SUBLAYERS = 0xC0220036,

        /// <summary>
        /// MessageId: STATUS_FWP_CALLOUT_NOTIFICATION_FAILED
        /// MessageText:
        /// The notification function for a callout returned an error.
        /// </summary>
        STATUS_FWP_CALLOUT_NOTIFICATION_FAILED = 0xC0220037,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_AUTH_TRANSFORM
        /// MessageText:
        /// The IPsec authentication transform is not valid.
        /// </summary>
        STATUS_FWP_INVALID_AUTH_TRANSFORM = 0xC0220038,

        /// <summary>
        /// MessageId: STATUS_FWP_INVALID_CIPHER_TRANSFORM
        /// MessageText:
        /// The IPsec cipher transform is not valid.
        /// </summary>
        STATUS_FWP_INVALID_CIPHER_TRANSFORM = 0xC0220039,

        /// <summary>
        /// MessageId: STATUS_FWP_TCPIP_NOT_READY
        /// MessageText:
        /// The TCP/IP stack is not ready.
        /// </summary>
        STATUS_FWP_TCPIP_NOT_READY = 0xC0220100,

        /// <summary>
        /// MessageId: STATUS_FWP_INJECT_HANDLE_CLOSING
        /// MessageText:
        /// The injection handle is being closed by another thread.
        /// </summary>
        STATUS_FWP_INJECT_HANDLE_CLOSING = 0xC0220101,

        /// <summary>
        /// MessageId: STATUS_FWP_INJECT_HANDLE_STALE
        /// MessageText:
        /// The injection handle is stale.
        /// </summary>
        STATUS_FWP_INJECT_HANDLE_STALE = 0xC0220102,

        /// <summary>
        /// MessageId: STATUS_FWP_CANNOT_PEND
        /// MessageText:
        /// The classify cannot be pended.
        /// </summary>
        STATUS_FWP_CANNOT_PEND = 0xC0220103,

        /// <summary>
        /// NDIS error codes (ndis.sys)
        /// MessageId: STATUS_NDIS_CLOSING
        /// MessageText:
        /// The binding to the network interface is being closed.
        /// </summary>
        STATUS_NDIS_CLOSING = 0xC0230002,

        /// <summary>
        /// MessageId: STATUS_NDIS_BAD_VERSION
        /// MessageText:
        /// An invalid version was specified.
        /// </summary>
        STATUS_NDIS_BAD_VERSION = 0xC0230004,

        /// <summary>
        /// MessageId: STATUS_NDIS_BAD_CHARACTERISTICS
        /// MessageText:
        /// An invalid characteristics table was used.
        /// </summary>
        STATUS_NDIS_BAD_CHARACTERISTICS = 0xC0230005,

        /// <summary>
        /// MessageId: STATUS_NDIS_ADAPTER_NOT_FOUND
        /// MessageText:
        /// Failed to find the network interface or network interface is not ready.
        /// </summary>
        STATUS_NDIS_ADAPTER_NOT_FOUND = 0xC0230006,

        /// <summary>
        /// MessageId: STATUS_NDIS_OPEN_FAILED
        /// MessageText:
        /// Failed to open the network interface.
        /// </summary>
        STATUS_NDIS_OPEN_FAILED = 0xC0230007,

        /// <summary>
        /// MessageId: STATUS_NDIS_DEVICE_FAILED
        /// MessageText:
        /// Network interface has encountered an internal unrecoverable failure.
        /// </summary>
        STATUS_NDIS_DEVICE_FAILED = 0xC0230008,

        /// <summary>
        /// MessageId: STATUS_NDIS_MULTICAST_FULL
        /// MessageText:
        /// The multicast list on the network interface is full.
        /// </summary>
        STATUS_NDIS_MULTICAST_FULL = 0xC0230009,

        /// <summary>
        /// MessageId: STATUS_NDIS_MULTICAST_EXISTS
        /// MessageText:
        /// An attempt was made to add a duplicate multicast address to the list.
        /// </summary>
        STATUS_NDIS_MULTICAST_EXISTS = 0xC023000A,

        /// <summary>
        /// MessageId: STATUS_NDIS_MULTICAST_NOT_FOUND
        /// MessageText:
        /// At attempt was made to remove a multicast address that was never added.
        /// </summary>
        STATUS_NDIS_MULTICAST_NOT_FOUND = 0xC023000B,

        /// <summary>
        /// MessageId: STATUS_NDIS_REQUEST_ABORTED
        /// MessageText:
        /// Network interface aborted the request.
        /// </summary>
        STATUS_NDIS_REQUEST_ABORTED = 0xC023000C,

        /// <summary>
        /// MessageId: STATUS_NDIS_RESET_IN_PROGRESS
        /// MessageText:
        /// Network interface can not process the request because it is being reset.
        /// </summary>
        STATUS_NDIS_RESET_IN_PROGRESS = 0xC023000D,

        /// <summary>
        /// MessageId: STATUS_NDIS_NOT_SUPPORTED
        /// MessageText:
        /// Network interface does not support this request.
        /// </summary>
        STATUS_NDIS_NOT_SUPPORTED = 0xC02300BB,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_PACKET
        /// MessageText:
        /// An attempt was made to send an invalid packet on a network interface.
        /// </summary>
        STATUS_NDIS_INVALID_PACKET = 0xC023000F,

        /// <summary>
        /// MessageId: STATUS_NDIS_ADAPTER_NOT_READY
        /// MessageText:
        /// Network interface is not ready to complete this operation.
        /// </summary>
        STATUS_NDIS_ADAPTER_NOT_READY = 0xC0230011,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_LENGTH
        /// MessageText:
        /// The length of the buffer submitted for this operation is not valid.
        /// </summary>
        STATUS_NDIS_INVALID_LENGTH = 0xC0230014,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_DATA
        /// MessageText:
        /// The data used for this operation is not valid.
        /// </summary>
        STATUS_NDIS_INVALID_DATA = 0xC0230015,

        /// <summary>
        /// MessageId: STATUS_NDIS_BUFFER_TOO_SHORT
        /// MessageText:
        /// The length of buffer submitted for this operation is too small.
        /// </summary>
        STATUS_NDIS_BUFFER_TOO_SHORT = 0xC0230016,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_OID
        /// MessageText:
        /// Network interface does not support this OID (Object Identifier)
        /// </summary>
        STATUS_NDIS_INVALID_OID = 0xC0230017,

        /// <summary>
        /// MessageId: STATUS_NDIS_ADAPTER_REMOVED
        /// MessageText:
        /// The network interface has been removed.
        /// </summary>
        STATUS_NDIS_ADAPTER_REMOVED = 0xC0230018,

        /// <summary>
        /// MessageId: STATUS_NDIS_UNSUPPORTED_MEDIA
        /// MessageText:
        /// Network interface does not support this media type.
        /// </summary>
        STATUS_NDIS_UNSUPPORTED_MEDIA = 0xC0230019,

        /// <summary>
        /// MessageId: STATUS_NDIS_GROUP_ADDRESS_IN_USE
        /// MessageText:
        /// An attempt was made to remove a token ring group address that is in use by other components.
        /// </summary>
        STATUS_NDIS_GROUP_ADDRESS_IN_USE = 0xC023001A,

        /// <summary>
        /// MessageId: STATUS_NDIS_FILE_NOT_FOUND
        /// MessageText:
        /// An attempt was made to map a file that can not be found.
        /// </summary>
        STATUS_NDIS_FILE_NOT_FOUND = 0xC023001B,

        /// <summary>
        /// MessageId: STATUS_NDIS_ERROR_READING_FILE
        /// MessageText:
        /// An error occurred while NDIS tried to map the file.
        /// </summary>
        STATUS_NDIS_ERROR_READING_FILE = 0xC023001C,

        /// <summary>
        /// MessageId: STATUS_NDIS_ALREADY_MAPPED
        /// MessageText:
        /// An attempt was made to map a file that is already mapped.
        /// </summary>
        STATUS_NDIS_ALREADY_MAPPED = 0xC023001D,

        /// <summary>
        /// MessageId: STATUS_NDIS_RESOURCE_CONFLICT
        /// MessageText:
        /// An attempt to allocate a hardware resource failed because the resource is used by another component.
        /// </summary>
        STATUS_NDIS_RESOURCE_CONFLICT = 0xC023001E,

        /// <summary>
        /// MessageId: STATUS_NDIS_MEDIA_DISCONNECTED
        /// MessageText:
        /// The I/O operation failed because network media is disconnected or wireless access point is out of range.
        /// </summary>
        STATUS_NDIS_MEDIA_DISCONNECTED = 0xC023001F,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_ADDRESS
        /// MessageText:
        /// The network address used in the request is invalid.
        /// </summary>
        STATUS_NDIS_INVALID_ADDRESS = 0xC0230022,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_DEVICE_REQUEST
        /// MessageText:
        /// The specified request is not a valid operation for the target device.
        /// </summary>
        STATUS_NDIS_INVALID_DEVICE_REQUEST = 0xC0230010,

        /// <summary>
        /// MessageId: STATUS_NDIS_PAUSED
        /// MessageText:
        /// The offload operation on the network interface has been paused.
        /// </summary>
        STATUS_NDIS_PAUSED = 0xC023002A,

        /// <summary>
        /// MessageId: STATUS_NDIS_INTERFACE_NOT_FOUND
        /// MessageText:
        /// Network interface was not found.
        /// </summary>
        STATUS_NDIS_INTERFACE_NOT_FOUND = 0xC023002B,

        /// <summary>
        /// MessageId: STATUS_NDIS_UNSUPPORTED_REVISION
        /// MessageText:
        /// The revision number specified in the structure is not supported.
        /// </summary>
        STATUS_NDIS_UNSUPPORTED_REVISION = 0xC023002C,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_PORT
        /// MessageText:
        /// The specified port does not exist on this network interface.
        /// </summary>
        STATUS_NDIS_INVALID_PORT = 0xC023002D,

        /// <summary>
        /// MessageId: STATUS_NDIS_INVALID_PORT_STATE
        /// MessageText:
        /// The current state of the specified port on this network interface does not support the requested operation.
        /// </summary>
        STATUS_NDIS_INVALID_PORT_STATE = 0xC023002E,

        /// <summary>
        /// MessageId: STATUS_NDIS_LOW_POWER_STATE
        /// MessageText:
        /// The miniport adapter is in lower power state.
        /// </summary>
        STATUS_NDIS_LOW_POWER_STATE = 0xC023002F,

        /// <summary>
        /// NDIS error codes (802.11 wireless LAN)
        /// MessageId: STATUS_NDIS_DOT11_AUTO_CONFIG_ENABLED
        /// MessageText:
        /// The wireless local area network interface is in auto configuration mode and doesn't support the requested parameter change operation.
        /// </summary>
        STATUS_NDIS_DOT11_AUTO_CONFIG_ENABLED = 0xC0232000,

        /// <summary>
        /// MessageId: STATUS_NDIS_DOT11_MEDIA_IN_USE
        /// MessageText:
        /// The wireless local area network interface is busy and can not perform the requested operation.
        /// </summary>
        STATUS_NDIS_DOT11_MEDIA_IN_USE = 0xC0232001,

        /// <summary>
        /// MessageId: STATUS_NDIS_DOT11_POWER_STATE_INVALID
        /// MessageText:
        /// The wireless local area network interface is power down and doesn't support the requested operation.
        /// </summary>
        STATUS_NDIS_DOT11_POWER_STATE_INVALID = 0xC0232002,

        /// <summary>
        /// NDIS informational codes(ndis.sys)
        /// MessageId: STATUS_NDIS_INDICATION_REQUIRED
        /// MessageText:
        /// The request will be completed later by NDIS status indication.
        /// </summary>
        STATUS_NDIS_INDICATION_REQUIRED = 0x40230001,

        /// <summary>
        /// IPSEC error codes (tcpip.sys)
        /// MessageId: STATUS_IPSEC_BAD_SPI
        /// MessageText:
        /// The SPI in the packet does not match a valid IPsec SA.
        /// </summary>
        STATUS_IPSEC_BAD_SPI = 0xC0360001,

        /// <summary>
        /// MessageId: STATUS_IPSEC_SA_LIFETIME_EXPIRED
        /// MessageText:
        /// Packet was received on an IPsec SA whose lifetime has expired.
        /// </summary>
        STATUS_IPSEC_SA_LIFETIME_EXPIRED = 0xC0360002,

        /// <summary>
        /// MessageId: STATUS_IPSEC_WRONG_SA
        /// MessageText:
        /// Packet was received on an IPsec SA that doesn't match the packet characteristics.
        /// </summary>
        STATUS_IPSEC_WRONG_SA = 0xC0360003,

        /// <summary>
        /// MessageId: STATUS_IPSEC_REPLAY_CHECK_FAILED
        /// MessageText:
        /// Packet sequence number replay check failed.
        /// </summary>
        STATUS_IPSEC_REPLAY_CHECK_FAILED = 0xC0360004,

        /// <summary>
        /// MessageId: STATUS_IPSEC_INVALID_PACKET
        /// MessageText:
        /// IPsec header and/or trailer in the packet is invalid.
        /// </summary>
        STATUS_IPSEC_INVALID_PACKET = 0xC0360005,

        /// <summary>
        /// MessageId: STATUS_IPSEC_INTEGRITY_CHECK_FAILED
        /// MessageText:
        /// IPsec integrity check failed.
        /// </summary>
        STATUS_IPSEC_INTEGRITY_CHECK_FAILED = 0xC0360006,

        /// <summary>
        /// MessageId: STATUS_IPSEC_CLEAR_TEXT_DROP
        /// MessageText:
        /// IPsec dropped a clear text packet.
        /// </summary>
        STATUS_IPSEC_CLEAR_TEXT_DROP = 0xC0360007,
    }
}
