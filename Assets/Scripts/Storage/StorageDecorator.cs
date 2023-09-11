using System;

using UnityEngine.Assertions;

namespace SevenDays.unLOC.Storage
{
    public class StorageDecorator : IStorageDecorator
    {
        private IStorageRepository _storage;

        public StorageDecorator()
        {
            _storage = new GlobalStorage();
        }

        public void SetStorage<T>()
            where T : IStorageRepository, new()
        {
            _storage = Activator.CreateInstance(typeof(T)) as IStorageRepository;
        }

        public void SetStorage<T, V>(V creationParams)
            where T : IStorageRepository
            where V : StorageCreationParameters, new()
        {
            _storage = Activator.CreateInstance(typeof(T), creationParams) as IStorageRepository;
        }

        public void Save<T>(string key, T data)
        {
            Assert.IsNotNull(key, $"[{nameof(StorageDecorator)}.{nameof(Remove)}] key is null");

            if (data == null)
            {
                return;
            }

            _storage.Save(key, data);
        }

        public void Remove(string key)
        {
            Assert.IsNotNull(key, $"[{nameof(StorageDecorator)}.{nameof(Remove)}] key is null");

            _storage.Remove(key);
        }

        public T Load<T>(string key)
        {
            Assert.IsNotNull(key, $"[{nameof(StorageDecorator)}.{nameof(Remove)}] key is null");

            return _storage.Load<T>(key);
        }

        public bool TryLoad<T>(string key, out T data)
        {
            Assert.IsNotNull(key, $"[{nameof(StorageDecorator)}.{nameof(Remove)}] key is null");

            return _storage.TryLoad(key, out data);
        }

        public bool IsExists(string key)
        {
            Assert.IsNotNull(key, $"[{nameof(StorageDecorator)}.{nameof(Remove)}] key is null");

            return _storage.IsExists(key);
        }
    }
}