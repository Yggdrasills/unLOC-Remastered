using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

namespace SevenDays.unLOC.Utils.Extensions
{
    public static class TweenExtensions
    {
        public static UniTask AwaitWithCancellation(this Tween tween, CancellationToken cancellationToken)
        {
            var registration = cancellationToken.Register(() =>
            {
                if (tween.IsActive())
                {
                    tween.Kill();
                }
            });

            tween.OnKill(() => { registration.Dispose(); });

            return tween.ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, cancellationToken);
        }
    }
}