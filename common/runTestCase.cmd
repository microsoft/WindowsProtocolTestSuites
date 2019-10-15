REM %1 is the path of dll, %2 is the path of ptfconfig, %3 is the name of testcase
set CurrentPath=%~dp0
call "%CurrentPath%setVsTestPath.cmd"
if ErrorLevel 1 (
	exit /b 1
)

echo %vstest%
%vstest% %1 /Settings:%2 /Tests:%3 /Logger:trx;LogFileName=%4.trx