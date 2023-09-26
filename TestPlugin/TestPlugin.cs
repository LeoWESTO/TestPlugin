using Resto.Front.Api;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Remoting;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Attributes;

namespace TestPlugin
{
    [UsedImplicitly]
    [PluginLicenseModuleId(21005108)]
    public sealed class TestPlugin : IFrontPlugin
    {
        private readonly Stack<IDisposable> subscriptions = new Stack<IDisposable>();
        public TestPlugin()
        {
            PluginContext.Log.Info("Initializing TestPlugin");

            subscriptions.Push(new PopupTester());

            PluginContext.Log.Info("TestPlugin started");

            // При старте плагина вывести PopUp с информацией, что плагин запустился
            PluginContext.Operations.AddNotificationMessage("Плагин запустился!", "TestPlugin", TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            while (subscriptions.Any())
            {
                var subscription = subscriptions.Pop();
                try
                {
                    subscription.Dispose();
                }
                catch (RemotingException ex)
                {
                    PluginContext.Log.Error($"TestPlugin stopped. Message: {ex.Message}");
                }
            }

            PluginContext.Log.Info("TestPlugin stopped");
        }
    }
}
