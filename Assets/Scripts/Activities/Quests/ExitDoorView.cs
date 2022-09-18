using System.Linq;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Core.Loaders;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests
{
    [RequireComponent(typeof(InteractableItem))]
    public class ExitDoorView : MonoBehaviour
    {
        [SerializeField]
        private QuestBase[] _quests;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void LoadStreet()
        {
            if (!CanPassNext())
            {
                // todo: dialogue about not all quests completed
                return;
            }

            _sceneLoader.LoadStreetAsync().Forget();
        }

        public void LoadWorkshop()
        {
            if (!CanPassNext())
            {
                // todo: dialogue about not all quests completed
                return;
            }

            _sceneLoader.LoadWorkshopAsync().Forget();
        }

        private bool CanPassNext()
        {
            return _quests != null && _quests.All(t => t.IsCompleted);
        }
    }
}