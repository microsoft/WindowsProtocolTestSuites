// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

package rdpsutcontrol.agent;

import picocli.CommandLine;
import picocli.CommandLine.Option;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class App implements Runnable
{
    @Option(names = {"-p", "--port"}, defaultValue = "4488", description = "Specify the listening port")
    int port;

    @Option(names = { "-c", "--config-file" }, required = true, description = "Specify the config file")
    File configFile;

    @Option(names = { "-h", "--help" }, usageHelp = true, description = "Display this help message")
    private boolean helpRequested = false;

    public void run()
    {
        Properties config = new Properties();

        try
        {
            InputStream input = new FileInputStream(configFile);
            config.load(input);
        }
        catch (FileNotFoundException e)
        {
            System.err.println("Cannot find file: " + configFile.getPath());
            System.exit(1);
        }
        catch (IOException e)
        {
            System.err.println("Error loading config file.");
            System.exit(1);
        }

        try
        {
            Listener listenThread = new Listener(port, config);
            listenThread.start();
            System.out.println("RDPSUTControl is listening on port " + port);
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }

    public static void main(String[] args)
    {
        CommandLine.run(new App(), args);
    }
}
