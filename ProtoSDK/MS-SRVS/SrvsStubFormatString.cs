// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Srvs
{
    internal class SrvsStubFormatString
    {
        internal static byte[] TypeFormatString
        {
            get
            {
                return RpceStubHelper.GetPlatform() == RpceStubTargetPlatform.Amd64 ?
                    RpceStubHelper.CreateFormatStringByteArray(TYPE_FORMAT_STRING_X64) :
                    RpceStubHelper.CreateFormatStringByteArray(TYPE_FORMAT_STRING_X86);
            }
        }

        internal static byte[] ProcFormatString
        {
            get
            {
                return RpceStubHelper.GetPlatform() == RpceStubTargetPlatform.Amd64 ?
                    RpceStubHelper.CreateFormatStringByteArray(PROC_FORMAT_STRING_X64) :
                    RpceStubHelper.CreateFormatStringByteArray(PROC_FORMAT_STRING_X86);
            }
        }

        internal static ushort[] ProcFormatStringOffsetTable
        {
            get
            {
                return RpceStubHelper.GetPlatform() == RpceStubTargetPlatform.Amd64 ?
                    ProcFormatStringOffsetTableX64 : ProcFormatStringOffsetTableX86;
            }
        }

        private const string PROC_FORMAT_STRING_X86 = @"
    /* Procedure Opnum0NotUsedOnWire */

            0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x0 ),	/* 0 */
/*  8 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 10 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 12 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 14 */	NdrFcShort( 0x0 ),	/* 0 */
/* 16 */	NdrFcShort( 0x0 ),	/* 0 */
/* 18 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 20 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 22 */	NdrFcShort( 0x0 ),	/* 0 */
/* 24 */	NdrFcShort( 0x0 ),	/* 0 */
/* 26 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum1NotUsedOnWire */


    /* Parameter IDL_handle */

/* 28 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 30 */	NdrFcLong( 0x0 ),	/* 0 */
/* 34 */	NdrFcShort( 0x1 ),	/* 1 */
/* 36 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 38 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 40 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 42 */	NdrFcShort( 0x0 ),	/* 0 */
/* 44 */	NdrFcShort( 0x0 ),	/* 0 */
/* 46 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 48 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 50 */	NdrFcShort( 0x0 ),	/* 0 */
/* 52 */	NdrFcShort( 0x0 ),	/* 0 */
/* 54 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum2NotUsedOnWire */


    /* Parameter IDL_handle */

/* 56 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 58 */	NdrFcLong( 0x0 ),	/* 0 */
/* 62 */	NdrFcShort( 0x2 ),	/* 2 */
/* 64 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 66 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 68 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 70 */	NdrFcShort( 0x0 ),	/* 0 */
/* 72 */	NdrFcShort( 0x0 ),	/* 0 */
/* 74 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 76 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 78 */	NdrFcShort( 0x0 ),	/* 0 */
/* 80 */	NdrFcShort( 0x0 ),	/* 0 */
/* 82 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum3NotUsedOnWire */


    /* Parameter IDL_handle */

/* 84 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 86 */	NdrFcLong( 0x0 ),	/* 0 */
/* 90 */	NdrFcShort( 0x3 ),	/* 3 */
/* 92 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 94 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 96 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 98 */	NdrFcShort( 0x0 ),	/* 0 */
/* 100 */	NdrFcShort( 0x0 ),	/* 0 */
/* 102 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 104 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 106 */	NdrFcShort( 0x0 ),	/* 0 */
/* 108 */	NdrFcShort( 0x0 ),	/* 0 */
/* 110 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum4NotUsedOnWire */


    /* Parameter IDL_handle */

/* 112 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 114 */	NdrFcLong( 0x0 ),	/* 0 */
/* 118 */	NdrFcShort( 0x4 ),	/* 4 */
/* 120 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 122 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 124 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 126 */	NdrFcShort( 0x0 ),	/* 0 */
/* 128 */	NdrFcShort( 0x0 ),	/* 0 */
/* 130 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 132 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 134 */	NdrFcShort( 0x0 ),	/* 0 */
/* 136 */	NdrFcShort( 0x0 ),	/* 0 */
/* 138 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum5NotUsedOnWire */


    /* Parameter IDL_handle */

/* 140 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 142 */	NdrFcLong( 0x0 ),	/* 0 */
/* 146 */	NdrFcShort( 0x5 ),	/* 5 */
/* 148 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 150 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 152 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 154 */	NdrFcShort( 0x0 ),	/* 0 */
/* 156 */	NdrFcShort( 0x0 ),	/* 0 */
/* 158 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 160 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 162 */	NdrFcShort( 0x0 ),	/* 0 */
/* 164 */	NdrFcShort( 0x0 ),	/* 0 */
/* 166 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum6NotUsedOnWire */


    /* Parameter IDL_handle */

/* 168 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 170 */	NdrFcLong( 0x0 ),	/* 0 */
/* 174 */	NdrFcShort( 0x6 ),	/* 6 */
/* 176 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 178 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 180 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 182 */	NdrFcShort( 0x0 ),	/* 0 */
/* 184 */	NdrFcShort( 0x0 ),	/* 0 */
/* 186 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 188 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 190 */	NdrFcShort( 0x0 ),	/* 0 */
/* 192 */	NdrFcShort( 0x0 ),	/* 0 */
/* 194 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum7NotUsedOnWire */


    /* Parameter IDL_handle */

/* 196 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 198 */	NdrFcLong( 0x0 ),	/* 0 */
/* 202 */	NdrFcShort( 0x7 ),	/* 7 */
/* 204 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 206 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 208 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 210 */	NdrFcShort( 0x0 ),	/* 0 */
/* 212 */	NdrFcShort( 0x0 ),	/* 0 */
/* 214 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 216 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 218 */	NdrFcShort( 0x0 ),	/* 0 */
/* 220 */	NdrFcShort( 0x0 ),	/* 0 */
/* 222 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetrConnectionEnum */


    /* Parameter IDL_handle */

/* 224 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 226 */	NdrFcLong( 0x0 ),	/* 0 */
/* 230 */	NdrFcShort( 0x8 ),	/* 8 */
/* 232 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 234 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 236 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 238 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 240 */	NdrFcShort( 0x24 ),	/* 36 */
/* 242 */	NdrFcShort( 0x40 ),	/* 64 */
/* 244 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 246 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 248 */	NdrFcShort( 0x1 ),	/* 1 */
/* 250 */	NdrFcShort( 0x1 ),	/* 1 */
/* 252 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 254 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 256 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 258 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Qualifier */

/* 260 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 262 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 264 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 266 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 268 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 270 */	NdrFcShort( 0xba ),	/* Type Offset=186 */

    /* Parameter PreferedMaximumLength */

/* 272 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 274 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 276 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 278 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 280 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 282 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 284 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 286 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 288 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 290 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 292 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 294 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrFileEnum */

/* 296 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 298 */	NdrFcLong( 0x0 ),	/* 0 */
/* 302 */	NdrFcShort( 0x9 ),	/* 9 */
/* 304 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 306 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 308 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 310 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 312 */	NdrFcShort( 0x24 ),	/* 36 */
/* 314 */	NdrFcShort( 0x40 ),	/* 64 */
/* 316 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 318 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 320 */	NdrFcShort( 0x1 ),	/* 1 */
/* 322 */	NdrFcShort( 0x1 ),	/* 1 */
/* 324 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 326 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 328 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 330 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter BasePath */

/* 332 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 334 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 336 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter UserName */

/* 338 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 340 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 342 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 344 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 346 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 348 */	NdrFcShort( 0x154 ),	/* Type Offset=340 */

    /* Parameter PreferedMaximumLength */

/* 350 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 352 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 354 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 356 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 358 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 360 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 362 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 364 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 366 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 368 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 370 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 372 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrFileGetInfo */

/* 374 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 376 */	NdrFcLong( 0x0 ),	/* 0 */
/* 380 */	NdrFcShort( 0xa ),	/* 10 */
/* 382 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 384 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 386 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 388 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 390 */	NdrFcShort( 0x10 ),	/* 16 */
/* 392 */	NdrFcShort( 0x8 ),	/* 8 */
/* 394 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 396 */	0x8,		/* 8 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 398 */	NdrFcShort( 0x1 ),	/* 1 */
/* 400 */	NdrFcShort( 0x0 ),	/* 0 */
/* 402 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 404 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 406 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 408 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter FileId */

/* 410 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 412 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 414 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Level */

/* 416 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 418 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 420 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 422 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 424 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 426 */	NdrFcShort( 0x166 ),	/* Type Offset=358 */

    /* Return value */

/* 428 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 430 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 432 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrFileClose */

/* 434 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 436 */	NdrFcLong( 0x0 ),	/* 0 */
/* 440 */	NdrFcShort( 0xb ),	/* 11 */
/* 442 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 444 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 446 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 448 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 450 */	NdrFcShort( 0x8 ),	/* 8 */
/* 452 */	NdrFcShort( 0x8 ),	/* 8 */
/* 454 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 456 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 458 */	NdrFcShort( 0x0 ),	/* 0 */
/* 460 */	NdrFcShort( 0x0 ),	/* 0 */
/* 462 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 464 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 466 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 468 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter FileId */

/* 470 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 472 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 474 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 476 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 478 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 480 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrSessionEnum */

/* 482 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 484 */	NdrFcLong( 0x0 ),	/* 0 */
/* 488 */	NdrFcShort( 0xc ),	/* 12 */
/* 490 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 492 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 494 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 496 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 498 */	NdrFcShort( 0x24 ),	/* 36 */
/* 500 */	NdrFcShort( 0x40 ),	/* 64 */
/* 502 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 504 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 506 */	NdrFcShort( 0x1 ),	/* 1 */
/* 508 */	NdrFcShort( 0x1 ),	/* 1 */
/* 510 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 512 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 514 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 516 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ClientName */

/* 518 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 520 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 522 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter UserName */

/* 524 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 526 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 528 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 530 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 532 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 534 */	NdrFcShort( 0x3d2 ),	/* Type Offset=978 */

    /* Parameter PreferedMaximumLength */

/* 536 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 538 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 540 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 542 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 544 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 546 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 548 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 550 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 552 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 554 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 556 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 558 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrSessionDel */

/* 560 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 562 */	NdrFcLong( 0x0 ),	/* 0 */
/* 566 */	NdrFcShort( 0xd ),	/* 13 */
/* 568 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 570 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 572 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 574 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 576 */	NdrFcShort( 0x0 ),	/* 0 */
/* 578 */	NdrFcShort( 0x8 ),	/* 8 */
/* 580 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 582 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 584 */	NdrFcShort( 0x0 ),	/* 0 */
/* 586 */	NdrFcShort( 0x0 ),	/* 0 */
/* 588 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 590 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 592 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 594 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ClientName */

/* 596 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 598 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 600 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter UserName */

/* 602 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 604 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 606 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Return value */

/* 608 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 610 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 612 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareAdd */

/* 614 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 616 */	NdrFcLong( 0x0 ),	/* 0 */
/* 620 */	NdrFcShort( 0xe ),	/* 14 */
/* 622 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 624 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 626 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 628 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 630 */	NdrFcShort( 0x24 ),	/* 36 */
/* 632 */	NdrFcShort( 0x24 ),	/* 36 */
/* 634 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 636 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 638 */	NdrFcShort( 0x0 ),	/* 0 */
/* 640 */	NdrFcShort( 0x1 ),	/* 1 */
/* 642 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 644 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 646 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 648 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 650 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 652 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 654 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 656 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 658 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 660 */	NdrFcShort( 0x3e4 ),	/* Type Offset=996 */

    /* Parameter ParmErr */

/* 662 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 664 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 666 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 668 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 670 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 672 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareEnum */

/* 674 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 676 */	NdrFcLong( 0x0 ),	/* 0 */
/* 680 */	NdrFcShort( 0xf ),	/* 15 */
/* 682 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 684 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 686 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 688 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 690 */	NdrFcShort( 0x24 ),	/* 36 */
/* 692 */	NdrFcShort( 0x40 ),	/* 64 */
/* 694 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 696 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 698 */	NdrFcShort( 0x1 ),	/* 1 */
/* 700 */	NdrFcShort( 0x1 ),	/* 1 */
/* 702 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 704 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 706 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 708 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 710 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 712 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 714 */	NdrFcShort( 0x75a ),	/* Type Offset=1882 */

    /* Parameter PreferedMaximumLength */

/* 716 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 718 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 720 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 722 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 724 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 726 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 728 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 730 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 732 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 734 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 736 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 738 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareGetInfo */

/* 740 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 742 */	NdrFcLong( 0x0 ),	/* 0 */
/* 746 */	NdrFcShort( 0x10 ),	/* 16 */
/* 748 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 750 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 752 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 754 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 756 */	NdrFcShort( 0x8 ),	/* 8 */
/* 758 */	NdrFcShort( 0x8 ),	/* 8 */
/* 760 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 762 */	0x8,		/* 8 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 764 */	NdrFcShort( 0x1 ),	/* 1 */
/* 766 */	NdrFcShort( 0x0 ),	/* 0 */
/* 768 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 770 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 772 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 774 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 776 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 778 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 780 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Level */

/* 782 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 784 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 786 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 788 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 790 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 792 */	NdrFcShort( 0x770 ),	/* Type Offset=1904 */

    /* Return value */

/* 794 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 796 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 798 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareSetInfo */

/* 800 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 802 */	NdrFcLong( 0x0 ),	/* 0 */
/* 806 */	NdrFcShort( 0x11 ),	/* 17 */
/* 808 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 810 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 812 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 814 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 816 */	NdrFcShort( 0x24 ),	/* 36 */
/* 818 */	NdrFcShort( 0x24 ),	/* 36 */
/* 820 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 822 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 824 */	NdrFcShort( 0x0 ),	/* 0 */
/* 826 */	NdrFcShort( 0x1 ),	/* 1 */
/* 828 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 830 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 832 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 834 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 836 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 838 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 840 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Level */

/* 842 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 844 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 846 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ShareInfo */

/* 848 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 850 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 852 */	NdrFcShort( 0x77e ),	/* Type Offset=1918 */

    /* Parameter ParmErr */

/* 854 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 856 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 858 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 860 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 862 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 864 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDel */

/* 866 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 868 */	NdrFcLong( 0x0 ),	/* 0 */
/* 872 */	NdrFcShort( 0x12 ),	/* 18 */
/* 874 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 876 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 878 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 880 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 882 */	NdrFcShort( 0x8 ),	/* 8 */
/* 884 */	NdrFcShort( 0x8 ),	/* 8 */
/* 886 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 888 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 890 */	NdrFcShort( 0x0 ),	/* 0 */
/* 892 */	NdrFcShort( 0x0 ),	/* 0 */
/* 894 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 896 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 898 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 900 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 902 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 904 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 906 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Reserved */

/* 908 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 910 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 912 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 914 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 916 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 918 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelSticky */

/* 920 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 922 */	NdrFcLong( 0x0 ),	/* 0 */
/* 926 */	NdrFcShort( 0x13 ),	/* 19 */
/* 928 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 930 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 932 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 934 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 936 */	NdrFcShort( 0x8 ),	/* 8 */
/* 938 */	NdrFcShort( 0x8 ),	/* 8 */
/* 940 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 942 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 944 */	NdrFcShort( 0x0 ),	/* 0 */
/* 946 */	NdrFcShort( 0x0 ),	/* 0 */
/* 948 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 950 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 952 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 954 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 956 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 958 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 960 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Reserved */

/* 962 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 964 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 966 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 968 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 970 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 972 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareCheck */

/* 974 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 976 */	NdrFcLong( 0x0 ),	/* 0 */
/* 980 */	NdrFcShort( 0x14 ),	/* 20 */
/* 982 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 984 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 986 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 988 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 990 */	NdrFcShort( 0x0 ),	/* 0 */
/* 992 */	NdrFcShort( 0x24 ),	/* 36 */
/* 994 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 996 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 998 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1000 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1002 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1004 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1006 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1008 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Device */

/* 1010 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1012 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1014 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Type */

/* 1016 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1018 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1020 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1022 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1024 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1026 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerGetInfo */

/* 1028 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1030 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1034 */	NdrFcShort( 0x15 ),	/* 21 */
/* 1036 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1038 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1040 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1042 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1044 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1046 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1048 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 1050 */	0x8,		/* 8 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1052 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1054 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1056 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1058 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1060 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1062 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1064 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1066 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1068 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 1070 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1072 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1074 */	NdrFcShort( 0x78c ),	/* Type Offset=1932 */

    /* Return value */

/* 1076 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1078 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1080 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerSetInfo */

/* 1082 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1084 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1088 */	NdrFcShort( 0x16 ),	/* 22 */
/* 1090 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1092 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1094 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1096 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1098 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1100 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1102 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 1104 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1106 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1108 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1110 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1112 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1114 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1116 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1118 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1120 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1122 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ServerInfo */

/* 1124 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1126 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1128 */	NdrFcShort( 0xa2a ),	/* Type Offset=2602 */

    /* Parameter ParmErr */

/* 1130 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 1132 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1134 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 1136 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1138 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1140 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerDiskEnum */

/* 1142 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1144 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1148 */	NdrFcShort( 0x17 ),	/* 23 */
/* 1150 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1152 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1154 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1156 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1158 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1160 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1162 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 1164 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 1166 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1168 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1170 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1172 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1174 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1176 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1178 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1180 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1182 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter DiskInfoStruct */

/* 1184 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 1186 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1188 */	NdrFcShort( 0xa60 ),	/* Type Offset=2656 */

    /* Parameter PreferedMaximumLength */

/* 1190 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1192 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1194 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 1196 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1198 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1200 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 1202 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 1204 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1206 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 1208 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1210 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1212 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerStatisticsGet */

/* 1214 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1216 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1220 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1222 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1224 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1226 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1228 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1230 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1232 */	NdrFcShort( 0x84 ),	/* 132 */
/* 1234 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1236 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1238 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1240 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1242 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1244 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1246 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1248 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Service */

/* 1250 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1252 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1254 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1256 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1258 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1260 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Options */

/* 1262 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1264 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1266 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 1268 */	NdrFcShort( 0x2012 ),	/* Flags:  must free, out, srv alloc size=8 */
/* 1270 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1272 */	NdrFcShort( 0xa74 ),	/* Type Offset=2676 */

    /* Return value */

/* 1274 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1276 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1278 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportAdd */

/* 1280 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1282 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1286 */	NdrFcShort( 0x19 ),	/* 25 */
/* 1288 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1290 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1292 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1294 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1296 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1298 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1300 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 1302 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1304 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1306 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1308 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1310 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1312 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1314 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1316 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1318 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1320 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 1322 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1324 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1326 */	NdrFcShort( 0xaa2 ),	/* Type Offset=2722 */

    /* Return value */

/* 1328 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1330 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1332 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportEnum */

/* 1334 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1336 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1340 */	NdrFcShort( 0x1a ),	/* 26 */
/* 1342 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1344 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1346 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1348 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1350 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1352 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1354 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1356 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 1358 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1360 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1362 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1364 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1366 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1368 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 1370 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 1372 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1374 */	NdrFcShort( 0xcea ),	/* Type Offset=3306 */

    /* Parameter PreferedMaximumLength */

/* 1376 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1378 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1380 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 1382 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1384 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1386 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 1388 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 1390 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1392 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 1394 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1396 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1398 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportDel */

/* 1400 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1402 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1406 */	NdrFcShort( 0x1b ),	/* 27 */
/* 1408 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1410 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1412 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1414 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1416 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1418 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1420 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 1422 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1424 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1426 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1428 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1430 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1432 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1434 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1436 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1438 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1440 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 1442 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1444 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1446 */	NdrFcShort( 0xaa2 ),	/* Type Offset=2722 */

    /* Return value */

/* 1448 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1450 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1452 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrRemoteTOD */

/* 1454 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1456 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1460 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1462 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1464 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1466 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1468 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1470 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1472 */	NdrFcShort( 0x70 ),	/* 112 */
/* 1474 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 1476 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1478 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1480 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1482 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1484 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1486 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1488 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter BufferPtr */

/* 1490 */	NdrFcShort( 0x2012 ),	/* Flags:  must free, out, srv alloc size=8 */
/* 1492 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1494 */	NdrFcShort( 0xcf8 ),	/* Type Offset=3320 */

    /* Return value */

/* 1496 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1498 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1500 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure Opnum29NotUsedOnWire */

/* 1502 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1504 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1508 */	NdrFcShort( 0x1d ),	/* 29 */
/* 1510 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1512 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 1514 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1516 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1518 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1520 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 1522 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1524 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1526 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1528 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetprPathType */


    /* Parameter IDL_handle */

/* 1530 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1532 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1536 */	NdrFcShort( 0x1e ),	/* 30 */
/* 1538 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1540 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1542 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1544 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1546 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1548 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1550 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 1552 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1554 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1556 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1558 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1560 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1562 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1564 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter PathName */

/* 1566 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1568 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1570 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter PathType */

/* 1572 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1574 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1576 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1578 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1580 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1582 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1584 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1586 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1588 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprPathCanonicalize */

/* 1590 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1592 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1596 */	NdrFcShort( 0x1f ),	/* 31 */
/* 1598 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 1600 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1602 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1604 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1606 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1608 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1610 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 1612 */	0x8,		/* 8 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1614 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1616 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1618 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1620 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1622 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1624 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter PathName */

/* 1626 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1628 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1630 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Outbuf */

/* 1632 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1634 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1636 */	NdrFcShort( 0xd16 ),	/* Type Offset=3350 */

    /* Parameter OutbufLen */

/* 1638 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 1640 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1642 */	NdrFcShort( 0xd22 ),	/* 3362 */

    /* Parameter Prefix */

/* 1644 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1646 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1648 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter PathType */

/* 1650 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 1652 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1654 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1656 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1658 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1660 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1662 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1664 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1666 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprPathCompare */

/* 1668 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1670 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1674 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1676 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1678 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1680 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1682 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1684 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1686 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1688 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1690 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1692 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1694 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1696 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1698 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1700 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1702 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter PathName1 */

/* 1704 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1706 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1708 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter PathName2 */

/* 1710 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1712 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1714 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter PathType */

/* 1716 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1718 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1720 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1722 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1724 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1726 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1728 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1730 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1732 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprNameValidate */

/* 1734 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1736 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1740 */	NdrFcShort( 0x21 ),	/* 33 */
/* 1742 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1744 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1746 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1748 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1750 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1752 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1754 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 1756 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1758 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1760 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1762 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1764 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1766 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1768 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Name */

/* 1770 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1772 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1774 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter NameType */

/* 1776 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1778 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1780 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1782 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1784 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1786 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1788 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1790 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1792 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprNameCanonicalize */

/* 1794 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1796 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1800 */	NdrFcShort( 0x22 ),	/* 34 */
/* 1802 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 1804 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1806 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1808 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1810 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1812 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1814 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 1816 */	0x8,		/* 8 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1818 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1820 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1822 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1824 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1826 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1828 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Name */

/* 1830 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1832 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1834 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Outbuf */

/* 1836 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1838 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1840 */	NdrFcShort( 0xd34 ),	/* Type Offset=3380 */

    /* Parameter OutbufLen */

/* 1842 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 1844 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1846 */	NdrFcShort( 0xd40 ),	/* 3392 */

    /* Parameter NameType */

/* 1848 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1850 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1852 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1854 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1856 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1858 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1860 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1862 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1864 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprNameCompare */

/* 1866 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1868 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1872 */	NdrFcShort( 0x23 ),	/* 35 */
/* 1874 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1876 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1878 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1880 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1882 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1884 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1886 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1888 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1890 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1892 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1894 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1896 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1898 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1900 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Name1 */

/* 1902 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1904 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1906 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Name2 */

/* 1908 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1910 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1912 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter NameType */

/* 1914 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1916 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1918 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1920 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1922 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1924 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1926 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1928 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1930 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareEnumSticky */

/* 1932 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1934 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1938 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1940 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 1942 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 1944 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1946 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1948 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1950 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1952 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1954 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 1956 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1958 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1960 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1962 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1964 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 1966 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 1968 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 1970 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1972 */	NdrFcShort( 0x75a ),	/* Type Offset=1882 */

    /* Parameter PreferedMaximumLength */

/* 1974 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1976 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1978 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 1980 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1982 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 1984 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 1986 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 1988 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 1990 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 1992 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1994 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 1996 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelStart */

/* 1998 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2000 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2004 */	NdrFcShort( 0x25 ),	/* 37 */
/* 2006 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2008 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2010 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2012 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2014 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2016 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2018 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 2020 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2022 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2024 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2026 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2028 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2030 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2032 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 2034 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2036 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2038 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Reserved */

/* 2040 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2042 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2044 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ContextHandle */

/* 2046 */	NdrFcShort( 0x110 ),	/* Flags:  out, simple ref, */
/* 2048 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2050 */	NdrFcShort( 0xd4e ),	/* Type Offset=3406 */

    /* Return value */

/* 2052 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2054 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2056 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelCommit */

/* 2058 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2060 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2064 */	NdrFcShort( 0x26 ),	/* 38 */
/* 2066 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2068 */	0x30,		/* FC_BIND_CONTEXT */
            0xe0,		/* Ctxt flags:  via ptr, in, out, */
/* 2070 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2072 */	0x0,		/* 0 */
            0x0,		/* 0 */
/* 2074 */	NdrFcShort( 0x38 ),	/* 56 */
/* 2076 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2078 */	0x44,		/* Oi2 Flags:  has return, has ext, */
            0x2,		/* 2 */
/* 2080 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2082 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2084 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2086 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ContextHandle */

/* 2088 */	NdrFcShort( 0x118 ),	/* Flags:  in, out, simple ref, */
/* 2090 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2092 */	NdrFcShort( 0xd56 ),	/* Type Offset=3414 */

    /* Return value */

/* 2094 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2096 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2098 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrpGetFileSecurity */

/* 2100 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2102 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2106 */	NdrFcShort( 0x27 ),	/* 39 */
/* 2108 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 2110 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2112 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2114 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2116 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2118 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2120 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 2122 */	0x8,		/* 8 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 2124 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2126 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2128 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2130 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2132 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2134 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ShareName */

/* 2136 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2138 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2140 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter lpFileName */

/* 2142 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2144 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2146 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter RequestedInformation */

/* 2148 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2150 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2152 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter SecurityDescriptor */

/* 2154 */	NdrFcShort( 0x2013 ),	/* Flags:  must size, must free, out, srv alloc size=8 */
/* 2156 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2158 */	NdrFcShort( 0xd5a ),	/* Type Offset=3418 */

    /* Return value */

/* 2160 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2162 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2164 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrpSetFileSecurity */

/* 2166 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2168 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2172 */	NdrFcShort( 0x28 ),	/* 40 */
/* 2174 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 2176 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2178 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2180 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2182 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2184 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2186 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 2188 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2190 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2192 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2194 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2196 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2198 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2200 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ShareName */

/* 2202 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2204 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2206 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter lpFileName */

/* 2208 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2210 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2212 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter SecurityInformation */

/* 2214 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2216 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2218 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter SecurityDescriptor */

/* 2220 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2222 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2224 */	NdrFcShort( 0x4f8 ),	/* Type Offset=1272 */

    /* Return value */

/* 2226 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2228 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2230 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportAddEx */

/* 2232 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2234 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2238 */	NdrFcShort( 0x29 ),	/* 41 */
/* 2240 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2242 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2244 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2246 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2248 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2250 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2252 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2254 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2256 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2258 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2260 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2262 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2264 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2266 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 2268 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2270 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2272 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 2274 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2276 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2278 */	NdrFcShort( 0xd66 ),	/* Type Offset=3430 */

    /* Return value */

/* 2280 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2282 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2284 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure Opnum42NotUsedOnWire */

/* 2286 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2288 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2292 */	NdrFcShort( 0x2a ),	/* 42 */
/* 2294 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2296 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 2298 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2300 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2302 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2304 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 2306 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2308 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2310 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2312 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetrDfsGetVersion */


    /* Parameter IDL_handle */

/* 2314 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2316 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2320 */	NdrFcShort( 0x2b ),	/* 43 */
/* 2322 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2324 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2326 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2328 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2330 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2332 */	NdrFcShort( 0x24 ),	/* 36 */
/* 2334 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 2336 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2338 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2340 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2342 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2344 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2346 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2348 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Version */

/* 2350 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 2352 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2354 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2356 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2358 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2360 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsCreateLocalPartition */

/* 2362 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2364 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2368 */	NdrFcShort( 0x2c ),	/* 44 */
/* 2370 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 2372 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2374 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2376 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2378 */	NdrFcShort( 0x4c ),	/* 76 */
/* 2380 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2382 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 2384 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2386 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2388 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2390 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2392 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2394 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2396 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ShareName */

/* 2398 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2400 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2402 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter EntryUid */

/* 2404 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2406 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2408 */	NdrFcShort( 0xd98 ),	/* Type Offset=3480 */

    /* Parameter EntryPrefix */

/* 2410 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2412 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2414 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter ShortName */

/* 2416 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2418 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2420 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter RelationInfo */

/* 2422 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2424 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2426 */	NdrFcShort( 0xde2 ),	/* Type Offset=3554 */

    /* Parameter Force */

/* 2428 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2430 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 2432 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2434 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2436 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 2438 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsDeleteLocalPartition */

/* 2440 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2442 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2446 */	NdrFcShort( 0x2d ),	/* 45 */
/* 2448 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2450 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2452 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2454 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2456 */	NdrFcShort( 0x44 ),	/* 68 */
/* 2458 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2460 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2462 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2464 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2466 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2468 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2470 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2472 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2474 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2476 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2478 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2480 */	NdrFcShort( 0xd98 ),	/* Type Offset=3480 */

    /* Parameter Prefix */

/* 2482 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2484 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2486 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Return value */

/* 2488 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2490 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2492 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsSetLocalVolumeState */

/* 2494 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2496 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2500 */	NdrFcShort( 0x2e ),	/* 46 */
/* 2502 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2504 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2506 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2508 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2510 */	NdrFcShort( 0x4c ),	/* 76 */
/* 2512 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2514 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 2516 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2518 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2520 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2522 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2524 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2526 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2528 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2530 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2532 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2534 */	NdrFcShort( 0xd98 ),	/* Type Offset=3480 */

    /* Parameter Prefix */

/* 2536 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2538 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2540 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter State */

/* 2542 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2544 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2546 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2548 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2550 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2552 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure Opnum47NotUsedOnWire */

/* 2554 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2556 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2560 */	NdrFcShort( 0x2f ),	/* 47 */
/* 2562 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2564 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 2566 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2568 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2570 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2572 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 2574 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2576 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2578 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2580 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetrDfsCreateExitPoint */


    /* Parameter IDL_handle */

/* 2582 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2584 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2588 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2590 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 2592 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2594 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2596 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2598 */	NdrFcShort( 0x54 ),	/* 84 */
/* 2600 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2602 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 2604 */	0x8,		/* 8 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 2606 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2608 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2610 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2612 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2614 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2616 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2618 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2620 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2622 */	NdrFcShort( 0xd98 ),	/* Type Offset=3480 */

    /* Parameter Prefix */

/* 2624 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2626 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2628 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Type */

/* 2630 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2632 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2634 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ShortPrefixLen */

/* 2636 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 2638 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2640 */	NdrFcShort( 0xdf6 ),	/* 3574 */

    /* Parameter ShortPrefix */

/* 2642 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 2644 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2646 */	NdrFcShort( 0xe04 ),	/* Type Offset=3588 */

    /* Return value */

/* 2648 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2650 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 2652 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsDeleteExitPoint */

/* 2654 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2656 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2660 */	NdrFcShort( 0x31 ),	/* 49 */
/* 2662 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2664 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2666 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2668 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2670 */	NdrFcShort( 0x4c ),	/* 76 */
/* 2672 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2674 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 2676 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2678 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2680 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2682 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2684 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2686 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2688 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2690 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2692 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2694 */	NdrFcShort( 0xd98 ),	/* Type Offset=3480 */

    /* Parameter Prefix */

/* 2696 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2698 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2700 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter Type */

/* 2702 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2704 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2706 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2708 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2710 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2712 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsModifyPrefix */

/* 2714 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2716 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2720 */	NdrFcShort( 0x32 ),	/* 50 */
/* 2722 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2724 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2726 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2728 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2730 */	NdrFcShort( 0x44 ),	/* 68 */
/* 2732 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2734 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2736 */	0x8,		/* 8 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2738 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2740 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2742 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2744 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2746 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2748 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2750 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2752 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2754 */	NdrFcShort( 0xd98 ),	/* Type Offset=3480 */

    /* Parameter Prefix */

/* 2756 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2758 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2760 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Return value */

/* 2762 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2764 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2766 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsFixLocalVolume */

/* 2768 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2770 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2774 */	NdrFcShort( 0x33 ),	/* 51 */
/* 2776 */	NdrFcShort( 0x28 ),	/* x86 Stack size/offset = 40 */
/* 2778 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2780 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2782 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2784 */	NdrFcShort( 0x5c ),	/* 92 */
/* 2786 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2788 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0xa,		/* 10 */
/* 2790 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2792 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2794 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2796 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2798 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2800 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2802 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter VolumeName */

/* 2804 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2806 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2808 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter EntryType */

/* 2810 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2812 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2814 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ServiceType */

/* 2816 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2818 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2820 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter StgId */

/* 2822 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2824 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2826 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter EntryUid */

/* 2828 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2830 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 2832 */	NdrFcShort( 0xd98 ),	/* Type Offset=3480 */

    /* Parameter EntryPrefix */

/* 2834 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2836 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 2838 */	NdrFcShort( 0x76a ),	/* Type Offset=1898 */

    /* Parameter RelationInfo */

/* 2840 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2842 */	NdrFcShort( 0x1c ),	/* x86 Stack size/offset = 28 */
/* 2844 */	NdrFcShort( 0xde2 ),	/* Type Offset=3554 */

    /* Parameter CreateDisposition */

/* 2846 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2848 */	NdrFcShort( 0x20 ),	/* x86 Stack size/offset = 32 */
/* 2850 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2852 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2854 */	NdrFcShort( 0x24 ),	/* x86 Stack size/offset = 36 */
/* 2856 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsManagerReportSiteInfo */

/* 2858 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2860 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2864 */	NdrFcShort( 0x34 ),	/* 52 */
/* 2866 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2868 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2870 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2872 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2874 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2876 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2878 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 2880 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 2882 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2884 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2886 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2888 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2890 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2892 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ppSiteInfo */

/* 2894 */	NdrFcShort( 0x201b ),	/* Flags:  must size, must free, in, out, srv alloc size=8 */
/* 2896 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2898 */	NdrFcShort( 0xe10 ),	/* Type Offset=3600 */

    /* Return value */

/* 2900 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2902 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2904 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportDelEx */

/* 2906 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2908 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2912 */	NdrFcShort( 0x35 ),	/* 53 */
/* 2914 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2916 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2918 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2920 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2922 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2924 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2926 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2928 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2930 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2932 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2934 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2936 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2938 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2940 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 2942 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2944 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2946 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 2948 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2950 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 2952 */	NdrFcShort( 0xe48 ),	/* Type Offset=3656 */

    /* Return value */

/* 2954 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2956 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 2958 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerAliasAdd */

/* 2960 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2962 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2966 */	NdrFcShort( 0x36 ),	/* 54 */
/* 2968 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 2970 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 2972 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2974 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2976 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2978 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2980 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2982 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2984 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2986 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2988 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2990 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2992 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 2994 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 2996 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2998 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3000 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 3002 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 3004 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 3006 */	NdrFcShort( 0xe56 ),	/* Type Offset=3670 */

    /* Return value */

/* 3008 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3010 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 3012 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerAliasEnum */

/* 3014 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3016 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3020 */	NdrFcShort( 0x37 ),	/* 55 */
/* 3022 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 3024 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 3026 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 3028 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3030 */	NdrFcShort( 0x24 ),	/* 36 */
/* 3032 */	NdrFcShort( 0x40 ),	/* 64 */
/* 3034 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 3036 */	0x8,		/* 8 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 3038 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3040 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3042 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3044 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3046 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 3048 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 3050 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 3052 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3054 */	NdrFcShort( 0xeee ),	/* Type Offset=3822 */

    /* Parameter PreferedMaximumLength */

/* 3056 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3058 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 3060 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 3062 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 3064 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 3066 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 3068 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 3070 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 3072 */	NdrFcShort( 0xcc ),	/* Type Offset=204 */

    /* Return value */

/* 3074 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3076 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 3078 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerAliasDel */

/* 3080 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3082 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3086 */	NdrFcShort( 0x38 ),	/* 56 */
/* 3088 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 3090 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 3092 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 3094 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3096 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3098 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3100 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 3102 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 3104 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3106 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3108 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3110 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3112 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 3114 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 3116 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3118 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3120 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 3122 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 3124 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 3126 */	NdrFcShort( 0xf00 ),	/* Type Offset=3840 */

    /* Return value */

/* 3128 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3130 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 3132 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelEx */

/* 3134 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3136 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3140 */	NdrFcShort( 0x39 ),	/* 57 */
/* 3142 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 3144 */	0x31,		/* FC_BIND_GENERIC */
            0x4,		/* 4 */
/* 3146 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 3148 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3150 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3152 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3154 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 3156 */	0x8,		/* 8 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 3158 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3160 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3162 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3164 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3166 */	NdrFcShort( 0x0 ),	/* x86 Stack size/offset = 0 */
/* 3168 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 3170 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3172 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3174 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ShareInfo */

/* 3176 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 3178 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 3180 */	NdrFcShort( 0xf0e ),	/* Type Offset=3854 */

    /* Return value */

/* 3182 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3184 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 3186 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

            0x0";

        private const string TYPE_FORMAT_STRING_X86 = @"
            NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/*  4 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/*  6 */	
            0x11, 0x0,	/* FC_RP */
/*  8 */	NdrFcShort( 0xb2 ),	/* Offset= 178 (186) */
/* 10 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 12 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 14 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 16 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 18 */	NdrFcShort( 0x2 ),	/* Offset= 2 (20) */
/* 20 */	NdrFcShort( 0x4 ),	/* 4 */
/* 22 */	NdrFcShort( 0x3002 ),	/* 12290 */
/* 24 */	NdrFcLong( 0x0 ),	/* 0 */
/* 28 */	NdrFcShort( 0xa ),	/* Offset= 10 (38) */
/* 30 */	NdrFcLong( 0x1 ),	/* 1 */
/* 34 */	NdrFcShort( 0x32 ),	/* Offset= 50 (84) */
/* 36 */	NdrFcShort( 0xffff ),	/* Offset= -1 (35) */
/* 38 */	
            0x12, 0x0,	/* FC_UP */
/* 40 */	NdrFcShort( 0x18 ),	/* Offset= 24 (64) */
/* 42 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 44 */	NdrFcShort( 0x4 ),	/* 4 */
/* 46 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 48 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 50 */	NdrFcShort( 0x4 ),	/* 4 */
/* 52 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 54 */	NdrFcShort( 0x0 ),	/* 0 */
/* 56 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 58 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 60 */	NdrFcShort( 0xffee ),	/* Offset= -18 (42) */
/* 62 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 64 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 66 */	NdrFcShort( 0x8 ),	/* 8 */
/* 68 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 70 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 72 */	NdrFcShort( 0x4 ),	/* 4 */
/* 74 */	NdrFcShort( 0x4 ),	/* 4 */
/* 76 */	0x12, 0x0,	/* FC_UP */
/* 78 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (48) */
/* 80 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 82 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 84 */	
            0x12, 0x0,	/* FC_UP */
/* 86 */	NdrFcShort( 0x50 ),	/* Offset= 80 (166) */
/* 88 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 90 */	NdrFcShort( 0x1c ),	/* 28 */
/* 92 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 94 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 96 */	NdrFcShort( 0x14 ),	/* 20 */
/* 98 */	NdrFcShort( 0x14 ),	/* 20 */
/* 100 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 102 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 104 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 106 */	NdrFcShort( 0x18 ),	/* 24 */
/* 108 */	NdrFcShort( 0x18 ),	/* 24 */
/* 110 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 112 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 114 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 116 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 118 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 120 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 122 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 124 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 126 */	NdrFcShort( 0x1c ),	/* 28 */
/* 128 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 130 */	NdrFcShort( 0x0 ),	/* 0 */
/* 132 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 134 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 136 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 138 */	NdrFcShort( 0x1c ),	/* 28 */
/* 140 */	NdrFcShort( 0x0 ),	/* 0 */
/* 142 */	NdrFcShort( 0x2 ),	/* 2 */
/* 144 */	NdrFcShort( 0x14 ),	/* 20 */
/* 146 */	NdrFcShort( 0x14 ),	/* 20 */
/* 148 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 150 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 152 */	NdrFcShort( 0x18 ),	/* 24 */
/* 154 */	NdrFcShort( 0x18 ),	/* 24 */
/* 156 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 158 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 160 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 162 */	0x0,		/* 0 */
            NdrFcShort( 0xffb5 ),	/* Offset= -75 (88) */
            0x5b,		/* FC_END */
/* 166 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 168 */	NdrFcShort( 0x8 ),	/* 8 */
/* 170 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 172 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 174 */	NdrFcShort( 0x4 ),	/* 4 */
/* 176 */	NdrFcShort( 0x4 ),	/* 4 */
/* 178 */	0x12, 0x0,	/* FC_UP */
/* 180 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (124) */
/* 182 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 184 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 186 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 188 */	NdrFcShort( 0x8 ),	/* 8 */
/* 190 */	NdrFcShort( 0x0 ),	/* 0 */
/* 192 */	NdrFcShort( 0x0 ),	/* Offset= 0 (192) */
/* 194 */	0x8,		/* FC_LONG */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 196 */	0x0,		/* 0 */
            NdrFcShort( 0xff45 ),	/* Offset= -187 (10) */
            0x5b,		/* FC_END */
/* 200 */	
            0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 202 */	0x8,		/* FC_LONG */
            0x5c,		/* FC_PAD */
/* 204 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 206 */	0x8,		/* FC_LONG */
            0x5c,		/* FC_PAD */
/* 208 */	
            0x11, 0x0,	/* FC_RP */
/* 210 */	NdrFcShort( 0x82 ),	/* Offset= 130 (340) */
/* 212 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 214 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 216 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 218 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 220 */	NdrFcShort( 0x2 ),	/* Offset= 2 (222) */
/* 222 */	NdrFcShort( 0x4 ),	/* 4 */
/* 224 */	NdrFcShort( 0x3002 ),	/* 12290 */
/* 226 */	NdrFcLong( 0x2 ),	/* 2 */
/* 230 */	NdrFcShort( 0xff40 ),	/* Offset= -192 (38) */
/* 232 */	NdrFcLong( 0x3 ),	/* 3 */
/* 236 */	NdrFcShort( 0x4 ),	/* Offset= 4 (240) */
/* 238 */	NdrFcShort( 0xffff ),	/* Offset= -1 (237) */
/* 240 */	
            0x12, 0x0,	/* FC_UP */
/* 242 */	NdrFcShort( 0x4e ),	/* Offset= 78 (320) */
/* 244 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 246 */	NdrFcShort( 0x14 ),	/* 20 */
/* 248 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 250 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 252 */	NdrFcShort( 0xc ),	/* 12 */
/* 254 */	NdrFcShort( 0xc ),	/* 12 */
/* 256 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 258 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 260 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 262 */	NdrFcShort( 0x10 ),	/* 16 */
/* 264 */	NdrFcShort( 0x10 ),	/* 16 */
/* 266 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 268 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 270 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 272 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 274 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 276 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 278 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 280 */	NdrFcShort( 0x14 ),	/* 20 */
/* 282 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 284 */	NdrFcShort( 0x0 ),	/* 0 */
/* 286 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 288 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 290 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 292 */	NdrFcShort( 0x14 ),	/* 20 */
/* 294 */	NdrFcShort( 0x0 ),	/* 0 */
/* 296 */	NdrFcShort( 0x2 ),	/* 2 */
/* 298 */	NdrFcShort( 0xc ),	/* 12 */
/* 300 */	NdrFcShort( 0xc ),	/* 12 */
/* 302 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 304 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 306 */	NdrFcShort( 0x10 ),	/* 16 */
/* 308 */	NdrFcShort( 0x10 ),	/* 16 */
/* 310 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 312 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 314 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 316 */	0x0,		/* 0 */
            NdrFcShort( 0xffb7 ),	/* Offset= -73 (244) */
            0x5b,		/* FC_END */
/* 320 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 322 */	NdrFcShort( 0x8 ),	/* 8 */
/* 324 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 326 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 328 */	NdrFcShort( 0x4 ),	/* 4 */
/* 330 */	NdrFcShort( 0x4 ),	/* 4 */
/* 332 */	0x12, 0x0,	/* FC_UP */
/* 334 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (278) */
/* 336 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 338 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 340 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 342 */	NdrFcShort( 0x8 ),	/* 8 */
/* 344 */	NdrFcShort( 0x0 ),	/* 0 */
/* 346 */	NdrFcShort( 0x0 ),	/* Offset= 0 (346) */
/* 348 */	0x8,		/* FC_LONG */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 350 */	0x0,		/* 0 */
            NdrFcShort( 0xff75 ),	/* Offset= -139 (212) */
            0x5b,		/* FC_END */
/* 354 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 356 */	NdrFcShort( 0x2 ),	/* Offset= 2 (358) */
/* 358 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 360 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 362 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 364 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 366 */	NdrFcShort( 0x2 ),	/* Offset= 2 (368) */
/* 368 */	NdrFcShort( 0x4 ),	/* 4 */
/* 370 */	NdrFcShort( 0x3002 ),	/* 12290 */
/* 372 */	NdrFcLong( 0x2 ),	/* 2 */
/* 376 */	NdrFcShort( 0xa ),	/* Offset= 10 (386) */
/* 378 */	NdrFcLong( 0x3 ),	/* 3 */
/* 382 */	NdrFcShort( 0x8 ),	/* Offset= 8 (390) */
/* 384 */	NdrFcShort( 0xffff ),	/* Offset= -1 (383) */
/* 386 */	
            0x12, 0x0,	/* FC_UP */
/* 388 */	NdrFcShort( 0xfea6 ),	/* Offset= -346 (42) */
/* 390 */	
            0x12, 0x0,	/* FC_UP */
/* 392 */	NdrFcShort( 0xff6c ),	/* Offset= -148 (244) */
/* 394 */	
            0x11, 0x0,	/* FC_RP */
/* 396 */	NdrFcShort( 0x246 ),	/* Offset= 582 (978) */
/* 398 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 400 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 402 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 404 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 406 */	NdrFcShort( 0x2 ),	/* Offset= 2 (408) */
/* 408 */	NdrFcShort( 0x4 ),	/* 4 */
/* 410 */	NdrFcShort( 0x3005 ),	/* 12293 */
/* 412 */	NdrFcLong( 0x0 ),	/* 0 */
/* 416 */	NdrFcShort( 0x1c ),	/* Offset= 28 (444) */
/* 418 */	NdrFcLong( 0x1 ),	/* 1 */
/* 422 */	NdrFcShort( 0x64 ),	/* Offset= 100 (522) */
/* 424 */	NdrFcLong( 0x2 ),	/* 2 */
/* 428 */	NdrFcShort( 0xc2 ),	/* Offset= 194 (622) */
/* 430 */	NdrFcLong( 0xa ),	/* 10 */
/* 434 */	NdrFcShort( 0x134 ),	/* Offset= 308 (742) */
/* 436 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 440 */	NdrFcShort( 0x190 ),	/* Offset= 400 (840) */
/* 442 */	NdrFcShort( 0xffff ),	/* Offset= -1 (441) */
/* 444 */	
            0x12, 0x0,	/* FC_UP */
/* 446 */	NdrFcShort( 0x38 ),	/* Offset= 56 (502) */
/* 448 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 450 */	NdrFcShort( 0x4 ),	/* 4 */
/* 452 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 454 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 456 */	NdrFcShort( 0x0 ),	/* 0 */
/* 458 */	NdrFcShort( 0x0 ),	/* 0 */
/* 460 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 462 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 464 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 466 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 468 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 470 */	NdrFcShort( 0x4 ),	/* 4 */
/* 472 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 474 */	NdrFcShort( 0x0 ),	/* 0 */
/* 476 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 478 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 480 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 482 */	NdrFcShort( 0x4 ),	/* 4 */
/* 484 */	NdrFcShort( 0x0 ),	/* 0 */
/* 486 */	NdrFcShort( 0x1 ),	/* 1 */
/* 488 */	NdrFcShort( 0x0 ),	/* 0 */
/* 490 */	NdrFcShort( 0x0 ),	/* 0 */
/* 492 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 494 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 496 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 498 */	0x0,		/* 0 */
            NdrFcShort( 0xffcd ),	/* Offset= -51 (448) */
            0x5b,		/* FC_END */
/* 502 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 504 */	NdrFcShort( 0x8 ),	/* 8 */
/* 506 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 508 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 510 */	NdrFcShort( 0x4 ),	/* 4 */
/* 512 */	NdrFcShort( 0x4 ),	/* 4 */
/* 514 */	0x12, 0x0,	/* FC_UP */
/* 516 */	NdrFcShort( 0xffd0 ),	/* Offset= -48 (468) */
/* 518 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 520 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 522 */	
            0x12, 0x0,	/* FC_UP */
/* 524 */	NdrFcShort( 0x4e ),	/* Offset= 78 (602) */
/* 526 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 528 */	NdrFcShort( 0x18 ),	/* 24 */
/* 530 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 532 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 534 */	NdrFcShort( 0x0 ),	/* 0 */
/* 536 */	NdrFcShort( 0x0 ),	/* 0 */
/* 538 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 540 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 542 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 544 */	NdrFcShort( 0x4 ),	/* 4 */
/* 546 */	NdrFcShort( 0x4 ),	/* 4 */
/* 548 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 550 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 552 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 554 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 556 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 558 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 560 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 562 */	NdrFcShort( 0x18 ),	/* 24 */
/* 564 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 566 */	NdrFcShort( 0x0 ),	/* 0 */
/* 568 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 570 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 572 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 574 */	NdrFcShort( 0x18 ),	/* 24 */
/* 576 */	NdrFcShort( 0x0 ),	/* 0 */
/* 578 */	NdrFcShort( 0x2 ),	/* 2 */
/* 580 */	NdrFcShort( 0x0 ),	/* 0 */
/* 582 */	NdrFcShort( 0x0 ),	/* 0 */
/* 584 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 586 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 588 */	NdrFcShort( 0x4 ),	/* 4 */
/* 590 */	NdrFcShort( 0x4 ),	/* 4 */
/* 592 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 594 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 596 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 598 */	0x0,		/* 0 */
            NdrFcShort( 0xffb7 ),	/* Offset= -73 (526) */
            0x5b,		/* FC_END */
/* 602 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 604 */	NdrFcShort( 0x8 ),	/* 8 */
/* 606 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 608 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 610 */	NdrFcShort( 0x4 ),	/* 4 */
/* 612 */	NdrFcShort( 0x4 ),	/* 4 */
/* 614 */	0x12, 0x0,	/* FC_UP */
/* 616 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (560) */
/* 618 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 620 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 622 */	
            0x12, 0x0,	/* FC_UP */
/* 624 */	NdrFcShort( 0x62 ),	/* Offset= 98 (722) */
/* 626 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 628 */	NdrFcShort( 0x1c ),	/* 28 */
/* 630 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 632 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 634 */	NdrFcShort( 0x0 ),	/* 0 */
/* 636 */	NdrFcShort( 0x0 ),	/* 0 */
/* 638 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 640 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 642 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 644 */	NdrFcShort( 0x4 ),	/* 4 */
/* 646 */	NdrFcShort( 0x4 ),	/* 4 */
/* 648 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 650 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 652 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 654 */	NdrFcShort( 0x18 ),	/* 24 */
/* 656 */	NdrFcShort( 0x18 ),	/* 24 */
/* 658 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 660 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 662 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 664 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 666 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 668 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 670 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 672 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 674 */	NdrFcShort( 0x1c ),	/* 28 */
/* 676 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 678 */	NdrFcShort( 0x0 ),	/* 0 */
/* 680 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 682 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 684 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 686 */	NdrFcShort( 0x1c ),	/* 28 */
/* 688 */	NdrFcShort( 0x0 ),	/* 0 */
/* 690 */	NdrFcShort( 0x3 ),	/* 3 */
/* 692 */	NdrFcShort( 0x0 ),	/* 0 */
/* 694 */	NdrFcShort( 0x0 ),	/* 0 */
/* 696 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 698 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 700 */	NdrFcShort( 0x4 ),	/* 4 */
/* 702 */	NdrFcShort( 0x4 ),	/* 4 */
/* 704 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 706 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 708 */	NdrFcShort( 0x18 ),	/* 24 */
/* 710 */	NdrFcShort( 0x18 ),	/* 24 */
/* 712 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 714 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 716 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 718 */	0x0,		/* 0 */
            NdrFcShort( 0xffa3 ),	/* Offset= -93 (626) */
            0x5b,		/* FC_END */
/* 722 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 724 */	NdrFcShort( 0x8 ),	/* 8 */
/* 726 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 728 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 730 */	NdrFcShort( 0x4 ),	/* 4 */
/* 732 */	NdrFcShort( 0x4 ),	/* 4 */
/* 734 */	0x12, 0x0,	/* FC_UP */
/* 736 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (672) */
/* 738 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 740 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 742 */	
            0x12, 0x0,	/* FC_UP */
/* 744 */	NdrFcShort( 0x4c ),	/* Offset= 76 (820) */
/* 746 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 748 */	NdrFcShort( 0x10 ),	/* 16 */
/* 750 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 752 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 754 */	NdrFcShort( 0x0 ),	/* 0 */
/* 756 */	NdrFcShort( 0x0 ),	/* 0 */
/* 758 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 760 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 762 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 764 */	NdrFcShort( 0x4 ),	/* 4 */
/* 766 */	NdrFcShort( 0x4 ),	/* 4 */
/* 768 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 770 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 772 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 774 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 776 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 778 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 780 */	NdrFcShort( 0x10 ),	/* 16 */
/* 782 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 784 */	NdrFcShort( 0x0 ),	/* 0 */
/* 786 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 788 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 790 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 792 */	NdrFcShort( 0x10 ),	/* 16 */
/* 794 */	NdrFcShort( 0x0 ),	/* 0 */
/* 796 */	NdrFcShort( 0x2 ),	/* 2 */
/* 798 */	NdrFcShort( 0x0 ),	/* 0 */
/* 800 */	NdrFcShort( 0x0 ),	/* 0 */
/* 802 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 804 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 806 */	NdrFcShort( 0x4 ),	/* 4 */
/* 808 */	NdrFcShort( 0x4 ),	/* 4 */
/* 810 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 812 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 814 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 816 */	0x0,		/* 0 */
            NdrFcShort( 0xffb9 ),	/* Offset= -71 (746) */
            0x5b,		/* FC_END */
/* 820 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 822 */	NdrFcShort( 0x8 ),	/* 8 */
/* 824 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 826 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 828 */	NdrFcShort( 0x4 ),	/* 4 */
/* 830 */	NdrFcShort( 0x4 ),	/* 4 */
/* 832 */	0x12, 0x0,	/* FC_UP */
/* 834 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (778) */
/* 836 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 838 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 840 */	
            0x12, 0x0,	/* FC_UP */
/* 842 */	NdrFcShort( 0x74 ),	/* Offset= 116 (958) */
/* 844 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 846 */	NdrFcShort( 0x20 ),	/* 32 */
/* 848 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 850 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 852 */	NdrFcShort( 0x0 ),	/* 0 */
/* 854 */	NdrFcShort( 0x0 ),	/* 0 */
/* 856 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 858 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 860 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 862 */	NdrFcShort( 0x4 ),	/* 4 */
/* 864 */	NdrFcShort( 0x4 ),	/* 4 */
/* 866 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 868 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 870 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 872 */	NdrFcShort( 0x18 ),	/* 24 */
/* 874 */	NdrFcShort( 0x18 ),	/* 24 */
/* 876 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 878 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 880 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 882 */	NdrFcShort( 0x1c ),	/* 28 */
/* 884 */	NdrFcShort( 0x1c ),	/* 28 */
/* 886 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 888 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 890 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 892 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 894 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 896 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 898 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 900 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 902 */	NdrFcShort( 0x20 ),	/* 32 */
/* 904 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 906 */	NdrFcShort( 0x0 ),	/* 0 */
/* 908 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 910 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 912 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 914 */	NdrFcShort( 0x20 ),	/* 32 */
/* 916 */	NdrFcShort( 0x0 ),	/* 0 */
/* 918 */	NdrFcShort( 0x4 ),	/* 4 */
/* 920 */	NdrFcShort( 0x0 ),	/* 0 */
/* 922 */	NdrFcShort( 0x0 ),	/* 0 */
/* 924 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 926 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 928 */	NdrFcShort( 0x4 ),	/* 4 */
/* 930 */	NdrFcShort( 0x4 ),	/* 4 */
/* 932 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 934 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 936 */	NdrFcShort( 0x18 ),	/* 24 */
/* 938 */	NdrFcShort( 0x18 ),	/* 24 */
/* 940 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 942 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 944 */	NdrFcShort( 0x1c ),	/* 28 */
/* 946 */	NdrFcShort( 0x1c ),	/* 28 */
/* 948 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 950 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 952 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 954 */	0x0,		/* 0 */
            NdrFcShort( 0xff91 ),	/* Offset= -111 (844) */
            0x5b,		/* FC_END */
/* 958 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 960 */	NdrFcShort( 0x8 ),	/* 8 */
/* 962 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 964 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 966 */	NdrFcShort( 0x4 ),	/* 4 */
/* 968 */	NdrFcShort( 0x4 ),	/* 4 */
/* 970 */	0x12, 0x0,	/* FC_UP */
/* 972 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (900) */
/* 974 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 976 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 978 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 980 */	NdrFcShort( 0x8 ),	/* 8 */
/* 982 */	NdrFcShort( 0x0 ),	/* 0 */
/* 984 */	NdrFcShort( 0x0 ),	/* Offset= 0 (984) */
/* 986 */	0x8,		/* FC_LONG */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 988 */	0x0,		/* 0 */
            NdrFcShort( 0xfdb1 ),	/* Offset= -591 (398) */
            0x5b,		/* FC_END */
/* 992 */	
            0x11, 0x0,	/* FC_RP */
/* 994 */	NdrFcShort( 0x2 ),	/* Offset= 2 (996) */
/* 996 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 998 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 1000 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1002 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1004 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1006) */
/* 1006 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1008 */	NdrFcShort( 0x300a ),	/* 12298 */
/* 1010 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1014 */	NdrFcShort( 0x3a ),	/* Offset= 58 (1072) */
/* 1016 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1020 */	NdrFcShort( 0x38 ),	/* Offset= 56 (1076) */
/* 1022 */	NdrFcLong( 0x2 ),	/* 2 */
/* 1026 */	NdrFcShort( 0x56 ),	/* Offset= 86 (1112) */
/* 1028 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 1032 */	NdrFcShort( 0x8c ),	/* Offset= 140 (1172) */
/* 1034 */	NdrFcLong( 0x3ec ),	/* 1004 */
/* 1038 */	NdrFcShort( 0x22 ),	/* Offset= 34 (1072) */
/* 1040 */	NdrFcLong( 0x3ee ),	/* 1006 */
/* 1044 */	NdrFcShort( 0xfd6e ),	/* Offset= -658 (386) */
/* 1046 */	NdrFcLong( 0x5dd ),	/* 1501 */
/* 1050 */	NdrFcShort( 0xce ),	/* Offset= 206 (1256) */
/* 1052 */	NdrFcLong( 0x3ed ),	/* 1005 */
/* 1056 */	NdrFcShort( 0xfd62 ),	/* Offset= -670 (386) */
/* 1058 */	NdrFcLong( 0x1f5 ),	/* 501 */
/* 1062 */	NdrFcShort( 0xe6 ),	/* Offset= 230 (1292) */
/* 1064 */	NdrFcLong( 0x1f7 ),	/* 503 */
/* 1068 */	NdrFcShort( 0x104 ),	/* Offset= 260 (1328) */
/* 1070 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1070) */
/* 1072 */	
            0x12, 0x0,	/* FC_UP */
/* 1074 */	NdrFcShort( 0xfd8e ),	/* Offset= -626 (448) */
/* 1076 */	
            0x12, 0x0,	/* FC_UP */
/* 1078 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1080) */
/* 1080 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1082 */	NdrFcShort( 0xc ),	/* 12 */
/* 1084 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1086 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1088 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1090 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1092 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1094 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1096 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1098 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1100 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1102 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1104 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1106 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1108 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1110 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1112 */	
            0x12, 0x0,	/* FC_UP */
/* 1114 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1116) */
/* 1116 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1118 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1120 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1122 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1124 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1126 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1128 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1130 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1132 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1134 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1136 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1138 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1140 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1142 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1144 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1146 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1148 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1150 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1152 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1154 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1156 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1158 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1160 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1162 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1164 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1166 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1168 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1170 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1172 */	
            0x12, 0x0,	/* FC_UP */
/* 1174 */	NdrFcShort( 0xe ),	/* Offset= 14 (1188) */
/* 1176 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 1178 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1180 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1182 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1184 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1186 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 1188 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1190 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1192 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1194 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1196 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1198 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1200 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1202 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1204 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1206 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1208 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1210 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1212 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1214 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1216 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1218 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1220 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1222 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1224 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1226 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1228 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1230 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1232 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1234 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1236 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1238 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1240 */	0x12, 0x0,	/* FC_UP */
/* 1242 */	NdrFcShort( 0xffbe ),	/* Offset= -66 (1176) */
/* 1244 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1246 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1248 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1250 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1252 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1254 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1256 */	
            0x12, 0x0,	/* FC_UP */
/* 1258 */	NdrFcShort( 0xe ),	/* Offset= 14 (1272) */
/* 1260 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 1262 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1264 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1266 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1268 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1270 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 1272 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1274 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1276 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1278 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1280 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1282 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1284 */	0x12, 0x0,	/* FC_UP */
/* 1286 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (1260) */
/* 1288 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1290 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1292 */	
            0x12, 0x0,	/* FC_UP */
/* 1294 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1296) */
/* 1296 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1298 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1300 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1302 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1304 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1306 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1308 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1310 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1312 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1314 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1316 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1318 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1320 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1322 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1324 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1326 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1328 */	
            0x12, 0x0,	/* FC_UP */
/* 1330 */	NdrFcShort( 0xe ),	/* Offset= 14 (1344) */
/* 1332 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 1334 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1336 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1338 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1340 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1342 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 1344 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1346 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1348 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1350 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1352 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1354 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1356 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1358 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1360 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1362 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1364 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1366 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1368 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1370 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1372 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1374 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1376 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1378 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1380 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1382 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1384 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1386 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1388 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1390 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1392 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1394 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1396 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1398 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1400 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1402 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1404 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1406 */	0x12, 0x0,	/* FC_UP */
/* 1408 */	NdrFcShort( 0xffb4 ),	/* Offset= -76 (1332) */
/* 1410 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1412 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1414 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1416 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1418 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1420 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1422 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1424 */	
            0x11, 0x0,	/* FC_RP */
/* 1426 */	NdrFcShort( 0x1c8 ),	/* Offset= 456 (1882) */
/* 1428 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1430 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 1432 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 1434 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1436 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1438) */
/* 1438 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1440 */	NdrFcShort( 0x3006 ),	/* 12294 */
/* 1442 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1446 */	NdrFcShort( 0xfc16 ),	/* Offset= -1002 (444) */
/* 1448 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1452 */	NdrFcShort( 0x1c ),	/* Offset= 28 (1480) */
/* 1454 */	NdrFcLong( 0x2 ),	/* 2 */
/* 1458 */	NdrFcShort( 0x58 ),	/* Offset= 88 (1546) */
/* 1460 */	NdrFcLong( 0x1f5 ),	/* 501 */
/* 1464 */	NdrFcShort( 0xa4 ),	/* Offset= 164 (1628) */
/* 1466 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 1470 */	NdrFcShort( 0xe0 ),	/* Offset= 224 (1694) */
/* 1472 */	NdrFcLong( 0x1f7 ),	/* 503 */
/* 1476 */	NdrFcShort( 0x134 ),	/* Offset= 308 (1784) */
/* 1478 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1477) */
/* 1480 */	
            0x12, 0x0,	/* FC_UP */
/* 1482 */	NdrFcShort( 0x2c ),	/* Offset= 44 (1526) */
/* 1484 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 1486 */	NdrFcShort( 0xc ),	/* 12 */
/* 1488 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1490 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1492 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1494 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1496 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 1498 */	NdrFcShort( 0xc ),	/* 12 */
/* 1500 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1502 */	NdrFcShort( 0x2 ),	/* 2 */
/* 1504 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1506 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1508 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1510 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1512 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1514 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1516 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1518 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1520 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1522 */	0x0,		/* 0 */
            NdrFcShort( 0xfe45 ),	/* Offset= -443 (1080) */
            0x5b,		/* FC_END */
/* 1526 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1528 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1530 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1532 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1534 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1536 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1538 */	0x12, 0x0,	/* FC_UP */
/* 1540 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (1484) */
/* 1542 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1544 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1546 */	
            0x12, 0x0,	/* FC_UP */
/* 1548 */	NdrFcShort( 0x3c ),	/* Offset= 60 (1608) */
/* 1550 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 1552 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1554 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1556 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1558 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1560 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1562 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 1564 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1566 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1568 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1570 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1572 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1574 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1576 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1578 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1580 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1582 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1584 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1586 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1588 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1590 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1592 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1594 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1596 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1598 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1600 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1602 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1604 */	0x0,		/* 0 */
            NdrFcShort( 0xfe17 ),	/* Offset= -489 (1116) */
            0x5b,		/* FC_END */
/* 1608 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1610 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1612 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1614 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1616 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1618 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1620 */	0x12, 0x0,	/* FC_UP */
/* 1622 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (1550) */
/* 1624 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1626 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1628 */	
            0x12, 0x0,	/* FC_UP */
/* 1630 */	NdrFcShort( 0x2c ),	/* Offset= 44 (1674) */
/* 1632 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 1634 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1636 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1638 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1640 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1642 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1644 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 1646 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1648 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1650 */	NdrFcShort( 0x2 ),	/* 2 */
/* 1652 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1654 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1656 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1658 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1660 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1662 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1664 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1666 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1668 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1670 */	0x0,		/* 0 */
            NdrFcShort( 0xfe89 ),	/* Offset= -375 (1296) */
            0x5b,		/* FC_END */
/* 1674 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1676 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1678 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1680 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1682 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1684 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1686 */	0x12, 0x0,	/* FC_UP */
/* 1688 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (1632) */
/* 1690 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1692 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1694 */	
            0x12, 0x0,	/* FC_UP */
/* 1696 */	NdrFcShort( 0x44 ),	/* Offset= 68 (1764) */
/* 1698 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 1700 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1702 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1704 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1706 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1708 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1710 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 1712 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1714 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1716 */	NdrFcShort( 0x5 ),	/* 5 */
/* 1718 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1720 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1722 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1724 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1726 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1728 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1730 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1732 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1734 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1736 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1738 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1740 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1742 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1744 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1746 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1748 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1750 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1752 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1754 */	0x12, 0x0,	/* FC_UP */
/* 1756 */	NdrFcShort( 0xfdbc ),	/* Offset= -580 (1176) */
/* 1758 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1760 */	0x0,		/* 0 */
            NdrFcShort( 0xfdc3 ),	/* Offset= -573 (1188) */
            0x5b,		/* FC_END */
/* 1764 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1766 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1768 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1770 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1772 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1774 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1776 */	0x12, 0x0,	/* FC_UP */
/* 1778 */	NdrFcShort( 0xffb0 ),	/* Offset= -80 (1698) */
/* 1780 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1782 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1784 */	
            0x12, 0x0,	/* FC_UP */
/* 1786 */	NdrFcShort( 0x4c ),	/* Offset= 76 (1862) */
/* 1788 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 1790 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1792 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1794 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1796 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1798 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1800 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 1802 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1804 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1806 */	NdrFcShort( 0x6 ),	/* 6 */
/* 1808 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1810 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1812 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1814 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1816 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1818 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1820 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1822 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1824 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1826 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1828 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1830 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1832 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1834 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1836 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1838 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1840 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1842 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1844 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1846 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1848 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1850 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1852 */	0x12, 0x0,	/* FC_UP */
/* 1854 */	NdrFcShort( 0xfdf6 ),	/* Offset= -522 (1332) */
/* 1856 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1858 */	0x0,		/* 0 */
            NdrFcShort( 0xfdfd ),	/* Offset= -515 (1344) */
            0x5b,		/* FC_END */
/* 1862 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 1864 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1866 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 1868 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 1870 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1872 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1874 */	0x12, 0x0,	/* FC_UP */
/* 1876 */	NdrFcShort( 0xffa8 ),	/* Offset= -88 (1788) */
/* 1878 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 1880 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 1882 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1884 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1886 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1888 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1888) */
/* 1890 */	0x8,		/* FC_LONG */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 1892 */	0x0,		/* 0 */
            NdrFcShort( 0xfe2f ),	/* Offset= -465 (1428) */
            0x5b,		/* FC_END */
/* 1896 */	
            0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 1898 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1900 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 1902 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1904) */
