using System;
using System.Collections.Generic;
using Arrowgene.Ez2Off.Server.Model;

namespace Arrowgene.Ez2Off.Server.Packet
{
    public class PacketCheck
    {
        private readonly Dictionary<uint, PacketMonitor> _inFlight;
        private readonly object _lock;
        private uint _currentId;

        public PacketCheck()
        {
            _currentId = 1;
            _lock = new object();
            _inFlight = new Dictionary<uint, PacketMonitor>();
        }

        public uint Register(uint type)
        {
            uint id;
            lock (_lock)
            {
                id = _currentId;
                _currentId++;
            }
            PacketMonitor monitor = new PacketMonitor();
            monitor.Id = id;
            monitor.Registered = DateTime.Now;
            monitor.Type = type;
            _inFlight.Add(id, monitor);
            return id;
        }

        public void Acknowledge(uint id)
        {
            if (id == 0)
            {
                return;
            }

            _inFlight.Remove(id);
        }

        public int Unacknowledged(uint type)
        {
            int count = 0;
            List<PacketMonitor> inFlight = new List<PacketMonitor>(_inFlight.Values);
            foreach (PacketMonitor monitor in inFlight)
            {
                if (monitor.Type == type)
                {
                    count++;
                }
            }

            return count;
        }
    }
}