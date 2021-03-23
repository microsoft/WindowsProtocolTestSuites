// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite.TraditionalTestCases.QueryDirectory
{
    partial class QueryDirectoryTestCases : PtfTestClassBase
    {
         public void BVT_QueryDirectoryBySearchPattern<T>(
            T[] FileInformation,
            FileInfoClass FileInformationClass,
            string WildCard,
            int FilesInDirectoryCount,
            int ExpectedFilesReturnedLength
            )
            where T : struct
        {
            byte[] outputBuffer;
            List<string> fileNames;

            int filesCount; // Count files returned from the query, that exist in the FileNames list

            fileNames = CreateRandomFileNames(FilesInDirectoryCount);
            outputBuffer = QueryByWildCardAndFileInfoClass(WildCard, FileInformationClass, fileNames);

            FileInformation = FsaUtility.UnmarshalFileInformationArray<T>(outputBuffer);
            dynamic dynamicFileInformationObject = FileInformation;
            filesCount = fileNames.Intersect(GetListFileInformation<T>(FileInformation)).ToList().Count();

            Site.Assert.AreEqual(ExpectedFilesReturnedLength, FileInformation.Length, $"The returned Buffer should contain {ExpectedFilesReturnedLength} entries of FileInformation.");
            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(dynamicFileInformationObject[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(dynamicFileInformationObject[1].FileName), "FileName of the second entry should be \"..\".");
            Site.Assert.AreEqual(FilesInDirectoryCount, filesCount, $"Number of files created should be equal to the number of files returned: {FilesInDirectoryCount}.");
        }

        public void BVT_QueryDirectoryBySearchPattern<T>(
           T[] FileInformation,
           FileInfoClass FileInformationClass,
           List<string> FileNames,
           string WildCard,
           int ExpectedFilesReturnedLength
           )
           where T : struct
        {
            byte[] outputBuffer;

            int filesCount; // Count files returned from the query, that exist in the FileNames list

            outputBuffer = QueryByWildCardAndFileInfoClass(WildCard, FileInformationClass, FileNames);
            FileInformation = FsaUtility.UnmarshalFileInformationArray<T>(outputBuffer);
            dynamic dynamicFileInformationObject = FileInformation;

            filesCount = FileNames.Intersect(GetListFileInformation<T>(FileInformation)).ToList().Count();

            Site.Assert.AreEqual(ExpectedFilesReturnedLength, FileInformation.Length, $"The returned Buffer should contain {ExpectedFilesReturnedLength} entries of FileInformation.");
            Site.Assert.AreEqual(ExpectedFilesReturnedLength, filesCount, $"Number of files returned should match the number of files that match the pattern: {ExpectedFilesReturnedLength}.");
        }
    }
}
