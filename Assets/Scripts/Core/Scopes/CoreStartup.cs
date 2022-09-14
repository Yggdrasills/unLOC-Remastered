using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Core.Loaders;

using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class CoreStartup : IAsyncStartable
    {
        private readonly SceneLoader _sceneLoader;

        public CoreStartup(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
            await _sceneLoader.LoadMenuAsync();
        }
    }
}