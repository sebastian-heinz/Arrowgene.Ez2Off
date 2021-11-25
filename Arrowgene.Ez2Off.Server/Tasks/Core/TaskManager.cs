using System.Collections.Generic;

namespace Arrowgene.Ez2Off.Server.Tasks.Core
{
    public class TaskManager
    {
        private readonly List<PeriodicTask> _tasks;

        public TaskManager()
        {
            _tasks = new List<PeriodicTask>();
        }

        public void AddTask(PeriodicTask task)
        {
            _tasks.Add(task);
        }

        public void RemoveTask(PeriodicTask task)
        {
            _tasks.Remove(task);
        }

        public void Start()
        {
            foreach (PeriodicTask task in _tasks)
            {
                task.Start();
            }
        }

        public void Stop()
        {
            foreach (PeriodicTask task in _tasks)
            {
                task.Stop();
            }
        }

        public void Clear()
        {
            _tasks.Clear();
        }
    }
}
