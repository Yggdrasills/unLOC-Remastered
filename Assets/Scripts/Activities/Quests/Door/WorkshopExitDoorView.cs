using System.Linq;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Core.Loaders;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests
{
    public class WorkshopExitDoorView : MonoBehaviour
    {
        [SerializeField]
        private QuestBase[] _quests;

        private SceneLoader _sceneLoader;

        [Inject, UsedImplicitly]
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

        public void LoadStreetStealth()
        {
            _sceneLoader.LoadStreetStealthAsync().Forget();
        }

        public void LoadCredits()
        {
            _sceneLoader.LoadCreditsAsync().Forget();
        }

        private bool CanPassNext()
        {
            return _quests != null && _quests.All(t => t.IsCompleted);
        }
    }
}