using System;
using System.Collections.Generic;
using Arrowgene.Ez2Off.Common.Models;
using Arrowgene.Ez2Off.Server.Database;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Ez2Off.Server.Packet;
using Arrowgene.Services.Tasks;

namespace Arrowgene.Ez2Off.Server.Tasks
{
    public class RemoveExpiredGifts : PeriodicTask
    {
        private readonly IDatabase _database;
        private readonly ClientLookup _clients;
        private readonly PacketRouter _router;


        public RemoveExpiredGifts(EzServer server)
        {
            _clients = server.Clients;
            _database = server.Database;
            _router = server.Router;
        }

        public override string Name => "RemoveExpiredGifts";
        public override TimeSpan TimeSpan => TimeSpan.FromMinutes(18);

        protected override bool RunAtStart => false;

        protected override void Execute()
        {
            List<int> expiredGiftIds = new List<int>();
            List<GiftItem> expiredGifts = _database.SelectExpiredGifts();
            foreach (GiftItem expiredGift in expiredGifts)
            {
                expiredGiftIds.Add(expiredGift.Id);
                EzClient client = _clients.GetClient(expiredGift.ReceiverId);
                if (client != null)
                {
                    GiftItem expiredClientGift = client.Inventory.GetGiftItemById(expiredGift.Id);
                    if (expiredClientGift != null)
                    {
                        if (client.Inventory.RemoveGiftItem(expiredClientGift))
                        {
                            _router.Send(client, PacketBuilder.InventoryPacket.ShowGiftsPacket(
                                client.Inventory.GetGiftItems()
                            ));
                            Logger.Info(
                                $"Removed gift [{expiredClientGift.Id}]{expiredClientGift.ItemId} from client");
                        }
                        else
                        {
                            Logger.Error(
                                $"failed to remove gift [{expiredClientGift.Id}]{expiredClientGift.ItemId} from client");
                        }
                    }
                }
            }

            if (expiredGiftIds.Count > 0)
            {
                Logger.Info($"Delete {expiredGiftIds.Count} expired gifts from database");
                if (!_database.DeleteGifts(expiredGiftIds))
                {
                    Logger.Error("Failed to delete expired gifts");
                }
            }
        }
    }
}