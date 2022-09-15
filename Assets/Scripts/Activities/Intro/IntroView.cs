using Cysharp.Threading.Tasks;

using SevenDays.DialogSystem.Runtime;
using SevenDays.unLOC.Core.Loaders;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Intro
{
    public class IntroView : MonoBehaviour
    {
        [field: SerializeField]
        public DialogWindow DialogWindow { get; private set; }

        private SceneLoader _sceneLoader;

        [Inject]
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