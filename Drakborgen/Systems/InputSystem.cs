using Drakborgen.Components;
using Gengine.Commands;
using Gengine.EntityComponentSystem;
using Gengine.Input;

namespace Drakborgen.Systems {
    public class InputSystem : EntityProcessingSystem {
        private CommandQueue _commandQueue;
        private ICommandFactory _commandFactory;

        public InputSystem() : base(typeof (InputComponent)){
            _commandFactory = new SimpleCommandFactory();
            _commandQueue = new CommandQueue();
        }

        public override void Process(Entity entity, float dt) {
            InputManager.Instance.HandleRealTimeInput(_commandQueue, _commandFactory);
            while (_commandQueue.HasCommands()) {
                RealTimeCommand(entity.GetComponent<InputComponent>(), _commandQueue.GetNext());
            }
            _commandQueue.Clear();
        }

        private void RealTimeCommand(InputComponent physicsComponent, ICommand command) {
            if (command.Name == "Left")
                physicsComponent.Direction = -1.0f;
            if (command.Name == "Right")
                physicsComponent.Direction = 1.0f;
            //if (command.Name == "Jump")
            //    _physicsComponent.WantToJump = true;

            //_equipmentComponent.HandleCommand(command);
        }
    }
}