/* 1904 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1906 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 1908 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1910 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1912 */	NdrFcShort( 0xfc76 ),	/* Offset= -906 (1006) */
/* 1914 */	
            0x11, 0x0,	/* FC_RP */
/* 1916 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1918) */
/* 1918 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1920 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 1922 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 1924 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1926 */	NdrFcShort( 0xfc68 ),	/* Offset= -920 (1006) */
/* 1928 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 1930 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1932) */
/* 1932 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1934 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 1936 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 1938 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1940 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1942) */
/* 1942 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1944 */	NdrFcShort( 0x3033 ),	/* 12339 */
/* 1946 */	NdrFcLong( 0x64 ),	/* 100 */
/* 1950 */	NdrFcShort( 0x130 ),	/* Offset= 304 (2254) */
/* 1952 */	NdrFcLong( 0x65 ),	/* 101 */
/* 1956 */	NdrFcShort( 0x142 ),	/* Offset= 322 (2278) */
/* 1958 */	NdrFcLong( 0x66 ),	/* 102 */
/* 1962 */	NdrFcShort( 0x162 ),	/* Offset= 354 (2316) */
/* 1964 */	NdrFcLong( 0x67 ),	/* 103 */
/* 1968 */	NdrFcShort( 0x194 ),	/* Offset= 404 (2372) */
/* 1970 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 1974 */	NdrFcShort( 0x1c6 ),	/* Offset= 454 (2428) */
/* 1976 */	NdrFcLong( 0x1f7 ),	/* 503 */
/* 1980 */	NdrFcShort( 0x1dc ),	/* Offset= 476 (2456) */
/* 1982 */	NdrFcLong( 0x257 ),	/* 599 */
/* 1986 */	NdrFcShort( 0x216 ),	/* Offset= 534 (2520) */
/* 1988 */	NdrFcLong( 0x3ed ),	/* 1005 */
/* 1992 */	NdrFcShort( 0xfc68 ),	/* Offset= -920 (1072) */
/* 1994 */	NdrFcLong( 0x453 ),	/* 1107 */
/* 1998 */	NdrFcShort( 0xf9b4 ),	/* Offset= -1612 (386) */
/* 2000 */	NdrFcLong( 0x3f2 ),	/* 1010 */
/* 2004 */	NdrFcShort( 0xf9ae ),	/* Offset= -1618 (386) */
/* 2006 */	NdrFcLong( 0x3f8 ),	/* 1016 */
/* 2010 */	NdrFcShort( 0xf9a8 ),	/* Offset= -1624 (386) */
/* 2012 */	NdrFcLong( 0x3f9 ),	/* 1017 */
/* 2016 */	NdrFcShort( 0xf9a2 ),	/* Offset= -1630 (386) */
/* 2018 */	NdrFcLong( 0x3fa ),	/* 1018 */
/* 2022 */	NdrFcShort( 0xf99c ),	/* Offset= -1636 (386) */
/* 2024 */	NdrFcLong( 0x5dd ),	/* 1501 */
/* 2028 */	NdrFcShort( 0xf996 ),	/* Offset= -1642 (386) */
/* 2030 */	NdrFcLong( 0x5de ),	/* 1502 */
/* 2034 */	NdrFcShort( 0xf990 ),	/* Offset= -1648 (386) */
/* 2036 */	NdrFcLong( 0x5df ),	/* 1503 */
/* 2040 */	NdrFcShort( 0xf98a ),	/* Offset= -1654 (386) */
/* 2042 */	NdrFcLong( 0x5e2 ),	/* 1506 */
/* 2046 */	NdrFcShort( 0xf984 ),	/* Offset= -1660 (386) */
/* 2048 */	NdrFcLong( 0x5e6 ),	/* 1510 */
/* 2052 */	NdrFcShort( 0xf97e ),	/* Offset= -1666 (386) */
/* 2054 */	NdrFcLong( 0x5e7 ),	/* 1511 */
/* 2058 */	NdrFcShort( 0xf978 ),	/* Offset= -1672 (386) */
/* 2060 */	NdrFcLong( 0x5e8 ),	/* 1512 */
/* 2064 */	NdrFcShort( 0xf972 ),	/* Offset= -1678 (386) */
/* 2066 */	NdrFcLong( 0x5e9 ),	/* 1513 */
/* 2070 */	NdrFcShort( 0xf96c ),	/* Offset= -1684 (386) */
/* 2072 */	NdrFcLong( 0x5ea ),	/* 1514 */
/* 2076 */	NdrFcShort( 0xf966 ),	/* Offset= -1690 (386) */
/* 2078 */	NdrFcLong( 0x5eb ),	/* 1515 */
/* 2082 */	NdrFcShort( 0xf960 ),	/* Offset= -1696 (386) */
/* 2084 */	NdrFcLong( 0x5ec ),	/* 1516 */
/* 2088 */	NdrFcShort( 0xf95a ),	/* Offset= -1702 (386) */
/* 2090 */	NdrFcLong( 0x5ee ),	/* 1518 */
/* 2094 */	NdrFcShort( 0xf954 ),	/* Offset= -1708 (386) */
/* 2096 */	NdrFcLong( 0x5f3 ),	/* 1523 */
/* 2100 */	NdrFcShort( 0xf94e ),	/* Offset= -1714 (386) */
/* 2102 */	NdrFcLong( 0x5f8 ),	/* 1528 */
/* 2106 */	NdrFcShort( 0xf948 ),	/* Offset= -1720 (386) */
/* 2108 */	NdrFcLong( 0x5f9 ),	/* 1529 */
/* 2112 */	NdrFcShort( 0xf942 ),	/* Offset= -1726 (386) */
/* 2114 */	NdrFcLong( 0x5fa ),	/* 1530 */
/* 2118 */	NdrFcShort( 0xf93c ),	/* Offset= -1732 (386) */
/* 2120 */	NdrFcLong( 0x5fd ),	/* 1533 */
/* 2124 */	NdrFcShort( 0xf936 ),	/* Offset= -1738 (386) */
/* 2126 */	NdrFcLong( 0x5fe ),	/* 1534 */
/* 2130 */	NdrFcShort( 0xf930 ),	/* Offset= -1744 (386) */
/* 2132 */	NdrFcLong( 0x5ff ),	/* 1535 */
/* 2136 */	NdrFcShort( 0xf92a ),	/* Offset= -1750 (386) */
/* 2138 */	NdrFcLong( 0x600 ),	/* 1536 */
/* 2142 */	NdrFcShort( 0xf924 ),	/* Offset= -1756 (386) */
/* 2144 */	NdrFcLong( 0x602 ),	/* 1538 */
/* 2148 */	NdrFcShort( 0xf91e ),	/* Offset= -1762 (386) */
/* 2150 */	NdrFcLong( 0x603 ),	/* 1539 */
/* 2154 */	NdrFcShort( 0xf918 ),	/* Offset= -1768 (386) */
/* 2156 */	NdrFcLong( 0x604 ),	/* 1540 */
/* 2160 */	NdrFcShort( 0xf912 ),	/* Offset= -1774 (386) */
/* 2162 */	NdrFcLong( 0x605 ),	/* 1541 */
/* 2166 */	NdrFcShort( 0xf90c ),	/* Offset= -1780 (386) */
/* 2168 */	NdrFcLong( 0x606 ),	/* 1542 */
/* 2172 */	NdrFcShort( 0xf906 ),	/* Offset= -1786 (386) */
/* 2174 */	NdrFcLong( 0x607 ),	/* 1543 */
/* 2178 */	NdrFcShort( 0xf900 ),	/* Offset= -1792 (386) */
/* 2180 */	NdrFcLong( 0x608 ),	/* 1544 */
/* 2184 */	NdrFcShort( 0xf8fa ),	/* Offset= -1798 (386) */
/* 2186 */	NdrFcLong( 0x609 ),	/* 1545 */
/* 2190 */	NdrFcShort( 0xf8f4 ),	/* Offset= -1804 (386) */
/* 2192 */	NdrFcLong( 0x60a ),	/* 1546 */
/* 2196 */	NdrFcShort( 0xf8ee ),	/* Offset= -1810 (386) */
/* 2198 */	NdrFcLong( 0x60b ),	/* 1547 */
/* 2202 */	NdrFcShort( 0xf8e8 ),	/* Offset= -1816 (386) */
/* 2204 */	NdrFcLong( 0x60c ),	/* 1548 */
/* 2208 */	NdrFcShort( 0xf8e2 ),	/* Offset= -1822 (386) */
/* 2210 */	NdrFcLong( 0x60d ),	/* 1549 */
/* 2214 */	NdrFcShort( 0xf8dc ),	/* Offset= -1828 (386) */
/* 2216 */	NdrFcLong( 0x60e ),	/* 1550 */
/* 2220 */	NdrFcShort( 0xf8d6 ),	/* Offset= -1834 (386) */
/* 2222 */	NdrFcLong( 0x610 ),	/* 1552 */
/* 2226 */	NdrFcShort( 0xf8d0 ),	/* Offset= -1840 (386) */
/* 2228 */	NdrFcLong( 0x611 ),	/* 1553 */
/* 2232 */	NdrFcShort( 0xf8ca ),	/* Offset= -1846 (386) */
/* 2234 */	NdrFcLong( 0x612 ),	/* 1554 */
/* 2238 */	NdrFcShort( 0xf8c4 ),	/* Offset= -1852 (386) */
/* 2240 */	NdrFcLong( 0x613 ),	/* 1555 */
/* 2244 */	NdrFcShort( 0xf8be ),	/* Offset= -1858 (386) */
/* 2246 */	NdrFcLong( 0x614 ),	/* 1556 */
/* 2250 */	NdrFcShort( 0xf8b8 ),	/* Offset= -1864 (386) */
/* 2252 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2251) */
/* 2254 */	
            0x12, 0x0,	/* FC_UP */
