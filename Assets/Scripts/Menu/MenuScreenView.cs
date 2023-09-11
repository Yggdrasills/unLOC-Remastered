using SevenDays.Screens.Views;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Menu
{
    public class MenuScreenView : DefaultScreenView
    {
        public Button NewGameButton => _newGameButton;

        public Button LoadGameButton => _loadGameButton;

        public Button SaveGameButton => _saveGameButton;

        public Button ExitGameButton => _exitGameButton;

        public Transform ProfileContainer => _profileContainer;

        [SerializeField]
        private Button _newGameButton;

        [SerializeField]
        private Button _loadGameButton;

        [SerializeField]
        private Button _saveGameButton;

        [SerializeField]
        private Button _exitGameButton;

        [SerializeField]
        private Transform _profileContainer;
    }
}