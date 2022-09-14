#if UNITY_EDITOR
using System.Linq;

using SevenDays.Screens.Models;

using UnityEditor;

using UnityEngine;

namespace SevenDays.Screens.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(ScreenIdentifier), true)]
    public class IdentifierPropertyDrawer : PropertyDrawer
    {
        private ScreenCollection _screenCollection;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.targetObject.GetType() == typeof(ScreenCollection))
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            var identifierProperty = property.FindPropertyRelative("_value");

            _screenCollection ??= Resources.Load<ScreenCollection>("Screens/" + nameof(ScreenCollection));

            var collection = _screenCollection.Collection.ToList();

            var names = collection
                .Select(screenData => screenData.Prefab)
                .Select(prefab => prefab.name);

            var currentScreen = collection
                .SingleOrDefault(t => t.ScreenIdentifier.Value == identifierProperty.stringValue);

            var currentIndex = currentScreen is null
                ? 0
                : collection.IndexOf(currentScreen);

            EditorGUI.BeginChangeCheck();

            var classRect = new Rect(position.x, position.y, position.width, position.height);

            var index = EditorGUI.Popup(classRect, currentIndex, names.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                var ids = collection
                    .Select(screenData => screenData.ScreenIdentifier)
                    .Select(prefab => prefab.Value);

                identifierProperty.stringValue = ids.ElementAt(index);
            }

            var newPosition = position;
            newPosition.y += 10f;

            var style = new GUIStyle(GUI.skin.label)
            {
                normal =
                {
                    textColor = Color.cyan
                },
                fontSize = 11,
                fontStyle = FontStyle.Bold
            };

            EditorGUI.LabelField(newPosition, $"Guid: {identifierProperty.stringValue}", style);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif