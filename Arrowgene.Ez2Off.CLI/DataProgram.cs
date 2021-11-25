using System;
using System.Collections.Generic;
using Arrowgene.Ez2Off.Data.Hdr;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.CLI
{
    public class DataProgram
    {
        private static List<Log> ErrorLogs = new List<Log>();

        public static int EntryPoint(string[] args)
        {
            LogProvider.GlobalLogWrite += LogProviderOnLogWrite;
            Console.Title = "Ez2Off Data";
            if (args.Length >= 3)
            {
                HdrFormat hdr = new HdrFormat();
                hdr.ProgressChanged += HdrOnProgressChanged;
                if (args[0] == "pack")
                {
                    bool encrypt = (args.Length >= 4 && args[3] == "-e");
                    hdr.Pack(args[1], args[2], encrypt);
                }
                else if (args[0] == "extract")
                {
                    bool decrypt = (args.Length >= 4 && args[3] == "-d");
                    hdr.Extract(args[1], args[2], decrypt);
                }
                else if (args[0] == "extract-folder")
                {
                    bool decrypt = (args.Length >= 4 && args[3] == "-d");
                    hdr.ExtractFolder(args[1], args[2], decrypt, true);
                }
                else if (args[0] == "pack-folder")
                {
                    bool encrypt = (args.Length >= 4 && args[3] == "-e");
                    hdr.PackFolder(args[1], args[2], encrypt, true);
                }
                else if (args[0] == "offset")
                {
                    if (args.Length == 3)
                    {
                        hdr.ExtractAtOffset(args[1], args[2], int.Parse(args[3]));
                    }
                    else
                    {
                        hdr.ExtractOffset(args[1], args[2], int.Parse(args[3]), int.Parse(args[4]));
                    }
                }
                else if (args[0] == "decrypt")
                {
                    hdr.DecryptFile(args[1], args[2]);
                }
                else if (args[0] == "encrypt")
                {
                    hdr.EncryptFile(args[1], args[2]);
                }
                else if (args[0] == "decrypt-archive")
                {
                    hdr.DecryptArchive(args[1], args[2]);
                }
                else
                {
                    Help();
                    return Program.ExitCodeWrongParameters;
                }
            }
            else
            {
                Help();
                return Program.ExitCodeWrongParameters;
            }

            if (ErrorLogs.Count > 0)
            {
                Console.WriteLine("--- ErrorLog ---");
                foreach (Log log in ErrorLogs)
                {
                    PrintLog(log);
                }

                ErrorLogs.Clear();
            }

            Console.WriteLine("Program Ended");
            return Program.ExitCodeOk;
        }

        private static void HdrOnProgressChanged(object sender, HdrProgressEventArgs hdrProgressEventArgs)
        {
            Console.WriteLine(String.Format("Progress: {0} - {2}/{3} - {1}", hdrProgressEventArgs.Action,
                hdrProgressEventArgs.Message, hdrProgressEventArgs.Current, hdrProgressEventArgs.Total));
        }

        public static void Help()
        {
            Console.WriteLine("Ez2Off Data");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Usage:");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data extract <source-file> <destination-folder> [-d]");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data extract-folder <source-folder> <destination-folder> [-d]");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data pack <source-folder> <destination-file> [-e]");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data pack-folder <source-file> <destination-file> [-d]");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data decrypt <source-file> <destination-file>");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data encrypt <source-file> <destination-file>");
            Console.WriteLine("Arrowgene.Ez2Off.CLI.exe data decrypt-archive <source-file> <destination-file>");
            Console.WriteLine(
                "Arrowgene.Ez2Off.CLI.exe data offset <source-file> <destination-file> <offset> [<length>]");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example Extract:");
            Console.WriteLine("'data extract /Users/name/EzData.tro /Users/name/EzData'");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example Extract with decryption:");
            Console.WriteLine("'data extract /Users/name/EzData.tro /Users/name/EzData -d'");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example Pack:");
            Console.WriteLine("'data pack /Users/name/DATA /Users/name/NewData.tro'");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Example Pack with encryption:");
            Console.WriteLine("'data pack /Users/name/DATA /Users/name/NewData.tro -e'");
        }

        private static void LogProviderOnLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            PrintLog(logWriteEventArgs.Log);
            if (logWriteEventArgs.Log.LogLevel == LogLevel.Error)
            {
                ErrorLogs.Add(logWriteEventArgs.Log);
            }
        }

        private static void PrintLog(Log log)
        {
            switch (log.LogLevel)
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

            Console.WriteLine(log);
            Console.ResetColor();
        }
    }
}