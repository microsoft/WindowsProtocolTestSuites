// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// This class is used to create an ITestSite proxy,
    /// which can be used to control or configure requirement
    /// captures.
    /// </summary>
    public class ReqConfigurableSite : RealProxy
    {
        private ITestSite site = null;

        /// <summary>
        /// Create the requirement capture configurable site proxy.
        /// </summary>
        /// <param name="site">The site instance to proxy.</param>
        /// <returns>The ITestSite proxy instance.</returns>
        public static ITestSite GetReqConfigurableSite(ITestSite site)
        {
            return (ITestSite)(new ReqConfigurableSite(site).GetTransparentProxy());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site">The ITestSite instance.</param>
        [PermissionSet(SecurityAction.LinkDemand)]
        private ReqConfigurableSite(ITestSite site)
            : base(typeof(ITestSite))
        {
            this.site = site;
        }

        /// <summary>
        /// Invoking a method on the ITestSite instance is represented in this method.
        /// Method names with 'CaptureRequirement' will be checked if needs to be disabled.
        /// </summary>
        /// <param name="msg">A IMessage that contains a IDictionary of information about 
        /// the method call.</param>
        /// <returns>The message returned by the invoked method, containing the return value parameters.</returns>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override IMessage Invoke(IMessage msg)
        {

            if (msg == null)
                throw new ArgumentNullException("msg");

            IMethodCallMessage methodCall = msg as IMethodCallMessage;

            if (msg == null)
            {
                throw new InvalidOperationException("Method call is expected.");
            }

            if (site == null)
            {
                throw new InvalidOperationException("Calling method on uninitialized ITestSite object.");
            }

            if (methodCall.MethodName.Contains("CaptureRequirement") &&
                (methodCall.InArgCount > 2) &&
                (methodCall.InArgs[methodCall.InArgCount - 1] is string) &&
                (methodCall.InArgs[methodCall.InArgCount - 2] is int)
                )
            {
                string verifyingReqDescription = (string)methodCall.InArgs[methodCall.InArgCount - 1];
                int verifyingReqId = (int)methodCall.InArgs[methodCall.InArgCount - 2];

                if (CommonUtility.IsRequirementVerificationDisabled(site, verifyingReqId))
                {
                    site.Log.Add(LogEntryKind.Debug, "The requirement ({0}) verification is disabled.", verifyingReqId);
                    site.Log.Add(LogEntryKind.Debug, "The requirement description: {0}.", verifyingReqDescription);
                    return new ReturnMessage(
                        null, 
                        null,
                        0, 
                        methodCall.LogicalCallContext, 
                        methodCall);
                }
            }

            object retVal = null;
            object[] args = methodCall.Args;

            retVal = methodCall.MethodBase.Invoke(site, args);

            return new ReturnMessage(
                retVal,
                null,
                0,
                methodCall.LogicalCallContext,
                methodCall);
        }
    }
}
