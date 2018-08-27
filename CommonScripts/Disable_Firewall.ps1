#Disable Firewall
netsh advfirewall set allprofiles state off

#With the SkipNetworkProfileCheck parameter, the public network connection will not throw error for PS remoting cmdlet.
Enable-PSRemoting -SkipNetworkProfileCheck -Force