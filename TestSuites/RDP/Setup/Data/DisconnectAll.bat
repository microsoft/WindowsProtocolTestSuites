del %SystemDrive%\DisconnectAll.signal /F
cmd /c schtasks /end  /TN Negotiate_RDPConnect
cmd /c schtasks /end  /TN DirectCredSSP_RDPConnect
cmd /c schtasks /end  /TN DirectTls_RDPConnect
cmd /c schtasks /end  /TN TriggerNetworkFailure
cmd /c schtasks /end  /TN TriggerInputEvents
cmd /c taskkill /F /IM mstsc.exe
echo finished. > %SystemDrive%\DisconnectAll.signal
