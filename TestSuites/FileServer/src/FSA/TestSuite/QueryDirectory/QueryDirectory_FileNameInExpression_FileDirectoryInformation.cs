// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite.TraditionalTestCases.QueryDirectory
{
    public partial class QueryDirectoryTestCases : PtfTestClassBase
    {

        #region Test cases
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search pattern * described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_AsteriskWildCard()
        {
            var fileInformation = new List<FileDirectoryInformation>();
            int filesInDirectoryCount = 10;
            int expectedFilesReturnedLength = 12;
            var wildCard = "*";

            BVT_QueryDirectoryBySearchPattern<FileDirectoryInformation>(
                fileInformation.ToArray(),
                FileInfoClass.FILE_DIRECTORY_INFORMATION,
                wildCard,
                filesInDirectoryCount,
                expectedFilesReturnedLength);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_SpecialCase_AsteriskWildCard()
        {
            var fileInformation = new List<FileDirectoryInformation>();
            int filesInDirectoryCount = 10;
            int expectedFilesReturnedLength = 12;
            var wildCard = "*.*";

            BVT_QueryDirectoryBySearchPattern<FileDirectoryInformation>(
                fileInformation.ToArray(),
                FileInfoClass.FILE_DIRECTORY_INFORMATION,
                wildCard,
                filesInDirectoryCount,
                expectedFilesReturnedLength);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search pattern ? described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_WildCard_QuestionMark()
        {
            var fileInformation = new List<FileDirectoryInformation>();
            int expectedFilesReturnedLength = 2;
            var fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            var wildCard = "Fi?e";

            BVT_QueryDirectoryBySearchPattern<FileDirectoryInformation>(
                fileInformation.ToArray(),
                FileInfoClass.FILE_DIRECTORY_INFORMATION,
                fileNames,
                wildCard,
                expectedFilesReturnedLength);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_AsteriskInStringWildCard()
        {
            var fileInformation = new List<FileDirectoryInformation>();
            int expectedFilesReturnedLength = 2;
            var fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            var wildCard = "*ile";

            BVT_QueryDirectoryBySearchPattern<FileDirectoryInformation>(
                fileInformation.ToArray(),
                FileInfoClass.FILE_DIRECTORY_INFORMATION,
                fileNames,
                wildCard,
                expectedFilesReturnedLength);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_DOS_STAR_WildCard()
        {
            var fileInformation = new List<FileDirectoryInformation>();
            int expectedFilesReturnedLength = 4;
            var fileNames = new List<string> { "Fine.txt", "FileGrip.txt", "Bile.txt", "Tile.txt", "Nine.jpg" };
            var wildCard = $"{DOS_STAR}txt";

            BVT_QueryDirectoryBySearchPattern<FileDirectoryInformation>(
                fileInformation.ToArray(),
                FileInfoClass.FILE_DIRECTORY_INFORMATION,
                fileNames,
                wildCard,
                expectedFilesReturnedLength);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_DOS_DOT_WildCard()
        {
            var fileInformation = new List<FileDirectoryInformation>();
            int expectedFilesReturnedLength = 2;
            var fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc_BadFile_1.txt", "grc_BadFile_2.txt", "grc_BadFile_3.txt" };
            var wildCard = $"grc{DOS_DOT_I}";

            BVT_QueryDirectoryBySearchPattern<FileDirectoryInformation>(
                fileInformation.ToArray(),
                FileInfoClass.FILE_DIRECTORY_INFORMATION,
                fileNames,
                wildCard,
                expectedFilesReturnedLength);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_DOS_DOT_QuestionMark_WildCard()
        {
            var fileInformation = new List<FileDirectoryInformation>();
            int expectedFilesReturnedLength = 3;
            var fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc.BlankFile.3.txt", "grc.BadFile_1.txt", "grc.BadFile_2.txt" };
            var wildCard = $"grc.BlankFile{ DOS_DOT_II}.txt";

            BVT_QueryDirectoryBySearchPattern<FileDirectoryInformation>(
                fileInformation.ToArray(),
                FileInfoClass.FILE_DIRECTORY_INFORMATION,
                fileNames,
                wildCard,
                expectedFilesReturnedLength);
        }


        #endregion

    }
}
