:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

"%VS110COMNTOOLS%..\IDE\mstest" /category:"BVT" /testcontainer:"..\Bin\MS-AZOD_ODTestSuite.dll" /runconfig:..\Bin\ODLocalTestRun.testrunconfig 
pause