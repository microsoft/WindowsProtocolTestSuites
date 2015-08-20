// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// MachineAccountCredential contains credential (DomainName, MachineName, AccountName, Password) 
    /// used by a machine.
    /// </summary>
    public class MachineAccountCredential : AccountCredential
    {
        /// <summary>
        /// Machine name
        /// </summary>
        private string machineName;


        /// <summary>
        /// Initialize an instance of MachineAccountCredential class
        /// with machine name and password
        /// </summary>
        /// <param name="domainName">
        /// The domain name.
        /// </param>
        /// <param name="machineName">
        /// The account name. In
        /// Windows, all machine account names are the name of
        /// the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="machinePassword">
        /// The password of the machine account.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when domainName, machineName or machinePassword is null.
        /// </exception>
        public MachineAccountCredential(string domainName, string machineName, string machinePassword) :
            base(domainName, machineName + '$', machinePassword)
        {
            if (domainName == null)
            {
                throw new ArgumentNullException("domainName");
            }
            if (machineName == null)
            {
                throw new ArgumentNullException("machineName");
            }
            if (machinePassword == null)
            {
                throw new ArgumentNullException("machinePassword");
            }

            this.machineName = machineName;
        }


        /// <summary>
        /// Initialize an instance of MachineAccountCredential class 
        /// </summary>
        /// <param name="domainName">
        /// Nrpc server domain name
        /// </param>
        /// <param name="machineName">
        /// Nrpc client machine name
        /// </param>
        /// <param name="accountName">
        /// Account name, such as trusted domain name or the name of
        /// the machine with a $ (dollar sign) appended.
        /// </param>
        /// <param name="accountPassword">
        /// Password of the account
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when domainName, machineName, accountName or accountPassword is null.
        /// </exception>
        public MachineAccountCredential(
            string domainName, 
            string machineName, 
            string accountName,
            string accountPassword) :
            base(domainName, accountName, accountPassword)
        {
            if (domainName == null)
            {
                throw new ArgumentNullException("domainName");
            }
            if (machineName == null)
            {
                throw new ArgumentNullException("machineName");
            }
            if (accountName == null)
            {
                throw new ArgumentNullException("accountName");
            }
            if (accountPassword == null)
            {
                throw new ArgumentNullException("accountPassword");
            }

            this.machineName = machineName;
        }


        /// <summary>
        /// Machine name.
        /// </summary>
        public string MachineName
        {
            get
            {
                return machineName;
            }
        }
    }
}
