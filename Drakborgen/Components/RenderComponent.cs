using Gengine.Entities;
using Gengine.EntityComponentSystem;
using Microsoft.Xna.Framework;

namespace Drakborgen.Components {
    public class RenderComponent : IComponent, IRenderable {
        public RenderComponent(string textureName, Rectangle sourceRectangle) {
            TextureName = textureName;
            SourceRectangle = sourceRectangle;
        }

        public string TextureName { get; set; }
        public Vector2 RenderPosition { get; set; }
        public Rectangle SourceRectangle { get; set; }
    }
}
