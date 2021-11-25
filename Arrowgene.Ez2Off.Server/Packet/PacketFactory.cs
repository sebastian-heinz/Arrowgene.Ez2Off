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

using System.Collections.Generic;
using Arrowgene.Ez2Off.Server.Logs;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Packet
{
    public abstract class PacketFactory : IPacketFactoryProvider
    {
        private const int Max1Mb = 1048576;

        private bool _readHeader;
        private int _size;
        private byte _id;
        private int _position;
        private IBuffer _buffer;

        private readonly int _maxPacketSize;

        protected readonly EzLogger _logger;
        protected readonly EzSettings _settings;

        public PacketFactory(EzSettings settings)
        {
            _logger = LogProvider.Logger<EzLogger>(this);
            _settings = settings;
            _maxPacketSize = Max1Mb;
            Reset();
        }

        protected abstract int HeaderSize { get; }

        protected abstract Endianness SizeEndianness { get; }

        public abstract PacketFactory Provide();
        public abstract IBuffer CreateBuffer(EzPacket packet, EzClient client);
        protected abstract EzPacket CreatePacket(byte id, byte[] packetData, EzClient client);

        public byte[] Write(EzPacket packet, EzClient client)
        {
            IBuffer buffer = CreateBuffer(packet, client);
            if (_settings.LogEncryptedPackets)
            {
                packet.Encrypted = buffer.Clone(HeaderSize, buffer.Size - HeaderSize);
            }

            byte[] data = buffer.GetAllBytes();
            return data;
        }

        public List<EzPacket> Read(byte[] data, EzClient client)
        {
            List<EzPacket> packets = new List<EzPacket>();
            if (_buffer == null)
            {
                _buffer = EzServer.Buffer.Provide(data);
            }
            else
            {
                _buffer.SetPositionEnd();
                _buffer.WriteBytes(data);
            }

            _buffer.Position = _position;

            bool read = true;
            while (read)
            {
                read = false;
                if (!_readHeader && _buffer.Size - _buffer.Position >= HeaderSize)
                {
                    _id = _buffer.ReadByte();
                    _size = _buffer.ReadInt16(SizeEndianness);
                    if (_size < 0)
                    {
                        _logger.Error(
                            $"Packet Id: {_id} - Size: {_size} is negative");
                        Reset();
                        return packets;
                    }
                    if (_size > _maxPacketSize)
                    {
                        _logger.Error(client,
                            $"Packet Id: {_id} - Size: {_size} is bigger than maximum allowed Size: {_maxPacketSize}");
                        Reset();
                        return packets;
                    }

                    int advance = HeaderSize - 3;
                    if (advance > 0)
                    {
                        _buffer.Position += advance;
                    }

                    _readHeader = true;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _size)
                {
                    byte[] packetData = _buffer.ReadBytes(_size);
                    EzPacket packet = CreatePacket(_id, packetData, client);
                    if (_settings.LogEncryptedPackets)
                    {
                        IBuffer encrypted = EzServer.Buffer.Provide(packetData);
                        packet.Encrypted = encrypted;
                    }

                    packets.Add(packet);
                    _readHeader = false;
                    read = _buffer.Position != _buffer.Size;
                }
            }

            if (_buffer.Position == _buffer.Size)
            {
                Reset();
            }
            else
            {
                _position = _buffer.Position;
            }

            return packets;
        }

        private void Reset()
        {
            _readHeader = false;
            _size = 0;
            _id = 0;
            _position = 0;
            _buffer = null;
        }
    }
}