/* 2256 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2258) */
/* 2258 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2260 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2262 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2264 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2266 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2268 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2270 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2272 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2274 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2276 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2278 */	
            0x12, 0x0,	/* FC_UP */
/* 2280 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2282) */
/* 2282 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2284 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2286 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2288 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2290 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2292 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2294 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2296 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2298 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2300 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2302 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2304 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2306 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2308 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2310 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2312 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2314 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2316 */	
            0x12, 0x0,	/* FC_UP */
/* 2318 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2320) */
/* 2320 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2322 */	NdrFcShort( 0x34 ),	/* 52 */
/* 2324 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2326 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2328 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2330 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2332 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2334 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2336 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2338 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2340 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2342 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2344 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2346 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2348 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2350 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2352 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2354 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2356 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2358 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2360 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2362 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2364 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2366 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2368 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2370 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2372 */	
            0x12, 0x0,	/* FC_UP */
/* 2374 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2376) */
/* 2376 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2378 */	NdrFcShort( 0x38 ),	/* 56 */
/* 2380 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2382 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2384 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2386 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2388 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2390 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2392 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2394 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2396 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2398 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2400 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2402 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2404 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2406 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2408 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2410 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2412 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2414 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2416 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2418 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2420 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2422 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2424 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2426 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2428 */	
            0x12, 0x0,	/* FC_UP */
