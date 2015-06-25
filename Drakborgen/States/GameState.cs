using System.Collections.Generic;
using System.Linq;
using Drakborgen.Components;
using Gengine.Commands;
using Gengine.Entities;
using Gengine.EntityComponentSystem;
using Gengine.State;
using Microsoft.Xna.Framework;

namespace Drakborgen.States {
    public class GameState : State{
        private EntityComponentSystem _entityComponentSystem;

        public override void Init(){
            _entityComponentSystem = new EntityComponentSystem();
            _entityComponentSystem.Create(new TestComponent(new Vector2(200, 100), "tiles32.png", new Rectangle(0, 0, 32, 32)));
            _entityComponentSystem.Create(new TestComponent(new Vector2(100, 100), "tiles32.png", new Rectangle(0, 0, 32, 32)));
        }

        public override bool Update(float deltaTime) {
            _entityComponentSystem.Update(deltaTime);
            return false;
        }

        public override void Unload(){
        }

        public override void HandleCommands(CommandQueue commandQueue){
            while (commandQueue.HasCommands()) {
                var command = commandQueue.GetNext();
                if (HandleCommand(command)) return;
            }
        }

        public override IEnumerable<IRenderable> GetRenderTargets(){
            return _entityComponentSystem.GetAllComponents<TestComponent>();
        }

        public override IEnumerable<IRenderableText> GetTextRenderTargets() {
            return Enumerable.Empty<IRenderableText>();
        }

        private bool HandleCommand(ICommand command) {
            if (command.Name == "Escape") {
                StateManager.PopState();
                StateManager.PushState("mainmenu");
                return true;
            }
            return false;
        }
    }
}
