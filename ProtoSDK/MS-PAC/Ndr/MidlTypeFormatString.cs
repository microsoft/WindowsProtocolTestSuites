// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    /// Format string and offset of NDR-marshalled structures,
    /// including KERB_VALIDATION_INFO, _S4U_DELEGATION_INFO,
    /// _SECPKG_SUPPLEMENTAL_CRED and _NTLM_SUPPLEMENTAL_CREDENTIAL.
    /// More details about NDR encoding/decoding and format string
    /// please refer summary of Common\Ndr\NdrMarshaller class.
    /// </summary>
    internal static class FormatString
    {
        #region format string
        /// <summary>
        /// Format string of NDR-marshalled structures.
        /// Copied from midl.exe generated RPC stub code.
        /// More details about NDR encoding/decoding and format string
        /// please refer summary of Common\Ndr\NdrMarshaller class.
        /// </summary>
        internal static byte[] Pac = new byte[] 
        {
			0x00,0x00, /* 0 */
/*  2 */ 
   0x12, 0x0, /* FC_UP */
/*  4 */ 0x22,0x01, /* Offset= 290 (294) */
/*  6 */ 
   0x2b,  /* FC_NON_ENCAPSULATED_UNION */
   0xd,  /* FC_ENUM16 */
/*  8 */ 0x6,  /* Corr desc: FC_SHORT */
   0x0,  /*  */
/* 10 */ 0xfc,0xff, /* -4 */
/* 12 */ 0x01,0x00, /* Corr flags:  early, */
/* 14 */ 0x02,0x00, /* Offset= 2 (16) */
/* 16 */ 0x08,0x00, /* 8 */
/* 18 */ 0x04,0x00, /* 4 */
/* 20 */ 0x01,0x00,0x00,0x00, /* 1 */
/* 24 */ 0x2c,0x00, /* Offset= 44 (68) */
/* 26 */ 0x02,0x00,0x00,0x00, /* 2 */
/* 30 */ 0x42,0x00, /* Offset= 66 (96) */
/* 32 */ 0x03,0x00,0x00,0x00, /* 3 */
/* 36 */ 0x78,0x00, /* Offset= 120 (156) */
/* 38 */ 0x06,0x00,0x00,0x00, /* 6 */
/* 42 */ 0x8e,0x00, /* Offset= 142 (184) */
/* 44 */ 0x00,0x00, /* Offset= 0 (44) */
/* 46 */ 0xb7,  /* FC_RANGE */
   0x8,  /* 8 */
/* 48 */ 0x01,0x00,0x00,0x00, /* 1 */
/* 52 */ 0x00,0x00,0xa0,0x00, /* 10485760 */
/* 56 */ 
   0x1b,  /* FC_CARRAY */
   0x7,  /* 7 */
/* 58 */ 0x08,0x00, /* 8 */
/* 60 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 62 */ 0x00,0x00, /* 0 */
/* 64 */ 0x01,0x00, /* Corr flags:  early, */
/* 66 */ 0xb,  /* FC_HYPER */
   0x5b,  /* FC_END */
/* 68 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 70 */ 0x08,0x00, /* 8 */
/* 72 */ 0x00,0x00, /* 0 */
/* 74 */ 0x08,0x00, /* Offset= 8 (82) */
/* 76 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 78 */ 0xe0,0xff, /* Offset= -32 (46) */
/* 80 */ 0x36,  /* FC_POINTER */
   0x5b,  /* FC_END */
/* 82 */ 
   0x12, 0x0, /* FC_UP */
/* 84 */ 0xe4,0xff, /* Offset= -28 (56) */
/* 86 */ 0xb7,  /* FC_RANGE */
   0x8,  /* 8 */
/* 88 */ 0x01,0x00,0x00,0x00, /* 1 */
/* 92 */ 0x00,0x00,0xa0,0x00, /* 10485760 */
/* 96 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 98 */ 0x08,0x00, /* 8 */
/* 100 */ 0x00,0x00, /* 0 */
/* 102 */ 0x08,0x00, /* Offset= 8 (110) */
/* 104 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 106 */ 0xec,0xff, /* Offset= -20 (86) */
/* 108 */ 0x36,  /* FC_POINTER */
   0x5b,  /* FC_END */
/* 110 */ 
   0x12, 0x0, /* FC_UP */
/* 112 */ 0xc8,0xff, /* Offset= -56 (56) */
/* 114 */ 0xb7,  /* FC_RANGE */
   0x8,  /* 8 */
/* 116 */ 0x01,0x00,0x00,0x00, /* 1 */
/* 120 */ 0x00,0x00,0xa0,0x00, /* 10485760 */
/* 124 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 126 */ 0x04,0x00, /* 4 */
/* 128 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 130 */ 0x00,0x00, /* 0 */
/* 132 */ 0x01,0x00, /* Corr flags:  early, */
/* 134 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 136 */ 
   0x48,  /* FC_VARIABLE_REPEAT */
   0x49,  /* FC_FIXED_OFFSET */
/* 138 */ 0x04,0x00, /* 4 */
/* 140 */ 0x00,0x00, /* 0 */
/* 142 */ 0x01,0x00, /* 1 */
/* 144 */ 0x00,0x00, /* 0 */
/* 146 */ 0x00,0x00, /* 0 */
/* 148 */ 0x12, 0x8, /* FC_UP [simple_pointer] */
/* 150 */ 
   0x25,  /* FC_C_WSTRING */
   0x5c,  /* FC_PAD */
/* 152 */ 
   0x5b,  /* FC_END */
   0x8,  /* FC_LONG */
/* 154 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 156 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 158 */ 0x08,0x00, /* 8 */
/* 160 */ 0x00,0x00, /* 0 */
/* 162 */ 0x08,0x00, /* Offset= 8 (170) */
/* 164 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 166 */ 0xcc,0xff, /* Offset= -52 (114) */
/* 168 */ 0x36,  /* FC_POINTER */
   0x5b,  /* FC_END */
/* 170 */ 
   0x12, 0x0, /* FC_UP */
/* 172 */ 0xd0,0xff, /* Offset= -48 (124) */
/* 174 */ 0xb7,  /* FC_RANGE */
   0x8,  /* 8 */
/* 176 */ 0x01,0x00,0x00,0x00, /* 1 */
/* 180 */ 0x00,0x00,0xa0,0x00, /* 10485760 */
/* 184 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 186 */ 0x08,0x00, /* 8 */
/* 188 */ 0x00,0x00, /* 0 */
/* 190 */ 0x08,0x00, /* Offset= 8 (198) */
/* 192 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 194 */ 0xec,0xff, /* Offset= -20 (174) */
/* 196 */ 0x36,  /* FC_POINTER */
   0x5b,  /* FC_END */
/* 198 */ 
   0x12, 0x0, /* FC_UP */
/* 200 */ 0x70,0xff, /* Offset= -144 (56) */
/* 202 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 204 */ 0x10,0x00, /* 16 */
/* 206 */ 0x00,0x00, /* 0 */
/* 208 */ 0x0a,0x00, /* Offset= 10 (218) */
/* 210 */ 0x36,  /* FC_POINTER */
   0xd,  /* FC_ENUM16 */
/* 212 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 214 */ 0x30,0xff, /* Offset= -208 (6) */
/* 216 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 218 */ 
   0x12, 0x8, /* FC_UP [simple_pointer] */
/* 220 */ 
   0x25,  /* FC_C_WSTRING */
   0x5c,  /* FC_PAD */
/* 222 */ 
   0x21,  /* FC_BOGUS_ARRAY */
   0x3,  /* 3 */
/* 224 */ 0x00,0x00, /* 0 */
/* 226 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 228 */ 0x04,0x00, /* 4 */
/* 230 */ 0x01,0x00, /* Corr flags:  early, */
/* 232 */ 0xff,0xff,0xff,0xff, /* -1 */
/* 236 */ 0x00,0x00, /* Corr flags:  */
/* 238 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 240 */ 0xda,0xff, /* Offset= -38 (202) */
/* 242 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 244 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 246 */ 0x0c,0x00, /* 12 */
/* 248 */ 0x00,0x00, /* 0 */
/* 250 */ 0x06,0x00, /* Offset= 6 (256) */
/* 252 */ 0xd,  /* FC_ENUM16 */
   0x8,  /* FC_LONG */
/* 254 */ 0x36,  /* FC_POINTER */
   0x5b,  /* FC_END */
/* 256 */ 
   0x12, 0x0, /* FC_UP */
/* 258 */ 0xdc,0xff, /* Offset= -36 (222) */
/* 260 */ 
   0x21,  /* FC_BOGUS_ARRAY */
   0x3,  /* 3 */
/* 262 */ 0x00,0x00, /* 0 */
/* 264 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 266 */ 0x00,0x00, /* 0 */
/* 268 */ 0x01,0x00, /* Corr flags:  early, */
/* 270 */ 0xff,0xff,0xff,0xff, /* -1 */
/* 274 */ 0x00,0x00, /* Corr flags:  */
/* 276 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 278 */ 0xde,0xff, /* Offset= -34 (244) */
/* 280 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 282 */ 
   0x1b,  /* FC_CARRAY */
   0x0,  /* 0 */
/* 284 */ 0x01,0x00, /* 1 */
/* 286 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 288 */ 0x0c,0x00, /* 12 */
/* 290 */ 0x01,0x00, /* Corr flags:  early, */
/* 292 */ 0x2,  /* FC_CHAR */
   0x5b,  /* FC_END */
/* 294 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 296 */ 0x14,0x00, /* 20 */
/* 298 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 300 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 302 */ 0x04,0x00, /* 4 */
/* 304 */ 0x04,0x00, /* 4 */
/* 306 */ 0x12, 0x0, /* FC_UP */
/* 308 */ 0xd0,0xff, /* Offset= -48 (260) */
/* 310 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 312 */ 0x10,0x00, /* 16 */
/* 314 */ 0x10,0x00, /* 16 */
/* 316 */ 0x12, 0x0, /* FC_UP */
/* 318 */ 0xdc,0xff, /* Offset= -36 (282) */
/* 320 */ 
   0x5b,  /* FC_END */
   0x8,  /* FC_LONG */
/* 322 */ 0x8,  /* FC_LONG */
   0x6,  /* FC_SHORT */
/* 324 */ 0x3e,  /* FC_STRUCTPAD2 */
   0x8,  /* FC_LONG */
/* 326 */ 0x8,  /* FC_LONG */
   0x5b,  /* FC_END */
/* 328 */ 
   0x12, 0x0, /* FC_UP */
/* 330 */ 0x52,0x00, /* Offset= 82 (412) */
/* 332 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 334 */ 0x02,0x00, /* 2 */
/* 336 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 338 */ 0x02,0x00, /* 2 */
/* 340 */ 0x01,0x00, /* Corr flags:  early, */
/* 342 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 344 */ 0x00,0x00, /* 0 */
/* 346 */ 0x01,0x00, /* Corr flags:  early, */
/* 348 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 350 */ 
   0x1b,  /* FC_CARRAY */
   0x0,  /* 0 */
/* 352 */ 0x01,0x00, /* 1 */
/* 354 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 356 */ 0x08,0x00, /* 8 */
/* 358 */ 0x01,0x00, /* Corr flags:  early, */
/* 360 */ 0x2,  /* FC_CHAR */
   0x5b,  /* FC_END */
/* 362 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 364 */ 0x10,0x00, /* 16 */
/* 366 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 368 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 370 */ 0x04,0x00, /* 4 */
/* 372 */ 0x04,0x00, /* 4 */
/* 374 */ 0x12, 0x0, /* FC_UP */
/* 376 */ 0xd4,0xff, /* Offset= -44 (332) */
/* 378 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 380 */ 0x0c,0x00, /* 12 */
/* 382 */ 0x0c,0x00, /* 12 */
/* 384 */ 0x12, 0x0, /* FC_UP */
/* 386 */ 0xdc,0xff, /* Offset= -36 (350) */
/* 388 */ 
   0x5b,  /* FC_END */
   0x6,  /* FC_SHORT */
/* 390 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 392 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 394 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 396 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 398 */ 0x10,0x00, /* 16 */
/* 400 */ 0x9,  /* Corr desc: FC_ULONG */
   0x0,  /*  */
/* 402 */ 0xfc,0xff, /* -4 */
/* 404 */ 0x01,0x00, /* Corr flags:  early, */
/* 406 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 408 */ 0xd2,0xff, /* Offset= -46 (362) */
/* 410 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 412 */ 
   0x18,  /* FC_CPSTRUCT */
   0x3,  /* 3 */
/* 414 */ 0x04,0x00, /* 4 */
/* 416 */ 0xec,0xff, /* Offset= -20 (396) */
/* 418 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 420 */ 
   0x48,  /* FC_VARIABLE_REPEAT */
   0x49,  /* FC_FIXED_OFFSET */
/* 422 */ 0x10,0x00, /* 16 */
/* 424 */ 0x04,0x00, /* 4 */
/* 426 */ 0x02,0x00, /* 2 */
/* 428 */ 0x08,0x00, /* 8 */
/* 430 */ 0x08,0x00, /* 8 */
/* 432 */ 0x12, 0x0, /* FC_UP */
/* 434 */ 0x9a,0xff, /* Offset= -102 (332) */
/* 436 */ 0x10,0x00, /* 16 */
/* 438 */ 0x10,0x00, /* 16 */
/* 440 */ 0x12, 0x0, /* FC_UP */
/* 442 */ 0xa4,0xff, /* Offset= -92 (350) */
/* 444 */ 
   0x5b,  /* FC_END */
   0x8,  /* FC_LONG */
/* 446 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 448 */ 
   0x12, 0x0, /* FC_UP */
/* 450 */ 0x08,0x00, /* Offset= 8 (458) */
/* 452 */ 
   0x1d,  /* FC_SMFARRAY */
   0x0,  /* 0 */
/* 454 */ 0x10,0x00, /* 16 */
/* 456 */ 0x2,  /* FC_CHAR */
   0x5b,  /* FC_END */
/* 458 */ 
   0x15,  /* FC_STRUCT */
   0x3,  /* 3 */
/* 460 */ 0x28,0x00, /* 40 */
/* 462 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 464 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 466 */ 0xf2,0xff, /* Offset= -14 (452) */
/* 468 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 470 */ 0xee,0xff, /* Offset= -18 (452) */
/* 472 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 474 */ 
   0x12, 0x0, /* FC_UP */
/* 476 */ 0x4a,0x01, /* Offset= 330 (806) */
/* 478 */ 
   0x15,  /* FC_STRUCT */
   0x3,  /* 3 */
/* 480 */ 0x08,0x00, /* 8 */
/* 482 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 484 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 486 */ 
   0x1d,  /* FC_SMFARRAY */
   0x0,  /* 0 */
/* 488 */ 0x08,0x00, /* 8 */
/* 490 */ 0x2,  /* FC_CHAR */
   0x5b,  /* FC_END */
/* 492 */ 
   0x15,  /* FC_STRUCT */
   0x0,  /* 0 */
/* 494 */ 0x08,0x00, /* 8 */
/* 496 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 498 */ 0xf4,0xff, /* Offset= -12 (486) */
/* 500 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 502 */ 
   0x1d,  /* FC_SMFARRAY */
   0x0,  /* 0 */
/* 504 */ 0x10,0x00, /* 16 */
/* 506 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 508 */ 0xf0,0xff, /* Offset= -16 (492) */
/* 510 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 512 */ 
   0x15,  /* FC_STRUCT */
   0x0,  /* 0 */
/* 514 */ 0x10,0x00, /* 16 */
/* 516 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 518 */ 0xf0,0xff, /* Offset= -16 (502) */
/* 520 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 522 */ 
   0x1d,  /* FC_SMFARRAY */
   0x3,  /* 3 */
/* 524 */ 0x08,0x00, /* 8 */
/* 526 */ 0x8,  /* FC_LONG */
   0x5b,  /* FC_END */
/* 528 */ 
   0x1d,  /* FC_SMFARRAY */
   0x3,  /* 3 */
/* 530 */ 0x1c,0x00, /* 28 */
/* 532 */ 0x8,  /* FC_LONG */
   0x5b,  /* FC_END */
/* 534 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 536 */ 0x02,0x00, /* 2 */
/* 538 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 540 */ 0x32,0x00, /* 50 */
/* 542 */ 0x01,0x00, /* Corr flags:  early, */
/* 544 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 546 */ 0x30,0x00, /* 48 */
/* 548 */ 0x01,0x00, /* Corr flags:  early, */
/* 550 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 552 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 554 */ 0x02,0x00, /* 2 */
/* 556 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 558 */ 0x3a,0x00, /* 58 */
/* 560 */ 0x01,0x00, /* Corr flags:  early, */
/* 562 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 564 */ 0x38,0x00, /* 56 */
/* 566 */ 0x01,0x00, /* Corr flags:  early, */
/* 568 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 570 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 572 */ 0x02,0x00, /* 2 */
/* 574 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 576 */ 0x42,0x00, /* 66 */
/* 578 */ 0x01,0x00, /* Corr flags:  early, */
/* 580 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 582 */ 0x40,0x00, /* 64 */
/* 584 */ 0x01,0x00, /* Corr flags:  early, */
/* 586 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 588 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 590 */ 0x02,0x00, /* 2 */
/* 592 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 594 */ 0x4a,0x00, /* 74 */
/* 596 */ 0x01,0x00, /* Corr flags:  early, */
/* 598 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 600 */ 0x48,0x00, /* 72 */
/* 602 */ 0x01,0x00, /* Corr flags:  early, */
/* 604 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 606 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 608 */ 0x02,0x00, /* 2 */
/* 610 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 612 */ 0x52,0x00, /* 82 */
/* 614 */ 0x01,0x00, /* Corr flags:  early, */
/* 616 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 618 */ 0x50,0x00, /* 80 */
/* 620 */ 0x01,0x00, /* Corr flags:  early, */
/* 622 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 624 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 626 */ 0x02,0x00, /* 2 */
/* 628 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 630 */ 0x5a,0x00, /* 90 */
/* 632 */ 0x01,0x00, /* Corr flags:  early, */
/* 634 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 636 */ 0x58,0x00, /* 88 */
/* 638 */ 0x01,0x00, /* Corr flags:  early, */
/* 640 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 642 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 644 */ 0x08,0x00, /* 8 */
/* 646 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 648 */ 0x6c,0x00, /* 108 */
/* 650 */ 0x01,0x00, /* Corr flags:  early, */
/* 652 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 654 */ 0x50,0xff, /* Offset= -176 (478) */
/* 656 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 658 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 660 */ 0x02,0x00, /* 2 */
/* 662 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 664 */ 0x8a,0x00, /* 138 */
/* 666 */ 0x01,0x00, /* Corr flags:  early, */
/* 668 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 670 */ 0x88,0x00, /* 136 */
/* 672 */ 0x01,0x00, /* Corr flags:  early, */
/* 674 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 676 */ 
   0x1c,  /* FC_CVARRAY */
   0x1,  /* 1 */
/* 678 */ 0x02,0x00, /* 2 */
/* 680 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 682 */ 0x92,0x00, /* 146 */
/* 684 */ 0x01,0x00, /* Corr flags:  early, */
/* 686 */ 0x17,  /* Corr desc:  field pointer, FC_USHORT */
   0x55,  /* FC_DIV_2 */
/* 688 */ 0x90,0x00, /* 144 */
/* 690 */ 0x01,0x00, /* Corr flags:  early, */
/* 692 */ 0x5,  /* FC_WCHAR */
   0x5b,  /* FC_END */
/* 694 */ 
   0x1d,  /* FC_SMFARRAY */
   0x0,  /* 0 */
/* 696 */ 0x06,0x00, /* 6 */
/* 698 */ 0x1,  /* FC_BYTE */
   0x5b,  /* FC_END */
/* 700 */ 
   0x15,  /* FC_STRUCT */
   0x0,  /* 0 */
/* 702 */ 0x06,0x00, /* 6 */
/* 704 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 706 */ 0xf4,0xff, /* Offset= -12 (694) */
/* 708 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 710 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 712 */ 0x04,0x00, /* 4 */
/* 714 */ 0x4,  /* Corr desc: FC_USMALL */
   0x0,  /*  */
/* 716 */ 0xf9,0xff, /* -7 */
/* 718 */ 0x01,0x00, /* Corr flags:  early, */
/* 720 */ 0x8,  /* FC_LONG */
   0x5b,  /* FC_END */
/* 722 */ 
   0x17,  /* FC_CSTRUCT */
   0x3,  /* 3 */
/* 724 */ 0x08,0x00, /* 8 */
/* 726 */ 0xf0,0xff, /* Offset= -16 (710) */
/* 728 */ 0x2,  /* FC_CHAR */
   0x2,  /* FC_CHAR */
/* 730 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 732 */ 0xe0,0xff, /* Offset= -32 (700) */
/* 734 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 736 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 738 */ 0x08,0x00, /* 8 */
/* 740 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 742 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 744 */ 0x00,0x00, /* 0 */
/* 746 */ 0x00,0x00, /* 0 */
/* 748 */ 0x12, 0x0, /* FC_UP */
/* 750 */ 0xe4,0xff, /* Offset= -28 (722) */
/* 752 */ 
   0x5b,  /* FC_END */
   0x8,  /* FC_LONG */
/* 754 */ 0x8,  /* FC_LONG */
   0x5b,  /* FC_END */
/* 756 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 758 */ 0x08,0x00, /* 8 */
/* 760 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 762 */ 0xc4,0x00, /* 196 */
/* 764 */ 0x01,0x00, /* Corr flags:  early, */
/* 766 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 768 */ 
   0x48,  /* FC_VARIABLE_REPEAT */
   0x49,  /* FC_FIXED_OFFSET */
/* 770 */ 0x08,0x00, /* 8 */
/* 772 */ 0x00,0x00, /* 0 */
/* 774 */ 0x01,0x00, /* 1 */
/* 776 */ 0x00,0x00, /* 0 */
/* 778 */ 0x00,0x00, /* 0 */
/* 780 */ 0x12, 0x0, /* FC_UP */
/* 782 */ 0xc4,0xff, /* Offset= -60 (722) */
/* 784 */ 
   0x5b,  /* FC_END */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 786 */ 0x0,  /* 0 */
0xcd,0xff, /* Offset= -51 (736) */
   0x5b,  /* FC_END */
/* 790 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 792 */ 0x08,0x00, /* 8 */
/* 794 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 796 */ 0xd0,0x00, /* 208 */
/* 798 */ 0x01,0x00, /* Corr flags:  early, */
/* 800 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 802 */ 0xbc,0xfe, /* Offset= -324 (478) */
/* 804 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 806 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 808 */ 0xd8,0x00, /* 216 */
/* 810 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 812 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 814 */ 0x34,0x00, /* 52 */
/* 816 */ 0x34,0x00, /* 52 */
/* 818 */ 0x12, 0x0, /* FC_UP */
/* 820 */ 0xe2,0xfe, /* Offset= -286 (534) */
/* 822 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 824 */ 0x3c,0x00, /* 60 */
/* 826 */ 0x3c,0x00, /* 60 */
/* 828 */ 0x12, 0x0, /* FC_UP */
/* 830 */ 0xea,0xfe, /* Offset= -278 (552) */
/* 832 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 834 */ 0x44,0x00, /* 68 */
/* 836 */ 0x44,0x00, /* 68 */
/* 838 */ 0x12, 0x0, /* FC_UP */
/* 840 */ 0xf2,0xfe, /* Offset= -270 (570) */
/* 842 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 844 */ 0x4c,0x00, /* 76 */
/* 846 */ 0x4c,0x00, /* 76 */
/* 848 */ 0x12, 0x0, /* FC_UP */
/* 850 */ 0xfa,0xfe, /* Offset= -262 (588) */
/* 852 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 854 */ 0x54,0x00, /* 84 */
/* 856 */ 0x54,0x00, /* 84 */
/* 858 */ 0x12, 0x0, /* FC_UP */
/* 860 */ 0x02,0xff, /* Offset= -254 (606) */
/* 862 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 864 */ 0x5c,0x00, /* 92 */
/* 866 */ 0x5c,0x00, /* 92 */
/* 868 */ 0x12, 0x0, /* FC_UP */
/* 870 */ 0x0a,0xff, /* Offset= -246 (624) */
/* 872 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 874 */ 0x70,0x00, /* 112 */
/* 876 */ 0x70,0x00, /* 112 */
/* 878 */ 0x12, 0x0, /* FC_UP */
/* 880 */ 0x12,0xff, /* Offset= -238 (642) */
/* 882 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 884 */ 0x8c,0x00, /* 140 */
/* 886 */ 0x8c,0x00, /* 140 */
/* 888 */ 0x12, 0x0, /* FC_UP */
/* 890 */ 0x18,0xff, /* Offset= -232 (658) */
/* 892 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 894 */ 0x94,0x00, /* 148 */
/* 896 */ 0x94,0x00, /* 148 */
/* 898 */ 0x12, 0x0, /* FC_UP */
/* 900 */ 0x20,0xff, /* Offset= -224 (676) */
/* 902 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 904 */ 0x98,0x00, /* 152 */
/* 906 */ 0x98,0x00, /* 152 */
/* 908 */ 0x12, 0x0, /* FC_UP */
/* 910 */ 0x44,0xff, /* Offset= -188 (722) */
/* 912 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 914 */ 0xc8,0x00, /* 200 */
/* 916 */ 0xc8,0x00, /* 200 */
/* 918 */ 0x12, 0x0, /* FC_UP */
/* 920 */ 0x5c,0xff, /* Offset= -164 (756) */
/* 922 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 924 */ 0xcc,0x00, /* 204 */
/* 926 */ 0xcc,0x00, /* 204 */
/* 928 */ 0x12, 0x0, /* FC_UP */
/* 930 */ 0x30,0xff, /* Offset= -208 (722) */
/* 932 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 934 */ 0xd4,0x00, /* 212 */
/* 936 */ 0xd4,0x00, /* 212 */
/* 938 */ 0x12, 0x0, /* FC_UP */
/* 940 */ 0x6a,0xff, /* Offset= -150 (790) */
/* 942 */ 
   0x5b,  /* FC_END */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 944 */ 0x0,  /* 0 */
0x2d,0xfe, /* Offset= -467 (478) */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 948 */ 0x0,  /* 0 */
0x29,0xfe, /* Offset= -471 (478) */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 952 */ 0x0,  /* 0 */
0x25,0xfe, /* Offset= -475 (478) */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 956 */ 0x0,  /* 0 */
0x21,0xfe, /* Offset= -479 (478) */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 960 */ 0x0,  /* 0 */
0x1d,0xfe, /* Offset= -483 (478) */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 964 */ 0x0,  /* 0 */
0x19,0xfe, /* Offset= -487 (478) */
   0x6,  /* FC_SHORT */
/* 968 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 970 */ 0x6,  /* FC_SHORT */
   0x6,  /* FC_SHORT */
/* 972 */ 0x8,  /* FC_LONG */
   0x6,  /* FC_SHORT */
/* 974 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 976 */ 0x6,  /* FC_SHORT */
   0x6,  /* FC_SHORT */
/* 978 */ 0x8,  /* FC_LONG */
   0x6,  /* FC_SHORT */
/* 980 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 982 */ 0x6,  /* FC_SHORT */
   0x6,  /* FC_SHORT */
/* 984 */ 0x8,  /* FC_LONG */
   0x6,  /* FC_SHORT */
/* 986 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 988 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 990 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 992 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 994 */ 0x1e,0xfe, /* Offset= -482 (512) */
/* 996 */ 0x6,  /* FC_SHORT */
   0x6,  /* FC_SHORT */
/* 998 */ 0x8,  /* FC_LONG */
   0x6,  /* FC_SHORT */
/* 1000 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 1002 */ 0x8,  /* FC_LONG */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 1004 */ 0x0,  /* 0 */
0x1d,0xfe, /* Offset= -483 (522) */
   0x8,  /* FC_LONG */
/* 1008 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 1010 */ 0x1e,0xfe, /* Offset= -482 (528) */
/* 1012 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1014 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1016 */ 0x8,  /* FC_LONG */
   0x5b,  /* FC_END */
/* 1018 */ 
   0x12, 0x0, /* FC_UP */
/* 1020 */ 0x3a,0x00, /* Offset= 58 (1078) */
/* 1022 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 1024 */ 0x08,0x00, /* 8 */
/* 1026 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 1028 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1030 */ 0x04,0x00, /* 4 */
/* 1032 */ 0x04,0x00, /* 4 */
/* 1034 */ 0x12, 0x0, /* FC_UP */
/* 1036 */ 0x40,0xfd, /* Offset= -704 (332) */
/* 1038 */ 
   0x5b,  /* FC_END */
   0x6,  /* FC_SHORT */
/* 1040 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 1042 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1044 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 1046 */ 0x08,0x00, /* 8 */
/* 1048 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 1050 */ 0x08,0x00, /* 8 */
/* 1052 */ 0x01,0x00, /* Corr flags:  early, */
/* 1054 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 1056 */ 
   0x48,  /* FC_VARIABLE_REPEAT */
   0x49,  /* FC_FIXED_OFFSET */
/* 1058 */ 0x08,0x00, /* 8 */
/* 1060 */ 0x00,0x00, /* 0 */
/* 1062 */ 0x01,0x00, /* 1 */
/* 1064 */ 0x04,0x00, /* 4 */
/* 1066 */ 0x04,0x00, /* 4 */
/* 1068 */ 0x12, 0x0, /* FC_UP */
/* 1070 */ 0x1e,0xfd, /* Offset= -738 (332) */
/* 1072 */ 
   0x5b,  /* FC_END */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 1074 */ 0x0,  /* 0 */
0xcb,0xff, /* Offset= -53 (1022) */
   0x5b,  /* FC_END */
/* 1078 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 1080 */ 0x10,0x00, /* 16 */
/* 1082 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 1084 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1086 */ 0x04,0x00, /* 4 */
/* 1088 */ 0x04,0x00, /* 4 */
/* 1090 */ 0x12, 0x0, /* FC_UP */
/* 1092 */ 0x08,0xfd, /* Offset= -760 (332) */
/* 1094 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1096 */ 0x0c,0x00, /* 12 */
/* 1098 */ 0x0c,0x00, /* 12 */
/* 1100 */ 0x12, 0x0, /* FC_UP */
/* 1102 */ 0xc6,0xff, /* Offset= -58 (1044) */
/* 1104 */ 
   0x5b,  /* FC_END */
   0x6,  /* FC_SHORT */
/* 1106 */ 0x6,  /* FC_SHORT */
   0x8,  /* FC_LONG */
/* 1108 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1110 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1112 */ 
   0x12, 0x0, /* FC_UP */
/* 1114 */ 0x34,0x00, /* Offset= 52 (1166) */
/* 1116 */ 
   0x1b,  /* FC_CARRAY */
   0x0,  /* 0 */
/* 1118 */ 0x01,0x00, /* 1 */
/* 1120 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 1122 */ 0x00,0x00, /* 0 */
/* 1124 */ 0x01,0x00, /* Corr flags:  early, */
/* 1126 */ 0x2,  /* FC_CHAR */
   0x5b,  /* FC_END */
/* 1128 */ 
   0x1b,  /* FC_CARRAY */
   0x0,  /* 0 */
/* 1130 */ 0x01,0x00, /* 1 */
/* 1132 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 1134 */ 0x14,0x00, /* 20 */
/* 1136 */ 0x01,0x00, /* Corr flags:  early, */
/* 1138 */ 0x2,  /* FC_CHAR */
   0x5b,  /* FC_END */
/* 1140 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 1142 */ 0x1c,0x00, /* 28 */
/* 1144 */ 0x00,0x00, /* 0 */
/* 1146 */ 0x0c,0x00, /* Offset= 12 (1158) */
/* 1148 */ 0x8,  /* FC_LONG */
   0x36,  /* FC_POINTER */
/* 1150 */ 0xd,  /* FC_ENUM16 */
   0x8,  /* FC_LONG */
/* 1152 */ 0x6,  /* FC_SHORT */
   0x3e,  /* FC_STRUCTPAD2 */
/* 1154 */ 0x8,  /* FC_LONG */
   0x36,  /* FC_POINTER */
/* 1156 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1158 */ 
   0x12, 0x0, /* FC_UP */
/* 1160 */ 0xd4,0xff, /* Offset= -44 (1116) */
/* 1162 */ 
   0x12, 0x0, /* FC_UP */
/* 1164 */ 0xdc,0xff, /* Offset= -36 (1128) */
/* 1166 */ 
   0x1a,  /* FC_BOGUS_STRUCT */
   0x3,  /* 3 */
/* 1168 */ 0x1c,0x00, /* 28 */
/* 1170 */ 0x00,0x00, /* 0 */
/* 1172 */ 0x00,0x00, /* Offset= 0 (1172) */
/* 1174 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 1176 */ 0xdc,0xff, /* Offset= -36 (1140) */
/* 1178 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1180 */ 
   0x12, 0x1, /* FC_UP [all_nodes] */
/* 1182 */ 0x8e,0x00, /* Offset= 142 (1324) */
/* 1184 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 1186 */ 0x08,0x00, /* 8 */
/* 1188 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 1190 */ 0x0c,0x00, /* 12 */
/* 1192 */ 0x01,0x00, /* Corr flags:  early, */
/* 1194 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 1196 */ 0x32,0xfd, /* Offset= -718 (478) */
/* 1198 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1200 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 1202 */ 0x08,0x00, /* 8 */
/* 1204 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 1206 */ 0x14,0x00, /* 20 */
/* 1208 */ 0x01,0x00, /* Corr flags:  early, */
/* 1210 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 1212 */ 
   0x48,  /* FC_VARIABLE_REPEAT */
   0x49,  /* FC_FIXED_OFFSET */
/* 1214 */ 0x08,0x00, /* 8 */
/* 1216 */ 0x00,0x00, /* 0 */
/* 1218 */ 0x01,0x00, /* 1 */
/* 1220 */ 0x00,0x00, /* 0 */
/* 1222 */ 0x00,0x00, /* 0 */
/* 1224 */ 0x12, 0x0, /* FC_UP */
/* 1226 */ 0x08,0xfe, /* Offset= -504 (722) */
/* 1228 */ 
   0x5b,  /* FC_END */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 1230 */ 0x0,  /* 0 */
0x11,0xfe, /* Offset= -495 (736) */
   0x5b,  /* FC_END */
/* 1234 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 1236 */ 0x08,0x00, /* 8 */
/* 1238 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 1240 */ 0x04,0x00, /* 4 */
/* 1242 */ 0x01,0x00, /* Corr flags:  early, */
/* 1244 */ 0x4c,  /* FC_EMBEDDED_COMPLEX */
   0x0,  /* 0 */
/* 1246 */ 0x00,0xfd, /* Offset= -768 (478) */
/* 1248 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1250 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 1252 */ 0x0c,0x00, /* 12 */
/* 1254 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 1256 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1258 */ 0x00,0x00, /* 0 */
/* 1260 */ 0x00,0x00, /* 0 */
/* 1262 */ 0x12, 0x0, /* FC_UP */
/* 1264 */ 0xe2,0xfd, /* Offset= -542 (722) */
/* 1266 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1268 */ 0x08,0x00, /* 8 */
/* 1270 */ 0x08,0x00, /* 8 */
/* 1272 */ 0x12, 0x0, /* FC_UP */
/* 1274 */ 0xd8,0xff, /* Offset= -40 (1234) */
/* 1276 */ 
   0x5b,  /* FC_END */
   0x8,  /* FC_LONG */
/* 1278 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1280 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1282 */ 
   0x1b,  /* FC_CARRAY */
   0x3,  /* 3 */
/* 1284 */ 0x0c,0x00, /* 12 */
/* 1286 */ 0x19,  /* Corr desc:  field pointer, FC_ULONG */
   0x0,  /*  */
/* 1288 */ 0x1c,0x00, /* 28 */
/* 1290 */ 0x01,0x00, /* Corr flags:  early, */
/* 1292 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 1294 */ 
   0x48,  /* FC_VARIABLE_REPEAT */
   0x49,  /* FC_FIXED_OFFSET */
/* 1296 */ 0x0c,0x00, /* 12 */
/* 1298 */ 0x00,0x00, /* 0 */
/* 1300 */ 0x02,0x00, /* 2 */
/* 1302 */ 0x00,0x00, /* 0 */
/* 1304 */ 0x00,0x00, /* 0 */
/* 1306 */ 0x12, 0x0, /* FC_UP */
/* 1308 */ 0xb6,0xfd, /* Offset= -586 (722) */
/* 1310 */ 0x08,0x00, /* 8 */
/* 1312 */ 0x08,0x00, /* 8 */
/* 1314 */ 0x12, 0x0, /* FC_UP */
/* 1316 */ 0xae,0xff, /* Offset= -82 (1234) */
/* 1318 */ 
   0x5b,  /* FC_END */
   0x4c,  /* FC_EMBEDDED_COMPLEX */
/* 1320 */ 0x0,  /* 0 */
0xb9,0xff, /* Offset= -71 (1250) */
   0x5b,  /* FC_END */
/* 1324 */ 
   0x16,  /* FC_PSTRUCT */
   0x3,  /* 3 */
/* 1326 */ 0x24,0x00, /* 36 */
/* 1328 */ 
   0x4b,  /* FC_PP */
   0x5c,  /* FC_PAD */
/* 1330 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1332 */ 0x08,0x00, /* 8 */
/* 1334 */ 0x08,0x00, /* 8 */
/* 1336 */ 0x12, 0x0, /* FC_UP */
/* 1338 */ 0x98,0xfd, /* Offset= -616 (722) */
/* 1340 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1342 */ 0x10,0x00, /* 16 */
/* 1344 */ 0x10,0x00, /* 16 */
/* 1346 */ 0x12, 0x0, /* FC_UP */
/* 1348 */ 0x5c,0xff, /* Offset= -164 (1184) */
/* 1350 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1352 */ 0x18,0x00, /* 24 */
/* 1354 */ 0x18,0x00, /* 24 */
/* 1356 */ 0x12, 0x0, /* FC_UP */
/* 1358 */ 0x62,0xff, /* Offset= -158 (1200) */
/* 1360 */ 
   0x46,  /* FC_NO_REPEAT */
   0x5c,  /* FC_PAD */
/* 1362 */ 0x20,0x00, /* 32 */
/* 1364 */ 0x20,0x00, /* 32 */
/* 1366 */ 0x12, 0x0, /* FC_UP */
/* 1368 */ 0xaa,0xff, /* Offset= -86 (1282) */
/* 1370 */ 
   0x5b,  /* FC_END */
   0x8,  /* FC_LONG */
/* 1372 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1374 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1376 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1378 */ 0x8,  /* FC_LONG */
   0x8,  /* FC_LONG */
/* 1380 */ 0x5c,  /* FC_PAD */
   0x5b,  /* FC_END */
/* 1382 */ 
   0x12, 0x1, /* FC_UP [all_nodes] */
/* 1384 */ 0x26,0xff, /* Offset= -218 (1166) */
   0x0


            };
        #endregion
        internal const int OffsetDeviceClaimsInfo = 1382;
        internal const int OffsetDeviceInfo = 1180;
        internal const int OffsetClaimSet = 2;
        internal const int OffsetClientClaim = 1112;
        /// <summary>
        /// Format offset of NDR-marshalled structure KERB_VALIDATION_INFO.
        /// Copied from midl.exe generated RPC stub code.
        /// More details about NDR encoding/decoding and format string
        /// please refer summary of Common\Ndr\NdrMarshaller class.
        /// </summary>
        internal const int OffsetKerb = 474;
        /// Format offset of NDR-marshalled structure _S4U_DELEGATION_INFO.
        /// Copied from midl.exe generated RPC stub code.
        /// More details about NDR encoding/decoding and format string
        /// please refer summary of Common\Ndr\NdrMarshaller class.
        internal const int OffsetS4u = 1018;
        /// Format offset of NDR-marshalled structure _NTLM_SUPPLEMENTAL_CREDENTIAL.
        /// Copied from midl.exe generated RPC stub code.
        /// More details about NDR encoding/decoding and format string
        /// please refer summary of Common\Ndr\NdrMarshaller class.
        internal const int OffsetNtlm = 448;
        /// Format offset of NDR-marshalled structure _PAC_CREDENTIAL_DATA.
        /// Copied from midl.exe generated RPC stub code.
        /// More details about NDR encoding/decoding and format string
        /// please refer summary of Common\Ndr\NdrMarshaller class.
        internal const int OffsetCredentialData = 328;
    }
}
