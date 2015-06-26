using Gengine.Entities;
using Gengine.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace Drakborgen.Components {
    public class PhysicsComponent : IComponent, ICollidable {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsOnGround { get; set; }
        private Rectangle _boundingBox;
        public Rectangle BoundingBox { get { return _boundingBox; } set { _boundingBox = value; } }

        public float MoveAcceleration = 0.1f;
        public float MaxMoveSpeed = 0.4f;
        public float GroundDragFactor = 0.9f;

        public PhysicsComponent(Vector2 position) {
            Position = position;
            Velocity = Vector2.Zero;
        }

        public void UpdateBoundingBox(){
            _boundingBox.X = (int)Position.X;
            _boundingBox.Y = (int)Position.Y;
        }
    }
}
