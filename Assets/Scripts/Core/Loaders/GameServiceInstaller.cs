using SevenDays.unLOC.Inventory.Services;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Loaders
{
    public static class GameServiceInstaller
    {
        public static LifetimeScope UseServices(LifetimeScope parent, GameServiceData serviceData)
        {
            LifetimeScope scope;

            using (LifetimeScope.EnqueueParent(parent))
            {
                var container = new GameObject("Service Container");
                scope = LifetimeScope.Create(builder =>
                {
                    builder.RegisterComponentInNewPrefab(serviceData.InventoryViewPrefab, Lifetime.Scoped)
                        .UnderTransform(container.transform);

                    builder.RegisterComponentInNewPrefab(serviceData.PadViewPrefab, Lifetime.Scoped)
                        .UnderTransform(container.transform);

                    builder.RegisterComponentInNewPrefab(serviceData.ScrewdriverViewPrefab, Lifetime.Scoped)
                        .UnderTransform(container.transform);

                    builder.RegisterEntryPoint<InventoryService>()
                        .WithParameter(serviceData.CellPrefab);
                });

                container.transform.SetParent(scope.transform);
                scope.name = "Services";
            }

            return scope;
        }
    }
}