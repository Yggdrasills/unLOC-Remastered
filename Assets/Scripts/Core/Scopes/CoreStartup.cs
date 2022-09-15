using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Core.Loaders;
#if UNITY_EDITOR
using SevenDays.unLOC.Profiles.Models;
using SevenDays.unLOC.Profiles.Services;
#endif
using SevenDays.unLOC.Storage;

using VContainer.Unity;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

namespace SevenDays.unLOC.Core.Scopes
{
    public class CoreStartup : IAsyncStartable
    {
        private readonly SceneLoader _sceneLoader;
#if UNITY_EDITOR
        private readonly IProfileService _profileService;
#endif
        public CoreStartup(SceneLoader sceneLoader, IProfileService profileService)
        {
            _sceneLoader = sceneLoader;
#if UNITY_EDITOR
            _profileService = profileService;
#endif
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            SetEditorCase(out var canGoNext);

            if (!canGoNext)
            {
                return;
            }

            await _sceneLoader.LoadMenuAsync();
        }

        private void SetEditorCase(out bool canGoNext)
        {
            canGoNext = true;
#if UNITY_EDITOR
            if (SceneManager.sceneCount > 1)
            {
                canGoNext = false;

                var scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

                SceneManager.SetActiveScene(scene);

                // note: add profile for editor if not exist
                if (scene.name == "Menu")
                {
                    return;
                }

                if (!new DataStorage().IsExists(nameof(ProfileCollection)))
                {
                    _profileService.CreateProfile();
                }
            }
#endif
        }
    }
}