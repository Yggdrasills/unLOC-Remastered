using ToolBox.Serialization;

using UnityEditor;

namespace SevenDays.Utils.Editor
{
    public static class CachePreferences
    {
        [MenuItem("Tools/" + nameof(ClearDataSerializer))]
        public static void ClearDataSerializer()
        {
            DataSerializer.DeleteAll();
        }
    }
}