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

using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packet.Builder;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packet.Login
{
    public class LoginRequest : Handler<EzServer>
    {
        public LoginRequest(EzServer server) : base(server)
        {
        }

        public override int Id => 0;

        public override void Handle(EzClient client, EzPacket packet)
        {
            byte[] paramOne = packet.Data.ReadBytes(17);
            byte[] paramTwo = packet.Data.ReadBytes(17);
            byte[] unknown = packet.Data.ReadBytes(4);
            byte[] paramThree = packet.Data.ReadBytes(35);
            byte[] paramVersion = packet.Data.ReadBytes(4);
            byte[] unknown1 = packet.Data.ReadBytes(7);
            byte[] paramFour = packet.Data.ReadBytes(4); // Last parameter, 4 numbers [0000 - 9999]
            byte[] unknown2 = packet.Data.ReadBytes(16);


            byte[] paramOneDecrypt = Utils.DecryptParameter(paramOne, Utils.KeyFirstParameter);
            byte[] paramTwoDecrypt = Utils.DecryptParameter(paramTwo, Utils.KeySecondParameter);

            string sessionKeyParam = Utils.ParameterToString(paramOneDecrypt);
            string accountNameParam = Utils.ParameterToString(paramTwoDecrypt);
            string passwordParam = Utils.ParameterToString(paramThree);
            string fourParam = Utils.ParameterToString(paramFour);
            string versionParam = Utils.ParameterToString(paramVersion);

            Account account = Database.SelectAccount(accountNameParam);
            if (account == null)
            {
                if (Settings.NeedRegistration)
                {
                    Logger.Error(client, $"AccountName: {accountNameParam} doesn't exist");
                    client.Socket.Close();
                    Server.PluginDispatcher.AccountNameDoesNotExist(client, accountNameParam);
                    return;
                }

                string bCryptHash = BCrypt.Net.BCrypt.HashPassword(passwordParam, EzSettings.BCryptWorkFactor);
                account = Database.CreateAccount(accountNameParam, bCryptHash);
            }

            if (!BCrypt.Net.BCrypt.Verify(passwordParam, account.Hash))
            {
                Logger.Error(client, $"Invalid password for AccountName: {accountNameParam}");
                client.Socket.Close();
                Server.PluginDispatcher.InvalidPassword(client, accountNameParam);
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
                session.MessageBox.Load(Database.SelectMessages(character.Id));
                session.Friends.Load(Database.SelectFriends(character.Id));
            }
            else if (Settings.NeedCharacter)
            {
                Logger.Error(client, $"AccountName: {accountNameParam} has no character");
                client.Socket.Close();
                Server.PluginDispatcher.NoCharacter(client, accountNameParam);
                return;
            }

            client.UpdateIdentity();
            Logger.Debug(client, $"Created SessionKey: {session.Key} for AccountName: {accountNameParam}");

            IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1); // 0 = NonBlocking // 1 = Blocking?
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

            IBuffer characterPacket = CharacterPacket.Create(client);
            Router.Send(client, 1, characterPacket);
        }
    }
}