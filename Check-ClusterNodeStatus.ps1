#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
##
#############################################################################

param(
    [ValidateSet("CreateCheckerTask", "StartChecker")]
    [string]$action="CreateCheckerTask"
)

#----------------------------------------------------------------------------
# Global variables
#----------------------------------------------------------------------------
$scriptPath = Split-Path $MyInvocation.MyCommand.Definition -parent
$scriptName = $MyInvocation.MyCommand.Path
$env:Path += ";$scriptPath;$scriptPath\Scripts"

#----------------------------------------------------------------------------
# Start loging using start-transcript cmdlet
#----------------------------------------------------------------------------
[string]$logFile = $MyInvocation.MyCommand.Path + ".log"
Start-Transcript -Path "$logFile" -Append -Force

#----------------------------------------------------------------------------
# Create checker task
#----------------------------------------------------------------------------
if($action -eq "CreateCheckerTask")
{
    Write-Info.ps1 "Create checker task."
    $TaskName = "ExecuteChecker"
    $Task = "PowerShell $scriptName StartChecker"
    # Create a task which will auto run current script every 5 minutes with StartChecker action.
    CMD.exe /C "schtasks /Create /RU SYSTEM /SC MINUTE /MO 5 /TN `"$TaskName`" /TR `"$Task`" /IT /F"  

    Write-Info.ps1 "Start checker after create the checker task."
    $action = "StartChecker" 
}

#----------------------------------------------------------------------------
# Start checker
#----------------------------------------------------------------------------
if($action -eq "StartChecker")
{
    $cluster = Get-Cluster
    if($cluster -ne $null)
    {        
        $clusterNodes = Get-ClusterNode -Cluster $cluster
        $clusterGroups = Get-ClusterGroup -Cluster $cluster
        $clusterQuorum = Get-ClusterQuorum -Cluster $cluster
        $clusterSharedVolume = Get-ClusterSharedVolume -Cluster $cluster
        $clusterResources = Get-ClusterResource -Cluster $cluster
        $clusterNetworks = Get-ClusterNetwork -Cluster $cluster
        $clusterNetworkInterfaces = Get-ClusterNetworkInterface -Cluster $cluster
        $clusterGroup = $clusterGroups | where {$_.Name -eq "Cluster Group"}
        $currentHostServer = $clusterGroup.OwnerNode
        $clusterQuorumDisk = $clusterQuorum.QuorumResource
        $physicalDisks = $clusterResources | where {$_.ResourceType -match "Physical Disk"}

        Write-Info.ps1 "Print out Cluster basic info."
        Write-Info.ps1 ("Name: " + $cluster.Name)
        Write-Info.ps1 ("Current Host Server: " + $currentHostServer.Name + "  Status: " + $currentHostServer.State)
        Write-Info.ps1 ("Quorum Disk (Witness): " + $clusterQuorumDisk.Name + "  Status: " + $clusterQuorumDisk.State)

        Write-Info.ps1 "Print out Cluster Roles info."
        $clusterGroups | where {$_.GroupType -match "FileServer"} | ft Name,State,GroupType,OwnerNode,Priority 

        Write-Info.ps1 "Print out Cluster Nodes info."
        $clusterNodes | ft Name,State

        Write-Info.ps1 "Print out Cluster Disks info." 
        $physicalDisks | ft Name,State,OwnerNode
        $clusterSharedVolume | ft Name,State,OwnerNode

        Write-Info.ps1 "Print out Cluster Networks info."
        $clusterNetworks | ft Name,State,Role,Address

        Write-Info.ps1 "Print out Cluster network adpaters info."    
        $clusterNetworkInterfaces | sort Name | ft Node,Name,State,Network

        Write-Info.ps1 "Print out latest Cluster Events."    
        $clusterEvents = Get-WinEvent System -ErrorAction Ignore | Where {($_.ProviderName -eq "Microsoft-Windows-FailoverClustering")}
        $latestCluterEvent = $clusterEvents | Select-Object -First 1 
        $latestCluterEvent | fl Id,LevelDisplayName,TimeCreated,Message

    }
    else
    {
        Write-Error.ps1 "Cannot find any cluster."
        Write-Info.ps1 "Check all disks."
        $disks = Get-Disk
        if($disks -ne $null)
        {
            Write-Info.ps1 "Print out disk status."
            Write-Info.ps1 ($disks | ft Number,FriendlyName,IsClustered,Size,BusType,OperationalStatus,PartitionStyle | out-string)
        }
        else
        {
            Write-Error.ps1 "There is no available disks."
        }

        Write-Info.ps1 "Check Microsoft iSCSI Initiator Service."
        $service = get-service MSiSCSI
        if($service -ne $null)
        {
            Write-Info.ps1 "Print out MSiSCSI servcie status."
            Write-Info.ps1 ($service | ft Name,Status,DisplayName | out-string)
        }
        else
        {
            Write-Error.ps1 "There is no MSiSCSI servcie."
        }

        Write-Info.ps1 "Check discovered iSCSI target portal."
        $iscsiTargetPortal = Get-iscsiTargetPortal
        if($iscsiTargetPortal -ne $null)
        {
            Write-Info.ps1 "Print out iSCSI target portal."
            Write-Info.ps1 ($iscsiTargetPortal | fl TargetPortalAddress,TargetPortalPortNumber | out-string)
        }
        else
        {
            Write-Error.ps1 "There is no discovered iSCSI target portal."
        }

        Write-Info.ps1 "View avaialbe iSCSI targets."
        $iscsiTarget = get-iscsiTarget
        if($iscsiTarget -ne $null)
        {
            Write-Info.ps1 "Print out avaialbe iSCSI targets."
            Write-Info.ps1 ($iscsiTarget | fl NodeAddress,IsConnected | out-string)
        }
        else
        {
            Write-Error.ps1 "There is no avaialbe iSCSI targets."
        }

        Write-Info.ps1 "View iSCSI Connections."
        $session = Get-IscsiSession    
        if($session -ne $null)
        {           
            Write-Info.ps1 "Print out iSCSI Connections."
            Write-Info.ps1 ($session | fl InitiatorNodeAddress,TargetNodeAddress,IsConnected,IsPersistent | out-string)
        }
        else
        {
            Write-Error.ps1 "There is no iSCSI Connections."
        }

    }

    Write-Info.ps1 "Finish checker."
    Sleep 5 # To display above messages
}

