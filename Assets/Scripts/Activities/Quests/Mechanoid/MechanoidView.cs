using Cysharp.Threading.Tasks;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class MechanoidView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private Sprite[] _explosionAnimationFrames;

        public async UniTask PlayExplosion()
        {
            for (int i = 0; i < _explosionAnimationFrames.Length; i++)
            {
                _renderer.sprite = _explosionAnimationFrames[i];

                await UniTask.WaitForEndOfFrame(this);
            }
        }
    }
}