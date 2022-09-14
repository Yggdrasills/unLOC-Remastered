#if UNITY_EDITOR
using SevenDays.Screens.Attributes;

using UnityEditor;

using UnityEngine;

namespace SevenDays.Screens.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(LabelAttribute), true)]
    internal sealed class LabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var labelAttribute = attribute as LabelAttribute;

            EditorGUI.LabelField(position, labelAttribute?.Prefix + property.stringValue);

            EditorGUI.EndProperty();
        }
    }
}
#endif