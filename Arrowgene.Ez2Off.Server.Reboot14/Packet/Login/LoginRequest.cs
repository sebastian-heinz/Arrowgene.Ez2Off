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
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot14.Packet.Builder;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.Login
{
    public class LoginRequest : Handler<EzServer>
    {
        private Dictionary<int, EzClient> _activeLogins;

        public LoginRequest(EzServer server) : base(server)
        {
            _activeLogins = new Dictionary<int, EzClient>();
        }

        public override int Id => 2;

        public override int ExpectedSize => 256;


        public override void Handle(EzClient client, EzPacket packet)
        {
            string version = packet.Data.ReadCString(Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(version))
            {
                Logger.Error(client, $"Invalid version supplied: {version}");
                client.Socket.Close();
                return;
            }

            packet.Data.Position = 12;
            string parameterString = packet.Data.ReadCString(Encoding.UTF8);

            if (string.IsNullOrWhiteSpace(parameterString))
            {
                Logger.Error(client, $"Invalid parameters supplied: {parameterString}");
                client.Socket.Close();
                return;
            }

            string[] parameters = parameterString.Split('|');
            if (parameters.Length < 2)
            {
                Logger.Error(client, $"To few parameters supplied: {parameterString}");
                client.Socket.Close();
                return;
            }

            string accountName = parameters[0];
            string password = parameters[1];

            Account account = Database.SelectAccount(accountName);
            if (account == null)
            {
                if (Settings.NeedRegistration)
                {
                    Logger.Error(client, $"AccountName: {accountName} doesn't exist");
                    client.Socket.Close();
                    Server.PluginDispatcher.AccountNameDoesNotExist(client, accountName);
                    return;
                }

                string bCryptHash = BCrypt.Net.BCrypt.HashPassword(password, EzSettings.BCryptWorkFactor);
                account = Database.CreateAccount(accountName, bCryptHash);
            }

            if (!BCrypt.Net.BCrypt.Verify(password, account.Hash))
            {
                Logger.Error(client, $"Invalid password for AccountName: {accountName}");
                client.Socket.Close();
                Server.PluginDispatcher.InvalidPassword(client, accountName);
                return;
            }

            if (Settings.AdminOnly)
            {
                if (account.State < AccountState.Admin)
                {
                    client.Socket.Close();
                    return;
                }
            }

            if (account.State <= AccountState.Banned)
            {
                client.Socket.Close();
                return;
            }

            Session existingSession = Server.Sessions.GetSession(account.Id);
            if (existingSession != null)
            {
                Logger.Error(client, $"Account: {accountName} already has a session.");
                client.Socket.Close();
                return;
            }

            EzClient existingClient = Server.Clients.GetClientByAccountId(account.Id);
            if (existingClient != null)
            {
                Logger.Error(client, $"Account: {accountName} already logged in.");
                client.Socket.Close();
                return;
            }

            string sessionKey = Server.Sessions.NewSessionKey();

            Session session = new Session(sessionKey, account);
            Server.Sessions.StoreSession(session);
            client.Session = session;

            Character character = Database.SelectCharacter(account.Id);
            if (character != null)
            {
                session.Character.Load(character);
                Setting setting = Database.SelectSetting(character.Id);
                if (setting == null)
                {
                    client.Setting.CharacterId = character.Id;
                    if (!Database.UpsertSetting(client.Setting))
                    {
                        Logger.Error(client, "Failed to save newly created setting for existing character");
                    }
                }
                else
                {
                    session.Setting.Load(setting);
                }

                session.Inventory.Load(Database.SelectInventoryItems(character.Id));
                session.Inventory.LoadGiftItems(Database.SelectGiftItems(character.Id));
                session.MessageBox.Load(Database.SelectMessages(character.Id));
                session.Friends.Load(Database.SelectFriends(character.Id));

                Score rubyMaxScore = Database.SelectMaxScore(session.Character.Id, ModeType.RubyMix);
                if (rubyMaxScore != null)
                {
                    session.Character.RubyMaxScore = rubyMaxScore.TotalScore;
                }

                Score streetMaxScore = Database.SelectMaxScore(session.Character.Id, ModeType.StreetMix);
                if (streetMaxScore != null)
                {
                    session.Character.StreetMaxScore = streetMaxScore.TotalScore;
                }

                Score clubMaxScore = Database.SelectMaxScore(session.Character.Id, ModeType.ClubMix);
                if (clubMaxScore != null)
                {
                    session.Character.ClubMaxScore = clubMaxScore.TotalScore;
                }
            }
            else if (Settings.NeedCharacter)
            {
                Logger.Error(client, $"AccountName: {accountName} has no character");
                client.Socket.Close();
                Server.PluginDispatcher.NoCharacter(client, accountName);
                return;
            }

            client.UpdateIdentity();
            Logger.Debug(client, $"Created SessionKey: {session.Key} for AccountName: {accountName}");


            account.LastLogin = DateTime.Now;
            Database.UpdateAccount(account);

            IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1); // 0 = NonBlocking | 1 = Blocking
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0x14);
            Router.Send(client, 0, response);

            IBuffer response1 = EzServer.Buffer.Provide();
            response1.WriteByte(0);
            response1.WriteByte(0);
            response1.WriteFixedString(sessionKey, 16);
            response1.WriteByte(0);
            Router.Send(client, 11, response1);

            IBuffer characterPacket = PacketBuilder.CharacterPacket.Create(client);
            Router.Send(client, 1, characterPacket);
        }
    }
}