using SevenDays.InkWrapper.Core;
using SevenDays.InkWrapper.Views.Dialogs;
using SevenDays.Localization;

using UnityEngine;

namespace SevenDays.InkWrapper.Example
{
    public class InkExample : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _dialogAsset;

        [SerializeField]
        private DialogView _dialogViewBase;

        private void Start()
        {
            var localizationService = new LocalizationService();
            var wrapper = new DialogWrapper(localizationService);

            var dialog = DialogWrapper.CreateDialog()
                .SetTextAsset(_dialogAsset)
                .SetAction("key", () => { Debug.Log("action"); })
                .WithGlobalParameter("key", null)
                .WithGlobalObserver("key", null)
                .OnComplete(() => { Debug.Log("end"); });

            wrapper.StartDialogueAsync(dialog, _dialogViewBase).Forget();
        }
    }
}