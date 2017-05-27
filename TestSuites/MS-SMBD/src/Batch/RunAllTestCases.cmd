:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
"%VS110COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "..\Bin\MS-SMBD_ServerTestSuite.dll" /Settings:..\Bin\ServerLocal.TestSettings /Logger:trx
pause