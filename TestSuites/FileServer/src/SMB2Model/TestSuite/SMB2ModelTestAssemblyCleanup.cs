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
            var skipCleanup = false;

            foreach (var directory in ModelManagedAdapterBase.AllTestDirectories)
            {
                if (skipCleanup)
                {
                    break;
                }

                try
                {
                    Directory.Delete(directory);
                }
                catch (UnauthorizedAccessException)
                {
                    skipCleanup = true;
                }
                catch
                {

                }
            }

            foreach (var file in ModelManagedAdapterBase.AllTestFiles)
            {
                if (skipCleanup)
                {
                    break;
                }

                try
                {
                    File.Delete(file);
                }
                catch (UnauthorizedAccessException)
                {
                    skipCleanup = true;
                }
                catch
                {

                }
            }
        }
    }
}
