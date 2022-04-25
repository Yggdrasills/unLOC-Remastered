using UnityEngine;

using VContainer;

namespace DialogSystem.Runtime
{
    public class DialogSystemDemo : MonoBehaviour
    {
        [SerializeField] private DialogWindow _dialogWindow;

        private DialogService _dialogService;
        
        [Inject]
        private void Construct(DialogService dialogService)
        {
            _dialogService = dialogService;
        }
        
        public void StartDialog()
        {
            _dialogService.StartDialog(_dialogWindow);
        }
    }
}