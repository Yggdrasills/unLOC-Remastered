using UnityEngine;

namespace Activities.Intro
{
    [CreateAssetMenu(menuName = "Configs/IntroConfig", order = 0)]
    public class IntroConfig : ScriptableObject
    {
        [field: SerializeField] public IntroView IntroView { get; private set; }
    }
}