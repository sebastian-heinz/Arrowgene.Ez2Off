using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.CLI
{
    public class GameProgram
    {
        private static readonly ILogger _logger = LogProvider.Logger(typeof(GameProgram));

        public static int EntryPoint(string[] args)
        {
            LogProvider.GlobalLogWrite += LogProviderOnLogWrite;
            Console.Title = "EzGame";
            GameProgram p = new GameProgram();
            return p.Run(args);
        }

        private static void LogProviderOnLogWrite(object sender, LogWriteEventArgs e)
        {
            Console.WriteLine(e.Log);
        }

        private string _session;
        private string _account;
        private string _hash;
        private ushort _port;
        private string _ip;
        private string _gamePath;
        private string _winePath;
        private VersionType _version;

        public GameProgram()
        {
            _port = 9350;
            _ip = "209.97.172.232";
        }

        private int Run(string[] args)
        {
            FileInfo wine = null;
            if (args.Length >= 4)
            {
                if (args[0] == "r13")
                {
                    _version = VersionType.Reboot13;
                }
                else if (args[0] == "r14")
                {
                    _version = VersionType.Reboot14;
                }
                else
                {
                    _logger.Info("Invalid arguments - .exe game [r13|r14] [GamePath] [Account] [Hash]");
                    return 1;
                }

                _gamePath = args[1];
                _account = args[2];
                _hash = args[3];
                if (args.Length >= 5)
                {
                    _session = args[4];
                }
                else
                {
                    _session = "Session";
                }

                if (args.Length >= 6)
                {
                    _winePath = args[5];
                    if (!File.Exists(_winePath))
                    {
                        _logger.Info("Invalid wine path - using default");
                        _winePath = "/Applications/Wine Devel.app/Contents/Resources/wine/bin/wine";
                    }

                    wine = new FileInfo(_winePath);
                }
            }
            else
            {
                _logger.Info("Invalid arguments - .exe game [r13|r14] [GamePath] [Account] [Hash]");
                return 1;
            }

            FileInfo executable = new FileInfo(_gamePath);

            switch (_version)
            {
                case VersionType.Reboot13:
                    R13(executable, wine);
                    break;
                case VersionType.Reboot14:
                    R14(executable, wine);
                    break;
            }

            return 0;
        }

        private void R13(FileInfo executable, FileInfo wine = null)
        {
            Ez2OnPatcher patcher = new Ez2OnPatcher(executable);
            try
            {
                patcher.SavePatches(_ip, _port, true, false, wine != null);
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return;
            }

            Process game = StartProcess(executable, $"{_session}|{_account}|{_hash}|9999", wine);
            if (game != null)
            {
                game.Exited += (sender, args) => { _logger.Info("Game Exited"); };
            }

            _logger.Info("Press any key to exit..");
            Console.ReadKey();
            if (game != null)
            {
                game.Close();
            }
        }

        private void R14(FileInfo executable, FileInfo wine = null)
        {
            int mapSize = 1062;
            string mapName = "EZTOSHR";

            MemoryMappedFile mapFile = MemoryMappedFile.CreateNew(mapName, mapSize,
                MemoryMappedFileAccess.ReadWriteExecute,
                MemoryMappedFileOptions.None, HandleInheritability.None);
            MemoryMappedViewAccessor map =
                mapFile.CreateViewAccessor(0, mapSize, MemoryMappedFileAccess.ReadWriteExecute);

            byte[] session = Encoding.ASCII.GetBytes(_session);
            byte[] account = Encoding.ASCII.GetBytes(_account);
            byte[] hash = Encoding.ASCII.GetBytes(_hash);

            byte[] ip = Encoding.ASCII.GetBytes(_ip);
            byte[] port = Encoding.ASCII.GetBytes(_port.ToString());

            map.WriteArray(0, session, 0, session.Length);
            map.WriteArray(100, account, 0, account.Length);
            map.WriteArray(200, hash, 0, hash.Length);
            map.WriteArray(512, ip, 0, ip.Length);
            map.WriteArray(532, port, 0, port.Length);

            Process game = StartProcess(executable, "", wine);
            if (game != null)
            {
                game.Exited += (sender, args) =>
                {
                    _logger.Info("Game Exited");
                    map.Dispose();
                    mapFile.Dispose();
                };
            }

            _logger.Info("Press any key to exit..");
            Console.ReadKey();
            if (game != null)
            {
                game.Close();
            }
        }

        private Process StartProcess(FileInfo executable, string arguments, FileInfo wine = null)
        {
            if (executable == null)
            {
                _logger.Info("Error, executable is null");
                return null;
            }

            if (!File.Exists(executable.FullName))
            {
                _logger.Info($"Error, file don't exists: {executable.FullName}");
                return null;
            }

            if (wine == null)
            {
                _logger.Info(
                    $"Executable: {executable.FullName} Arguments: {arguments} WorkDir: {executable.DirectoryName}");
                ProcessStartInfo game = new ProcessStartInfo();
                game.FileName = executable.FullName;
                game.WorkingDirectory = executable.DirectoryName;
                game.Arguments = arguments;
                game.UseShellExecute = false;
                return Process.Start(game);
            }
            else
            {
                string wineArguments = string.Format("{0} {1}", executable.FullName, arguments);
                _logger.Info(
                    $"Executable: {wine.FullName} Arguments: {wineArguments} WorkDir: {executable.DirectoryName}");
                ProcessStartInfo game = new ProcessStartInfo();
                game.FileName = wine.FullName;
                game.WorkingDirectory = executable.DirectoryName;
                game.Arguments = wineArguments;
                game.UseShellExecute = false;
                return Process.Start(game);
            }
        }
    }
}