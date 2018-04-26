// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestManager.Detector;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    partial class SMBDDetector
    {
        public bool PingSUT()
        {
            DetectorUtil.WriteLog("Ping Target SUT...");

            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default TtL value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "0123456789ABCDEF0123456789ABCDEF";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 5000;
            bool result = false;
            List<PingReply> replys = new List<PingReply>();
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    replys.Add(pingSender.Send(DetectionInfo.SUTName, timeout, buffer, options));
                }

            }
            catch
            {
                DetectorUtil.WriteLog("Error", false, LogStyle.Error);

                //return false;
                throw;
            }
            foreach (var reply in replys)
            {

                result |= (reply.Status == IPStatus.Success);
            }
            if (result)
            {
                DetectorUtil.WriteLog("Passed", false, LogStyle.StepPassed);
                return true;
            }
            else
            {
                DetectorUtil.WriteLog("Failed", false, LogStyle.StepFailed);
                DetectorUtil.WriteLog("Target SUT don't respond.");
                return false;
            }

        }

    }
}
