:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

if not defined WIX (
	echo Error: WiX Toolset version 3.14 or higher should be installed
	exit /b 1
)

exit /b 0
