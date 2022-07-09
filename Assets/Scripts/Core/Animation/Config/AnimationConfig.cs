using UnityEngine;

namespace SevenDays.unLOC.Core.Animations.Config
{
    [CreateAssetMenu(menuName = "Configs/" + nameof(AnimationConfig), order = 0)]
    public class AnimationConfig : ScriptableObject
    {
        [field: SerializeField]
        public string AnimatorWalkToggle { get; private set; } = "isWalk";

        [field: SerializeField]
        public string[] AnimatorSpecIdleTriggers { get; private set; } = new[] { "spec", "spec2" };

        [field: SerializeField]
        public Vector2 RangeRandomEnabledSpecIdle { get; private set; } = new Vector2(2f, 4f);
    }
}