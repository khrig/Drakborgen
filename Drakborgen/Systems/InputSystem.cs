using Gengine.Commands;
using Gengine.Components;
using Gengine.EntityComponentSystem;
using Gengine.Input;

namespace Drakborgen.Systems {
    public class InputSystem : EntityProcessingSystem {
        private readonly CommandQueue _commandQueue;
        private readonly ICommandFactory _commandFactory;

        public InputSystem() : base(typeof (InputComponent)){
            _commandFactory = new SimpleCommandFactory();
            _commandQueue = new CommandQueue();
        }

        public override void Process(Entity entity, float dt){
            var inputComponent = entity.GetComponent<InputComponent>();
            inputComponent.DirectionX = 0;
            inputComponent.DirectionY = 0;
            InputManager.Instance.HandleRealTimeInput(_commandQueue, _commandFactory);
            while (_commandQueue.HasCommands()) {
                RealTimeCommand(inputComponent, _commandQueue.GetNext());
            }
            _commandQueue.Clear();
        }

        private void RealTimeCommand(InputComponent inputComponent, ICommand command) {
            if (command.Name == "Left")
                inputComponent.DirectionX = -1;
            if (command.Name == "Right")
                inputComponent.DirectionX = 1;
            if (command.Name == "Up")
                inputComponent.DirectionY = -1;
            if (command.Name == "Down")
                inputComponent.DirectionY = 1;
            //if (command.Name == "Jump")
            //    _physicsComponent.WantToJump = true;

            //_equipmentComponent.HandleCommand(command);
        }
    }
}
