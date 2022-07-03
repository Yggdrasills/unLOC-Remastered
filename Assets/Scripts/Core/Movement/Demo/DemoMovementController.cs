using VContainer.Unity;

namespace SevenDays.unLOC.Core.Movement.Demo
{
    public class DemoMovementController : IStartable
    {
        private readonly IMovementService _movementService;
        private readonly DemoPlayerView _playerView;

        public DemoMovementController(
            IMovementService movementService,
            DemoPlayerView playerView)
        {
            _movementService = movementService;
            _playerView = playerView;
        }

        public void Start()
        {
            _playerView.IsActive = true;
        }
    }
}