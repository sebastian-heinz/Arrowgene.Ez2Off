using System;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Tasks.Core
{
    public abstract class PeriodicTask
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(PeriodicTask));

        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;

        public abstract string TaskName { get; }
        public abstract TimeSpan TaskTimeSpan { get; }

        protected abstract void Execute();
        protected abstract bool TaskRunAtStart { get; }

        public void Start()
        {
            if (_task != null)
            {
                Logger.Error($"Task {TaskName} already started");
                return;
            }


            _cancellationTokenSource = new CancellationTokenSource();
            _task = new Task(Run, _cancellationTokenSource.Token);
            _task.Start();
        }

        public void Stop()
        {
            if (_task == null)
            {
                Logger.Error($"Task {TaskName} already stopped");
                return;
            }

            _cancellationTokenSource.Cancel();
            _task = null;
        }

        private async void Run()
        {
            Logger.Debug($"Task {TaskName} started");
            if (TaskRunAtStart)
            {
                Logger.Trace($"Task {TaskName} run");
                ExecuteUserCode();
                Logger.Trace($"Task {TaskName} completed");
            }

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TaskTimeSpan, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger.Debug($"Task {TaskName} canceled");
                }

                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Logger.Trace($"Task {TaskName} run");
                    ExecuteUserCode();
                    Logger.Trace($"Task {TaskName} completed");
                }
            }

            Logger.Debug($"Task {TaskName} ended");
        }

        private void ExecuteUserCode()
        {
            try
            {
                Execute();
            }
            catch (Exception ex)
            {
                Logger.Error($"Task {TaskName} crashed");
                Logger.Exception(ex);
            }
        }
    }
}
