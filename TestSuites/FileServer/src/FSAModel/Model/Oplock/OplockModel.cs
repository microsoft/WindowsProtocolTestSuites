// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    /// <summary>
    /// MS-FSA model program
    /// </summary>
    public static partial class ModelProgram
    {
        #region 3.1.5.17 Server Requests an Oplock

        /// <summary>
        /// Server Requests an Oplock
        /// </summary>
        /// <param name="opType">Oplock type</param>
        /// <param name="openListContainStream">Open.File.OpenList contains more than one 
        /// Open whose stream is the same as Open.Stream.</param>
        /// <param name="opLockLevel">Requested oplock level</param>
        /// <param name="isOpenStreamOplockEmpty">True : if Open.Steam.Oplock is empty</param>
        /// <param name="oplockState">The current state of the oplock, expressed as a combination of one or more flags</param>
        /// <param name="streamIsDeleted">True : if stream is deleted.</param>
        /// <param name="isRHBreakQueueEmpty">True: if Open.Stream.Oplock.State.RHBreakQueue is empty</param>
        /// <param name="isOplockKeyEqual">True: if ThisOpen.OplockKey == Open.OplockKey</param>
        /// <param name="isOplockKeyEqualExclusive">False: if Open.OplockKey != 
        /// Open.Stream.Oplock.ExclusiveOpen.OplockKey</param>
        /// <param name="requestOplock">Request oplock level</param>
        /// <param name="GrantingInAck">A boolean value, true if this oplock is being requested as part of an oplock break acknowledgement, 
        /// false if not</param>
        /// <param name="keyEqualOnRHOplocks">True: if there is an Open on Open.Stream.Oplock.RHOplocks whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnRHBreakQueue">True: if there is an Open on Open.Stream.Oplock.RHBreakQueue whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnROplocks">True: if there is an Open ThisOpen on Open.Stream.Oplock.ROplocks whose OplockKey is equal to Open.OplockKey</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        [Rule(Action = @"Oplock(opType,openListContainStream,opLockLevel,isOpenStreamOplockEmpty,oplockState,
            streamIsDeleted,isRHBreakQueueEmpty,isOplockKeyEqual,isOplockKeyEqualExclusive,requestOplock,
            GrantingInAck,keyEqualOnRHOplocks,keyEqualOnRHBreakQueue,keyEqualOnROplocks)/result")]
        public static MessageStatus Oplock(
            OpType opType,
            bool openListContainStream,
            RequestedOplockLevel opLockLevel,
            bool isOpenStreamOplockEmpty,
            OplockState oplockState,
            bool streamIsDeleted,
            bool isRHBreakQueueEmpty,
            bool isOplockKeyEqual,
            bool isOplockKeyEqualExclusive,
            RequestedOplock requestOplock,
            bool GrantingInAck,
            bool keyEqualOnRHOplocks,
            bool keyEqualOnRHBreakQueue,
            bool keyEqualOnROplocks
            )
        {
            Combination.NWise(13, opType,
            openListContainStream,
            opLockLevel,
            isOpenStreamOplockEmpty,
            oplockState,
            streamIsDeleted,
            isRHBreakQueueEmpty,
            isOplockKeyEqual,
            isOplockKeyEqualExclusive,
            requestOplock,
            GrantingInAck,
            keyEqualOnRHOplocks,
            keyEqualOnRHBreakQueue,
            keyEqualOnROplocks);

            Combination.Expand(opType,
            openListContainStream,
            opLockLevel,
            isOpenStreamOplockEmpty,
            oplockState,
            streamIsDeleted,
            isRHBreakQueueEmpty,
            isOplockKeyEqual,
            isOplockKeyEqualExclusive,
            requestOplock,
            GrantingInAck,
            keyEqualOnRHOplocks,
            keyEqualOnRHBreakQueue,
            keyEqualOnROplocks);

            Condition.IsTrue((opLockLevel == RequestedOplockLevel.READ_CACHING) ||
                (opLockLevel == (RequestedOplockLevel.READ_CACHING |
                RequestedOplockLevel.WRITE_CACHING)) ||
                (opLockLevel == (RequestedOplockLevel.READ_CACHING |
                RequestedOplockLevel.HANDLE_CACHING)) ||
                opLockLevel == (RequestedOplockLevel.READ_CACHING |
                RequestedOplockLevel.HANDLE_CACHING | RequestedOplockLevel.WRITE_CACHING) ||
                opLockLevel == RequestedOplockLevel.ZERO);

            //If Type is LEVEL_EXCLUSIVE or LEVEL_BATCH:
            if (opType == OpType.LEVEL_BATCH || opType == OpType.LEVEL_ONE)
            {
                //Open.File.OpenList contains more than 
                //one Open whose Stream is the same as Open.Stream.
                if (openListContainStream)
                {
                    Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                    Helper.CaptureRequirement(6314, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        If Type is LEVEL_EXCLUSIVE or LEVEL_BATCH:The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED 
                        under either of the following conditions:Open.File.OpenList contains more than one Open whose Stream is the same as Open.Stream.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

                //Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or 
                //FILE_SYNCHRONOUS_IO_NONALERT.
                if (((gOpenMode & CreateOptions.SYNCHRONOUS_IO_ALERT) != 0) ||
                    ((gOpenMode & CreateOptions.SYNCHRONOUS_IO_NONALERT) != 0))
                {
                    Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                    Helper.CaptureRequirement(6315, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:
                        If Type is LEVEL_EXCLUSIVE or LEVEL_BATCH:]The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED 
                        under either of the following conditions:Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or FILE_SYNCHRONOUS_IO_NONALERT.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

                //call 3.1.5.17.1
                return ExclusiveOplock(isOpenStreamOplockEmpty, oplockState, openListContainStream, streamIsDeleted, isRHBreakQueueEmpty, isOplockKeyEqual, isOplockKeyEqualExclusive, requestOplock);
            }
            //Else If Type is LEVEL_TWO:
            else if (opType == OpType.LEVEL_TWO)
            {
                //Open.Stream.ByteRangeLockList is not empty.
                if (ByteRangeLockList.Count != 0)
                {
                    Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                    Helper.CaptureRequirement(6319, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Else If Type is LEVEL_TWO:The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED under either of the following conditions:
                        Open.Stream.ByteRangeLockList is not empty.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

                //Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or 
                //FILE_SYNCHRONOUS_IO_NONALERT.
                if (((gOpenMode & CreateOptions.SYNCHRONOUS_IO_ALERT) != 0) ||
                    ((gOpenMode & CreateOptions.SYNCHRONOUS_IO_NONALERT) != 0))
                {
                    Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                    Helper.CaptureRequirement(6320, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:
                        Else If Type is LEVEL_TWO:]The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED under either of the following conditions:
                        Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or FILE_SYNCHRONOUS_IO_NONALERT.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }
                //call 3.1.5.17.2
                return SharedOplock(isOpenStreamOplockEmpty, oplockState, GrantingInAck, keyEqualOnRHOplocks, keyEqualOnRHBreakQueue, keyEqualOnROplocks, streamIsDeleted, requestOplock);
            }
            //Else If Type is LEVEL_GRANULAR
            else if (opType == OpType.LEVEL_GRANULAR)
            {
                //If RequestedOplockLevel is READ_CACHING or (READ_CACHING|HANDLE_CACHING):
                if ((opLockLevel == RequestedOplockLevel.READ_CACHING) ||
                    (opLockLevel ==
                    (RequestedOplockLevel.READ_CACHING | RequestedOplockLevel.HANDLE_CACHING)))
                {
                    //Open.Stream.ByteRangeLockList is not empty
                    if (ByteRangeLockList.Count != 0)
                    {
                        Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                        Helper.CaptureRequirement(6325, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                            Else If Type is LEVEL_GRANULAR:If RequestedOplockLevel is READ_CACHING or (READ_CACHING|HANDLE_CACHING):
                            The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED under either of the following conditions:
                            Open.Stream.ByteRangeLockList is not empty.");
                        return MessageStatus.OPLOCK_NOT_GRANTED;
                    }

                    //Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT 
                    //or FILE_SYNCHRONOUS_IO_NONALERT.
                    if (((gOpenMode & CreateOptions.SYNCHRONOUS_IO_ALERT) != 0) ||
                    ((gOpenMode & CreateOptions.SYNCHRONOUS_IO_NONALERT) != 0))
                    {
                        Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                        Helper.CaptureRequirement(6326, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:
                            Else If Type is LEVEL_GRANULAR:If RequestedOplockLevel is READ_CACHING or (READ_CACHING|HANDLE_CACHING):]
                            The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED under either of the following conditions:Open.Mode contains 
                            either FILE_SYNCHRONOUS_IO_ALERT or FILE_SYNCHRONOUS_IO_NONALERT.");
                        return MessageStatus.OPLOCK_NOT_GRANTED;
                    }
                    //call 3.1.5.17.2
                    return SharedOplock(isOpenStreamOplockEmpty, oplockState, GrantingInAck, keyEqualOnRHOplocks, keyEqualOnRHBreakQueue, keyEqualOnROplocks, streamIsDeleted, requestOplock);
                }
                //Else If RequestedOplockLevel is (READ_CACHING|WRITE_CACHING) or 
                //(READ_CACHING|WRITE_CACHING|HANDLE_CACHING):
                else if ((opLockLevel ==
                    (RequestedOplockLevel.READ_CACHING | RequestedOplockLevel.WRITE_CACHING)) ||
                    (opLockLevel == (RequestedOplockLevel.READ_CACHING |
                    RequestedOplockLevel.WRITE_CACHING |
                    RequestedOplockLevel.HANDLE_CACHING)))
                {
                    //If Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or 
                    //FILE_SYNCHRONOUS_IO_NONALERT, 
                    //the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.
                    if ((gOpenMode & CreateOptions.SYNCHRONOUS_IO_ALERT) != 0 ||
                    (gOpenMode & CreateOptions.SYNCHRONOUS_IO_NONALERT) != 0)
                    {
                        Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                        Helper.CaptureRequirement(6331, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:
                            Else If Type is LEVEL_GRANULAR:If RequestedOplockLevel is READ_CACHING or (READ_CACHING|HANDLE_CACHING):]
                            Else If RequestedOplockLevel is (READ_CACHING|WRITE_CACHING) or (READ_CACHING|WRITE_CACHING|HANDLE_CACHING):
                            If Open.Mode contains either FILE_SYNCHRONOUS_IO_ALERT or FILE_SYNCHRONOUS_IO_NONALERT, 
                            the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                        return MessageStatus.OPLOCK_NOT_GRANTED;
                    }
                    //call 3.1.5.17.1
                    return ExclusiveOplock(isOpenStreamOplockEmpty, oplockState, openListContainStream, streamIsDeleted,
                        isRHBreakQueueEmpty, isOplockKeyEqual, isOplockKeyEqualExclusive, requestOplock);
                }
                //Else if RequestedOplockLevel is 0 (that is, no flags):
                else if (opLockLevel == RequestedOplockLevel.ZERO)
                {
                    Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                    Helper.CaptureRequirement(6335, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:
                        Else If Type is LEVEL_GRANULAR:If RequestedOplockLevel is READ_CACHING or (READ_CACHING|HANDLE_CACHING):]
                        Else if RequestedOplockLevel is 0 (that is, no flags):The operation MUST immediately return STATUS_SUCCESS.");
                    return MessageStatus.SUCCESS;
                }
                else
                {
                    Helper.CaptureRequirement(6306, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:]
                        Whether the oplock is granted or not, the object store MUST return:[Status].");
                    Helper.CaptureRequirement(6336, @"[In Server Requests an Oplock,Pseudocode for the operation is as follows:
                        Else If Type is LEVEL_GRANULAR:If RequestedOplockLevel is READ_CACHING or (READ_CACHING|HANDLE_CACHING):]
                        Else The operation MUST be failed with STATUS_INVALID_PARAMETER.");
                    return MessageStatus.INVALID_PARAMETER;
                }
            }

            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.18 Server Acknowledges an Oplock Break

        /// <summary>
        /// Server Acknowledges an Oplock Break
        /// </summary>
        /// <param name="OpenStreamOplockEmpty">true if Open.Stream.Oplock is empty</param>
        /// <param name="opType">Oplock type</param>
        /// <param name="level">Requested Oplock level</param>
        /// <param name="ExclusiveOpenEqual">True if Open.Stream.Oplock.ExclusiveOpen is not equal to Open</param>
        /// <param name="oplockState">Oplock state</param>
        /// <param name="RHBreakQueueIsEmpty">True if Open.Stream.Oplock.RHBreakQueue is empty</param>
        /// <param name="ThisContextOpenEqual">True if ThisContext.Open equals Open</param>
        /// <param name="ThisContextBreakingToRead">False if ThisContext.BreakingToRead is FALSE</param>
        /// <param name="OplockWaitListEmpty">False if Open.Stream.Oplock.WaitList is not empty</param>
        /// <param name="StreamIsDeleted">True if Open.Stream.IsDeleted is true</param>
        /// <param name="GrantingInAck">A boolean value.
        /// True: if this oplock is requested as part of an oplock break acknowledgement, 
        /// false if not</param>
        /// <param name="keyEqualOnRHOplocks">True: if there is an open on Open.Stream.Oplock.RHOplocks whose oplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnRHBreakQueue">True: if there is an Open on Open.Stream.Oplock.RHBreakQueue whose oplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnROplocks">True: if there is an open ThisOpen on Open.Stream.Oplock.ROplocks whose oplockKey is equal to Open.OplockKey</param>
        /// <param name="requestOplock">The oplock type being requested.</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [Rule]
        public static MessageStatus OplockBreakAcknowledge(
            bool OpenStreamOplockEmpty,
            OpType opType,
            RequestedOplockLevel level,
            bool ExclusiveOpenEqual,
            OplockState oplockState,
            bool RHBreakQueueIsEmpty,
            bool ThisContextOpenEqual,
            bool ThisContextBreakingToRead,
            bool OplockWaitListEmpty,
            bool StreamIsDeleted,
            bool GrantingInAck,
            bool keyEqualOnRHOplocks,
            bool keyEqualOnRHBreakQueue,
            bool keyEqualOnROplocks,
            RequestedOplock requestOplock
            )
        {
            bool NewOplockGranted = false;
            bool ReturnBreakToNone = false;
            bool FoundMatchingRHOplock = false;
            OplockState BREAK_LEVEL_MASK, R_AND_RH_GRANTED, RH_GRANTED;

            //If Open.Stream.Oplock is empty, the operation MUST be failed 
            //with STATUS_INVALID_OPLOCK_PROTOCOL.
            if (OpenStreamOplockEmpty)
            {
                Helper.CaptureRequirement(6450, @"[In Server Acknowledges an Oplock Break]Pseudocode for the operation is as follows:
                    If Open.Stream.Oplock is empty, the operation MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.");
                return MessageStatus.INVALID_OPLOCK_PROTOCOL;
            }

            if (opType == OpType.LEVEL_ONE || opType == OpType.LEVEL_TWO)
            {
                //If Open.Stream.Oplock.ExclusiveOpen is not equal to Open, 
                //the operation MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.
                if (!ExclusiveOpenEqual)
                {
                    Helper.CaptureRequirement(6451, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:]
                        If Type is LEVEL_NONE or LEVEL_TWO:If Open.Stream.Oplock.ExclusiveOpen is not equal to Open, 
                        the operation MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.");
                    return MessageStatus.INVALID_OPLOCK_PROTOCOL;
                }

                if (opType == OpType.LEVEL_TWO &&
                    (oplockState & OplockState.BREAK_TO_TWO) != 0)
                {
                    oplockState = OplockState.LEVEL_TWO_OPLOCK;
                    NewOplockGranted = true;
                }
                else if ((oplockState & OplockState.BREAK_TO_TWO) != 0 ||
                    (oplockState & OplockState.BREAK_TO_TWO_TO_NONE) != 0)
                {
                    oplockState = OplockState.NO_OPLOCK;
                }
                else if ((oplockState & OplockState.BREAK_TO_TWO_TO_NONE) != 0)
                {
                    oplockState = OplockState.NO_OPLOCK;
                    ReturnBreakToNone = true;
                }
                else
                {
                    Helper.CaptureRequirement(6457, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:]
                        Else[If Type is not LEVEL_NONE and not LEVEL_TWO:The operation MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.");
                    return MessageStatus.INVALID_OPLOCK_PROTOCOL;
                }


                if (NewOplockGranted)
                {
                    //The operation waits until the newly-granted Level 2 oplock is broken
                }
                else if (ReturnBreakToNone)
                {
                    //call 3.1.5.17.3
                    NewOplockLevel newLevel = NewOplockLevel.LEVEL_NONE;
                    bool b = false;
                    MessageStatus m = MessageStatus.SUCCESS;
                    IndicateOplockBreak(ref newLevel, ref b, ref m);
                    Helper.CaptureRequirement(6465, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                        Else If ReturnBreakToNone is TRUE:]In this case the server was expecting the oplock to break to Level 2, 
                        but because the oplock is actually breaking to None (that is, no oplock), 
                        the object store MUST indicate an oplock break to the server according to the algorithm in section 3.1.5.17.3, 
                        setting the algorithm's parameters as follows:ReturnStatus equal to STATUS_SUCCESS.");
                }
                else
                {
                    Helper.CaptureRequirement(6466, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:]
                        Else[If ReturnBreakToNone is FALSE]The operation MUST immediately return STATUS_SUCCESS.");
                    return MessageStatus.SUCCESS;
                }
            }
            else if (opType == OpType.LEVEL_GRANULAR)
            {
                //Let BREAK_LEVEL_MASK = (BREAK_TO_READ_CACHING | BREAK_TO_WRITE_CACHING | 
                //BREAK_TO_HANDLE_CACHING | BREAK_TO_NO_CACHING)
                BREAK_LEVEL_MASK = OplockState.BREAK_TO_READ_CACHING |
                    OplockState.BREAK_TO_WRITE_CACHING |
                    OplockState.BREAK_TO_HANDLE_CACHING |
                    OplockState.BREAK_TO_NO_CACHING;

                //Let R_AND_RH_GRANTED = (READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH)
                R_AND_RH_GRANTED = OplockState.READ_CACHING |
                    OplockState.HANDLE_CACHING |
                    OplockState.MIXED_R_AND_RH;

                //Let RH_GRANTED = (READ_CACHING|HANDLE_CACHING)
                RH_GRANTED = OplockState.READ_CACHING | OplockState.HANDLE_CACHING;

                if ((((oplockState & BREAK_LEVEL_MASK) == 0) &&
                    (oplockState != R_AND_RH_GRANTED) &&
                    (oplockState != RH_GRANTED)) ||
                    (((oplockState == R_AND_RH_GRANTED) ||
                    (oplockState == RH_GRANTED)) && RHBreakQueueIsEmpty))
                {
                    Helper.CaptureRequirement(6471, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                        Else If Type is LEVEL_GRANULAR:]If (Open.Stream.Oplock.State does not contain any flag in BREAK_LEVEL_MASK 
                        and (Open.Stream.Oplock.State != R_AND_RH_GRANTED) and (Open.Stream.Oplock.State != RH_GRANTED)) 
                        or (((Open.Stream.Oplock.State == R_AND_RH_GRANTED) or (Open.Stream.Oplock.State == RH_GRANTED)) 
                        and Open.Stream.Oplock.RHBreakQueue is empty):The request MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.");
                    return MessageStatus.INVALID_OPLOCK_PROTOCOL;
                }

                switch (oplockState)
                {
                    case OplockState.READ_CACHING | OplockState.HANDLE_CACHING | OplockState.MIXED_R_AND_RH:
                    case OplockState.READ_CACHING | OplockState.HANDLE_CACHING:
                    case OplockState.READ_CACHING | OplockState.HANDLE_CACHING | OplockState.BREAK_TO_READ_CACHING:
                    case OplockState.READ_CACHING | OplockState.HANDLE_CACHING | OplockState.BREAK_TO_NO_CACHING:
                        {
                            //begin first for lop
                            if (ThisContextOpenEqual)
                            {
                                FoundMatchingRHOplock = true;
                                //If ThisContext.BreakingToRead is FALSE:
                                if (!ThisContextBreakingToRead)
                                {
                                    //If RequestedOplockLevel is not 0 and 
                                    //Open.Stream.Oplock.WaitList is not empty:
                                    if (level != RequestedOplockLevel.ZERO &&
                                        !OplockWaitListEmpty)
                                    {
                                        //call 3.1.5.17.3
                                        NewOplockLevel newLevel = NewOplockLevel.LEVEL_NONE;
                                        bool b = true;
                                        MessageStatus m = MessageStatus.CANNOT_GRANT_REQUESTED_OPLOCK;
                                        IndicateOplockBreak(ref newLevel, ref b, ref m);
                                        Helper.CaptureRequirement(6476, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                            Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.StateCase (READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH):
                                            Case (READ_CACHING|HANDLE_CACHING):Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_READ_CACHING):
                                            Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_NO_CACHING):
                                            For each RHOpContext ThisContext in Open.Stream.Oplock.RHBreakQueue:
                                            If ThisContext.Open equals Open:If ThisContext.BreakingToRead is FALSE:
                                            If RequestedOplockLevel is not 0 and Open.Stream.Oplock.WaitList is not empty:
                                            The object store MUST indicate an oplock break to the server according to the algorithm in section 3.1.5.17.3, 
                                            setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_CANNOT_GRANT_REQUESTED_OPLOCK.");
                                    }
                                }
                                else
                                {
                                    if (!OplockWaitListEmpty &&
                                        ((level == (RequestedOplockLevel.READ_CACHING |
                                        RequestedOplockLevel.WRITE_CACHING)) ||
                                        (level == (RequestedOplockLevel.READ_CACHING |
                                        RequestedOplockLevel.WRITE_CACHING |
                                        RequestedOplockLevel.HANDLE_CACHING))))
                                    {
                                        //call 3.1.5.17.3
                                        NewOplockLevel newLevel = NewOplockLevel.READ_CACHING;
                                        bool b = true;
                                        MessageStatus m = MessageStatus.CANNOT_GRANT_REQUESTED_OPLOCK;
                                        IndicateOplockBreak(ref newLevel, ref b, ref m);
                                        Helper.CaptureRequirement(6480, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                            Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.StateCase (READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH):
                                            Case (READ_CACHING|HANDLE_CACHING):Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_READ_CACHING):
                                            Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_NO_CACHING):
                                            For each RHOpContext ThisContext in Open.Stream.Oplock.RHBreakQueue:
                                            If ThisContext.Open equals Open:Else // ThisContext.BreakingToRead is TRUE.
                                            If Open.Stream.Oplock.WaitList is not empty and( RequestedOplockLevel is (READ_CACHING|WRITE_CACHING) 
                                            or (READ_CACHING|WRITE_CACHING|HANDLE_CACHING)):
                                            The object store MUST indicate an oplock break to the server according to the algorithm in section 3.1.5.17.3, 
                                            setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_CANNOT_GRANT_REQUESTED_OPLOCK.");
                                    }
                                }

                                if (level == RequestedOplockLevel.ZERO)
                                {
                                    //Recompute Open.Stream.Oplock.State according to the algorithm 
                                    //in section 3.1.4.13, passing Open.Stream.Oplock as the 
                                    //ThisOplock parameter.
                                    Helper.CaptureRequirement(6485, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                        Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.StateCase (READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH):
                                        Case (READ_CACHING|HANDLE_CACHING):Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_READ_CACHING):
                                        Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_NO_CACHING):
                                        For each RHOpContext ThisContext in Open.Stream.Oplock.RHBreakQueue:If RequestedOplockLevel is 0 (that is, no flags):]
                                        The algorithm MUST immediately return STATUS_SUCCESS.");
                                    return MessageStatus.SUCCESS;
                                }
                                else if ((level & RequestedOplockLevel.WRITE_CACHING) == 0)
                                {
                                    //call 3.1.5.17.2
                                    return SharedOplock(OpenStreamOplockEmpty, oplockState, GrantingInAck, keyEqualOnRHOplocks, keyEqualOnRHBreakQueue, keyEqualOnROplocks, StreamIsDeleted, requestOplock);
                                }
                                else
                                {
                                }
                            }

                            if (!FoundMatchingRHOplock)
                            {
                                Helper.CaptureRequirement(6494, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.StateCase (READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH):Case (READ_CACHING|HANDLE_CACHING):Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_READ_CACHING):Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_NO_CACHING):]If FoundMatchingRHOplock is FALSE:The operation MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.");
                                return MessageStatus.INVALID_OPLOCK_PROTOCOL;
                            }

                            Helper.CaptureRequirement(6495, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.StateCase (READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH):
                                Case (READ_CACHING|HANDLE_CACHING):Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_READ_CACHING):Case (READ_CACHING|HANDLE_CACHING|BREAK_TO_NO_CACHING):]The operation immediately returns STATUS_SUCCESS.");
                            return MessageStatus.SUCCESS;
                        }
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING | OplockState.EXCLUSIVE | OplockState.BREAK_TO_READ_CACHING:
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING | OplockState.EXCLUSIVE | OplockState.BREAK_TO_NO_CACHING:
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING | OplockState.HANDLE_CACHING | OplockState.EXCLUSIVE | OplockState.BREAK_TO_READ_CACHING | OplockState.BREAK_TO_WRITE_CACHING:
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING | OplockState.HANDLE_CACHING | OplockState.EXCLUSIVE | OplockState.BREAK_TO_READ_CACHING | OplockState.BREAK_TO_HANDLE_CACHING:
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING | OplockState.HANDLE_CACHING | OplockState.EXCLUSIVE | OplockState.BREAK_TO_READ_CACHING:
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING | OplockState.HANDLE_CACHING | OplockState.EXCLUSIVE | OplockState.BREAK_TO_NO_CACHING:
                        {
                            //If Open.Stream.Oplock.ExclusiveOpen != Open
                            if (!ExclusiveOpenEqual)
                            {
                                Helper.CaptureRequirement(6496, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                    Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.State]Case 
                                   (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_WRITE_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_HANDLE_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):
                                    If Open.Stream.Oplock.ExclusiveOpen != Open:The operation MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.");
                                return MessageStatus.INVALID_OPLOCK_PROTOCOL;
                            }

                            //If Open.Stream.Oplock.WaitList is not empty and
                            //Open.Stream.Oplock.State does not contain HANDLE_CACHING and
                            //RequestedOplockLevel is (READ_CACHING|WRITE_CACHING|HANDLE_CACHING):
                            if (!OplockWaitListEmpty &&
                                ((oplockState & OplockState.HANDLE_CACHING) == 0) &&
                                (level == (RequestedOplockLevel.READ_CACHING |
                                RequestedOplockLevel.WRITE_CACHING |
                                RequestedOplockLevel.HANDLE_CACHING)))
                            {
                                //call 3.1.5.17.3
                                NewOplockLevel newLevel = NewOplockLevel.READ_CACHING;
                                bool b = true;
                                MessageStatus m = MessageStatus.CANNOT_GRANT_REQUESTED_OPLOCK;
                                IndicateOplockBreak(ref newLevel, ref b, ref m);
                                Helper.CaptureRequirement(6503, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                    Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.State,
                                    Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_WRITE_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_HANDLE_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):]
                                    If Open.Stream.Oplock.WaitList is not empty and Open.Stream.Oplock.
                                    State does not contain HANDLE_CACHING and RequestedOplockLevel is (READ_CACHING|WRITE_CACHING|HANDLE_CACHING):]
                                    ReturnStatus equal to STATUS_CANNOT_GRANT_REQUESTED_OPLOCK.");

                            }
                            else
                            {
                                if (StreamIsDeleted && ((level & RequestedOplockLevel.HANDLE_CACHING) != 0))
                                {
                                    //call 3.1.5.17.3
                                    NewOplockLevel newLevel = NewOplockLevel.READ_CACHING | NewOplockLevel.HANDLE_CACHING;
                                    bool b = true;
                                    MessageStatus m = MessageStatus.CANNOT_GRANT_REQUESTED_OPLOCK;
                                    IndicateOplockBreak(ref newLevel, ref b, ref m);
                                    Helper.CaptureRequirement(6507, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                        Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.State,
                                        Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_WRITE_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_HANDLE_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):
                                        ElseIf Open.Stream.IsDeleted is TRUE and RequestedOplockLevel contains HANDLE_CACHING:]
                                        The object store MUST indicate an oplock break to the server according to the algorithm in section 3.1.5.17.3, 
                                        setting the algorithm's parameters as follows:ReturnStatus equal to STATUS_CANNOT_GRANT_REQUESTED_OPLOCK.");
                                }

                                if (level == RequestedOplockLevel.ZERO)
                                {
                                    oplockState = OplockState.NO_OPLOCK;
                                    Helper.CaptureRequirement(6512, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                                        Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.State,Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_WRITE_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING|BREAK_TO_HANDLE_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_READ_CACHING):
                                        Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE|BREAK_TO_NO_CACHING):
                                        If RequestedOplockLevel is 0 (that is, no flags):]The operation immediately returns STATUS_SUCCESS.");
                                    return MessageStatus.SUCCESS;
                                }
                                else if ((level & RequestedOplockLevel.WRITE_CACHING) == 0)
                                {
                                    //The object store MUST request a shared oplock according to the 
                                    //algorithm in section 3.1.5.17.2, setting the algorithm's 
                                    //parameters as follows:
                                }
                                else
                                {
                                    oplockState = (OplockState)level | OplockState.EXCLUSIVE;
                                }
                            }
                            break;
                        }
                    default:
                        Helper.CaptureRequirement(6442, @"[In Server Acknowledges an Oplock Break]Whether the new oplock is granted or not, the object store MUST return:[Status].");
                        Helper.CaptureRequirement(6519, @"[In Server Acknowledges an Oplock Break,Pseudocode for the operation is as follows:
                            Else If Type is LEVEL_GRANULAR:Switch Open.Stream.Oplock.State]The operation MUST be failed with STATUS_INVALID_OPLOCK_PROTOCOL.");
                        return MessageStatus.INVALID_OPLOCK_PROTOCOL;
                }
            }
            return MessageStatus.SUCCESS;
        }

        #endregion

    }
}
