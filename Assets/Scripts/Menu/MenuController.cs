using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;

using SevenDays.SaveSystem;
using SevenDays.Utils.Constants;

using ToolBox.Serialization;
using ToolBox.Serialization.OdinSerializer.Utilities;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

using VContainer.Unity;

namespace SevenDays.Menu
{
    public class MenuController : IInitializable
    {
        private readonly MenuView _menuView;
        private readonly LoadingPanelView _loadingPanelView;
        private SaveSystemComponent _saveComponent;

        private Dictionary<int, SaveData> _savesList;
        private List<LoadProfileButton> _profileButtons;
        private bool _isShoved;

        public MenuController(
            MenuView menuView, 
            LoadingPanelView loadingPanelView
            )
        {
            _menuView = menuView;
            _loadingPanelView = loadingPanelView;
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
            
            _menuView.SaveGameButton.gameObject.SetActive(false);
            
            if(_savesList.Count == 0)
                _menuView.LoadGameButton.gameObject.SetActive(false);
        }


        private void HandleContinueGameClicked()
        {
            var autoSaveData = _savesList[0];
            LoadSceneAsync(autoSaveData.SceneName).Forget();
        }

        private void HandleNewGameClicked()
        {
            var autoSaveData = new SaveData {ProfileIndex = 0, SceneName = GameConstants.IntroScene};
            AddToSaveList(autoSaveData);
            RedrawProfilePanel();
            LoadSceneAsync(GameConstants.IntroScene).Forget();
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
           await _loadingPanelView.ShowAsync(1, CancellationToken.None);

            var activeScene = SceneManager.GetActiveScene();

            if (activeScene.name != GameConstants.MenuScene)
                await SceneManager.UnloadSceneAsync(activeScene);

            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            _menuView.Hide();
            _loadingPanelView.HideAsync(0.3f, CancellationToken.None).Forget();
        }

        private void HandleMenuButtonPressed()
        {
            if (SceneManager.GetActiveScene().name.Equals(GameConstants.MenuScene))
                return;

            _menuView.NewGameButton.gameObject.SetActive(false);
            _menuView.SaveGameButton.gameObject.SetActive(true);
            if(_savesList.Count > 0)
                _menuView.LoadGameButton.gameObject.SetActive(true);
            
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
            DataSerializer.Save(GameConstants.SavesList, _savesList);
        }

        private Dictionary<int, SaveData> GetSavesList()
        {
            return DataSerializer.TryLoad(GameConstants.SavesList, out Dictionary<int, SaveData> saves)
                ? saves
                : new Dictionary<int, SaveData>();
        }
    }
}