using System.Collections.Generic;
using System.Linq;

using SevenDays.unLOC.Core.Movement;
using SevenDays.unLOC.Core.Player;
using SevenDays.unLOC.Utils.Helpers;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Scopes
{
    public class WorkshopLifetimeScope : AutoInjectableLifetimeScope
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private InitializeConfig _initializeConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterMovement(builder);
        }

        private void RegisterMovement(IContainerBuilder builder)
        {
            builder.RegisterInstance(_initializeConfig);
            IEnumerable<IInputModel> horizontalInput = new[] {new HorizontalInputModel()};
            builder.RegisterEntryPoint<InputController>().AsSelf().WithParameter(horizontalInput);
            
            var player = CreatePlayer();

            builder.RegisterInstance(player);
            
            builder.RegisterEntryPoint<PlayerCreator>()
                .WithParameter(_camera)
                .WithParameter(horizontalInput.First())
                .AsSelf();
        }
        
        private PlayerView CreatePlayer()
        {
            var player = Instantiate(_initializeConfig.PlayerViewPrefab,transform);
            player.transform.position = _initializeConfig.PlayerInitPosition;

            player.CharacterScale.SetScale(_initializeConfig.PlayerSize, _initializeConfig.PlayerColliderSize);
            return player;
        }
    }
}