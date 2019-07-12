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
using Arrowgene.Ez2Off.Server.Client;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Ez2Off.Server.Reboot13.Packets.Builder;
using Arrowgene.Ez2Off.Server.Settings;
using Arrowgene.Services.Buffers;

namespace Arrowgene.Ez2Off.Server.Reboot13.Packets.Login
{
    public class LoginRequest : Handler<LoginServer>
    {
        public LoginRequest(LoginServer server) : base(server)
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

            string sessionKey = Utils.ParameterToString(paramOneDecrypt);
            string accountName = Utils.ParameterToString(paramTwoDecrypt);
            string password = Utils.ParameterToString(paramThree);
            string four = Utils.ParameterToString(paramFour);
            string version = Utils.ParameterToString(paramVersion);

            Session session;
            if (Server.Settings.ApiSettings.NeedRegistration)
            {
                session = Server.SessionManager.GetSession(sessionKey);
                if (session == null)
                {
                    _logger.Info("Invalid session key ({0}) provided by client ({1})",
                        sessionKey, client.Identity);
                    client.Socket.Close();
                    return;
                }

                if (session.Account.Name != accountName)
                {
                    _logger.Info("Account ({0}) doesn't match session {{1}) for client ({2})",
                        accountName, session.Account, client.Identity);
                    client.Socket.Close();
                    return;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, session.Account.Hash))
                {
                    _logger.Info("Hash ({0}) doesn't match session {{1}) for client ({2})",
                        password, session.Account.Hash, client.Identity);
                    client.Socket.Close();
                    return;
                }
            }
            else
            {
                Account account = Server.Database.SelectAccount(accountName);

                if (account == null)
                {
                    string bCryptHash = BCrypt.Net.BCrypt.HashPassword(password, ApiSettings.BCryptWorkFactor);
                    account = Server.Database.CreateAccount(accountName, bCryptHash);
                }
                else if (!BCrypt.Net.BCrypt.Verify(password, account.Hash))
                {
                    _logger.Info("Wrong password for account: {0}", accountName);
                    client.Socket.Close();
                    return;
                }

                session = new Session(sessionKey, account);
                Server.SessionManager.StoreSession(session);
                _logger.Info("Created session ({0}) for {0} ", session.Key, session.Account);
            }

            session.Character = Database.SelectCharacter(session.Account.Id);
            session.Setting = Database.SelectSetting(session.Account.Id);
            session.Inventory = new Inventory(Database.SelectInventoryItems(session.Account.Id));

            if (session.Setting == null)
            {
                session.Setting = new Setting();
                if (!Server.Database.UpsertSetting(session.Setting, session.Account.Id))
                {
                    _logger.Error("Failed to save newly created setting.");
                }
            }

            client.Session = session;

            // TODO implement login protection
            client.Session.IsLoggedIn = true;

            _logger.Debug("Client {0} Login (params: sessionKey:{1} account:{2} hash:{3} Four:{4}) Version:{5}",
                client.Identity, sessionKey, accountName, password, four, version);

            IBuffer response = EzServer.Buffer.Provide();
            response.WriteByte(1); // 0 = NonBlocking // 1 = Blocking?
            response.WriteByte(0);
            response.WriteByte(0);
            response.WriteByte(0x14);
            Send(client, 0, response);

            IBuffer characterPacket = CharacterPacket.Create(client);
            Send(client, 1, characterPacket);
        }
    }
}