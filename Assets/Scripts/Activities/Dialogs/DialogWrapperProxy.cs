using Cysharp.Threading.Tasks;

using SevenDays.InkWrapper.Core;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;

using UnityEngine;

using VContainer;

namespace Activities.Dialogs
{
    public class DialogWrapperProxy : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _dialogAsset;

        [SerializeField]
        private DialogView _dialogViewBehaviour;

        private DialogWrapper _wrapper;
        private IDialogView _dialogView;

        [Inject]
        private void Construct(LocalizationService localization)
        {
            _wrapper = new DialogWrapper(localization);
        }

        private void Awake()
        {
            _dialogView = _dialogViewBehaviour;
        }

        public void StartDialog()
        {
            StartDialogAsync().Forget();
        }

        private async UniTaskVoid StartDialogAsync()
        {
            await _dialogView.ShowAsync();

            var dialog = DialogWrapper.CreateDialog()
                .SetTextAsset(_dialogAsset)
                .OnComplete(() => _dialogView.HideAsync().Forget());

            _wrapper.StartDialogueAsync(dialog, _dialogView).Forget();
        }
    }
}