using System;
using System.Diagnostics;
using System.ServiceProcess;
using Arrowgene.Ez2Off.Server;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Reboot13;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.CLI
{
    public class WinService : ServiceBase
    {
        private static readonly ILogger _logger = LogProvider.Logger(typeof(WinService));
        
        public static void Initialize()
        {
            using (var service = new WinService())
            {
                Run(service);
            }
        }

        public const string LocalSettingsContainer = "server_settings.json";
        private readonly object _eventLock;
        private readonly EventLog _eventlog;
        private EzSettings _settings;
        private EzServer _server;

        public WinService()
        {
            _eventLock = new object();
            ServiceName = "Ez2On Windows Service";
            string eventSource = "Ez2OnEvent";
            if (!EventLog.SourceExists(eventSource))
            {
                EventLog.CreateEventSource(eventSource, "Ez2OnEventLog");
                Environment.Exit(100);
            }

            _eventlog = new EventLog();
            _eventlog.Source = eventSource;
            LogProvider.GlobalLogWrite += LogProviderOnLogWrite;
        }

        protected override void OnStart(string[] args)
        {
            SettingsProvider settingsProvider = new SettingsProvider();
            _settings = settingsProvider.Load<EzSettings>(LocalSettingsContainer);
            if (_settings == null)
            {
                _settings = new EzSettings();
                settingsProvider.Save(_settings, LocalSettingsContainer);
                _logger.Info(
                    $"No Settings found in: {settingsProvider.GetSettingsPath(LocalSettingsContainer)}. Creating new ...");
            }

            _logger.Info("Starting Server");
            _logger.Info($"Loaded Settings: {settingsProvider.GetSettingsPath(LocalSettingsContainer)}");

            R13Database dbR13 = new R13Database();
            dbR13.Prepare(_settings.DatabaseSettings);
            _server = new EzServer(_settings, new R13Provider());
            _server.Start();
        }

        protected override void OnStop()
        {
            _server.Stop();
        }

        private void LogProviderOnLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            int eventId = 1000;
            EventLogEntryType eventLogEntryType = EventLogEntryType.Information;
            switch (logWriteEventArgs.Log.LogLevel)
            {
                case LogLevel.Debug:
                    eventLogEntryType = EventLogEntryType.Information;
                    break;
                case LogLevel.Info:
                    eventLogEntryType = EventLogEntryType.Information;
                    break;
                case LogLevel.Error:
                    eventLogEntryType = EventLogEntryType.Error;
                    break;
            }

            if (logWriteEventArgs.Log.Tag is EzLogPacketType)
            {
                eventId = 1001;
            }

            lock (_eventLock)
            {
                _eventlog.WriteEntry(logWriteEventArgs.Log.ToString(), eventLogEntryType, eventId);
            }
        }
    }
}