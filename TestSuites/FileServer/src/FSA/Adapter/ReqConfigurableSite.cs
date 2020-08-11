// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter
{
    /// <summary>
    /// This class is used to create an ITestSite proxy,
    /// which can be used to control or configure requirement
    /// captures.
    /// </summary>
    public class ReqConfigurableSite : DispatchProxy
    {
        private ITestSite site = null;

        /// <summary>
        /// Create the requirement capture configurable site proxy.
        /// </summary>
        /// <param name="site">The site instance to proxy.</param>
        /// <returns>The ITestSite proxy instance.</returns>
        public static ITestSite GetReqConfigurableSite(ITestSite site)
        {
            object proxy = Create<ITestSite, ReqConfigurableSite>();
            ((ReqConfigurableSite)proxy).SetParameters(site);
            return (ITestSite)proxy;
        }

        /// <summary>
        /// Set parameters for the proxy.
        /// </summary>
        /// <param name="site">The ITestSite instance.</param>
        private void SetParameters(ITestSite site)
        {
            this.site = site;
        }

        /// <summary>
        /// Invoking a method on the ITestSite instance is represented in this method.
        /// Method names with 'CaptureRequirement' will be checked if needs to be disabled.
        /// </summary>
        /// <param name="targetMethod">The method invoked.</param>
        /// <param name="args">The arguments of the method.</param>
        /// <returns>The return value of the invoked method.</returns>
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod == null)
            {
                throw new ArgumentNullException(nameof(targetMethod));
            }

            if (site == null)
            {
                throw new InvalidOperationException("Calling method on uninitialized ITestSite object.");
            }

            if (targetMethod.Name.Contains("CaptureRequirement") &&
                (args.Length > 2) &&
                (args[args.Length - 1] is string) &&
                (args[args.Length - 2] is int)
                )
            {
                string verifyingReqDescription = (string)args[args.Length - 1];
                int verifyingReqId = (int)args[args.Length - 2];

                if (CommonUtility.IsRequirementVerificationDisabled(site, verifyingReqId))
                {
                    site.Log.Add(LogEntryKind.Debug, "The requirement ({0}) verification is disabled.", verifyingReqId);
                    site.Log.Add(LogEntryKind.Debug, "The requirement description: {0}.", verifyingReqDescription);
                    return null;
                }
            }

            var retVal = targetMethod.Invoke(site, args);
            return retVal;
        }
    }
}
