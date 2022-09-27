using UnityEngine;

namespace SevenDays.Screens.Views
{
    public class DefaultScreenView : ScreenViewBase
    {
        [SerializeField]
        private GameObject _content;

        private void OnValidate()
        {
            if (_content is null && transform.childCount > 0)
            {
                _content = transform.GetChild(0).gameObject;
            }

            Validated();
        }

        protected override void Enable()
        {
            _content.SetActive(true);
        }

        protected override void Disable()
        {
            _content.SetActive(false);
        }

        protected virtual void Validated()
        {
        }
    }
}