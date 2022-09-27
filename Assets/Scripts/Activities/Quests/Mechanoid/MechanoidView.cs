using Cysharp.Threading.Tasks;

using SevenDays.unLOC.Activities.Quests.Manager;

using UnityEngine;

namespace SevenDays.unLOC.Activities.Quests.Mechanoid
{
    public class MechanoidView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private Sprite[] _explosionAnimationFrames;

        private ManagerDialogQuest _managerDialogQuest;

        private void Start()
        {
            _managerDialogQuest = FindObjectOfType<ManagerDialogQuest>(true);

            if (_managerDialogQuest.IsCompleted)
            {
                Disable();
            }
            else
            {
                WaitForManagerQuestCompleted().Forget();
            }
        }

        private async UniTaskVoid WaitForManagerQuestCompleted()
        {
            await UniTask.WaitUntil(() => _managerDialogQuest.IsCompleted);

            _content.SetActive(true);
        }

        public void Disable()
        {
            _content.SetActive(false);
        }

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