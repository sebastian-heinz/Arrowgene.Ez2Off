using System;
using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Sessions;
using Arrowgene.Services.Tasks;

namespace Arrowgene.Ez2Off.Server.Tasks
{
    public class CleanSessionsTask : PeriodicTask
    {
        private const int MaximumAgeMinutes = 15;

        readonly SessionManager _sessionManager;

        public CleanSessionsTask(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public override string Name => "ClearSessions";
        public override TimeSpan TimeSpan => TimeSpan.FromMinutes(30);

        protected override bool RunAtStart => false;

        protected override void Execute()
        {
            List<Session> sessions = _sessionManager.GetSessions();
            DateTime now = DateTime.Now;
            foreach (Session session in sessions)
            {
                TimeSpan diff = now - session.Creation;
                if (diff.TotalMinutes < MaximumAgeMinutes)
                {
                    Logger.Debug($"Deleting Session: {session.Key} due to unclaimed for {diff.TotalMinutes} minutes");
                    _sessionManager.DeleteSession(session.Key);
                }
            }
        }
    }
}