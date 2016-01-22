// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocol.TestSuites.Azod.Adapter.Util;

namespace Microsoft.Protocol.TestSuites.Azod.Adapter
{
    public class SutControlAdapter : ManagedAdapterBase, ISutControlAdapter
    {
        #region Fields
        private AzodTestConfig AzodTestConfig;        
        public ITestSite TestSite;
        #endregion

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            TestSite = testSite;
            AzodTestConfig = new AzodTestConfig(testSite);            
        }        

        private void CheckIfTriggerScriptPass(string output)
        {
            string[] strs = output.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //All trigger script will print 0 at last line if pass and print 1 if fail
            this.TestSite.Assert.IsTrue(strs[strs.Length - 2] == "0", "The triggered test cases should pass");
        }

        /// <summary>
        /// Turn on or off CBAC on remote computer.
        /// </summary>
        public void CbacSwitch(string cbacSwitch, string computerName, string userName, string userPassword)
        {
            Console.WriteLine("============Turn on or off CBAC on remote computer============"); 
        }

        /// <summary>
        /// Turn on or off FAST on remote computer.
        /// </summary>
        public void FASTSwitch(string FASTSwitch, string cbacSwitch, string computerName, string userName, string userPassword)
        {
            Console.WriteLine("============Turn on or off FAST on remote computer============");
        }
            
        /// <summary>
        /// Clean up cached tickets and DNS on remote computer.
        /// </summary>
        public void CleanupCachedTickets(string computerName, string userName, string userPassword)
        {
            Console.WriteLine("============Clean up cached tickets and dns on remote computer============");
        }

        /// <summary>
        /// Trigger client to create a file in a share folder
        /// </summary>
        public bool WriteToShareFolder(string uncPath, string fileName, string userName, string password, string domainName, string logFileName)
        {
            Console.WriteLine(string.Format(@"============Trigger client to create a file to share folder {0} ============"), uncPath);            
            return true;
        }

        /// <summary>
        /// Trigger client to list directory information of a share folder
        /// </summary>
        public bool ReadShareFolder(string uncPath, string userName, string password, string domainName, string logFileName)
        {
            Console.WriteLine(string.Format(@"============Trigger client to list directory information of share folder {0} ============"), uncPath);                        
            return true;
        }
    }
}
