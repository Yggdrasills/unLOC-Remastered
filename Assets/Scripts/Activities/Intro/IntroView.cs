using SevenDays.DialogSystem.Runtime;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Intro
{
    public class IntroView : MonoBehaviour
    {
        [field: SerializeField]
        public DialogWindow DialogWindow { get; private set; }
    }
}