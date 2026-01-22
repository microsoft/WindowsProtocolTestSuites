# Prerequisites

You need to install docker on your platform, go to the [Get Docker][] for help.

[Get Docker]: https://docs.docker.com/get-docker/


# Featured Tags

- `fileserver`: The fileserver test suite image.
  - Debian 11 for Linux
  - `docker pull mcr.microsoft.com/windowsprotocoltestsuites:fileserver-v8`

# About the Image

Windows Protocol Test Suites provide interoperability testing against the implementation of Windows open specifications including File Server Services.
If you are new to Windows Protocol Test Suites and want to learn more, go to the [windows protocol test suites][] to get started.

[windows protocol test suites]: https://github.com/microsoft/WindowsProtocolTestSuites

# How to Use the Image

To run the `fileserver` image, you need to mount a folder with [fileserver ptfconfig][] files included and pre-configured. You may need to update some fields in the *.deployment.ptfconfig files like "SutComputerName", "SutIPAddress", etc.

[fileserver ptfconfig]: https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/4.20.9.0/fileserver-docker-ptfconfig.tar.gz

- `--hostname`: **Required**. The host name of the running container, for example: FS-CLI
- `--network`: **Required**. The network the running container will use, using host as default. While using host, please make sure that the connection between the host which the container is runnning and the server is valid.
- `-v`: **Required**. The /path/of/ptfconfig should include all the ptfconfig files with pre-configured, and mount this path to the fixed path /data/fileserver in the container
- `-i`: **Required**. The image name, for example: windowsprotocoltestsuites:fileserver
- `$filter`: Optional. The expression used to filter test cases. For example, "TestCategory=BVT&TestCategory=SMB311" will filter out test cases with test category BVT and SMB311.
- `$dryRun`: Optional. If set as "y", just list all the test cases match the filter string instead of running them. If it's null or empty, the filtered test cases will be executed directly

To run the windows protocol test suites image for `fileserver`:

```
docker run \
  --hostname <hostname> \
  --network host \
  -v /path/of/ptfconfig:/data/fileserver \
  -i mcr.microsoft.com/windowsprotocoltestsuites:fileserver \
  [optional]$filter \
  [optional]$dryRun
```

# Description and Example Usage

This value can be specified as "RunTestCases", "RunPTMCli" or "RunPTMService". 

A test run will start if the Usage is set to "RunTestCases".

A test run initiated by PTMCli will start if the Usage is set to "RunPTMCli".

A PTMService instance will be deployed if the Usage is set to "RunPTMService".

The following variables can be specified if the Usage is set to "RunTestCases":

```
sudo docker run --hostname FS-CLI --network host --env Usage="RunTestCases" --env SutComputerName="node01.contoso.com" --env SutIPAddress="192.168.1.11" --env DomainName="contoso.com" --env AdminUserName="Administrator" --env PasswordForAllUsers="XXXX" --env Filter="TestCategory=BVT&TestCategory=SMB311" --env DryRun="" -v /home/test/ptfconfig:/data/fileserver windowsprotocoltestsuites:fileserver
```

- **SutComputerName**: Computer name of system under test (SUT). If SUT does not have a computer name, set the value to SUT's IP address.

- **SutIPAddress**: IP address or Host Name of SUT to establish connections.

- **DomainName**: Domain name where the SUT locates. If SUT is in WORKGROUP, set it to the value of SutComputerName. If SUT does not have a computer name, leave it blank.

- **AdminUserName**: Administrator user account name of the SUT.

- **PasswordForAllUsers**: Password for all the users listed as follows: AdminUserName, NonAdminUserName and GuestUserName. (To simplify the config, the 3 accounts use the same password.)

- **Filter**: Expression used to filter test cases. For example, "TestCategory=BVT&TestCategory=SMB311" will filter out test cases which have test category BVT and SMB311.

- **DryRun**: If set as "y", just list all filtered test cases instead of running tests actually. Else if it's null or empty, the filtered test cases will be executed directly.

The following variables can be specified if the Usage is set to "RunPTMCli":

```
sudo docker run --hostname FS-CLI --network host --env Usage="RunPTMCli" --env Profile="FileServerBasic.ptm" --env Selected="false" --env Filter="TestCategory=BVT" --env Config="\"Common.SutComputerName=node01.contoso.com\" \"Common.SutIPAddress=192.168.1.11\"" --env ReportFormat="Plain" -v $(pwd):/data/fileserver windowsprotocoltestsuites:fileserver
```

- **Profile**: The file name of the PTM profile archive in the current directory on the host.

- **Selected**: When specified as "true", only the selected test cases will be executed. Otherwise, all the test cases in the profile will be executed.

- **Filter**: Specifies the filter expression of test cases to run. This parameter overrides the test cases in profile.

- **Config**: Specifies the configuration items which will override the values in profile. Each configuration should be in format {property_name}={property_value}, and multiple items should be separated by whitespace.

- **ReportFormat**: Specifies the report format. Valid values are: plain, json, xunit.

The following variables can be specified if the Usage is set to "RunPTMService":

```
sudo docker run --hostname FS-CLI --network host --env Usage="RunPTMService" --env HttpPort="80" --env HttpsPort="443" -v /home/test/ptfconfig:/data/fileserver -i windowsprotocoltestsuites:fileserver
```

- **-i**: Optional. The option can be specified if the user wants to interact with the PTMService instance running in the container.

- **HttpPort**: The HTTP port of the PTMService.

- **HttpsPort**: The HTTPS port of the PTMService.

When the test suites run finished, the result file with trx format will be generated under the location /path/of/ptfconfig

# Feedback

If you have any issues or concerns, reach out to us through a [GitHub issue](https://github.com/microsoft/WindowsProtocolTestSuites/issues/new).

# License

- Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
- [.NET license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
- Please check the [github repository](https://github.com/microsoft/WindowsProtocolTestSuites) for project license details


