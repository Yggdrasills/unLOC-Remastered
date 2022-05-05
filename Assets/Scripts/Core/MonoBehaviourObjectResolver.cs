using System;

using SevenDays.DialogSystem.Components;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core
{
    public class MonoBehaviourObjectResolver : MonoBehaviour, IInitializable
    {
        [SerializeField] private InjectableMonoBehaviour[] _injectableMonoBehaviours;
        private IObjectResolver _resolver;

        private void OnValidate()
        {
            _injectableMonoBehaviours = FindObjectsOfType<InjectableMonoBehaviour>();
        }

        [Inject]
        private void Constructor(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public void Initialize()
        {
            foreach (var behaviour in _injectableMonoBehaviours)
            {
                _resolver.Inject(behaviour);
            }
        }
    }
}