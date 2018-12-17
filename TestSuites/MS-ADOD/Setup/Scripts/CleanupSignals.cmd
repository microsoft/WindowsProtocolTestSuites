:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

@echo off

set testpath=%SystemDrive%\Test

IF EXIST %testpath%\test.started.signal (
    del %testpath%\test.started.signal
)

IF EXIST %testpath%\test.finished.signal (
    del %testpath%\test.finished.signal
)

IF EXIST %testpath%\config.finished.signal (
    del %testpath%\config.finished.signal
)
