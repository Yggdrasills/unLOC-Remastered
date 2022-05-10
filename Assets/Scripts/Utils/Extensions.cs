using ToolBox.Serialization;

using UnityEditor;

namespace Utils
{
    public static class Extensions
    {
        [MenuItem("Tools/ClearAllCache")]
        public static void ClearAllCache()
        {
            DataSerializer.DeleteAll();
        }
    }
}