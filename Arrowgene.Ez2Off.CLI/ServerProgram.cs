/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2018 Sebastian Heinz
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
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server;
using Arrowgene.Ez2Off.Server.Log;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Logging;

namespace Arrowgene.Ez2Off.CLI
{
    public class ServerProgram
    {
        public const string LocalSettingsContainer = "server_settings.json";

        public static int EntryPoint(string[] args)
        {
            Console.OutputEncoding = Utils.KoreanEncoding;
            LogProvider.GlobalLogWrite += LogProviderOnLogWrite;
            Console.Title = "EzServer";
            ServerProgram p = new ServerProgram();
            return p.Run(args);
        }

        private static void LogProviderOnLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            switch (logWriteEventArgs.Log.LogLevel)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            if (logWriteEventArgs.Log.Tag is EzLogPacketType)
            {
                switch (logWriteEventArgs.Log.Tag)
                {
                    case EzLogPacketType.In:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case EzLogPacketType.Out:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case EzLogPacketType.Unhandeled:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
            }

            Console.WriteLine(logWriteEventArgs.Log);
            Console.ResetColor();
        }

        private VersionType _versionType;
        private Logger _logger;

        public ServerProgram()
        {
            _logger = LogProvider<Logger>.GetLogger(this);
        }

        private int Run(string[] args)
        {
            if (args.Length >= 1)
            {
                if (args[0] == "solista")
                {
                    _versionType = VersionType.Solista;
                }
                else if (args[0] == "reboot13")
                {
                    _versionType = VersionType.Reboot13;
                }
                else
                {
                    Help();
                    return Program.ExitCodeWrongParameters;
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
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe server solista");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("You can adjust the configuration in the file '" + ConfigFileName + "'.");
        }

        public object ConfigFileName { get; set; }

        private void Start()
        {
            _logger.Debug("Environment:");
            _logger.Debug("CurrentDirectory: {0}", Environment.CurrentDirectory);
            _logger.Debug("ApplicationDirectory: {0}", Utils.ApplicationDirectory());
            _logger.Debug("RelativeApplicationDirectory: {0}", Utils.RelativeApplicationDirectory());
            _logger.Debug("OS: {0}", Environment.OSVersion);
            _logger.Debug(".NET: {0}", Environment.Version);
            _logger.Debug("x64: {0}", Environment.Is64BitProcess);
            _logger.Debug("Processors: {0}", Environment.ProcessorCount);
            _logger.Debug("---");

            SettingsProvider settingsProvider = new SettingsProvider();
            SettingsContainer settingsContainer = settingsProvider.Load<SettingsContainer>(LocalSettingsContainer);
            if (settingsContainer == null)
            {
                settingsContainer = settingsProvider.CreateLocalSettings();
                settingsProvider.Save(settingsContainer, LocalSettingsContainer);
                _logger.Info("No settings found ({0}), creating new one.", LocalSettingsContainer);
            }
            else
            {
                _logger.Info("Loaded settings configuration ({0}).", LocalSettingsContainer);
            }

            List<EzServer> servers = new List<EzServer>();
            if (_versionType == VersionType.Reboot13)
            {
                foreach (WorldServerSettings serverSettings in settingsContainer.WorldSettingsList)
                {
                    servers.Add(new Server.Reboot13.WorldServer(settingsContainer, serverSettings));
                }
                servers.Add(new Server.Reboot13.LoginServer(settingsContainer));
            }
            else if (_versionType == VersionType.Solista)
            {
                foreach (WorldServerSettings serverSettings in settingsContainer.WorldSettingsList)
                {
                    servers.Add(new Server.Solista.WorldServer(settingsContainer, serverSettings));
                }
                servers.Add(new Server.Solista.LoginServer(settingsContainer));
            }
            else
            {
                Console.WriteLine("Invalid parameter.");
                return;
            }

            foreach (EzServer server in servers)
            {
                server.Start();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            foreach (EzServer server in servers)
            {
                server.Stop();
            }
        }
    }
}