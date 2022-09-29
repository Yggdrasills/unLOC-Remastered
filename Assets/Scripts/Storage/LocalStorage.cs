using System.Text;

using Newtonsoft.Json;

using UnityEngine;

namespace SevenDays.unLOC.Storage
{
    public class LocalStorage : IStorageRepository
    {
        private static readonly JsonSerializerSettings SerializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private readonly StringBuilder _stringBuilder;

        private readonly int _profileIndex;

        public LocalStorage(LocalStorageCreationParameters creationParameters)
        {
            _stringBuilder = new StringBuilder();
            _profileIndex = creationParameters.Index;
        }

        public void Save<T>(string key, T data)
        {
            var json = JsonConvert.SerializeObject(data, SerializationSettings);

            PlayerPrefs.SetString(GetFullKey(key), json);
            PlayerPrefs.Save();
        }

        public void Remove(string key)
        {
            PlayerPrefs.DeleteKey(GetFullKey(key));
            PlayerPrefs.Save();
        }

        public T Load<T>(string key)
        {
            var json = PlayerPrefs.GetString(GetFullKey(key), string.Empty);

            return JsonConvert.DeserializeObject<T>(json, SerializationSettings);
        }

        public bool TryLoad<T>(string key, out T data)
        {
            data = default;

            if (IsExists(key))
            {
                key = GetFullKey(key);

                var json = PlayerPrefs.GetString(key);

                data = JsonConvert.DeserializeObject<T>(json, SerializationSettings);

                return true;
            }

            return false;
        }

        public bool IsExists(string key)
        {
            return !string.IsNullOrEmpty(PlayerPrefs.GetString(GetFullKey(key)));
        }

        private string GetFullKey(string key)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(key);
            _stringBuilder.Append("_");
            _stringBuilder.Append(_profileIndex);

            return _stringBuilder.ToString();
        }
    }
}