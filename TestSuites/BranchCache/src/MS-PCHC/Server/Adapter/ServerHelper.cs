// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;

    /// <summary>
    /// The Helper to convert type between adapter and stack
    /// </summary>
    public static class ServerHelper
    {
        /// <summary>
        /// Convert the responsemessage struct defined in adapter to stack
        /// </summary>
        /// <param name="responseMessageStack">The response message of send initialInfoMeaage or segmentInfoMessage</param>
        /// <returns>Return the ResponseMessage type defined in adapter</returns>
        public static ResponseMessage ConvertFromStackForResponseMsg(RESPONSE_MESSAGE responseMessageStack)
        {
            ResponseMessage responseMessage;
            responseMessage.ResponseCode = ConvertFromStackForResponseCode(responseMessageStack.ResponseCode);
            responseMessage.TransportHeader = ConvertFromStackForTransHeader(responseMessageStack.TransportHeader);

            return responseMessage;
        }

        /// <summary>
        /// Convert the responsemessage struct defined in adapter to stack
        /// </summary>
        /// <param name="responseMessage">The response message of send initialInfoMeaage or segmentInfoMessage</param>
        /// <returns>Return the ResponseMessage type defined in stack</returns>
        public static RESPONSE_MESSAGE ConvertToStackForResponseMsg(ResponseMessage responseMessage)
        {
            RESPONSE_MESSAGE responseMessageStack;
            responseMessageStack.ResponseCode = ConvertToStackForResponseCode(responseMessage.ResponseCode);
            responseMessageStack.TransportHeader = ConvertToStackForTransHeader(responseMessage.TransportHeader);

            return responseMessageStack;
        }

        /// <summary>
        /// Convert the ContentInformaiton struct defined in stack to adapter
        /// </summary>
        /// <param name="contentInfoStack">The contentInformation  defined in pccrc</param>
        /// <returns>Return the ContentInformaiton type defined in adapter</returns>
        public static SegmentInformation ConvertFromstackForContentInfo(Content_Information_Data_Structure contentInfoStack)
        {
            SegmentInformation contentInfo;
            contentInfo.DwHashAlgo = ConvertFromStackFordwHash(contentInfoStack.dwHashAlgo);
            contentInfo.DwOffsetInFirstSegment = contentInfoStack.dwOffsetInFirstSegment;
            contentInfo.DwReadBytesInLastSegment = contentInfoStack.dwReadBytesInLastSegment;
            contentInfo.Version = contentInfoStack.Version;
            contentInfo.CSegments = contentInfoStack.cSegments;
            contentInfo.Blocks = ConvertFromStackForSegBlocks(contentInfoStack.blocks);
            contentInfo.Segments = ConvertFromStackForSegDescription(contentInfoStack.segments);

            return contentInfo;
        }

        /// <summary>
        /// Convert the ContentInformaiton struct defined in adapter to stack
        /// </summary>
        /// <param name="contentInfo">The contentInformation  defined in pccrc</param>
        /// <returns>Return the ContentInformaiton type defined in stack</returns>
        public static TestTools.StackSdk.BranchCache.Pccrc.Content_Information_Data_Structure ConvertTostackForContentInfo(
            TestSuites.Pchc.SegmentInformation contentInfo)
        {
            TestTools.StackSdk.BranchCache.Pccrc.Content_Information_Data_Structure contentInfoStack;
            contentInfoStack.dwHashAlgo = ConvertToStackFordwHash(contentInfo.DwHashAlgo);
            contentInfoStack.dwOffsetInFirstSegment = contentInfo.DwOffsetInFirstSegment;
            contentInfoStack.dwReadBytesInLastSegment = contentInfo.DwReadBytesInLastSegment;
            contentInfoStack.Version = contentInfo.Version;
            contentInfoStack.cSegments = contentInfo.CSegments;
            contentInfoStack.blocks = ConvertToStackForSegBlocks(contentInfo.Blocks);
            contentInfoStack.segments = ConvertToStackForSegDescription(contentInfo.Segments);

            return contentInfoStack;
        }

        /// <summary>
        /// Convert the responsecode struct defined in stack to adapter
        /// </summary>
        /// <param name="responseCodeStack">ResponseCode values.</param>
        /// <returns>Return the ResponseCode type defined in adapter</returns>
        private static ResponseCode ConvertFromStackForResponseCode(RESPONSE_CODE responseCodeStack)
        {
            ResponseCode responseCode;
            responseCode = (ResponseCode)responseCodeStack;

            return responseCode;
        }

        /// <summary>
        /// Convert the responsecode struct defined in adapter to stack
        /// </summary>
        /// <param name="responseCode">ResponseCode values.</param>
        /// <returns>Return the RESPONSE_CODE type defined in stack</returns>
        private static RESPONSE_CODE ConvertToStackForResponseCode(ResponseCode responseCode)
        {
            RESPONSE_CODE responseCodeStack;
            responseCodeStack = (RESPONSE_CODE)responseCode;

            return responseCodeStack;
        }

        /// <summary>
        /// Convert the dwHashAlgo_Values struct defined in stack to adapter
        /// </summary>
        /// <param name="stackDwHashValue">The value indicate which hash algorithm to use.</param>
        /// <returns>Return the dwHashAlgo_Values type defined in adapter</returns>
        private static DwHashAlgo_Values ConvertFromStackFordwHash(
            TestTools.StackSdk.BranchCache.Pccrc.dwHashAlgo_Values stackDwHashValue)
        {
            DwHashAlgo_Values adapterdwHashValue;
            adapterdwHashValue = (DwHashAlgo_Values)stackDwHashValue;

            return adapterdwHashValue;
        }

        /// <summary>
        /// Convert the dwHashAlgo_Values struct defined in adapter to stack
        /// </summary>
        /// <param name="adapterdwHashValue">The value indicate which hash algorithm to use.</param>
        /// <returns>Return the dwHashAlgo_Values type defined in stack</returns>
        private static TestTools.StackSdk.BranchCache.Pccrc.dwHashAlgo_Values ConvertToStackFordwHash(
            DwHashAlgo_Values adapterdwHashValue)
        {
            TestTools.StackSdk.BranchCache.Pccrc.dwHashAlgo_Values stackdwHashValue;
            stackdwHashValue = (TestTools.StackSdk.BranchCache.Pccrc.dwHashAlgo_Values)adapterdwHashValue;

            return stackdwHashValue;
        }

        /// <summary>
        /// Convert the SegmentContentBlocks struct defined in stack to adapter
        /// </summary>
        /// <param name="segmentBlocksStack">The blocks field contains a number cSegments of SegmentContentBlocks fields. 
        /// The Nth SegmentContentBlocks field corresponds to the Nth SegmentDescription and hence the Nth content segment.
        /// </param>
        /// <returns>Return the SegmentContentBlocks type defined in adapter</returns>
        private static SegmentContentBlocks[] ConvertFromStackForSegBlocks(
            TestTools.StackSdk.BranchCache.Pccrc.SegmentContentBlocks[] segmentBlocksStack)
        {
            SegmentContentBlocks[] segmentBlocks = new SegmentContentBlocks[segmentBlocksStack.Length];
            for (int i = 0; i < segmentBlocksStack.Length; i++)
            {
                segmentBlocks[i].BlockHashes = segmentBlocksStack[i].BlockHashes;
                segmentBlocks[i].Cblocks = segmentBlocksStack[i].cBlocks;
            }

            return segmentBlocks;
        }

        /// <summary>
        /// Convert the SegmentContentBlocks struct defined in adapter to stack
        /// </summary>
        /// <param name="segmentBlocks">The blocks field contains a number cSegments of SegmentContentBlocks fields. 
        /// The Nth SegmentContentBlocks field corresponds to the Nth SegmentDescription and hence the Nth content segment.
        /// </param>
        /// <returns>Return the SegmentContentBlocks type defined in stack</returns>
        private static TestTools.StackSdk.BranchCache.Pccrc.SegmentContentBlocks[] ConvertToStackForSegBlocks(
            SegmentContentBlocks[] segmentBlocks)
        {
            TestTools.StackSdk.BranchCache.Pccrc.SegmentContentBlocks[] segmentBlocksStack
                = new TestTools.StackSdk.BranchCache.Pccrc.SegmentContentBlocks[segmentBlocks.Length];
            for (int i = 0; i < segmentBlocks.Length; i++)
            {
                segmentBlocksStack[i].BlockHashes = segmentBlocks[i].BlockHashes;
                segmentBlocksStack[i].cBlocks = segmentBlocks[i].Cblocks;
            }

            return segmentBlocksStack;
        }

        /// <summary>
        /// Convert the SegmentDescription struct defined in stack to adapter 
        /// </summary>
        /// <param name="segmentDescriptionStack">The segments field is composed of a number cSegments
        ///  of SegmentDescription fields. Each SegmentDescription field corresponds to a content segment
        ///  in the order in which they appear in the original content.
        ///  </param>
        /// <returns>Return the SegmentDescription type defined in adapter</returns>
        private static SegmentDescription[] ConvertFromStackForSegDescription(
            TestTools.StackSdk.BranchCache.Pccrc.SegmentDescription[] segmentDescriptionStack)
        {
            Microsoft.Protocols.TestSuites.Pchc.SegmentDescription[] segmentDescription
                = new SegmentDescription[segmentDescriptionStack.Length];
            for (int i = 0; i < segmentDescription.Length; i++)
            {
                segmentDescription[i].CbBlockSize = segmentDescriptionStack[i].cbBlockSize;
                segmentDescription[i].CbSegment = segmentDescriptionStack[i].cbSegment;
                segmentDescription[i].SegmentHashOfData = segmentDescriptionStack[i].SegmentHashOfData;
                segmentDescription[i].SegmentSecret = segmentDescriptionStack[i].SegmentSecret;
                segmentDescription[i].UllOffsetInContent = segmentDescriptionStack[i].ullOffsetInContent;
            }

            return segmentDescription;
        }

        /// <summary>
        /// Convert the SegmentDescription struct defined in adapter to stack 
        /// </summary>
        /// <param name="segmentDescription">The segments field is composed of a number cSegments
        ///  of SegmentDescription fields. Each SegmentDescription field corresponds to a content segment
        ///  in the order in which they appear in the original content.
        ///  </param>
        /// <returns>Return the SegmentDescription type defined in stack</returns>
        private static TestTools.StackSdk.BranchCache.Pccrc.SegmentDescription[] ConvertToStackForSegDescription(
            SegmentDescription[] segmentDescription)
        {
            TestTools.StackSdk.BranchCache.Pccrc.SegmentDescription[] segmentDescriptionStack
                = new TestTools.StackSdk.BranchCache.Pccrc.SegmentDescription[segmentDescription.Length];
            for (int i = 0; i < segmentDescriptionStack.Length; i++)
            {
                segmentDescriptionStack[i].cbBlockSize = segmentDescription[i].CbBlockSize;
                segmentDescriptionStack[i].cbSegment = segmentDescription[i].CbSegment;
                segmentDescriptionStack[i].SegmentHashOfData = segmentDescription[i].SegmentHashOfData;
                segmentDescriptionStack[i].SegmentSecret = segmentDescription[i].SegmentSecret;
                segmentDescriptionStack[i].ullOffsetInContent = segmentDescription[i].UllOffsetInContent;
            }

            return segmentDescriptionStack;
        }

        /// <summary>
        /// Convert the transportheader struct defined in stack to adapter
        /// </summary>
        /// <param name="transportHeaderStack">The header of the transpport</param>
        /// <returns>Return the transportheader defined in adapter</returns>
        private static TransportHeader ConvertFromStackForTransHeader(TRANSPORT_HEADER transportHeaderStack)
        {
            TransportHeader transportHeader;
            transportHeader.Size = transportHeaderStack.Size;

            return transportHeader;
        }

        /// <summary>
        /// Convert the transportheader struct defined in adapter to stack
        /// </summary>
        /// <param name="transportHeader">The header of the transpport</param>
        /// <returns>Return the transportheader type defined in stack</returns>
        private static TRANSPORT_HEADER ConvertToStackForTransHeader(TransportHeader transportHeader)
        {
            TRANSPORT_HEADER transportHeaderStack;
            transportHeaderStack.Size = transportHeader.Size;

            return transportHeaderStack;
        }
    }
}
