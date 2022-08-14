using System.Collections.Generic;

using SevenDays.unLOC.Core.Movement;

using VContainer.Unity;

namespace SevenDays.unLOC.Core
{
    public class InputController : ITickable
    {
        private readonly IEnumerable<IInputModel> _inputModels;
        public InputController(IEnumerable<IInputModel> inputModels)
        {
            _inputModels = inputModels;
        }
        
        void ITickable.Tick()
        {
            foreach (var inputModel in _inputModels)
            {
                UpdateInput(inputModel);
            }
        }

        private void UpdateInput(IInputModel inputModel)
        {
            inputModel.Input.Value = inputModel.ValueGetter();
            inputModel.PreviousInput = inputModel.Input.Value;
        }
    }
}