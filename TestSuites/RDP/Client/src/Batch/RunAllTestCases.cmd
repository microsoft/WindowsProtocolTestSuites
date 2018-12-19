:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: RDPEUSB cases cannot run in lab aurotmation since device limition.
:: RDPEI cases are exclusive since the touch UI tool issue.

call CommonRunTestCase.cmd "TestCategory!=RDPEUSB&TestCategory!=RDPEI"
echo %RunRDPTestSuite%

%RunRDPTestSuite%
