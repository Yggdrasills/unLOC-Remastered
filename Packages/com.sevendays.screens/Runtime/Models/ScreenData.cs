using System;

using SevenDays.Screens.Views;

using UnityEngine;

namespace SevenDays.Screens.Models
{
    [Serializable]
    internal class ScreenData
    {
        internal ScreenViewBase Prefab => _screenPrefab;

        internal ScreenIdentifier ScreenIdentifier => _screenIdentifier;

        [SerializeField]
        private ScreenViewBase _screenPrefab;

        [SerializeField]
        private ScreenIdentifier _screenIdentifier;
    }
}