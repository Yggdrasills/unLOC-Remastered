using System;
using System.Threading;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using JetBrains.Annotations;

using SevenDays.Screens.Models;
using SevenDays.Screens.Services;
using SevenDays.unLOC.Core.Loaders;
using SevenDays.unLOC.Profiles.Models;
using SevenDays.unLOC.Profiles.Services;

using UnityEditor;

using VContainer.Unity;

using Object = UnityEngine.Object;

namespace SevenDays.unLOC.Menu
{
    [UsedImplicitly]
    public class MenuController : IAsyncStartable, IDisposable
    {
        private readonly SceneLoader _sceneLoader;

        private readonly ScreenIdentifier _menuScreen;

        private readonly ProfileLoadButton _profileLoadButtonPrefab;

        private readonly IProfileService _profileService;
        private readonly IScreenService _screenService;

        private MenuScreenView _menuScreenView;

        public MenuController(
            SceneLoader sceneLoader,
            IScreenService screenService,
            ScreenIdentifier menuScreen,
            ProfileLoadButton profileLoadButtonPrefab,
            IProfileService profileService)
        {
            _sceneLoader = sceneLoader;
            _screenService = screenService;
            _menuScreen = menuScreen;
            _profileLoadButtonPrefab = profileLoadButtonPrefab;
            _profileService = profileService;
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            _menuScreenView = await _screenService.ShowAsync<MenuScreenView>(_menuScreen, cancellation);

            _menuScreenView.NewGameButton.OnClickAsAsyncEnumerable().SubscribeAwait(OnNewGameButtonClickedAsync);

            _menuScreenView.ExitGameButton.OnClickAsAsyncEnumerable().Subscribe(_ => OnExitButtonClickedAsync());
            _menuScreenView.LoadGameButton.OnClickAsAsyncEnumerable().Subscribe(_ => LoadProfileButton());

            LoadProfileButton();
        }

        void IDisposable.Dispose()
        {
            _screenService.HideAsync(_menuScreen, CancellationToken.None).Forget();
        }

        private async UniTask OnNewGameButtonClickedAsync(AsyncUnit _, CancellationToken __)
        {
            _profileService.CreateProfile();

            await _sceneLoader.LoadIntroAsync();
        }

        private void OnExitButtonClickedAsync()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void LoadProfileButton()
        {
            for (var i = 0; i < _menuScreenView.ProfileContainer.childCount; i++)
            {
                var child = _menuScreenView.ProfileContainer.GetChild(i);

                Object.Destroy(child.gameObject);
            }

            var profiles = _profileService.Profiles;

            for (var i = 0; i < profiles.Length; i++)
            {
                var profile = profiles[i];

                var profileLoadButton = Object.Instantiate(
                    _profileLoadButtonPrefab,
                    _menuScreenView.ProfileContainer);

                profileLoadButton.SetButtonText(
                    profile.Index.ToString(),
                    profile.DateCreation,
                    profile.DateActivity);

                profileLoadButton.Button.OnClickAsAsyncEnumerable().SubscribeAwait((_, __) =>
                    OnProfileLoadButtonOnClicked(profile));
            }
        }

        private async UniTask OnProfileLoadButtonOnClicked(Profile profile)
        {
            _profileService.SetActiveProfile(profile.Index);

            await _sceneLoader.LoadSceneByBuildIndexAsync(profile.SceneIndex);
        }
    }
}