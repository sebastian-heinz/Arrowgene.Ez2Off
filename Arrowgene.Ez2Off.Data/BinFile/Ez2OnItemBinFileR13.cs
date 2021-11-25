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

using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Data.BinFile
{
    public class Ez2OnItemBinFileR13 : Ez2OnBinFile<Ez2OnModelItem>
    {
        public override string Header => "M_ITEM";

        public override Ez2OnModelItem ReadEntry(IBuffer buffer)
        {
            Ez2OnModelItem item = new Ez2OnModelItem();
            item.Id = buffer.ReadInt32();
            item.Enabled = buffer.ReadInt32();
            item.Type = (ItemType) buffer.ReadInt32();
            item.S = buffer.ReadInt32();
            item.T = buffer.ReadInt32();
            item.U = buffer.ReadInt32();
            item.Image = ReadString(buffer);
            item.A = buffer.ReadInt32();
            item.Name = ReadString(buffer);
            item.Currency = (ItemCurrencyType) buffer.ReadInt32();
            item.Duration = buffer.ReadInt32();
            item.Price = buffer.ReadInt32();
            item.Level = buffer.ReadInt32();
            item.ExpPlus = buffer.ReadInt32();
            item.CoinPlus = buffer.ReadInt32();
            item.HpPlus = buffer.ReadInt32();
            item.ResiliencePlus = buffer.ReadInt32();
            item.DefensePlus = buffer.ReadInt32();
            item.K = buffer.ReadInt32();
            item.L = buffer.ReadInt32();
            item.M = buffer.ReadInt32();
            item.N = buffer.ReadInt32();
            item.O = buffer.ReadInt32();
            item.Effect = ReadString(buffer);
            return item;
        }

        public override void WriteEntry(Ez2OnModelItem item, IBuffer buffer)
        {
            buffer.WriteInt32(item.Id);
            buffer.WriteInt32(item.Enabled);
            buffer.WriteInt32((int) item.Type);
            buffer.WriteInt32(item.S);
            buffer.WriteInt32(item.T);
            buffer.WriteInt32(item.U);
            WriteString(item.Image, buffer);
            buffer.WriteInt32(item.A);
            WriteString(item.Name, buffer);
            buffer.WriteInt32((int) item.Currency);
            buffer.WriteInt32(item.Duration);
            buffer.WriteInt32(item.Price);
            buffer.WriteInt32(item.Level);
            buffer.WriteInt32(item.ExpPlus);
            buffer.WriteInt32(item.CoinPlus);
            buffer.WriteInt32(item.HpPlus);
            buffer.WriteInt32(item.ResiliencePlus);
            buffer.WriteInt32(item.DefensePlus);
            buffer.WriteInt32(item.K);
            buffer.WriteInt32(item.L);
            buffer.WriteInt32(item.M);
            buffer.WriteInt32(item.N);
            buffer.WriteInt32(item.O);
            WriteString(item.Effect, buffer);
        }
    }
}