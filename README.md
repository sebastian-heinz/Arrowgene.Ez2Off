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
    - [Windows - Development - VSCode](#windows---development---vscode)
- [Clients](#clients)
  - [R13 (Reboot13)](#r13-reboot13)
    - [R13 - OSX - Game](#r13---osx---game)
    - [R13 - Windows - Game](#r13---windows---game)
  - [R14 (Reboot14)](#r14-reboot14)
- [Attribution](#attribution)
  - [Contributers](#contributers)
  - [3rd Parties and Libraries](#3rd-parties-and-libraries)

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
The project is split into different sub projects.

### Arrowgene.Ez2Off.CLI
[Command Line Interface] tool to execute all tasks that are related to the server and its development. 
It mainly helps to start and run the code from the core libraries.

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

3) Change to the Project:
   ```
   cd Arrowgene.Ez2Off
   ```

4) Restore the project dependencies:
   ```
   dotnet restore
   ```

5) Run the Server:
   ```
   dotnet run --project Arrowgene.Ez2Off.CLI/Arrowgene.Ez2Off.CLI.csproj server reboot13
   ```

### OSX - Development - VSCode

1) Download VSCode [https://code.visualstudio.com/](https://code.visualstudio.com/)
2) Install the C# Extension [https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
3) Open the project folder (Arrowgene.Ez2Off/)


## Running under Windows

### Windows - Server
1) Ensure you have .NET Core 2.0 or higher 
   [https://www.microsoft.com/net/download/windows](https://www.microsoft.com/net/download/windows)

2) Clone the project:
   ```
   git clone https://github.com/Arrowgene/Arrowgene.Ez2Off.git
   ```

3) Change to the Project:
   ```
   cd Arrowgene.Ez2Off
   ```

4) Restore the project dependencies:
   ```
   dotnet restore
   ```

5) Run the Server:
   ```
   dotnet run --project Arrowgene.Ez2Off.CLI/Arrowgene.Ez2Off.CLI.csproj server reboot13
   ```

### Windows - Development - VSCode

1) Download VSCode [https://code.visualstudio.com/](https://code.visualstudio.com/)
2) Install the C# Extension [https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
3) Open the project folder (Arrowgene.Ez2Off/)


Clients
===

## R13 (Reboot13)
The client that is compatible when using `server reboot13`-argument is ez2on reboot from 2013.
We assigned the arbitrary name "Reboot13" or "R13" to it.


### R13 - OSX - Game
To Run the game on OSX you will need wine

#### Install wine
A more detailed instruction can be found here: https://www.davidbaumgold.com/tutorials/wine-mac/

1) Install homebrew https://brew.sh/
2) Install XQuartz Using Homebrew
   ```
   brew cask install xquartz
   ```
3) Install Wine Using Homebrew
   ```
   brew install wine
   ```
   
#### Start Game
1) Copy the game directory to your wine 'C' drive.
   The 'C'-drive can usually be found at your home directory
   ```
   cd ~/.wine/drive_c
   ```

2) Start the game with the following command: 
   ```
   wine EZ2ON_Online.exe "session^|account^|hash^|9999"
   ```

3) If the server is running the game should connect to the Server.

### R13 - Windows - Game

1) Navigate to the game directory

2) Start the game with the following command: 
   ```
   EZ2ON_Online.exe session\|account\|hash\|9999
   ```

## R14 (Reboot14)
Work In Progress

Attribution
===
## Contributers
- Sebastian Heinz [@sebastian-heinz](https://github.com/sebastian-heinz)

## 3rd Parties and Libraries
- System.Data.SQLite (https://system.data.sqlite.org/)
- bcrypt.net (https://github.com/BcryptNet/bcrypt.net)
- xUnit.net (https://github.com/xunit/xunit)
- .NET Standard (https://github.com/dotnet/standard)
- Arrowgene.Services (https://github.com/Arrowgene/Arrowgene.Services)

Thank you all, 
without your work this would never be possible!