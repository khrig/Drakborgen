using System.Collections.Generic;
using System.Linq;
using Drakborgen.Components;
using Drakborgen.Prototype;
using Drakborgen.Systems;
using Gengine.Commands;
using Gengine.Entities;
using Gengine.EntityComponentSystem;
using Gengine.State;
using Gengine.Systems;
using Microsoft.Xna.Framework;

namespace Drakborgen.States {
    public class GameState : State{
        private EntityComponentSystem _entityComponentSystem;
        private ICollisionSystem _collisionSystem;
        private Map _map;

        public override void Init(){
            _collisionSystem = new ArcadeCollisionSystem(true);
            _entityComponentSystem = new EntityComponentSystem();
            _entityComponentSystem.Create(new InputComponent(), new RenderComponent("player", new Rectangle(0, 0, 32, 32)), new PhysicsComponent(new Vector2(100, 100), 32));
            //_entityComponentSystem.Create(new RenderComponent("player", new Rectangle(32, 0, 32, 32)), new PhysicsComponent(new Vector2(200, 100)));

            _entityComponentSystem.RegisterSystem(new InputSystem());
            _entityComponentSystem.RegisterSystem(new PhysicsSystem());
            _entityComponentSystem.RegisterSystem(new RenderSystem());

            _map = new Map(World.View.Width, World.View.Height, 32);
        }

        public override bool Update(float deltaTime) {
            _entityComponentSystem.Update(deltaTime);
            _collisionSystem.Collide(_entityComponentSystem.GetAllComponents<PhysicsComponent>(), _map);
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
            return _map.RenderTiles().Concat(_entityComponentSystem.GetAllComponents<RenderComponent>()).Concat(_collisionSystem.Collisions);
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
