using Drakborgen.Components;
using Gengine.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace Drakborgen.Systems {
    public class PhysicsSystem : EntityProcessingSystem {
        public PhysicsSystem() : base(typeof(PhysicsComponent)) { }

        public override void Process(Entity entity, float dt){
            var physics = entity.GetComponent<PhysicsComponent>();
            var inputComponent = entity.GetComponent<InputComponent>();
            var velocity = physics.Velocity;
            velocity.X = inputComponent.Direction * physics.MoveAcceleration * dt;
            velocity.X = MathHelper.Clamp(velocity.X, -physics.MaxMoveSpeed, physics.MaxMoveSpeed);
            velocity.X *= physics.GroundDragFactor;

            //velocity.Y = physics.Direction * physics.MoveAcceleration * dt;

            //if (!physics.IsOnGround)
            //    velocity.Y += physics.Gravity;
            //else
            //    velocity.Y = 0;

            physics.Velocity = velocity;
            physics.Position = physics.Position + (velocity * dt);
            inputComponent.Direction = 0.0f;

            physics.UpdateBoundingBox();

            //_velocity.X += Direction * MoveAcceleration * deltaTime;
            //_velocity.X = MathHelper.Clamp(_velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
            //_velocity.X *= GroundDragFactor;

            //if (!IsOnGround)
            //    _velocity.Y += Gravity;
            //else
            //    _velocity.Y = 0;

            //Position += _velocity * deltaTime;
            //Direction = 0.0f;
        }
    }
}
