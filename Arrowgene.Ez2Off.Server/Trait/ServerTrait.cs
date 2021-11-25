using Arrowgene.Ez2Off.Server.Model;

namespace Arrowgene.Ez2Off.Server.Trait
{
    public abstract class ServerTrait : EzTrait
    {
        public ServerTrait(EzServer server) : base(server)
        {
        }

        public abstract void ClientDisconnected(EzClient client);
    }
}