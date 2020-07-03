// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    public partial class CPMCreateQueryTestCases : WspCommonTestBase
    {
        #region Test Cases

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ItemFolderNameDisplay property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_ItemFolderNameDisplay()
        {
            var queryResult = GetQueryResult(WspConsts.System_ItemFolderNameDisplay, "document");

            foreach (var row in queryResult.Rows)
            {
                var fileName = row.Columns[2].Data as string;
                Site.Assert.AreEqual("CreateQuery_CFullPropSpec", row.Columns[1].Data as string, $"The System.ItemFolderNameDisplay of {fileName} should be CreateQuery_CFullPropSpec");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if Path property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_Path()
        {
            var queryPath = Site.Properties.Get("QueryPath") + "Data/CreateQuery_CFullPropSpec";
            var queryResult = GetQueryResult(WspConsts.Path, "document", queryPath: queryPath);

            var expectedResults = new string[]
            {
                $"{queryPath}/document1.doc",
                $"{queryPath}/document2.ppt",
                $"{queryPath}/document3.xls"
            };

            ValidateQueryResult(nameof(WspConsts.Path), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ItemNameDisplay property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_ItemNameDisplay()
        {
            var queryResult = GetQueryResult(WspConsts.System_ItemNameDisplay, "document");

            var expectedResults = new string[]
            {
                "document1.doc",
                "document2.ppt",
                "document3.xls"
            };

            ValidateQueryResult(nameof(WspConsts.System_ItemNameDisplay), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Size property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_Size()
        {
            var queryResult = GetQueryResult(WspConsts.System_Size, "document");

            var expectedResults = new ulong[]
            {
                23040,
                43520,
                26624
            };

            ValidateQueryResult(nameof(WspConsts.System_Size), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.FileAttributes property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_FileAttributes()
        {
            for (var i = 1; i <= 3; i++)
            {
                var fileName = $"..\\..\\Data\\CreateQuery_CFullPropSpec\\attr{i}.txt";
                wspSutAdapter.ModifyFileAttributes(fileName, i == 2, i == 3);
            }

            var queryResult = GetQueryResult(WspConsts.System_FileAttributes, "attr");

            var expectedResults = new uint[]
            {
                    32, // Archive file
                    33, // Read-only file
                    34  // Hidden file
            };

            ValidateQueryResult(nameof(WspConsts.System_FileAttributes), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.DateModified property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_DateModified()
        {
            var queryResult = GetQueryResult(WspConsts.System_DateModified, "document");

            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is DateTime, $"The System.DateModified column should be in DateTime form.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.DateAccessed property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_DateAccessed()
        {
            var queryResult = GetQueryResult(WspConsts.System_DateAccessed, "document");

            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is DateTime, $"The System.DateAccessed column should be in DateTime form.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Category property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_Category()
        {
            var queryResult = GetQueryResult(WspConsts.System_Category, "document");

            var expectedResults = new string[]
            {
                "memo",
                "schedule",
                "white paper"
            };

            ValidateQueryResult(nameof(WspConsts.System_Category), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Title property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_Title()
        {
            var queryResult = GetQueryResult(WspConsts.System_Title, "document");

            var expectedResults = new string[]
            {
                "Very Important Text",
                "Very Important Slide",
                "Very Important Chart"
            };

            ValidateQueryResult(nameof(WspConsts.System_Title), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ApplicationName property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_ApplicationName()
        {
            var queryResult = GetQueryResult(WspConsts.System_ApplicationName, "document");

            var expectedResults = new string[]
            {
                "Microsoft Office Word",
                "Microsoft Office PowerPoint",
                "Microsoft Excel"
            };

            ValidateQueryResult(nameof(WspConsts.System_ApplicationName), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Music.Artist property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Music_Artist()
        {
            var queryResult = GetQueryResult(WspConsts.System_Music_Artist, "music");

            var expectedResults = new string[]
            {
                "AAA",
                "BBB",
                "CCC",
            };

            ValidateQueryResult(nameof(WspConsts.System_Music_Artist), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Music.Album property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Music_Album()
        {
            var queryResult = GetQueryResult(WspConsts.System_Music_Album, "music");

            var expectedResults = new string[]
            {
                "WSPTest1",
                "WSPTest2",
                "WSPTest3",
            };

            ValidateQueryResult(nameof(WspConsts.System_Music_Album), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Music.Genre property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Music_Genre()
        {
            var queryResult = GetQueryResult(WspConsts.System_Music_Genre, "music");

            var expectedResults = new string[]
            {
                "Pop",
                "Dance",
                "Rock",
            };

            ValidateQueryResult(nameof(WspConsts.System_Music_Genre), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.HorizontalResolution property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_HorizontalResolution()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_HorizontalResolution, "img");

            var expectedResults = new double[]
            {
                96,
                72,
                96,
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_HorizontalResolution), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.Dimensions property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_Dimensions()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_Dimensions, "img");

            var expectedResults = new string[]
            {
                "‪93 x 64‬",
                "‪930 x 636‬",
                "‪111 x 76‬",
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_Dimensions), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.BitDepth property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_BitDepth()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_BitDepth, "img");

            var expectedResults = new uint[]
            {
                24,
                24,
                1,
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_BitDepth), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if AudioFormat property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_AudioFormat()
        {
            var queryResult = GetQueryResult(WspConsts.AudioFormat, "music");

            var expectedResults = new string[]
            {
                "{00001610-0000-0010-8000-00AA00389B71}",
                "{00000055-0000-0010-8000-00AA00389B71}",
                "{00000055-0000-0010-8000-00AA00389B71}",
            };

            ValidateQueryResult(nameof(WspConsts.AudioFormat), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Media.Duration property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_System_Media_Duration()
        {
            var queryResult = GetQueryResult(WspConsts.System_Media_Duration, "music");

            var expectedResults = new ulong[]
            {
                57120861,
                57728750,
                57730500,
            };

            ValidateQueryResult(nameof(WspConsts.System_Media_Duration), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Audio.EncodingBitrate property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_System_Audio_EncodingBitrate()
        {
            var queryResult = GetQueryResult(WspConsts.System_Audio_EncodingBitrate, "music");

            var expectedResults = new uint[]
            {
                163552,
                64000,
                160000,
            };

            ValidateQueryResult(nameof(WspConsts.System_Audio_EncodingBitrate), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.EncodingBitrate property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_EncodingBitrate()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_EncodingBitrate, "video");

            var expectedResults = new uint[]
            {
                58168,
                30584,
                20504,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_EncodingBitrate), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.FrameWidth property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_FrameWidth()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_FrameWidth, "video");

            var expectedResults = new uint[]
            {
                1920,
                1280,
                960,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_FrameWidth), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.FrameHeight property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_FrameHeight()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_FrameHeight, "video");

            var expectedResults = new uint[]
            {
                1080,
                720,
                540,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_FrameHeight), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.HomeTelephone property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_HomeTelephone()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_HomeTelephone, "contact");

            var expectedResults = new string[]
            {
                "123456789",
                "456123789",
                "789456123",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_HomeTelephone), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.EmailAddress property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_EmailAddress()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_EmailAddress, "contact");

            var expectedResults = new string[]
            {
                "AAA.BBB@example.com",
                "CCC.DDD@example.com",
                "EEE.FFF@example.com",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_EmailAddress), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.FullName property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_FullName()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_FullName, "contact");

            var expectedResults = new string[]
            {
                "AAA BBB",
                "CCC DDD",
                "EEE FFF",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_FullName), expectedResults, queryResult);
        }

        #endregion

        private CPMGetRowsOut GetQueryResult(CFullPropSpec property, string queryText, string queryPath = null, uint totalRows = 3)
        {
            queryPath = queryPath ?? Site.Properties.Get("QueryPath") + "Data/CreateQuery_CFullPropSpec";

            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            var columnSet = new CColumnSet();
            columnSet.count = 3;
            columnSet.indexes = new uint[] { 0, 1, 3 };

            CBaseStorageVariant queryPathVaraint = wspAdapter.builder.GetBaseStorageVariant(vType_Values.VT_LPWSTR, new VT_LPWSTR(queryPath));
            var restrictionArray = wspAdapter.builder.GetRestrictionArray(
                wspAdapter.builder.GetPropertyRestriction(_relop_Values.PREQ, WspConsts.System_Search_Scope, queryPathVaraint),
                wspAdapter.builder.GetContentRestriction(queryText, WspConsts.System_FileName, _ulGenerateMethod_Values.GENERATE_METHOD_PREFIX));

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                property,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            var inGroupSortAggregSets = new CInGroupSortAggregSets();
            inGroupSortAggregSets.cCount = 1;
            inGroupSortAggregSets.SortSets = new CSortSet[1];
            inGroupSortAggregSets.SortSets[0].count = 1;
            inGroupSortAggregSets.SortSets[0].sortArray = new CSort[1];
            inGroupSortAggregSets.SortSets[0].sortArray[0].dwOrder = dwOrder_Values.QUERY_SORTASCEND;
            inGroupSortAggregSets.SortSets[0].sortArray[0].pidColumn = 3; // Sort by file name.
            inGroupSortAggregSets.SortSets[0].sortArray[0].locale = wspAdapter.builder.parameter.LCID_VALUE;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, inGroupSortAggregSets, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.builder.parameter.LCID_VALUE);

            var columns = new CTableColumn[]
            {
                wspAdapter.builder.GetTableColumn(WspConsts.System_ItemName, vType_Values.VT_VARIANT),
                wspAdapter.builder.GetTableColumn(property, vType_Values.VT_VARIANT),
                wspAdapter.builder.GetTableColumn(WspConsts.System_FileName, vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(totalRows, getRowsOut._cRowsReturned, $"Number of rows returned should be {totalRows}.");

            for (var i = 0; i < totalRows; i++)
            {
                var fileNamePrefix = $"{queryText}{i + 1}.";
                Site.Assert.IsTrue((getRowsOut.Rows[i].Columns[2].Data as string).StartsWith(fileNamePrefix), $"The file name of Row {i} in Ascend order should start with \"{fileNamePrefix}\".");
            }

            return getRowsOut;
        }

        private void ValidateQueryResult<T>(string propertyConstantName, T[] expectedResults, CPMGetRowsOut queryResult)
        {
            var propertyName = propertyConstantName.Replace("_", ".");
            for (var i = 0; i < expectedResults.Length; i++)
            {
                var fileName = queryResult.Rows[i].Columns[2].Data as string;
                if (queryResult.Rows[i].Columns[1].rowVariant.vType.HasFlag(vType_Values.VT_VECTOR))
                {
                    Site.Assert.AreEqual(expectedResults[i], (queryResult.Rows[i].Columns[1].Data as T[])[0], $"The {propertyName} of {fileName} shoulde be {expectedResults[i]}");
                }
                else
                {
                    Site.Assert.AreEqual(expectedResults[i], (T)queryResult.Rows[i].Columns[1].Data, $"The {propertyName} of {fileName} shoulde be {expectedResults[i]}");
                }
            }
        }
    }
}