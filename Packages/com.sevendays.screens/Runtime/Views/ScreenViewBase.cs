using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace SevenDays.Screens.Views
{
    public abstract class ScreenViewBase : MonoBehaviour
    {
        internal async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            Enable();

            await VisualizeShowAsync(cancellationToken);
        }

        internal async UniTask HideAsync(CancellationToken cancellationToken)
        {
            await VisualizeHideAsync(cancellationToken);

            Disable();
        }

        protected virtual UniTask VisualizeShowAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask VisualizeHideAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        protected virtual void Enable()
        {
            gameObject.SetActive(true);
        }

        protected virtual void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}