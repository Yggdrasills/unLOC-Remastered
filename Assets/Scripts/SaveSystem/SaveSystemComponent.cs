using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using ToolBox.Serialization;
using ToolBox.Serialization.OdinSerializer.Utilities;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.SaveSystem
{
    public class SaveSystemComponent : MonoBehaviour
    {
        [SerializeField]
        private List<ISavableMono> _savableMonos;

        private IEnumerable<ISavable> _savables;

        private void OnValidate()
        {
            _savableMonos = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<ISavableMono>().ToList();
        }

        [Inject]
        [UsedImplicitly]
        private void Construct(IEnumerable<ISavable> savables)
        {
            _savables = savables;
        }

        public void SaveData(SaveData saveData)
        {
            CheckSavables();

            DataSerializer.ChangeProfile(saveData.ProfileIndex);
            _savableMonos.ForEach(x => x.Save());
            _savables.ForEach(x => x.Save());
        }

        public void LoadData(SaveData saveData)
        {
            CheckSavables();

            DataSerializer.ChangeProfile(saveData.ProfileIndex);
            _savableMonos.ForEach(x => x.Load());
            _savables.ForEach(x => x.Load());
        }

        private void CheckSavables()
        {
            if (!_savables.Any())
                Debug.Log("There is no ISavables in scene");

            if (!_savableMonos.Any())
                Debug.Log("There is no ISavablesMono in scene");
        }
    }
}