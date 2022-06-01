// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMFreeCursorTestCases : WspCommonTestBase
    {
        private ArgumentType argumentType;

        public enum ArgumentType
        {
            AllValid,
            ClientNotConnected,
            InvalidCursor,
            CursorFreed,
            CursorDoubleFreed
        }

        #region Test Class Initialize and Cleanup

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

        #endregion Test Class Initialize and Cleanup

        #region Test Case Initialize and Cleanup

        protected override void TestInitialize()
        {
            base.TestInitialize();

            wspAdapter.CPMSetBindingsInResponse -= EnsureSuccessfulCPMSetBindingsOut;
            wspAdapter.CPMSetBindingsInResponse += CPMSetBindingsOut;

            wspAdapter.CPMGetRowsOutResponse -= EnsureSuccessfulCPMGetRowsOut;
            wspAdapter.CPMGetRowsOutResponse += CPMGetRowsOut;

            wspAdapter.CPMFreeCursorOutResponse -= EnsureSuccessfulCPMFreeCursorOut;
            wspAdapter.CPMFreeCursorOutResponse += CPMFreeCursorOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }

        #endregion Test Case Initialize and Cleanup

        #region Test Cases

        [TestMethod]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn is sent when the client is not connected.")]
        public void CPMFreeCursor_NotConnected()
        {
            argumentType = ArgumentType.ClientNotConnected;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMFreeCursorIn(true);
        }

        [TestMethod]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn is sent after CPMConnectIn.")]
        public void CPMFreeCursor_AfterConnect()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            argumentType = ArgumentType.InvalidCursor;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMFreeCursorIn(true);
        }

        [TestMethod]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn is sent after CPMCreateQueryIn.")]
        public void CPMFreeCursor_AfterQuery()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects success.");
            wspAdapter.CPMFreeCursorIn(true, out var freeCursorOut);

            Site.Assert.AreEqual(0U, freeCursorOut._cCursorsRemaining, "The count of remaining cursors should be 0 since all cursors associated to the query are freed.");

            argumentType = ArgumentType.CursorFreed;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMSetBindingsIn(true, true);
        }

        [TestMethod]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn with an invalid cursor is sent after CPMCreateQueryIn.")]
        public void CPMFreeCursor_InvalidCursor()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            argumentType = ArgumentType.InvalidCursor;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMFreeCursorIn(false);
        }

        [TestMethod]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn is sent after CPMSetBindingsIn.")]
        public void CPMFreeCursor_AfterBinding()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects success.");
            wspAdapter.CPMFreeCursorIn(true, out var freeCursorOut);

            Site.Assert.AreEqual(0U, freeCursorOut._cCursorsRemaining, "The count of remaining cursors should be 0 since all cursors associated to the query are freed.");

            argumentType = ArgumentType.CursorFreed;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects failure.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn is sent after CPMGetRowsIn.")]
        public void BVT_CPMFreeCursor_AfterGetRows()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects success.");
            wspAdapter.CPMFreeCursorIn(true, out var freeCursorOut);

            Site.Assert.AreEqual(0U, freeCursorOut._cCursorsRemaining, "The count of remaining cursors should be 0 since all cursors associated to the query are freed.");

            argumentType = ArgumentType.CursorFreed;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn is sent after CPMFreeCursorIn.")]
        public void CPMFreeCursor_AfterFreeCursor()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects success.");
            wspAdapter.CPMFreeCursorIn(true, out var freeCursorOut);

            Site.Assert.AreEqual(0U, freeCursorOut._cCursorsRemaining, "The count of remaining cursors should be 0 since all cursors associated to the query are freed.");

            argumentType = ArgumentType.CursorDoubleFreed;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects STATUS_INVALID_PARAMETER.");
            wspAdapter.CPMFreeCursorIn(true);
        }

        [TestMethod]
        [TestCategory("CPMFreeCursor")]
        [Description("This test case is designed to test the server response if CPMFreeCursorIn is sent when multiple cursors exist after CPMCreateQueryIn.")]
        public void CPMFreeCursor_MultipleCursors()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var searchScope = BaseTestSite.Properties.Get("QueryPath");
            var searchScopeRetriction = wspAdapter.Builder.GetPropertyRestriction(
                    CPropertyRestriction_relop_Values.PREQ,
                    WspConsts.System_Search_Scope,
                    wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(searchScope)));
            var restrictionArray = new CRestrictionArray { count = 1, isPresent = 1, Restriction = searchScopeRetriction };

            var columnSet = new CColumnSet { count = 2, indexes = new uint[] { 0, 1 } };

            var pidMapper = new CPidMapper { count = 2, aPropSpec = new CFullPropSpec[] { WspConsts.System_Size, WspConsts.System_ItemName } };

            var sortSets = new CInGroupSortAggregSets
            {
                cCount = 1,
                SortSets = new CSortSet[]
                {
                    new CSortSet
                    {
                        count = 1,
                        sortArray = new CSort[]
                        {
                            new CSort
                            {
                                dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND,
                                dwIndividual = CSort_dwIndividual_Values.QUERY_SORTALL,
                                pidColumn = 0,
                                locale = wspAdapter.Builder.Parameter.LcidValue
                            }
                        }
                    }
                }
            };

            var categorizationSet = new CCategorizationSet
            {
                count = 1,
                categories = new CCategorizationSpec[]
                {
                    new CCategorizationSpec
                    {
                        _csColumns = new CColumnSet { count = 0, indexes = new uint[0] },
                        _Spec = new CCategSpec
                        {
                            _ulCategType = CCategSpec_ulCategType_Values.CATEGORIZE_UNIQUE,
                            _sortKey = new CSort
                            {
                                dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND,
                                dwIndividual = CSort_dwIndividual_Values.QUERY_SORTALL,
                                pidColumn = 0,
                                locale = wspAdapter.Builder.Parameter.LcidValue
                            }
                        },
                        _AggregSet = new CAggregSet { cCount = 0, AggregSpecs = new CAggregSpec[0] },
                        _SortAggregSet = new CSortAggregSet { cCount = 0, SortKeys = new CAggregSortKey[0] },
                        _InGroupSortAggregSets = new CInGroupSortAggregSets { cCount = 0, Reserved = 0, SortSets = new CSortSet[0] }
                    }
                }
            };

            Site.Log.Add(LogEntryKind.TestStep, $"Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(
                columnSet,
                restrictionArray,
                sortSets,
                categorizationSet,
                new CRowsetProperties(),
                pidMapper,
                new CColumnGroupArray(),
                wspAdapter.Builder.Parameter.LcidValue,
                out var createQueryOut);

            Site.Assert.AreEqual(2, createQueryOut.aCursors.Length, "The count of hierarchical cursors of the current query should be 2.");

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects success.");
            wspAdapter.CPMFreeCursorIn(createQueryOut.aCursors[0], out var freeCursorOut);

            Site.Assert.AreEqual(1U, freeCursorOut._cCursorsRemaining, "The count of remaining cursors should be 1 since 1 of the 2 hierarchical cursors are freed.");

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFreeCursorIn and expects success.");
            wspAdapter.CPMFreeCursorIn(createQueryOut.aCursors[1], out freeCursorOut);

            Site.Assert.AreEqual(0U, freeCursorOut._cCursorsRemaining, "The count of remaining cursors should be 0 since all cursors associated to the query are freed.");
        }
        #endregion Test Cases

        private void CPMSetBindingsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMSetBindingsOut should return SUCCESS if everything is valid.");
                    break;

                case ArgumentType.CursorFreed:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMSetBindingsOut should fail if the cursor is freed.");
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

                case ArgumentType.CursorFreed:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "CPMGetRowsOut should return STATUS_INVALID_PARAMETER if the cursor is freed.");
                    break;
            }
        }

        private void CPMFreeCursorOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMFreeCursorOut should return SUCCESS if everything is valid.");
                    break;

                case ArgumentType.ClientNotConnected:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "CPMFreeCursorOut should return STATUS_INVALID_PARAMETER if the handle is not present.");
                    break;

                case ArgumentType.InvalidCursor:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "CPMFreeCursorOut should return STATUS_INVALID_PARAMETER if the cursor is invalid.");
                    break;

                case ArgumentType.CursorDoubleFreed:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "CPMFreeCursorOut should return STATUS_INVALID_PARAMETER if the cursor is already freed.");
                    break;
            }
        }
    }
}