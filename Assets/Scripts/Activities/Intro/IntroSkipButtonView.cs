using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Core.Loaders;

using UnityEngine;
using UnityEngine.UI;

using VContainer;

namespace SevenDays.unLOC.Activities.Intro
{
    public class IntroSkipButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button _skipButton;

        private SceneLoader _sceneLoader;

        [Inject, UsedImplicitly]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            _skipButton.onClick.AddListener(LoadWorkshop);
        }

        private void LoadWorkshop()
        {
            _sceneLoader.LoadWorkshopAsync().Forget();
        }
    }
}