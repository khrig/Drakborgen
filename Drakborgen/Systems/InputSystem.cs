using Gengine.Commands;
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
            inputComponent.Input = InputKey.None;
            InputManager.Instance.HandleRealTimeInput(_commandQueue, _commandFactory);
            while (_commandQueue.HasCommands()) {
                RealTimeCommand(inputComponent, _commandQueue.GetNext());
            }
            _commandQueue.Clear();
        }

        private void RealTimeCommand(InputComponent inputComponent, ICommand command) {
            if (command.Name == "Left"){
                inputComponent.Input |= InputKey.Left;
            }
            if (command.Name == "Right"){
                inputComponent.Input |= InputKey.Right;
            }
            if (command.Name == "Up"){
                inputComponent.Input |= InputKey.Up;
            }
            if (command.Name == "Down"){
                inputComponent.Input |= InputKey.Down;
            }
            if (command.Name == "Shoot"){
                inputComponent.Input |= InputKey.Shoot;
            }
            //if (command.Name == "Jump")
            //    _physicsComponent.WantToJump = true;

            //_equipmentComponent.HandleCommand(command);
        }
    }
}
