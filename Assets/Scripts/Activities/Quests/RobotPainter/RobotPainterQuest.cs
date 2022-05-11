using Cysharp.Threading.Tasks;

using TMPro;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.RobotPainter
{
    public class RobotPainterQuest : QuestBase
    {
        [SerializeField]
        private RobotPainterButtonView[] _buttons;

        [SerializeField]
        private RobotPainterView _robotView;

        [SerializeField]
        private TextColorBlinker _textBlinker;

        [SerializeField]
        private TextMeshPro _text;

        [SerializeField]
        private Color[] _correctColors;

        [SerializeField]
        private Color[] _wrongColors;

        [SerializeField]
        private int _maxEnteredLenght = 5;

        [SerializeField]
        private int _correctPassword = 73326;

        // todo: add dialogue after quest complete
        [SerializeField]
        private string _questDoneDialogueBubbleText = "Так-то лучше. Теперь тебя не взломают.";

        private bool _canEnterPassword = true;

        private void OnValidate()
        {
            _buttons ??= GetComponentsInChildren<RobotPainterButtonView>();

            _text ??= GetComponentInChildren<TextMeshPro>();

            _textBlinker ??= GetComponentInChildren<TextColorBlinker>();

            _robotView ??= FindObjectOfType<RobotPainterView>();
        }

        private void OnEnable()
        {
            ResetToDefault();

            for (int i = 0; i < _buttons.Length; i++)
            {
                var closure = i;

                _buttons[i].Clicked += () => EnterNumber(_buttons[closure].Code);
            }
        }

        private void EnterNumber(string num)
        {
            if (!_canEnterPassword) return;

            _text.text += num;

            TryCompleteQuest().Forget();
        }

        private async UniTaskVoid TryCompleteQuest()
        {
            if (_text.text.Length != _maxEnteredLenght) return;

            if (IsCorrectPassword(int.Parse(_text.text)))
            {
                await _textBlinker.Blink(_correctColors);

                CompleteQuest();

                _robotView.Play();

                gameObject.SetActive(false);
            }

            _canEnterPassword = false;

            await _textBlinker.Blink(_wrongColors);

            ResetToDefault();
        }

        private void ResetToDefault()
        {
            _canEnterPassword = true;
            _text.text = string.Empty;
        }

        private bool IsCorrectPassword(int password)
        {
            return password.Equals(_correctPassword);
        }
    }
}