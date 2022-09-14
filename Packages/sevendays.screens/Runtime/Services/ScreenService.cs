using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.Screens.Models;
using SevenDays.Screens.Views;

using UnityEngine;

namespace SevenDays.Screens.Services
{
    public class ScreenService : IScreenService
    {
        private readonly ScreenCollection _screenCollection;
        private readonly Dictionary<string, ScreenViewBase> _activeScreens;

        public ScreenService(ScreenCollection screenCollection)
        {
            _screenCollection = screenCollection;

            _activeScreens = new Dictionary<string, ScreenViewBase>(16);
        }

        public async UniTask ShowAsync(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken)
        {
            try
            {
                var screenId = screenIdentifier.Value;

                var screenPrefab = _screenCollection.Get(screenId);

                if (screenPrefab is null)
                {
                    return;
                }

                if (_activeScreens.ContainsKey(screenId))
                {
                    return;
                }

                // todo: нужно подумать о пуле
                var screen = Object.Instantiate(screenPrefab);

                _activeScreens[screenId] = screen;

                screen.transform.SetAsLastSibling();

                await screen.ShowAsync(cancellationToken);
            }
            catch
            {
                // ignored
            }
        }

        public async UniTask HideAsync(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken)
        {
            var screenId = screenIdentifier.Value;

            if (!_activeScreens.TryGetValue(screenId, out var screen))
            {
                return;
            }

            try
            {
                await screen.HideAsync(cancellationToken);
                
                Object.Destroy(screen.gameObject);
            }
            catch
            {
                // ignored
            }
            finally
            {
                _activeScreens.Remove(screenId);
            }
        }
    }
}