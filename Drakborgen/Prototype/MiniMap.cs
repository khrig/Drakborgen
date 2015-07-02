using System.Collections.Generic;
using Gengine.Rendering;
using Gengine.UI;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class MiniMap{
        private readonly IEnumerable<IRenderable> _renderables;

        public MiniMap(){
            _renderables = new List<IRenderable>{
                new SpriteNode("minimap", new Vector2(30, 30), new Rectangle(0, 0, 64, 16))
            };
        }

        public IEnumerable<IRenderable> RenderTiles(){
            return _renderables;
        }
    }
}
