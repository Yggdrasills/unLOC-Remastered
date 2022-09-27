using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Core.Loaders;
using SevenDays.unLOC.Profiles.Models;
using SevenDays.unLOC.Profiles.Services;
using SevenDays.unLOC.Storage;

using UnityEngine.SceneManagement;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class CoreStartup : IStartable
    {
        private readonly SceneLoader _sceneLoader;
#if UNITY_EDITOR
        private readonly IProfileService _profileService;
        private readonly IStorageRepository _storage;
        private readonly LifetimeScope _parentScope;
        private readonly GameServiceData _serviceData;
#endif
        public CoreStartup(SceneLoader sceneLoader,
            IProfileService profileService,
            IStorageRepository storage,
            LifetimeScope parentScope,
            GameServiceData serviceData)
        {
            _sceneLoader = sceneLoader;
#if UNITY_EDITOR
            _profileService = profileService;
            _storage = storage;
            _parentScope = parentScope;
            _serviceData = serviceData;
#endif
        }

        void IStartable.Start()
        {
            SetEditorCase(out var canGoNext);

            if (!canGoNext)
            {
                return;
            }

            _sceneLoader.LoadMenuAsync().Forget();
        }

        private void SetEditorCase(out bool canGoNext)
        {
            canGoNext = true;
#if UNITY_EDITOR
            if (SceneManager.sceneCount > 1)
            {
                canGoNext = false;

                // note: поддерживается только вторая сцена. Третью не добавляйте. Только Core + что-то
                var scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

                SceneManager.SetActiveScene(scene);

                var sceneRootObjects = scene.GetRootGameObjects();

                for (int k = 0; k < sceneRootObjects.Length; k++)
                {
                    if (sceneRootObjects[k].TryGetComponent(out LifetimeScope scope))
                    {
                        LifetimeScope outerScope = _parentScope;

                        if (scene.buildIndex == 3 || scene.buildIndex == 4)
                        {
                            outerScope = GameServiceInstaller.UseServices(_parentScope, _serviceData);

                            SceneManager.MoveGameObjectToScene(outerScope.gameObject, SceneManager.GetSceneAt(0));

                            _sceneLoader.SetOuterScope(outerScope);
                        }

                        using (LifetimeScope.EnqueueParent(outerScope))
                        {
                            scope.Build();
                        }
                    }
                }

                // note: add profile for editor if not exist
                if (scene.buildIndex > 1)
                {
                    if (!_storage.IsExists(typeof(ProfileCollection).FullName))
                    {
                        _profileService.CreateProfile();
                    }
                }
            }
#endif
        }
    }
}