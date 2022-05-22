using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using SaveSystem;

using ToolBox.Serialization;
using ToolBox.Serialization.OdinSerializer.Utilities;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Utils;

using VContainer.Unity;

using Object = UnityEngine.Object;

namespace Menu
{
    public class MenuController : IInitializable
    {
        private readonly MenuView _menuView;
        private SaveSystemComponent _saveComponent;

        private Dictionary<int, SaveData> _savesList;
        private List<LoadProfileButton> _profileButtons;
        private bool _isShoved;

        public MenuController(MenuView menuView)
        {
            _menuView = menuView;
            _savesList = GetSavesList();
            _profileButtons = new List<LoadProfileButton>();
        }

        void IInitializable.Initialize()
        {
            RedrawProfilePanel();

            _isShoved = true;

            _menuView.ContinueGameButton.onClick.AddListener(HandleContinueGameClicked);
            _menuView.SaveGameButton.onClick.AddListener(HandleSaveClicked);
            _menuView.NewGameButton.onClick.AddListener(HandleNewGameClicked);
            _menuView.ExitGameButton.onClick.AddListener(HandleExitClicked);

            _menuView.MenuButtonPressed += HandleMenuButtonPressed;
        }


        private void HandleContinueGameClicked()
        {
            var autoSaveData = _savesList[0];
            LoadSceneAsync(autoSaveData.SceneName).Forget();
        }

        private void HandleNewGameClicked()
        {
            var autoSaveData = new SaveData {ProfileIndex = 0, SceneName = Constants.IntroScene};
            AddToSaveList(autoSaveData);
            RedrawProfilePanel();
            LoadSceneAsync(Constants.IntroScene).Forget();
        }

        private void HandleSaveClicked()
        {
            _saveComponent = GetSaveComponent();

            var newProfileIndex = _savesList.Count + 1;

            var saveData = new SaveData
                {ProfileIndex = newProfileIndex, SceneName = SceneManager.GetActiveScene().name};

            AddToSaveList(saveData);
            RedrawProfilePanel();

            _saveComponent.SaveData(saveData);
        }

        private async UniTaskVoid HandleLoadClickedAsync(SaveData saveData)
        {
            await LoadSceneAsync(saveData.SceneName);

            _saveComponent = GetSaveComponent();
            _saveComponent.LoadData(saveData);
        }

        private void HandleExitClicked()
        {
            _saveComponent = GetSaveComponent();
            _saveComponent.AutoSaveData();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private async UniTask LoadSceneAsync(string sceneName)
        {
            //todo: запустить загрузочный экран

            var activeScene = SceneManager.GetActiveScene();

            if (activeScene.name != Constants.MenuScene)
                await SceneManager.UnloadSceneAsync(activeScene);

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            _menuView.Hide();
        }

        private void HandleMenuButtonPressed()
        {
            if (SceneManager.GetActiveScene().name.Equals(Constants.MenuScene))
                return;

            if (_isShoved)
            {
                _menuView.Hide();
                _isShoved = false;
            }
            else
            {
                _menuView.Show();
                _isShoved = true;
            }
        }

        private SaveSystemComponent GetSaveComponent()
        {
            if (_saveComponent is null)
                return _saveComponent = Object.FindObjectOfType<SaveSystemComponent>();

            return _saveComponent;
        }

        private void RedrawProfilePanel()
        {
            _profileButtons.ForEach(Object.Destroy);

            _savesList.ForEach(x =>
            {
                var profileButton = _menuView.CreateProfileButton(x.Value);
                profileButton.Subscribe(() => HandleLoadClickedAsync(x.Value).Forget());
                _profileButtons.Add(profileButton);
            });
        }

        private void AddToSaveList(SaveData saveData)
        {
            if (_savesList.ContainsKey(saveData.ProfileIndex))
                _savesList.Remove(saveData.ProfileIndex);
            
            _savesList.Add(saveData.ProfileIndex, saveData);
            DataSerializer.Save(Constants.SavesList, _savesList);
        }

        private Dictionary<int, SaveData> GetSavesList()
        {
            return DataSerializer.TryLoad(Constants.SavesList, out Dictionary<int, SaveData> saves)
                ? saves
                : new Dictionary<int, SaveData>();
        }
    }
}