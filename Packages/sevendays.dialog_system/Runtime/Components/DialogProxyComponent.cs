using JetBrains.Annotations;

using SevenDays.DialogSystem.Runtime;

using UnityEngine;

using VContainer;

namespace SevenDays.DialogSystem.Components
{
    public class DialogProxyComponent : MonoBehaviour
    {
        [SerializeField]
        private DialogWindow _dialogWindow;

        [SerializeField]
        private TextAsset _dialogJson;

        private DialogService _dialogService;

        [Inject, UsedImplicitly]
        private void Construct(DialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public void StartDialog()
        {
            _dialogWindow.DialogJson = _dialogJson;
            _dialogService.StartDialog(_dialogWindow);
        }
    }
}