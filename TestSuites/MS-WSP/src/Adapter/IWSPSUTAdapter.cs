using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using System.Text;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    /// <summary>
    /// SUT Control Adapter to create , delete and modify files on remote server
    /// </summary>
    public interface IWSPSUTAdapter : IAdapter
    {
  
        /// <summary>
        /// this method is to create a file on remote server machine
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="fileName"></param>
        [MethodHelp(@"this method is to create a file on remote server machine")]
        int CreateFile(string serverName, string domainName,string userName, string password, string fileName);

        /// <summary>
        ///  this method is to delete a file on remote server machine
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="fileName"></param>
        [MethodHelp(@"this method is to delete a file on remote server machine")]
        int DeleteFile(string serverName, string domainName, string userName,string password, string fileName);

        /// <summary>
        /// this method is to modify a file on remote server machine
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="fileName"></param>
        [MethodHelp(@"this method is to modify a file on remote server machine")]
        int ModifyFile(string serverName, string domainName,string userName, string password,string fileName);
    }
}
