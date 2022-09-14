using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using SevenDays.Screens.Views;

using UnityEngine;

namespace SevenDays.Screens.Models
{
    [CreateAssetMenu(
        fileName = "new " + nameof(ScreenCollection),
        menuName = "SevenDays/unLOC/" + nameof(ScreenCollection))]
    public class ScreenCollection : ScriptableObject
    {
        [SerializeField]
        private ScreenData[] _collection;

        private void OnValidate()
        {
            foreach (var target in _collection.Reverse())
            {
                var existingGuids = _collection
                    .Select(t => t.Identifier)
                    .Select(t => t.Value);

                if (string.IsNullOrEmpty(target.Identifier.Value) || existingGuids.Count(t => target.Identifier.Value == t) > 1)
                {
                    var screenIdentifierFieldValue = target.GetType().GetField("_screenIdentifier",
                        BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(target);

                    var identifierField = screenIdentifierFieldValue?.GetType().GetField("_value",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                    identifierField?.SetValue(screenIdentifierFieldValue, Guid.NewGuid().ToString());
                }
            }
        }

        [CanBeNull]
        internal ScreenViewBase Get(ScreenIdentifier screenIdentifier)
        {
            return _collection.FirstOrDefault(t => t.Identifier == screenIdentifier)?.Prefab;
        }
    }
}