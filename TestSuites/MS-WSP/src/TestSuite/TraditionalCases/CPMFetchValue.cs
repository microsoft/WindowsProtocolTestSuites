// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMFetchValueTestCases : WspCommonTestBase
    {
        private ArgumentType argumentType;

        private ulong documentSize;

        public enum ArgumentType
        {
            AllValid,
            NotEnoughChunkSize
        }

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

            wspAdapter.CPMFetchValueOutResponse += CPMFetchValueOut;

            documentSize = ulong.Parse(Site.Properties.Get("DocumentSize"));
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the basic functionality of CPMFetchValueIn.")]
        public void BVT_CPMFetchValue_DeferredValue()
        {
            var wids = GetWids(WspConsts.System_Search_Autosummary, isDeferredValue: true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFetchValueIn and expects success.");
            wspAdapter.CPMFetchValueIn((uint)wids[0], WspConsts.System_Search_Autosummary, out var propValue);

            Site.Assert.AreEqual(CBaseStorageVariant_vType_Values.VT_LPWSTR, (CBaseStorageVariant_vType_Values)propValue.Value.dwType, "The type of System.Search.Autosummary should be VT_LPWSTR.");
            Site.Assert.IsTrue(propValue.Value.rgb is VT_LPWSTR, "The value of System.Search.Autosummary should be in VT_LPWSTR form.");
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server response when an invalid wid is sent in CPMFetchValueIn.")]
        public void CPMFetchValue_InvalidWid()
        {
            var wids = GetWids(WspConsts.System_Search_Autosummary);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFetchValueIn with an invalid wid and expects success.");
            wspAdapter.CPMFetchValueIn((uint)(wids[0] | 0x4000), WspConsts.System_Search_Autosummary, out var propValue);

            Site.Assert.IsNull(propValue, "The value of System.Search.Autosummary should be null since the wid sent in CPMFetchValueIn is invalid.");
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server response when the _cbChunk sent in CPMFetchValueIn is not enough to be filled by a portion of the property to be fetched.")]
        public void CPMFetchValue_NotEnoughChunkSize()
        {
            var wids = GetWids(WspConsts.System_Search_Autosummary);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFetchValueIn with a _cbChunk value that is not enough to be filled by a portion of the property to be fetched and expects failure.");
            argumentType = ArgumentType.NotEnoughChunkSize;
            wspAdapter.CPMFetchValueIn((uint)wids[0], 0, 24, WspConsts.System_Search_Autosummary, out _);
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_UI8 property is fetched by a single CPMFetchValueIn message.")]
        public void CPMFetchValue_SingleChunk_VT_UI8()
        {
            CPMFetchValue_Positive(WspConsts.System_Size, CBaseStorageVariant_vType_Values.VT_UI8, documentSize);
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_FILETIME property is fetched by a single CPMFetchValueIn message.")]
        public void CPMFetchValue_SingleChunk_VT_FILETIME()
        {
            CPMFetchValue_Positive(WspConsts.System_DateCreated, CBaseStorageVariant_vType_Values.VT_FILETIME, null);
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_LPWSTR property is fetched by a single CPMFetchValueIn message.")]
        public void CPMFetchValue_SingleChunk_VT_LPWSTR()
        {
            CPMFetchValue_Positive(WspConsts.System_Title, CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR { cLen = 20, _string = "Very Important Text" });
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_VECTOR of VT_LPWSTR property is fetched by a single CPMFetchValueIn message.")]
        public void CPMFetchValue_SingleChunk_VT_VECTOR_Of_VT_LPWSTR()
        {
            CPMFetchValue_Positive(
                WspConsts.System_Author,
                CBaseStorageVariant_vType_Values.VT_LPWSTR | CBaseStorageVariant_vType_Values.VT_VECTOR,
                new VT_VECTOR<VT_LPWSTR>
                {
                    vVectorElements = 4,
                    vVectorData = new VT_LPWSTR[]
                    {
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "AAA"
                        },
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "BBB"
                        },
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "CCC"
                        },
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "DDD"
                        }
                    }
                });
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_UI8 property is fetched by consequent CPMFetchValueIn messages.")]
        public void CPMFetchValue_MultipleChunks_VT_UI8()
        {
            CPMFetchValue_Positive(WspConsts.System_Size, CBaseStorageVariant_vType_Values.VT_UI8, documentSize, false);
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_FILETIME property is fetched by consequent CPMFetchValueIn messages.")]
        public void CPMFetchValue_MultipleChunks_VT_FILETIME()
        {
            CPMFetchValue_Positive(WspConsts.System_DateCreated, CBaseStorageVariant_vType_Values.VT_FILETIME, null, false);
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_LPWSTR property is fetched by consequent CPMFetchValueIn messages.")]
        public void CPMFetchValue_MultipleChunks_VT_LPWSTR()
        {
            CPMFetchValue_Positive(WspConsts.System_Title, CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR { cLen = 20, _string = "Very Important Text" }, false);
        }

        [TestMethod]
        [TestCategory("CPMFetchValue")]
        [Description("This test case is designed to test the server behavior when a VT_VECTOR of VT_LPWSTR property is fetched by consequent CPMFetchValueIn messages.")]
        public void CPMFetchValue_MultipleChunks_VT_VECTOR_Of_VT_LPWSTR()
        {
            CPMFetchValue_Positive(
                WspConsts.System_Author,
                CBaseStorageVariant_vType_Values.VT_LPWSTR | CBaseStorageVariant_vType_Values.VT_VECTOR,
                new VT_VECTOR<VT_LPWSTR>
                {
                    vVectorElements = 4,
                    vVectorData = new VT_LPWSTR[]
                    {
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "AAA"
                        },
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "BBB"
                        },
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "CCC"
                        },
                        new VT_LPWSTR
                        {
                            cLen = 4,
                            _string = "DDD"
                        }
                    }
                },
                false);
        }

        #endregion

        private void CPMFetchValue_Positive(CFullPropSpec propToFetch, CBaseStorageVariant_vType_Values vType, object expectedResult, bool useSingleMessage = true)
        {
            var wids = GetWids(propToFetch);
            var widToFetch = (uint)wids[0];
            var chunkSize = useSingleMessage ? 0x4000U : 32U;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMFetchValueIn and expects success.");
            wspAdapter.CPMFetchValueIn(widToFetch, chunkSize, propToFetch, out var propValue);

            switch (vType)
            {
                case CBaseStorageVariant_vType_Values.VT_UI8:
                    Site.Assert.AreEqual((ulong)expectedResult, (ulong)propValue.Value.rgb, $"The property value of type VT_UI8 should be {expectedResult}.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_FILETIME:
                    Site.Assert.IsTrue(propValue.Value.rgb is DateTime, "The property value of type VT_FILETIME should be in DateTime form.");
                    break;

                case CBaseStorageVariant_vType_Values.VT_LPWSTR:
                    var expectedLpwstr = (VT_LPWSTR)expectedResult;
                    var actualPropValue = (VT_LPWSTR)propValue.Value.rgb;
                    Site.Log.Add(LogEntryKind.TestStep, "Verification for the property value of type VT_LPWSTR:");
                    Site.Assert.AreEqual(expectedLpwstr.cLen, actualPropValue.cLen, $"The cLen field should be {expectedLpwstr.cLen}");
                    Site.Assert.AreEqual(expectedLpwstr._string, actualPropValue._string, $"The _string field should be {expectedLpwstr._string}");
                    break;

                case CBaseStorageVariant_vType_Values.VT_LPWSTR | CBaseStorageVariant_vType_Values.VT_VECTOR:
                    var expectedLpwstrVector = (VT_VECTOR<VT_LPWSTR>)expectedResult;
                    var actualLpwstrVector = (VT_VECTOR<VT_LPWSTR>)propValue.Value.rgb;
                    Site.Log.Add(LogEntryKind.TestStep, "Verification for the property value of type VT_LPWSTR | VT_VECTOR:");
                    Site.Assert.AreEqual(expectedLpwstrVector.vVectorElements, actualLpwstrVector.vVectorElements, $"The vVectorElements field should be {expectedLpwstrVector.vVectorElements}");
                    for (var idx = 0; idx < expectedLpwstrVector.vVectorElements; idx++)
                    {
                        Site.Assert.AreEqual(expectedLpwstrVector.vVectorData[idx].cLen, actualLpwstrVector.vVectorData[idx].cLen, $"The cLen field of the Index {idx} element should be {expectedLpwstrVector.vVectorData[idx].cLen}");
                        Site.Assert.AreEqual(expectedLpwstrVector.vVectorData[idx]._string, actualLpwstrVector.vVectorData[idx]._string, $"The _string field of the Index {idx} element should be {expectedLpwstrVector.vVectorData[idx]._string}");
                    }
                    break;

                default:
                    throw new NotImplementedException($"The test case for {vType} is not implemented.");

            }
        }

        private List<int> GetWids(CFullPropSpec propToFetch, bool isDeferredValue = false)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = wspAdapter.Builder.GetColumnSet(3);
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray("document.doc", Site.Properties.Get("QueryPath") + "Data/FetchValue", WspConsts.System_FileName);
            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                WspConsts.System_Search_EntryID,
                propToFetch
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, null, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_Search_EntryID, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(propToFetch, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(out var getRowsOut);

            Site.Assert.IsTrue(getRowsOut._cRowsReturned > 0, "There should be rows returned for the current query.");

            if (isDeferredValue)
            {
                foreach (var row in getRowsOut.Rows)
                {
                    Site.Assert.AreEqual(StoreStatus.StoreStatusDeferred, row.Columns[2].Status.Value, "The server should set the status of the property to StoreStatusDeferred for the row.");
                }
            }

            var wids = new List<int>();
            foreach (var row in getRowsOut.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is int, "The System.Search.EntryID property should be an int value.");
                wids.Add((int)row.Columns[1].Data);
            }

            return wids;
        }

        private void CPMFetchValueOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMFetchValueOut should return SUCCESS if everything is valid.");
                    break;

                case ArgumentType.NotEnoughChunkSize:
                    Site.Assert.AreNotEqual((uint)WspErrorCode.SUCCESS, errorCode, "CPMFetchValueOut should fail if a _cbChunk value that is not enough to be filled by a portion of the property to be fetched is sent in CPMFetchValueIn.");
                    break;
            }
        }
    }
}
