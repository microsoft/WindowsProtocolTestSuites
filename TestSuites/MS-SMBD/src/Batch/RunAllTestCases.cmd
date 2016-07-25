@echo off
"%VS110COMNTOOLS%..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "..\Bin\MS-SMBD_ServerTestSuite.dll" /Settings:..\Bin\ServerLocal.TestSettings /Logger:trx
pause