Arrowgene.Ez2Off
===
Server Emulator for the Online Game Ez2On.

## Table of contents
- [Disclaimer](#disclaimer)
- [Project](#project)
  - [Arrowgene.Ez2Off.CLI](#arrowgeneez2offcli)
  - [Arrowgene.Ez2Off.Common](#arrowgeneez2offcommon)
  - [Arrowgene.Ez2Off.Data](#arrowgeneez2offdata)
  - [Arrowgene.Ez2Off.Server](#arrowgeneez2offserver)
  - [Arrowgene.Ez2Off.Test](#arrowgeneez2offtest)
  - [Running under OSX](#running-under-osx)
    - [OSX - Server](#osx---server)
    - [OSX - Development - VSCode](#osx---development---vscode)
  - [Running under Windows](#running-under-windows)
    - [Windows - Server](#windows---server)
- [Clients](#clients)
  - [R13 (Reboot13)](#r13-(reboot13))
    - [R13 - OSX - Game](#r13---osx---game)
    - [R13 - Windows - Game](#r13---windows---game)
    - [R13 - Startup Parameter](#r13---startup-parameter)
  - [R14 (Reboot14)](#r14-(reboot14))
- [Architecture](#architecture)
  - [Flow](#flow)
- [Attribution](#attribution)
  - [Contributers](#contributers)
  - [3rd Parties and Libaries](#3rd-parties-and-libaries)

Disclaimer
===
The project is intended for educational purpose only, we strongly discourage operating a public server.
This repository does not distribute any game related assets or copyrighted material, 
pull requests containing such material will not be accepted.
All files have been created from scratch, and are subject to the copyright notice of this project.
If a part of this repository violates any copyright or license, please state which files and why,
including proof and it will be removed.

Project
===
The project is splitted into different sub projects.

### Arrowgene.Ez2Off.CLI
[Command Line Interface] tool to execute all tasks that are related to the server and its development. 
It mainly helps to kickstart and run the code from the core libaries.

### Arrowgene.Ez2Off.Common
[Library] that includes helper methods that are used across the project.

### Arrowgene.Ez2Off.Data
[Library] containing the core logic related to ez2on game data.

### Arrowgene.Ez2Off.Server
[Library] containing the core logic related to ez2on server.
- starting R13 server
- starting Solista server

### Arrowgene.Ez2Off.Test
[Unit Test] Used to define tests that run to ensure the integrity of methods.

## Running under OSX

### OSX - Server
1) Ensure you have .NET Core 2.0 or higher
- [https://www.microsoft.com/net/download/macos](https://www.microsoft.com/net/download/macos)

2) Clone the project:
```
git clone https://github.com/Arrowgene/Arrowgene.Ez2Off.git
```

3) Change to the 'Command Line Interface'-Project:
```
cd Arrowgene.Ez2Off/Arrowgene.Ez2Off.CLI
```

4) Restore the project dependencies:
```
dotnet restore
```

5) Run the Server:
```
dotnet run
```

### OSX - Development - VSCode

1) Download VSCode [https://code.visualstudio.com/](https://code.visualstudio.com/)
2) Install the C# Extension [https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
3) Make changes :)


## Running under Windows

### Windows - Server
1) Ensure you have .NET Core 2.0 or higher 
[https://www.microsoft.com/net/download/windows](https://www.microsoft.com/net/download/windows)

2) Clone the project:
```
git clone https://github.com/Arrowgene/Arrowgene.Ez2Off.git
```

3) Open to the Project in Visual Studio and run the 'Command Line Interface'-Project.
I'm not sure about the exact steps as I havent tested this on a Windows box yet.
I Will update this tutorial once I know the exact steps


Clients
===

## R13 (Reboot13)
The client that is compatible with this server is ez2on reboot from 2013.
We assigned the arbitary name "Reboot13" or "R13" to it.


### R13 - OSX - Game
To Run the game on OSX you will need wine

1) Follow the tutorial until you completed Part 4:  
https://www.davidbaumgold.com/tutorials/wine-mac/

2) Copy the game directory to your wine 'C' drive.
The 'C'-drive can usually be found at your home directory
```
cd ~/.wine/drive_c
```

TODO

If the server is running the game should connect to the Server.

### R13 - Windows - Game

TODO


### R13 - Startup Parameter

The separator used is a '|' symbol, in some systems you might need to escape it.
```
ez.exe "session|account|hash|9999"
```

osx 
```
wine ez.exe "session^|account^|hash^|9999"
```

win 
```
ez.exe session\|account\|hash\|9999
```

## R14 (Reboot14)
IF YOU ARE A REVERSE ENGINEER, PLEASE HELP TO FIND A WAY TO START THE EXECUTABLE.  

We would like to develop the server against the latest official client (ez2on reboot from 2014 (Reboot14/R14).
The executable was packet with VMProtect 2.07 and we were able to unpack the executable,
so it can be loaded into a debugger.  

BUT we have not found out how to start the game or which parameters it requires, 
we always get a message stating "Please start the game from the web".
If you can find out how to correctly start this version of ez2on reboot, please let us know.
For more details feel free to contact us or open an issue.

Thanks alot!

Architecture
===

### Flow
1) Request a Session from the API by providing credentials (Account + Password).
2) Provide the Session along with Account and Password to the client (Session Account Password).

Attribution
===
## Contributers
- Sebastian Heinz

## 3rd Parties and Libaries
- System.Data.SQLite (https://system.data.sqlite.org/)
- bcrypt.net (https://github.com/BcryptNet/bcrypt.net)
- JavaScript-MD5 (https://github.com/blueimp/JavaScript-MD5) 
- xUnit.net (https://github.com/xunit/xunit)
- .NET Standard (https://github.com/dotnet/standard)
- Arrowgene.Services (https://github.com/Arrowgene/Arrowgene.Services)

Thank you all, 
without your work this would never be possible!