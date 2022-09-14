using System;

using SevenDays.unLOC.SaveSystem;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Menu
{
    public class LoadProfileButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        public void Initialize(SaveData saveData)
        {
            var profileIndexText = saveData.ProfileIndex == 0 ? "Auto save" : saveData.ProfileIndex.ToString();
            var text = $"Save: {profileIndexText} \n Scene: {saveData.SceneName}";
            _textMeshProUGUI.text = text;
        }
        
        public void Subscribe(Action action)
        {
            _button.onClick.AddListener(action.Invoke);
        }
    }
}