/* 2430 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2432) */
/* 2432 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 2434 */	NdrFcShort( 0x48 ),	/* 72 */
/* 2436 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2438 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2440 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2442 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2444 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2446 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2448 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2450 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2452 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2454 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2456 */	
            0x12, 0x0,	/* FC_UP */
/* 2458 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2460) */
/* 2460 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2462 */	NdrFcShort( 0xa8 ),	/* 168 */
/* 2464 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2466 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2468 */	NdrFcShort( 0x48 ),	/* 72 */
/* 2470 */	NdrFcShort( 0x48 ),	/* 72 */
/* 2472 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2474 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2476 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2478 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2480 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2482 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2484 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2486 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2488 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2490 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2492 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2494 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2496 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2498 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2500 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2502 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2504 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2506 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2508 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2510 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2512 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2514 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2516 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2518 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2520 */	
            0x12, 0x0,	/* FC_UP */
/* 2522 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2524) */
/* 2524 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2526 */	NdrFcShort( 0xe0 ),	/* 224 */
/* 2528 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2530 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2532 */	NdrFcShort( 0x48 ),	/* 72 */
/* 2534 */	NdrFcShort( 0x48 ),	/* 72 */
/* 2536 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2538 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2540 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2542 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2544 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2546 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2548 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2550 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2552 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2554 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2556 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2558 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2560 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2562 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2564 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2566 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2568 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2570 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2572 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2574 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2576 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2578 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2580 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2582 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2584 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2586 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2588 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2590 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2592 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2594 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2596 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2598 */	
            0x11, 0x0,	/* FC_RP */
/* 2600 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2602) */
/* 2602 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 2604 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2606 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 2608 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2610 */	NdrFcShort( 0xfd64 ),	/* Offset= -668 (1942) */
/* 2612 */	
            0x11, 0x0,	/* FC_RP */
/* 2614 */	NdrFcShort( 0x2a ),	/* Offset= 42 (2656) */
/* 2616 */	
            0x29,		/* FC_WSTRING */
            0x5c,		/* FC_PAD */
/* 2618 */	NdrFcShort( 0x3 ),	/* 3 */
/* 2620 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x1,		/* 1 */
/* 2622 */	NdrFcShort( 0x6 ),	/* 6 */
/* 2624 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2626 */	NdrFcShort( 0x0 ),	/* Offset= 0 (2626) */
/* 2628 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2630 */	NdrFcShort( 0xfff2 ),	/* Offset= -14 (2616) */
/* 2632 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2634 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x1,		/* 1 */
/* 2636 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2638 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2640 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2642 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2644 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2646 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2648 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2650 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2652 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (2620) */
/* 2654 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2656 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2658 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2660 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2662 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2664 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2666 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2668 */	0x12, 0x0,	/* FC_UP */
/* 2670 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2634) */
/* 2672 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2674 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2676 */	
            0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 2678 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2680) */
/* 2680 */	
            0x12, 0x0,	/* FC_UP */
/* 2682 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2684) */
/* 2684 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 2686 */	NdrFcShort( 0x44 ),	/* 68 */
/* 2688 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2690 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2692 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2694 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2696 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2698 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2700 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2702 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2704 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2706 */	
            0x11, 0x0,	/* FC_RP */
/* 2708 */	NdrFcShort( 0xe ),	/* Offset= 14 (2722) */
/* 2710 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 2712 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2714 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2716 */	NdrFcShort( 0xc ),	/* 12 */
/* 2718 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2720 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 2722 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2724 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2726 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2728 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2730 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2732 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2734 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2736 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2738 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2740 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2742 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2744 */	0x12, 0x0,	/* FC_UP */
/* 2746 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2710) */
/* 2748 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2750 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2752 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2754 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2756 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2758 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2760 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2762 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2764 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2766 */	
            0x11, 0x0,	/* FC_RP */
/* 2768 */	NdrFcShort( 0x21a ),	/* Offset= 538 (3306) */
/* 2770 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 2772 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 2774 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 2776 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2778 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2780) */
/* 2780 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2782 */	NdrFcShort( 0x3004 ),	/* 12292 */
/* 2784 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2788 */	NdrFcShort( 0x16 ),	/* Offset= 22 (2810) */
/* 2790 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2794 */	NdrFcShort( 0x5a ),	/* Offset= 90 (2884) */
/* 2796 */	NdrFcLong( 0x2 ),	/* 2 */
/* 2800 */	NdrFcShort( 0xdc ),	/* Offset= 220 (3020) */
/* 2802 */	NdrFcLong( 0x3 ),	/* 3 */
/* 2806 */	NdrFcShort( 0x160 ),	/* Offset= 352 (3158) */
/* 2808 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2807) */
/* 2810 */	
            0x12, 0x0,	/* FC_UP */
/* 2812 */	NdrFcShort( 0x34 ),	/* Offset= 52 (2864) */
/* 2814 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 2816 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2818 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2820 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2822 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2824 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2826 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 2828 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2830 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2832 */	NdrFcShort( 0x3 ),	/* 3 */
/* 2834 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2836 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2838 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2840 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2842 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2844 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2846 */	0x12, 0x0,	/* FC_UP */
/* 2848 */	NdrFcShort( 0xff76 ),	/* Offset= -138 (2710) */
/* 2850 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2852 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2854 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2856 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2858 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2860 */	0x0,		/* 0 */
            NdrFcShort( 0xff75 ),	/* Offset= -139 (2722) */
            0x5b,		/* FC_END */
/* 2864 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2866 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2868 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2870 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2872 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2874 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2876 */	0x12, 0x0,	/* FC_UP */
/* 2878 */	NdrFcShort( 0xffc0 ),	/* Offset= -64 (2814) */
/* 2880 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2882 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2884 */	
            0x12, 0x0,	/* FC_UP */
/* 2886 */	NdrFcShort( 0x72 ),	/* Offset= 114 (3000) */
/* 2888 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 2890 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2892 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2894 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2896 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2898 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2900 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2902 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2904 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2906 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2908 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2910 */	0x12, 0x0,	/* FC_UP */
/* 2912 */	NdrFcShort( 0xff36 ),	/* Offset= -202 (2710) */
/* 2914 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2916 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2918 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2920 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2922 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2924 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 2926 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2928 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2930 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2932 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2934 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 2936 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2938 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2940 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2942 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 2944 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2946 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2948 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2950 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2952 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 2954 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 2956 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2958 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2960 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2962 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2964 */	NdrFcShort( 0x4 ),	/* 4 */
/* 2966 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2968 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2970 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2972 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2974 */	0x12, 0x0,	/* FC_UP */
/* 2976 */	NdrFcShort( 0xfef6 ),	/* Offset= -266 (2710) */
/* 2978 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2980 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2982 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2984 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2986 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2988 */	NdrFcShort( 0x14 ),	/* 20 */
/* 2990 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2992 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2994 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2996 */	0x0,		/* 0 */
            NdrFcShort( 0xff93 ),	/* Offset= -109 (2888) */
            0x5b,		/* FC_END */
/* 3000 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3002 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3004 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3006 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3008 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3010 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3012 */	0x12, 0x0,	/* FC_UP */
/* 3014 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (2942) */
/* 3016 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3018 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 3020 */	
            0x12, 0x0,	/* FC_UP */
/* 3022 */	NdrFcShort( 0x74 ),	/* Offset= 116 (3138) */
/* 3024 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3026 */	NdrFcShort( 0x1c ),	/* 28 */
/* 3028 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3030 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3032 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3034 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3036 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3038 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3040 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3042 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3044 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3046 */	0x12, 0x0,	/* FC_UP */
/* 3048 */	NdrFcShort( 0xfeae ),	/* Offset= -338 (2710) */
/* 3050 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3052 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3054 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3056 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3058 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3060 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3062 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3064 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3066 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3068 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3070 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3072 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3074 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3076 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3078 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3080 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 3082 */	NdrFcShort( 0x1c ),	/* 28 */
/* 3084 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 3086 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3088 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3090 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3092 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 3094 */	NdrFcShort( 0x1c ),	/* 28 */
/* 3096 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3098 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3100 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3102 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3104 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3106 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3108 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3110 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3112 */	0x12, 0x0,	/* FC_UP */
/* 3114 */	NdrFcShort( 0xfe6c ),	/* Offset= -404 (2710) */
/* 3116 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3118 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3120 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3122 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3124 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3126 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3128 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3130 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3132 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3134 */	0x0,		/* 0 */
            NdrFcShort( 0xff91 ),	/* Offset= -111 (3024) */
            0x5b,		/* FC_END */
/* 3138 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3140 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3142 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3144 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3146 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3148 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3150 */	0x12, 0x0,	/* FC_UP */
/* 3152 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (3080) */
/* 3154 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3156 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 3158 */	
            0x12, 0x0,	/* FC_UP */
/* 3160 */	NdrFcShort( 0x7e ),	/* Offset= 126 (3286) */
/* 3162 */	
            0x1d,		/* FC_SMFARRAY */
            0x0,		/* 0 */
/* 3164 */	NdrFcShort( 0x100 ),	/* 256 */
/* 3166 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 3168 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3170 */	NdrFcShort( 0x120 ),	/* 288 */
/* 3172 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3174 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3176 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3178 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3180 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3182 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3184 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3186 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3188 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3190 */	0x12, 0x0,	/* FC_UP */
/* 3192 */	NdrFcShort( 0xfe1e ),	/* Offset= -482 (2710) */
/* 3194 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3196 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3198 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3200 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3202 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3204 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3206 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3208 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3210 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3212 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3214 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3216 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3218 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3220 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3222 */	0x8,		/* FC_LONG */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3224 */	0x0,		/* 0 */
            NdrFcShort( 0xffc1 ),	/* Offset= -63 (3162) */
            0x5b,		/* FC_END */
/* 3228 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 3230 */	NdrFcShort( 0x120 ),	/* 288 */
/* 3232 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 3234 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3236 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3238 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3240 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 3242 */	NdrFcShort( 0x120 ),	/* 288 */
/* 3244 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3246 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3248 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3250 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3252 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3254 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3256 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3258 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3260 */	0x12, 0x0,	/* FC_UP */
/* 3262 */	NdrFcShort( 0xfdd8 ),	/* Offset= -552 (2710) */
/* 3264 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3266 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3268 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3270 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3272 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3274 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3276 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3278 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3280 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3282 */	0x0,		/* 0 */
            NdrFcShort( 0xff8d ),	/* Offset= -115 (3168) */
            0x5b,		/* FC_END */
/* 3286 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3288 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3290 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3292 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3294 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3296 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3298 */	0x12, 0x0,	/* FC_UP */
/* 3300 */	NdrFcShort( 0xffb8 ),	/* Offset= -72 (3228) */
/* 3302 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3304 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 3306 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 3308 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3310 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3312 */	NdrFcShort( 0x0 ),	/* Offset= 0 (3312) */
/* 3314 */	0x8,		/* FC_LONG */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3316 */	0x0,		/* 0 */
            NdrFcShort( 0xfddd ),	/* Offset= -547 (2770) */
            0x5b,		/* FC_END */
/* 3320 */	
            0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 3322 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3324) */
/* 3324 */	
            0x12, 0x0,	/* FC_UP */
/* 3326 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3328) */
/* 3328 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 3330 */	NdrFcShort( 0x30 ),	/* 48 */
/* 3332 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3334 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3336 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3338 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3340 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3342 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 3344 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3346 */	
            0x11, 0x0,	/* FC_RP */
/* 3348 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3350) */
/* 3350 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 3352 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3354 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3356 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 3358 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3360 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 3362 */	0xb7,		/* FC_RANGE */
            0x8,		/* 8 */
/* 3364 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3368 */	NdrFcLong( 0xfa00 ),	/* 64000 */
/* 3372 */	
            0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 3374 */	0x8,		/* FC_LONG */
            0x5c,		/* FC_PAD */
/* 3376 */	
            0x11, 0x0,	/* FC_RP */
/* 3378 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3380) */
/* 3380 */	
            0x1b,		/* FC_CARRAY */
            0x1,		/* 1 */
/* 3382 */	NdrFcShort( 0x2 ),	/* 2 */
/* 3384 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3386 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 3388 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3390 */	0x5,		/* FC_WCHAR */
            0x5b,		/* FC_END */
/* 3392 */	0xb7,		/* FC_RANGE */
            0x8,		/* 8 */
/* 3394 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3398 */	NdrFcLong( 0xfa00 ),	/* 64000 */
/* 3402 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3404 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3406) */
/* 3406 */	0x30,		/* FC_BIND_CONTEXT */
            0xa0,		/* Ctxt flags:  via ptr, out, */
/* 3408 */	0x0,		/* 0 */
            0x0,		/* 0 */
/* 3410 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 3412 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3414) */
/* 3414 */	0x30,		/* FC_BIND_CONTEXT */
            0xe1,		/* Ctxt flags:  via ptr, in, out, can't be null */
/* 3416 */	0x0,		/* 0 */
            0x0,		/* 0 */
/* 3418 */	
            0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 3420 */	NdrFcShort( 0xf78c ),	/* Offset= -2164 (1256) */
/* 3422 */	
            0x11, 0x0,	/* FC_RP */
/* 3424 */	NdrFcShort( 0xf798 ),	/* Offset= -2152 (1272) */
/* 3426 */	
            0x11, 0x0,	/* FC_RP */
/* 3428 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3430) */
/* 3430 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3432 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3434 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3436 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3438 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3440) */
/* 3440 */	NdrFcShort( 0x120 ),	/* 288 */
/* 3442 */	NdrFcShort( 0x3004 ),	/* 12292 */
/* 3444 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3448 */	NdrFcShort( 0xfd2a ),	/* Offset= -726 (2722) */
/* 3450 */	NdrFcLong( 0x1 ),	/* 1 */
/* 3454 */	NdrFcShort( 0xfdca ),	/* Offset= -566 (2888) */
/* 3456 */	NdrFcLong( 0x2 ),	/* 2 */
/* 3460 */	NdrFcShort( 0xfe4c ),	/* Offset= -436 (3024) */
/* 3462 */	NdrFcLong( 0x3 ),	/* 3 */
/* 3466 */	NdrFcShort( 0xfed6 ),	/* Offset= -298 (3168) */
/* 3468 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3467) */
/* 3470 */	
            0x11, 0x0,	/* FC_RP */
/* 3472 */	NdrFcShort( 0x8 ),	/* Offset= 8 (3480) */
/* 3474 */	
            0x1d,		/* FC_SMFARRAY */
            0x0,		/* 0 */
/* 3476 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3478 */	0x1,		/* FC_BYTE */
            0x5b,		/* FC_END */
/* 3480 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 3482 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3484 */	0x8,		/* FC_LONG */
            0x6,		/* FC_SHORT */
/* 3486 */	0x6,		/* FC_SHORT */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3488 */	0x0,		/* 0 */
            NdrFcShort( 0xfff1 ),	/* Offset= -15 (3474) */
            0x5b,		/* FC_END */
/* 3492 */	
            0x11, 0x0,	/* FC_RP */
/* 3494 */	NdrFcShort( 0x3c ),	/* Offset= 60 (3554) */
/* 3496 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3498 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3500 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3502 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3504 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3506 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3508 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3510 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3512 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3514 */	0x0,		/* 0 */
            NdrFcShort( 0xffdd ),	/* Offset= -35 (3480) */
            0x8,		/* FC_LONG */
/* 3518 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3520 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 3522 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3524 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 3526 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3528 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3530 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3532 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 3534 */	NdrFcShort( 0x14 ),	/* 20 */
/* 3536 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3538 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3540 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3542 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3544 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3546 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3548 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3550 */	0x0,		/* 0 */
            NdrFcShort( 0xffc9 ),	/* Offset= -55 (3496) */
            0x5b,		/* FC_END */
/* 3554 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3556 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3558 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3560 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3562 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3564 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3566 */	0x12, 0x0,	/* FC_UP */
/* 3568 */	NdrFcShort( 0xffd0 ),	/* Offset= -48 (3520) */
/* 3570 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3572 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 3574 */	0xb7,		/* FC_RANGE */
            0x8,		/* 8 */
/* 3576 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3580 */	NdrFcLong( 0x20 ),	/* 32 */
/* 3584 */	
            0x11, 0x0,	/* FC_RP */
/* 3586 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3588) */
/* 3588 */	
            0x1b,		/* FC_CARRAY */
            0x1,		/* 1 */
/* 3590 */	NdrFcShort( 0x2 ),	/* 2 */
/* 3592 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3594 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 3596 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3598 */	0x5,		/* FC_WCHAR */
            0x5b,		/* FC_END */
/* 3600 */	
            0x12, 0x14,	/* FC_UP [alloced_on_stack] [pointer_deref] */
/* 3602 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3604) */
/* 3604 */	
            0x12, 0x0,	/* FC_UP */
/* 3606 */	NdrFcShort( 0x12 ),	/* Offset= 18 (3624) */
/* 3608 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 3610 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3612 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 3614 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 3616 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3618 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 3620 */	NdrFcShort( 0xfaae ),	/* Offset= -1362 (2258) */
/* 3622 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3624 */	
            0x18,		/* FC_CPSTRUCT */
            0x3,		/* 3 */
/* 3626 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3628 */	NdrFcShort( 0xffec ),	/* Offset= -20 (3608) */
/* 3630 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3632 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 3634 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3636 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3638 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3640 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3642 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3644 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3646 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3648 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3650 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3652 */	
            0x11, 0x0,	/* FC_RP */
/* 3654 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3656) */
/* 3656 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3658 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3660 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3662 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3664 */	NdrFcShort( 0xff20 ),	/* Offset= -224 (3440) */
/* 3666 */	
            0x11, 0x0,	/* FC_RP */
/* 3668 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3670) */
/* 3670 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3672 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3674 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3676 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3678 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3680) */
/* 3680 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3682 */	NdrFcShort( 0x3001 ),	/* 12289 */
/* 3684 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3688 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3692) */
/* 3690 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3689) */
/* 3692 */	
            0x12, 0x0,	/* FC_UP */
/* 3694 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3696) */
/* 3696 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3698 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3700 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3702 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3704 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3706 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3708 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3710 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3712 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3714 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3716 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3718 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3720 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3722 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3724 */	0x8,		/* FC_LONG */
            0x2,		/* FC_CHAR */
/* 3726 */	0x3f,		/* FC_STRUCTPAD3 */
            0x8,		/* FC_LONG */
/* 3728 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3730 */	
            0x11, 0x0,	/* FC_RP */
/* 3732 */	NdrFcShort( 0x5a ),	/* Offset= 90 (3822) */
/* 3734 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3736 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 3738 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 3740 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3742 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3744) */
/* 3744 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3746 */	NdrFcShort( 0x3001 ),	/* 12289 */
/* 3748 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3752 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3756) */
/* 3754 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3753) */
/* 3756 */	
            0x12, 0x0,	/* FC_UP */
/* 3758 */	NdrFcShort( 0x2c ),	/* Offset= 44 (3802) */
/* 3760 */	
            0x1b,		/* FC_CARRAY */
            0x3,		/* 3 */
/* 3762 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3764 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 3766 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3768 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3770 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3772 */	
            0x48,		/* FC_VARIABLE_REPEAT */
            0x49,		/* FC_FIXED_OFFSET */
/* 3774 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3776 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3778 */	NdrFcShort( 0x2 ),	/* 2 */
/* 3780 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3782 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3784 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3786 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3788 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3790 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3792 */	0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3794 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3796 */	
            0x5b,		/* FC_END */

            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3798 */	0x0,		/* 0 */
            NdrFcShort( 0xff99 ),	/* Offset= -103 (3696) */
            0x5b,		/* FC_END */
/* 3802 */	
            0x16,		/* FC_PSTRUCT */
            0x3,		/* 3 */
/* 3804 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3806 */	
            0x4b,		/* FC_PP */
            0x5c,		/* FC_PAD */
/* 3808 */	
            0x46,		/* FC_NO_REPEAT */
            0x5c,		/* FC_PAD */
/* 3810 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3812 */	NdrFcShort( 0x4 ),	/* 4 */
/* 3814 */	0x12, 0x0,	/* FC_UP */
/* 3816 */	NdrFcShort( 0xffc8 ),	/* Offset= -56 (3760) */
/* 3818 */	
            0x5b,		/* FC_END */

            0x8,		/* FC_LONG */
/* 3820 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 3822 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 3824 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3826 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3828 */	NdrFcShort( 0x0 ),	/* Offset= 0 (3828) */
/* 3830 */	0x8,		/* FC_LONG */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 3832 */	0x0,		/* 0 */
            NdrFcShort( 0xff9d ),	/* Offset= -99 (3734) */
            0x5b,		/* FC_END */
/* 3836 */	
            0x11, 0x0,	/* FC_RP */
/* 3838 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3840) */
/* 3840 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3842 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3844 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3846 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3848 */	NdrFcShort( 0xff58 ),	/* Offset= -168 (3680) */
/* 3850 */	
            0x11, 0x0,	/* FC_RP */
