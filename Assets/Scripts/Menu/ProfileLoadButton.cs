using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SevenDays.unLOC.Menu
{
    public class ProfileLoadButton : MonoBehaviour
    {
        public Button Button => _button;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI;

        public void SetButtonText(string saveName, DateTime creationDate, DateTime lastActivity)
        {
            _textMeshProUGUI.text = $"Save: {saveName}\n" +
                                    $"Creation Date: {creationDate:dddd, dd MMMM yyyy}\n" +
                                    $"Last activity: {lastActivity:dddd, dd MMMM yyyy}\n";
        }
    }
}