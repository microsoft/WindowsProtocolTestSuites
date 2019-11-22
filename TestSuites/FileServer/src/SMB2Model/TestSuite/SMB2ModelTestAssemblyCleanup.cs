// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.TestSuite
{
    [TestClass]
    public class SMB2ModelTestAssemblyCleanup
    {
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            foreach (var directory in ModelManagedAdapterBase.AllTestDirectories)
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch (UnauthorizedAccessException)
                {
                    break;
                }
                catch
                {

                }
            }

            foreach (var file in ModelManagedAdapterBase.AllTestFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (UnauthorizedAccessException)
                {
                    break;
                }
                catch
                {

                }
            }
        }
    }
}
