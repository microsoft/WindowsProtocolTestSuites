:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

if not exist "%programfiles(x86)%\Spec Explorer 2010\SpecExplorer.exe" (
    if not exist "%programfiles%\Spec Explorer 2010\SpecExplorer.exe" (
		echo Error: Spec Explorer 2010 v3.5.3146.0 should be installed
		exit /b 1
	)
)

exit /b 0