/* 3852 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3854) */
/* 3854 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3856 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3858 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 3860 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3862 */	NdrFcShort( 0xf4d8 ),	/* Offset= -2856 (1006) */

            0x0";

        internal static readonly ushort[] ProcFormatStringOffsetTableX86 = new ushort[]
        {
            0,
            28,
            56,
            84,
            112,
            140,
            168,
            196,
            224,
            296,
            374,
            434,
            482,
            560,
            614,
            674,
            740,
            800,
            866,
            920,
            974,
            1028,
            1082,
            1142,
            1214,
            1280,
            1334,
            1400,
            1454,
            1502,
            1530,
            1590,
            1668,
            1734,
            1794,
            1866,
            1932,
            1998,
            2058,
            2100,
            2166,
            2232,
            2286,
            2314,
            2362,
            2440,
            2494,
            2554,
            2582,
            2654,
            2714,
            2768,
            2858,
            2906,
            2960,
            3014,
            3080,
            3134
        };

        private const string PROC_FORMAT_STRING_X64 = @"
    /* Procedure Opnum0NotUsedOnWire */

            0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x0 ),	/* 0 */
/*  8 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 10 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 12 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 14 */	NdrFcShort( 0x0 ),	/* 0 */
/* 16 */	NdrFcShort( 0x0 ),	/* 0 */
/* 18 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 20 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 22 */	NdrFcShort( 0x0 ),	/* 0 */
/* 24 */	NdrFcShort( 0x0 ),	/* 0 */
/* 26 */	NdrFcShort( 0x0 ),	/* 0 */
/* 28 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum1NotUsedOnWire */


    /* Parameter IDL_handle */

/* 30 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 32 */	NdrFcLong( 0x0 ),	/* 0 */
/* 36 */	NdrFcShort( 0x1 ),	/* 1 */
/* 38 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 40 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 42 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 44 */	NdrFcShort( 0x0 ),	/* 0 */
/* 46 */	NdrFcShort( 0x0 ),	/* 0 */
/* 48 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 50 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 52 */	NdrFcShort( 0x0 ),	/* 0 */
/* 54 */	NdrFcShort( 0x0 ),	/* 0 */
/* 56 */	NdrFcShort( 0x0 ),	/* 0 */
/* 58 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum2NotUsedOnWire */


    /* Parameter IDL_handle */

/* 60 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 62 */	NdrFcLong( 0x0 ),	/* 0 */
/* 66 */	NdrFcShort( 0x2 ),	/* 2 */
/* 68 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 70 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 72 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 74 */	NdrFcShort( 0x0 ),	/* 0 */
/* 76 */	NdrFcShort( 0x0 ),	/* 0 */
/* 78 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 80 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 82 */	NdrFcShort( 0x0 ),	/* 0 */
/* 84 */	NdrFcShort( 0x0 ),	/* 0 */
/* 86 */	NdrFcShort( 0x0 ),	/* 0 */
/* 88 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum3NotUsedOnWire */


    /* Parameter IDL_handle */

/* 90 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 92 */	NdrFcLong( 0x0 ),	/* 0 */
/* 96 */	NdrFcShort( 0x3 ),	/* 3 */
/* 98 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 100 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 102 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 104 */	NdrFcShort( 0x0 ),	/* 0 */
/* 106 */	NdrFcShort( 0x0 ),	/* 0 */
/* 108 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 110 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 112 */	NdrFcShort( 0x0 ),	/* 0 */
/* 114 */	NdrFcShort( 0x0 ),	/* 0 */
/* 116 */	NdrFcShort( 0x0 ),	/* 0 */
/* 118 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum4NotUsedOnWire */


    /* Parameter IDL_handle */

/* 120 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 122 */	NdrFcLong( 0x0 ),	/* 0 */
/* 126 */	NdrFcShort( 0x4 ),	/* 4 */
/* 128 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 130 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 132 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 134 */	NdrFcShort( 0x0 ),	/* 0 */
/* 136 */	NdrFcShort( 0x0 ),	/* 0 */
/* 138 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 140 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 142 */	NdrFcShort( 0x0 ),	/* 0 */
/* 144 */	NdrFcShort( 0x0 ),	/* 0 */
/* 146 */	NdrFcShort( 0x0 ),	/* 0 */
/* 148 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum5NotUsedOnWire */


    /* Parameter IDL_handle */

/* 150 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 152 */	NdrFcLong( 0x0 ),	/* 0 */
/* 156 */	NdrFcShort( 0x5 ),	/* 5 */
/* 158 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 160 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 162 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 164 */	NdrFcShort( 0x0 ),	/* 0 */
/* 166 */	NdrFcShort( 0x0 ),	/* 0 */
/* 168 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 170 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 172 */	NdrFcShort( 0x0 ),	/* 0 */
/* 174 */	NdrFcShort( 0x0 ),	/* 0 */
/* 176 */	NdrFcShort( 0x0 ),	/* 0 */
/* 178 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum6NotUsedOnWire */


    /* Parameter IDL_handle */

/* 180 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 182 */	NdrFcLong( 0x0 ),	/* 0 */
/* 186 */	NdrFcShort( 0x6 ),	/* 6 */
/* 188 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 190 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 192 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 194 */	NdrFcShort( 0x0 ),	/* 0 */
/* 196 */	NdrFcShort( 0x0 ),	/* 0 */
/* 198 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 200 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 202 */	NdrFcShort( 0x0 ),	/* 0 */
/* 204 */	NdrFcShort( 0x0 ),	/* 0 */
/* 206 */	NdrFcShort( 0x0 ),	/* 0 */
/* 208 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure Opnum7NotUsedOnWire */


    /* Parameter IDL_handle */

/* 210 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 212 */	NdrFcLong( 0x0 ),	/* 0 */
/* 216 */	NdrFcShort( 0x7 ),	/* 7 */
/* 218 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 220 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 222 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 224 */	NdrFcShort( 0x0 ),	/* 0 */
/* 226 */	NdrFcShort( 0x0 ),	/* 0 */
/* 228 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 230 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 232 */	NdrFcShort( 0x0 ),	/* 0 */
/* 234 */	NdrFcShort( 0x0 ),	/* 0 */
/* 236 */	NdrFcShort( 0x0 ),	/* 0 */
/* 238 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetrConnectionEnum */


    /* Parameter IDL_handle */

/* 240 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 242 */	NdrFcLong( 0x0 ),	/* 0 */
/* 246 */	NdrFcShort( 0x8 ),	/* 8 */
/* 248 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 250 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 252 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 254 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 256 */	NdrFcShort( 0x24 ),	/* 36 */
/* 258 */	NdrFcShort( 0x40 ),	/* 64 */
/* 260 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 262 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 264 */	NdrFcShort( 0x1 ),	/* 1 */
/* 266 */	NdrFcShort( 0x1 ),	/* 1 */
/* 268 */	NdrFcShort( 0x0 ),	/* 0 */
/* 270 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 272 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 274 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 276 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Qualifier */

/* 278 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 280 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 282 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 284 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 286 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 288 */	NdrFcShort( 0x9a ),	/* Type Offset=154 */

    /* Parameter PreferedMaximumLength */

/* 290 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 292 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 294 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 296 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 298 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 300 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 302 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 304 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 306 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 308 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 310 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 312 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrFileEnum */

/* 314 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 316 */	NdrFcLong( 0x0 ),	/* 0 */
/* 320 */	NdrFcShort( 0x9 ),	/* 9 */
/* 322 */	NdrFcShort( 0x40 ),	/* ia64 Stack size/offset = 64 */
/* 324 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 326 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 328 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 330 */	NdrFcShort( 0x24 ),	/* 36 */
/* 332 */	NdrFcShort( 0x40 ),	/* 64 */
/* 334 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 336 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 338 */	NdrFcShort( 0x1 ),	/* 1 */
/* 340 */	NdrFcShort( 0x1 ),	/* 1 */
/* 342 */	NdrFcShort( 0x0 ),	/* 0 */
/* 344 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 346 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 348 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 350 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter BasePath */

/* 352 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 354 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 356 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter UserName */

/* 358 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 360 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 362 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 364 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 366 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 368 */	NdrFcShort( 0x128 ),	/* Type Offset=296 */

    /* Parameter PreferedMaximumLength */

/* 370 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 372 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 374 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 376 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 378 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 380 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 382 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 384 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 386 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 388 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 390 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 392 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrFileGetInfo */

/* 394 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 396 */	NdrFcLong( 0x0 ),	/* 0 */
/* 400 */	NdrFcShort( 0xa ),	/* 10 */
/* 402 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 404 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 406 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 408 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 410 */	NdrFcShort( 0x10 ),	/* 16 */
/* 412 */	NdrFcShort( 0x8 ),	/* 8 */
/* 414 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 416 */	0xa,		/* 10 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 418 */	NdrFcShort( 0x1 ),	/* 1 */
/* 420 */	NdrFcShort( 0x0 ),	/* 0 */
/* 422 */	NdrFcShort( 0x0 ),	/* 0 */
/* 424 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 426 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 428 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 430 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter FileId */

/* 432 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 434 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 436 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Level */

/* 438 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 440 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 442 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 444 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 446 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 448 */	NdrFcShort( 0x13c ),	/* Type Offset=316 */

    /* Return value */

/* 450 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 452 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 454 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrFileClose */

/* 456 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 458 */	NdrFcLong( 0x0 ),	/* 0 */
/* 462 */	NdrFcShort( 0xb ),	/* 11 */
/* 464 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 466 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 468 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 470 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 472 */	NdrFcShort( 0x8 ),	/* 8 */
/* 474 */	NdrFcShort( 0x8 ),	/* 8 */
/* 476 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 478 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 480 */	NdrFcShort( 0x0 ),	/* 0 */
/* 482 */	NdrFcShort( 0x0 ),	/* 0 */
/* 484 */	NdrFcShort( 0x0 ),	/* 0 */
/* 486 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 488 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 490 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 492 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter FileId */

/* 494 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 496 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 498 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 500 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 502 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 504 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrSessionEnum */

/* 506 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 508 */	NdrFcLong( 0x0 ),	/* 0 */
/* 512 */	NdrFcShort( 0xc ),	/* 12 */
/* 514 */	NdrFcShort( 0x40 ),	/* ia64 Stack size/offset = 64 */
/* 516 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 518 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 520 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 522 */	NdrFcShort( 0x24 ),	/* 36 */
/* 524 */	NdrFcShort( 0x40 ),	/* 64 */
/* 526 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 528 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 530 */	NdrFcShort( 0x1 ),	/* 1 */
/* 532 */	NdrFcShort( 0x1 ),	/* 1 */
/* 534 */	NdrFcShort( 0x0 ),	/* 0 */
/* 536 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 538 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 540 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 542 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ClientName */

/* 544 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 546 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 548 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter UserName */

/* 550 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 552 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 554 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 556 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 558 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 560 */	NdrFcShort( 0x2de ),	/* Type Offset=734 */

    /* Parameter PreferedMaximumLength */

/* 562 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 564 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 566 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 568 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 570 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 572 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 574 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 576 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 578 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 580 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 582 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 584 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrSessionDel */

/* 586 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 588 */	NdrFcLong( 0x0 ),	/* 0 */
/* 592 */	NdrFcShort( 0xd ),	/* 13 */
/* 594 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 596 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 598 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 600 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 602 */	NdrFcShort( 0x0 ),	/* 0 */
/* 604 */	NdrFcShort( 0x8 ),	/* 8 */
/* 606 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 608 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 610 */	NdrFcShort( 0x0 ),	/* 0 */
/* 612 */	NdrFcShort( 0x0 ),	/* 0 */
/* 614 */	NdrFcShort( 0x0 ),	/* 0 */
/* 616 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 618 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 620 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 622 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ClientName */

/* 624 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 626 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 628 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter UserName */

/* 630 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 632 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 634 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Return value */

/* 636 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 638 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 640 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareAdd */

/* 642 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 644 */	NdrFcLong( 0x0 ),	/* 0 */
/* 648 */	NdrFcShort( 0xe ),	/* 14 */
/* 650 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 652 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 654 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 656 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 658 */	NdrFcShort( 0x24 ),	/* 36 */
/* 660 */	NdrFcShort( 0x24 ),	/* 36 */
/* 662 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 664 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 666 */	NdrFcShort( 0x0 ),	/* 0 */
/* 668 */	NdrFcShort( 0x1 ),	/* 1 */
/* 670 */	NdrFcShort( 0x0 ),	/* 0 */
/* 672 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 674 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 676 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 678 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 680 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 682 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 684 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 686 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 688 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 690 */	NdrFcShort( 0x2f2 ),	/* Type Offset=754 */

    /* Parameter ParmErr */

/* 692 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 694 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 696 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 698 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 700 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 702 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareEnum */

/* 704 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 706 */	NdrFcLong( 0x0 ),	/* 0 */
/* 710 */	NdrFcShort( 0xf ),	/* 15 */
/* 712 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 714 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 716 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 718 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 720 */	NdrFcShort( 0x24 ),	/* 36 */
/* 722 */	NdrFcShort( 0x40 ),	/* 64 */
/* 724 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 726 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 728 */	NdrFcShort( 0x1 ),	/* 1 */
/* 730 */	NdrFcShort( 0x1 ),	/* 1 */
/* 732 */	NdrFcShort( 0x0 ),	/* 0 */
/* 734 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 736 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 738 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 740 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 742 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 744 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 746 */	NdrFcShort( 0x58e ),	/* Type Offset=1422 */

    /* Parameter PreferedMaximumLength */

/* 748 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 750 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 752 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 754 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 756 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 758 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 760 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 762 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 764 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 766 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 768 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 770 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareGetInfo */

/* 772 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 774 */	NdrFcLong( 0x0 ),	/* 0 */
/* 778 */	NdrFcShort( 0x10 ),	/* 16 */
/* 780 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 782 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 784 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 786 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 788 */	NdrFcShort( 0x8 ),	/* 8 */
/* 790 */	NdrFcShort( 0x8 ),	/* 8 */
/* 792 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 794 */	0xa,		/* 10 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 796 */	NdrFcShort( 0x1 ),	/* 1 */
/* 798 */	NdrFcShort( 0x0 ),	/* 0 */
/* 800 */	NdrFcShort( 0x0 ),	/* 0 */
/* 802 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 804 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 806 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 808 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 810 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 812 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 814 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Level */

/* 816 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 818 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 820 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 822 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 824 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 826 */	NdrFcShort( 0x5a6 ),	/* Type Offset=1446 */

    /* Return value */

/* 828 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 830 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 832 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareSetInfo */

/* 834 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 836 */	NdrFcLong( 0x0 ),	/* 0 */
/* 840 */	NdrFcShort( 0x11 ),	/* 17 */
/* 842 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 844 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 846 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 848 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 850 */	NdrFcShort( 0x24 ),	/* 36 */
/* 852 */	NdrFcShort( 0x24 ),	/* 36 */
/* 854 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 856 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 858 */	NdrFcShort( 0x0 ),	/* 0 */
/* 860 */	NdrFcShort( 0x1 ),	/* 1 */
/* 862 */	NdrFcShort( 0x0 ),	/* 0 */
/* 864 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 866 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 868 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 870 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 872 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 874 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 876 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Level */

/* 878 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 880 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 882 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ShareInfo */

/* 884 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 886 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 888 */	NdrFcShort( 0x5b4 ),	/* Type Offset=1460 */

    /* Parameter ParmErr */

/* 890 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 892 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 894 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 896 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 898 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 900 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDel */

/* 902 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 904 */	NdrFcLong( 0x0 ),	/* 0 */
/* 908 */	NdrFcShort( 0x12 ),	/* 18 */
/* 910 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 912 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 914 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 916 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 918 */	NdrFcShort( 0x8 ),	/* 8 */
/* 920 */	NdrFcShort( 0x8 ),	/* 8 */
/* 922 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 924 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 926 */	NdrFcShort( 0x0 ),	/* 0 */
/* 928 */	NdrFcShort( 0x0 ),	/* 0 */
/* 930 */	NdrFcShort( 0x0 ),	/* 0 */
/* 932 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 934 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 936 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 938 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 940 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 942 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 944 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Reserved */

/* 946 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 948 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 950 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 952 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 954 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 956 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelSticky */

/* 958 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 960 */	NdrFcLong( 0x0 ),	/* 0 */
/* 964 */	NdrFcShort( 0x13 ),	/* 19 */
/* 966 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 968 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 970 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 972 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 974 */	NdrFcShort( 0x8 ),	/* 8 */
/* 976 */	NdrFcShort( 0x8 ),	/* 8 */
/* 978 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 980 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 982 */	NdrFcShort( 0x0 ),	/* 0 */
/* 984 */	NdrFcShort( 0x0 ),	/* 0 */
/* 986 */	NdrFcShort( 0x0 ),	/* 0 */
/* 988 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 990 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 992 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 994 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 996 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 998 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1000 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Reserved */

/* 1002 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1004 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1006 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1008 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1010 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1012 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareCheck */

/* 1014 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1016 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1020 */	NdrFcShort( 0x14 ),	/* 20 */
/* 1022 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1024 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1026 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1028 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1030 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1032 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1034 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 1036 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1038 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1040 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1042 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1044 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1046 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1048 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1050 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Device */

/* 1052 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1054 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1056 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Type */

/* 1058 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1060 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1062 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1064 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1066 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1068 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerGetInfo */

/* 1070 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1072 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1076 */	NdrFcShort( 0x15 ),	/* 21 */
/* 1078 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1080 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1082 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1084 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1086 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1088 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1090 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 1092 */	0xa,		/* 10 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1094 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1096 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1098 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1100 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1102 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1104 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1106 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1108 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1110 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1112 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 1114 */	NdrFcShort( 0x2113 ),	/* Flags:  must size, must free, out, simple ref, srv alloc size=8 */
/* 1116 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1118 */	NdrFcShort( 0x5c2 ),	/* Type Offset=1474 */

    /* Return value */

/* 1120 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1122 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1124 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerSetInfo */

/* 1126 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1128 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1132 */	NdrFcShort( 0x16 ),	/* 22 */
/* 1134 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1136 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1138 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1140 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1142 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1144 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1146 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 1148 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1150 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1152 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1154 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1156 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1158 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1160 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1162 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1164 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1166 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1168 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ServerInfo */

/* 1170 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1172 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1174 */	NdrFcShort( 0x840 ),	/* Type Offset=2112 */

    /* Parameter ParmErr */

/* 1176 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 1178 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1180 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 1182 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1184 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1186 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerDiskEnum */

/* 1188 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1190 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1194 */	NdrFcShort( 0x17 ),	/* 23 */
/* 1196 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 1198 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1200 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1202 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1204 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1206 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1208 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 1210 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 1212 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1214 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1216 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1218 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1220 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1222 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1224 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1226 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1228 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1230 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter DiskInfoStruct */

/* 1232 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 1234 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1236 */	NdrFcShort( 0x876 ),	/* Type Offset=2166 */

    /* Parameter PreferedMaximumLength */

/* 1238 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1240 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1242 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 1244 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1246 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1248 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 1250 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 1252 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1254 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 1256 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1258 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 1260 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerStatisticsGet */

/* 1262 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1264 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1268 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1270 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 1272 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1274 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1276 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1278 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1280 */	NdrFcShort( 0x84 ),	/* 132 */
/* 1282 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1284 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1286 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1288 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1290 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1292 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1294 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1296 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1298 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Service */

/* 1300 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1302 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1304 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1306 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1308 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1310 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Options */

/* 1312 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1314 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1316 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 1318 */	NdrFcShort( 0x2012 ),	/* Flags:  must free, out, srv alloc size=8 */
/* 1320 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1322 */	NdrFcShort( 0x886 ),	/* Type Offset=2182 */

    /* Return value */

/* 1324 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1326 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1328 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportAdd */

/* 1330 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1332 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1336 */	NdrFcShort( 0x19 ),	/* 25 */
/* 1338 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1340 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1342 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1344 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1346 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1348 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1350 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 1352 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1354 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1356 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1358 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1360 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1362 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1364 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1366 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1368 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1370 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1372 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 1374 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1376 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1378 */	NdrFcShort( 0x8b4 ),	/* Type Offset=2228 */

    /* Return value */

/* 1380 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1382 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1384 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportEnum */

/* 1386 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1388 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1392 */	NdrFcShort( 0x1a ),	/* 26 */
/* 1394 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 1396 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1398 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1400 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1402 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1404 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1406 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1408 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 1410 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1412 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1414 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1416 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1418 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1420 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1422 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 1424 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 1426 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1428 */	NdrFcShort( 0xa18 ),	/* Type Offset=2584 */

    /* Parameter PreferedMaximumLength */

/* 1430 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1432 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1434 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 1436 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1438 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1440 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 1442 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 1444 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1446 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 1448 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1450 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1452 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportDel */

/* 1454 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1456 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1460 */	NdrFcShort( 0x1b ),	/* 27 */
/* 1462 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1464 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1466 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1468 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1470 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1472 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1474 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 1476 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 1478 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1480 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1482 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1484 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1486 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1488 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1490 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 1492 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1494 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1496 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 1498 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1500 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1502 */	NdrFcShort( 0x8b4 ),	/* Type Offset=2228 */

    /* Return value */

/* 1504 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1506 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1508 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrRemoteTOD */

/* 1510 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1512 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1516 */	NdrFcShort( 0x1c ),	/* 28 */
/* 1518 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1520 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1522 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1524 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1526 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1528 */	NdrFcShort( 0x70 ),	/* 112 */
/* 1530 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 1532 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1534 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1536 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1538 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1540 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1542 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1544 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1546 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter BufferPtr */

/* 1548 */	NdrFcShort( 0x2012 ),	/* Flags:  must free, out, srv alloc size=8 */
/* 1550 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1552 */	NdrFcShort( 0xa28 ),	/* Type Offset=2600 */

    /* Return value */

/* 1554 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1556 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1558 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure Opnum29NotUsedOnWire */

/* 1560 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1562 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1566 */	NdrFcShort( 0x1d ),	/* 29 */
/* 1568 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1570 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 1572 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1574 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1576 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1578 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 1580 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1582 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1584 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1586 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1588 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetprPathType */


    /* Parameter IDL_handle */

/* 1590 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1592 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1596 */	NdrFcShort( 0x1e ),	/* 30 */
/* 1598 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1600 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1602 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1604 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1606 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1608 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1610 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 1612 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1614 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1616 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1618 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1620 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1622 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1624 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1626 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter PathName */

/* 1628 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1630 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1632 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter PathType */

/* 1634 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 1636 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1638 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1640 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1642 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1644 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1646 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1648 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1650 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprPathCanonicalize */

/* 1652 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1654 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1658 */	NdrFcShort( 0x1f ),	/* 31 */
/* 1660 */	NdrFcShort( 0x40 ),	/* ia64 Stack size/offset = 64 */
/* 1662 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1664 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1666 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1668 */	NdrFcShort( 0x2c ),	/* 44 */
/* 1670 */	NdrFcShort( 0x24 ),	/* 36 */
/* 1672 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 1674 */	0xa,		/* 10 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1676 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1678 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1680 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1682 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1684 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1686 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1688 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter PathName */

/* 1690 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1692 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1694 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Outbuf */

/* 1696 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1698 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1700 */	NdrFcShort( 0xa46 ),	/* Type Offset=2630 */

    /* Parameter OutbufLen */

/* 1702 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 1704 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1706 */	NdrFcShort( 0xa52 ),	/* 2642 */

    /* Parameter Prefix */

/* 1708 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1710 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1712 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter PathType */

/* 1714 */	NdrFcShort( 0x158 ),	/* Flags:  in, out, base type, simple ref, */
/* 1716 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1718 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1720 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1722 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 1724 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1726 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1728 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 1730 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprPathCompare */

/* 1732 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1734 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1738 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1740 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 1742 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1744 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1746 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1748 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1750 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1752 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1754 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1756 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1758 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1760 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1762 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1764 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1766 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1768 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter PathName1 */

/* 1770 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1772 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1774 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter PathName2 */

/* 1776 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1778 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1780 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter PathType */

/* 1782 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1784 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1786 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1788 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1790 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1792 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1794 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1796 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1798 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprNameValidate */

/* 1800 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1802 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1806 */	NdrFcShort( 0x21 ),	/* 33 */
/* 1808 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1810 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1812 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1814 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1816 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1818 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1820 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 1822 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1824 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1826 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1828 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1830 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1832 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1834 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1836 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Name */

/* 1838 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1840 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1842 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter NameType */

/* 1844 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1846 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1848 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1850 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1852 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1854 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1856 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1858 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1860 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprNameCanonicalize */

/* 1862 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1864 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1868 */	NdrFcShort( 0x22 ),	/* 34 */
/* 1870 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 1872 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1874 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1876 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1878 */	NdrFcShort( 0x18 ),	/* 24 */
/* 1880 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1882 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 1884 */	0xa,		/* 10 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 1886 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1888 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1890 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1892 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1894 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1896 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1898 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Name */

/* 1900 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1902 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1904 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Outbuf */

/* 1906 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 1908 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1910 */	NdrFcShort( 0xa64 ),	/* Type Offset=2660 */

    /* Parameter OutbufLen */

/* 1912 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 1914 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1916 */	NdrFcShort( 0xa70 ),	/* 2672 */

    /* Parameter NameType */

/* 1918 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1920 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1922 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1924 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1926 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 1928 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1930 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 1932 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 1934 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetprNameCompare */

/* 1936 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 1938 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1942 */	NdrFcShort( 0x23 ),	/* 35 */
/* 1944 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 1946 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 1948 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1950 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 1952 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1954 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1956 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 1958 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 1960 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1962 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1964 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1966 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 1968 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 1970 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 1972 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Name1 */

/* 1974 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1976 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1978 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Name2 */

/* 1980 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 1982 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1984 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter NameType */

/* 1986 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1988 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 1990 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Flags */

/* 1992 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 1994 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 1996 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 1998 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2000 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2002 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareEnumSticky */

