# RDP SUT Control Agent

**RDP SUT Control Agent** facilitates interoperability testing via RDP client test suites by providing automated control operations to the RDP client on SUT. You can either choose to use existing implementation, or implement your own one according to [Documents](#documents).

## Prerequisites

In order to utilize **RDP SUT Control Agent**, please make sure you have changed the type of SUT control adapter to `managed` in `RDP_ClientTestSuite.ptfconfig` on driver computer as below:

```
<Adapter xsi:type="managed" name="IRdpSutControlAdapter" adaptertype="Microsoft.Protocols.TestSuites.Rdp.ProtocolBasedRdpSUTControlAdapter"/>
```

## Implementations

Currently, there are 4 different implementations of **RDP SUT Control Agent** available. You can choose one according to your preference.

* [C#](./CSharp)

	It can be used on platforms where [.NET](https://dotnet.microsoft.com/) is available, including Windows, Linux and macOS. You could either choose to download our built packages from [Releases](https://github.com/microsoft/WindowsProtocolTestSuites/releases), or build it by yourself.

	* Prerequisites

		You need [.NET SDK](https://dotnet.microsoft.com/download) to build and run.

	* Build

		You can run `build.ps1` in PowerShell or `build.sh` in shell to build.

	* How to use

		1. In `RDPSUTControlAgent.dll.config`, you need to specify the commands which will be used to start and stop RDP client.
			
			For example, if you use mstsc, you could modify the corresponding setting as below:

			```
			<add key ="REMOTE_CLIENT" value="mstsc"/>
			```

		1. Run below command to launch RDPSUTControlAgent on SUT:

			```
			dotnet RDPSUTControlAgent.dll /port:{port_num}
			```

			Below are the detail of arguments:
	
			* `/port:{port_num}`: optional, it can be used to specify the local port number to listen on. If not specified, `4488` is used as default. 

			If you run on Windows, you could run by starting the `RDPSUTControlAgent.exe` with the same command-line arguments. 

* [Java](./Java)

	It is implemented by Java, so you can use it across different platforms. You could either choose to download our built packages from [Releases](https://github.com/microsoft/WindowsProtocolTestSuites/releases), or build it by yourself.

	* Prerequisites

		You need [Java](https://www.java.com/) version 7.0 or later version to build and run the solution.    

    * Build

		Run `./gradlew build` to build the project. The generated .jar file is located at `./build/libs/RDPSUTControlAgent.jar`
		You can pass additional options with `_JAVA_OPTIONS`. For example http and https proxy:

		```
		export _JAVA_OPTIONS='-Dhttp.proxyHost=localhost -Dhttp.proxyPort=3128 -Dhttps.proxyHost=localhost -Dhttps.proxyPort=3128'
		```

	* How to use

		Use below command to launch RDPSUTControlAgent on SUT:

		`java -jar RDPSUTControlAgent.jar -c <configFile>`

		In the config file, you need to specify the commands which will be used to start and stop RDP client.

		Here is an example file for [FreeRDP](./Java/freerdp.config).

		Take RDP connection via negotiate as an example, 
		
		```
		Negotiate=xfreerdp /t:RDPClient /rfx /u:PUT_THE_USERNAME_HERE /p:PUT_THE_PASSWORD_HERE /v:{{address}}:{{port}}
		```
		You need to change `PUT_THE_USERNAME_HERE` and `PUT_THE_PASSWORD_HERE` to the actual user name and password for testing.


* [Python](./Python)

	You could choose it if you prefer Python. You need to install Python on your SUT before using it.

	* How to use

		1. In `settings.ini`, you need to specify below configurations:

			* The IP address and port number to listen on. 
			* The commands which will be used to start and stop RDP client.

		1. Run below command to launch RDPSUTControlAgent on SUT.

			```
			python ./RDPSUTControlAgent.py
			```
	

* [C](./C)

	It can be compiled and used almost on all platform with a little changes.

	
	* How to use

		You could use below commands to build and execute on SUT:

		```
		cd C
		mkdir build && cd build && cmake ..
		./control_agent
		```

## Documents

**RDP SUT Control Agent** extends the [SUT Remote Control Protocol](./Docs/SUT_Remote_Control_Protocol.md), you can find its details at [SUT Remote Control Protocol RDP Extension](./Docs/SUT_Remote_Control_Protocol_RDP_Extension.md).
