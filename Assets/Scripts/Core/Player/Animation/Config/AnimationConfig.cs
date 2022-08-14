using System;

using UnityEngine;

namespace SevenDays.unLOC.Core.Player.Animations.Config
{
    [Serializable]
    public class AnimationConfig
    {
        [field: SerializeField]
        public string AnimatorWalkToggle { get; private set; } = "isWalk";

        [field: SerializeField]
        public string[] AnimatorSpecIdleTriggers { get; private set; } = new[] { "spec", "spec2" };

        [field: SerializeField]
        public Vector2 RangeRandomEnabledSpecIdle { get; private set; } = new Vector2(2f, 4f);
    }
}