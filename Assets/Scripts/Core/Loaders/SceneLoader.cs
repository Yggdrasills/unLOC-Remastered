using Cysharp.Threading.Tasks;

using UnityEngine.SceneManagement;

namespace SevenDays.unLOC.Core.Loaders
{
    public class SceneLoader
    {
        private int _activeSceneIndex;

        public async UniTask LoadMenuAsync()
        {
            await LoadSceneAsync(1);
        }

        public async UniTask LoadIntroAsync()
        {
            await LoadSceneAsync(2);
        }

        public async UniTask LoadWorkshopAsync()
        {
            await LoadSceneAsync(3);
        }

        public async UniTask LoadStreetAsync()
        {
            await LoadSceneAsync(4);
        }

        public async UniTask LoadStreetStealthAsync()
        {
            await LoadSceneAsync(5);
        }

        public async UniTask LoadMelissaRoomAsync()
        {
            await LoadSceneAsync(6);
        }

        private async UniTask LoadSceneAsync(int buildIndex)
        {
            var activeScene = SceneManager.GetActiveScene();

            await SceneManager.UnloadSceneAsync(activeScene);

            var sceneLoadOperation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

            await UniTask.WaitUntil(() => sceneLoadOperation.isDone);

            var loadedScene = SceneManager.GetSceneByBuildIndex(buildIndex);

            SceneManager.SetActiveScene(loadedScene);

            _activeSceneIndex = buildIndex;
        }
    }
}