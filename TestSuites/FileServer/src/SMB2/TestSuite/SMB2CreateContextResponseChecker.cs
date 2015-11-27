// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    public abstract class BaseResponseChecker
    {
        public ITestSite TestSite;
        public Type ResponseType;

        public BaseResponseChecker(ITestSite TestSite, Type ResponseType)
        {
            this.TestSite = TestSite;
            this.ResponseType = ResponseType;
        }

        public virtual void Check(Smb2CreateContextResponse response)
        {
            TestSite.Assume.AreEqual(ResponseType, response.GetType(),
                "The response type should be {0}. The actual value is {1}.", ResponseType, response.GetType());
        }
    }

    public class DefaultLeaseResponseChecker : BaseResponseChecker
    {
        public Guid leaseKey;
        public LeaseStateValues leaseState;
        public LeaseFlagsValues leaseFlag;

        /// <summary>
        /// Initializes a new instance of the DefaultLeaseResponseChecker class with specified TestSite, LeaseKey and LeaseState.
        /// </summary>
        /// <param name="TestSite">The TestSite used to provide logging, assertions, and SUT adapters.</param>
        /// <param name="leaseKey">The expected LeaseKey value in the response.</param>
        /// <param name="leaseState">The expected LeaseState value in the response.</param>
        /// <param name="leaseFlag">The expected LeaseFlag value in the response.</param>
        public DefaultLeaseResponseChecker(ITestSite TestSite, Guid leaseKey, LeaseStateValues leaseState, LeaseFlagsValues leaseFlag)
            : base(TestSite, typeof(Smb2CreateResponseLease))
        {
            this.leaseKey = leaseKey;
            this.leaseState = leaseState;
            this.leaseFlag = leaseFlag;
        }

        public override void Check(Smb2CreateContextResponse response)
        {
            base.Check(response);
            Smb2CreateResponseLease leaseResponse = response as Smb2CreateResponseLease;
            TestSite.Assert.AreEqual(leaseKey, leaseResponse.LeaseKey, "LeaseKey in the response is expected to be {0}. The actual value is {1}.", leaseKey, leaseResponse.LeaseKey);
            TestSite.Assert.AreEqual(leaseState, leaseResponse.LeaseState, "LeaseState in the response is expected to be {0}. The actual value is {1}.", leaseState, leaseResponse.LeaseState);
            TestSite.Assert.AreEqual(leaseFlag, leaseResponse.LeaseFlags, "LeaseFlag in the response is expected to be {0}. The actual value is {1}.", leaseFlag, leaseResponse.LeaseFlags);
            TestSite.CaptureRequirementIfAreEqual<ulong>(0, leaseResponse.LeaseDuration,
                RequirementCategory.MUST_BE_ZERO.Id,
                RequirementCategory.MUST_BE_ZERO.Description);
        }
    }

    public class DefaultLeaseV2ResponseChecker : BaseResponseChecker
    {
        public Guid leaseKey;
        public LeaseStateValues leaseState;
        public LeaseFlagsValues leaseFlag;
        public Guid parentLeaseKey;

        /// <summary>
        /// Initializes a new instance of the DefaultLeaseV2ResponseChecker class with specified TestSite, LeaseKey, LeaseState and ParentLeaseKey.
        /// </summary>
        /// <param name="TestSite">The TestSite used to provide logging, assertions, and SUT adapters.</param>
        /// <param name="leaseKey">The expected LeaseKey value in the response.</param>
        /// <param name="leaseState">The expected LeaseState value in the response.</param>
        /// <param name="leaseFlag">The expected LeaseFlag value in the response.</param>
        /// <param name="parentLeaseKey">The expected ParentLeaseKey value in the response. Default value indicates not checking this field.</param>
        public DefaultLeaseV2ResponseChecker(ITestSite TestSite, Guid leaseKey, LeaseStateValues leaseState, LeaseFlagsValues leaseFlag, Guid parentLeaseKey = default(Guid))
            : base(TestSite, typeof(Smb2CreateResponseLeaseV2))
        {
            this.leaseKey = leaseKey;
            this.leaseState = leaseState;
            this.leaseFlag = leaseFlag;
            this.parentLeaseKey = parentLeaseKey;
        }

        public override void Check(Smb2CreateContextResponse response)
        {
            base.Check(response);
            Smb2CreateResponseLeaseV2 leaseResponse = response as Smb2CreateResponseLeaseV2;
            TestSite.Assert.AreEqual(leaseKey, leaseResponse.LeaseKey, "LeaseKey in the response is expected to be {0}. The actual value is {1}.", leaseKey, leaseResponse.LeaseKey);
            TestSite.Assert.AreEqual(leaseState, leaseResponse.LeaseState, "LeaseState in the response is expected to be {0}. The actual value is {1}.", leaseState, leaseResponse.LeaseState);
            TestSite.Assert.AreEqual(leaseFlag, leaseResponse.Flags, "LeaseFlag in the response is expected to be {0}. The actual value is {1}.", leaseFlag, leaseResponse.Flags);
            if (parentLeaseKey != default(Guid))
            {
                TestSite.Assert.AreEqual(parentLeaseKey, leaseResponse.ParentLeaseKey,
                    "ParentLeaseKey in the response is expected to be {0}. The actual value is {1}.",
                    parentLeaseKey, leaseResponse.ParentLeaseKey);
            }
            TestSite.CaptureRequirementIfAreEqual<ulong>(0, leaseResponse.LeaseDuration,
                RequirementCategory.MUST_BE_ZERO.Id,
                RequirementCategory.MUST_BE_ZERO.Description);
        }
    }

    public class DefaultDurableHandleResponseChecker : BaseResponseChecker
    {
        /// <summary>
        /// Initializes a new instance of the DefaultDurableHandleResponseChecker class.
        /// </summary>
        /// <param name="TestSite">The TestSite used to provide logging, assertions, and SUT adapters.</param>
        public DefaultDurableHandleResponseChecker(ITestSite TestSite)
            : base(TestSite, typeof(Smb2CreateDurableHandleResponse))
        {
        }
    }

    public class DefaultDurableHandleV2ResponseChecker : BaseResponseChecker
    {
        public CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags flag;
        public uint timeout;

        /// <summary>
        /// Initializes a new instance of the DefaultDurableHandleV2ResponseChecker class with specified TestSite, Flag and Timeout.
        /// Timeout checking can be skipped, because it could be an implementation-specific value.
        /// </summary>
        /// <param name="TestSite">The TestSite used to provide logging, assertions, and SUT adapters.</param>
        /// <param name="flag">The expected CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags in the response.</param>
        /// <param name="timeout">The expected Timeout in the response. 0xFFFFFFFF indicates not checking this field.</param>
        public DefaultDurableHandleV2ResponseChecker(ITestSite TestSite,
            CREATE_DURABLE_HANDLE_RESPONSE_V2_Flags flag,
            uint timeout)
            : base(TestSite, typeof(Smb2CreateDurableHandleResponseV2))
        {
            this.flag = flag;
            this.timeout = timeout;
        }

        public override void Check(Smb2CreateContextResponse response)
        {
            base.Check(response);
            Smb2CreateDurableHandleResponseV2 durableHandleResponse = response as Smb2CreateDurableHandleResponseV2;
            TestSite.Assert.AreEqual(flag, durableHandleResponse.Flags, "Flag in the response is expected to be {0}. The actual value is {1}.", flag, durableHandleResponse.Flags);
            if (timeout != uint.MaxValue)
            {
                TestSite.Assert.AreEqual(timeout, durableHandleResponse.Timeout, "Timeout in the response is expected to be {0}. The actual value is {1}.", timeout, durableHandleResponse.Timeout);
            }
        }
    }
}
