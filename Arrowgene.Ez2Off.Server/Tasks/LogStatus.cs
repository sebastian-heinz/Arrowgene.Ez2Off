using System;
using Arrowgene.Logging;
using Arrowgene.Services.Tasks;

namespace Arrowgene.Ez2Off.Server.Tasks
{
    public class LogStatus : PeriodicTask
    {
        private readonly EzServer _server;
        private readonly ILogger _logger;

        public LogStatus(EzServer server)
        {
            _logger = LogProvider.Logger(this);
            _server = server;
        }

        public override string Name => "LogStatus";
        public override TimeSpan TimeSpan => TimeSpan.FromMinutes(10);

        protected override bool RunAtStart => false;

        protected override void Execute()
        {
            _logger.Info($"Clients: {_server.Clients.GetAllClients().Count}");
            _logger.Info($"_server.IsRunning: {_server.IsRunning}");
            _server.LoginConsumer.LogStatus("LoginConsumer");
            _server.GameConsumer.LogStatus("GameConsumer");
        }
    }
}