/* 2004 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2006 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2010 */	NdrFcShort( 0x24 ),	/* 36 */
/* 2012 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 2014 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2016 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2018 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2020 */	NdrFcShort( 0x24 ),	/* 36 */
/* 2022 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2024 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 2026 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 2028 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2030 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2032 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2034 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2036 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2038 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2040 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 2042 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 2044 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2046 */	NdrFcShort( 0x58e ),	/* Type Offset=1422 */

    /* Parameter PreferedMaximumLength */

/* 2048 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2050 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2052 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 2054 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 2056 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2058 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 2060 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 2062 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2064 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 2066 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2068 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2070 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelStart */

/* 2072 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2074 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2078 */	NdrFcShort( 0x25 ),	/* 37 */
/* 2080 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2082 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2084 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2086 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2088 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2090 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2092 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 2094 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2096 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2098 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2100 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2102 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2104 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2106 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2108 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter NetName */

/* 2110 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2112 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2114 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Reserved */

/* 2116 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2118 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2120 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ContextHandle */

/* 2122 */	NdrFcShort( 0x110 ),	/* Flags:  out, simple ref, */
/* 2124 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2126 */	NdrFcShort( 0xa7e ),	/* Type Offset=2686 */

    /* Return value */

/* 2128 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2130 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2132 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelCommit */

/* 2134 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2136 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2140 */	NdrFcShort( 0x26 ),	/* 38 */
/* 2142 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2144 */	0x30,		/* FC_BIND_CONTEXT */
            0xe0,		/* Ctxt flags:  via ptr, in, out, */
/* 2146 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2148 */	0x0,		/* 0 */
            0x0,		/* 0 */
/* 2150 */	NdrFcShort( 0x38 ),	/* 56 */
/* 2152 */	NdrFcShort( 0x40 ),	/* 64 */
/* 2154 */	0x44,		/* Oi2 Flags:  has return, has ext, */
            0x2,		/* 2 */
/* 2156 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2158 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2160 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2162 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2164 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ContextHandle */

/* 2166 */	NdrFcShort( 0x118 ),	/* Flags:  in, out, simple ref, */
/* 2168 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2170 */	NdrFcShort( 0xa86 ),	/* Type Offset=2694 */

    /* Return value */

/* 2172 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2174 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2176 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrpGetFileSecurity */

/* 2178 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2180 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2184 */	NdrFcShort( 0x27 ),	/* 39 */
/* 2186 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 2188 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2190 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2192 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2194 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2196 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2198 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 2200 */	0xa,		/* 10 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 2202 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2204 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2206 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2208 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2210 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2212 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2214 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ShareName */

/* 2216 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2218 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2220 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter lpFileName */

/* 2222 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2224 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2226 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter RequestedInformation */

/* 2228 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2230 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2232 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter SecurityDescriptor */

/* 2234 */	NdrFcShort( 0x2013 ),	/* Flags:  must size, must free, out, srv alloc size=8 */
/* 2236 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2238 */	NdrFcShort( 0xa8a ),	/* Type Offset=2698 */

    /* Return value */

/* 2240 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2242 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2244 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrpSetFileSecurity */

/* 2246 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2248 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2252 */	NdrFcShort( 0x28 ),	/* 40 */
/* 2254 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 2256 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2258 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2260 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2262 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2264 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2266 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 2268 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2270 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2272 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2274 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2276 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2278 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2280 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2282 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ShareName */

/* 2284 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2286 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2288 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter lpFileName */

/* 2290 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2292 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2294 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter SecurityInformation */

/* 2296 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2298 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2300 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter SecurityDescriptor */

/* 2302 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2304 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2306 */	NdrFcShort( 0xa92 ),	/* Type Offset=2706 */

    /* Return value */

/* 2308 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2310 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2312 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportAddEx */

/* 2314 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2316 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2320 */	NdrFcShort( 0x29 ),	/* 41 */
/* 2322 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2324 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2326 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2328 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2330 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2332 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2334 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2336 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2338 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2340 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2342 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2344 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2346 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2348 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2350 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 2352 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2354 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2356 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 2358 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2360 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2362 */	NdrFcShort( 0xaaa ),	/* Type Offset=2730 */

    /* Return value */

/* 2364 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2366 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2368 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure Opnum42NotUsedOnWire */

/* 2370 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2372 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2376 */	NdrFcShort( 0x2a ),	/* 42 */
/* 2378 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2380 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 2382 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2384 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2386 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2388 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 2390 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2392 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2394 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2396 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2398 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetrDfsGetVersion */


    /* Parameter IDL_handle */

/* 2400 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2402 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2406 */	NdrFcShort( 0x2b ),	/* 43 */
/* 2408 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2410 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2412 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2414 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2416 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2418 */	NdrFcShort( 0x24 ),	/* 36 */
/* 2420 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 2422 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2424 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2426 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2428 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2430 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2432 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2434 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2436 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Version */

/* 2438 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 2440 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2442 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2444 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2446 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2448 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsCreateLocalPartition */

/* 2450 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2452 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2456 */	NdrFcShort( 0x2c ),	/* 44 */
/* 2458 */	NdrFcShort( 0x40 ),	/* ia64 Stack size/offset = 64 */
/* 2460 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2462 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2464 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2466 */	NdrFcShort( 0x4c ),	/* 76 */
/* 2468 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2470 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x8,		/* 8 */
/* 2472 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2474 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2476 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2478 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2480 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2482 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2484 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2486 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ShareName */

/* 2488 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2490 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2492 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter EntryUid */

/* 2494 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2496 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2498 */	NdrFcShort( 0xadc ),	/* Type Offset=2780 */

    /* Parameter EntryPrefix */

/* 2500 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2502 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2504 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter ShortName */

/* 2506 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2508 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2510 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter RelationInfo */

/* 2512 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2514 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2516 */	NdrFcShort( 0xb14 ),	/* Type Offset=2836 */

    /* Parameter Force */

/* 2518 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2520 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 2522 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2524 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2526 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 2528 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsDeleteLocalPartition */

/* 2530 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2532 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2536 */	NdrFcShort( 0x2d ),	/* 45 */
/* 2538 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2540 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2542 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2544 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2546 */	NdrFcShort( 0x44 ),	/* 68 */
/* 2548 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2550 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2552 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2554 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2556 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2558 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2560 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2562 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2564 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2566 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2568 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2570 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2572 */	NdrFcShort( 0xadc ),	/* Type Offset=2780 */

    /* Parameter Prefix */

/* 2574 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2576 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2578 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Return value */

/* 2580 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2582 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2584 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsSetLocalVolumeState */

/* 2586 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2588 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2592 */	NdrFcShort( 0x2e ),	/* 46 */
/* 2594 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2596 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2598 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2600 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2602 */	NdrFcShort( 0x4c ),	/* 76 */
/* 2604 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2606 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 2608 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2610 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2612 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2614 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2616 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2618 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2620 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2622 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2624 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2626 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2628 */	NdrFcShort( 0xadc ),	/* Type Offset=2780 */

    /* Parameter Prefix */

/* 2630 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2632 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2634 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter State */

/* 2636 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2638 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2640 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2642 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2644 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2646 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure Opnum47NotUsedOnWire */

/* 2648 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2650 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2654 */	NdrFcShort( 0x2f ),	/* 47 */
/* 2656 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2658 */	0x32,		/* FC_BIND_PRIMITIVE */
            0x0,		/* 0 */
/* 2660 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2662 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2664 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2666 */	0x40,		/* Oi2 Flags:  has ext, */
            0x0,		/* 0 */
/* 2668 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2670 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2672 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2674 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2676 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Procedure NetrDfsCreateExitPoint */


    /* Parameter IDL_handle */

/* 2678 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2680 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2684 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2686 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 2688 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2690 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2692 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2694 */	NdrFcShort( 0x54 ),	/* 84 */
/* 2696 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2698 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x7,		/* 7 */
/* 2700 */	0xa,		/* 10 */
            0x3,		/* Ext Flags:  new corr desc, clt corr check, */
/* 2702 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2704 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2706 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2708 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2710 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2712 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2714 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2716 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2718 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2720 */	NdrFcShort( 0xadc ),	/* Type Offset=2780 */

    /* Parameter Prefix */

/* 2722 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2724 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2726 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Type */

/* 2728 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2730 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2732 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ShortPrefixLen */

/* 2734 */	NdrFcShort( 0x88 ),	/* Flags:  in, by val, */
/* 2736 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2738 */	NdrFcShort( 0xb24 ),	/* 2852 */

    /* Parameter ShortPrefix */

/* 2740 */	NdrFcShort( 0x113 ),	/* Flags:  must size, must free, out, simple ref, */
/* 2742 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2744 */	NdrFcShort( 0xb32 ),	/* Type Offset=2866 */

    /* Return value */

/* 2746 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2748 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 2750 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsDeleteExitPoint */

/* 2752 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2754 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2758 */	NdrFcShort( 0x31 ),	/* 49 */
/* 2760 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2762 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2764 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2766 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2768 */	NdrFcShort( 0x4c ),	/* 76 */
/* 2770 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2772 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x5,		/* 5 */
/* 2774 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2776 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2778 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2780 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2782 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2784 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2786 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2788 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2790 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2792 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2794 */	NdrFcShort( 0xadc ),	/* Type Offset=2780 */

    /* Parameter Prefix */

/* 2796 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2798 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2800 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter Type */

/* 2802 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2804 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2806 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2808 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2810 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2812 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsModifyPrefix */

/* 2814 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2816 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2820 */	NdrFcShort( 0x32 ),	/* 50 */
/* 2822 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2824 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2826 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2828 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2830 */	NdrFcShort( 0x44 ),	/* 68 */
/* 2832 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2834 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 2836 */	0xa,		/* 10 */
            0x1,		/* Ext Flags:  new corr desc, */
/* 2838 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2840 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2842 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2844 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2846 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2848 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2850 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Uid */

/* 2852 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2854 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2856 */	NdrFcShort( 0xadc ),	/* Type Offset=2780 */

    /* Parameter Prefix */

/* 2858 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2860 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2862 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Return value */

/* 2864 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2866 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2868 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsFixLocalVolume */

/* 2870 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2872 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2876 */	NdrFcShort( 0x33 ),	/* 51 */
/* 2878 */	NdrFcShort( 0x50 ),	/* ia64 Stack size/offset = 80 */
/* 2880 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2882 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2884 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2886 */	NdrFcShort( 0x5c ),	/* 92 */
/* 2888 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2890 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0xa,		/* 10 */
/* 2892 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 2894 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2896 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2898 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2900 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2902 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2904 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2906 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter VolumeName */

/* 2908 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2910 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2912 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter EntryType */

/* 2914 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2916 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 2918 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ServiceType */

/* 2920 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2922 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2924 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter StgId */

/* 2926 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2928 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2930 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter EntryUid */

/* 2932 */	NdrFcShort( 0x10a ),	/* Flags:  must free, in, simple ref, */
/* 2934 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 2936 */	NdrFcShort( 0xadc ),	/* Type Offset=2780 */

    /* Parameter EntryPrefix */

/* 2938 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2940 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 2942 */	NdrFcShort( 0x5a0 ),	/* Type Offset=1440 */

    /* Parameter RelationInfo */

/* 2944 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 2946 */	NdrFcShort( 0x38 ),	/* ia64 Stack size/offset = 56 */
/* 2948 */	NdrFcShort( 0xb14 ),	/* Type Offset=2836 */

    /* Parameter CreateDisposition */

/* 2950 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 2952 */	NdrFcShort( 0x40 ),	/* ia64 Stack size/offset = 64 */
/* 2954 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Return value */

/* 2956 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 2958 */	NdrFcShort( 0x48 ),	/* ia64 Stack size/offset = 72 */
/* 2960 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrDfsManagerReportSiteInfo */

/* 2962 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 2964 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2968 */	NdrFcShort( 0x34 ),	/* 52 */
/* 2970 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2972 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 2974 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2976 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 2978 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2980 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2982 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x3,		/* 3 */
/* 2984 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 2986 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2988 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2990 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2992 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 2994 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 2996 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 2998 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter ppSiteInfo */

/* 3000 */	NdrFcShort( 0x201b ),	/* Flags:  must size, must free, in, out, srv alloc size=8 */
/* 3002 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3004 */	NdrFcShort( 0xb3e ),	/* Type Offset=2878 */

    /* Return value */

/* 3006 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3008 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 3010 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerTransportDelEx */

/* 3012 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3014 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3018 */	NdrFcShort( 0x35 ),	/* 53 */
/* 3020 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 3022 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 3024 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3026 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3028 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3030 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3032 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 3034 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 3036 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3038 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3040 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3042 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3044 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3046 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3048 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 3050 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3052 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3054 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter Buffer */

/* 3056 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 3058 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 3060 */	NdrFcShort( 0xb7c ),	/* Type Offset=2940 */

    /* Return value */

/* 3062 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3064 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 3066 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerAliasAdd */

/* 3068 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3070 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3074 */	NdrFcShort( 0x36 ),	/* 54 */
/* 3076 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 3078 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 3080 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3082 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3084 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3086 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3088 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 3090 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 3092 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3094 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3096 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3098 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3100 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3102 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3104 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 3106 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3108 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3110 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 3112 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 3114 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 3116 */	NdrFcShort( 0xb8a ),	/* Type Offset=2954 */

    /* Return value */

/* 3118 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3120 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 3122 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerAliasEnum */

/* 3124 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3126 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3130 */	NdrFcShort( 0x37 ),	/* 55 */
/* 3132 */	NdrFcShort( 0x30 ),	/* ia64 Stack size/offset = 48 */
/* 3134 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 3136 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3138 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3140 */	NdrFcShort( 0x24 ),	/* 36 */
/* 3142 */	NdrFcShort( 0x40 ),	/* 64 */
/* 3144 */	0x47,		/* Oi2 Flags:  srv must size, clt must size, has return, has ext, */
            0x6,		/* 6 */
/* 3146 */	0xa,		/* 10 */
            0x7,		/* Ext Flags:  new corr desc, clt corr check, srv corr check, */
/* 3148 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3150 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3152 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3154 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3156 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3158 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3160 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter InfoStruct */

/* 3162 */	NdrFcShort( 0x11b ),	/* Flags:  must size, must free, in, out, simple ref, */
/* 3164 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3166 */	NdrFcShort( 0xbfe ),	/* Type Offset=3070 */

    /* Parameter PreferedMaximumLength */

/* 3168 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3170 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 3172 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter TotalEntries */

/* 3174 */	NdrFcShort( 0x2150 ),	/* Flags:  out, base type, simple ref, srv alloc size=8 */
/* 3176 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 3178 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ResumeHandle */

/* 3180 */	NdrFcShort( 0x1a ),	/* Flags:  must free, in, out, */
/* 3182 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 3184 */	NdrFcShort( 0xae ),	/* Type Offset=174 */

    /* Return value */

/* 3186 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3188 */	NdrFcShort( 0x28 ),	/* ia64 Stack size/offset = 40 */
/* 3190 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrServerAliasDel */

/* 3192 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3194 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3198 */	NdrFcShort( 0x38 ),	/* 56 */
/* 3200 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 3202 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 3204 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3206 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3208 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3210 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3212 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 3214 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 3216 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3218 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3220 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3222 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3224 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3226 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3228 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 3230 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3232 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3234 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter InfoStruct */

/* 3236 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 3238 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 3240 */	NdrFcShort( 0xc12 ),	/* Type Offset=3090 */

    /* Return value */

/* 3242 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3244 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 3246 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Procedure NetrShareDelEx */

/* 3248 */	0x0,		/* 0 */
            0x48,		/* Old Flags:  */
/* 3250 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3254 */	NdrFcShort( 0x39 ),	/* 57 */
/* 3256 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 3258 */	0x31,		/* FC_BIND_GENERIC */
            0x8,		/* 8 */
/* 3260 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3262 */	0x0,		/* 0 */
            0x5c,		/* FC_PAD */
/* 3264 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3266 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3268 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
            0x4,		/* 4 */
/* 3270 */	0xa,		/* 10 */
            0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 3272 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3274 */	NdrFcShort( 0x1 ),	/* 1 */
/* 3276 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3278 */	NdrFcShort( 0x0 ),	/* 0 */

    /* Parameter ServerName */

/* 3280 */	NdrFcShort( 0xb ),	/* Flags:  must size, must free, in, */
/* 3282 */	NdrFcShort( 0x0 ),	/* ia64 Stack size/offset = 0 */
/* 3284 */	NdrFcShort( 0x2 ),	/* Type Offset=2 */

    /* Parameter Level */

/* 3286 */	NdrFcShort( 0x48 ),	/* Flags:  in, base type, */
/* 3288 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3290 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

    /* Parameter ShareInfo */

/* 3292 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 3294 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 3296 */	NdrFcShort( 0xc20 ),	/* Type Offset=3104 */

    /* Return value */

/* 3298 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 3300 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 3302 */	0x8,		/* FC_LONG */
            0x0,		/* 0 */

            0x0";
        private const string TYPE_FORMAT_STRING_X64 = @"
            NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/*  4 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/*  6 */	
            0x11, 0x0,	/* FC_RP */
/*  8 */	NdrFcShort( 0x92 ),	/* Offset= 146 (154) */
/* 10 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 12 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 14 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 16 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 18 */	NdrFcShort( 0x2 ),	/* Offset= 2 (20) */
/* 20 */	NdrFcShort( 0x8 ),	/* 8 */
/* 22 */	NdrFcShort( 0x3002 ),	/* 12290 */
/* 24 */	NdrFcLong( 0x0 ),	/* 0 */
/* 28 */	NdrFcShort( 0xa ),	/* Offset= 10 (38) */
/* 30 */	NdrFcLong( 0x1 ),	/* 1 */
/* 34 */	NdrFcShort( 0x34 ),	/* Offset= 52 (86) */
/* 36 */	NdrFcShort( 0xffff ),	/* Offset= -1 (35) */
/* 38 */	
            0x12, 0x0,	/* FC_UP */
/* 40 */	NdrFcShort( 0x1e ),	/* Offset= 30 (70) */
/* 42 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 44 */	NdrFcShort( 0x4 ),	/* 4 */
/* 46 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 48 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 50 */	NdrFcShort( 0x0 ),	/* 0 */
/* 52 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 54 */	NdrFcShort( 0x0 ),	/* 0 */
/* 56 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 58 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 62 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 64 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 66 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (42) */
/* 68 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 70 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 72 */	NdrFcShort( 0x10 ),	/* 16 */
/* 74 */	NdrFcShort( 0x0 ),	/* 0 */
/* 76 */	NdrFcShort( 0x6 ),	/* Offset= 6 (82) */
/* 78 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 80 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 82 */	
            0x12, 0x0,	/* FC_UP */
/* 84 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (48) */
/* 86 */	
            0x12, 0x0,	/* FC_UP */
/* 88 */	NdrFcShort( 0x32 ),	/* Offset= 50 (138) */
/* 90 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 92 */	NdrFcShort( 0x28 ),	/* 40 */
/* 94 */	NdrFcShort( 0x0 ),	/* 0 */
/* 96 */	NdrFcShort( 0xc ),	/* Offset= 12 (108) */
/* 98 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 100 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 102 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 104 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 106 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 108 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 110 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 112 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 114 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 116 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 118 */	NdrFcShort( 0x0 ),	/* 0 */
/* 120 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 122 */	NdrFcShort( 0x0 ),	/* 0 */
/* 124 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 126 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 130 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 132 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 134 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (90) */
/* 136 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 138 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 140 */	NdrFcShort( 0x10 ),	/* 16 */
/* 142 */	NdrFcShort( 0x0 ),	/* 0 */
/* 144 */	NdrFcShort( 0x6 ),	/* Offset= 6 (150) */
/* 146 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 148 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 150 */	
            0x12, 0x0,	/* FC_UP */
/* 152 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (116) */
/* 154 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 156 */	NdrFcShort( 0x10 ),	/* 16 */
/* 158 */	NdrFcShort( 0x0 ),	/* 0 */
/* 160 */	NdrFcShort( 0x0 ),	/* Offset= 0 (160) */
/* 162 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 164 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 166 */	NdrFcShort( 0xff64 ),	/* Offset= -156 (10) */
/* 168 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 170 */	
            0x11, 0xc,	/* FC_RP [alloced_on_stack] [simple_pointer] */
/* 172 */	0x8,		/* FC_LONG */
            0x5c,		/* FC_PAD */
/* 174 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 176 */	0x8,		/* FC_LONG */
            0x5c,		/* FC_PAD */
/* 178 */	
            0x11, 0x0,	/* FC_RP */
/* 180 */	NdrFcShort( 0x74 ),	/* Offset= 116 (296) */
/* 182 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 184 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 186 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 188 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 190 */	NdrFcShort( 0x2 ),	/* Offset= 2 (192) */
/* 192 */	NdrFcShort( 0x8 ),	/* 8 */
/* 194 */	NdrFcShort( 0x3002 ),	/* 12290 */
/* 196 */	NdrFcLong( 0x2 ),	/* 2 */
/* 200 */	NdrFcShort( 0xa ),	/* Offset= 10 (210) */
/* 202 */	NdrFcLong( 0x3 ),	/* 3 */
/* 206 */	NdrFcShort( 0x18 ),	/* Offset= 24 (230) */
/* 208 */	NdrFcShort( 0xffff ),	/* Offset= -1 (207) */
/* 210 */	
            0x12, 0x0,	/* FC_UP */
/* 212 */	NdrFcShort( 0x2 ),	/* Offset= 2 (214) */
/* 214 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 216 */	NdrFcShort( 0x10 ),	/* 16 */
/* 218 */	NdrFcShort( 0x0 ),	/* 0 */
/* 220 */	NdrFcShort( 0x6 ),	/* Offset= 6 (226) */
/* 222 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 224 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 226 */	
            0x12, 0x0,	/* FC_UP */
/* 228 */	NdrFcShort( 0xff4c ),	/* Offset= -180 (48) */
/* 230 */	
            0x12, 0x0,	/* FC_UP */
/* 232 */	NdrFcShort( 0x30 ),	/* Offset= 48 (280) */
/* 234 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 236 */	NdrFcShort( 0x20 ),	/* 32 */
/* 238 */	NdrFcShort( 0x0 ),	/* 0 */
/* 240 */	NdrFcShort( 0xa ),	/* Offset= 10 (250) */
/* 242 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 244 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 246 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 248 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 250 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 252 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 254 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 256 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 258 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 260 */	NdrFcShort( 0x0 ),	/* 0 */
/* 262 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 264 */	NdrFcShort( 0x0 ),	/* 0 */
/* 266 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 268 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 272 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 274 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 276 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (234) */
/* 278 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 280 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 282 */	NdrFcShort( 0x10 ),	/* 16 */
/* 284 */	NdrFcShort( 0x0 ),	/* 0 */
/* 286 */	NdrFcShort( 0x6 ),	/* Offset= 6 (292) */
/* 288 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 290 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 292 */	
            0x12, 0x0,	/* FC_UP */
/* 294 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (258) */
/* 296 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 298 */	NdrFcShort( 0x10 ),	/* 16 */
/* 300 */	NdrFcShort( 0x0 ),	/* 0 */
/* 302 */	NdrFcShort( 0x0 ),	/* Offset= 0 (302) */
/* 304 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 306 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 308 */	NdrFcShort( 0xff82 ),	/* Offset= -126 (182) */
/* 310 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 312 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 314 */	NdrFcShort( 0x2 ),	/* Offset= 2 (316) */
/* 316 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 318 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 320 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 322 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 324 */	NdrFcShort( 0x2 ),	/* Offset= 2 (326) */
/* 326 */	NdrFcShort( 0x8 ),	/* 8 */
/* 328 */	NdrFcShort( 0x3002 ),	/* 12290 */
/* 330 */	NdrFcLong( 0x2 ),	/* 2 */
/* 334 */	NdrFcShort( 0xa ),	/* Offset= 10 (344) */
/* 336 */	NdrFcLong( 0x3 ),	/* 3 */
/* 340 */	NdrFcShort( 0x8 ),	/* Offset= 8 (348) */
/* 342 */	NdrFcShort( 0xffff ),	/* Offset= -1 (341) */
/* 344 */	
            0x12, 0x0,	/* FC_UP */
/* 346 */	NdrFcShort( 0xfed0 ),	/* Offset= -304 (42) */
/* 348 */	
            0x12, 0x0,	/* FC_UP */
/* 350 */	NdrFcShort( 0xff8c ),	/* Offset= -116 (234) */
/* 352 */	
            0x11, 0x0,	/* FC_RP */
/* 354 */	NdrFcShort( 0x17c ),	/* Offset= 380 (734) */
/* 356 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 358 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 360 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 362 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 364 */	NdrFcShort( 0x2 ),	/* Offset= 2 (366) */
/* 366 */	NdrFcShort( 0x8 ),	/* 8 */
/* 368 */	NdrFcShort( 0x3005 ),	/* 12293 */
/* 370 */	NdrFcLong( 0x0 ),	/* 0 */
/* 374 */	NdrFcShort( 0x1c ),	/* Offset= 28 (402) */
/* 376 */	NdrFcLong( 0x1 ),	/* 1 */
/* 380 */	NdrFcShort( 0x4e ),	/* Offset= 78 (458) */
/* 382 */	NdrFcLong( 0x2 ),	/* 2 */
/* 386 */	NdrFcShort( 0x8a ),	/* Offset= 138 (524) */
/* 388 */	NdrFcLong( 0xa ),	/* 10 */
/* 392 */	NdrFcShort( 0xca ),	/* Offset= 202 (594) */
/* 394 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 398 */	NdrFcShort( 0x104 ),	/* Offset= 260 (658) */
/* 400 */	NdrFcShort( 0xffff ),	/* Offset= -1 (399) */
/* 402 */	
            0x12, 0x0,	/* FC_UP */
