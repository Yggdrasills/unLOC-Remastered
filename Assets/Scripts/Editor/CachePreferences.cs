using System.IO;

using UnityEditor;

using UnityEngine;

namespace SevenDays.Utils.Editor
{
    public static class CachePreferences
    {
        [MenuItem("Tools/" + nameof(ClearCache))]
        public static void ClearCache()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath, "*.data");

            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }

            PlayerPrefs.DeleteAll();
        }
    }
}