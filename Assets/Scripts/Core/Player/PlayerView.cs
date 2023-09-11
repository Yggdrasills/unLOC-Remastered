using SevenDays.unLOC.Core.Player.Animations;
using SevenDays.unLOC.Core.Player.Animations.Config;
using SevenDays.unLOC.SaveSystem;
using SevenDays.Utils.Constants;

using ToolBox.Serialization;

using UnityEngine;

namespace SevenDays.unLOC.Core.Player
{
    public class PlayerView : MonoBehaviour, ISavableMono
    {
        [field: SerializeField]
        public AnimationCallBackView AnimationCallBackView { get; private set; }

        [field: SerializeField]
        public AnimationConfig AnimationConfig { get; private set; }

        [field: SerializeField]
        public CharacterScale CharacterScale { get; private set; }

        public Animator PlayerAnimator => _playerAnimator;
        public float MovingSpeed => _movingSpeed;

        [SerializeField]
        private float _movingSpeed = 2;

        [SerializeField]
        private Animator _playerAnimator;
        

        void ISavableMono.Save()
        {
            DataSerializer.Save(GameConstants.PlayerPosition, transform.position.x);
        }

        void ISavableMono.Load()
        {
            if (DataSerializer.TryLoad<float>(GameConstants.PlayerPosition, out var x))
            {
                var transformPosition = transform.position;
                transformPosition.x = x;
                transform.position = transformPosition;
            }
        }
    }
}