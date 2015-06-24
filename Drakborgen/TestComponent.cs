using Gengine.Entities;
using Gengine.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace Drakborgen {
    public class TestComponent : Component, IRenderable {
        public TestComponent(Vector2 renderPosition, string textureName, Rectangle sourceRectangle){
            RenderPosition = renderPosition;
            TextureName = textureName;
            SourceRectangle = sourceRectangle;
        }

        public string TextureName { get; private set; }
        public Vector2 RenderPosition { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
    }
}
