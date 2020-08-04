// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    class RDPBCGROutput
    {

        public static TS_BITMAP_DATA CreateBitmapData(ushort left, ushort top, ushort width, ushort height)
        {
            Random r = new Random();
            TS_BITMAP_DATA bitmap = new TS_BITMAP_DATA();
            bitmap.destLeft = left;
            bitmap.destTop = top;
            bitmap.destRight = (ushort)(left + width - 1);
            bitmap.destBottom = (ushort)(top + height - 1);

            bitmap.width = width;
            bitmap.height = height;

            bitmap.bitsPerPixel = 16;
            bitmap.Flags = TS_BITMAP_DATA_Flags_Values.NO_BITMAP_COMPRESSION_HDR;

            bitmap.bitmapLength = (ushort)((width * 2 + 3) / 4 * 4 * height);
            bitmap.bitmapDataStream = new byte[bitmap.bitmapLength];

            for (int i = 0; i < bitmap.bitmapLength; i++)
                bitmap.bitmapDataStream[i] = (byte)r.Next(0, 64);

            return bitmap;
        }

        public static TS_FP_UPDATE_PDU CreateFPUpdatePDU(RdpbcgrServerSessionContext context, TS_FP_UPDATE[] updates)
        {
            TS_FP_UPDATE_PDU fpOutput = new TS_FP_UPDATE_PDU(context);

            fpOutput.fpOutputHeader = new nested_TS_FP_UPDATE_PDU_fpOutputHeader();

            fpOutput.dataSignature = null;
            fpOutput.length1 = 0;
            fpOutput.length2 = 0;

            if (updates != null)
            {
                fpOutput.fpOutputUpdates = updates;
            }

            return fpOutput;
        }

        public static TS_FP_UPDATE_PDU CreateFPUpdatePDU(RdpbcgrServerSessionContext context, TS_FP_UPDATE update)
        {
            TS_FP_UPDATE_PDU fpOutput = new TS_FP_UPDATE_PDU(context);

            fpOutput.fpOutputHeader = new nested_TS_FP_UPDATE_PDU_fpOutputHeader();

            fpOutput.dataSignature = null;
            fpOutput.length1 = 0;
            fpOutput.length2 = 0;

            fpOutput.fpOutputUpdates = new TS_FP_UPDATE[1];
            fpOutput.fpOutputUpdates[0] = update;

            return fpOutput;
        }

        public static TS_FP_UPDATE_BITMAP CreateFPUpdateBitmap(TS_BITMAP_DATA data)
        {
            TS_FP_UPDATE_BITMAP bitmap = new TS_FP_UPDATE_BITMAP();

            bitmap.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_BITMAP);

            bitmap.compressionFlags = compressedType_Values.None;
            bitmap.bitmapUpdateData.updateType = (ushort)updateType_Values.UPDATETYPE_BITMAP;
            bitmap.bitmapUpdateData.numberRectangles = 1;
            bitmap.bitmapUpdateData.rectangles = new TS_BITMAP_DATA[1];
            bitmap.bitmapUpdateData.rectangles[0] = data;

            bitmap.AssignUpdateDataAndSize();

            return bitmap;
        }

        public static TS_FP_UPDATE_BITMAP CreateFPUpdateBitmap(ushort left, ushort top, ushort width, ushort height)
        {
            TS_FP_UPDATE_BITMAP bitmap = new TS_FP_UPDATE_BITMAP();
            TS_BITMAP_DATA bdata;

            bitmap.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_BITMAP);

            bitmap.compressionFlags = compressedType_Values.None;
            bitmap.bitmapUpdateData.updateType = (ushort)updateType_Values.UPDATETYPE_BITMAP;
            bitmap.bitmapUpdateData.numberRectangles = 1;
            bitmap.bitmapUpdateData.rectangles = new TS_BITMAP_DATA[1];

            bdata = CreateBitmapData(left, top, width, height);
            bitmap.bitmapUpdateData.rectangles[0] = bdata;

            bitmap.AssignUpdateDataAndSize();

            return bitmap;
        }

        public static TS_FP_POINTERPOSATTRIBUTE CreateFPPointerPosAttribute(int x, int y)
        {
            TS_FP_POINTERPOSATTRIBUTE fpPos = new TS_FP_POINTERPOSATTRIBUTE();

            fpPos.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_PTR_POSITION);

            fpPos.compressionFlags = compressedType_Values.None;
            fpPos.pointerPositionUpdateData.position.xPos = (ushort)x;
            fpPos.pointerPositionUpdateData.position.yPos = (ushort)y;

            fpPos.AssignUpdateDataAndSize();

            return fpPos;
        }

        public static TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE CreateFPSystemPointerHiddenAttribute()
        {
            TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE fpSysPointer = new TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE();

            fpSysPointer.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_PTR_NULL);

            fpSysPointer.AssignUpdateDataAndSize();

            return fpSysPointer;
        }

        public static TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE CreateFPSystemPointerDefaultAttribute()
        {
            TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE fpSysPointer = new TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE();

            fpSysPointer.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_PTR_DEFAULT);

            fpSysPointer.AssignUpdateDataAndSize();

            return fpSysPointer;
        }

        public static TS_FP_POINTERATTRIBUTE CreateFPPointerAttribute(ushort xorBpp, ushort cacheIndex, ushort hotSpotX, ushort hotSpotY, ushort width, ushort height,
            byte[] xorMaskData = null, byte[] andMaskData = null)
        {
            TS_FP_POINTERATTRIBUTE fpPtr = new TS_FP_POINTERATTRIBUTE();

            fpPtr.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_POINTER);

            fpPtr.newPointerUpdateData.xorBpp = xorBpp;
            fpPtr.newPointerUpdateData.colorPtrAttr.cacheIndex = cacheIndex;
            fpPtr.newPointerUpdateData.colorPtrAttr.hotSpot.xPos = hotSpotX;
            fpPtr.newPointerUpdateData.colorPtrAttr.hotSpot.yPos = hotSpotY;
            fpPtr.newPointerUpdateData.colorPtrAttr.width = width;
            fpPtr.newPointerUpdateData.colorPtrAttr.height = height;

            if (xorMaskData != null)
            {
                fpPtr.newPointerUpdateData.colorPtrAttr.lengthXorMask = (ushort)xorMaskData.Length;
                fpPtr.newPointerUpdateData.colorPtrAttr.xorMaskData = xorMaskData;
            }
            else
            {
                fpPtr.newPointerUpdateData.colorPtrAttr.lengthXorMask = (ushort)(((width * xorBpp + 7) / 8 + 1) / 2 * 2 * height);
                fpPtr.newPointerUpdateData.colorPtrAttr.xorMaskData = new byte[fpPtr.newPointerUpdateData.colorPtrAttr.lengthXorMask];
                for (int i = 0; i < fpPtr.newPointerUpdateData.colorPtrAttr.lengthXorMask; i++)
                {
                    if (xorBpp == 32 && i % 4 == 3)
                    {
                        //If color depth is 32, set R, G, B to 0xff, and set A to 0x00
                        fpPtr.newPointerUpdateData.colorPtrAttr.xorMaskData[i] = 0x00;
                    }
                    else
                    {
                        fpPtr.newPointerUpdateData.colorPtrAttr.xorMaskData[i] = 0xff;
                    }
                }
            }

            if (andMaskData != null)
            {
                fpPtr.newPointerUpdateData.colorPtrAttr.lengthAndMask = (ushort)andMaskData.Length;
                fpPtr.newPointerUpdateData.colorPtrAttr.andMaskData = andMaskData;
            }
            else
            {
                fpPtr.newPointerUpdateData.colorPtrAttr.lengthAndMask = (ushort)(((width + 7) / 8 + 1) / 2 * 2 * height);
                fpPtr.newPointerUpdateData.colorPtrAttr.andMaskData = new byte[fpPtr.newPointerUpdateData.colorPtrAttr.lengthAndMask];
                for (int i = 0; i < fpPtr.newPointerUpdateData.colorPtrAttr.lengthAndMask; i++)
                    fpPtr.newPointerUpdateData.colorPtrAttr.andMaskData[i] = 0xff;
            }

            fpPtr.newPointerUpdateData.colorPtrAttr.pad = 0;

            fpPtr.AssignUpdateDataAndSize();

            return fpPtr;
        }

        public static TS_FP_COLORPOINTERATTRIBUTE CreateFPColorPointerAttribute(ushort cacheIndex, ushort hotSpotX, ushort hotSpotY, ushort width, ushort height,
            byte[] xorMaskData = null, byte[] andMaskData = null)
        {
            TS_FP_COLORPOINTERATTRIBUTE fpColorPtr = new TS_FP_COLORPOINTERATTRIBUTE();

            fpColorPtr.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_COLOR);

            fpColorPtr.colorPointerUpdateData.cacheIndex = cacheIndex;
            fpColorPtr.colorPointerUpdateData.hotSpot.xPos = hotSpotX;
            fpColorPtr.colorPointerUpdateData.hotSpot.yPos = hotSpotY;
            fpColorPtr.colorPointerUpdateData.width = width;
            fpColorPtr.colorPointerUpdateData.height = height;

            if (xorMaskData != null)
            {
                fpColorPtr.colorPointerUpdateData.lengthXorMask = (ushort)xorMaskData.Length;
                fpColorPtr.colorPointerUpdateData.xorMaskData = xorMaskData;
            }
            else
            {
                fpColorPtr.colorPointerUpdateData.lengthXorMask = (ushort)((width * 3 + 1) / 2 * 2 * height);
                fpColorPtr.colorPointerUpdateData.xorMaskData = new byte[fpColorPtr.colorPointerUpdateData.lengthXorMask];
                for (int i = 0; i < fpColorPtr.colorPointerUpdateData.lengthXorMask; i++)
                    fpColorPtr.colorPointerUpdateData.xorMaskData[i] = 0xff;
            }

            if (andMaskData != null)
            {
                fpColorPtr.colorPointerUpdateData.lengthAndMask = (ushort)andMaskData.Length;
                fpColorPtr.colorPointerUpdateData.andMaskData = andMaskData;
            }
            else
            {
                fpColorPtr.colorPointerUpdateData.lengthAndMask = (ushort)(((width + 7) / 8 + 1) / 2 * 2 * height);
                fpColorPtr.colorPointerUpdateData.andMaskData = new byte[fpColorPtr.colorPointerUpdateData.lengthAndMask];
                for (int i = 0; i < fpColorPtr.colorPointerUpdateData.lengthAndMask; i++)
                    fpColorPtr.colorPointerUpdateData.andMaskData[i] = 0xff;
            }

            fpColorPtr.colorPointerUpdateData.pad = 0;

            fpColorPtr.AssignUpdateDataAndSize();

            return fpColorPtr;
        }


        public static TS_FP_COLORPOINTERATTRIBUTE CreateFPColorPointerAttribute(int width, int height)
        {
            TS_FP_COLORPOINTERATTRIBUTE fpColorPtr = new TS_FP_COLORPOINTERATTRIBUTE();

            fpColorPtr.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_COLOR);

            fpColorPtr.colorPointerUpdateData.cacheIndex = 0;
            fpColorPtr.colorPointerUpdateData.hotSpot.xPos = 5;
            fpColorPtr.colorPointerUpdateData.hotSpot.yPos = 5;
            fpColorPtr.colorPointerUpdateData.width = (ushort)width;
            fpColorPtr.colorPointerUpdateData.height = (ushort)height;
            fpColorPtr.colorPointerUpdateData.lengthAndMask = (ushort)(((width + 7) / 8 + 1) / 2 * 2 * height);
            fpColorPtr.colorPointerUpdateData.lengthXorMask = (ushort)((width * 3 + 1) / 2 * 2 * height);
            fpColorPtr.colorPointerUpdateData.xorMaskData = new byte[fpColorPtr.colorPointerUpdateData.lengthXorMask];
            for (int i = 0; i < fpColorPtr.colorPointerUpdateData.lengthXorMask; i++)
                fpColorPtr.colorPointerUpdateData.xorMaskData[i] = 0xf1;
            fpColorPtr.colorPointerUpdateData.andMaskData = new byte[fpColorPtr.colorPointerUpdateData.lengthAndMask];
            for (int i = 0; i < fpColorPtr.colorPointerUpdateData.lengthAndMask; i++)
                fpColorPtr.colorPointerUpdateData.andMaskData[i] = 0x1f;
            fpColorPtr.colorPointerUpdateData.pad = 0;

            fpColorPtr.AssignUpdateDataAndSize();

            return fpColorPtr;
        }

        public static TS_FP_CACHEDPOINTERATTRIBUTE CreateCachedPointerAttribute(ushort cacheIndex)
        {
            TS_FP_CACHEDPOINTERATTRIBUTE cachePtr = new TS_FP_CACHEDPOINTERATTRIBUTE();

            cachePtr.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_CACHED);

            cachePtr.cachedPointerUpdateData.cacheIndex = cacheIndex;

            cachePtr.AssignUpdateDataAndSize();

            return cachePtr;
        }
        public static TS_FP_SURFCMDS CreateFPSurfCmds(TS_SURFCMD_SET_SURF_BITS setSurfBits)
        {
            TS_FP_SURFCMDS surfCmds = new TS_FP_SURFCMDS();

            surfCmds.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_SURFCMDS);

            surfCmds.compressionFlags = compressedType_Values.None;
            surfCmds.surfaceCommands = new TS_SURFCMD[1];
            surfCmds.surfaceCommands[0] = setSurfBits;

            surfCmds.AssignUpdateDataAndSize();

            return surfCmds;
        }

        public static TS_SURFCMD_SET_SURF_BITS CreateSurfCmdSetSurfBits(ushort left, ushort top, ushort width, ushort height)
        {
            TS_SURFCMD_SET_SURF_BITS setSurfBits = new TS_SURFCMD_SET_SURF_BITS();

            setSurfBits.cmdType = cmdType_Values.CMDTYPE_SET_SURFACE_BITS;
            setSurfBits.destLeft = left;
            setSurfBits.destTop = top;
            setSurfBits.destRight = (ushort)(setSurfBits.destLeft + width - 1);
            setSurfBits.destBottom = (ushort)(setSurfBits.destTop + height - 1);
            setSurfBits.bitmapData.bpp = 16;
            setSurfBits.bitmapData.codecID = 0;
            setSurfBits.bitmapData.width = width;
            setSurfBits.bitmapData.height = height;
            setSurfBits.bitmapData.bitmapDataLength = (ushort)((width * 2 + 3) / 4 * 4 * height);
            setSurfBits.bitmapData.bitmapData = new byte[setSurfBits.bitmapData.bitmapDataLength];
            for (int i = 0; i < setSurfBits.bitmapData.bitmapDataLength; i++)
                setSurfBits.bitmapData.bitmapData[i] = 0x67;

            return setSurfBits;
        }

        public static TS_FP_LARGEPOINTERATTRIBUTE CreateFPLargePointerAttribute(ushort cacheIndex, ushort hotSpotX, ushort hotSpotY, ushort width, ushort height, byte[] xorMaskData = null, byte[] andMaskData = null)
        {
            var fpLargePointerAttribute = new TS_FP_LARGEPOINTERATTRIBUTE();

            fpLargePointerAttribute.updateHeader = new nested_TS_FP_UPDATE_updateHeader(updateCode_Values.FASTPATH_UPDATETYPE_LARGE_POINTER);

            fpLargePointerAttribute.xorBpp = 24;
            fpLargePointerAttribute.cacheIndex = cacheIndex;
            fpLargePointerAttribute.hotSpot.xPos = hotSpotX;
            fpLargePointerAttribute.hotSpot.yPos = hotSpotY;
            fpLargePointerAttribute.width = width;
            fpLargePointerAttribute.height = height;

            if (xorMaskData != null)
            {
                fpLargePointerAttribute.lengthXorMask = (UInt32)xorMaskData.Length;
                fpLargePointerAttribute.xorMaskData = xorMaskData;
            }
            else
            {
                // If not provided, generate a pointer with all pixels set to white.
                fpLargePointerAttribute.lengthXorMask = (UInt32)((width * 3 + 1) / 2 * 2 * height);
                fpLargePointerAttribute.xorMaskData = new byte[fpLargePointerAttribute.lengthXorMask];
                for (int i = 0; i < fpLargePointerAttribute.lengthXorMask; i++)
                {
                    fpLargePointerAttribute.xorMaskData[i] = 0xff;
                }
            }

            if (andMaskData != null)
            {
                fpLargePointerAttribute.lengthAndMask = (UInt32)andMaskData.Length;
                fpLargePointerAttribute.andMaskData = andMaskData;
            }
            else
            {
                fpLargePointerAttribute.lengthAndMask = (UInt32)(((width + 7) / 8 + 1) / 2 * 2 * height);
                fpLargePointerAttribute.andMaskData = new byte[fpLargePointerAttribute.lengthAndMask];
                for (int i = 0; i < fpLargePointerAttribute.lengthAndMask; i++)
                {
                    fpLargePointerAttribute.andMaskData[i] = 0xff;
                }
            }

            fpLargePointerAttribute.pad = null;

            fpLargePointerAttribute.AssignUpdateDataAndSize();

            return fpLargePointerAttribute;
        }
    }
}
