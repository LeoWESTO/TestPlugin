using Resto.Front.Api;
using System;
using System.Reactive.Disposables;

namespace TestPlugin
{
    internal sealed class PopupTester : IDisposable
    {
        private readonly CompositeDisposable subscriptions;

        public PopupTester()
        {
            subscriptions = new CompositeDisposable()
            {
                // Добавить кнопку на экран заказа, при нажатии на которую выводить PopUp о общем количестве добавленных позиций в заказ
                PluginContext.Operations.AddButtonToOrderEditScreen("TestPlugin", x =>
                {
                    int itemsCount = x.order.Items.Count;
                    x.vm.ShowOkPopup("TestPlugin", $"Добавлено позиицй: {itemsCount}");
                }),

                // При создании заказа вывести PopUp с номером заказа и его типом (доставка или обычный)
                PluginContext.Notifications.OrderChanged.Subscribe(x =>
                {
                    PluginContext.Operations.AddNotificationMessage($"Заказ №{x.Entity.Number} типа {x.Entity.OrderType.Name}", "TestPlugin", TimeSpan.FromSeconds(10));
                }),
            };
        }

        public void Dispose()
        {
            subscriptions.Dispose();
        }
    }
}
