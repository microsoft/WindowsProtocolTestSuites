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
        #region Common Algorithms

        #region 3.1.5.17.1 Algorithm to Request an Exclusive Oplock

        /// <summary>
        /// Algorithm to Request an Exclusive Oplock
        /// </summary>
        /// <param name="isOpenStreamOplockEmpty">True if Open.Stream.Oplock is empty</param>
        /// <param name="oplockState">Oplock state</param>
        /// <param name="isOpenListContains">True: Open.File.OpenList contains more than one 
        /// Open whose Stream is the same as Open.Stream</param>
        /// <param name="streamIsDeleted">Open.Stream.IsDeleted is true or false</param>
        /// <param name="isRHBreakQueueEmpty">True if Open.Stream.Oplock.State.RHBreakQueue 
        /// is empty</param>
        /// <param name="isOplockKeyEqual">True if ThisOpen.OplockKey == Open.OplockKey</param>
        /// <param name="isOplockKeyEqualExclusive">False if Open.OplockKey != 
        /// Open.Stream.Oplock.ExclusiveOpen.OplockKey</param>
        /// <param name="requestOplock">The oplock type being requested</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static MessageStatus ExclusiveOplock(
            bool isOpenStreamOplockEmpty,
            OplockState oplockState,
            bool isOpenListContains,
            bool streamIsDeleted,
            bool isRHBreakQueueEmpty,
            bool isOplockKeyEqual,
            bool isOplockKeyEqualExclusive,
            RequestedOplock requestOplock)
        {
            //If Open.Stream.Oplock is empty
            if (isOpenStreamOplockEmpty)
            {
                gOplockState = OplockState.NO_OPLOCK;
            }

            //If Open.Stream.Oplock.State contains LEVEL_TWO_OPLOCK or NO_OPLOCK:
            if ((oplockState & OplockState.LEVEL_TWO_OPLOCK) != 0 ||
                (oplockState & OplockState.NO_OPLOCK) != 0)
            {
                //If Open.Stream.Oplock.State contains LEVEL_TWO_OPLOCK and RequestedOplock 
                //contains one or more of READ_CACHING, HANDLE_CACHING, or WRITE_CACHING, 
                //the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.
                if ((oplockState & OplockState.LEVEL_TWO_OPLOCK) != 0 &&
                    (requestOplock == RequestedOplock.READ_CACHING ||
                    requestOplock == RequestedOplock.HANDLE_CACHING ||
                    requestOplock == RequestedOplock.WRITE_CACHING ||
                    requestOplock == RequestedOplock.READ_HANDLE ||
                    requestOplock == RequestedOplock.READ_WRITE ||
                    requestOplock == RequestedOplock.READ_WRITE_HANDLE))
                {
                    Helper.CaptureRequirement(6344, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:]
                        If Open.Stream.Oplock.State contains LEVEL_TWO_OPLOCK or NO_OPLOCK:If Open.Stream.Oplock.State contains LEVEL_TWO_OPLOCK 
                        and RequestedOplock contains one or more of READ_CACHING, HANDLE_CACHING, or WRITE_CACHING, 
                        the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

                if (oplockState == OplockState.LEVEL_TWO_OPLOCK)
                {
                    NewOplockLevel newLevel = NewOplockLevel.LEVEL_NONE;
                    bool b = false;
                    MessageStatus m = MessageStatus.SUCCESS;
                    IndicateOplockBreak(ref newLevel, ref b, ref m);
                    Helper.CaptureRequirement(6348, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:
                        If Open.Stream.Oplock.State contains LEVEL_TWO_OPLOCK or NO_OPLOCK:
                        Remove the first Open ThisOpen from Open.Stream.Oplock.IIOplocks (there should be exactly one present), 
                        and notify the server of an oplock break according to the algorithm in section 3.1.5.17.3, 
                        setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_SUCCESS.");
                }

                //If Open.File.OpenList contains more than one Open whose Stream is the same 
                //as Open.Stream, and NO_OPLOCK is present in Open.Stream.Oplock.State
                if (isOpenListContains && (oplockState & OplockState.NO_OPLOCK) != 0)
                {
                    Helper.CaptureRequirement(6349, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:]
                        If Open.File.OpenList contains more than one Open whose Stream is the same as Open.Stream, 
                        and NO_OPLOCK is present in Open.Stream.Oplock.State, the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

                //If Open.Stream.IsDeleted is true and RequestedOplock contains HANDLE_CACHING, 
                //the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.
                if (streamIsDeleted &&
                    (requestOplock == RequestedOplock.HANDLE_CACHING ||
                    requestOplock == RequestedOplock.READ_HANDLE ||
                    requestOplock == RequestedOplock.READ_WRITE_HANDLE))
                {
                    Helper.CaptureRequirement(6350, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:]
                        If Open.Stream.IsDeleted is TRUE and RequestedOplock contains HANDLE_CACHING, 
                        the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

            }
            else if (((oplockState & (OplockState.READ_CACHING |
                OplockState.WRITE_CACHING |
                OplockState.HANDLE_CACHING)) != 0) &&
            ((oplockState & (OplockState.BREAK_TO_TWO |
            OplockState.BREAK_TO_NONE |
            OplockState.BREAK_TO_TWO_TO_NONE |
            OplockState.BREAK_TO_READ_CACHING |
            OplockState.BREAK_TO_WRITE_CACHING |
            OplockState.BREAK_TO_HANDLE_CACHING |
            OplockState.BREAK_TO_NO_CACHING)) == 0) &&
            isRHBreakQueueEmpty)
            {
                if (requestOplock != RequestedOplock.READ_CACHING &&
                    requestOplock != RequestedOplock.WRITE_CACHING &&
                    requestOplock != RequestedOplock.HANDLE_CACHING &&
                    requestOplock != RequestedOplock.READ_HANDLE &&
                    requestOplock != RequestedOplock.READ_WRITE &&
                    requestOplock != RequestedOplock.READ_WRITE_HANDLE)
                {
                    Helper.CaptureRequirement(6353, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:]
                        If RequestedOplock contains none of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING, 
                         the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

                //If Open.Stream.IsDeleted is true and RequestedOplock contains HANDLE_CACHING, 
                //the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.
                if (streamIsDeleted &&
                    (requestOplock == RequestedOplock.HANDLE_CACHING ||
                    requestOplock == RequestedOplock.READ_HANDLE ||
                    requestOplock == RequestedOplock.READ_WRITE_HANDLE))
                {
                    Helper.CaptureRequirement(6354, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:]
                        If Open.Stream.IsDeleted is TRUE and RequestedOplock contains HANDLE_CACHING, 
                        the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                    return MessageStatus.OPLOCK_NOT_GRANTED;
                }

                switch (oplockState)
                {
                    case OplockState.READ_CACHING:
                        {
                            if (requestOplock != RequestedOplock.READ_WRITE &&
                                requestOplock != RequestedOplock.READ_WRITE_HANDLE)
                            {
                                Helper.CaptureRequirement(6355, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:
                                    Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING) 
                                    and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, 
                                    BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                                    (Open.Stream.Oplock.State.RHBreakQueue is empty):]Switch (Open.Stream.Oplock.State):
                                    Case READ_CACHING:If RequestedOplock is neither (READ_CACHING|WRITE_CACHING) nor (READ_CACHING|WRITE_CACHING|HANDLE_CACHING), 
                                    the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            //If ThisOpen.OplockKey != Open.OplockKey, the operation 
                            //MUST be failed with STATUS_OPLOCK_NOT_GRANTED.
                            if (!isOplockKeyEqual)
                            {
                                Helper.CaptureRequirement(6356, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:
                                    Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING) 
                                    and (Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, 
                                    BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                                    (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):Case READ_CACHING:]
                                    For each Open ThisOpen in Open.Stream.Oplock.ROplocks:If ThisOpen.OplockKey != Open.OplockKey, 
                                    the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            NewOplockLevel newLevel = NewOplockLevel.LEVEL_NONE;
                            bool b = false;
                            MessageStatus m = MessageStatus.OPLOCK_SWITCHED_TO_NEW_HANDLE;
                            IndicateOplockBreak(ref newLevel, ref b, ref m);
                            Helper.CaptureRequirement(6361, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:
                                Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING) 
                                and (Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, 
                                BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                                (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):
                                Case READ_CACHING:For each Open ThisOpen in Open.Stream.Oplock.ROplocks:Notify the server of an oplock break 
                                according to the algorithm in section 3.1.5.17.3, setting the algorithm's parameters as follows:]
                                ReturnStatus equal to STATUS_OPLOCK_SWITCHED_TO_NEW_HANDLE.");

                            break;
                        }
                    case OplockState.READ_CACHING | OplockState.HANDLE_CACHING:
                        {
                            if (requestOplock != RequestedOplock.READ_WRITE_HANDLE ||
                                !isRHBreakQueueEmpty)
                            {
                                Helper.CaptureRequirement(6363, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:
                                    Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING) 
                                    and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING,
                                    BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                                    (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):]
                                    Case (READ_CACHING|HANDLE_CACHING):If RequestedOplock is not (READ_CACHING|WRITE_CACHING|HANDLE_CACHING) 
                                    or Open.Stream.Oplock.RHBreakQueue is not empty, the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            //If ThisOpen.OplockKey != Open.OplockKey, the operation MUST 
                            //be failed with STATUS_OPLOCK_NOT_GRANTED
                            if (!isOplockKeyEqual)
                            {
                                Helper.CaptureRequirement(6364, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:
                                    Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, 
                                    or HANDLE_CACHING) and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, 
                                    BREAK_TO_READ_CACHING, BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                                   (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):Case (READ_CACHING|HANDLE_CACHING):]
                                    For each Open ThisOpen in Open.Stream.Oplock.RHOplocks:If ThisOpen.OplockKey != Open.OplockKey,
                                    the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            NewOplockLevel newLevel = NewOplockLevel.LEVEL_NONE;
                            bool b = false;
                            MessageStatus m = MessageStatus.OPLOCK_SWITCHED_TO_NEW_HANDLE;
                            IndicateOplockBreak(ref newLevel, ref b, ref m);
                            Helper.CaptureRequirement(6369, @"[In Algorithm to Request an Exclusive Oplock,Pseudocode for the algorithm is as follows:
                                Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING) 
                                and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, 
                                BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                               (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):Case (READ_CACHING|HANDLE_CACHING):
                                For each Open ThisOpen in Open.Stream.Oplock.RHOplocks:Notify the server of an oplock break according to the algorithm in section 3.1.5.17.3, 
                                setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_OPLOCK_SWITCHED_TO_NEW_HANDLE.");

                            break;
                        }
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING |
                    OplockState.HANDLE_CACHING | OplockState.EXCLUSIVE:
                        {
                            if (requestOplock != RequestedOplock.READ_WRITE_HANDLE)
                            {
                                Helper.CaptureRequirement(6371, @"[In Algorithm to Request an Exclusive Oplock,
                                    Pseudocode for the algorithm is as follows:Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, 
                                    WRITE_CACHING, or HANDLE_CACHING) and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, 
                                    BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                                   (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):]
                                    Case (READ_CACHING|WRITE_CACHING|HANDLE_CACHING|EXCLUSIVE):If RequestedOplock is not (READ_CACHING|WRITE_CACHING|HANDLE_CACHING),
                                    the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }
                            break;
                        }
                    case OplockState.READ_CACHING | OplockState.WRITE_CACHING |
                    OplockState.EXCLUSIVE:
                        {
                            if (requestOplock != RequestedOplock.READ_WRITE_HANDLE &&
                                requestOplock != RequestedOplock.READ_WRITE)
                            {
                                Helper.CaptureRequirement(6372, @"[In Algorithm to Request an Exclusive Oplock,
                                    Pseudocode for the algorithm is as follows:Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, 
                                    WRITE_CACHING, or HANDLE_CACHING) and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, 
                                    BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and
                                    (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):]
                                    Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE):If RequstedOplock is neither (READ_CACHING|WRITE_CACHING|HANDLE_CACHING) 
                                    nor (READ_CACHING|WRITE_CACHING), the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            //If Open.OplockKey != Open.Stream.Oplock.ExclusiveOpen.OplockKey, 
                            //the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.
                            if (!isOplockKeyEqualExclusive)
                            {
                                Helper.CaptureRequirement(6373, @"[In Algorithm to Request an Exclusive Oplock,
                                    Pseudocode for the algorithm is as follows:Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, 
                                    WRITE_CACHING, or HANDLE_CACHING) and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, 
                                    BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                                    (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):
                                    Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE):]If Open.OplockKey != Open.Stream.Oplock.ExclusiveOpen.OplockKey,
                                    the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            NewOplockLevel newLevel = NewOplockLevel.LEVEL_NONE;
                            bool b = false;
                            MessageStatus m = MessageStatus.OPLOCK_SWITCHED_TO_NEW_HANDLE;
                            IndicateOplockBreak(ref newLevel, ref b, ref m);
                            Helper.CaptureRequirement(6377, @"[In Algorithm to Request an Exclusive Oplock,
                                Pseudocode for the algorithm is as follows:
                                Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING) 
                                and(Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, 
                                BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                               (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):
                                Case (READ_CACHING|WRITE_CACHING|EXCLUSIVE):Notify the server of an oplock break 
                                according to the algorithm in section 3.1.5.17.3, 
                                setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_OPLOCK_SWITCHED_TO_NEW_HANDLE.");

                            break;
                        }
                    default:
                        {
                            Helper.CaptureRequirement(6380, @"[In Algorithm to Request an Exclusive Oplock,
                                Pseudocode for the algorithm is as follows:
                                Else If (Open.Stream.Oplock.State contains one or more of READ_CACHING, WRITE_CACHING, or HANDLE_CACHING) 
                                and (Open.Stream.Oplock.State contains none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, 
                                BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and           
                               (Open.Stream.Oplock.State.RHBreakQueue is empty):Switch (Open.Stream.Oplock.State):]
                                The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                            return MessageStatus.OPLOCK_NOT_GRANTED;
                        }
                }
            }
            else
            {
                Helper.CaptureRequirement(6381, @"[In Algorithm to Request an Exclusive Oplock,
                    Pseudocode for the algorithm is as follows:]
                    Else[If (Open.Stream.Oplock.State does not contains one or more of READ_CACHING, WRITE_CACHING, and HANDLE_CACHING) 
                    or (Open.Stream.Oplock.State does not contain none of BREAK_TO_TWO, BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, 
                    BREAK_TO_READ_CACHING, BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, or BREAK_TO_NO_CACHING) and
                    (Open.Stream.Oplock.State.RHBreakQueue is empty):]The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                return MessageStatus.OPLOCK_NOT_GRANTED;
            }

            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.17.2 Algorithm to Request a Shared Oplock

        /// <summary>
        /// Algorithm to request a shared oplock
        /// </summary>
        /// <param name="isOpenStreamOplockEmpty">true if Open.Stream.Oplock is empty</param>
        /// <param name="oplockState">Oplock state</param>
        /// <param name="GrantingInAck"> A boolean value. 
        /// True: if this oplock is being requested as part of an oplock break acknowledgement, false if not</param>
        /// <param name="keyEqualOnRHOplocks">True if there is an Open on 
        /// Open.Stream.Oplock.RHOplocks whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnRHBreakQueue">True if there is an Open on 
        /// Open.Stream.Oplock.RHBreakQueue whose OplockKey is equal to Open.OplockKey</param>
        /// <param name="keyEqualOnROplocks">True if there is an Open ThisOpen on 
        /// Open.Stream.Oplock.ROplocks whose OplockKey is equal to Open.OplockKey (there should be at most one present)</param>
        /// <param name="StreamIsDeleted">True if Open.Stream.IsDeleted is true</param>
        /// <param name="requestOplock">The oplock type being requested</param>
        /// <returns> An NTSTATUS code indicating the result of the operation</returns>
        public static MessageStatus SharedOplock(
            bool isOpenStreamOplockEmpty,
            OplockState oplockState,
            bool GrantingInAck,
            bool keyEqualOnRHOplocks,
            bool keyEqualOnRHBreakQueue,
            bool keyEqualOnROplocks,
            bool StreamIsDeleted,
            RequestedOplock requestOplock)
        {

            //If Open.Stream.Oplock is empty
            if (isOpenStreamOplockEmpty)
            {
                gOplockState = OplockState.NO_OPLOCK;
            }

            if (!GrantingInAck &&
                (oplockState & (OplockState.BREAK_TO_TWO |
                OplockState.BREAK_TO_NONE |
                OplockState.BREAK_TO_TWO_TO_NONE |
                OplockState.BREAK_TO_READ_CACHING |
                OplockState.BREAK_TO_WRITE_CACHING |
                OplockState.BREAK_TO_HANDLE_CACHING |
                OplockState.BREAK_TO_NO_CACHING |
                OplockState.EXCLUSIVE)) != 0)
            {
                Helper.CaptureRequirement(6396, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:]
                    If (GrantingInAck is FALSE) and (Open.Stream.Oplock.State contains one or more of BREAK_TO_TWO, 
                    BREAK_TO_NONE, BREAK_TO_TWO_TO_NONE, BREAK_TO_READ_CACHING, BREAK_TO_WRITE_CACHING, BREAK_TO_HANDLE_CACHING, 
                    BREAK_TO_NO_CACHING, or EXCLUSIVE), then:The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                return MessageStatus.OPLOCK_NOT_GRANTED;
            }

            switch (requestOplock)
            {
                case RequestedOplock.LEVEL_TWO:
                    {
                        if ((oplockState & (OplockState.NO_OPLOCK |
                            OplockState.LEVEL_TWO_OPLOCK |
                            OplockState.READ_CACHING |
                            (OplockState.LEVEL_TWO_OPLOCK | OplockState.READ_CACHING))) == 0)
                        {
                            Helper.CaptureRequirement(6397, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:]
                                Switch (RequestedOplock):Case LEVEL_TWO:
                                The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED if Open.Stream.Oplock.
                                State is anything other than the following:NO_OPLOCK,LEVEL_TWO_OPLOCK,READ_CACHING,(LEVEL_TWO_OPLOCK|READ_CACHING).");
                            return MessageStatus.OPLOCK_NOT_GRANTED;
                        }
                        break;
                    }
                case RequestedOplock.READ_CACHING:
                    {
                        if (!GrantingInAck &&
                            (oplockState & (OplockState.NO_OPLOCK |
                            OplockState.LEVEL_TWO_OPLOCK |
                            OplockState.READ_CACHING |
                            (OplockState.LEVEL_TWO_OPLOCK | OplockState.READ_CACHING) |
                            (OplockState.READ_CACHING | OplockState.HANDLE_CACHING) |
                            (OplockState.READ_CACHING | OplockState.HANDLE_CACHING |
                            OplockState.MIXED_R_AND_RH) |
                            (OplockState.READ_CACHING | OplockState.HANDLE_CACHING |
                            OplockState.BREAK_TO_READ_CACHING) |
                            (OplockState.READ_CACHING | OplockState.HANDLE_CACHING |
                            OplockState.BREAK_TO_NO_CACHING))) == 0)
                        {
                            Helper.CaptureRequirement(6398, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                Switch (RequestedOplock):]Case READ_CACHING:The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED 
                                if GrantingInAck is FALSE and Open.Stream.Oplock.State is anything other than the following:NO_OPLOCK,LEVEL_TWO_OPLOCK,
                                READ_CACHING,(LEVEL_TWO_OPLOCK|READ_CACHING),(READ_CACHING|HANDLE_CACHING),(READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH),
                                (READ_CACHING|HANDLE_CACHING|BREAK_TO_READ_CACHING),(READ_CACHING|HANDLE_CACHING|BREAK_TO_NO_CACHING).");
                            return MessageStatus.OPLOCK_NOT_GRANTED;
                        }

                        if (!GrantingInAck)
                        {
                            //If there is an Open on Open.Stream.Oplock.RHOplocks whose OplockKey 
                            //is equal to Open.OplockKey, the operation MUST be failed with 
                            //STATUS_OPLOCK_NOT_GRANTED.
                            if (keyEqualOnRHOplocks)
                            {
                                Helper.CaptureRequirement(6399, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                    Switch (RequestedOplock):Case READ_CACHING:]If GrantingInAck is FALSE:
                                    If there is an Open on Open.Stream.Oplock.RHOplocks whose OplockKey is equal to Open.OplockKey, 
                                    the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            //If there is an Open on Open.Stream.Oplock.RHBreakQueue whose OplockKey 
                            //is equal to Open.OplockKey, the operation MUST be failed with 
                            //STATUS_OPLOCK_NOT_GRANTED.
                            if (keyEqualOnRHBreakQueue)
                            {
                                Helper.CaptureRequirement(6400, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                    Switch (RequestedOplock):Case READ_CACHING:If GrantingInAck is FALSE:]
                                    If there is an Open on Open.Stream.Oplock.RHBreakQueue whose OplockKey is equal to Open.OplockKey, 
                                    the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                                return MessageStatus.OPLOCK_NOT_GRANTED;
                            }

                            //If there is an Open ThisOpen on Open.Stream.Oplock.ROplocks whose 
                            //OplockKey is equal to Open.OplockKey (there should be at most one 
                            //present):
                            if (keyEqualOnROplocks)
                            {
                                NewOplockLevel newLevel = NewOplockLevel.READ_CACHING;
                                bool b = false;
                                MessageStatus m = MessageStatus.OPLOCK_SWITCHED_TO_NEW_HANDLE;
                                IndicateOplockBreak(ref newLevel, ref b, ref m);
                                Helper.CaptureRequirement(6405, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                    Switch (RequestedOplock):Case READ_CACHING:If GrantingInAck is FALSE:
                                    If there is an Open ThisOpen on Open.Stream.Oplock.ROplocks whose OplockKey is equal to Open.OplockKey 
                                    (there should be at most one present):Notify the server of an oplock break according to the algorithm in section 3.1.5.17.3,
                                    setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_OPLOCK_SWITCHED_TO_NEW_HANDLE.");

                            }
                        }
                        break;
                    }
                case RequestedOplock.READ_HANDLE:
                    {
                        if (!GrantingInAck && (oplockState & (OplockState.NO_OPLOCK |
                            OplockState.READ_CACHING |
                            (OplockState.READ_CACHING | OplockState.HANDLE_CACHING) |
                            (OplockState.READ_CACHING | OplockState.HANDLE_CACHING |
                            OplockState.MIXED_R_AND_RH))) == 0)
                        {
                            Helper.CaptureRequirement(6410, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                Switch (RequestedOplock):]Case (READ_CACHING|HANDLE_CACHING):The operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED 
                                 if GrantingInAck is FALSE and Open.Stream.Oplock.State is anything other than the following:NO_OPLOCK,READ_CACHING,(READ_CACHING|HANDLE_CACHING),(READ_CACHING|HANDLE_CACHING|MIXED_R_AND_RH).");
                            return MessageStatus.OPLOCK_NOT_GRANTED;
                        }

                        //If Open.Stream.IsDeleted is true
                        if (StreamIsDeleted)
                        {
                            Helper.CaptureRequirement(6411, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                Switch (RequestedOplock):Case (READ_CACHING|HANDLE_CACHING):]If Open.Stream.IsDeleted is TRUE, 
                                the operation MUST be failed with STATUS_OPLOCK_NOT_GRANTED.");
                            return MessageStatus.OPLOCK_NOT_GRANTED;
                        }

                        if (!GrantingInAck)
                        {
                            if (keyEqualOnROplocks)
                            {
                                NewOplockLevel newLevel = NewOplockLevel.READ_CACHING | NewOplockLevel.HANDLE_CACHING;
                                bool b = false;
                                MessageStatus m = MessageStatus.OPLOCK_SWITCHED_TO_NEW_HANDLE;
                                IndicateOplockBreak(ref newLevel, ref b, ref m);
                                Helper.CaptureRequirement(6416, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                    Switch (RequestedOplock):Case (READ_CACHING|HANDLE_CACHING):If GrantingInAck is FALSE: 
                                    If there is an Open ThisOpen on Open.Stream.Oplock.ROplocks whose OplockKey is equal to Open.OplockKey 
                                    (there should be at most one present):Notify the server of an oplock break according to the algorithm in section 3.1.5.17.3, 
                                    setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_OPLOCK_SWITCHED_TO_NEW_HANDLE.");
                            }

                            if (keyEqualOnRHOplocks)
                            {
                                NewOplockLevel newLevel = NewOplockLevel.READ_CACHING | NewOplockLevel.HANDLE_CACHING;
                                bool b = false;
                                MessageStatus m = MessageStatus.OPLOCK_SWITCHED_TO_NEW_HANDLE;
                                IndicateOplockBreak(ref newLevel, ref b, ref m);
                                Helper.CaptureRequirement(6420, @"[In Algorithm to Request a Shared Oplock,Pseudocode for the algorithm is as follows:
                                    Switch (RequestedOplock):Case (READ_CACHING|HANDLE_CACHING):If GrantingInAck is FALSE:
                                    If there is an Open ThisOpen on Open.Stream.Oplock.RHOplocks whose OplockKey is equal to Open.OplockKey 
                                    (there should be at most one present):Notify the server of an oplock break according to the algorithm in section 3.1.5.17.3, 
                                    setting the algorithm's parameters as follows:]ReturnStatus equal to STATUS_OPLOCK_SWITCHED_TO_NEW_HANDLE.");
                            }
                        }

                        break;
                    }
                default:
                    break;
            }

            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.17.3 Indicating an Oplock Break to the Server

        /// <summary>
        /// Indicating an Oplock Break to the Server
        /// </summary>
        /// <param name="newOplockLevel">The type of oplock the requested oplock has been broken to</param>
        /// <param name="AcknowledgeRequired">A boolean value; True: if the server must acknowledge the oplock break,
        /// False: if not (see section 3.1.5.18 for acknowledgement processing)</param>
        /// <param name="returnStatus"> The NTSTATUS code to return to the server</param>
        /// Disable warning CA1801, because the parameter of "capabilities" is used for extend the model logic,
        /// which will affect the implementation of the model if it is removed.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void IndicateOplockBreak(
            ref NewOplockLevel newOplockLevel,
            ref bool AcknowledgeRequired,
            ref MessageStatus returnStatus)
        {
            Helper.CaptureRequirement(6437, @"[In Indicating an Oplock Break to the Server]The object store MUST return ReturnStatus, 
                AcknowledgeRequired, and NewOplockLevel to the server.");
        }

        #endregion

        #region 3.1.5.19 Server Requests Canceling an Operation

        /// <summary>
        /// Server Requests Canceling an Operation
        /// this method was called by 3.1.5.7, and the scenario was in Scenario16_ByteRangeLock
        /// </summary>
        /// <param name="iorequest">An implementation-specific identifier that is unique for each 
        /// outstanding IO operation. See [MS-CIFS] section 3.3.5.51.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        public static MessageStatus CancelinganOperation(IORequest iorequest)
        {
            //When a cancelation request is received, scan CancelableOperations.
            //CancelableOperationList looking for an operation CanceledOperation 
            //that matches IORequest. If found,
            if (sequenceIORequest.Contains(iorequest))
            {
                Condition.IsTrue(sequenceIORequest.Contains(iorequest));
                sequenceIORequest.Remove(iorequest);
                Helper.CaptureRequirement(6527, @"[In Server Requests Canceling an Operation,If an operation CanceledOperation that matches IORequest. found,]
                    CanceledOperation MUST be failed with STATUS_CANCELED returned for the status of the canceled operation.");
                return MessageStatus.CANCELLED;
            }

            return MessageStatus.INVALID_PARAMETER;
        }

        #endregion

        #endregion

        #region 3.1.5.7    Application Requests a Byte-Range Lock

        static long gLockOffset = 0;
        static long gLockLength = 0;

        // Disable warning CA1823, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        static bool gLockIsExclusive;

        /// <summary>
        /// 3.1.5.7    Application Requests a Byte-Range Lock
        /// </summary>
        /// <param name="FileOffset">A 64-bit unsigned integer containing the starting offset, in bytes</param> 
        /// <param name="Length">A 64-bit unsigned integer containing the length, in bytes. This value MAY be zero</param> 
        /// <param name="ExclusiveLock">A boolean indicating whether the range is to be locked exclusively (true) or shared (FALSE)</param> 
        /// <param name="failImmediately">A boolean indicating whether the lock request is to fail immediately (true) if the range is locked by another open or if it is to wait until the lock can be acquired (FALSE).</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        /// <param name="isOpenNotEqual">used in 3.1.4.10</param>
        /// Disable warning CA1801, because the parameter of "capabilities" is used for extend the model logic, 
        /// which will affect the implementation of the model if it is removed.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [Rule]
        public static MessageStatus ByteRangeLock(
            long FileOffset,
            long Length,
            bool ExclusiveLock,
            bool failImmediately,
            bool isOpenNotEqual
            )
        {

            if (gStreamType == StreamType.DirectoryStream)
            {
                Helper.CaptureRequirement(904, @"[In Server Requests a Byte-Range Lock]Pseudocode for the operation is as follows:
                    If Open.Stream.StreamType is DirectoryStream, return STATUS_INVALID_PARAMETER, as byte range locks are not permitted on directories.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (gLockOffset >= 0 && 
                gLockLength > 0 && 
                FileOffset > gLockOffset &&
                (FileOffset + Length) > (gLockOffset + gLockLength) && // Lock range conflict
                gLockIsExclusive && 
                !isOpenNotEqual)
            {
                if (failImmediately)
                {
                    Requirement.Capture(@"[2.1.5.7 Server Requests a Byte-Range Lock, Pseudocode for the operation is as follows:]
                        The object store MUST check for byte range lock conflicts by using the algorithm described in section 2.1.4.10, 
                        with ByteOffset set to FileOffset, Length set to Length, IsExclusive set to ExclusiveLock, LockIntent set to TRUE, and Open set to Open. 
                        If a conflict is detected, then:
                            If a conflict is detected, then:If FailImmediately is TRUE, the operation MUST be failed with STATUS_LOCK_NOT_GRANTED.");
                    return MessageStatus.LOCK_NOT_GRANTED;
                }
                else
                {
                    //call 3.1.5.19
                    if (!sequenceIORequest.Contains(IORequest.ByteRangeLock))
                    {
                        sequenceIORequest.Add(IORequest.ByteRangeLock);
                        CancelinganOperation(IORequest.ByteRangeLock);
                    }
                }
            }

            //Initialize a new ByteRangeLock
            ByteRangeLock byteRangeLock = new ByteRangeLock();
            byteRangeLock.IsExclusive = ExclusiveLock;
            byteRangeLock.LockLength = Length;
            byteRangeLock.LockOffset = FileOffset;

            gLockIsExclusive = ExclusiveLock;
            gLockOffset = FileOffset;
            gLockLength = Length;

            ByteRangeLockList.Add(byteRangeLock);

            Helper.CaptureRequirement(903, @"[In Server Requests a Byte-Range Lock]On completion, the object store MUST return:Status:
                An NTSTATUS code that specifies the result.");
            Helper.CaptureRequirement(919, @"[In Server Requests a Byte-Range Lock,,Pseudocode for the operation is as follows:
                if the operation succeeds]Complete this operation with STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.8    Application Requests an Unlock of a Byte-Range

        /// <summary>
        /// Application Requests an Unlock of a Byte-Range
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus ByteRangeUnlock()
        {
            long FileOffset = gLockOffset;
            long Length = gLockLength;

            if (gStreamType == StreamType.DirectoryStream)
            {
                return MessageStatus.INVALID_PARAMETER;
            }

            //true: if lockToRemove is empty
            ByteRangeLock LockToRemove = new ByteRangeLock();
            bool isFoundLockToRemove = false;
            if (ByteRangeLockList != null || ByteRangeLockList.Count > 0)
                foreach (ByteRangeLock item in ByteRangeLockList)
                {
                    if (item.LockOffset == FileOffset &&
                        item.LockLength == Length)
                    {
                        LockToRemove = item;
                        isFoundLockToRemove = true;
                    }
                }

            //Capture requirement
            if (isFoundLockToRemove)
            {
                ByteRangeLockList.Clear();
                Helper.CaptureRequirement(1248, @"[In Server Requests an Unlock of a Byte-Range]On completion, the object store MUST return:[Status].");
                Helper.CaptureRequirement(942, @"[In Server Requests an Unlock of a Byte-Range,Pseudocode for the operation is as follows:]
                    If LockToRemove is not NULL:Complete this operation with STATUS_SUCCESS.");
                return MessageStatus.SUCCESS;
            }
            else
            {
                Helper.CaptureRequirement(1248, @"[In Server Requests an Unlock of a Byte-Range]On completion, the object store MUST return:[Status].");
                Helper.CaptureRequirement(943, @"[In Server Requests an Unlock of a Byte-Range,Pseudocode for the operation is as follows:
                    else If LockToRemove is NULL:]Complete this operation with STATUS_RANGE_NOT_LOCKED.");
                return MessageStatus.RANGE_NOT_LOCKED;
            }
        }

        #endregion
  
    }
}
