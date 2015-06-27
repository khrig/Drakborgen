using System;
using Gengine.Entities;
using Gengine.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace Drakborgen.Components {
    public class PhysicsComponent : IComponent, ICollidable {
        private readonly int _spriteSize;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsOnGround { get; set; }
        private Rectangle _boundingBox;
        public Rectangle BoundingBox { get { return _boundingBox; } set { _boundingBox = value; } }

        public float MoveAcceleration = 0.1f;
        public float MaxMoveSpeed = 0.3f;
        public float GroundDragFactor = 0.9f;

        public PhysicsComponent(Vector2 position, int spriteSize) {
            _spriteSize = spriteSize;
            Position = position;
            Velocity = Vector2.Zero;
            _boundingBox = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), _spriteSize, _spriteSize);
        }

        public void UpdateBoundingBox(){
            _boundingBox.X = (int)Math.Round(Position.X);
            _boundingBox.Y = (int)Math.Round(Position.Y);
        }
    }
}
