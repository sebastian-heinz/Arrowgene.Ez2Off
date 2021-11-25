/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2020 Sebastian Heinz
 *
 * Github: https://github.com/Arrowgene/Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ez2Off is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ez2Off. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Packet.Builder;
using Arrowgene.Ez2Off.Server.Reboot13;
using Arrowgene.Ez2Off.Server.Reboot14;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.CLI
{
    public class ServerProgram
    {
        public const string LocalSettingsContainer = "server_settings.json";

        private static readonly ILogger _logger = LogProvider.Logger(typeof(ServerProgram));
        
        public static int EntryPoint(string[] args)
        {
            Console.OutputEncoding = Utils.KoreanEncoding;
            Console.Title = "EzServer";
            ServerProgram p = new ServerProgram();
            return p.Run(args);
        }

        private VersionType _versionType;
        private bool _server;
        private EzSettings _settings;
        private readonly object _consoleLock;

        public ServerProgram()
        {
            LogProvider.GlobalLogWrite += LogProviderOnLogWrite;
            _server = false;
            _consoleLock = new object();
        }

        private int Run(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0] == "reboot13")
                {
                    _versionType = VersionType.Reboot13;
                }
                else if (args[0] == "reboot14")
                {
                    _versionType = VersionType.Reboot14;
                }
                else
                {
                    Help();
                    return Program.ExitCodeWrongParameters;
                }

                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i] == "--server")
                    {
                        _server = true;
                    }
                }

                Start();
            }
            else
            {
                Help();
                return Program.ExitCodeWrongParameters;
            }

            Console.WriteLine("Program Ended");
            return Program.ExitCodeOk;
        }

        private void Help()
        {
            Console.WriteLine("Ez2Off EzServer");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Usage:");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe server reboot13");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe server reboot14");
        }

        private void Start()
        {
            SettingsProvider settingsProvider = new SettingsProvider();
            _settings = settingsProvider.Load<EzSettings>(LocalSettingsContainer);
            if (_settings == null)
            {
                _settings = new EzSettings();
                settingsProvider.Save(_settings, LocalSettingsContainer);
            }

            _logger.Info("Starting Server");
            _logger.Info($"Loaded Settings: {settingsProvider.GetSettingsPath(LocalSettingsContainer)}");

            EzServer server;
            switch (_versionType)
            {
                case VersionType.Reboot13:
                    R13Database dbR13 = new R13Database();
                    dbR13.Prepare(_settings.DatabaseSettings);
                    server = new EzServer(_settings, new R13Provider());
                    break;
                case VersionType.Reboot14:
                    R14Database dbR14 = new R14Database();
                    dbR14.Prepare(_settings.DatabaseSettings);
                    server = new EzServer(_settings, new R14Provider());
                    break;
                default:
                    _logger.Error("Invalid Parameter");
                    return;
            }

            server.Start();

            if (_server)
            {
                while (_server)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(5));
                }
            }
            else
            {
                Console.WriteLine("Press 'e' to exit");
                bool readKey = true;
                while (readKey)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.R:
                            Console.WriteLine("Restart...");
                            server.Stop();
                            server.Start();
                            break;
                        case ConsoleKey.E:
                            Console.WriteLine("Exiting...");
                            readKey = false;
                            Environment.Exit(0);
                            break;
                    }
                }
            }
        }
        
        private void LogProviderOnLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            if (_settings.LogLevel > (int) logWriteEventArgs.Log.LogLevel)
            {
                return;
            }


            ConsoleColor consoleColor = ConsoleColor.Gray;
            switch (logWriteEventArgs.Log.LogLevel)
            {
                case LogLevel.Debug:
                    consoleColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    consoleColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Error:
                    consoleColor = ConsoleColor.Red;
                    break;
            }

            if (logWriteEventArgs.Log.Tag is EzLogPacketType)
            {
                switch (logWriteEventArgs.Log.Tag)
                {
                    case EzLogPacketType.In:
                        consoleColor = ConsoleColor.Green;
                        break;
                    case EzLogPacketType.Out:
                        consoleColor = ConsoleColor.Magenta;
                        break;
                    case EzLogPacketType.Unhandled:
                        consoleColor = ConsoleColor.Red;
                        break;
                }
            }

            lock (_consoleLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(logWriteEventArgs.Log);
                Console.ResetColor();
            }
        }
    }
}