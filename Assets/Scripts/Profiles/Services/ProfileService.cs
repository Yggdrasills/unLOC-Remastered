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

        private ProfileCollection _profileCollection;
        private readonly SceneLoader _sceneLoader;
        private readonly DataStorage _storage;

        public ProfileService(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;

            _storage = new DataStorage();
        }

        void IInitializable.Initialize()
        {
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
        }

        private void OnSceneLoaded(int sceneBuildIndex)
        {
            _profileCollection.ActiveProfile.SceneIndex = sceneBuildIndex;

            Save();
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

            _profileCollection.ActiveProfile = profile;

            Save();
        }

        void IProfileService.SetActiveProfile(int profileIndex)
        {
            var profile = _profileCollection.Profiles.SingleOrDefault(p => p.Index == profileIndex);

            if (profile == null)
            {
                return;
            }

            _profileCollection.ActiveProfile = profile;

            Save();
        }

        private void Save()
        {
            var profile = _profileCollection.ActiveProfile;

            profile.DateActivity = DateTime.UtcNow;

            _storage.Save(typeof(ProfileCollection).FullName, _profileCollection);
        }
    }
}