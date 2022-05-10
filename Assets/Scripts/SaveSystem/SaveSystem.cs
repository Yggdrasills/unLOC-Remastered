using System.Collections.Generic;
using System.Linq;

using ToolBox.Serialization;

using UnityEngine;

namespace SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        private List<ISavableMono> _savableMonos;

        private void Awake()
        {
            _savableMonos = new List<ISavableMono>();

            var savableBehaviours = FindObjectsOfType<MonoBehaviour>(true).OfType<ISavableMono>();
            _savableMonos.AddRange(savableBehaviours);
        }

        public void SaveData(int profileIndex)
        {
            DataSerializer.ChangeProfile(profileIndex);
            _savableMonos.ForEach(x => x.Save());
        }

        public void LoadData(int profileIndex)
        {
            DataSerializer.ChangeProfile(profileIndex);
            _savableMonos.ForEach(x => x.Load());
        }

        public void ClearAll()
        {
            DataSerializer.DeleteAll();
        }
    }
}