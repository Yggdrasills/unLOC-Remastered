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
        private readonly Transform _root;
        private readonly Dictionary<string, ScreenViewBase> _activeScreens;

        public ScreenService(ScreenCollection screenCollection, Transform parentCanvas)
        {
            _screenCollection = screenCollection;

            _root = parentCanvas;

            _activeScreens = new Dictionary<string, ScreenViewBase>(16);
        }

        public async UniTask ShowAsync(ScreenIdentifier screenIdentifier, CancellationToken cancellationToken)
        {
            await ShowAsync<ScreenViewBase>(screenIdentifier, cancellationToken);
        }

        public async UniTask<T> ShowAsync<T>(ScreenIdentifier screenIdentifier,
            CancellationToken cancellationToken)
            where T : ScreenViewBase
        {
            try
            {
                var screenId = screenIdentifier.Value;

                var screenPrefab = _screenCollection.Get(screenId);

                if (screenPrefab is null)
                {
                    return null;
                }

                if (_activeScreens.ContainsKey(screenId))
                {
                    return null;
                }

                // todo: нужно подумать о пуле
                var screen = Object.Instantiate(screenPrefab, _root);

                _activeScreens[screenId] = screen;

                screen.transform.SetAsLastSibling();

                await screen.ShowAsync(cancellationToken);

                return (T) screen;
            }
            catch
            {
                // ignored
            }

            return null;
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
            }
            catch
            {
                // ignored
            }
            finally
            {
                Object.Destroy(screen.gameObject);
                _activeScreens.Remove(screenId);
            }
        }
    }
}