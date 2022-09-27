using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Core.Loaders;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests
{
    public class ExitDoorView : MonoBehaviour
    {
        private SceneLoader _sceneLoader;

        [Inject, UsedImplicitly]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void LoadWorkshop()
        {
            _sceneLoader.LoadWorkshopAsync().Forget();
        }
    }
}