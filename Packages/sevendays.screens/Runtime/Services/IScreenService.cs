using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.Screens.Models;

namespace SevenDays.Screens.Services
{
    public interface IScreenService
    {
        UniTask ShowAsync(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken);

        UniTask HideAsync(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken);
    }
}