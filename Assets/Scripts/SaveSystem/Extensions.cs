using VContainer;
using VContainer.Unity;

namespace SevenDays.SaveSystem
{
    public static class Extensions
    {
        public static void UseSaveSystem(this IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SaveSystemComponent>().AsImplementedInterfaces().AsSelf();
        }
    }
}