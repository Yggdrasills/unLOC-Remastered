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
        public Profile[] Profiles => _profileCollection.Profiles.ToArray();

        private Profile Current => _profileCollection.ActiveProfile;

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
            _storage.SetStorage(new GlobalStorage());

            if (!_storage.TryLoad(typeof(ProfileCollection).FullName, out _profileCollection))
            {
                _profileCollection = new ProfileCollection();
            }
            else
            {
                _storage.SetStorage(new LocalStorage(Current.Index));
            }
        }

        void IStartable.Start()
        {
            _sceneLoader.Loaded += OnSceneLoaded;
        }

        void IDisposable.Dispose()
        {
            _sceneLoader.Loaded -= OnSceneLoaded;

            _storage.SetStorage(new GlobalStorage());

            _storage.Save(typeof(ProfileCollection).FullName, _profileCollection);

            _storage.SetStorage(new LocalStorage(Current.Index));
        }

        private void OnSceneLoaded(int sceneBuildIndex)
        {
            if (_profileCollection.ActiveProfile == null)
            {
                return;
            }

            _profileCollection.ActiveProfile.SceneIndex = sceneBuildIndex;
            _profileCollection.ActiveProfile.DateActivity = DateTime.UtcNow;
        }

        void IProfileService.CreateProfile()
        {
            var profileIndex = _profileCollection.Profiles.LastOrDefault()?.Index + 1 ?? 0;

            var profile = new Profile()
            {
                Index = profileIndex,
                SceneIndex = 1,
                DateCreation = DateTime.UtcNow,
                DateActivity = DateTime.UtcNow
            };

            _profileCollection.Profiles.Add(profile);

            SetActiveProfile(profile);
        }

        void IProfileService.SetActiveProfile(int profileIndex)
        {
            var profile = _profileCollection.Profiles.SingleOrDefault(p => p.Index == profileIndex);

            if (profile == null)
            {
                return;
            }

            SetActiveProfile(profile);
        }

        private void SetActiveProfile(Profile profile)
        {
            _profileCollection.ActiveProfile = profile;

            _storage.SetStorage(new GlobalStorage());

            _storage.SetProfileIndex(profile.Index);

            _storage.SetStorage(new LocalStorage(profile.Index));
        }
    }
}