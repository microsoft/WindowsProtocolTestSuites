// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if UT
using System;
using System.IO;
using System.Text;
using System.Management;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk

{
    /// <summary>
    /// This is a Mock Class for RemoteCommandLine,
    /// which is used in Environmentally-Independent unit tests.
    /// </summary>
    public class RemoteCommandLineMock : IDisposable
    {
        private ManagementClass classInstance;

        // process ID counter
        private static uint currentProcessID;
        // dictionary: processID vs. outputResult
        private static Dictionary<uint, string> dictResult;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">just for consistency with real class</param>
        /// <param name="remoteFolderPath">just for consistency with real class</param>
        public RemoteCommandLineMock(string serverName, string remoteFolderPath)
        {
            classInstance = new ManagementClass();

            // initialize process ID and dictionary
            currentProcessID = 0;
            dictResult = new Dictionary<uint, string>();
        }

        #region IDisposable Members
        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    classInstance.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion

        /// <summary>
        /// Mock method of AsynExecute in RemoteCommandLine Class
        /// </summary>
        /// <param name="args">commandline input</param>
        /// <returns>process ID</returns>
        public uint AsynExecute(string args)
        {
            // Save result to dictionary according to command line content
            string strResult = "";
            string strReq = args.ToLower(CultureInfo.InvariantCulture);
            if (strReq.Contains("certreq") && strReq.Contains("-new"))
            {
                strResult = "CertReq: Request Created";
            }
            else if (strReq.Contains("certutil") && strReq.Contains("-ping"))
            {
                strResult = "CertUtil: -ping command completed successfully";
            }
            else if (strReq.Contains("certutil") && strReq.Contains("-cainfo"))
            {
                strResult = "CertUtil: -CAInfo command completed successfully";
            }
            else if (strReq.Contains("certutil") && strReq.Contains("-capropinfo"))
            {
                strResult = "CertUtil: -CAPropInfo command completed successfully";
            }
            
            // add result to dictionary
            ++currentProcessID;
            dictResult.Add(currentProcessID, strResult);
            
            // return processID
            return currentProcessID;
        }

        /// <summary>
        /// Mock Method of GetRunningResult in RemoteCommandLine Class
        /// </summary>
        /// <param name="processId">process ID</param>
        /// <param name="outputResult">output Result</param>
        /// <returns>SUCESS/FAIL status</returns>
        public uint GetRunningResult(uint processId, out string outputResult)
        {
            if (dictResult.ContainsKey(processId))
            {
                outputResult = dictResult[processId];
                return 0;
            }
            outputResult = null;
            return 1;
        }
    }
}
#endif