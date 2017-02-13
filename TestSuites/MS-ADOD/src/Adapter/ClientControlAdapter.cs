// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.ADOD.Adapter.Util;


namespace Microsoft.Protocol.TestSuites.ADOD.Adapter
{
    /// <summary>
    /// ClientControlAdapter is a class which implements the IClientControlAdapter interface.
    /// This is a managed adapter providing methods for communications with non-Windows environments.
    /// </summary>
    public class ClientControlAdapter : ManagedAdapterBase, IClientControlAdapter
    {
        private ADODTestConfig config;

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            config = new ADODTestConfig(testSite);
        }

        public bool IsJoinDomainSuccess()
        {
            TelnetClient telnetClient = new TelnetClient(config.ClientIP, config.TelnetPort, config.ClientAdminUsername, config.ClientAdminPwd);
            string response = string.Empty;
            StringBuilder command = new StringBuilder();
            command.Append(config.ClientScriptPath + config.IsJoinDomainSuccessScript + " ");
            command.Append(config.FullDomainName);
            telnetClient.WriteCommand(command.ToString());
            response = telnetClient.ReadResponse();
            return telnetClient.CheckResponse(response);
        }

        public bool IsUnjoinDomainSuccess()
        {
            TelnetClient telnetClient = new TelnetClient(config.ClientIP, config.TelnetPort, config.ClientAdminUsername, config.ClientAdminPwd);
            string response = string.Empty;
            StringBuilder command = new StringBuilder();
            command.Append(config.ClientScriptPath + config.IsUnjoinDomainSuccessScript + " ");
            telnetClient.WriteCommand(command.ToString());
            response = telnetClient.ReadResponse();
            return telnetClient.CheckResponse(response);
        }

        public string LocateDomainController()
        {
            TelnetClient telnetClient = new TelnetClient(config.ClientIP, config.TelnetPort, config.ClientAdminUsername, config.ClientAdminPwd);
            string response = string.Empty;
            StringBuilder command = new StringBuilder();
            command.Append(config.ClientScriptPath + config.LocateDomainControllerScript + " ");
            command.Append(config.FullDomainName);
            telnetClient.WriteCommand(command.ToString());
            response = telnetClient.ReadResponse();
            string splitter = "\r\n";
            int start = response.IndexOf(splitter, StringComparison.OrdinalIgnoreCase) + splitter.Length;
            int stop = response.LastIndexOf(splitter, StringComparison.OrdinalIgnoreCase);
            if (start >= stop)
            {
                return null;
            }
            else
            {
                string result = response.Substring(start, stop - start);
                return result;
            }
        }
                

        public bool JoinDomainCreateAcctLDAP()
        {
            TelnetClient telnetClient = new TelnetClient(config.ClientIP, config.TelnetPort, config.ClientAdminUsername, config.ClientAdminPwd);
            string response = string.Empty;
            StringBuilder command = new StringBuilder();
            command.Append(config.ClientScriptPath + config.JoinDomainCreateAcctLDAPScript + " ");
            command.Append(config.FullDomainName + " ");
            command.Append(config.DomainAdminUsername + " ");
            command.Append(config.DomainAdminPwd);
            telnetClient.WriteCommand(command.ToString());
            response = telnetClient.ReadResponse();
            return telnetClient.CheckResponse(response);
        }


        public bool JoinDomainCreateAcctSAMR()
        {
            TelnetClient telnetClient = new TelnetClient(config.ClientIP, config.TelnetPort, config.ClientAdminUsername, config.ClientAdminPwd);
            string response = string.Empty;
            StringBuilder command = new StringBuilder();
            command.Append(config.ClientScriptPath + config.JoinDomainCreateAcctSAMRScript + " ");
            command.Append(config.FullDomainName + " ");
            command.Append(config.DomainAdminUsername + " ");
            command.Append(config.DomainAdminPwd);
            telnetClient.WriteCommand(command.ToString());
            response = telnetClient.ReadResponse();
            return telnetClient.CheckResponse(response);
        }

        public bool JoinDomainPredefAcct()
        {
            TelnetClient telnetClient = new TelnetClient(config.ClientIP, config.TelnetPort, config.ClientAdminUsername, config.ClientAdminPwd);
            string response = string.Empty;
            StringBuilder command = new StringBuilder();
            command.Append(config.ClientScriptPath + config.JoinDomainPredefAcctScript + " ");
            command.Append(config.FullDomainName + " ");
            command.Append(config.DomainAdminUsername + " ");
            command.Append(config.DomainAdminPwd);
            telnetClient.WriteCommand(command.ToString());
            response = telnetClient.ReadResponse();
            return telnetClient.CheckResponse(response);
        }


        public bool UnjoinDomain()
        {
            //Samba does not support unjoin domain
            //or refer to nick's suggestion by deleting some ldb file on samba client
            TelnetClient telnetClient = new TelnetClient(config.ClientIP, config.TelnetPort, config.ClientAdminUsername, config.ClientAdminPwd);
            string response = string.Empty;
            StringBuilder command = new StringBuilder();
            command.Append(config.ClientScriptPath + config.UnjoinDomainScript);
            telnetClient.WriteCommand(command.ToString());
            response = telnetClient.ReadResponse();
            return telnetClient.CheckResponse(response);
        }


        public bool ProvisionUserAcctLDAP()
        {
            throw new NotImplementedException();
        }

        public bool ProvisionUserAcctSAMR()
        {
            throw new NotImplementedException();
        }

        public bool ChangeUserAcctPasswordLDAP()
        {
            throw new NotImplementedException();
        }

        public bool ChangeUserAcctPasswordSAMR()
        {
            throw new NotImplementedException();
        }

        public string DetermineUserAcctMembershipLDAP()
        {
            throw new NotImplementedException();
        }

        public string DetermineUserAcctMembershipSAMR()
        {
            throw new NotImplementedException();
        }

        public string ObtainUserAcctListLDAP()
        {
            throw new NotImplementedException();
        }
        
        public bool DeleteUserAcct()
        {
            throw new NotImplementedException();
        }
                
        public string ManageGroupsandTheirMemberships()
        {
            throw new NotImplementedException();
        }
        
        public bool DeleteGroup()
        {
            throw new NotImplementedException();
        }        
    }
}
