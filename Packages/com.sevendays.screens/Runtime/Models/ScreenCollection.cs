using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using SevenDays.Screens.Views;

using UnityEngine;

namespace SevenDays.Screens.Models
{
    [CreateAssetMenu(
        fileName = "new " + nameof(ScreenCollection),
        menuName = "SevenDays/Screens/" + nameof(ScreenCollection))]
    public class ScreenCollection : ScriptableObject
    {
        internal ScreenData[] Collection => _collection;

        [SerializeField]
        private ScreenData[] _collection;

        private void OnValidate()
        {
            foreach (var target in _collection.Reverse())
            {
                var existingGuids = GetIdentifiers();

                if (string.IsNullOrEmpty(target.ScreenIdentifier.Value) ||
                    existingGuids.Count(t => target.ScreenIdentifier.Value == t) > 1)
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
        internal ScreenViewBase Get(string identifier)
        {
            return _collection.FirstOrDefault(t => t.ScreenIdentifier.Value == identifier)?.Prefab;
        }

        private IEnumerable<string> GetIdentifiers()
        {
            return _collection
                .Select(t => t.ScreenIdentifier)
                .Select(t => t.Value);
        }
    }
}