using System;
using Gengine.CollisionDetection;
using Gengine.Entities;
using Gengine.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace Drakborgen.Components {
    public class PhysicsComponent : IComponent, ICollidable {
        private readonly int _spriteSize;
        private Vector2 _position;
        public Vector2 Position { get { return _position; } set { _position = value; } }
        public Vector2 Velocity { get; set; }

        public bool IsOnGround { get; set; }

        public float MoveAcceleration = 0.1f;
        public float MaxMoveSpeed = 0.3f;
        public float GroundDragFactor = 0.9f;

        public PhysicsComponent(Vector2 position, int spriteSize) {
            _spriteSize = spriteSize;
            _position = position;
            Velocity = Vector2.Zero;
        }
        
        public Rectangle GetBoundingBox(){
            return new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), _spriteSize, _spriteSize);
        }
    }
}