/* 404 */	NdrFcShort( 0x26 ),	/* Offset= 38 (442) */
/* 406 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 408 */	NdrFcShort( 0x8 ),	/* 8 */
/* 410 */	NdrFcShort( 0x0 ),	/* 0 */
/* 412 */	NdrFcShort( 0x4 ),	/* Offset= 4 (416) */
/* 414 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 416 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 418 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 420 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 422 */	NdrFcShort( 0x0 ),	/* 0 */
/* 424 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 426 */	NdrFcShort( 0x0 ),	/* 0 */
/* 428 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 430 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 434 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 436 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 438 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (406) */
/* 440 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 442 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 444 */	NdrFcShort( 0x10 ),	/* 16 */
/* 446 */	NdrFcShort( 0x0 ),	/* 0 */
/* 448 */	NdrFcShort( 0x6 ),	/* Offset= 6 (454) */
/* 450 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 452 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 454 */	
            0x12, 0x0,	/* FC_UP */
/* 456 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (420) */
/* 458 */	
            0x12, 0x0,	/* FC_UP */
/* 460 */	NdrFcShort( 0x30 ),	/* Offset= 48 (508) */
/* 462 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 464 */	NdrFcShort( 0x20 ),	/* 32 */
/* 466 */	NdrFcShort( 0x0 ),	/* 0 */
/* 468 */	NdrFcShort( 0xa ),	/* Offset= 10 (478) */
/* 470 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 472 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 474 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 476 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 478 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 480 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 482 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 484 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 486 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 488 */	NdrFcShort( 0x0 ),	/* 0 */
/* 490 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 492 */	NdrFcShort( 0x0 ),	/* 0 */
/* 494 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 496 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 500 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 502 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 504 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (462) */
/* 506 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 508 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 510 */	NdrFcShort( 0x10 ),	/* 16 */
/* 512 */	NdrFcShort( 0x0 ),	/* 0 */
/* 514 */	NdrFcShort( 0x6 ),	/* Offset= 6 (520) */
/* 516 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 518 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 520 */	
            0x12, 0x0,	/* FC_UP */
/* 522 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (486) */
/* 524 */	
            0x12, 0x0,	/* FC_UP */
/* 526 */	NdrFcShort( 0x34 ),	/* Offset= 52 (578) */
/* 528 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 530 */	NdrFcShort( 0x28 ),	/* 40 */
/* 532 */	NdrFcShort( 0x0 ),	/* 0 */
/* 534 */	NdrFcShort( 0xa ),	/* Offset= 10 (544) */
/* 536 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 538 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 540 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 542 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 544 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 546 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 548 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 550 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 552 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 554 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 556 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 558 */	NdrFcShort( 0x0 ),	/* 0 */
/* 560 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 562 */	NdrFcShort( 0x0 ),	/* 0 */
/* 564 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 566 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 570 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 572 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 574 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (528) */
/* 576 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 578 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 580 */	NdrFcShort( 0x10 ),	/* 16 */
/* 582 */	NdrFcShort( 0x0 ),	/* 0 */
/* 584 */	NdrFcShort( 0x6 ),	/* Offset= 6 (590) */
/* 586 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 588 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 590 */	
            0x12, 0x0,	/* FC_UP */
/* 592 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (556) */
/* 594 */	
            0x12, 0x0,	/* FC_UP */
/* 596 */	NdrFcShort( 0x2e ),	/* Offset= 46 (642) */
/* 598 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 600 */	NdrFcShort( 0x18 ),	/* 24 */
/* 602 */	NdrFcShort( 0x0 ),	/* 0 */
/* 604 */	NdrFcShort( 0x8 ),	/* Offset= 8 (612) */
/* 606 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 608 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 610 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 612 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 614 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 616 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 618 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 620 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 622 */	NdrFcShort( 0x0 ),	/* 0 */
/* 624 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 626 */	NdrFcShort( 0x0 ),	/* 0 */
/* 628 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 630 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 634 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 636 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 638 */	NdrFcShort( 0xffd8 ),	/* Offset= -40 (598) */
/* 640 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 642 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 644 */	NdrFcShort( 0x10 ),	/* 16 */
/* 646 */	NdrFcShort( 0x0 ),	/* 0 */
/* 648 */	NdrFcShort( 0x6 ),	/* Offset= 6 (654) */
/* 650 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 652 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 654 */	
            0x12, 0x0,	/* FC_UP */
/* 656 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (620) */
/* 658 */	
            0x12, 0x0,	/* FC_UP */
/* 660 */	NdrFcShort( 0x3a ),	/* Offset= 58 (718) */
/* 662 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 664 */	NdrFcShort( 0x30 ),	/* 48 */
/* 666 */	NdrFcShort( 0x0 ),	/* 0 */
/* 668 */	NdrFcShort( 0xc ),	/* Offset= 12 (680) */
/* 670 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 672 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 674 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 676 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 678 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 680 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 682 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 684 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 686 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 688 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 690 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 692 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 694 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 696 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 698 */	NdrFcShort( 0x0 ),	/* 0 */
/* 700 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 702 */	NdrFcShort( 0x0 ),	/* 0 */
/* 704 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 706 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 710 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 712 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 714 */	NdrFcShort( 0xffcc ),	/* Offset= -52 (662) */
/* 716 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 718 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 720 */	NdrFcShort( 0x10 ),	/* 16 */
/* 722 */	NdrFcShort( 0x0 ),	/* 0 */
/* 724 */	NdrFcShort( 0x6 ),	/* Offset= 6 (730) */
/* 726 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 728 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 730 */	
            0x12, 0x0,	/* FC_UP */
/* 732 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (696) */
/* 734 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 736 */	NdrFcShort( 0x10 ),	/* 16 */
/* 738 */	NdrFcShort( 0x0 ),	/* 0 */
/* 740 */	NdrFcShort( 0x0 ),	/* Offset= 0 (740) */
/* 742 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 744 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 746 */	NdrFcShort( 0xfe7a ),	/* Offset= -390 (356) */
/* 748 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 750 */	
            0x11, 0x0,	/* FC_RP */
/* 752 */	NdrFcShort( 0x2 ),	/* Offset= 2 (754) */
/* 754 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 756 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 758 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 760 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 762 */	NdrFcShort( 0x2 ),	/* Offset= 2 (764) */
/* 764 */	NdrFcShort( 0x8 ),	/* 8 */
/* 766 */	NdrFcShort( 0x300a ),	/* 12298 */
/* 768 */	NdrFcLong( 0x0 ),	/* 0 */
/* 772 */	NdrFcShort( 0x3a ),	/* Offset= 58 (830) */
/* 774 */	NdrFcLong( 0x1 ),	/* 1 */
/* 778 */	NdrFcShort( 0x46 ),	/* Offset= 70 (848) */
/* 780 */	NdrFcLong( 0x2 ),	/* 2 */
/* 784 */	NdrFcShort( 0x5a ),	/* Offset= 90 (874) */
/* 786 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 790 */	NdrFcShort( 0x7c ),	/* Offset= 124 (914) */
/* 792 */	NdrFcLong( 0x3ec ),	/* 1004 */
/* 796 */	NdrFcShort( 0xb0 ),	/* Offset= 176 (972) */
/* 798 */	NdrFcLong( 0x3ee ),	/* 1006 */
/* 802 */	NdrFcShort( 0xfe36 ),	/* Offset= -458 (344) */
/* 804 */	NdrFcLong( 0x5dd ),	/* 1501 */
/* 808 */	NdrFcShort( 0xb6 ),	/* Offset= 182 (990) */
/* 810 */	NdrFcLong( 0x3ed ),	/* 1005 */
/* 814 */	NdrFcShort( 0xfe2a ),	/* Offset= -470 (344) */
/* 816 */	NdrFcLong( 0x1f5 ),	/* 501 */
/* 820 */	NdrFcShort( 0xca ),	/* Offset= 202 (1022) */
/* 822 */	NdrFcLong( 0x1f7 ),	/* 503 */
/* 826 */	NdrFcShort( 0xe0 ),	/* Offset= 224 (1050) */
/* 828 */	NdrFcShort( 0x0 ),	/* Offset= 0 (828) */
/* 830 */	
            0x12, 0x0,	/* FC_UP */
/* 832 */	NdrFcShort( 0x2 ),	/* Offset= 2 (834) */
/* 834 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 836 */	NdrFcShort( 0x8 ),	/* 8 */
/* 838 */	NdrFcShort( 0x0 ),	/* 0 */
/* 840 */	NdrFcShort( 0x4 ),	/* Offset= 4 (844) */
/* 842 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 844 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 846 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 848 */	
            0x12, 0x0,	/* FC_UP */
/* 850 */	NdrFcShort( 0x2 ),	/* Offset= 2 (852) */
/* 852 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 854 */	NdrFcShort( 0x18 ),	/* 24 */
/* 856 */	NdrFcShort( 0x0 ),	/* 0 */
/* 858 */	NdrFcShort( 0x8 ),	/* Offset= 8 (866) */
/* 860 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 862 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 864 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 866 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 868 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 870 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 872 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 874 */	
            0x12, 0x0,	/* FC_UP */
/* 876 */	NdrFcShort( 0x2 ),	/* Offset= 2 (878) */
/* 878 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 880 */	NdrFcShort( 0x38 ),	/* 56 */
/* 882 */	NdrFcShort( 0x0 ),	/* 0 */
/* 884 */	NdrFcShort( 0xe ),	/* Offset= 14 (898) */
/* 886 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 888 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 890 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 892 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 894 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 896 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 898 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 900 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 902 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 904 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 906 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 908 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 910 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 912 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 914 */	
            0x12, 0x0,	/* FC_UP */
/* 916 */	NdrFcShort( 0xe ),	/* Offset= 14 (930) */
/* 918 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 920 */	NdrFcShort( 0x1 ),	/* 1 */
/* 922 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 924 */	NdrFcShort( 0x38 ),	/* 56 */
/* 926 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 928 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 930 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 932 */	NdrFcShort( 0x48 ),	/* 72 */
/* 934 */	NdrFcShort( 0x0 ),	/* 0 */
/* 936 */	NdrFcShort( 0x10 ),	/* Offset= 16 (952) */
/* 938 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 940 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 942 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 944 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 946 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 948 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 950 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 952 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 954 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 956 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 958 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 960 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 962 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 964 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 966 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 968 */	
            0x12, 0x0,	/* FC_UP */
/* 970 */	NdrFcShort( 0xffcc ),	/* Offset= -52 (918) */
/* 972 */	
            0x12, 0x0,	/* FC_UP */
/* 974 */	NdrFcShort( 0x2 ),	/* Offset= 2 (976) */
/* 976 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 978 */	NdrFcShort( 0x8 ),	/* 8 */
/* 980 */	NdrFcShort( 0x0 ),	/* 0 */
/* 982 */	NdrFcShort( 0x4 ),	/* Offset= 4 (986) */
/* 984 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 986 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 988 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 990 */	
            0x12, 0x0,	/* FC_UP */
/* 992 */	NdrFcShort( 0xe ),	/* Offset= 14 (1006) */
/* 994 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 996 */	NdrFcShort( 0x1 ),	/* 1 */
/* 998 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1000 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1002 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1004 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 1006 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1008 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1010 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1012 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1018) */
/* 1014 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1016 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1018 */	
            0x12, 0x0,	/* FC_UP */
/* 1020 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (994) */
/* 1022 */	
            0x12, 0x0,	/* FC_UP */
/* 1024 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1026) */
/* 1026 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1028 */	NdrFcShort( 0x20 ),	/* 32 */
/* 1030 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1032 */	NdrFcShort( 0xa ),	/* Offset= 10 (1042) */
/* 1034 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1036 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 1038 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1040 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1042 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1044 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1046 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1048 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1050 */	
            0x12, 0x0,	/* FC_UP */
/* 1052 */	NdrFcShort( 0xe ),	/* Offset= 14 (1066) */
/* 1054 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 1056 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1058 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1060 */	NdrFcShort( 0x40 ),	/* 64 */
/* 1062 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1064 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 1066 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1068 */	NdrFcShort( 0x50 ),	/* 80 */
/* 1070 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1072 */	NdrFcShort( 0x12 ),	/* Offset= 18 (1090) */
/* 1074 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1076 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 1078 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1080 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1082 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 1084 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1086 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 1088 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1090 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1092 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1094 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1096 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1098 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1100 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1102 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1104 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1106 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1108 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1110 */	
            0x12, 0x0,	/* FC_UP */
/* 1112 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (1054) */
/* 1114 */	
            0x11, 0x0,	/* FC_RP */
/* 1116 */	NdrFcShort( 0x132 ),	/* Offset= 306 (1422) */
/* 1118 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1120 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 1122 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 1124 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1126 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1128) */
/* 1128 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1130 */	NdrFcShort( 0x3006 ),	/* 12294 */
/* 1132 */	NdrFcLong( 0x0 ),	/* 0 */
/* 1136 */	NdrFcShort( 0x22 ),	/* Offset= 34 (1170) */
/* 1138 */	NdrFcLong( 0x1 ),	/* 1 */
/* 1142 */	NdrFcShort( 0x46 ),	/* Offset= 70 (1212) */
/* 1144 */	NdrFcLong( 0x2 ),	/* 2 */
/* 1148 */	NdrFcShort( 0x6a ),	/* Offset= 106 (1254) */
/* 1150 */	NdrFcLong( 0x1f5 ),	/* 501 */
/* 1154 */	NdrFcShort( 0x8e ),	/* Offset= 142 (1296) */
/* 1156 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 1160 */	NdrFcShort( 0xb2 ),	/* Offset= 178 (1338) */
/* 1162 */	NdrFcLong( 0x1f7 ),	/* 503 */
/* 1166 */	NdrFcShort( 0xd6 ),	/* Offset= 214 (1380) */
/* 1168 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1167) */
/* 1170 */	
            0x12, 0x0,	/* FC_UP */
/* 1172 */	NdrFcShort( 0x18 ),	/* Offset= 24 (1196) */
/* 1174 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 1176 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1178 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1180 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1182 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1184 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1188 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1190 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 1192 */	NdrFcShort( 0xfe9a ),	/* Offset= -358 (834) */
/* 1194 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1196 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1198 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1200 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1202 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1208) */
/* 1204 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1206 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1208 */	
            0x12, 0x0,	/* FC_UP */
/* 1210 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1174) */
/* 1212 */	
            0x12, 0x0,	/* FC_UP */
/* 1214 */	NdrFcShort( 0x18 ),	/* Offset= 24 (1238) */
/* 1216 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 1218 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1220 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1222 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1224 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1226 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1230 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1232 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 1234 */	NdrFcShort( 0xfe82 ),	/* Offset= -382 (852) */
/* 1236 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1238 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1240 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1242 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1244 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1250) */
/* 1246 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1248 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1250 */	
            0x12, 0x0,	/* FC_UP */
/* 1252 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1216) */
/* 1254 */	
            0x12, 0x0,	/* FC_UP */
/* 1256 */	NdrFcShort( 0x18 ),	/* Offset= 24 (1280) */
/* 1258 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 1260 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1262 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1264 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1266 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1268 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1272 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1274 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 1276 */	NdrFcShort( 0xfe72 ),	/* Offset= -398 (878) */
/* 1278 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1280 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1282 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1284 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1286 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1292) */
/* 1288 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1290 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1292 */	
            0x12, 0x0,	/* FC_UP */
/* 1294 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1258) */
/* 1296 */	
            0x12, 0x0,	/* FC_UP */
/* 1298 */	NdrFcShort( 0x18 ),	/* Offset= 24 (1322) */
/* 1300 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 1302 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1304 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1306 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1308 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1310 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1314 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1316 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 1318 */	NdrFcShort( 0xfedc ),	/* Offset= -292 (1026) */
/* 1320 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1322 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1324 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1326 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1328 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1334) */
/* 1330 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1332 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1334 */	
            0x12, 0x0,	/* FC_UP */
/* 1336 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1300) */
/* 1338 */	
            0x12, 0x0,	/* FC_UP */
/* 1340 */	NdrFcShort( 0x18 ),	/* Offset= 24 (1364) */
/* 1342 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 1344 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1346 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1348 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1350 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1352 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1356 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1358 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 1360 */	NdrFcShort( 0xfe52 ),	/* Offset= -430 (930) */
/* 1362 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1364 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1366 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1368 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1370 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1376) */
/* 1372 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1374 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1376 */	
            0x12, 0x0,	/* FC_UP */
/* 1378 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1342) */
/* 1380 */	
            0x12, 0x0,	/* FC_UP */
/* 1382 */	NdrFcShort( 0x18 ),	/* Offset= 24 (1406) */
/* 1384 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 1386 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1388 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 1390 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1392 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1394 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 1398 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 1400 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 1402 */	NdrFcShort( 0xfeb0 ),	/* Offset= -336 (1066) */
/* 1404 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1406 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1408 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1410 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1412 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1418) */
/* 1414 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1416 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1418 */	
            0x12, 0x0,	/* FC_UP */
/* 1420 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (1384) */
/* 1422 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1424 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1426 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1428 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1428) */
/* 1430 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1432 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 1434 */	NdrFcShort( 0xfec4 ),	/* Offset= -316 (1118) */
/* 1436 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1438 */	
            0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 1440 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1442 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 1444 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1446) */
/* 1446 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1448 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 1450 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1452 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1454 */	NdrFcShort( 0xfd4e ),	/* Offset= -690 (764) */
/* 1456 */	
            0x11, 0x0,	/* FC_RP */
/* 1458 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1460) */
/* 1460 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1462 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 1464 */	NdrFcShort( 0x10 ),	/* ia64 Stack size/offset = 16 */
/* 1466 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1468 */	NdrFcShort( 0xfd40 ),	/* Offset= -704 (764) */
/* 1470 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 1472 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1474) */
/* 1474 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 1476 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 1478 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 1480 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1482 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1484) */
/* 1484 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1486 */	NdrFcShort( 0x3033 ),	/* 12339 */
/* 1488 */	NdrFcLong( 0x64 ),	/* 100 */
/* 1492 */	NdrFcShort( 0x130 ),	/* Offset= 304 (1796) */
/* 1494 */	NdrFcLong( 0x65 ),	/* 101 */
/* 1498 */	NdrFcShort( 0x13e ),	/* Offset= 318 (1816) */
/* 1500 */	NdrFcLong( 0x66 ),	/* 102 */
/* 1504 */	NdrFcShort( 0x156 ),	/* Offset= 342 (1846) */
/* 1506 */	NdrFcLong( 0x67 ),	/* 103 */
/* 1510 */	NdrFcShort( 0x178 ),	/* Offset= 376 (1886) */
/* 1512 */	NdrFcLong( 0x1f6 ),	/* 502 */
/* 1516 */	NdrFcShort( 0x19c ),	/* Offset= 412 (1928) */
/* 1518 */	NdrFcLong( 0x1f7 ),	/* 503 */
/* 1522 */	NdrFcShort( 0x1b2 ),	/* Offset= 434 (1956) */
/* 1524 */	NdrFcLong( 0x257 ),	/* 599 */
/* 1528 */	NdrFcShort( 0x1e8 ),	/* Offset= 488 (2016) */
/* 1530 */	NdrFcLong( 0x3ed ),	/* 1005 */
/* 1534 */	NdrFcShort( 0x22c ),	/* Offset= 556 (2090) */
/* 1536 */	NdrFcLong( 0x453 ),	/* 1107 */
/* 1540 */	NdrFcShort( 0xfb54 ),	/* Offset= -1196 (344) */
/* 1542 */	NdrFcLong( 0x3f2 ),	/* 1010 */
/* 1546 */	NdrFcShort( 0xfb4e ),	/* Offset= -1202 (344) */
/* 1548 */	NdrFcLong( 0x3f8 ),	/* 1016 */
/* 1552 */	NdrFcShort( 0xfb48 ),	/* Offset= -1208 (344) */
/* 1554 */	NdrFcLong( 0x3f9 ),	/* 1017 */
/* 1558 */	NdrFcShort( 0xfb42 ),	/* Offset= -1214 (344) */
/* 1560 */	NdrFcLong( 0x3fa ),	/* 1018 */
/* 1564 */	NdrFcShort( 0xfb3c ),	/* Offset= -1220 (344) */
/* 1566 */	NdrFcLong( 0x5dd ),	/* 1501 */
/* 1570 */	NdrFcShort( 0xfb36 ),	/* Offset= -1226 (344) */
/* 1572 */	NdrFcLong( 0x5de ),	/* 1502 */
/* 1576 */	NdrFcShort( 0xfb30 ),	/* Offset= -1232 (344) */
/* 1578 */	NdrFcLong( 0x5df ),	/* 1503 */
/* 1582 */	NdrFcShort( 0xfb2a ),	/* Offset= -1238 (344) */
/* 1584 */	NdrFcLong( 0x5e2 ),	/* 1506 */
/* 1588 */	NdrFcShort( 0xfb24 ),	/* Offset= -1244 (344) */
/* 1590 */	NdrFcLong( 0x5e6 ),	/* 1510 */
/* 1594 */	NdrFcShort( 0xfb1e ),	/* Offset= -1250 (344) */
/* 1596 */	NdrFcLong( 0x5e7 ),	/* 1511 */
/* 1600 */	NdrFcShort( 0xfb18 ),	/* Offset= -1256 (344) */
/* 1602 */	NdrFcLong( 0x5e8 ),	/* 1512 */
/* 1606 */	NdrFcShort( 0xfb12 ),	/* Offset= -1262 (344) */
/* 1608 */	NdrFcLong( 0x5e9 ),	/* 1513 */
/* 1612 */	NdrFcShort( 0xfb0c ),	/* Offset= -1268 (344) */
/* 1614 */	NdrFcLong( 0x5ea ),	/* 1514 */
/* 1618 */	NdrFcShort( 0xfb06 ),	/* Offset= -1274 (344) */
/* 1620 */	NdrFcLong( 0x5eb ),	/* 1515 */
/* 1624 */	NdrFcShort( 0xfb00 ),	/* Offset= -1280 (344) */
/* 1626 */	NdrFcLong( 0x5ec ),	/* 1516 */
/* 1630 */	NdrFcShort( 0xfafa ),	/* Offset= -1286 (344) */
/* 1632 */	NdrFcLong( 0x5ee ),	/* 1518 */
/* 1636 */	NdrFcShort( 0xfaf4 ),	/* Offset= -1292 (344) */
/* 1638 */	NdrFcLong( 0x5f3 ),	/* 1523 */
/* 1642 */	NdrFcShort( 0xfaee ),	/* Offset= -1298 (344) */
/* 1644 */	NdrFcLong( 0x5f8 ),	/* 1528 */
/* 1648 */	NdrFcShort( 0xfae8 ),	/* Offset= -1304 (344) */
/* 1650 */	NdrFcLong( 0x5f9 ),	/* 1529 */
/* 1654 */	NdrFcShort( 0xfae2 ),	/* Offset= -1310 (344) */
/* 1656 */	NdrFcLong( 0x5fa ),	/* 1530 */
/* 1660 */	NdrFcShort( 0xfadc ),	/* Offset= -1316 (344) */
/* 1662 */	NdrFcLong( 0x5fd ),	/* 1533 */
/* 1666 */	NdrFcShort( 0xfad6 ),	/* Offset= -1322 (344) */
/* 1668 */	NdrFcLong( 0x5fe ),	/* 1534 */
/* 1672 */	NdrFcShort( 0xfad0 ),	/* Offset= -1328 (344) */
/* 1674 */	NdrFcLong( 0x5ff ),	/* 1535 */
/* 1678 */	NdrFcShort( 0xfaca ),	/* Offset= -1334 (344) */
/* 1680 */	NdrFcLong( 0x600 ),	/* 1536 */
/* 1684 */	NdrFcShort( 0xfac4 ),	/* Offset= -1340 (344) */
/* 1686 */	NdrFcLong( 0x602 ),	/* 1538 */
/* 1690 */	NdrFcShort( 0xfabe ),	/* Offset= -1346 (344) */
/* 1692 */	NdrFcLong( 0x603 ),	/* 1539 */
/* 1696 */	NdrFcShort( 0xfab8 ),	/* Offset= -1352 (344) */
/* 1698 */	NdrFcLong( 0x604 ),	/* 1540 */
/* 1702 */	NdrFcShort( 0xfab2 ),	/* Offset= -1358 (344) */
/* 1704 */	NdrFcLong( 0x605 ),	/* 1541 */
/* 1708 */	NdrFcShort( 0xfaac ),	/* Offset= -1364 (344) */
/* 1710 */	NdrFcLong( 0x606 ),	/* 1542 */
/* 1714 */	NdrFcShort( 0xfaa6 ),	/* Offset= -1370 (344) */
/* 1716 */	NdrFcLong( 0x607 ),	/* 1543 */
/* 1720 */	NdrFcShort( 0xfaa0 ),	/* Offset= -1376 (344) */
/* 1722 */	NdrFcLong( 0x608 ),	/* 1544 */
/* 1726 */	NdrFcShort( 0xfa9a ),	/* Offset= -1382 (344) */
/* 1728 */	NdrFcLong( 0x609 ),	/* 1545 */
/* 1732 */	NdrFcShort( 0xfa94 ),	/* Offset= -1388 (344) */
/* 1734 */	NdrFcLong( 0x60a ),	/* 1546 */
/* 1738 */	NdrFcShort( 0xfa8e ),	/* Offset= -1394 (344) */
/* 1740 */	NdrFcLong( 0x60b ),	/* 1547 */
/* 1744 */	NdrFcShort( 0xfa88 ),	/* Offset= -1400 (344) */
/* 1746 */	NdrFcLong( 0x60c ),	/* 1548 */
/* 1750 */	NdrFcShort( 0xfa82 ),	/* Offset= -1406 (344) */
/* 1752 */	NdrFcLong( 0x60d ),	/* 1549 */
/* 1756 */	NdrFcShort( 0xfa7c ),	/* Offset= -1412 (344) */
/* 1758 */	NdrFcLong( 0x60e ),	/* 1550 */
/* 1762 */	NdrFcShort( 0xfa76 ),	/* Offset= -1418 (344) */
/* 1764 */	NdrFcLong( 0x610 ),	/* 1552 */
/* 1768 */	NdrFcShort( 0xfa70 ),	/* Offset= -1424 (344) */
/* 1770 */	NdrFcLong( 0x611 ),	/* 1553 */
/* 1774 */	NdrFcShort( 0xfa6a ),	/* Offset= -1430 (344) */
/* 1776 */	NdrFcLong( 0x612 ),	/* 1554 */
/* 1780 */	NdrFcShort( 0xfa64 ),	/* Offset= -1436 (344) */
/* 1782 */	NdrFcLong( 0x613 ),	/* 1555 */
/* 1786 */	NdrFcShort( 0xfa5e ),	/* Offset= -1442 (344) */
/* 1788 */	NdrFcLong( 0x614 ),	/* 1556 */
/* 1792 */	NdrFcShort( 0xfa58 ),	/* Offset= -1448 (344) */
/* 1794 */	NdrFcShort( 0xffff ),	/* Offset= -1 (1793) */
/* 1796 */	
            0x12, 0x0,	/* FC_UP */
