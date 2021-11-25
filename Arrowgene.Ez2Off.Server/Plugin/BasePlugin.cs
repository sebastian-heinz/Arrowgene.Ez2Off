using Arrowgene.Ez2Off.Server.Model;

namespace Arrowgene.Ez2Off.Server.Plugin
{
    public abstract class BasePlugin : IPlugin
    {
        public void Start()
        {
        }

        public void Stop()
        {
        }

        public virtual void AccountNameDoesNotExist(EzClient client, string accountName)
        {
        }

        public virtual void InvalidPassword(EzClient client, string accountName)
        {
        }

        public virtual void NoCharacter(EzClient client, string accountName)
        {
        }
    }
}