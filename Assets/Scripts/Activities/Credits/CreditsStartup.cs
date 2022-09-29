using System;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Core.Loaders;

using VContainer.Unity;

namespace Activities.Credits
{
    public class CreditsStartup : IInitializable, IDisposable
    {
        private readonly ClickableItem _clickableItem;
        private readonly SceneLoader _sceneLoader;

        public CreditsStartup(SceneLoader sceneLoader, ClickableItem clickableItem)
        {
            _clickableItem = clickableItem;
            _sceneLoader = sceneLoader;
        }

        void IInitializable.Initialize()
        {
            _clickableItem.Clicked += LoadMenu;
        }

        void IDisposable.Dispose()
        {
            _clickableItem.Clicked -= LoadMenu;
        }

        private void LoadMenu()
        {
            _sceneLoader.LoadMenuAsync().Forget();
        }
    }
}