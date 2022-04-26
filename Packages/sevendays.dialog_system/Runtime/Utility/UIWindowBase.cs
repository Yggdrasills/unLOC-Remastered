using UnityEngine;

namespace SevenDays.unLOC.Core
{
    public abstract class UIWindowBase : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}