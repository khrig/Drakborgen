using Drakborgen.Components;
using Gengine.Components;
using Gengine.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace Drakborgen.Systems {
    public class PhysicsSystem : EntityProcessingSystem {
        public PhysicsSystem() : base(typeof(PhysicsComponent), typeof(InputComponent)) { }

        public override void Process(Entity entity, float dt){
            var physics = entity.GetComponent<PhysicsComponent>();
            var inputComponent = entity.GetComponent<InputComponent>();
            var velocity = physics.Velocity;

            velocity.X = inputComponent.DirectionX * physics.MoveAcceleration * dt;
            velocity.X = MathHelper.Clamp(velocity.X, -physics.MaxMoveSpeed, physics.MaxMoveSpeed);
            velocity.X *= physics.GroundDragFactor;

            velocity.Y = inputComponent.DirectionY * physics.MoveAcceleration * dt;
            velocity.Y = MathHelper.Clamp(velocity.Y, -physics.MaxMoveSpeed, physics.MaxMoveSpeed);
            velocity.Y *= physics.GroundDragFactor;
            
            physics.Velocity = velocity;
            physics.Position = physics.Position + (velocity * dt);
        }
    }
}
