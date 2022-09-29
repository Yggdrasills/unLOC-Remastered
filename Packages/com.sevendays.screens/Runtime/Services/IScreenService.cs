using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.Screens.Models;
using SevenDays.Screens.Views;

namespace SevenDays.Screens.Services
{
    public interface IScreenService
    {
        UniTask<T> ShowAsync<T>(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken)
            where T : ScreenViewBase;

        UniTask ShowAsync(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken);

        UniTask HideAsync(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken);
    }
}