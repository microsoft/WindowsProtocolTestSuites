using Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Directory.Delete(directory);
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
                catch
                {

                }
            }
        }
    }
}
