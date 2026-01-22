# Prerequisites

You need to install docker on your platform, go to the [Get Docker][] for help.

[Get Docker]: https://docs.docker.com/get-docker/


# Featured Tags
- `rdpserver`: The rdpserver test suite image.
  - Ubuntu 20.04 for Linux
  - `docker pull mcr.microsoft.com/windowsprotocoltestsuites:rdpserver`
- `rdpclient`: The rdpclient test suite image.
  - Ubuntu 20.04 for Linux
  - `docker pull mcr.microsoft.com/windowsprotocoltestsuites:rdpclient`

# About the Image

Windows Protocol Test Suites provide interoperability testing against the implementation of Windows open specifications including Remote Desktop Services.
If you are new to Windows Protocol Test Suites and want to learn more, go to the [windows protocol test suites][] to get started.

[windows protocol test suites]: https://github.com/microsoft/WindowsProtocolTestSuites

# How to Use the Image

To run the `rdpserver` or `rdpclient` image, you need to mount a folder with [rdpserver ptfconfig][] or [rdpclient ptfconfig][] files included and pre-configured. You may need to update some fields in the *.deployment.ptfconfig files like "RDP.ServerUserName", "RDP.ServerUserPassword", etc.

[rdpserver ptfconfig]: https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/4.21.1.0/rdpserver-docker-ptfconfig.tar.gz
[rdpclient ptfconfig]: https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/4.21.1.0/rdpclient-docker-ptfconfig.tar.gz

- `--hostname`: **Required**. The host name of the running container, for example: RDPClient. 

   **Note:** If you are running rdpclient image, you have to use the ip address here as the hostname.
- `--network`: **Required**. The network the running container will use, using host as default. While using host, please make sure that the connection between the host which the container is runnning and the server is valid.
- `-v`: **Required**. The /path/of/ptfconfig should include all the ptfconfig files with pre-configured, and mount this path to the fixed path /data/rdpserver in the container. 

   **Note:** If you are running rdpclient image, you need to create a self-signed certificate named "RDPServer.pfx" and put it together with the ptfconfig files, and update the related ptfconfig value. You can also use the default certificate and settings contained in the [rdpclient-docker-ptfconfig.tar.gz](https://github.com/microsoft/WindowsProtocolTestSuites/releases/download/4.21.1.0/rdpclient-docker-ptfconfig.tar.gz)
- `-i`: **Required**. The image name, for example: windowsprotocoltestsuites:rdpserver or windowsprotocoltestsuites:rdpclient
- `Filter`: Optional environment variable. The expression used to filter test cases. For example, "'TestCategory=BVT&TestCategory=RDPBCGR'" will filter out test cases with test category BVT and RDPBCGR.
- `DryRun`: Optional environment variable. Default is "y", which will just list all the test cases match the filter string instead of running them. If it's null or empty, the filtered test cases will be executed directly.

To run the windows protocol test suites image for `rdpserver`:

```
docker run \
  --hostname <hostname> \
  --network host \
  -v /path/of/ptfconfig:/data/rdpserver \
  -e Filter="'TestCategory=BVT&TestCategory=RDPBCGR'" \
  -e DryRun="" \
  -i mcr.microsoft.com/windowsprotocoltestsuites:rdpserver
```

To run the windows protocol test suites image for `rdpclient`:

```
docker run \
  --hostname <hostIPAddress> \
  --network host \
  -v /path/of/ptfconfigAndcert:/data/rdpclient \
  -e Filter="'TestCategory=BVT&TestCategory=RDPBCGR'" \
  -e DryRun="" \
  -i mcr.microsoft.com/windowsprotocoltestsuites:rdpclient
```

When the test suites run finished, the result file with trx format will be generated under the location /path/of/ptfconfig

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


# Feedback

If you have any issues or concerns, reach out to us through a [GitHub issue](https://github.com/microsoft/WindowsProtocolTestSuites/issues/new).

# License

- Legal Notice: [Container License Information](https://aka.ms/mcr/osslegalnotice)
- [.NET license](https://github.com/dotnet/dotnet-docker/blob/master/LICENSE)
- Please check the [github repository](https://github.com/microsoft/WindowsProtocolTestSuites) for project license details

