using System.Linq;
using System.Reflection;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Utils.Helpers
{
    public class AutoInjectableLifetimeScope : LifetimeScope
    {
        private void OnValidate()
        {
            UpdateInjectables();
        }

        /// Used by reflection
        /// <see cref="T:SevenDays.unLOC.Core.CustomEditors.AutoInjectableLifeTimeScopeEditor"/>
        private void UpdateInjectables()
        {
            autoInjectGameObjects.Clear();

            var sceneObjects = FindObjectsOfType<Transform>(true);

            foreach (var sceneObject in sceneObjects)
            {
                if (sceneObject.gameObject.scene != gameObject.scene)
                    continue;

                var behaviours = sceneObject.GetComponents<MonoBehaviour>();

                var methods = behaviours.SelectMany(beh =>
                    beh.GetType().GetRuntimeMethods());

                var hasInjectAttribute = methods
                    .Any(mInfo => mInfo.GetCustomAttributes(typeof(InjectAttribute), true).Any());

                if (!hasInjectAttribute)
                    continue;

                autoInjectGameObjects.Add(sceneObject.gameObject);
            }
        }
    }
}