/* 1798 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1800) */
/* 1800 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1802 */	NdrFcShort( 0x10 ),	/* 16 */
/* 1804 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1806 */	NdrFcShort( 0x6 ),	/* Offset= 6 (1812) */
/* 1808 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1810 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1812 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1814 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1816 */	
            0x12, 0x0,	/* FC_UP */
/* 1818 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1820) */
/* 1820 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1822 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1824 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1826 */	NdrFcShort( 0xc ),	/* Offset= 12 (1838) */
/* 1828 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1830 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1832 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1834 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 1836 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1838 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1840 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1842 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1844 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1846 */	
            0x12, 0x0,	/* FC_UP */
/* 1848 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1850) */
/* 1850 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1852 */	NdrFcShort( 0x48 ),	/* 72 */
/* 1854 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1856 */	NdrFcShort( 0x12 ),	/* Offset= 18 (1874) */
/* 1858 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1860 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1862 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1864 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 1866 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1868 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1870 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1872 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 1874 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1876 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1878 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1880 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1882 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1884 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1886 */	
            0x12, 0x0,	/* FC_UP */
/* 1888 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1890) */
/* 1890 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1892 */	NdrFcShort( 0x50 ),	/* 80 */
/* 1894 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1896 */	NdrFcShort( 0x14 ),	/* Offset= 20 (1916) */
/* 1898 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 1900 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1902 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1904 */	0x40,		/* FC_STRUCTPAD4 */
            0x36,		/* FC_POINTER */
/* 1906 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1908 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1910 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1912 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1914 */	0x40,		/* FC_STRUCTPAD4 */
            0x5b,		/* FC_END */
/* 1916 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1918 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1920 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1922 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1924 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 1926 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 1928 */	
            0x12, 0x0,	/* FC_UP */
/* 1930 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1932) */
/* 1932 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 1934 */	NdrFcShort( 0x48 ),	/* 72 */
/* 1936 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1938 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1940 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1942 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1944 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1946 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1948 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1950 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1952 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1954 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 1956 */	
            0x12, 0x0,	/* FC_UP */
/* 1958 */	NdrFcShort( 0x2 ),	/* Offset= 2 (1960) */
/* 1960 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 1962 */	NdrFcShort( 0xb0 ),	/* 176 */
/* 1964 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1966 */	NdrFcShort( 0x2e ),	/* Offset= 46 (2012) */
/* 1968 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1970 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1972 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1974 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1976 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1978 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1980 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1982 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1984 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1986 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 1988 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1990 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1992 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1994 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1996 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 1998 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2000 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2002 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2004 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2006 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2008 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2010 */	0x40,		/* FC_STRUCTPAD4 */
            0x5b,		/* FC_END */
/* 2012 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2014 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2016 */	
            0x12, 0x0,	/* FC_UP */
/* 2018 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2020) */
/* 2020 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2022 */	NdrFcShort( 0xe8 ),	/* 232 */
/* 2024 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2026 */	NdrFcShort( 0x3c ),	/* Offset= 60 (2086) */
/* 2028 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2030 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2032 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2034 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2036 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2038 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2040 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2042 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2044 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2046 */	0x36,		/* FC_POINTER */
            0x8,		/* FC_LONG */
/* 2048 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2050 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2052 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2054 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2056 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2058 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2060 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2062 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2064 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2066 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2068 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2070 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2072 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2074 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2076 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2078 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2080 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2082 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2084 */	0x40,		/* FC_STRUCTPAD4 */
            0x5b,		/* FC_END */
/* 2086 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2088 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2090 */	
            0x12, 0x0,	/* FC_UP */
/* 2092 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2094) */
/* 2094 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2096 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2098 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2100 */	NdrFcShort( 0x4 ),	/* Offset= 4 (2104) */
/* 2102 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2104 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2106 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2108 */	
            0x11, 0x0,	/* FC_RP */
/* 2110 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2112) */
/* 2112 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 2114 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2116 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2118 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2120 */	NdrFcShort( 0xfd84 ),	/* Offset= -636 (1484) */
/* 2122 */	
            0x11, 0x0,	/* FC_RP */
/* 2124 */	NdrFcShort( 0x2a ),	/* Offset= 42 (2166) */
/* 2126 */	
            0x29,		/* FC_WSTRING */
            0x5c,		/* FC_PAD */
/* 2128 */	NdrFcShort( 0x3 ),	/* 3 */
/* 2130 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x1,		/* 1 */
/* 2132 */	NdrFcShort( 0x6 ),	/* 6 */
/* 2134 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2136 */	NdrFcShort( 0x0 ),	/* Offset= 0 (2136) */
/* 2138 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2140 */	NdrFcShort( 0xfff2 ),	/* Offset= -14 (2126) */
/* 2142 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2144 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x1,		/* 1 */
/* 2146 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2148 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2150 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2152 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2154 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2156 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2158 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2160 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2162 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (2130) */
/* 2164 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2166 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2168 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2170 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2172 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2178) */
/* 2174 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2176 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2178 */	
            0x12, 0x0,	/* FC_UP */
/* 2180 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2144) */
/* 2182 */	
            0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 2184 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2186) */
/* 2186 */	
            0x12, 0x0,	/* FC_UP */
/* 2188 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2190) */
/* 2190 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 2192 */	NdrFcShort( 0x44 ),	/* 68 */
/* 2194 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2196 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2198 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2200 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2202 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2204 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2206 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2208 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2210 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2212 */	
            0x11, 0x0,	/* FC_RP */
/* 2214 */	NdrFcShort( 0xe ),	/* Offset= 14 (2228) */
/* 2216 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 2218 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2220 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2222 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2224 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2226 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 2228 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2230 */	NdrFcShort( 0x28 ),	/* 40 */
/* 2232 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2234 */	NdrFcShort( 0xa ),	/* Offset= 10 (2244) */
/* 2236 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2238 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2240 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2242 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2244 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2246 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2248 */	
            0x12, 0x0,	/* FC_UP */
/* 2250 */	NdrFcShort( 0xffde ),	/* Offset= -34 (2216) */
/* 2252 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2254 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2256 */	
            0x11, 0x0,	/* FC_RP */
/* 2258 */	NdrFcShort( 0x146 ),	/* Offset= 326 (2584) */
/* 2260 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 2262 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 2264 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 2266 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2268 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2270) */
/* 2270 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2272 */	NdrFcShort( 0x3004 ),	/* 12292 */
/* 2274 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2278 */	NdrFcShort( 0x16 ),	/* Offset= 22 (2300) */
/* 2280 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2284 */	NdrFcShort( 0x3a ),	/* Offset= 58 (2342) */
/* 2286 */	NdrFcLong( 0x2 ),	/* 2 */
/* 2290 */	NdrFcShort( 0x80 ),	/* Offset= 128 (2418) */
/* 2292 */	NdrFcLong( 0x3 ),	/* 3 */
/* 2296 */	NdrFcShort( 0xc8 ),	/* Offset= 200 (2496) */
/* 2298 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2297) */
/* 2300 */	
            0x12, 0x0,	/* FC_UP */
/* 2302 */	NdrFcShort( 0x18 ),	/* Offset= 24 (2326) */
/* 2304 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 2306 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2308 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2310 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2312 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2314 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2318 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2320 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2322 */	NdrFcShort( 0xffa2 ),	/* Offset= -94 (2228) */
/* 2324 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2326 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2328 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2330 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2332 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2338) */
/* 2334 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2336 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2338 */	
            0x12, 0x0,	/* FC_UP */
/* 2340 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2304) */
/* 2342 */	
            0x12, 0x0,	/* FC_UP */
/* 2344 */	NdrFcShort( 0x3a ),	/* Offset= 58 (2402) */
/* 2346 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2348 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2350 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2352 */	NdrFcShort( 0xc ),	/* Offset= 12 (2364) */
/* 2354 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2356 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2358 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2360 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2362 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2364 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2366 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2368 */	
            0x12, 0x0,	/* FC_UP */
/* 2370 */	NdrFcShort( 0xff66 ),	/* Offset= -154 (2216) */
/* 2372 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2374 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2376 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2378 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2380 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 2382 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2384 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2386 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2388 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2390 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2394 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2396 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2398 */	NdrFcShort( 0xffcc ),	/* Offset= -52 (2346) */
/* 2400 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2402 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2404 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2406 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2408 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2414) */
/* 2410 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2412 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2414 */	
            0x12, 0x0,	/* FC_UP */
/* 2416 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2380) */
/* 2418 */	
            0x12, 0x0,	/* FC_UP */
/* 2420 */	NdrFcShort( 0x3c ),	/* Offset= 60 (2480) */
/* 2422 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2424 */	NdrFcShort( 0x38 ),	/* 56 */
/* 2426 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2428 */	NdrFcShort( 0xe ),	/* Offset= 14 (2442) */
/* 2430 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2432 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2434 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2436 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2438 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2440 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2442 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2444 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2446 */	
            0x12, 0x0,	/* FC_UP */
/* 2448 */	NdrFcShort( 0xff18 ),	/* Offset= -232 (2216) */
/* 2450 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2452 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2454 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2456 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2458 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 2460 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2462 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2464 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2466 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2468 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2472 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2474 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2476 */	NdrFcShort( 0xffca ),	/* Offset= -54 (2422) */
/* 2478 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2480 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2482 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2484 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2486 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2492) */
/* 2488 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2490 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2492 */	
            0x12, 0x0,	/* FC_UP */
/* 2494 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2458) */
/* 2496 */	
            0x12, 0x0,	/* FC_UP */
/* 2498 */	NdrFcShort( 0x46 ),	/* Offset= 70 (2568) */
/* 2500 */	
            0x1d,		/* FC_SMFARRAY */
            0x0,		/* 0 */
/* 2502 */	NdrFcShort( 0x100 ),	/* 256 */
/* 2504 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 2506 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2508 */	NdrFcShort( 0x138 ),	/* 312 */
/* 2510 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2512 */	NdrFcShort( 0x12 ),	/* Offset= 18 (2530) */
/* 2514 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2516 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2518 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2520 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2522 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2524 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2526 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (2500) */
/* 2528 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2530 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2532 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2534 */	
            0x12, 0x0,	/* FC_UP */
/* 2536 */	NdrFcShort( 0xfec0 ),	/* Offset= -320 (2216) */
/* 2538 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2540 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2542 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2544 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2546 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 2548 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2550 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2552 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2554 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2556 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2560 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2562 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2564 */	NdrFcShort( 0xffc6 ),	/* Offset= -58 (2506) */
/* 2566 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2568 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2570 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2572 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2574 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2580) */
/* 2576 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2578 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2580 */	
            0x12, 0x0,	/* FC_UP */
/* 2582 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2546) */
/* 2584 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2586 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2588 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2590 */	NdrFcShort( 0x0 ),	/* Offset= 0 (2590) */
/* 2592 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2594 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2596 */	NdrFcShort( 0xfeb0 ),	/* Offset= -336 (2260) */
/* 2598 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2600 */	
            0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 2602 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2604) */
/* 2604 */	
            0x12, 0x0,	/* FC_UP */
/* 2606 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2608) */
/* 2608 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 2610 */	NdrFcShort( 0x30 ),	/* 48 */
/* 2612 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2614 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2616 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2618 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2620 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2622 */	0x8,		/* FC_LONG */
            0x8,		/* FC_LONG */
/* 2624 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2626 */	
            0x11, 0x0,	/* FC_RP */
/* 2628 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2630) */
/* 2630 */	
            0x1b,		/* FC_CARRAY */
            0x0,		/* 0 */
/* 2632 */	NdrFcShort( 0x1 ),	/* 1 */
/* 2634 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2636 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2638 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2640 */	0x2,		/* FC_CHAR */
            0x5b,		/* FC_END */
/* 2642 */	0xb7,		/* FC_RANGE */
            0x8,		/* 8 */
/* 2644 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2648 */	NdrFcLong( 0xfa00 ),	/* 64000 */
/* 2652 */	
            0x11, 0x8,	/* FC_RP [simple_pointer] */
/* 2654 */	0x8,		/* FC_LONG */
            0x5c,		/* FC_PAD */
/* 2656 */	
            0x11, 0x0,	/* FC_RP */
/* 2658 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2660) */
/* 2660 */	
            0x1b,		/* FC_CARRAY */
            0x1,		/* 1 */
/* 2662 */	NdrFcShort( 0x2 ),	/* 2 */
/* 2664 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2666 */	NdrFcShort( 0x18 ),	/* ia64 Stack size/offset = 24 */
/* 2668 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2670 */	0x5,		/* FC_WCHAR */
            0x5b,		/* FC_END */
/* 2672 */	0xb7,		/* FC_RANGE */
            0x8,		/* 8 */
/* 2674 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2678 */	NdrFcLong( 0xfa00 ),	/* 64000 */
/* 2682 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2684 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2686) */
/* 2686 */	0x30,		/* FC_BIND_CONTEXT */
            0xa0,		/* Ctxt flags:  via ptr, out, */
/* 2688 */	0x0,		/* 0 */
            0x0,		/* 0 */
/* 2690 */	
            0x11, 0x4,	/* FC_RP [alloced_on_stack] */
/* 2692 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2694) */
/* 2694 */	0x30,		/* FC_BIND_CONTEXT */
            0xe1,		/* Ctxt flags:  via ptr, in, out, can't be null */
/* 2696 */	0x0,		/* 0 */
            0x0,		/* 0 */
/* 2698 */	
            0x11, 0x14,	/* FC_RP [alloced_on_stack] [pointer_deref] */
/* 2700 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2702) */
/* 2702 */	
            0x12, 0x0,	/* FC_UP */
/* 2704 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2706) */
/* 2706 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2708 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2710 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2712 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2718) */
/* 2714 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2716 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2718 */	
            0x12, 0x0,	/* FC_UP */
/* 2720 */	NdrFcShort( 0xf942 ),	/* Offset= -1726 (994) */
/* 2722 */	
            0x11, 0x0,	/* FC_RP */
/* 2724 */	NdrFcShort( 0xffee ),	/* Offset= -18 (2706) */
/* 2726 */	
            0x11, 0x0,	/* FC_RP */
/* 2728 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2730) */
/* 2730 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 2732 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2734 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2736 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2738 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2740) */
/* 2740 */	NdrFcShort( 0x138 ),	/* 312 */
/* 2742 */	NdrFcShort( 0x3004 ),	/* 12292 */
/* 2744 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2748 */	NdrFcShort( 0xfdf8 ),	/* Offset= -520 (2228) */
/* 2750 */	NdrFcLong( 0x1 ),	/* 1 */
/* 2754 */	NdrFcShort( 0xfe68 ),	/* Offset= -408 (2346) */
/* 2756 */	NdrFcLong( 0x2 ),	/* 2 */
/* 2760 */	NdrFcShort( 0xfeae ),	/* Offset= -338 (2422) */
/* 2762 */	NdrFcLong( 0x3 ),	/* 3 */
/* 2766 */	NdrFcShort( 0xfefc ),	/* Offset= -260 (2506) */
/* 2768 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2767) */
/* 2770 */	
            0x11, 0x0,	/* FC_RP */
/* 2772 */	NdrFcShort( 0x8 ),	/* Offset= 8 (2780) */
/* 2774 */	
            0x1d,		/* FC_SMFARRAY */
            0x0,		/* 0 */
/* 2776 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2778 */	0x1,		/* FC_BYTE */
            0x5b,		/* FC_END */
/* 2780 */	
            0x15,		/* FC_STRUCT */
            0x3,		/* 3 */
/* 2782 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2784 */	0x8,		/* FC_LONG */
            0x6,		/* FC_SHORT */
/* 2786 */	0x6,		/* FC_SHORT */
            0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 2788 */	0x0,		/* 0 */
            NdrFcShort( 0xfff1 ),	/* Offset= -15 (2774) */
            0x5b,		/* FC_END */
/* 2792 */	
            0x11, 0x0,	/* FC_RP */
/* 2794 */	NdrFcShort( 0x2a ),	/* Offset= 42 (2836) */
/* 2796 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2798 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2800 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2802 */	NdrFcShort( 0x8 ),	/* Offset= 8 (2810) */
/* 2804 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2806 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (2780) */
/* 2808 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2810 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2812 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2814 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 2816 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2818 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 2820 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2822 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2824 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2828 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2830 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2832 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2796) */
/* 2834 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2836 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2838 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2840 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2842 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2848) */
/* 2844 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2846 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2848 */	
            0x12, 0x0,	/* FC_UP */
/* 2850 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (2814) */
/* 2852 */	0xb7,		/* FC_RANGE */
            0x8,		/* 8 */
/* 2854 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2858 */	NdrFcLong( 0x20 ),	/* 32 */
/* 2862 */	
            0x11, 0x0,	/* FC_RP */
/* 2864 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2866) */
/* 2866 */	
            0x1b,		/* FC_CARRAY */
            0x1,		/* 1 */
/* 2868 */	NdrFcShort( 0x2 ),	/* 2 */
/* 2870 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2872 */	NdrFcShort( 0x20 ),	/* ia64 Stack size/offset = 32 */
/* 2874 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2876 */	0x5,		/* FC_WCHAR */
            0x5b,		/* FC_END */
/* 2878 */	
            0x12, 0x14,	/* FC_UP [alloced_on_stack] [pointer_deref] */
/* 2880 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2882) */
/* 2882 */	
            0x12, 0x0,	/* FC_UP */
/* 2884 */	NdrFcShort( 0x28 ),	/* Offset= 40 (2924) */
/* 2886 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2888 */	NdrFcShort( 0x10 ),	/* 16 */
/* 2890 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2892 */	NdrFcShort( 0x6 ),	/* Offset= 6 (2898) */
/* 2894 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2896 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 2898 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2900 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2902 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 2904 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2906 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 2908 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 2910 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2912 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 2916 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 2918 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 2920 */	NdrFcShort( 0xffde ),	/* Offset= -34 (2886) */
/* 2922 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2924 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2926 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2928 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (2902) */
/* 2930 */	NdrFcShort( 0x0 ),	/* Offset= 0 (2930) */
/* 2932 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 2934 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 2936 */	
            0x11, 0x0,	/* FC_RP */
/* 2938 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2940) */
/* 2940 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 2942 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2944 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2946 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2948 */	NdrFcShort( 0xff30 ),	/* Offset= -208 (2740) */
/* 2950 */	
            0x11, 0x0,	/* FC_RP */
/* 2952 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2954) */
/* 2954 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 2956 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 2958 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 2960 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 2962 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2964) */
/* 2964 */	NdrFcShort( 0x8 ),	/* 8 */
/* 2966 */	NdrFcShort( 0x3001 ),	/* 12289 */
/* 2968 */	NdrFcLong( 0x0 ),	/* 0 */
/* 2972 */	NdrFcShort( 0x4 ),	/* Offset= 4 (2976) */
/* 2974 */	NdrFcShort( 0xffff ),	/* Offset= -1 (2973) */
/* 2976 */	
            0x12, 0x0,	/* FC_UP */
/* 2978 */	NdrFcShort( 0x2 ),	/* Offset= 2 (2980) */
/* 2980 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 2982 */	NdrFcShort( 0x18 ),	/* 24 */
/* 2984 */	NdrFcShort( 0x0 ),	/* 0 */
/* 2986 */	NdrFcShort( 0x8 ),	/* Offset= 8 (2994) */
/* 2988 */	0x36,		/* FC_POINTER */
            0x36,		/* FC_POINTER */
/* 2990 */	0x2,		/* FC_CHAR */
            0x3f,		/* FC_STRUCTPAD3 */
/* 2992 */	0x8,		/* FC_LONG */
            0x5b,		/* FC_END */
/* 2994 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 2996 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 2998 */	
            0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 3000 */	
            0x25,		/* FC_C_WSTRING */
            0x5c,		/* FC_PAD */
/* 3002 */	
            0x11, 0x0,	/* FC_RP */
/* 3004 */	NdrFcShort( 0x42 ),	/* Offset= 66 (3070) */
/* 3006 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3008 */	0x9,		/* Corr desc: FC_ULONG */
            0x0,		/*  */
/* 3010 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 3012 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3014 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3016) */
/* 3016 */	NdrFcShort( 0x8 ),	/* 8 */
/* 3018 */	NdrFcShort( 0x3001 ),	/* 12289 */
/* 3020 */	NdrFcLong( 0x0 ),	/* 0 */
/* 3024 */	NdrFcShort( 0x4 ),	/* Offset= 4 (3028) */
/* 3026 */	NdrFcShort( 0xffff ),	/* Offset= -1 (3025) */
/* 3028 */	
            0x12, 0x0,	/* FC_UP */
/* 3030 */	NdrFcShort( 0x18 ),	/* Offset= 24 (3054) */
/* 3032 */	
            0x21,		/* FC_BOGUS_ARRAY */
            0x3,		/* 3 */
/* 3034 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3036 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
            0x0,		/*  */
/* 3038 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3040 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3042 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 3046 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 3048 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 3050 */	NdrFcShort( 0xffba ),	/* Offset= -70 (2980) */
/* 3052 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3054 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 3056 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3058 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3060 */	NdrFcShort( 0x6 ),	/* Offset= 6 (3066) */
/* 3062 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 3064 */	0x36,		/* FC_POINTER */
            0x5b,		/* FC_END */
/* 3066 */	
            0x12, 0x0,	/* FC_UP */
/* 3068 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (3032) */
/* 3070 */	
            0x1a,		/* FC_BOGUS_STRUCT */
            0x3,		/* 3 */
/* 3072 */	NdrFcShort( 0x10 ),	/* 16 */
/* 3074 */	NdrFcShort( 0x0 ),	/* 0 */
/* 3076 */	NdrFcShort( 0x0 ),	/* Offset= 0 (3076) */
/* 3078 */	0x8,		/* FC_LONG */
            0x40,		/* FC_STRUCTPAD4 */
/* 3080 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
            0x0,		/* 0 */
/* 3082 */	NdrFcShort( 0xffb4 ),	/* Offset= -76 (3006) */
/* 3084 */	0x5c,		/* FC_PAD */
            0x5b,		/* FC_END */
/* 3086 */	
            0x11, 0x0,	/* FC_RP */
/* 3088 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3090) */
/* 3090 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3092 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3094 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3096 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3098 */	NdrFcShort( 0xff7a ),	/* Offset= -134 (2964) */
/* 3100 */	
            0x11, 0x0,	/* FC_RP */
/* 3102 */	NdrFcShort( 0x2 ),	/* Offset= 2 (3104) */
/* 3104 */	
            0x2b,		/* FC_NON_ENCAPSULATED_UNION */
            0x9,		/* FC_ULONG */
/* 3106 */	0x29,		/* Corr desc:  parameter, FC_ULONG */
            0x0,		/*  */
/* 3108 */	NdrFcShort( 0x8 ),	/* ia64 Stack size/offset = 8 */
/* 3110 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 3112 */	NdrFcShort( 0xf6d4 ),	/* Offset= -2348 (764) */

            0x0";
        internal static readonly ushort[] ProcFormatStringOffsetTableX64 = new ushort[]
    {
    0,
    30,
    60,
    90,
    120,
    150,
    180,
    210,
    240,
    314,
    394,
    456,
    506,
    586,
    642,
    704,
    772,
    834,
    902,
    958,
    1014,
    1070,
    1126,
    1188,
    1262,
    1330,
    1386,
    1454,
    1510,
    1560,
    1590,
    1652,
    1732,
    1800,
    1862,
    1936,
    2004,
    2072,
    2134,
    2178,
    2246,
    2314,
    2370,
    2400,
    2450,
    2530,
    2586,
    2648,
    2678,
    2752,
    2814,
    2870,
    2962,
    3012,
    3068,
    3124,
    3192,
    3248
    };
    }


}
