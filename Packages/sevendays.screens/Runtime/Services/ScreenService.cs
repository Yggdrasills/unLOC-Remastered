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
        private readonly Dictionary<ScreenIdentifier, ScreenViewBase> _activeScreens;

        public ScreenService(ScreenCollection screenCollection)
        {
            _screenCollection = screenCollection;

            _activeScreens = new Dictionary<ScreenIdentifier, ScreenViewBase>(16);
        }

        public async UniTask ShowAsync(ScreenIdentifier identifier, CancellationToken cancellationToken)
        {
            try
            {
                var screenPrefab = _screenCollection.Get(identifier);

                if (screenPrefab is null)
                {
                    return;
                }

                if (_activeScreens.ContainsKey(identifier))
                {
                    return;
                }

                var screen = Object.Instantiate(screenPrefab);

                _activeScreens[identifier] = screen;
                
                screen.transform.SetAsLastSibling();

                await screen.ShowAsync(cancellationToken);
            }
            catch
            {
                // ignored
            }
        }

        public async UniTask HideAsync(ScreenIdentifier identifier, CancellationToken cancellationToken)
        {
            if (!_activeScreens.TryGetValue(identifier, out ScreenViewBase screen))
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
                _activeScreens.Remove(identifier);
            }
        }
    }
}