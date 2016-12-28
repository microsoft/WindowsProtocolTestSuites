:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

"%VS110COMNTOOLS%..\IDE\mstest" /category:Traditional /testcontainer:..\Bin\MS-SMB_ServerTestSuite.dll /runconfig:..\Bin\ServerLocalTestRun.testrunconfig 
pause