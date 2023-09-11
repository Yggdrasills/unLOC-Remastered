#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;

using SevenDays.unLOC.Utils.Helpers;

using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SevenDays.unLOC.Utils.Editor
{
    [CustomEditor(typeof(AutoInjectableLifetimeScope), true)]
    public class AutoInjectableLifeTimeScopeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (AutoInjectableLifetimeScope) target;
            
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.green;
            style.fontSize = 18;

            if (GUILayout.Button("Update injectables", style))
            {
                var field = script.GetType().GetField("autoInjectGameObjects",
                    BindingFlags.Instance | BindingFlags.NonPublic);

                var fieldValue = field?.GetValue(script);

                if (!(fieldValue is List<GameObject> injectables))
                    return;

                int countBefore = injectables.Count;

                var method = script.GetType().GetMethod("UpdateInjectables",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                method?.Invoke(script, new object[] { });

                if (countBefore != injectables.Count)
                {
                    var scene = SceneManager.GetActiveScene();
                    EditorSceneManager.MarkSceneDirty(scene);
                }
            }
        }
    }
}

#endif