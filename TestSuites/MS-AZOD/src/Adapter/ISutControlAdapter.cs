// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Azod.Adapter
{
    public interface ISutControlAdapter : IAdapter
    {
        [MethodHelp("Please create a file to a share folder and input the return value: true or false")]
        bool WriteToShareFolder(string uncPath, string fileName, string userName, string password, string domainName, string logFileName);

        [MethodHelp("Please list the directory information of a share folder and input the return value: true or false")]
        bool ReadShareFolder(string uncPath, string userName, string password, string domainName, string logFileName);    

        [MethodHelp("Please turn on or off CBAC on remote computer.")]
        void CbacSwitch(string cbacSwitch,string computerName,string userName, string userPassword);

        [MethodHelp("Please turn on or off FAST on remote computer.")]
        void FASTSwitch(string FASTLevel, string cbacSwitch, string computerName, string userName, string userPassword);

        [MethodHelp("Please clean up cached tickets and dns on remote computer.")]
        void CleanupCachedTickets(string computerName, string userName, string userPassword);        
        
    }
}

