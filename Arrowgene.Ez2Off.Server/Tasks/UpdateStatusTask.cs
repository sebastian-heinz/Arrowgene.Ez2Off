using System;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Services.Tasks;

namespace Arrowgene.Ez2Off.Server.Tasks
{
    public class UpdateStatusTask : PeriodicTask
    {
        private readonly IDatabase _database;

        public UpdateStatusTask(IDatabase database)
        {
            _database = database;
        }

        public override string Name => "UpdateStatus";
        public override TimeSpan TimeSpan => TimeSpan.FromMinutes(12);

        protected override bool RunAtStart => false;

        protected override void Execute()
        {
            if (!_database.SetSetting("ez2on_status", $"{Utils.GetUnixTime(DateTime.Now)}"))
            {
                Logger.Error("Could not update status");
            }
        }
    }
}