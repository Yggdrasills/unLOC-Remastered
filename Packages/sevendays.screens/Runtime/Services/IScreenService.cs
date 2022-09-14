using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.Screens.Models;

namespace SevenDays.Screens.Services
{
    public interface IScreenService
    {
        UniTask ShowAsync(ScreenIdentifier screen, CancellationToken cancellationToken);

        UniTask HideAsync(ScreenIdentifier screen, CancellationToken cancellationToken);
    }
}