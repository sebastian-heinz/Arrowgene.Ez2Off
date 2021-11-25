using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Ez2Off.Server.Model;
using Arrowgene.Services;
using Arrowgene.Logging;

namespace Arrowgene.Ez2Off.Server.Plugin
{
    public class PluginDispatcher : IPlugin
    {
        private static readonly ILogger _logger = LogProvider.Logger(typeof(PluginDispatcher));
        
        private readonly PluginRegistry _pluginRegistry;
        private BlockingCollection<Action> _queue;
        private ICollection<IPlugin> _plugins;
        private Thread _thread;
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        public PluginDispatcher()
        {
            _pluginRegistry = new PluginRegistry();
            _isRunning = false;
        }

        public void Start()
        {
            _queue = new BlockingCollection<Action>();
            _pluginRegistry.Load("./");
            _plugins = _pluginRegistry.GetPlugins<IPlugin>();
            foreach (IPlugin plugin in _plugins)
            {
                _logger.Info($"Loaded Plugin: {plugin.GetType()}");
            }

            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();
            _thread = new Thread(Dispatcher);
            _thread.Name = "PluginDispatcher";
            _thread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _cancellationTokenSource.Cancel();
            Service.JoinThread(_thread, 1000, _logger);
            _queue = null;
        }

        public void AccountNameDoesNotExist(EzClient client, string accountName)
        {
            foreach (IPlugin plugin in _plugins)
            {
                _queue.Add(() => { plugin.AccountNameDoesNotExist(client, accountName); });
            }
        }

        public void InvalidPassword(EzClient client, string accountName)
        {
            foreach (IPlugin plugin in _plugins)
            {
                _queue.Add(() => { plugin.InvalidPassword(client, accountName); });
            }
        }

        public void NoCharacter(EzClient client, string accountName)
        {
            foreach (IPlugin plugin in _plugins)
            {
                _queue.Add(() => { plugin.NoCharacter(client, accountName); });
            }
        }

        private void Dispatcher()
        {
            while (_isRunning)
            {
                Action action;
                try
                {
                    action = _queue.Take(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    _logger.Error("Plugin Exception");
                    _logger.Exception(ex);
                }
            }
        }
    }
}