#----------------------------------------------------------------------------
# Ending
#----------------------------------------------------------------------------
Stop-Transcript
exit 0
# SIG # Begin signature block
# MIIkWQYJKoZIhvcNAQcCoIIkSjCCJEYCAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCAsX8TK7EJFeUwy
# Xulm/hPVplj2jk4GuhPg30OGd03kxqCCDYEwggX/MIID56ADAgECAhMzAAABA14l
# HJkfox64AAAAAAEDMA0GCSqGSIb3DQEBCwUAMH4xCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25p
# bmcgUENBIDIwMTEwHhcNMTgwNzEyMjAwODQ4WhcNMTkwNzI2MjAwODQ4WjB0MQsw
# CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9u
# ZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMR4wHAYDVQQDExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
# AQDRlHY25oarNv5p+UZ8i4hQy5Bwf7BVqSQdfjnnBZ8PrHuXss5zCvvUmyRcFrU5
# 3Rt+M2wR/Dsm85iqXVNrqsPsE7jS789Xf8xly69NLjKxVitONAeJ/mkhvT5E+94S
# nYW/fHaGfXKxdpth5opkTEbOttU6jHeTd2chnLZaBl5HhvU80QnKDT3NsumhUHjR
# hIjiATwi/K+WCMxdmcDt66VamJL1yEBOanOv3uN0etNfRpe84mcod5mswQ4xFo8A
# DwH+S15UD8rEZT8K46NG2/YsAzoZvmgFFpzmfzS/p4eNZTkmyWPU78XdvSX+/Sj0
# NIZ5rCrVXzCRO+QUauuxygQjAgMBAAGjggF+MIIBejAfBgNVHSUEGDAWBgorBgEE
# AYI3TAgBBggrBgEFBQcDAzAdBgNVHQ4EFgQUR77Ay+GmP/1l1jjyA123r3f3QP8w
# UAYDVR0RBEkwR6RFMEMxKTAnBgNVBAsTIE1pY3Jvc29mdCBPcGVyYXRpb25zIFB1
# ZXJ0byBSaWNvMRYwFAYDVQQFEw0yMzAwMTIrNDM3OTY1MB8GA1UdIwQYMBaAFEhu
# ZOVQBdOCqhc3NyK1bajKdQKVMFQGA1UdHwRNMEswSaBHoEWGQ2h0dHA6Ly93d3cu
# bWljcm9zb2Z0LmNvbS9wa2lvcHMvY3JsL01pY0NvZFNpZ1BDQTIwMTFfMjAxMS0w
# Ny0wOC5jcmwwYQYIKwYBBQUHAQEEVTBTMFEGCCsGAQUFBzAChkVodHRwOi8vd3d3
# Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2NlcnRzL01pY0NvZFNpZ1BDQTIwMTFfMjAx
# MS0wNy0wOC5jcnQwDAYDVR0TAQH/BAIwADANBgkqhkiG9w0BAQsFAAOCAgEAn/XJ
# Uw0/DSbsokTYDdGfY5YGSz8eXMUzo6TDbK8fwAG662XsnjMQD6esW9S9kGEX5zHn
# wya0rPUn00iThoj+EjWRZCLRay07qCwVlCnSN5bmNf8MzsgGFhaeJLHiOfluDnjY
# DBu2KWAndjQkm925l3XLATutghIWIoCJFYS7mFAgsBcmhkmvzn1FFUM0ls+BXBgs
# 1JPyZ6vic8g9o838Mh5gHOmwGzD7LLsHLpaEk0UoVFzNlv2g24HYtjDKQ7HzSMCy
# RhxdXnYqWJ/U7vL0+khMtWGLsIxB6aq4nZD0/2pCD7k+6Q7slPyNgLt44yOneFuy
# bR/5WcF9ttE5yXnggxxgCto9sNHtNr9FB+kbNm7lPTsFA6fUpyUSj+Z2oxOzRVpD
# MYLa2ISuubAfdfX2HX1RETcn6LU1hHH3V6qu+olxyZjSnlpkdr6Mw30VapHxFPTy
# 2TUxuNty+rR1yIibar+YRcdmstf/zpKQdeTr5obSyBvbJ8BblW9Jb1hdaSreU0v4
# 6Mp79mwV+QMZDxGFqk+av6pX3WDG9XEg9FGomsrp0es0Rz11+iLsVT9qGTlrEOla
# P470I3gwsvKmOMs1jaqYWSRAuDpnpAdfoP7YO0kT+wzh7Qttg1DO8H8+4NkI6Iwh
# SkHC3uuOW+4Dwx1ubuZUNWZncnwa6lL2IsRyP64wggd6MIIFYqADAgECAgphDpDS
# AAAAAAADMA0GCSqGSIb3DQEBCwUAMIGIMQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMTIwMAYDVQQDEylNaWNyb3NvZnQgUm9vdCBDZXJ0aWZpY2F0
# ZSBBdXRob3JpdHkgMjAxMTAeFw0xMTA3MDgyMDU5MDlaFw0yNjA3MDgyMTA5MDla
# MH4xCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdS
# ZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMT
# H01pY3Jvc29mdCBDb2RlIFNpZ25pbmcgUENBIDIwMTEwggIiMA0GCSqGSIb3DQEB
# AQUAA4ICDwAwggIKAoICAQCr8PpyEBwurdhuqoIQTTS68rZYIZ9CGypr6VpQqrgG
# OBoESbp/wwwe3TdrxhLYC/A4wpkGsMg51QEUMULTiQ15ZId+lGAkbK+eSZzpaF7S
# 35tTsgosw6/ZqSuuegmv15ZZymAaBelmdugyUiYSL+erCFDPs0S3XdjELgN1q2jz
# y23zOlyhFvRGuuA4ZKxuZDV4pqBjDy3TQJP4494HDdVceaVJKecNvqATd76UPe/7
# 4ytaEB9NViiienLgEjq3SV7Y7e1DkYPZe7J7hhvZPrGMXeiJT4Qa8qEvWeSQOy2u
# M1jFtz7+MtOzAz2xsq+SOH7SnYAs9U5WkSE1JcM5bmR/U7qcD60ZI4TL9LoDho33
# X/DQUr+MlIe8wCF0JV8YKLbMJyg4JZg5SjbPfLGSrhwjp6lm7GEfauEoSZ1fiOIl
# XdMhSz5SxLVXPyQD8NF6Wy/VI+NwXQ9RRnez+ADhvKwCgl/bwBWzvRvUVUvnOaEP
# 6SNJvBi4RHxF5MHDcnrgcuck379GmcXvwhxX24ON7E1JMKerjt/sW5+v/N2wZuLB
# l4F77dbtS+dJKacTKKanfWeA5opieF+yL4TXV5xcv3coKPHtbcMojyyPQDdPweGF
# RInECUzF1KVDL3SV9274eCBYLBNdYJWaPk8zhNqwiBfenk70lrC8RqBsmNLg1oiM
# CwIDAQABo4IB7TCCAekwEAYJKwYBBAGCNxUBBAMCAQAwHQYDVR0OBBYEFEhuZOVQ
# BdOCqhc3NyK1bajKdQKVMBkGCSsGAQQBgjcUAgQMHgoAUwB1AGIAQwBBMAsGA1Ud
# DwQEAwIBhjAPBgNVHRMBAf8EBTADAQH/MB8GA1UdIwQYMBaAFHItOgIxkEO5FAVO
# 4eqnxzHRI4k0MFoGA1UdHwRTMFEwT6BNoEuGSWh0dHA6Ly9jcmwubWljcm9zb2Z0
# LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY1Jvb0NlckF1dDIwMTFfMjAxMV8wM18y
# Mi5jcmwwXgYIKwYBBQUHAQEEUjBQME4GCCsGAQUFBzAChkJodHRwOi8vd3d3Lm1p
# Y3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY1Jvb0NlckF1dDIwMTFfMjAxMV8wM18y
# Mi5jcnQwgZ8GA1UdIASBlzCBlDCBkQYJKwYBBAGCNy4DMIGDMD8GCCsGAQUFBwIB
# FjNodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2RvY3MvcHJpbWFyeWNw
# cy5odG0wQAYIKwYBBQUHAgIwNB4yIB0ATABlAGcAYQBsAF8AcABvAGwAaQBjAHkA
# XwBzAHQAYQB0AGUAbQBlAG4AdAAuIB0wDQYJKoZIhvcNAQELBQADggIBAGfyhqWY
# 4FR5Gi7T2HRnIpsLlhHhY5KZQpZ90nkMkMFlXy4sPvjDctFtg/6+P+gKyju/R6mj
# 82nbY78iNaWXXWWEkH2LRlBV2AySfNIaSxzzPEKLUtCw/WvjPgcuKZvmPRul1LUd
# d5Q54ulkyUQ9eHoj8xN9ppB0g430yyYCRirCihC7pKkFDJvtaPpoLpWgKj8qa1hJ
# Yx8JaW5amJbkg/TAj/NGK978O9C9Ne9uJa7lryft0N3zDq+ZKJeYTQ49C/IIidYf
# wzIY4vDFLc5bnrRJOQrGCsLGra7lstnbFYhRRVg4MnEnGn+x9Cf43iw6IGmYslmJ
# aG5vp7d0w0AFBqYBKig+gj8TTWYLwLNN9eGPfxxvFX1Fp3blQCplo8NdUmKGwx1j
# NpeG39rz+PIWoZon4c2ll9DuXWNB41sHnIc+BncG0QaxdR8UvmFhtfDcxhsEvt9B
# xw4o7t5lL+yX9qFcltgA1qFGvVnzl6UJS0gQmYAf0AApxbGbpT9Fdx41xtKiop96
# eiL6SJUfq/tHI4D1nvi/a7dLl+LrdXga7Oo3mXkYS//WsyNodeav+vyL6wuA6mk7
# r/ww7QRMjt/fdW1jkT3RnVZOT7+AVyKheBEyIXrvQQqxP/uozKRdwaGIm1dxVk5I
# RcBCyZt2WwqASGv9eZ/BvW1taslScxMNelDNMYIWLjCCFioCAQEwgZUwfjELMAkG
# A1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
# HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEoMCYGA1UEAxMfTWljcm9z
# b2Z0IENvZGUgU2lnbmluZyBQQ0EgMjAxMQITMwAAAQNeJRyZH6MeuAAAAAABAzAN
# BglghkgBZQMEAgEFAKCBrjAZBgkqhkiG9w0BCQMxDAYKKwYBBAGCNwIBBDAcBgor
# BgEEAYI3AgELMQ4wDAYKKwYBBAGCNwIBFTAvBgkqhkiG9w0BCQQxIgQgOKFlgwW5
# a4SdJhwTRLSdnH6N5I/E4xZI+TblZUJZAzgwQgYKKwYBBAGCNwIBDDE0MDKgFIAS
# AE0AaQBjAHIAbwBzAG8AZgB0oRqAGGh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbTAN
# BgkqhkiG9w0BAQEFAASCAQAnEkdiCbS+rHpLxiyShF3hCKqMsEnf0syCZja0hYDI
# bYZRdMh+TRBwOAbedxXqSGVkjeJPLwdabMin9SQ2su6GLTbvlUrTeOrkJGTqQwOp
# HPP2ISyfVVO2FwVVEMTCsOBbB/Qd5v90HHkJi/E3NgQ19+I6hIBy9PNZq6/A1bIE
# AMsUpKz5lur1WKcTRtHm0UdwgcVNK7kAdqsx4op+D0bqsO2ByQNx/x10MyujHtdO
# 1ow46pRFEudnhFgWy2qvBSgc9NVV6pKcfTOreq3QeFI4C8IiwOy5dxqz+zBu82W/
# VgeircFcrZtVYTyVvhG9T6nRoECRQ6XPhqptOX6x7pWKoYITuDCCE7QGCisGAQQB
# gjcDAwExghOkMIIToAYJKoZIhvcNAQcCoIITkTCCE40CAQMxDzANBglghkgBZQME
# AgEFADCCAVgGCyqGSIb3DQEJEAEEoIIBRwSCAUMwggE/AgEBBgorBgEEAYRZCgMB
# MDEwDQYJYIZIAWUDBAIBBQAEIPvJ9GfvGUeePCEWRy9sZnChjJDuLXlbKsYtfE4P
# x1UlAgZbrBKOjXYYEzIwMTgxMDA5MDYwMzMzLjU4MlowBwIBAYACAfSggdSkgdEw
# gc4xCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdS
# ZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKTAnBgNVBAsT
# IE1pY3Jvc29mdCBPcGVyYXRpb25zIFB1ZXJ0byBSaWNvMSYwJAYDVQQLEx1UaGFs
# ZXMgVFNTIEVTTjpCQkVDLTMwQ0EtMkRCRTElMCMGA1UEAxMcTWljcm9zb2Z0IFRp
# bWUtU3RhbXAgU2VydmljZaCCDyAwggT1MIID3aADAgECAhMzAAAAziDjflBqaKQu
# AAAAAADOMA0GCSqGSIb3DQEBCwUAMHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpX
# YXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQg
# Q29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAy
# MDEwMB4XDTE4MDgyMzIwMjYyNloXDTE5MTEyMzIwMjYyNlowgc4xCzAJBgNVBAYT
# AlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYD
# VQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKTAnBgNVBAsTIE1pY3Jvc29mdCBP
# cGVyYXRpb25zIFB1ZXJ0byBSaWNvMSYwJAYDVQQLEx1UaGFsZXMgVFNTIEVTTjpC
# QkVDLTMwQ0EtMkRCRTElMCMGA1UEAxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAgU2Vy
# dmljZTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALZuqc5mW8FROtzh
# CWfSsJhR053TD7Dm6d1KK1Uh7borGwYHXbvBH2N2P3xV30coZv9i3hSveAhiw5Nu
# k51M9kJzd6EvOqc+AOYYQgbPJfDM7kz6v3UnIXMZp6BGtJ4clInlokr36sFBR+7i
# 3Z2TstfgpmPUUJ2tQZYCtrVeskZFZV+CLpVC8KJAyrozwrg2cOWDCdLMi/EGfmCM
# SRN6Abgu6cSPP93mHOzUMsjyJAXw8BtBIxndgxXxqfZwW0Y1Xw6YuHe8Lvg9/G4W
# PzteFHCcvDm19B5NNOL2QKDHULqRfprhMyYbUBZQxjyhGT1PJ+Ypki4NRGkf2mAq
# H+XlGVUCAwEAAaOCARswggEXMB0GA1UdDgQWBBRIGDHQuoCoi6fiIrVPtje/5swF
# fzAfBgNVHSMEGDAWgBTVYzpcijGQ80N7fEYbxTNoWoVtVTBWBgNVHR8ETzBNMEug
# SaBHhkVodHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtpL2NybC9wcm9kdWN0cy9N
# aWNUaW1TdGFQQ0FfMjAxMC0wNy0wMS5jcmwwWgYIKwYBBQUHAQEETjBMMEoGCCsG
# AQUFBzAChj5odHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY1Rp
# bVN0YVBDQV8yMDEwLTA3LTAxLmNydDAMBgNVHRMBAf8EAjAAMBMGA1UdJQQMMAoG
# CCsGAQUFBwMIMA0GCSqGSIb3DQEBCwUAA4IBAQBrfqH1SHZs5dnFkunNJZaYsXho
# QTrK/sEKg+3wSF/odLwBO7qQUtl7bVrjkC/tYzB3iRMyiznlTb4mI5iS0tkh/hXW
# 8UYVCoTNlfDJVQzewSSLt67Wt13uMgsUF3zFkxnNkhHdQQLhU44KWYIz70Josofo
# U3Lj3pAhx2DEWeVhWSircxr0ujXn5u71q/Vr3vXxjbG+FmHwTUMfHNj7KYu7qPZ6
# 6WQCLdiAQjvx7idF1l2Dc7D48fp8oS1MACNpN8RqaxM0DB4Q1yoXe7xulRtA3QlM
# jOT8PzRVI+nr/u84m6QcIXKCRbReFTbmBckPFXHwb0D8nAq3heGRJNhztxJJMIIG
# cTCCBFmgAwIBAgIKYQmBKgAAAAAAAjANBgkqhkiG9w0BAQsFADCBiDELMAkGA1UE
# BhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAc
# BgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMpTWljcm9zb2Z0
# IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5IDIwMTAwHhcNMTAwNzAxMjEzNjU1
# WhcNMjUwNzAxMjE0NjU1WjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGlu
# Z3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
# cmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDCC
# ASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKkdDbx3EYo6IOz8E5f1+n9p
# lGt0VBDVpQoAgoX77XxoSyxfxcPlYcJ2tz5mK1vwFVMnBDEfQRsalR3OCROOfGEw
# WbEwRA/xYIiEVEMM1024OAizQt2TrNZzMFcmgqNFDdDq9UeBzb8kYDJYYEbyWEeG
# MoQedGFnkV+BVLHPk0ySwcSmXdFhE24oxhr5hoC732H8RsEnHSRnEnIaIYqvS2SJ
# UGKxXf13Hz3wV3WsvYpCTUBR0Q+cBj5nf/VmwAOWRH7v0Ev9buWayrGo8noqCjHw
# 2k4GkbaICDXoeByw6ZnNPOcvRLqn9NxkvaQBwSAJk3jN/LzAyURdXhacAQVPIk0C
# AwEAAaOCAeYwggHiMBAGCSsGAQQBgjcVAQQDAgEAMB0GA1UdDgQWBBTVYzpcijGQ
# 80N7fEYbxTNoWoVtVTAZBgkrBgEEAYI3FAIEDB4KAFMAdQBiAEMAQTALBgNVHQ8E
# BAMCAYYwDwYDVR0TAQH/BAUwAwEB/zAfBgNVHSMEGDAWgBTV9lbLj+iiXGJo0T2U
# kFvXzpoYxDBWBgNVHR8ETzBNMEugSaBHhkVodHRwOi8vY3JsLm1pY3Jvc29mdC5j
# b20vcGtpL2NybC9wcm9kdWN0cy9NaWNSb29DZXJBdXRfMjAxMC0wNi0yMy5jcmww
# WgYIKwYBBQUHAQEETjBMMEoGCCsGAQUFBzAChj5odHRwOi8vd3d3Lm1pY3Jvc29m
# dC5jb20vcGtpL2NlcnRzL01pY1Jvb0NlckF1dF8yMDEwLTA2LTIzLmNydDCBoAYD
# VR0gAQH/BIGVMIGSMIGPBgkrBgEEAYI3LgMwgYEwPQYIKwYBBQUHAgEWMWh0dHA6
# Ly93d3cubWljcm9zb2Z0LmNvbS9QS0kvZG9jcy9DUFMvZGVmYXVsdC5odG0wQAYI
# KwYBBQUHAgIwNB4yIB0ATABlAGcAYQBsAF8AUABvAGwAaQBjAHkAXwBTAHQAYQB0
# AGUAbQBlAG4AdAAuIB0wDQYJKoZIhvcNAQELBQADggIBAAfmiFEN4sbgmD+BcQM9
# naOhIW+z66bM9TG+zwXiqf76V20ZMLPCxWbJat/15/B4vceoniXj+bzta1RXCCtR
# gkQS+7lTjMz0YBKKdsxAQEGb3FwX/1z5Xhc1mCRWS3TvQhDIr79/xn/yN31aPxzy
# mXlKkVIArzgPF/UveYFl2am1a+THzvbKegBvSzBEJCI8z+0DpZaPWSm8tv0E4XCf
# Mkon/VWvL/625Y4zu2JfmttXQOnxzplmkIz/amJ/3cVKC5Em4jnsGUpxY517IW3D
# nKOiPPp/fZZqkHimbdLhnPkd/DjYlPTGpQqWhqS9nhquBEKDuLWAmyI4ILUl5WTs
# 9/S/fmNZJQ96LjlXdqJxqgaKD4kWumGnEcua2A5HmoDF0M2n0O99g/DhO3EJ3110
# mCIIYdqwUB5vvfHhAN/nMQekkzr3ZUd46PioSKv33nJ+YWtvd6mBy6cJrDm77MbL
# 2IK0cs0d9LiFAR6A+xuJKlQ5slvayA1VmXqHczsI5pgt6o3gMy4SKfXAL1QnIffI
# rE7aKLixqduWsqdCosnPGUFN4Ib5KpqjEWYw07t0MkvfY3v1mYovG8chr1m1rtxE
# PJdQcdeh0sVV42neV8HR3jDA/czmTfsNv11P6Z0eGTgvvM9YBS7vDaBQNdrvCScc
# 1bN+NR4Iuto229Nfj950iEkSoYIDrjCCApYCAQEwgf6hgdSkgdEwgc4xCzAJBgNV
# BAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4w
# HAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKTAnBgNVBAsTIE1pY3Jvc29m
# dCBPcGVyYXRpb25zIFB1ZXJ0byBSaWNvMSYwJAYDVQQLEx1UaGFsZXMgVFNTIEVT
# TjpCQkVDLTMwQ0EtMkRCRTElMCMGA1UEAxMcTWljcm9zb2Z0IFRpbWUtU3RhbXAg
# U2VydmljZaIlCgEBMAkGBSsOAwIaBQADFQCJbpW8N5eBAbWZtcqyCvkAi19tkaCB
# 3jCB26SB2DCB1TELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAO
# BgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEp
# MCcGA1UECxMgTWljcm9zb2Z0IE9wZXJhdGlvbnMgUHVlcnRvIFJpY28xJzAlBgNV
# BAsTHm5DaXBoZXIgTlRTIEVTTjo1N0Y2LUMxRTAtNTU0QzErMCkGA1UEAxMiTWlj
# cm9zb2Z0IFRpbWUgU291cmNlIE1hc3RlciBDbG9jazANBgkqhkiG9w0BAQUFAAIF
# AN9mdD4wIhgPMjAxODEwMDkwODI2MzhaGA8yMDE4MTAxMDA4MjYzOFowdTA7Bgor
# BgEEAYRZCgQBMS0wKzAKAgUA32Z0PgIBADAIAgEAAgMBGQAwBwIBAAICGyAwCgIF
# AN9nxb4CAQAwNgYKKwYBBAGEWQoEAjEoMCYwDAYKKwYBBAGEWQoDAaAKMAgCAQAC
# AxbjYKEKMAgCAQACAwehIDANBgkqhkiG9w0BAQUFAAOCAQEAeMUPfbH6ZZy1e6F6
# HsmwSVV+YsLtkkTAepBSywZ8h9fa4rUeo4dB/lSHVlyCKJ3m+5UyMUSIk2+u/Ltf
# UE2Rps40SC0OUYQcpRfdCQ1nu+9eGZNGyrOLUUaKLEMML+RpbwlIMAp6/LaqFkX1
# xMBiheApTMxrSjtRhXsQ+4ccoK3uX5aC3bht33PmZXbq43dyeA2w487meY2r+sXu
# K6j7jwqX9LLGj/krpXb388XK5C18Ut4RQPUrjQ9otb7yKiLl4GL93EErd8FuC4iP
# GJ8lm8Jo2iF3yCitnYccGXhaHuUGpAApYAgFbOBalTVTOnXQykEoa1vf3pSZTxMl
# Zb0+qjGCAvUwggLxAgEBMIGTMHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNo
# aW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
# cG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEw
# AhMzAAAAziDjflBqaKQuAAAAAADOMA0GCWCGSAFlAwQCAQUAoIIBMjAaBgkqhkiG
# 9w0BCQMxDQYLKoZIhvcNAQkQAQQwLwYJKoZIhvcNAQkEMSIEINEXpxIE+NpolDOc
# CnWvvHUDp7m4WA/FKcgxDATxUoAhMIHiBgsqhkiG9w0BCRACDDGB0jCBzzCBzDCB
# sQQUiW6VvDeXgQG1mbXKsgr5AItfbZEwgZgwgYCkfjB8MQswCQYDVQQGEwJVUzET
# MBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMV
# TWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1T
# dGFtcCBQQ0EgMjAxMAITMwAAAM4g435QamikLgAAAAAAzjAWBBQFLPoUNcZ6cUq4
# tlE279jV6mHjMzANBgkqhkiG9w0BAQsFAASCAQAqzq74oOnCkqZqGXjTfpTtWKcK
# oBgWPgQ8oin4VF4WBedtbVoFDGgccEaWCiRptMoD42/EqlNN/f8xc61W5L8xfqWI
# a1hNGR/AkUetEURgxZtT3hG6TbzR67jBHRpJ1tS/e7qZH4D333OoiSvf+J5iaOPd
# OXgbo+ETQB83jRbWS207lNHKnB4WExgO5DeKxBH9JAkB4SD33DCb9lCuQFbg9xRG
# s+6+NOTtT/isCnE0ppwdN65N6mrFsSU/BS1NYVJnC+roNCM9CxddqKKt9OR4PtZx
# cYBQwbNzAK06AG4exqSfF57MgLCErbEeNzkY1bgrHBGBzHMJsorYFkRPE5Hb
# SIG # End signature block
