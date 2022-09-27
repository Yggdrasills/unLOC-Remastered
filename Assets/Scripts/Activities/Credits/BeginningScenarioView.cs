using System;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using JetBrains.Annotations;

using SevenDays.unLOC.Storage;

using TMPro;

using UnityEngine;

using VContainer;

namespace Activities.Credits
{
    public class BeginningScenarioView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private IStorageRepository _storage;

        [Inject, UsedImplicitly]
        private void Construct(IStorageRepository storage)
        {
            _storage = storage;
        }

        private void Start()
        {
            _text.DOFade(0, 0);

            if (_storage.IsExists(GetType().FullName))
            {
                gameObject.SetActive(false);
            }
            else
            {
                RunAsync().ContinueWith(() =>
                {
                    gameObject.SetActive(false);
                    _storage.Save(GetType().FullName, true);
                }).Forget();
            }
        }

        private async UniTask RunAsync()
        {
            await _text.DOFade(1, 3).SetEase(Ease.InQuad);

            await UniTask.Delay(TimeSpan.FromSeconds(3));
        }
    }
}