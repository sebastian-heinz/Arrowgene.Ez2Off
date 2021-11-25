using System;

namespace Arrowgene.Ez2Off.Data.Hdr
{
    public class HdrProgressEventArgs : EventArgs
    {
        public HdrProgressEventArgs(string action, string message, int total, int current)
        {
            Action = action;
            Total = total;
            Current = current;
            Message = message;
        }

        public string Action { get; }
        public int Total { get; }
        public int Current { get; }
        public string Message { get; }
    }
}