using System.Collections.Generic;
using Drakborgen.Components;
using Drakborgen.Prototype;
using Drakborgen.Systems;
using Gengine.Animation;
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
        private readonly ICollisionSystem _collisionSystem;
        private readonly Castle _castle;
        private Entity _player;
        private MiniMap _miniMap;
        
        public GameState(){
            _collisionSystem = new ArcadeCollisionSystem(true);
            EntityWorld.RegisterUpdateSystems(new InputSystem(), new PhysicsSystem(), new AnimationSystem(new AnimationMapper()));
            EntityWorld.RegisterRenderSystem(new RenderSystem());
            _castle = new Castle();
            _miniMap = new MiniMap();
        }

        public override void Setup() {
            _castle.GenerateCastle();
            
            ResetGameWorld();
        }

        public override void Run() {
            _player = AddEntity(new InputComponent(),
                new RenderComponent("player", new Rectangle(0, 0, 32, 32), new Vector2(100, 100)),
                new PhysicsComponent(new Vector2(World.View.Width * 0.5f - 16, World.View.Height * 0.5f - 16), 32),
                new AnimationComponent(GetPlayerAnimations()));

            _castle.Load(GetStateValue<int>("room"));
            AddRenderable(_miniMap.RenderTiles(), 2);
            AddRenderable(_castle.RenderTiles());
            AddRenderable(EntityWorld.GetAllComponents<RenderComponent>());
        }

        public override bool Update(float deltaTime) {
            EntityWorld.Update(deltaTime);

            _collisionSystem.Collide(EntityWorld.GetAllComponents<PhysicsComponent>(), _castle.CollisionLayer);
            _collisionSystem.Overlap(_player.GetComponent<PhysicsComponent>(), _castle.Doors, OnDoorOverlap);

            EntityWorld.UpdateBeforeDraw(deltaTime);
            return false;
        }
        
        public override IEnumerable<IRenderableText> GetTextRenderTargets(){
            yield return _castle.RoomId;
        }

        protected override bool HandleCommand(ICommand command) {
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
                SetStateValue("room", door.TargetTileMap);
                var turns = GetStateValue<int>("turns");
                turns--;
                if (turns <= 0) {
                    StateManager.ClearStates();
                    StateManager.PushState(new FadeTransition(0.5f, "mainmenu"));
                    ResetGameWorld();
                } else {
                    SetStateValue("turns", turns);
                    StateManager.PushState(new FadeTransition(0.1f, "game", () =>{
                        _player.GetComponent<PhysicsComponent>().Position = new Vector2(door.TargetX, door.TargetY);
                        _player.GetComponent<RenderComponent>().RenderPosition = new Vector2(door.TargetX, door.TargetY);
                    }));
                }
                return true;
            }
            return false;
        }

        private void ResetGameWorld() {
            SetStateValue("turns", 3);
            SetStateValue("room", 1);
        }

        private Dictionary<string, Animation> GetPlayerAnimations(){

            // Should read from some file or something

            var animations = new Dictionary<string, Animation>();
            var idle = new Animation();
            idle.AddFrame(float.MaxValue, new Rectangle(0, 0, 32, 32));
            animations.Add("idle", idle);

            var moveleft = new Animation();
            moveleft.AddFrame(100.8f, new Rectangle(352, 0, 32, 32));
            moveleft.AddFrame(100.2f, new Rectangle(384, 0, 32, 32));
            animations.Add("moveleft", moveleft);
            
            var moveright = new Animation();
            moveright.AddFrame(100.8f, new Rectangle(416, 0, 32, 32));
            moveright.AddFrame(100.2f, new Rectangle(448, 0, 32, 32));
            animations.Add("moveright", moveright);
            
            var moveup = new Animation();
            moveup.AddFrame(100.8f, new Rectangle(480, 0, 32, 32));
            moveup.AddFrame(100.2f, new Rectangle(512, 0, 32, 32));
            animations.Add("moveup", moveup);
            
            var movedown = new Animation();
            movedown.AddFrame(100.8f, new Rectangle(288, 0, 32, 32));
            movedown.AddFrame(100.2f, new Rectangle(320, 0, 32, 32));
            animations.Add("movedown", movedown);
            return animations;
        }
    }
}
