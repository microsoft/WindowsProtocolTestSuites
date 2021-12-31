// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    public partial class CPMCreateQueryTestCases : WspCommonTestBase
    {
        #region Test Cases

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if rows with a correct count can be retrieved with the _cMaxResults in RowSetProperties set to a smaller value than the number of rows in the result.")]
        public void CPMCreateQuery_RowsetProperties_SmallerCMaxResults()
        {
            CPMCreateQuery_RowsetProperties_cMaxResults(_cMaxResults: 1U);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if rows with a correct count can be retrieved with the _cMaxResults in RowSetProperties set to a larger value than the number of rows in the result.")]
        public void CPMCreateQuery_RowsetProperties_LargerCMaxResults()
        {
            CPMCreateQuery_RowsetProperties_cMaxResults(_cMaxResults: 5U);
        }

        #endregion

        private void CPMCreateQuery_RowsetProperties_cMaxResults(uint _cMaxResults)
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

            var rowsetProperties = new CRowsetProperties();
            rowsetProperties._cMaxResults = _cMaxResults; // Use an alternative _cMaxResults value rather than the deault value.

            Site.Log.Add(LogEntryKind.TestStep, $"Client sends CPMCreateQueryIn with _cMaxResults set to {_cMaxResults} and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, rowsetProperties, pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Size, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AlternativeCMaxResultsValue;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            var expectedRowsCount = _cMaxResults <= 3 ? _cMaxResults : 3; // Only 3 files are located in the search scope.
            Site.Assert.AreEqual(expectedRowsCount, getRowsOut._cRowsReturned, $"Number of rows returned should be {expectedRowsCount}.");
        }
    }
}

