using SevenDays.unLOC.Inventory.Services;

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
                scope = LifetimeScope.Create(builder =>
                {
                    builder.RegisterComponentInNewPrefab(serviceData.InventoryViewPrefab, Lifetime.Scoped)
                        .UnderTransform(parent.transform);

                    builder.RegisterComponentInNewPrefab(serviceData.PadViewPrefab, Lifetime.Scoped)
                        .UnderTransform(parent.transform);

                    builder.RegisterComponentInNewPrefab(serviceData.ScrewdriverViewPrefab, Lifetime.Scoped)
                        .UnderTransform(parent.transform);

                    builder.RegisterEntryPoint<InventoryService>()
                        .WithParameter(serviceData.CellPrefab);
                });

                scope.name = "Services";
            }

            return scope;
        }
    }
}