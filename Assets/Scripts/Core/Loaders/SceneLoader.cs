using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.Screens.Models;
using SevenDays.Screens.Services;

using UnityEngine.SceneManagement;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Loaders
{
    public class SceneLoader
    {
        private readonly ScreenIdentifier _loadingScreen;
        private readonly IScreenService _screenService;

        private int _activeSceneIndex;

        public SceneLoader(ScreenIdentifier loadingScreen, IScreenService screenService)
        {
            _loadingScreen = loadingScreen;
            _screenService = screenService;
        }

        public async UniTask LoadMenuAsync()
        {
            await LoadSceneAsync(1);
        }

        public async UniTask LoadIntroAsync()
        {
            await LoadSceneAsync(2);
        }

        public async UniTask LoadWorkshopAsync()
        {
            await LoadSceneAsync(3);
        }

        public async UniTask LoadStreetAsync()
        {
            await LoadSceneAsync(4);
        }

        public async UniTask LoadStreetStealthAsync()
        {
            await LoadSceneAsync(5);
        }

        public async UniTask LoadMelissaRoomAsync()
        {
            await LoadSceneAsync(6);
        }

        private async UniTask LoadSceneAsync(int buildIndex)
        {
            await _screenService.ShowAsync(_loadingScreen, CancellationToken.None);

            var activeScene = SceneManager.GetActiveScene();

            // todo: костыль. Не нравится
            if (activeScene.buildIndex != 0)
            {
                await SceneManager.UnloadSceneAsync(activeScene);
            }

            var sceneLoadOperation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

            await UniTask.WaitUntil(() => sceneLoadOperation.isDone);

            var loadedScene = SceneManager.GetSceneByBuildIndex(buildIndex);

            SceneManager.SetActiveScene(loadedScene);

            _activeSceneIndex = buildIndex;

            var rootGameObjects = loadedScene.GetRootGameObjects();

            foreach (var rootObject in rootGameObjects)
            {
                if (rootObject.TryGetComponent(out LifetimeScope scope))
                {
                    scope.Build();
                }
            }

            await _screenService.HideAsync(_loadingScreen, CancellationToken.None);
        }
    }
}