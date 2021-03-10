using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite.QueryDirectory
{
    [TestClassAttribute()]
    public partial class QuerySearchPatternTestCases : PtfTestClassBase
    {
        #region Variables
        private FSAAdapter fsaAdapter;
        private const uint BytesToWrite = 1024;
        private const int FileNameLength = 20;
        // DOS_STAR can be transmogrify to *.
        private const string DOS_STAR = "*.";
        // DOS_DOT can be transmogrify to .* or .?
        private const string DOS_DOT_I = ".*";
        private const string DOS_DOT_II = ".?";
        // DOS_QM can be transmogrify to ?
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)Site.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.fsaAdapter = new FSAAdapter();
            this.fsaAdapter.Initialize(BaseTestSite);
            this.fsaAdapter.LogTestCaseDescription(BaseTestSite);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Test environment:");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 1. File System: " + this.fsaAdapter.FileSystem.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 2. Transport: " + this.fsaAdapter.Transport.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 3. Share Path: " + this.fsaAdapter.UncSharePath);
            this.fsaAdapter.FsaInitial();
        }

        protected override void TestCleanup()
        {
            this.fsaAdapter.Dispose();
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test cases
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileNamesInformation from the server for search pattern * described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileNamesInformation_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileNamesInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_NAMES_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileNamesInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");

            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();

            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileNamesInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileNamesInformation_SpecialCase_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileNamesInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_NAMES_INFORMATION;

            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*.*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileNamesInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileNamesInformation from the server for search pattern ? described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileNamesInformation_WildCard_QuestionMark()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileNamesInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 2; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_NAMES_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "?" (DOS_QM) within string
            fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("Fi?e", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileNamesInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain entries that match the pattern.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileNamesInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileNamesInformation_KleeneStarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileNamesInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 4; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_NAMES_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*" within string
            fileNames = new List<string> { "File", "Pile", "While", "Tile", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("*ile", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileNamesInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileNamesInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileNamesInformation_DOS_STAR_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileNamesInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_NAMES_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_STAR
            fileNames = new List<string> { "Fine.txt", "FileGrip.txt", "Bile.txt", "Tile.txt", "Nine.jpg" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"{DOS_STAR}txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileNamesInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {4}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileNamesInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileNamesInformation_DOS_DOT_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileNamesInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_NAMES_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .* )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc_BadFile_1.txt", "grc_BadFile_2.txt", "grc_BadFile_3.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc{DOS_DOT_I}", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileNamesInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {2}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileNamesInformation from the server for search pattern *.* described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileNamesInformation_DOS_DOT_QuestionMark_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileNamesInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_NAMES_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .? )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc.BlankFile.3.txt", "grc.BadFile_1.txt", "grc.BadFile_2.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc.BlankFile{ DOS_DOT_II}.txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileNamesInformation>(outputBuffer);

            Site.Assert.AreEqual(3, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(3, filesCount, $"Number of files created should be equal to the number of files returned: {3}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileDirectoryInformation[] fileDirectoryInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*", fileInfoClass, fileNames);

            fileDirectoryInformation = FsaUtility.UnmarshalFileInformationArray<FileDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileDirectoryInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileDirectoryInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileDirectoryInformation[1].FileName), "FileName of the second entry should be \"..\".");

            filesCount = fileNames.Intersect(GetListFileInformation(fileDirectoryInformation)).ToList().Count();

            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_SpecialCase_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileDirectoryInformation[] fileDirectoryInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*.*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*.*", fileInfoClass, fileNames);

            fileDirectoryInformation = FsaUtility.UnmarshalFileInformationArray<FileDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileDirectoryInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileDirectoryInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileDirectoryInformation[1].FileName), "FileName of the second entry should be \"..\".");
            filesCount = fileNames.Intersect(GetListFileInformation(fileDirectoryInformation)).ToList().Count();
            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_WildCard_QuestionMark()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileDirectoryInformation[] fileDirectoryInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 2; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "?" (DOS_QM) within string
            fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("Fi?e", fileInfoClass, fileNames);

            fileDirectoryInformation = FsaUtility.UnmarshalFileInformationArray<FileDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileDirectoryInformation.Length, "The returned Buffer should contain entries that match the pattern.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileDirectoryInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_KleeneStarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileDirectoryInformation[] fileDirectoryInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 4; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*" within string
            fileNames = new List<string> { "File", "Pile", "While", "Tile", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("*ile", fileInfoClass, fileNames);

            fileDirectoryInformation = FsaUtility.UnmarshalFileInformationArray<FileDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileDirectoryInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileDirectoryInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_DOS_STAR_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileDirectoryInformation[] fileDirectoryInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_STAR
            fileNames = new List<string> { "Fine.txt", "FileGrip.txt", "Bile.txt", "Tile.txt", "Nine.jpg" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"{DOS_STAR}txt", fileInfoClass, fileNames);

            fileDirectoryInformation = FsaUtility.UnmarshalFileInformationArray<FileDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileDirectoryInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileDirectoryInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {4}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_DOS_DOT_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileDirectoryInformation[] fileDirectoryInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .* )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc_BadFile_1.txt", "grc_BadFile_2.txt", "grc_BadFile_3.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc{DOS_DOT_I}", fileInfoClass, fileNames);

            fileDirectoryInformation = FsaUtility.UnmarshalFileInformationArray<FileDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileDirectoryInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileDirectoryInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {2}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileDirectoryInformation_DOS_DOT_QuestionMark_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileDirectoryInformation[] fileDirectoryInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_DIRECTORY_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .? )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc.BlankFile.3.txt", "grc.BadFile_1.txt", "grc.BadFile_2.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc.BlankFile{ DOS_DOT_II}.txt", fileInfoClass, fileNames);

            fileDirectoryInformation = FsaUtility.UnmarshalFileInformationArray<FileDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(3, fileDirectoryInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileDirectoryInformation)).ToList().Count();
            Site.Assert.AreEqual(3, filesCount, $"Number of files created should be equal to the number of files returned: {3}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileFullDirectoryInformation_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");

            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();

            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileFullDirectoryInformation_SpecialCase_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*.*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*.*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileFullDirectoryInformation_KleeneStarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 4; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*" within string
            fileNames = new List<string> { "File", "Pile", "While", "Tile", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("*ile", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileFullDirectoryInformation_WildCard_QuestionMark()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 2; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "?" (DOS_QM) within string
            fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("Fi?e", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain entries that match the pattern.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileFullDirectoryInformation_DOS_STAR_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_STAR
            fileNames = new List<string> { "Fine.txt", "FileGrip.txt", "Bile.txt", "Tile.txt", "Nine.jpg" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"{DOS_STAR}txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {4}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileFullDirectoryInformation_DOS_DOT_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .* )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc_BadFile_1.txt", "grc_BadFile_2.txt", "grc_BadFile_3.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc{DOS_DOT_I}", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {2}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileFullDirectoryInformation_DOS_DOT_QuestionMark_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .? )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc.BlankFile.3.txt", "grc.BadFile_1.txt", "grc.BadFile_2.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc.BlankFile{ DOS_DOT_II}.txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(3, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(3, filesCount, $"Number of files created should be equal to the number of files returned: {3}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdFullDirectoryInformation_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdFullDirectoryInformation_SpecialCase_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*.*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*.*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdFullDirectoryInformation_KleeneStarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 4; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*" within string
            fileNames = new List<string> { "File", "Pile", "While", "Tile", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("*ile", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdFullDirectoryInformation_WildCard_QuestionMark()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 2; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "?" (DOS_QM) within string
            fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("Fi?e", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain entries that match the pattern.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdFullDirectoryInformation_DOS_STAR_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_STAR
            fileNames = new List<string> { "Fine.txt", "FileGrip.txt", "Bile.txt", "Tile.txt", "Nine.jpg" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"{DOS_STAR}txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {4}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdFullDirectoryInformation_DOS_DOT_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .* )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc_BadFile_1.txt", "grc_BadFile_2.txt", "grc_BadFile_3.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc{DOS_DOT_I}", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {2}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdFullDirectoryInformation_DOS_DOT_QuestionMark_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdFullDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_FULL_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .? )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc.BlankFile.3.txt", "grc.BadFile_1.txt", "grc.BadFile_2.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc.BlankFile{ DOS_DOT_II}.txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdFullDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(3, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(3, filesCount, $"Number of files created should be equal to the number of files returned: {3}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileBothDirectoryInformation_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileBothDirectoryInformation[] fileInformation;
            int filesCount = 0; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");

            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();

            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileBothDirectoryInformation_SpecialCase_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileBothDirectoryInformation[] fileInformation;
            int filesCount = 0; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*.*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*.*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileBothDirectoryInformation_KleeneStarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 4; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*" within string
            fileNames = new List<string> { "File", "Pile", "While", "Tile", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("*ile", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileBothDirectoryInformation_WildCard_QuestionMark()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 2; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "?" (DOS_QM) within string
            fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("Fi?e", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain entries that match the pattern.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileBothDirectoryInformation_DOS_STAR_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_STAR
            fileNames = new List<string> { "Fine.txt", "FileGrip.txt", "Bile.txt", "Tile.txt", "Nine.jpg" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"{DOS_STAR}txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {4}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileBothDirectoryInformation_DOS_DOT_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .* )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc_BadFile_1.txt", "grc_BadFile_2.txt", "grc_BadFile_3.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc{DOS_DOT_I}", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {2}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileBothDirectoryInformation_DOS_DOT_QuestionMark_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .? )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc.BlankFile.3.txt", "grc.BadFile_1.txt", "grc.BadFile_2.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc.BlankFile{ DOS_DOT_II}.txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(3, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(3, filesCount, $"Number of files created should be equal to the number of files returned: {3}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdBothDirectoryInformation_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdBothDirectoryInformation[] fileInformation;
            int filesCount = 0; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");

            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();

            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdBothDirectoryInformation_SpecialCase_StarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 10; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*.*"
            fileNames = CreateRandomFileNames(numberOfFiles);
            outputBuffer = QueryByWidldCardAndFileInfoClass("*.*", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(12, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(fileInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(fileInformation[1].FileName), "FileName of the second entry should be \"..\".");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(numberOfFiles, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdBothDirectoryInformation_KleeneStarWildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 4; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "*" within string
            fileNames = new List<string> { "File", "Pile", "While", "Tile", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("*ile", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdBothDirectoryInformation_WildCard_QuestionMark()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            int numberOfFiles = 2; //random file name count
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard "?" (DOS_QM) within string
            fileNames = new List<string> { "Fine", "File", "Bile", "Fille", "Nine" };
            outputBuffer = QueryByWidldCardAndFileInfoClass("Fi?e", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain entries that match the pattern.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {numberOfFiles}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdBothDirectoryInformation_DOS_STAR_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_STAR
            fileNames = new List<string> { "Fine.txt", "FileGrip.txt", "Bile.txt", "Tile.txt", "Nine.jpg" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"{DOS_STAR}txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(4, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(4, filesCount, $"Number of files created should be equal to the number of files returned: {4}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdBothDirectoryInformation_DOS_DOT_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .* )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc_BadFile_1.txt", "grc_BadFile_2.txt", "grc_BadFile_3.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc{DOS_DOT_I}", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(2, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(2, filesCount, $"Number of files created should be equal to the number of files returned: {2}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the Query Directory response with FileDirectoryInformation from the server for search patterns described in [MS-FSA] 2.1.4.")]
        public void BVT_QueryDirectoryBySearchPattern_FileIdBothDirectoryInformation_DOS_DOT_QuestionMark_WildCard()
        {
            byte[] outputBuffer;
            List<string> fileNames;
            FileIdBothDirectoryInformation[] fileInformation;
            int filesCount; // Count files returned from the query, that exist in the FileNames list
            FileInfoClass fileInfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION;

            //[MS-FSA] 2.1.4.4 - Test case for wildcard DOS_DOT  ( .? )
            fileNames = new List<string> { "grc.BlankFile.1.txt", "grc.BlankFile.2.txt", "grc.BlankFile.3.txt", "grc.BadFile_1.txt", "grc.BadFile_2.txt" };
            outputBuffer = QueryByWidldCardAndFileInfoClass($"grc.BlankFile{ DOS_DOT_II}.txt", fileInfoClass, fileNames);

            fileInformation = FsaUtility.UnmarshalFileInformationArray<FileIdBothDirectoryInformation>(outputBuffer);

            Site.Assert.AreEqual(3, fileInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            filesCount = fileNames.Intersect(GetListFileInformation(fileInformation)).ToList().Count();
            Site.Assert.AreEqual(3, filesCount, $"Number of files created should be equal to the number of files returned: {3}.");
        }

        #endregion


        #region Utility

        /// <summary>
        /// Create directory
        /// </summary>
        /// <param name="dirName">Direcotry name</param>        
        /// <returns>An NTSTATUS code that specifies the result</returns>
        private MessageStatus CreateDirectory(string dirName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a directory with name: {dirName}");

            MessageStatus status = MessageStatus.SUCCESS;

            status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.OPEN_IF);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create directory and return with status {status}");

            return status;
        }
        /// <summary>
        /// Create directory
        /// </summary>
        /// <param name="dirName">Direcotry name</param>
        /// <param name="fileId">The fileid of the created directory</param>
        /// <param name="treeId">The treeId of the created directory</param>
        /// <param name="sessionId">The sessionId of the created directory</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        private MessageStatus CreateDirectory(
            string dirName,
            out FILEID fileId,
            out uint treeId,
            out ulong sessionId)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a directory with name: {dirName}");

            MessageStatus status = MessageStatus.SUCCESS;

            status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.OPEN_IF,
                        out fileId,
                        out treeId,
                        out sessionId
                        );

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create directory and return with status {status}");

            return status;
        }

        /// </summary>
        /// <param name="dirName">The directory name for query. </param>
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="returnSingleEntry">A boolean indicating whether the return single entry for query.</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isNoRecordsReturned">True: if No Records Returned.</param>
        /// <param name="isOutBufferSizeLess">True: if OutputBufferSize is less than the size needed to return a single entry</param>
        /// <param name="outBufferSize">The state of OutBufferSize in subsection 
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        private MessageStatus QueryDirectory(
            string dirName,
            string searchPattern = "*",
            FileInfoClass fileinfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
            bool returnSingleEntry = false,
            bool restartScan = false,
            bool isDirectoryNotRight = false,
            bool isOutPutBufferNotEnough = false
            )
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query a directory information: {dirName}");

            MessageStatus status = this.fsaAdapter.QueryDirectoryInfo(
              searchPattern,
              FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
              returnSingleEntry,
              restartScan,
              isOutPutBufferNotEnough);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query directory with search pattern {searchPattern} and return with status {status}. ");

            return status;
        }

        /// </summary>
        /// <param name="fileId">The fileid for the directory. </param>
        /// <param name="treeId">The treeId for the directory. </param>
        /// <param name="sessionId">The sessionId for the directory. </param>
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="returnSingleEntry">A boolean indicating whether the return single entry for query.</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isOutBufferSizeLess">True: if OutputBufferSize is less than the size needed to return a single entry</param>
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        private MessageStatus QueryDirectory(
            FILEID fileId,
            uint treeId,
            ulong sessionId,
            string searchPattern = "*",
            FileInfoClass fileinfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
            bool returnSingleEntry = false,
            bool restartScan = false,
            bool isOutPutBufferNotEnough = false
            )
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query a directory information with fileid {fileId}");

            MessageStatus status = this.fsaAdapter.QueryDirectoryInfo(
                fileId,
                treeId,
                sessionId,
                searchPattern,
                FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
                returnSingleEntry,
                restartScan,
                isOutPutBufferNotEnough);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query directory with search pattern {searchPattern} and return with status {status}. ");

            return status;
        }

        /// <summary>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="fileNames">The File Names to be added to the directory. </param>
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="outputBuffer">The buffer containing the directory enumeration being returned. </param>
        /// <param name="dirFileId">The fileid for the directory. </param>
        /// Prepare before testing, including:
        /// 1. creating a new directory
        /// 2. creating a new file under the directory
        /// 3. writing some content to the file
        /// 4. closing the file to flush the data to the disk
        /// Then send QueryDirectory with specified FileInfoClass to the server and return the outputBuffer.
        /// </summary>
        private void PrepareAndQueryDirectory(
            FileInfoClass fileInfoClass,
            List<string> fileNames,
            string searchPattern,
            out byte[] outputBuffer,
            out FILEID dirFileId)
        {
            outputBuffer = null;
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            uint treeId = 0;
            ulong sessionId = 0;

            MessageStatus status = CreateDirectory(dirName, out dirFileId, out treeId, out sessionId);

            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Create should succeed.");

            foreach (string fileName in fileNames)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a file with name: {fileName} under the directory {dirName}");
                status = this.fsaAdapter.CreateFile(
                    $"{dirName}\\{fileName}",
                    (FileAttribute)0,
                    CreateOptions.NON_DIRECTORY_FILE,
                    (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                    (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE),
                    CreateDisposition.OPEN_IF);
                Site.Assert.AreEqual(
                    MessageStatus.SUCCESS,
                    status,
                    $"Create should succeed.");
            }


            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query directory with {fileInfoClass}");
            status = this.fsaAdapter.QueryDirectory(dirFileId, treeId, sessionId, searchPattern, fileInfoClass, false, true, out outputBuffer);
            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Query directory should succeed.");
        }

        /// <summary>
        /// Prepare before testing, including:
        /// 1. creating a new directory
        /// 2. creating a new file under the directory
        /// 3. writing some content to the file
        /// 4. closing the file to flush the data to the disk
        /// Then send QueryDirectory with specified FileInfoClass to the server and return the outputBuffer.
        /// </summary>
        private void PrepareAndQueryDirectory(FileInfoClass fileInfoClass, out byte[] outputBuffer, out string fileName, out FILEID dirFileId)
        {
            outputBuffer = null;
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            uint treeId = 0;
            ulong sessionId = 0;

            MessageStatus status = CreateDirectory(dirName, out dirFileId, out treeId, out sessionId);

            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Create should succeed.");

            fileName = this.fsaAdapter.ComposeRandomFileName(FileNameLength, opt: CreateOptions.NON_DIRECTORY_FILE);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a file with name: {fileName} under the directory {dirName}");
            status = this.fsaAdapter.CreateFile(
                $"{dirName}\\{fileName}",
                (FileAttribute)0,
                CreateOptions.NON_DIRECTORY_FILE,
                (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE),
                CreateDisposition.OPEN_IF);
            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Create should succeed.");
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Write {BytesToWrite} bytes to the file {fileName}");
            long bytesWritten;
            status = this.fsaAdapter.WriteFile(0, BytesToWrite, out bytesWritten);
            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Write should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Close the open to the file {fileName}");
            status = this.fsaAdapter.CloseOpen();
            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Close should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query directory with {fileInfoClass}");
            status = this.fsaAdapter.QueryDirectory(dirFileId, treeId, sessionId, "*", fileInfoClass, false, true, out outputBuffer);
            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Query directory should succeed.");
        }


        /// <summary>
        /// <param name="count">The number of files to be created. </param>
        /// Create n number of file names.
        /// </summary>
        private List<string> CreateRandomFileNames(
            int count)
        {
            List<string> fileNames = new List<string>();

            for (int i = 1; i <= count; i++)
            {
                fileNames.Add(this.fsaAdapter.ComposeRandomFileName(FileNameLength, ".txt", opt: CreateOptions.NON_DIRECTORY_FILE));
            }

            return fileNames;
        }

        /// <summary>
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="fileNames">The File Names to be added to the directory. </param>
        /// Then send QueryDirectory with specified FileInfoClass to the server and return the outputBuffer.
        /// </summary>
        private byte[] QueryByWidldCardAndFileInfoClass(
            string searchPattern,
            FileInfoClass fileInfoClass,
            List<string> fileNames)
        {
            byte[] outputBuffer;
            FILEID dirFileId;

            PrepareAndQueryDirectory(fileInfoClass, fileNames, searchPattern, out outputBuffer, out dirFileId);

            Site.Log.Add(LogEntryKind.Debug, "Start to verify the Query Directory response.");

            return outputBuffer;
        }

        /// <summary>
        /// <param name="fileInformation">The file information array containing file name(s). </param>
        /// Load FileInformation into List of string.
        /// </summary>
        private List<string> GetListFileInformation(FileNamesInformation[] fileInformation)
        {
            List<string> fileInformationList = new List<string>();

            foreach (FileNamesInformation information in fileInformation)
            {
                fileInformationList.Add(Encoding.Unicode.GetString(information.FileName));
            }

            return fileInformationList;
        }

        /// <summary>
        /// <param name="fileInformation">The file information array containing file name(s). </param>
        /// Load FileInformation into List of string.
        /// </summary>
        private List<string> GetListFileInformation(FileFullDirectoryInformation[] fileInformation)
        {
            List<string> fileInformationList = new List<string>();

            foreach (FileFullDirectoryInformation information in fileInformation)
            {
                fileInformationList.Add(Encoding.Unicode.GetString(information.FileName));
            }

            return fileInformationList;
        }

        /// <summary>
        /// <param name="fileInformation">The file information array containing file name(s). </param>
        /// Load FileInformation into List of string.
        /// </summary>
        private List<string> GetListFileInformation(FileIdFullDirectoryInformation[] fileInformation)
        {
            List<string> fileInformationList = new List<string>();

            foreach (FileIdFullDirectoryInformation information in fileInformation)
            {
                fileInformationList.Add(Encoding.Unicode.GetString(information.FileName));
            }

            return fileInformationList;
        }

        /// <summary>
        /// <param name="fileInformation">The file information array containing file name(s). </param>
        /// Load FileInformation into List of string.
        /// </summary>
        private List<string> GetListFileInformation(FileBothDirectoryInformation[] fileInformation)
        {
            List<string> fileInformationList = new List<string>();

            foreach (FileBothDirectoryInformation information in fileInformation)
            {
                fileInformationList.Add(Encoding.Unicode.GetString(information.FileName));
            }

            return fileInformationList;
        }

        /// <summary>
        /// <param name="fileInformation">The file information array containing file name(s). </param>
        /// Load FileInformation into List of string.
        /// </summary>
        private List<string> GetListFileInformation(FileIdBothDirectoryInformation[] fileInformation)
        {
            List<string> fileInformationList = new List<string>();

            foreach (FileIdBothDirectoryInformation information in fileInformation)
            {
                fileInformationList.Add(Encoding.Unicode.GetString(information.FileName));
            }

            return fileInformationList;
        }

        /// <summary>
        /// <param name="fileInformation">The file information array containing file name(s). </param>
        /// Load FileInformation into List of string.
        /// </summary>
        private List<string> GetListFileInformation(FileDirectoryInformation[] fileInformation)
        {
            List<string> fileInformationList = new List<string>();

            foreach (FileDirectoryInformation information in fileInformation)
            {
                fileInformationList.Add(Encoding.Unicode.GetString(information.FileName));
            }

            return fileInformationList;
        }
        #endregion
    }
}
