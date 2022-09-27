using JetBrains.Annotations;

using SevenDays.Localization;
using SevenDays.unLOC.Activities.Quests;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Items.Pad
{
    public class PadView : MonoBehaviour
    {
        [SerializeField]
        private Canvas[] _canvases;

        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private DialogQuest _managerDialogQuest;

        [SerializeField]
        private DialogQuest _lawyerDialogQuest;

        private Camera _mainCamera;

        [Inject, UsedImplicitly]
        private void Construct(LocalizationService localization,
            IStorageRepository storage,
            Camera mainCamera)
        {
            _mainCamera = mainCamera;

            _managerDialogQuest.Setup(localization, storage);
            _lawyerDialogQuest.Setup(localization, storage);
        }

        private void OnValidate()
        {
            if (_canvases == null || _canvases.Length < 1)
            {
                _canvases = GetComponentsInChildren<Canvas>();
            }
        }

        private void Start()
        {
            for (int i = 0; i < _canvases.Length; i++)
            {
                _canvases[i].worldCamera = _mainCamera;
            }

            _managerDialogQuest.Initialize();
            _lawyerDialogQuest.Initialize();
        }

        public void PickUp()
        {
            _content.SetActive(true);
        }
    }
}