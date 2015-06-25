using Drakborgen.Components;
using Gengine.Commands;
using Gengine.EntityComponentSystem;
using Gengine.Input;

namespace Drakborgen.Systems {
    public class InputSystem : EntityProcessingSystem {
        private CommandQueue _commandQueue;
        private ICommandFactory _commandFactory;
        public InputSystem() : base(typeof(InputComponent)){}

        public override void Process(Entity entity, float dt) {
            InputManager.Instance.HandleRealTimeInput(_commandQueue, _commandFactory);
            while (_commandQueue.HasCommands()) {
                RealTimeCommand(_commandQueue.GetNext());
            }
            _commandQueue.Clear();

            //_physicsComponent.Movement = _newmovement;
            //_newmovement = 0;
        }

        //public void GetRealTimeInput() {
        //    InputManager.Instance.HandleRealTimeInput(_commandQueue, _commandFactory);
        //    while (_commandQueue.HasCommands()) {
        //        RealTimeCommand(_commandQueue.GetNext());
        //    }
        //    _commandQueue.Clear();

        //}

        private void RealTimeCommand(ICommand command) {
            //if (command.Name == "Left")
            //    _newmovement = -1.0f;
            //if (command.Name == "Right")
            //    _newmovement = 1.0f;
            //if (command.Name == "Jump")
            //    _physicsComponent.WantToJump = true;

            //_equipmentComponent.HandleCommand(command);
        }
    }
}
