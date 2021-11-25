REM https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore2x
  SET VERSION=1.40
  
  SET RUNTIMES=win-x64
  mkdir .\publish
  (for %%x in (%RUNTIMES%) do ( 
  REM Clean
  if exist .\publish\%%x-%VERSION%\ RMDIR /S /Q .\publish\%%x-%VERSION%\
  REM Server
  dotnet publish Arrowgene.Ez2Off.CLI\Arrowgene.Ez2Off.CLI.csproj /p:Version=%VERSION% --runtime %%x --configuration Release --output ../publish/%%x-%VERSION%/Server
  REM Starter
  xcopy .\Starter .\publish\%%x-%VERSION%\
  ))