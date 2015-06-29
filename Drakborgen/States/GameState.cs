using System.Collections.Generic;
using System.Linq;
using Drakborgen.Components;
using Drakborgen.Prototype;
using Drakborgen.Systems;
using Gengine.Animation;
using Gengine.Camera;
using Gengine.CollisionDetection;
using Gengine.Commands;
using Gengine.EntityComponentSystem;
using Gengine.Input;
using Gengine.Map;
using Gengine.Rendering;
using Gengine.State;
using Microsoft.Xna.Framework;

namespace Drakborgen.States {
    public class GameState : SceneState {
        private readonly EntityComponentSystem _entityComponentSystem;
        private readonly ICollisionSystem _collisionSystem;
        private readonly MapLoader _mapLoader;
        private Entity _player;
        private static int _currentRoom = 1;

        public GameState(){
            _collisionSystem = new ArcadeCollisionSystem(true);
            _entityComponentSystem = new EntityComponentSystem();
            _entityComponentSystem.RegisterUpdateSystems(new InputSystem(), new PhysicsSystem(), new AnimationSystem(new AnimationMapper()));
            _entityComponentSystem.RegisterRenderSystem(new RenderSystem());
            _mapLoader = new MapLoader();
        }

        public override void Init(){
            _player = _entityComponentSystem.Create(new InputComponent(), 
                new RenderComponent("player", new Rectangle(0, 0, 32, 32), new Vector2(100, 100)), 
                new PhysicsComponent(new Vector2(100, 100), 32),
                new AnimationComponent(GetPlayerAnimations()));

            _mapLoader.Load(World, _currentRoom);

            AddRenderable(_mapLoader.RenderTiles());
            AddRenderable(_collisionSystem.Collisions);
            AddRenderable(_entityComponentSystem.GetAllComponents<RenderComponent>());
        }

        public override bool Update(float deltaTime) {
            _entityComponentSystem.Update(deltaTime);

            _collisionSystem.Collide(_entityComponentSystem.GetAllComponents<PhysicsComponent>(), _mapLoader.CollisionLayer);
            _collisionSystem.Overlap(_player.GetComponent<PhysicsComponent>(), _mapLoader.Doors, OnDoorOverlap);

            _entityComponentSystem.UpdateBeforeDraw(deltaTime);
            return false;
        }

        public override void Unload(){
            RemoveRenderable(_mapLoader.RenderTiles());
            RemoveRenderable(_collisionSystem.Collisions);
            RemoveRenderable(_entityComponentSystem.GetAllComponents<RenderComponent>());
            _entityComponentSystem.Remove(_player);
        }

        public override void HandleCommands(CommandQueue commandQueue){
            while (commandQueue.HasCommands()) {
                var command = commandQueue.GetNext();
                if (HandleCommand(command)) return;
            }
        }

        public override IEnumerable<IRenderableText> GetTextRenderTargets() {
            return Enumerable.Empty<IRenderableText>();
        }

        private bool HandleCommand(ICommand command) {
            if (command.Name == "Escape") {
                StateManager.ClearStates();
                StateManager.PushState(new FadeTransition(0.5f, "mainmenu"));
                return true;
            }
            return false;
        }

        private bool OnDoorOverlap(ICollidable first, ICollidable second) {
            var door = second as TileWithMapTransition;
            if (door != null) {
                _currentRoom = door.TargetTileMap;
                StateManager.PushState(new FadeTransition(0.5f, "game", () =>{
                    _player.GetComponent<PhysicsComponent>().Position = new Vector2(door.TargetX, door.TargetY);
                    _player.GetComponent<RenderComponent>().RenderPosition = new Vector2(door.TargetX, door.TargetY);
                }));
                return true;
            }
            return false;
        }

        private Dictionary<string, Animation> GetPlayerAnimations(){

            // Should read from some file or something

            var animations = new Dictionary<string, Animation>();
            var idle = new Animation();
            idle.AddFrame(float.MaxValue, new Rectangle(32, 32, 32, 32));
            animations.Add("idle", idle);
            var moveleft = new Animation();
            moveleft.AddFrame(100.8f, new Rectangle(128, 0, 32, 32));
            moveleft.AddFrame(100.2f, new Rectangle(160, 0, 32, 32));
            animations.Add("moveleft", moveleft);
            var moveright = new Animation();
            moveright.AddFrame(100.8f, new Rectangle(0, 0, 32, 32));
            moveright.AddFrame(100.2f, new Rectangle(32, 0, 32, 32));
            animations.Add("moveright", moveright);
            var moveup = new Animation();
            moveup.AddFrame(100.8f, new Rectangle(0, 32, 32, 32));
            moveup.AddFrame(100.2f, new Rectangle(32, 32, 32, 32));
            animations.Add("moveup", moveup);
            var movedown = new Animation();
            movedown.AddFrame(100.8f, new Rectangle(128, 32, 32, 32));
            movedown.AddFrame(100.2f, new Rectangle(0, 32, 32, 32));
            animations.Add("movedown", movedown);
            return animations;
        }
    }
}
