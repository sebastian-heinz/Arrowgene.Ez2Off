using Arrowgene.Ez2Off.Server.Model;

namespace Arrowgene.Ez2Off.Server.Plugin
{
    public interface IPlugin
    {
        void Start();
        void Stop();
        void AccountNameDoesNotExist(EzClient client, string accountName);
        void InvalidPassword(EzClient client, string accountName);
        void NoCharacter(EzClient client, string accountName);
    }
}