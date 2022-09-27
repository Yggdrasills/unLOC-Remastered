using UnityEngine;

namespace SevenDays.InkWrapper.Views.Choices
{
    public class DefaultAppearanceStrategy : AppearanceStrategy
    {
        [SerializeField]
        private GameObject _content;

        private void OnValidate()
        {
            if (_content == null)
            {
                if (transform.childCount > 0)
                {
                    _content = transform.GetChild(0).gameObject;
                }
                else
                {
                    _content = gameObject;
                }
            }
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}