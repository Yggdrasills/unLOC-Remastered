using System;
using System.Linq;

using JetBrains.Annotations;

using SevenDays.unLOC.Core.Loaders;
using SevenDays.unLOC.Profiles.Models;
using SevenDays.unLOC.Storage;

using VContainer.Unity;

namespace SevenDays.unLOC.Profiles.Services
{
    [UsedImplicitly]
    public class ProfileService : IProfileService, IInitializable, IStartable, IDisposable
    {
        public ProfileInfo[] ProfileInfos => _profileCollection.Profiles.Select(p => p.Info).ToArray();

        private Profile _current;

        private ProfileCollection _profileCollection;
        private readonly SceneLoader _sceneLoader;
        private readonly IStorageDecorator _storage;

        public ProfileService(SceneLoader sceneLoader, IStorageDecorator storage)
        {
            _sceneLoader = sceneLoader;
            _storage = storage;
        }

        void IInitializable.Initialize()
        {
            _storage.SetStorage<GlobalStorage>();

            if (!_storage.TryLoad(typeof(ProfileCollection).FullName, out _profileCollection))
            {
                _profileCollection = new ProfileCollection();
            }
        }

        void IStartable.Start()
        {
            _sceneLoader.Loaded += OnSceneLoaded;
        }

        void IDisposable.Dispose()
        {
            _sceneLoader.Loaded -= OnSceneLoaded;

            _storage.SetStorage<GlobalStorage>();

            _storage.Save(typeof(ProfileCollection).FullName, _profileCollection);

            if (_current != null)
            {
                _storage.SetStorage<LocalStorage, LocalStorageCreationParameters>(
                    new LocalStorageCreationParameters(_current.Info.Index));
            }
        }

        private void OnSceneLoaded(int sceneBuildIndex)
        {
            if (_current == null)
            {
                return;
            }

            _current.SceneIndex = sceneBuildIndex;
            _current.Info.DateActivity = DateTime.UtcNow;
        }

        void IProfileService.CreateProfile()
        {
            var profileIndex = _profileCollection.Profiles.LastOrDefault()?.Info.Index + 1 ?? 0;

            var profile = new Profile()
            {
                SceneIndex = 1,
                Info = new ProfileInfo()
                {
                    Index = profileIndex,
                    DateCreation = DateTime.UtcNow,
                    DateActivity = DateTime.UtcNow
                }
            };

            _profileCollection.Profiles.Add(profile);

            SetCurrent(profile);
        }

        void IProfileService.SetActiveProfile(int profileIndex)
        {
            var profile = _profileCollection.Profiles.SingleOrDefault(p => p.Info.Index == profileIndex);

            if (profile == null)
            {
                return;
            }

            SetCurrent(profile);
        }

        int IProfileService.GetSceneIndex(int profileIndex)
        {
            var profile = _profileCollection.Profiles.SingleOrDefault(p => p.Info.Index == profileIndex);

            return profile?.SceneIndex ?? -1;
        }

        private void SetCurrent(Profile profile)
        {
            _current = profile;
            _storage.SetStorage<LocalStorage, LocalStorageCreationParameters>(
                new LocalStorageCreationParameters(_current.Info.Index));
        }
    }
}