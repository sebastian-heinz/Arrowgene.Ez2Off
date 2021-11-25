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
using System.Xml;
using Arrowgene.Ez2Off.Common;

namespace Arrowgene.Ez2Off.CLI
{
    public static class Program
    {
        public const int ExitCodeOk = 0;
        public const int ExitCodeWrongParameters = 1;

        static int Main(string[] args)
        {
            //  List<string> ids = new List<string>();
         //  foreach (string file in Directory.GetFiles(@"C:\Users\railgun\Downloads\a", "*",
         //      SearchOption.TopDirectoryOnly))
         //  {
         //      string text = Utils.ReadFileText(file);
         //      if (text.Contains("cheat"))
         //      {
         //          int startIdx = file.IndexOf("audit-") + 6;
         //          int endIdx = file.IndexOf("-", startIdx);
         //          int len = endIdx - startIdx;
         //          string id = file.Substring(startIdx, len);
         //          if (!ids.Contains(id))
         //          {
         //              ids.Add(id);
         //          }
         //      }
         //  }
         // string result = string.Join(",", ids);
            
            PrintVersion();
            if (args.Length >= 1)
            {
                string command = args[0];
                int programArgsLength = args.Length - 1;
                string[] programArgs = new string[programArgsLength];
                Array.Copy(args, 1, programArgs, 0, programArgsLength);
                switch (command)
                {
                    case "server":
                        return ServerProgram.EntryPoint(programArgs);
                    case "data":
                        return DataProgram.EntryPoint(programArgs);
                    case "game":
                        return GameProgram.EntryPoint(programArgs);
                    default:
                    {
                        Help();
                        return ExitCodeWrongParameters;
                    }
                }
            }

            Help();
            return ExitCodeWrongParameters;
        }

        private static void Help()
        {
            Console.WriteLine("Ez2Off CLI - Command line interface");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Usage:");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe server");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe game");
        }

        private static void PrintVersion()
        {
            Console.WriteLine("Command Line Ver.: {0}", GetVersion());
        }

        public static string GetVersion()
        {
            Version version = Utils.GetAssemblyVersion("Arrowgene.Ez2Off.CLI");
            if (version != null && version.Major > 0)
            {
                return version.ToString();
            }

            return Utils.DefaultVersion;
        }

    }
}