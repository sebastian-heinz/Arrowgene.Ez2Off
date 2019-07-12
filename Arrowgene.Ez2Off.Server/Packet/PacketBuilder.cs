/*
 * This file is part of Arrowgene.Ez2Off
 *
 * Arrowgene.Ez2Off is a server implementation for the game "Ez2On".
 * Copyright (C) 2017-2018 Sebastian Heinz
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

using Arrowgene.Ez2Off.Server.Log;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;

namespace Arrowgene.Ez2Off.Server.Packet
{
    public class PacketBuilder
    {
        private bool _isFinished;
        private int _currentDesiredSize;
        private byte _currentId;
        private IBuffer _currentPacketBuffer;
        private Logger _logger;

        public PacketBuilder()
        {
            _isFinished = true;
            _logger = LogProvider<EzLogger>.GetLogger(this);
        }

        //TODO handle nagle, protect against OOM by defining a max packet size.
        public EzPacket Read(byte[] data)
        {
            IBuffer buffer = EzServer.Buffer.Provide(data);
            EzPacket packet = null;
            if (_isFinished)
            {
                buffer.SetPositionStart();
                byte id = buffer.ReadByte();
                int size = buffer.ReadInt16(Endianness.Big);

                int desiredSize = size + EzPacket.HeaderSize;

                if (buffer.Size == desiredSize)
                {
                    IBuffer packetBuffer = buffer.Clone(EzPacket.HeaderSize, size);
                    packet = new EzPacket(id, packetBuffer);
                }
                else if (buffer.Size < desiredSize)
                {
                    _currentId = id;
                    _currentDesiredSize = desiredSize;
                    _isFinished = false;
                    _currentPacketBuffer = buffer;
                }
                else
                {
                    _logger.Error("TODO");
                }
            }
            else
            {
                long dataSize = _currentPacketBuffer.Size + buffer.Size;

                if (dataSize == _currentDesiredSize)
                {
                    _isFinished = true;
                    IBuffer packetBuffer = _currentPacketBuffer.Clone(EzPacket.HeaderSize, _currentDesiredSize);
                    packet = new EzPacket(_currentId, packetBuffer);
                }
                else if (dataSize < _currentDesiredSize)
                {
                    _currentPacketBuffer.SetPositionEnd();
                    _currentPacketBuffer.WriteBuffer(buffer);
                }
                else
                {
                    _logger.Error("TODO1");
                }
            }

            return packet;
        }
    }
}