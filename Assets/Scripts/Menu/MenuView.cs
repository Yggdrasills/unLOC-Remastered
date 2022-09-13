using System;

using SevenDays.SaveSystem;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;

        [Header("Buttons")] 
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button _continueGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _saveGameButton;
        [SerializeField] private Button _loadGameButton;

        [Header("Load Panel")] 
        [SerializeField] private Transform _profilesContainer;
        [SerializeField] private LoadProfileButton _profileButtonPrefab;
        
        public Button NewGameButton => newGameButton;
        public Button ExitGameButton => _exitGameButton;
        public Button SaveGameButton => _saveGameButton;
        public Button ContinueGameButton => _continueGameButton;
        public Button LoadGameButton => _loadGameButton;

        public event Action MenuButtonPressed = delegate {  };

        public LoadProfileButton CreateProfileButton(SaveData saveData)
        {
            var button = Instantiate(_profileButtonPrefab, _profilesContainer);
            button.Initialize(saveData);
            return button;
        }

        public void Show()
        {
            _root.SetActive(true);
        }

        public void Hide()
        {
            _root.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                MenuButtonPressed.Invoke();
        }
    }
}