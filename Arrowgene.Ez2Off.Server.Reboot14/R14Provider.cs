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
using Arrowgene.Ez2Off.Server.Chat.Command.Commands;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Chat;
using Arrowgene.Ez2Off.Server.Reboot14.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Login;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.World;
using Arrowgene.Ez2Off.Server.Reboot14.Trait;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Ez2Off.Server.Trait;

namespace Arrowgene.Ez2Off.Server.Reboot14
{
    public class R14Provider : IProvider
    {
        public R14Provider()
        {
            PacketBuilder.ChatPacket = new ChatPacket();
            PacketBuilder.GamePacket = new GamePacket();
            PacketBuilder.RoomPacket = new RoomPacket();
            PacketBuilder.ShopPacket = new ShopPacket();
            PacketBuilder.LobbyPacket = new LobbyPacket();
            PacketBuilder.MessagePacket = new MessagePacket();
            PacketBuilder.SettingsPacket = new SettingsPacket();
            PacketBuilder.CharacterPacket = new CharacterPacket();
            PacketBuilder.InventoryPacket = new InventoryPacket();
        }

        public IPacketFactoryProvider ProvidePacketFactoryProvider(EzSettings settings)
        {
            return new R14PacketFactory(settings);
        }

        public ChatTrait ProvideChatTrait(EzServer server)
        {
            return new R14ChatTrait(server);
        }

        public List<BaseChatCommand> ProvideChatCommands(EzServer server)
        {
            List<BaseChatCommand> commands = new List<BaseChatCommand>();
            commands.Add(new ClientCmd(server));
            commands.Add(new PlayerCmd(server));
            return commands;
        }

        public RoomTrait ProvideRoomTrait(EzServer server)
        {
            return new R14RoomTrait(server);
        }

        public ChannelTrait ProvideChannelTrait(EzServer server)
        {
            return new R14ChannelTrait(server);
        }

        public ServerTrait ProvideServerTrait(EzServer server)
        {
            return new R14ServerTrait(server);
        }

        public IEnumerable<IHandler> ProvideLoginHandler(EzServer server)
        {
            List<IHandler> loginHandler = new List<IHandler>();
            loginHandler.Add(new LoginRequest(server));
            loginHandler.Add(new SelectMode(server));
            loginHandler.Add(new SelectServer(server));
            loginHandler.Add(new SelectChannel(server));
            loginHandler.Add(new CreateCharacter(server));
            return loginHandler;
        }

        public IEnumerable<IHandler> ProvideWorldHandler(EzServer server)
        {
            List<IHandler> worldHandler = new List<IHandler>();
            worldHandler.Add(new LobbyEnter(server));
            worldHandler.Add(new BackButton(server));
            worldHandler.Add(new ChangeChannel(server));
            worldHandler.Add(new ChangeChannelSelect(server));
            worldHandler.Add(new ChangeMode(server));
            worldHandler.Add(new CharacterInformation(server));
            worldHandler.Add(new ChatLobby(server));
            worldHandler.Add(new ChatPrivate(server));
            worldHandler.Add(new GameBattleMeter(server));
            worldHandler.Add(new GameCheck(server));
            worldHandler.Add(new GameLoading(server));
            worldHandler.Add(new GameResult(server));
            worldHandler.Add(new GameSongBestScore(server));
            worldHandler.Add(new GameStart(server));
            worldHandler.Add(new InventoryAcceptGift(server));
            worldHandler.Add(new InventoryApplyItem(server));
            worldHandler.Add(new InventoryDeleteItem(server));
            worldHandler.Add(new InventoryGifts(server));
            worldHandler.Add(new InventoryMoveItem(server));
            worldHandler.Add(new InventoryShow(server));
            worldHandler.Add(new InventoryUseItem(server));
            worldHandler.Add(new MessengerAddFriend(server));
            worldHandler.Add(new MessengerBox(server));
            worldHandler.Add(new MessengerDeleteFriend(server));
            worldHandler.Add(new MessengerFriendChat(server));
            worldHandler.Add(new MessengerOption(server));
            worldHandler.Add(new Mission(server));
            worldHandler.Add(new MusicBuyLicense(server));
            worldHandler.Add(new RoomChangeOptions(server));
            worldHandler.Add(new RoomCreate(server));
            worldHandler.Add(new RoomEntry(server));
            worldHandler.Add(new RoomInvite(server));
            worldHandler.Add(new RoomKick(server));
            worldHandler.Add(new RoomSelectSong(server));
            worldHandler.Add(new RoomViewLobbyMembers(server));
            worldHandler.Add(new SaveSettings(server));
            worldHandler.Add(new ShopPurchaseItem(server));
            worldHandler.Add(new ShopSendGift(server));
            worldHandler.Add(new ShowCaseMovieBack(server));
            worldHandler.Add(new ShowCaseMovieStart(server));
            return worldHandler;
        }
    }
}