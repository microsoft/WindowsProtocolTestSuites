:: NOTE: This file is needed by VSO, remember to delete when checkout to GitHub

pushed %~dp0
call build.cmd

:: Copy lab automation files into drop folder
set KerberosdropRoot=%TestSuiteRoot%\drop\TestSuites\Kerberos
set KerberosRoot=%TestSuiteRoot%\TestSuites\Kerberos

%systemroot%\System32\xcopy %KerberosRoot%\Setup\Data\* %KerberosdropRoot%\Data /s /i /y
%systemroot%\System32\xcopy %KerberosRoot%\Setup\Lab\Scripts\* %KerberosdropRoot%\Scripts /s /i /y
%systemroot%\System32\xcopy %KerberosRoot%\Setup\Lab\Vstormlitefiles\* %KerberosdropRoot%\Vstormlitefiles /s /i /y
%systemroot%\System32\xcopy %KerberosRoot%\Setup\Scripts\* %KerberosdropRoot%\Scripts /s /i /y
%systemroot%\System32\xcopy %KerberosRoot%\Src\Batch\* %KerberosdropRoot%\Batch /s /i /y


