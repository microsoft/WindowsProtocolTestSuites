// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMCreateQueryTestCases : WspCommonTestBase
    {
        private IWspSutAdapter wspSutAdapter;

        private WspAdapter prepareAdapter;

        // Size values of files under test whose size is queried.
        private int test1Size;
        private int test27Size;
        private int test132Size;

        private ulong document1Size;
        private ulong document2Size;
        private ulong document3Size;

        public enum ArgumentType
        {
            AllValid,
            ColumnSetAbsent,
            EmptyColumnSet,
            InvalidColumnSet,
            MismatchedColumnSet,
            RestrictionArrayAbsent,
            PidMapperAbsent,
            AlternativeCMaxResultsValue,
            InvalidSearchScope
        }

        public enum SortingOrder
        {
            PhoneticOrder,
            StrokeCountOrder
        }

        public enum WildcardType
        {
            Asterisk,
            QuestionMark
        }

        private ArgumentType argumentType;

        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            wspAdapter.CPMCreateQueryOutResponse -= EnsureSuccessfulCPMCreateQueryOut;
            wspAdapter.CPMCreateQueryOutResponse += CPMCreateQueryOut;

            wspAdapter.CPMSetBindingsInResponse -= EnsureSuccessfulCPMSetBindingsOut;
            wspAdapter.CPMSetBindingsInResponse += CPMSetBindingsOut;

            wspAdapter.CPMGetRowsOutResponse -= EnsureSuccessfulCPMGetRowsOut;
            wspAdapter.CPMGetRowsOutResponse += CPMGetRowsOut;

            wspSutAdapter = Site.GetAdapter<IWspSutAdapter>();

            test1Size = int.Parse(Site.Properties.Get("Test1Size"));
            test27Size = int.Parse(Site.Properties.Get("Test27Size"));
            test132Size = int.Parse(Site.Properties.Get("Test132Size"));

            document1Size = ulong.Parse(Site.Properties.Get("Document1Size"));
            document2Size = ulong.Parse(Site.Properties.Get("Document2Size"));
            document3Size = ulong.Parse(Site.Properties.Get("Document3Size"));
        }

        protected override void TestCleanup()
        {
            prepareAdapter?.Reset();
            base.TestCleanup();
        }
        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test the basic functionality of CPMCreateQuery.")]
        public void BVT_CPMCreateQuery()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with a regular expression by searching file extension.")]
        public void CPMCreateQuery_FileExtension_RegularExpression()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            CBaseStorageVariant searchSope = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(Site.Properties.Get("QueryPath") + "Data/Test"));
            CBaseStorageVariant querytext = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR("|[.]doc*"));
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PREQ, WspConsts.System_Search_Scope, searchSope),
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PRRE, WspConsts.System_FileExtension, querytext));

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
                WspConsts.System_FileExtension
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn with query \".doc\" and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, CreateSortSets(), null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);


            Site.Assert.AreEqual((uint)4, getRowsOut._cRowsReturned, "The number of rows returned should be 4.");
            var fileNameList = new string[] { "test13.doc", "test15.docx", "test17.docx", "test127.doc" };
            for (int i = 0; i < 4; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, $"The file name of Row {i} should be {fileNameList[i]}.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with an asterisk in the query.")]
        public void CPMCreateQuery_FileName_Wildcard_Asterisk()
        {
            CPMCreateQuery_FileName_Wildcard(WildcardType.Asterisk);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with a question mark in the query.")]
        public void CPMCreateQuery_FileName_Wildcard_QuestionMark()
        {
            CPMCreateQuery_FileName_Wildcard(WildcardType.QuestionMark);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with a not-equal comparison.")]
        public void CPMCreateQuery_FileName_NotEqual()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            CBaseStorageVariant searchSope = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(Site.Properties.Get("QueryPath") + "Data/CreateQuery_FileNameNotEqual"));
            CBaseStorageVariant querytext = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR("test"));
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PREQ, WspConsts.System_Search_Scope, searchSope),
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PRNE, WspConsts.System_FileName, querytext));

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn with a not-equal query \"test\" and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, CreateSortSets(), null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual((uint)2, getRowsOut._cRowsReturned, "The number of rows returned should be 2.");
            var fileNameList = new string[] { "1", "2" };
            for (int i = 0; i < 2; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, $"The file name of Row {i} should be {fileNameList[i]}.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery by querying file size with a greater-than comparison.")]
        public void CPMCreateQuery_Size_GreaterThan()
        {
            CPMCreateQuery_Size(CPropertyRestriction_relop_Values.PRGT, test1Size + 1);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery by querying file size with a greater-than or equal-to comparison.")]
        public void CPMCreateQuery_Size_GreaterThanOrEqualTo()
        {
            CPMCreateQuery_Size(CPropertyRestriction_relop_Values.PRGE, test1Size);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery by querying file size with a less-than comparison.")]
        public void CPMCreateQuery_Size_LessThan()
        {
            CPMCreateQuery_Size(CPropertyRestriction_relop_Values.PRLT, test1Size + 1);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery by querying file size with a less-than or equal-to comparison.")]
        public void CPMCreateQuery_Size_LessThanOrEqualTo()
        {
            CPMCreateQuery_Size(CPropertyRestriction_relop_Values.PRLE, test1Size);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with _ulType of CRestriction set to RTOr.")]
        public void CPMCreateQuery_CRestriction_ulType_RTOr()
        {
            CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values.RTOr);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with _ulType of CRestriction set to RTAnd.")]
        public void CPMCreateQuery_CRestriction_ulType_RTAnd()
        {
            CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values.RTAnd);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with _ulType of CRestriction set to RTNone.")]
        public void CPMCreateQuery_CRestriction_ulType_RTNone()
        {
            CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values.RTNone);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with _ulType of CRestriction set to RTNot.")]
        public void CPMCreateQuery_CRestriction_ulType_RTNot()
        {
            CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values.RTNot);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with _ulType of CRestriction set to RTProperty.")]
        public void CPMCreateQuery_CRestriction_ulType_RTProperty()
        {
            CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values.RTProperty);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with _ulType of CRestriction set to RTContent.")]
        public void CPMCreateQuery_CRestriction_ulType_RTContent()
        {
            CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values.RTContent);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test CPMCreateQuery with _ulType of CRestriction set to RTReuseWhere.")]
        public void CPMCreateQuery_CRestriction_ulType_RTReuseWhere()
        {
            CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values.RTReuseWhere);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if no ColumnSet is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_ColumnSetAbsent()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn without ColumnSet.");
            argumentType = ArgumentType.ColumnSetAbsent;
            wspAdapter.CPMCreateQueryIn(null, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if any columns requested in CPMSetBindingsIn when an empty ColumnSet is sent in the previous CPMCreateQueryIn.")]
        public void CPMCreateQuery_EmptyColumnSet()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);

            var columnSet = new CColumnSet();
            columnSet.count = 0;
            columnSet.indexes = new uint[] { };

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn with an empty ColumnSet.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemFolderNameDisplay, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects DB_E_BADCOLUMNID if the ColumnSet of the previous CPMCreateQueryIn is empty.");
            argumentType = ArgumentType.EmptyColumnSet;
            wspAdapter.CPMSetBindingsIn(columns);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if an invalid ColumnSet whose count is smaller than the length of indexes is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_InvalidColumnSet_CountSmallerThanLengthOfIndexes()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);

            var columnSet = new CColumnSet();
            columnSet.count = 2;
            columnSet.indexes = new uint[] { 0, 1, 2, 3 };

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn with an invalid ColumnSet whose count is smaller than the length of indexes and expects ERROR_INVALID_PARAMETER.");
            Site.Log.Add(LogEntryKind.Debug, $"columnSet.count: {columnSet.count}");
            Site.Log.Add(LogEntryKind.Debug, $"Length of columnSet.indexes: {columnSet.indexes.Length}");
            argumentType = ArgumentType.InvalidColumnSet;
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if an invalid ColumnSet whose count is larger than the length of indexes is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_InvalidColumnSet_CountLargerThanLengthOfIndexes()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);

            var columnSet = new CColumnSet();
            columnSet.count = 4;
            columnSet.indexes = new uint[] { 0, 1 };

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn with an invalid ColumnSet whose count is larger than the length of indexes and expects ERROR_INVALID_PARAMETER.");
            Site.Log.Add(LogEntryKind.Debug, $"columnSet.count: {columnSet.count}");
            Site.Log.Add(LogEntryKind.Debug, $"Length of columnSet.indexes: {columnSet.indexes.Length}");
            argumentType = ArgumentType.InvalidColumnSet;
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if the cloumns requested in CPMSetBindingsIn do not match the ColumnSet of previous CPMCreateQueryIn.")]
        public void CPMCreateQuery_MismatchedColumnSet()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_FileName);

            var columnSet = new CColumnSet();
            columnSet.count = 2;
            columnSet.indexes = new uint[] { 0, 1 };

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Search_Scope, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_FileName, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects DB_E_BADCOLUMNID if the columns requested are not present in the ColumnSet of the previous CPMCreateQueryIn.");
            argumentType = ArgumentType.MismatchedColumnSet;
            wspAdapter.CPMSetBindingsIn(columns);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if no RestrictionArray is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_RestrictionArrayAbsent()
        {
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Search_Contents,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn without RestrictionArray.");
            argumentType = ArgumentType.RestrictionArrayAbsent;
            wspAdapter.CPMCreateQueryIn(columnSet, null, null, new CCategorizationSet(), new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if no PidMapper is sent in CPMCreateQueryIn.")]
        public void CPMCreateQuery_PidMapperAbsent()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn without PidMapper.");

            var columnSet = wspAdapter.Builder.GetColumnSet(2);

            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), Site.Properties.Get("QueryPath"), WspConsts.System_FileName);

            argumentType = ArgumentType.PidMapperAbsent;
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, new CCategorizationSet(), new CRowsetProperties(), new CPidMapper(), new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if image info can be retrieved by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_QueryImage()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("*.png", Site.Properties.Get("QueryPath"), WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Image_HorizontalSize,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Image_HorizontalSize, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual((uint)1, getRowsOut._cRowsReturned, "The number of image file should be 1.");
            Site.Assert.AreEqual("test.png", getRowsOut.Rows[0].Columns[0].Data, "The name of the image file should be test.png.");
            Site.Assert.AreEqual(1279, Convert.ToInt32(getRowsOut.Rows[0].Columns[1].Data), "The HorizontalSize of the image file should be 1279.");
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the create date of the file can be retrieved by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_QueryDateCreated()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("test106.txt", Site.Properties.Get("QueryPath"), WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_DateCreated,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_DateCreated, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual((uint)1, getRowsOut._cRowsReturned, "The number of row returned should be 1.");
            Site.Assert.AreEqual("test106.txt", getRowsOut.Rows[0].Columns[0].Data.ToString().ToLower(), "The file name should be test106.txt.");
            Site.Assert.IsTrue(getRowsOut.Rows[0].Columns[1].Data is DateTime, "The second column of the row should be in DateTime format.");
            Site.Log.Add(LogEntryKind.Debug, "The create date is {0}", getRowsOut.Rows[0].Columns[1].Data);
        }


        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the size of the files can be retrieved in ascend order by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_SortAscend()
        {
            CPMCreateQuery_Sort(ascend: true);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the size of the files can be retrieved in descend order by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_SortDescend()
        {
            CPMCreateQuery_Sort(ascend: false);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the author of the files can be retrieved and ordered in Chinese phonetic order by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_SortLCID2052()
        {
            CPMCreateQuery_SortByAuthorsWithLCID(SortingOrder.PhoneticOrder);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if the author of the files can be retrieved and ordered in Chinese stroke count order by CPMCreateQuery.")]
        public void BVT_CPMCreateQuery_SortLCID133124()
        {
            CPMCreateQuery_SortByAuthorsWithLCID(SortingOrder.StrokeCountOrder);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to verify the server response if an invalid search scope is specified in CPMCreateQueryIn.")]
        public void BVT_CPMCreateQuery_InvalidSearchScope()
        {
            argumentType = ArgumentType.InvalidSearchScope;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            string invalidQueryPath = "file://sut/Invalid/";
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(Site.Properties.Get("QueryText"), invalidQueryPath, WspConsts.System_Search_Contents);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects QRY_E_INVALIDSCOPES.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);
        }
        #endregion

        /// <summary>
        /// Construct a default SortSet
        /// </summary>
        private CInGroupSortAggregSets CreateSortSets(params uint[] pidColumns)
        {
            if (pidColumns == null || pidColumns.Length == 0)
            {
                // Set default value for pidColumns.
                pidColumns = new uint[1] { 0 };
            }

            CInGroupSortAggregSets inGroupSortAggregSets = new CInGroupSortAggregSets();
            inGroupSortAggregSets.cCount = 1;
            inGroupSortAggregSets.SortSets = new CSortSet[1];
            inGroupSortAggregSets.SortSets[0].count = (uint)pidColumns.Length;
            inGroupSortAggregSets.SortSets[0].sortArray = new CSort[pidColumns.Length];

            for (int i = 0; i < pidColumns.Length; i++)
            {
                inGroupSortAggregSets.SortSets[0].sortArray[i].dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND;
                inGroupSortAggregSets.SortSets[0].sortArray[i].pidColumn = pidColumns[i];
                inGroupSortAggregSets.SortSets[0].sortArray[i].locale = wspAdapter.Builder.Parameter.LcidValue;
                inGroupSortAggregSets.SortSets[0].sortArray[i].dwIndividual = CSort_dwIndividual_Values.QUERY_SORTALL;
            }

            return inGroupSortAggregSets;
        }

        private void CPMCreateQuery_FileName_Wildcard(WildcardType type)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            string queryString = null;
            uint expectedRowsCount = 0;
            string[] fileNameList = null;
            switch (type)
            {
                case WildcardType.Asterisk:
                    queryString = "test12*";
                    expectedRowsCount = 5;
                    fileNameList = new string[] { "test121.txt", "test122.txt", "test127.doc", "test128.txt", "test129.cpp" };
                    break;
                case WildcardType.QuestionMark:
                    queryString = "test12?.txt";
                    expectedRowsCount = 3;
                    fileNameList = new string[] { "test121.txt", "test122.txt", "test128.txt" };
                    break;
                default:
                    break;
            }
            CBaseStorageVariant searchSope = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(Site.Properties.Get("QueryPath") + "Data/Test"));
            CBaseStorageVariant querytext = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(queryString));
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PREQ, WspConsts.System_Search_Scope, searchSope),
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PRRE, WspConsts.System_FileName, querytext));

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, $"Client sends CPMCreateQueryIn with query {queryString} and expects success.");
            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, CreateSortSets(), null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(expectedRowsCount, getRowsOut._cRowsReturned, $"The number of row returned should be {expectedRowsCount}.");
            for (int i = 0; i < expectedRowsCount; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, $"The file name of Row {i} should be {fileNameList[i]}.");
            }
        }

        private void CPMCreateQuery_Size(CPropertyRestriction_relop_Values relation, int comparedSize)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            CBaseStorageVariant searchSope = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size"));
            CBaseStorageVariant size = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_INT, comparedSize);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PREQ, WspConsts.System_Search_Scope, searchSope),
                wspAdapter.Builder.GetPropertyRestriction(relation, WspConsts.System_Size, size));

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Size
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;
            string log = null;
            uint expectedRowsCount = 0;
            string[] fileNameList = null;
            switch (relation)
            {
                case CPropertyRestriction_relop_Values.PRLT:
                    log = $"less than {comparedSize}";
                    expectedRowsCount = 2;
                    fileNameList = new string[] { "test1.bin", "test27.bin" };
                    break;
                case CPropertyRestriction_relop_Values.PRLE:
                    log = $"less than or equal to {comparedSize}";
                    expectedRowsCount = 2;
                    fileNameList = new string[] { "test1.bin", "test27.bin" };
                    break;
                case CPropertyRestriction_relop_Values.PRGT:
                    log = $"greater than {comparedSize}";
                    expectedRowsCount = 1;
                    fileNameList = new string[] { "test132.bin" };
                    break;
                case CPropertyRestriction_relop_Values.PRGE:
                    log = $"greater than or equal to {comparedSize}";
                    expectedRowsCount = 2;
                    fileNameList = new string[] { "test1.bin", "test132.bin" };
                    break;
                default:
                    throw new Exception($"The relation should not be {relation}!");
            }

            Site.Log.Add(LogEntryKind.TestStep, $"Client sends CPMCreateQueryIn to query the file whose size is {log} and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, CreateSortSets(), null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(expectedRowsCount, getRowsOut._cRowsReturned, $"The number of row returned should be {expectedRowsCount}.");
            for (int i = 0; i < expectedRowsCount; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, $"The file name of Row {i} should be {fileNameList[i]}.");
            }
        }

        private void CPMCreateQuery_CRestriction_ulType(CRestriction_ulType_Values ulType)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            int comparedSize = 0;
            string fileQueryPath = Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size";
            string fileQueryString = null;
            string[] fileNameList = null;
            string log = null;
            CPropertyRestriction_relop_Values relation = CPropertyRestriction_relop_Values.PRLT;
            uint expectedRowsCount = 0;

            switch (ulType)
            {
                case CRestriction_ulType_Values.RTNone:
                    log = "with ulType set to RTNone";
                    expectedRowsCount = 0;
                    break;
                case CRestriction_ulType_Values.RTAnd:
                    comparedSize = test132Size;
                    fileQueryString = "test?.bin";
                    relation = CPropertyRestriction_relop_Values.PRLT;
                    log = $"whose size is less than {comparedSize} bytes and whose file name matches {fileQueryString}";
                    expectedRowsCount = 1;
                    fileNameList = new string[] { "test1.bin" };
                    break;
                case CRestriction_ulType_Values.RTOr:
                    comparedSize = test1Size;
                    fileQueryString = "test?.bin";
                    relation = CPropertyRestriction_relop_Values.PRGT;
                    log = $"whose size is larger than {comparedSize} bytes or whose file name matches {fileQueryString}";
                    expectedRowsCount = 2;
                    fileNameList = new string[] { "test1.bin", "test132.bin" };
                    break;
                case CRestriction_ulType_Values.RTNot:
                    comparedSize = test1Size;
                    relation = CPropertyRestriction_relop_Values.PRLT;
                    log = $"whose size is NOT less than {comparedSize}";
                    expectedRowsCount = 2;
                    fileNameList = new string[] { "test1.bin", "test132.bin" };
                    break;
                case CRestriction_ulType_Values.RTProperty:
                    comparedSize = test132Size;
                    relation = CPropertyRestriction_relop_Values.PRGE;
                    log = $"whose size is larger than or equal to {comparedSize} bytes";
                    expectedRowsCount = 1;
                    fileNameList = new string[] { "test132.bin" };
                    break;
                case CRestriction_ulType_Values.RTContent:
                    log = $"whose content contains {fileQueryString}";
                    fileQueryPath = Site.Properties.Get("QueryPath") + "Data/Test";
                    fileQueryString = "Adapter";
                    expectedRowsCount = 2;
                    fileNameList = new string[] { "test13.doc", "test17.docx" };
                    break;
                case CRestriction_ulType_Values.RTReuseWhere:
                    fileQueryPath = Site.Properties.Get("QueryPath") + "Data/Test";
                    expectedRowsCount = 2;
                    fileNameList = new string[] { "test13.doc", "test17.docx" };
                    break;

                default:
                    throw new NotImplementedException($"The test case of ulType {ulType} is not implemented.");
            }

            CBaseStorageVariant size = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_INT, comparedSize);
            CBaseStorageVariant querytext = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(fileQueryString));

            // Construct restriction according to _ulType
            CRestriction fileRestriction;
            switch (ulType)
            {
                case CRestriction_ulType_Values.RTNone:
                    fileRestriction = new CRestriction();
                    fileRestriction._ulType = CRestriction_ulType_Values.RTNone;
                    fileRestriction.Restriction = null;
                    break;
                case CRestriction_ulType_Values.RTAnd:
                case CRestriction_ulType_Values.RTOr:
                    fileRestriction = wspAdapter.Builder.GetNodeRestriction(ulType,
                    wspAdapter.Builder.GetPropertyRestriction(relation, WspConsts.System_Size, size),
                    wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PRRE, WspConsts.System_FileName, querytext));
                    break;
                case CRestriction_ulType_Values.RTNot:
                    fileRestriction = new CRestriction();
                    fileRestriction._ulType = ulType;
                    fileRestriction.Restriction = wspAdapter.Builder.GetPropertyRestriction(relation, WspConsts.System_Size, size);
                    break;
                case CRestriction_ulType_Values.RTContent:
                    fileRestriction = wspAdapter.Builder.GetContentRestriction(fileQueryString, WspConsts.System_Search_Contents);
                    break;
                case CRestriction_ulType_Values.RTProperty:
                    fileRestriction = wspAdapter.Builder.GetPropertyRestriction(relation, WspConsts.System_Size, size);
                    break;
                case CRestriction_ulType_Values.RTReuseWhere:
                    var reuseWhere = new CReuseWhere();
                    reuseWhere.whereID = GetWhereID(); // User whereID of another WSP client to share the same query.
                    fileRestriction = new CRestriction();
                    fileRestriction._ulType = CRestriction_ulType_Values.RTReuseWhere;
                    fileRestriction.Restriction = reuseWhere;
                    break;
                default:
                    throw new NotImplementedException($"The test case of ulType {ulType} is not implemented.");
            }

            CBaseStorageVariant searchSope = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(fileQueryPath));
            var scopeRestriction = wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PREQ, WspConsts.System_Search_Scope, searchSope);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(scopeRestriction, fileRestriction);

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_Size
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, $"Client sends CPMCreateQueryIn to query the file {log} and expects success.");
            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, CreateSortSets(), null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(expectedRowsCount, getRowsOut._cRowsReturned, $"The number of row returned should be {expectedRowsCount}.");
            for (int i = 0; i < expectedRowsCount; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, $"The file name of Row {i} should be {fileNameList[i]}.");
            }
        }

        private void CPMCreateQuery_Sort(bool ascend)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("*.bin", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Size", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Size,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            CInGroupSortAggregSets inGroupSortAggregSets = new CInGroupSortAggregSets();
            inGroupSortAggregSets.cCount = 1;
            inGroupSortAggregSets.SortSets = new CSortSet[1];
            inGroupSortAggregSets.SortSets[0].count = 1;
            inGroupSortAggregSets.SortSets[0].sortArray = new CSort[1];
            inGroupSortAggregSets.SortSets[0].sortArray[0].dwOrder = ascend ? CSort_dwOrder_Values.QUERY_SORTASCEND : CSort_dwOrder_Values.QUERY_DESCEND;
            inGroupSortAggregSets.SortSets[0].sortArray[0].pidColumn = 1; // Sort by Size.

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, inGroupSortAggregSets, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Size, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual((uint)3, getRowsOut._cRowsReturned, "Number of rows returned should be 3.");

            string[] fileNameList = null;
            int[] sizeList = null;
            if (ascend)
            {
                fileNameList = new string[] { "test27.bin", "test1.bin", "test132.bin" };
                sizeList = new int[] { test27Size, test1Size, test132Size };
            }
            else
            {
                fileNameList = new string[] { "test132.bin", "test1.bin", "test27.bin" };
                sizeList = new int[] { test132Size, test1Size, test27Size };
            }

            for (int i = 0; i < 3; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, "The index {0} file in {1} order should be {2}.", i, ascend ? "Ascend" : "Descend", fileNameList[i]);
                Site.Assert.AreEqual(sizeList[i], Convert.ToInt32(getRowsOut.Rows[i].Columns[1].Data), "The size of {0} should be {1} bytes.", fileNameList[i], sizeList[i]);
            }
        }

        private void CPMCreateQuery_SortByAuthorsWithLCID(SortingOrder order)
        {
            argumentType = ArgumentType.AllValid;
            var sortingLCID = GetLCIDValueBySortingOrder(order);
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(2);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("*.doc", Site.Properties.Get("QueryPath") + "Data/CreateQuery_Locale", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Author,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            CInGroupSortAggregSets inGroupSortAggregSets = new CInGroupSortAggregSets();
            inGroupSortAggregSets.cCount = 1;
            inGroupSortAggregSets.SortSets = new CSortSet[1];
            inGroupSortAggregSets.SortSets[0].count = 1;
            inGroupSortAggregSets.SortSets[0].sortArray = new CSort[1];
            inGroupSortAggregSets.SortSets[0].sortArray[0].dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND;
            inGroupSortAggregSets.SortSets[0].sortArray[0].pidColumn = 1; // Sort by author.
            inGroupSortAggregSets.SortSets[0].sortArray[0].locale = sortingLCID; // Sort Chinese textual values by phonetic order (2052), stroke count order (133124) or other orders.

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, inGroupSortAggregSets, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Author, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(6U, getRowsOut._cRowsReturned, "Number of rows returned should be 6.");

            string[] fileNameList = null;
            string[] authorList = null;
            if (order is SortingOrder.PhoneticOrder)
            {
                fileNameList = new string[] { "locale6.doc", "locale5.doc", "locale2.doc", "locale4.doc", "locale1.doc", "locale3.doc" };
                authorList = new string[]
                {
                    "ABC",
                    "东南西",
                    "甲乙丙",
                    "水火木",
                    "一二三",
                    "子丑寅"
                };
            }
            else if (order is SortingOrder.StrokeCountOrder)
            {
                fileNameList = new string[] { "locale6.doc", "locale1.doc", "locale3.doc", "locale4.doc", "locale5.doc", "locale2.doc" };
                authorList = new string[]
                {
                    "ABC",
                    "一二三",
                    "子丑寅",
                    "水火木",
                    "东南西",
                    "甲乙丙"
                };
            }

            for (int i = 0; i < 6; i++)
            {
                Site.Assert.AreEqual(fileNameList[i], getRowsOut.Rows[i].Columns[0].Data, $"The index {i} file in Ascend order should be {fileNameList[i]}.");
                Site.Assert.AreEqual(authorList[i], (getRowsOut.Rows[i].Columns[1].Data as string[])[0], $"The author of {fileNameList[i]} should be {authorList[i]}.");
            }
        }

        private void CPMCreateQueryOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMCreateQueryOut should return SUCCESS if everything is valid.");
                    break;
                case ArgumentType.ColumnSetAbsent:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "CPMCreateQueryOut should return ERROR_INVALID_PARAMETER if no ColumnSet is sent in CPMCreateQueryIn.");
                    break;
                case ArgumentType.InvalidColumnSet:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "CPMCreateQueryOut should return ERROR_INVALID_PARAMETER if invalid ColumnSet is sent in CPMCreateQueryIn.");
                    break;
                case ArgumentType.RestrictionArrayAbsent:
                    Site.Assert.AreEqual((uint)WspErrorCode.QRY_E_INVALIDSCOPES, errorCode, "CPMCreateQueryOut should return QRY_E_INVALIDSCOPES if no RestrictionArray is sent in CPMCreateQueryIn.");
                    break;
                case ArgumentType.PidMapperAbsent:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "CPMCreateQueryOut should return ERROR_INVALID_PARAMETER if no PidMapper is sent in CPMCreateQueryIn.");
                    break;
                case ArgumentType.InvalidSearchScope:
                    Site.Assert.AreEqual((uint)WspErrorCode.QRY_E_INVALIDSCOPES, errorCode, "CPMCreateQueryOut should return QRY_E_INVALIDSCOPES if the search scope is invalid.");
                    break;
            }
        }

        private void CPMSetBindingsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.EmptyColumnSet:
                    Site.Assert.AreEqual((uint)WspErrorCode.DB_E_BADCOLUMNID, errorCode, "CPMSetBindingsIn should return DB_E_BADCOLUMNID if ColumnSet in previous CPMCreateQueryIn is empty.");
                    break;
                case ArgumentType.MismatchedColumnSet:
                    Site.Assert.AreEqual((uint)WspErrorCode.DB_E_BADCOLUMNID, errorCode, "CPMSetBindingsIn should return DB_E_BADCOLUMNID if columns requested in CPMSetBindingsIn do not match the ColumnSet in previous CPMCreateQueryIn.");
                    break;
                default:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMSetBindingsIn should succeed.");
                    break;
            }
        }

        private void CPMGetRowsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    bool succeed = errorCode == (uint)WspErrorCode.SUCCESS || errorCode == (uint)WspErrorCode.DB_S_ENDOFROWSET ? true : false;
                    Site.Assert.IsTrue(succeed, "Server should return SUCCESS or DB_S_ENDOFROWSET for CPMGetRowsIn, actual status is {0}", errorCode);
                    break;
                case ArgumentType.AlternativeCMaxResultsValue:
                    Site.Assert.AreEqual((uint)WspErrorCode.DB_S_ENDOFROWSET, errorCode, "Server should return DB_S_ENDOFROWSET for CPMGetRowsIn if _cMaxResults is set and the complete rowset can be retrieved by a single CPMGetRowsIn call.");
                    break;
            }
        }

        private uint GetLCIDValueBySortingOrder(SortingOrder order)
        {
            switch (order)
            {
                case SortingOrder.PhoneticOrder:
                    return 2052U; // Phonetic order (2052) 
                case SortingOrder.StrokeCountOrder:
                    return 133124U; // Stroke count order (133124)
                default:
                    return 1033U;
            }
        }

        /// <summary>
        /// Construct a whereID:
        /// Another WSP client sends CPMCreateQuery and returns whereID by CPMGetQueryStatusEx.
        /// </summary>
        /// <returns>The whereID of the query.</returns>
        private uint GetWhereID()
        {
            prepareAdapter = new WspAdapter();
            prepareAdapter.Initialize(this.Site);
            prepareAdapter.CPMConnectOutResponse += EnsureSuccessfulCPMConnectOut;
            prepareAdapter.CPMGetQueryStatusExOutResponse += EnsureSuccessfulCPMGetQueryStatusExOut;
            prepareAdapter.CPMCreateQueryOutResponse += CPMCreateQueryOut;
            Site.Log.Add(LogEntryKind.TestStep, "A second Client sends CPMConnectIn and expects success.");
            prepareAdapter.CPMConnectIn();

            var columnSet = prepareAdapter.Builder.GetColumnSet(2);
            var restrictionArray = prepareAdapter.Builder.GetRestrictionArray("Adapter", Site.Properties.Get("QueryPath"), WspConsts.System_Search_Contents);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_ItemFolderNameDisplay,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "The second Client sends CPMCreateQueryIn and expects success.");
            prepareAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), prepareAdapter.Builder.Parameter.LcidValue);

            Site.Log.Add(LogEntryKind.TestStep, "The second Client sends CPMGetQueryStatusExIn and expects success.");
            prepareAdapter.CPMGetQueryStatusExIn(out CPMGetQueryStatusExOut response);
            return response._whereID;
        }
    }
}
