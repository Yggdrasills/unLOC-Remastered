using System.Collections.Generic;
using System.Linq;

using SevenDays.unLOC.Core.Movement;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SevenDays.unLOC.Core.Player
{
    public static class CharacterExtensions
    {
        public static void RegisterCharacter(this IContainerBuilder builder,
            Camera camera,
            InitializeConfig initializeConfig,
            Transform transform)
        {
            IEnumerable<IInputModel> horizontalInput = new[] {new HorizontalInputModel()};

            var player = CreatePlayer(initializeConfig, transform);


            builder.RegisterEntryPoint<InputController>()
                .AsSelf()
                .WithParameter(horizontalInput);

            builder.RegisterInstance(player);

            builder.RegisterEntryPoint<PlayerCreator>()
                .WithParameter(camera)
                .WithParameter(initializeConfig)
                .WithParameter(horizontalInput.First())
                .AsSelf();
        }

        private static PlayerView CreatePlayer(InitializeConfig initializeConfig, Transform transform)
        {
            var player = Object.Instantiate(initializeConfig.PlayerViewPrefab, transform);
            player.transform.position = initializeConfig.PlayerInitPosition;

            player.CharacterScale.SetScale(initializeConfig.PlayerSize, initializeConfig.PlayerColliderSize);
            return player;
        }
    }
}