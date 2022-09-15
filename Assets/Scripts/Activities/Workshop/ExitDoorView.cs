using System.Linq;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Activities.Items;
using SevenDays.unLOC.Activities.Quests;
using SevenDays.unLOC.Core.Loaders;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Workshop
{
    [RequireComponent(typeof(InteractableItem))]
    public class ExitDoorView : MonoBehaviour
    {
        [SerializeField]
        private InteractableItem _clickableItem;

        [SerializeField]
        private QuestBase[] _quests;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void OnValidate()
        {
            if (_clickableItem == null)
                _clickableItem = GetComponent<InteractableItem>();
        }

        private void OnEnable()
        {
            _clickableItem.Clicked += GoToStreet;
        }

        private void OnDisable()
        {
            _clickableItem.Clicked -= GoToStreet;
        }

        private void GoToStreet()
        {
            if (_quests.Any(t => !t.IsCompleted))
            {
                // todo: dialogue about not all quests completed
                return;
            }

            _sceneLoader.LoadStreetAsync().Forget();
        }
    }
}