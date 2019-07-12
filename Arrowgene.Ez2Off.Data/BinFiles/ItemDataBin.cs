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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFiles
{
    public class ItemDataBin : BinFile<Item>
    {
        public override string Header => "M_ITEM";

        public override Item ReadEntry(IBuffer buffer)
        {
            Item item = new Item();
            item.Id = buffer.ReadInt32();
            item.q = buffer.ReadInt32();
            item.Type = (ItemType) buffer.ReadInt32();
            item.s = buffer.ReadInt32();
            item.t = buffer.ReadInt32();
            item.u = buffer.ReadInt32();
            item.Image = ReadString(buffer);
            item.a = buffer.ReadInt32();
            item.Name = ReadString(buffer);
            item.b = buffer.ReadInt32();
            item.Duration = buffer.ReadInt32();
            item.Coins = buffer.ReadInt32();
            item.Level = buffer.ReadInt32();
            item.ExpPlus = buffer.ReadInt32();
            item.CoinPlus = buffer.ReadInt32();
            item.HpPlus = buffer.ReadInt32();
            item.ResiliencePlus = buffer.ReadInt32();
            item.DefensePlus = buffer.ReadInt32();
            item.k = buffer.ReadInt32();
            item.l = buffer.ReadInt32();
            item.m = buffer.ReadInt32();
            item.n = buffer.ReadInt32();
            item.o = buffer.ReadInt32();
            item.Effect = ReadString(buffer);
            return item;
        }

        public override void WriteEntry(Item item, IBuffer buffer)
        {
            buffer.WriteInt32(item.Id);
            buffer.WriteInt32(item.q);
            buffer.WriteInt32((int) item.Type);
            buffer.WriteInt32(item.s);
            buffer.WriteInt32(item.t);
            buffer.WriteInt32(item.u);
            WriteString(item.Image, buffer);
            buffer.WriteInt32(item.a);
            WriteString(item.Name, buffer);
            buffer.WriteInt32(item.b);
            buffer.WriteInt32(item.Duration);
            buffer.WriteInt32(item.Coins);
            buffer.WriteInt32(item.Level);
            buffer.WriteInt32(item.ExpPlus);
            buffer.WriteInt32(item.CoinPlus);
            buffer.WriteInt32(item.HpPlus);
            buffer.WriteInt32(item.ResiliencePlus);
            buffer.WriteInt32(item.DefensePlus);
            buffer.WriteInt32(item.k);
            buffer.WriteInt32(item.l);
            buffer.WriteInt32(item.m);
            buffer.WriteInt32(item.n);
            buffer.WriteInt32(item.o);
            WriteString(item.Effect, buffer);
        }
    }
}