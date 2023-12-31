﻿using System;

using Activities.Dialogs;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

using SevenDays.unLOC.Storage;

using TMPro;

using UnityEngine;

using VContainer;

namespace SevenDays.unLOC.Activities.Quests.RobotPainter
{
    public class RobotPainterQuest : QuestBase
    {
        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private RobotPainterButtonView[] _buttons;

        [SerializeField]
        private RobotPainterView _robotView;

        [SerializeField]
        private TextColorBlinker _textBlinker;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Color[] _correctColors;

        [SerializeField]
        private Color[] _wrongColors;

        [SerializeField]
        private int _maxEnteredLenght = 5;

        [SerializeField]
        private int _correctPassword = 73326;

        [SerializeField]
        private DialogWrapperProxy _dialogWrapper;

        private IStorageRepository _storage;

        private bool _canEnterPassword = true;

        [Inject, UsedImplicitly]
        private void Construct(IStorageRepository storage)
        {
            _storage = storage;
        }

        private void OnValidate()
        {
            if (_content == null)
            {
                if (transform.childCount > 0)
                {
                    _content = transform.GetChild(0).gameObject;
                }
            }

            if (_buttons == null)
            {
                _buttons = GetComponentsInChildren<RobotPainterButtonView>();
            }

            if (_text == null)
            {
                _text = GetComponentInChildren<TextMeshProUGUI>();
            }

            if (_textBlinker == null)
            {
                _textBlinker = GetComponentInChildren<TextColorBlinker>();
            }

            if (_robotView == null)
            {
                _robotView = GetComponentInChildren<RobotPainterView>();
            }
        }

        private void Start()
        {
            if (_storage.IsExists(typeof(RobotPainterQuest).FullName))
            {
                Complete();
            }
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

        public void Enable()
        {
            _content.SetActive(true);
        }

        public void Disable()
        {
            _content.SetActive(false);
        }

        private void EnterNumber(string num)
        {
            if (!_canEnterPassword) return;

            _text.text += num;

            TryCompleteQuestAsync().Forget();
        }

        private async UniTaskVoid TryCompleteQuestAsync()
        {
            if (_text.text.Length != _maxEnteredLenght) return;

            if (IsCorrectPassword(int.Parse(_text.text)))
            {
                await _textBlinker.BlinkAsync(_correctColors);

                Complete();

                _dialogWrapper.StartDialog();

                _storage.Save(typeof(RobotPainterQuest).FullName, true);

                await UniTask.Delay(TimeSpan.FromSeconds(3));

                _dialogWrapper.HideDialogAsync().Forget();
            }

            _canEnterPassword = false;

            await _textBlinker.BlinkAsync(_wrongColors);

            ResetToDefault();
        }

        private void Complete()
        {
            CompleteQuest();

            _robotView.Play();

            gameObject.SetActive(false);
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