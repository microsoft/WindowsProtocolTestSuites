## Overview

This is the Java version of RDPSUTControlAgent. You need Java version 7.0 or later version to build and run the solution.

## Build

Run `./gradlew build` to build the project. The generated .jar file is located at `./build/libs/RDPSUTControlAgent.jar`

## How to use

`java -jar RDPSUTControlAgent.jar -c <configFile>`

In the config file, you need to specify the commands which will be used to start and stop RDP client.

Here is an example file for [FreeRDP](freerdp.config).
