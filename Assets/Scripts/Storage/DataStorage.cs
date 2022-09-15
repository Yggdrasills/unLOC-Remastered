﻿using Newtonsoft.Json;

using UnityEngine;

namespace SevenDays.unLOC.Storage
{
    public class DataStorage
    {
        public void Save<T>(string key, T data)
        {
            var json = JsonConvert.SerializeObject(data);

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

            return JsonConvert.DeserializeObject<T>(json);
        }

        public bool TryLoad<T>(string key, out T data)
        {
            data = default;
            var json = PlayerPrefs.GetString(key);

            if (IsExists(key))
            {
                data = JsonConvert.DeserializeObject<T>(json);

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