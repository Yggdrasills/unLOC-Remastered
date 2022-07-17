using System;

using UnityEngine;

namespace SevenDays.unLOC.Core.Player.Animations
{
    /// <summary>
    /// Class handle animation event
    /// </summary>
    public class AnimationCallBackView : MonoBehaviour
    {
        public event Action IdleStart = delegate { };
        public event Action IdleSpecFirstStart = delegate { };
        public event Action IdleSpecSecondStart = delegate { };
        public event Action WalkStart = delegate { };
        
        public void OnIdleStart() => IdleStart.Invoke();
        public void OnSpecFirstStart() => IdleSpecFirstStart.Invoke();
        public void OnSpecSecondStart() => IdleSpecSecondStart.Invoke();
        public void OnWalkStart() => WalkStart.Invoke();
    }
}