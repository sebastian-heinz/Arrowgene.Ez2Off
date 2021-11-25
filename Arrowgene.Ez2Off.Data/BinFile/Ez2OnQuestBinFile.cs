using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnQuestBinFile : Ez2OnBinFile<Ez2OnModelQuest>
    {
        public override string Header => "M_QUEST";

        public override Ez2OnModelQuest ReadEntry(IBuffer buffer)
        {
            Ez2OnModelQuest quest = new Ez2OnModelQuest();
            quest.Id = buffer.ReadInt32();
            quest.A = buffer.ReadInt32();
            quest.B = buffer.ReadInt32();
            quest.C = buffer.ReadInt32();
            quest.D = buffer.ReadInt32();
            quest.Title = ReadString(buffer);
            quest.Mission = ReadString(buffer);
            quest.G = buffer.ReadInt32();
            quest.H = buffer.ReadInt32();
            quest.I = buffer.ReadInt32();
            quest.J = buffer.ReadInt32();
            quest.K = buffer.ReadInt32();
            quest.L = buffer.ReadInt32();
            quest.M = buffer.ReadInt32();
            quest.N = buffer.ReadInt32();
            quest.O = buffer.ReadInt32();
            quest.P = buffer.ReadInt32();
            quest.Q = buffer.ReadInt32();
            quest.R = buffer.ReadInt32();
            quest.S = buffer.ReadInt32();
            quest.T = buffer.ReadInt32();
            quest.U = buffer.ReadInt32();
            quest.V = buffer.ReadInt32();
            quest.W = buffer.ReadInt32();
            quest.X = buffer.ReadInt32();
            quest.Y = buffer.ReadInt32();
            quest.Z = buffer.ReadInt32();
            quest.Z1 = buffer.ReadInt32();
            quest.Z2 = buffer.ReadInt32();
            quest.Z3 = buffer.ReadInt32();
            quest.Z4 = buffer.ReadInt32();
            quest.Z5 = buffer.ReadInt32();
            quest.Z6 = buffer.ReadInt32();
            quest.Z7 = buffer.ReadInt32();
            quest.Z8 = buffer.ReadInt32();
            quest.Z9 = buffer.ReadInt32();
            quest.Z10 = buffer.ReadInt32();
            quest.Z11 = buffer.ReadInt32();
            return quest;
        }

        public override void WriteEntry(Ez2OnModelQuest quest, IBuffer buffer)
        {
            buffer.WriteInt32(quest.Id);
            buffer.WriteInt32(quest.Id);
            buffer.WriteInt32(quest.A);
            buffer.WriteInt32(quest.B);
            buffer.WriteInt32(quest.C);
            buffer.WriteInt32(quest.D);
            WriteString(quest.Title, buffer);
            WriteString(quest.Mission, buffer);
            buffer.WriteInt32(quest.G);
            buffer.WriteInt32(quest.H);
            buffer.WriteInt32(quest.I);
            buffer.WriteInt32(quest.J);
            buffer.WriteInt32(quest.K);
            buffer.WriteInt32(quest.L);
            buffer.WriteInt32(quest.M);
            buffer.WriteInt32(quest.N);
            buffer.WriteInt32(quest.O);
            buffer.WriteInt32(quest.P);
            buffer.WriteInt32(quest.Q);
            buffer.WriteInt32(quest.R);
            buffer.WriteInt32(quest.S);
            buffer.WriteInt32(quest.T);
            buffer.WriteInt32(quest.U);
            buffer.WriteInt32(quest.V);
            buffer.WriteInt32(quest.W);
            buffer.WriteInt32(quest.X);
            buffer.WriteInt32(quest.Y);
            buffer.WriteInt32(quest.Z);
            buffer.WriteInt32(quest.Z1);
            buffer.WriteInt32(quest.Z2);
            buffer.WriteInt32(quest.Z3);
            buffer.WriteInt32(quest.Z4);
            buffer.WriteInt32(quest.Z5);
            buffer.WriteInt32(quest.Z6);
            buffer.WriteInt32(quest.Z7);
            buffer.WriteInt32(quest.Z8);
            buffer.WriteInt32(quest.Z9);
            buffer.WriteInt32(quest.Z10);
            buffer.WriteInt32(quest.Z11);
        }
    }
}