using System;

using Activities.Dialogs;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Core.Player;
using SevenDays.unLOC.Storage;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Items
{
    [RequireComponent(typeof(BoxCollider2D), typeof(DialogWrapperProxy))]
    public class OnceActivatedDialogAreaView : MonoBehaviour
    {
        [SerializeField]
        private DialogWrapperProxy _dialogProxy;

        private IStorageRepository _storage;

        [Inject, UsedImplicitly]
        private void Construct(IStorageRepository storage)
        {
            _storage = storage;
        }

        private void Start()
        {
            if (_storage.IsExists(GetType().FullName))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerTag _))
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
            // todo: stop player movement
            _dialogProxy.StartDialog();

            // note: visual delay
            await UniTask.Delay(TimeSpan.FromSeconds(3));

            await _dialogProxy.HideDialogAsync();
            // todo: start player movement
        }
    }
}