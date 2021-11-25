/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2020 Sebastian Heinz
 *
 * Github: https://github.com/Arrowgene/Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ez2Off is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ez2Off. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ez2Off.Server.Model
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EzClient
    {
        private readonly EzLogger _logger;

        private string DebuggerDisplay => Identity;

        public EzClient(ITcpSocket clientSocket, PacketFactory packetFactory)
        {
            _logger = LogProvider.Logger<EzLogger>(this);
            PacketFactory = packetFactory;
            Socket = clientSocket;
            Session = null;
            Audit = new Audit();
            NoteHistory = new NoteHistory();
            PacketCheck = new PacketCheck();
            UpdateIdentity();
        }

        public string Identity { get; private set; }

        public ITcpSocket Socket { get; }
        public PacketCheck PacketCheck { get; }
        public PacketFactory PacketFactory { get; }
        public Room Room { get; set; }
        public Channel Channel { get; set; }
        public Player Player { get; set; }
        public Score Score { get; set; }
        public Score BestScore { get; set; }
        public Rank Rank { get; set; }
        public Game Game { get; set; }
        public short ChannelIndex { get; set; }
        public Audit Audit { get; set; }
        public NoteHistory NoteHistory { get; set; }

        public Session Session { get; set; }
        public Account Account => Session.Account;
        public Character Character => Session.Character;
        public Setting Setting => Session.Setting;
        public MessageBox MessageBox => Session.MessageBox;
        public ModeType Mode => Session.Mode;
        public Inventory Inventory => Session.Inventory;
        public FriendList Friends => Session.Friends;

        public List<EzPacket> Receive(byte[] data)
        {
            List<EzPacket> packets;
            try
            {
                packets = PacketFactory.Read(data, this);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                packets = new List<EzPacket>();
            }

            return packets;
        }

        public void Send(EzPacket packet)
        {
            byte[] data;
            try
            {
                data = PacketFactory.Write(packet, this);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                return;
            }

            _logger.LogOutgoingPacket(this, packet);
            Socket.Send(data);
        }

        public void UpdateIdentity()
        {
            if (Session != null)
            {
                if (Account != null && Character != null)
                {
                    Identity = $"[{Socket.Identity}][#{Account.Id}][{Character.Name}]";
                    return;
                }

                if (Character != null)
                {
                    Identity = $"[{Socket.Identity}][{Character.Name}]";
                    return;
                }

                if (Account != null)
                {
                    Identity = $"[{Socket.Identity}][#{Account.Id}]";
                    return;
                }
            }

            Identity = $"[{Socket.Identity}]";
        }

        /// <summary>
        /// A signed integer that indicates the relative values of x and y:
        /// - If less than 0, x is less than y.
        /// - If 0, x equals y.
        /// - If greater than 0, x is greater than y.
        /// </summary>
        public class ScoreComparer : IComparer<EzClient>
        {
            private SortOrderType _sortOrder;

            public ScoreComparer(SortOrderType sortOrder)
            {
                _sortOrder = sortOrder;
            }

            public int Compare(EzClient x, EzClient y)
            {
                if (_sortOrder == SortOrderType.DESC)
                {
                    EzClient z = x;
                    x = y;
                    y = z;
                }

                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                if (y.Score == null && x.Score == null)
                {
                    return 0;
                }

                if (x.Score == null)
                {
                    return -1;
                }

                if (y.Score == null)
                {
                    return 1;
                }

                if (y.Score.Incident && x.Score.Incident)
                {
                    return 0;
                }

                if (x.Score.Incident)
                {
                    return -1;
                }

                if (y.Score.Incident)
                {
                    return 1;
                }

                int xScore = x.Score.TotalScore;
                int yScore = y.Score.TotalScore;

                if (!y.Score.StageClear && !x.Score.StageClear)
                {
                    // Both game over, sort by score
                    if (xScore < yScore)
                    {
                        return -1;
                    }

                    if (xScore > yScore)
                    {
                        return 1;
                    }

                    return 0;
                }

                if (!x.Score.StageClear)
                {
                    return -1;
                }

                if (!y.Score.StageClear)
                {
                    return 1;
                }

                if (xScore < yScore)
                {
                    return -1;
                }

                if (xScore > yScore)
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}