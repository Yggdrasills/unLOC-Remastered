using System;

using Cysharp.Threading.Tasks;

using UnityEngine.EventSystems;

namespace SevenDays.InkWrapper.Views.Dialogs
{
    public interface IDialogView : IPointerClickHandler
    {
        public event Action Clicked;

        public bool IsRevealing { get; }

        public UniTaskVoid RevealAsync(string text);

        public void Reveal();

        public UniTask ShowAsync();

        public UniTask HideAsync();
    }
}