:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off
call RefreshTestConfig.cmd
%RunFileSharingTestSuite% /TestCaseFilter:"TestCategory=BVT"
pause