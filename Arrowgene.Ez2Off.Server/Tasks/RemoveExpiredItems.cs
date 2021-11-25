using System;
using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Buffers;
using Arrowgene.Services.Tasks;

namespace Arrowgene.Ez2Off.Server.Tasks
{
    public class RemoveExpiredItems : PeriodicTask
    {
        private readonly IDatabase _database;
        private readonly ClientLookup _clients;
        private readonly PacketRouter _router;


        public RemoveExpiredItems(EzServer server)
        {
            _clients = server.Clients;
            _database = server.Database;
            _router = server.Router;
        }

        public override string Name => "RemoveExpiredItems";
        public override TimeSpan TimeSpan => TimeSpan.FromMinutes(10);

        protected override bool RunAtStart => false;

        protected override void Execute()
        {
            List<int> expiredInventoryIds = new List<int>();
            List<InventoryItem> expiredItems = _database.SelectExpiredInventoryItems();
            foreach (InventoryItem expiredItem in expiredItems)
            {
                expiredInventoryIds.Add(expiredItem.Id);
                EzClient client = _clients.GetClient(expiredItem.CharacterId);
                if (client != null)
                {
                    InventoryItem expiredClientItem = client.Inventory.GetItemById(expiredItem.Id);
                    if (expiredClientItem != null)
                    {
                        if (client.Inventory.RemoveItem(expiredClientItem))
                        {
                            int slot = expiredItem.Slot;
                            if (slot == Inventory.InvalidSlot)
                            {
                                client.Inventory.ItemsChanged = true;
                                if (client.Game == null)
                                {
                                    IBuffer showInventoryPacket =
                                        PacketBuilder.InventoryPacket.ShowInventoryPacket(client.Inventory);
                                    _router.Send(client, 30, showInventoryPacket);
                                    if (client.Room != null)
                                    {
                                        _router.Send(client.Room, 31,
                                            PacketBuilder.RoomPacket.ItemUpdate((byte) client.Player.Slot,
                                                client.Inventory));
                                    }
                                }
                            }
                            else
                            {
                                IBuffer buffer = EzServer.Buffer.Provide();
                                buffer.WriteInt32(0); // 0=Success 3=Inventory full
                                buffer.WriteByte(0);
                                buffer.WriteByte((byte) slot); // Index
                                buffer.WriteInt16(0); // ItemId
                                buffer.WriteInt32(client.Character.Coin);
                                _router.Send(client, 35, buffer);
                            }

                            Logger.Info(
                                $"Removed item [{expiredClientItem.Id}]{expiredClientItem.Item.Name} from client");
                        }
                        else
                        {
                            Logger.Error(
                                $"failed to remove item [{expiredClientItem.Id}]{expiredClientItem.Item.Name} from client");
                        }
                    }
                }
            }

            if (expiredInventoryIds.Count > 0)
            {
                Logger.Info($"Delete {expiredItems.Count} expired items from database");
                if (!_database.DeleteInventoryItems(expiredInventoryIds))
                {
                    Logger.Error("Failed to delete expired items");
                }
            }
        }
    }
}