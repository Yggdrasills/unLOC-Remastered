using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.Screens.Models;
using SevenDays.Screens.Services;
using SevenDays.Utils.Constants;

using UnityEngine.SceneManagement;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Loaders
{
    public class SceneLoader
    {
        public event Action<int> Loaded;

        private readonly ScreenIdentifier _loadingScreen;
        private readonly IScreenService _screenService;

        public SceneLoader(ScreenIdentifier loadingScreen, IScreenService screenService)
        {
            _loadingScreen = loadingScreen;
            _screenService = screenService;
        }

        public async UniTask LoadMenuAsync()
        {
            await LoadSceneAsync(GameConstants.MenuSceneIndex);
        }

        public async UniTask LoadIntroAsync()
        {
            await LoadSceneAsync(GameConstants.IntroSceneIndex);
        }

        public async UniTask LoadWorkshopAsync()
        {
            await LoadSceneAsync(GameConstants.WorkshopSceneIndex);
        }

        public async UniTask LoadStreetAsync()
        {
            await LoadSceneAsync(GameConstants.StreetSceneIndex);
        }

        public async UniTask LoadStreetStealthAsync()
        {
            await LoadSceneAsync(GameConstants.StreetStealthSceneIndex);
        }

        public async UniTask LoadMelissaRoomAsync()
        {
            await LoadSceneAsync(GameConstants.MelissaRoomSceneIndex);
        }

        public async UniTask LoadSceneByBuildIndexAsync(int buildIndex)
        {
            await LoadSceneAsync(buildIndex);
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

            var rootGameObjects = loadedScene.GetRootGameObjects();

            foreach (var rootObject in rootGameObjects)
            {
                if (rootObject.TryGetComponent(out LifetimeScope scope))
                {
                    scope.Build();
                }
            }
            
            Loaded?.Invoke(loadedScene.buildIndex);

            await _screenService.HideAsync(_loadingScreen, CancellationToken.None);
        }
    }
}