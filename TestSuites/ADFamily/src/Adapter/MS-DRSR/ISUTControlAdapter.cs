// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    public interface IDrsrSutControlAdapter: IAdapter
    {       

        /// <summary>
        /// Upload the domain rename script into the directory that will perform the domain rename related directory changes 
        /// on all domain controllers.
        /// </summary>
        /// <param name="oldDNSName">The original DNS name of the domain to be renamed.</param>
        /// <param name="newDNSName">The new DNS name of the domain.</param>
        /// <returns>True if successful, otherwise false.</returns>
        [MethodHelp("Upload the domain rename script into AD")]
        bool UploadDomainRenameScript(string oldDNSName, string newDNSName);

        /// <summary>
        /// End the domain rename operation. Removes the restrictions placed on the Directory Service 
        /// during the rename operation.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        [MethodHelp("Finish domain rename procedure and release related domain locks")]
        bool EndDomainRename();

        /// <summary>
        /// Change the state of the replication traffic on a DC server.
        /// </summary>
        /// <param name="remoteServerAddress">The network address of the remote server.</param>
        /// <param name="userName">The user name with administrative access to WinRM services, in the form of UPN (name@domain)</param>
        /// <param name="remotePassword">The password of the user.</param>
        /// <param name="domainFQDN">The FQDN of the domain.</param>
        /// <param name="isEnabling">Whether replication traffic is enabled on the DC.</param>
        /// <returns>True when changing succeeded, false otherwise.</returns>
        [MethodHelp("Enable or disable replication traffic on DC")]
        bool ChangeReplTrafficStatus(string remoteServerAddress, string userName, string remotePassword, string domainFQDN, bool isEnabling, int port = 389);

        /// <summary>
        /// Force replication on all DCs.
        /// </summary>
        /// <param name="remoteServerAddress">The network address (DNS name) of the remote server.</param>
        /// <param name="userName">The user name with administrative access to WinRM services, in the form of NT4 account name (domain\name).</param>
        /// <param name="remotePassword">The password of the user.</param>
        /// <returns></returns>
        [MethodHelp("Trigger DC to force replicate with other DCs")]
        bool ForceReplSync(string remoteServerAddress, string userName, string remotePassword);

        /// <summary>
        /// Enable Auditing On Server
        /// </summary>
        /// <param name="computerName">computer to enable auditing </param>
        /// <param name="usr">computer user </param>
        /// <param name="pwd">computer user's password </param>
        /// <returns></returns>
        [MethodHelp("Enable Auditing on Server")]
        bool EnableAuditingOnServer(string computerName, string usr, string pwd);

        /// <summary>
        /// Disable Auditing On Server
        /// </summary>
        /// <param name="computerName">computer to disable auditing</param>
        /// <param name="usr">computer user </param>
        /// <param name="pwd">computer user's password </param>
        /// <returns></returns>
        [MethodHelp("Disable Auditing on Server")]
        bool DisableAuditingOnServer(string computerName, string usr, string pwd);


        bool CreateLingeringObject(string dc1Name, string dc2Name, string userName, string remotePassword, string configNC, string remoteScriptPath);
        bool RemoveLingeringObject(string dc1Name, string dc2Name, string userName, string remotePassword, string configNC, string remoteScriptPath);
    }
}
