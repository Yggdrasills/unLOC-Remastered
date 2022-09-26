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

        private readonly LifetimeScope _parentScope;

        private readonly GameServiceData _serviceData;

        private LifetimeScope _outerScope;

        public SceneLoader(ScreenIdentifier loadingScreen,
            IScreenService screenService,
            LifetimeScope parentScope,
            GameServiceData serviceData)
        {
            _loadingScreen = loadingScreen;
            _screenService = screenService;
            _parentScope = parentScope;
            _serviceData = serviceData;
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

        public async UniTask LoadCreditsAsync()
        {
            await LoadSceneAsync(GameConstants.CreditsSceneIndex);
        }

        public async UniTask LoadSceneByBuildIndexAsync(int buildIndex)
        {
            await LoadSceneAsync(buildIndex);
        }

#if UNITY_EDITOR
        // note: set scope if run two scenes from editor
        public void SetOuterScope(LifetimeScope scope)
        {
            _outerScope = scope;
        }
#endif

        private async UniTask LoadSceneAsync(int buildIndex)
        {
            await _screenService.ShowAsync(_loadingScreen, CancellationToken.None);

            var activeScene = SceneManager.GetActiveScene();

            // todo: костыль. Не нравится
            if (activeScene.buildIndex != 0)
            {
                await SceneManager.UnloadSceneAsync(activeScene);
            }

            switch (buildIndex)
            {
                case GameConstants.WorkshopSceneIndex:
                case GameConstants.StreetSceneIndex:
                case GameConstants.StreetStealthSceneIndex:
                case GameConstants.MelissaRoomSceneIndex:
                    if (_outerScope == _parentScope)
                    {
                        _outerScope = GameServiceInstaller.UseServices(_parentScope, _serviceData);
                    }

                    break;

                case GameConstants.CreditsSceneIndex:
                    _outerScope.Dispose();
                    _outerScope = _parentScope;
                    break;

                default:
                    // todo: тут должен быть баг, не разбирался
                    // todo: Думаю что при переходе в меню из геймплея будет что-то

                    _outerScope = _parentScope;
                    break;
            }

            using (LifetimeScope.EnqueueParent(_outerScope))
            {
                await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

                var loadedScene = SceneManager.GetSceneByBuildIndex(buildIndex);

                SceneManager.SetActiveScene(loadedScene);

                var rootGameObjects = loadedScene.GetRootGameObjects();

                foreach (var rootObject in rootGameObjects)
                {
                    if (rootObject.TryGetComponent(out LifetimeScope innerScope))
                    {
                        innerScope.Build();
                    }
                }

                Loaded?.Invoke(loadedScene.buildIndex);
            }

            await _screenService.HideAsync(_loadingScreen, CancellationToken.None);
        }
    }
}