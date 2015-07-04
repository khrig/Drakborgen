using Drakborgen.Components;
using Gengine.EntityComponentSystem;
using Gengine.Input;
using Microsoft.Xna.Framework;

namespace Drakborgen.Systems {
    public class PhysicsSystem : EntityProcessingSystem {
        public PhysicsSystem() : base(typeof(PhysicsComponent), typeof(InputComponent)) { }

        public override void Process(Entity entity, float dt){
            var physics = entity.GetComponent<PhysicsComponent>();
            var inputComponent = entity.GetComponent<InputComponent>();
            var velocity = physics.Velocity;

            int directionX = GetDirectionX(inputComponent.Input);
            int directionY = GetDirectionY(inputComponent.Input);

            velocity.X = directionX * physics.MoveAcceleration * dt;
            velocity.X = MathHelper.Clamp(velocity.X, -physics.MaxMoveSpeed, physics.MaxMoveSpeed);
            velocity.X *= physics.GroundDragFactor;

            velocity.Y = directionY * physics.MoveAcceleration * dt;
            velocity.Y = MathHelper.Clamp(velocity.Y, -physics.MaxMoveSpeed, physics.MaxMoveSpeed);
            velocity.Y *= physics.GroundDragFactor;
            
            physics.Velocity = velocity;
            physics.Position = physics.Position + (velocity * dt);
        }

        private int GetDirectionX(InputKey input){
            if (input.HasFlag(InputKey.Left))
                return -1;
            if (input.HasFlag(InputKey.Right))
                return 1;
            return 0;
        }

        private int GetDirectionY(InputKey input) {
            if (input.HasFlag(InputKey.Up))
                return -1;
            if (input.HasFlag(InputKey.Down))
                return 1;
            return 0;
        }
    }
}
