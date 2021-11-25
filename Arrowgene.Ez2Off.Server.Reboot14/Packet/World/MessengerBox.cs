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
using Arrowgene.Ez2Off.Common;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;

namespace Arrowgene.Ez2Off.Server.Reboot14.Packet.World
{
    public class MessengerBox : Handler<EzServer>
    {
        public MessengerBox(EzServer server) : base(server)
        {
        }

        public override int Id => 43;

        public override void Handle(EzClient client, EzPacket packet)
        {
            int unknown = packet.Data.ReadInt32();
            int messageAction = packet.Data.ReadInt32();
            int messageId = packet.Data.ReadInt32();
            MessageBoxActionType action = (MessageBoxActionType) messageAction;
            string receiver = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);
            string sender = packet.Data.ReadFixedString(18, Utils.KoreanEncoding);
            string content = packet.Data.ReadFixedString(120, Utils.KoreanEncoding);
            int unknown3 = packet.Data.ReadInt32();
            int unknown4 = packet.Data.ReadInt32();

            Logger.Debug($"Unknown {unknown}");
            Logger.Debug($"action {action}");
            Logger.Debug($"messageId {messageId}");
            Logger.Debug($"receiver {receiver}");
            Logger.Debug($"sender {sender}");
            Logger.Debug($"message {content}");
            Logger.Debug($"unknown3 {unknown3}");
            Logger.Debug($"unknown4 {unknown4}");


            switch (action)
            {
                case MessageBoxActionType.View:
                {
                    List<Message> messages = client.MessageBox.GetMessages();
                    Router.Send(client, 61, PacketBuilder.MessagePacket.CreateMessageBox(messages));
                    break;
                }
                case MessageBoxActionType.Open:
                {
                    Message message = client.MessageBox.GetMessage(messageId);
                    message.Read = true;
                    if (!Database.UpdateMessage(message))
                    {
                        Logger.Error(client, $"Couldn't update MessageId: {message.Id} from database");
                    }

                    break;
                }
                case MessageBoxActionType.Delete:
                {
                    Message message = client.MessageBox.GetMessage(messageId);
                    client.MessageBox.RemoveMessage(message);

                    if (!Database.DeleteMessage(message.Id))
                    {
                        Logger.Error(client, $"Couldn't delete MessageId: {message.Id} from database");
                    }

                    List<Message> messages = client.MessageBox.GetMessages();
                    Router.Send(client, 61, PacketBuilder.MessagePacket.CreateMessageBox(messages));
                    //Router.Send(client, 62,
                    //    PacketBuilder.MessagePacket.CreateMessageResponse(MessageResponseType.Success));
                    break;
                }
                case MessageBoxActionType.Send:
                {
                    Message message = new Message();
                    message.Content = content;
                    message.Read = false;
                    message.Sender = sender;
                    message.SenderId = client.Character.Id;
                    message.Receiver = receiver;
                    message.SendAt = DateTime.Now;

                    EzClient receiverClient = Server.Clients.GetClient(receiver);
                    if (receiverClient != null)
                    {
                        message.ReceiverId = receiverClient.Character.Id;
                        receiverClient.MessageBox.AddMessage(message);
                        Logger.Debug(client, $"Added MessageId: {message.Id} to MessageBox");
                        Router.Send(receiverClient, 63, PacketBuilder.MessagePacket.CreateNewMessageNotification());
                    }
                    else
                    {
                        Character receiverCharacter = Database.SelectCharacter(receiver);
                        if (receiverCharacter == null)
                        {
                            Logger.Error(client, $"Receiver: {receiver} does not exist");
                            Router.Send(client, 62,
                                PacketBuilder.MessagePacket.CreateMessageResponse(MessageResponseType
                                    .ReceiverNotExist));
                            return;
                        }

                        Logger.Debug(client,
                            $"Character: {receiver} offline, save MessageId: {message.Id} to database");
                        message.ReceiverId = receiverCharacter.Id;
                    }

                    if (!Database.InsertMessage(message))
                    {
                        Logger.Error(client, $"Could not save MessageId: {message.Id} to database");
                    }

                    Router.Send(client, 62,
                        PacketBuilder.MessagePacket.CreateMessageResponse(MessageResponseType.Success));
                    break;
                }
            }
        }
    }
}