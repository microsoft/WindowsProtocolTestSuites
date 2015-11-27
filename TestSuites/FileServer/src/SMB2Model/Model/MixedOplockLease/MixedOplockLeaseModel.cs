// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.MixedOplockLease;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Model.MixedOplockLease
{
    /// <summary>
    /// This model is designed to test the below scenario：
    /// One client requests Oplock, and another client requests Lease.
    /// The model will verify if there should be an oplock or lease break, and if the granted OplockLevel or LeaseState is correct
    /// </summary>
    public static class MixedOplockLeaseModel
    {
        #region States
        /// <summary>
        /// If the first client requests an Oplock, the field saves the granted OplockLevel, otherwise it should be OPLOCK_LEVEL_NONE
        /// </summary>
        public static OplockLevel_Values OplockState = OplockLevel_Values.OPLOCK_LEVEL_NONE;
        /// <summary>
        /// If the first client requests a Lease, the field saves the granted LeaseState, otherwise it should be Lease_None
        /// </summary>
        public static ModelLeaseStateType LeaseState = ModelLeaseStateType.Lease_None;
        #endregion

        #region Rules
        /// <summary>
        /// The two clients connects to the share of server, including: Negotiate, SessionSetup, TreeConnect
        /// </summary>
        [Rule]
        public static void Preparation()
        {
            Condition.IsTrue(OplockState == OplockLevel_Values.OPLOCK_LEVEL_NONE);
        }

        /// <summary>
        /// One client requests Oplock
        /// </summary>
        /// <param name="oplockType">Type of OplockLevel</param>
        [Rule]
        public static void RequestOplock(OplockLevel_Values oplockType)
        {
            // OPLOCK_LEVEL_NONE is only initial state, it's meaningless when requesting an Oplock
            Condition.IsFalse(oplockType == OplockLevel_Values.OPLOCK_LEVEL_NONE);
            // OPLOCK_LEVEL_LEASE is used when requesting Lease, so it's not applicable for requesting Oplock
            Condition.IsFalse(oplockType == OplockLevel_Values.OPLOCK_LEVEL_LEASE);

            if (OplockState == OplockLevel_Values.OPLOCK_LEVEL_NONE && LeaseState == ModelLeaseStateType.Lease_None)
            {
                OplockState = oplockType;
            }
        }

        /// <summary>
        /// One client requests Lease
        /// </summary>
        /// <param name="leaseType">Type of LeaseState</param>
        [Rule]
        public static void RequestLease(ModelLeaseStateType leaseType)
        {
            // Lease_None is only initial state, it's meaningless when requesting a Lease
            Condition.IsFalse(leaseType == ModelLeaseStateType.Lease_None);

            if (OplockState == OplockLevel_Values.OPLOCK_LEVEL_NONE && LeaseState == ModelLeaseStateType.Lease_None)
            {
                LeaseState = leaseType;
            }
        }

        /// <summary>
        /// Verify if there's Oplock/Lease break and if the granted OplockLevel/LeaseState is correct
        /// The verification is based on windows behavior.
        /// </summary>
        /// <param name="breakType">Indicates if there's Oplock/Lease break</param>
        /// <param name="grantedOplockType">Indicates type of the granted OplockLevel</param>
        /// <param name="grantedLeaseType">Indicates type of the granted LeaseState</param>
        [Rule]
        public static void Verification(ModelBreakType breakType, OplockLevel_Values grantedOplockType, ModelLeaseStateType grantedLeaseType)
        {
            PrintMatrixAndState();
            // 1st Client(Oplock)      2nd Client(Lease)   OplockBreakExist   GrantedLeaseState
            // ================================================================================
            // OPLOCK_LEVEL_II         Any Lease           No                 Lease_R
            // OPLOCK_LEVEL_EXCLUSIVE  Any Lease           Yes                Lease_R
            // OPLOCK_LEVEL_BATCH      Any Lease           Yes                Lease_R
            switch (OplockState)
            {
                // The first request is not for Oplock
                case OplockLevel_Values.OPLOCK_LEVEL_NONE:
                    break;
                case OplockLevel_Values.OPLOCK_LEVEL_II:
                    Condition.IsTrue(breakType == ModelBreakType.NoBreak);
                    Condition.IsTrue(grantedLeaseType == ModelLeaseStateType.Lease_R);
                    break;
                case OplockLevel_Values.OPLOCK_LEVEL_EXCLUSIVE:
                case OplockLevel_Values.OPLOCK_LEVEL_BATCH:
                    Condition.IsTrue(breakType == ModelBreakType.OplockBreak);
                    Condition.IsTrue(grantedLeaseType == ModelLeaseStateType.Lease_R);
                    break;
                default:
                    break;
            }

            // 1st Client(Lease)      2nd Client(Oplock)   LeaseBreakExist   GrantedOplockLevel
            // ================================================================================
            // Lease_R                Any Oplock           No                OPLOCK_LEVEL_II
            // Lease_RH               Any Oplock           No                OPLOCK_LEVEL_NONE
            // Lease_RW               Any Oplock           Yes               OPLOCK_LEVEL_II
            // Lease_RWH              Any Oplock           Yes               OPLOCK_LEVEL_NONE
            switch (LeaseState)
            {
                // The first request is not for Lease
                case ModelLeaseStateType.Lease_None:
                    break;
                case ModelLeaseStateType.Lease_R:
                    Condition.IsTrue(breakType == ModelBreakType.NoBreak);
                    Condition.IsTrue(grantedOplockType == OplockLevel_Values.OPLOCK_LEVEL_II);
                    break;
                case ModelLeaseStateType.Lease_RH:
                    Condition.IsTrue(breakType == ModelBreakType.NoBreak);
                    Condition.IsTrue(grantedOplockType == OplockLevel_Values.OPLOCK_LEVEL_NONE);
                    break;
                case ModelLeaseStateType.Lease_RW:
                    Condition.IsTrue(breakType == ModelBreakType.LeaseBreak);
                    Condition.IsTrue(grantedOplockType == OplockLevel_Values.OPLOCK_LEVEL_II);
                    break;
                case ModelLeaseStateType.Lease_RWH:
                    Condition.IsTrue(breakType == ModelBreakType.LeaseBreak);
                    Condition.IsTrue(grantedOplockType == OplockLevel_Values.OPLOCK_LEVEL_NONE);
                    break;
                default:
                    break;
            }
        }

        #endregion

        private static void PrintMatrixAndState()
        {
            if (LeaseState != ModelLeaseStateType.Lease_None)
            {
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                ModelHelper.Log(LogType.TestInfo, "The first client requests lease, and lease state is {0}.", LeaseState);
                ModelHelper.Log(LogType.TestInfo, "Refer to the below matrix to check if lease break should exist and what the granted oplock level is.");
                ModelHelper.Log(LogType.TestInfo, "Note: the matrix is made based on Windows behavior");
                ModelHelper.Log(LogType.TestInfo, "================================================================================");
                ModelHelper.Log(LogType.TestInfo, "1st Client(Lease)\t2nd Client(Oplock)\tLeaseBreakExist\tGrantedOplockLevel");
                ModelHelper.Log(LogType.TestInfo, "================================================================================");
                ModelHelper.Log(LogType.TestInfo, "Lease_R\t\tAny Oplock\t\tNo\t\tOPLOCK_LEVEL_II");
                ModelHelper.Log(LogType.TestInfo, "Lease_RH\t\tAny Oplock\t\tNo\t\tOPLOCK_LEVEL_NONE");
                ModelHelper.Log(LogType.TestInfo, "Lease_RW\t\tAny Oplock\t\tYes\t\tOPLOCK_LEVEL_II");
                ModelHelper.Log(LogType.TestInfo, "Lease_RWH\t\tAny Oplock\t\tYes\t\tOPLOCK_LEVEL_NONE");
                ModelHelper.Log(LogType.TestInfo, "================================================================================");
            }

            if (OplockState != OplockLevel_Values.OPLOCK_LEVEL_NONE)
            {
                ModelHelper.Log(LogType.TestTag, TestTag.Compatibility);
                ModelHelper.Log(LogType.TestInfo, "The first client requests oplock, and oplock level is {0}.", OplockState);
                ModelHelper.Log(LogType.TestInfo, "Refer to the below matrix to check if oplock break should exist and what the granted lease state is.");
                ModelHelper.Log(LogType.TestInfo, "Note: the matrix is made based on Windows behavior");                
                ModelHelper.Log(LogType.TestInfo, "================================================================================");
                ModelHelper.Log(LogType.TestInfo, "1st Client(Oplock)\t\t2nd Client(Lease)\tOplockBreakExist\tGrantedLeaseState");
                ModelHelper.Log(LogType.TestInfo, "================================================================================");
                ModelHelper.Log(LogType.TestInfo, "OPLOCK_LEVEL_II\t\tAny Lease\tNo\t\tLease_R");
                ModelHelper.Log(LogType.TestInfo, "OPLOCK_LEVEL_EXCLUSIVE\tAny Lease\tYes\t\tLease_R");
                ModelHelper.Log(LogType.TestInfo, "OPLOCK_LEVEL_BATCH\tAny Lease\tYes\t\tLease_R");
                ModelHelper.Log(LogType.TestInfo, "================================================================================");
            }
        }
    }
}
