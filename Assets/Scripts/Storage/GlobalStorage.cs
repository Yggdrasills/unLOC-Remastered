using Newtonsoft.Json;

using UnityEngine;

namespace SevenDays.unLOC.Storage
{
    public class GlobalStorage : IStorageRepository
    {
        private static readonly JsonSerializerSettings SerializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public void Save<T>(string key, T data)
        {
            var json = JsonConvert.SerializeObject(data, SerializationSettings);

            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public void Remove(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public T Load<T>(string key)
        {
            var json = PlayerPrefs.GetString(key, string.Empty);

            return JsonConvert.DeserializeObject<T>(json, SerializationSettings);
        }

        public bool TryLoad<T>(string key, out T data)
        {
            data = default;

            if (IsExists(key))
            {
                var json = PlayerPrefs.GetString(key);

                data = JsonConvert.DeserializeObject<T>(json, SerializationSettings);

                return true;
            }

            return false;
        }

        public bool IsExists(string key)
        {
            return !string.IsNullOrEmpty(PlayerPrefs.GetString(key));
        }
    }
}