using System.Collections.Generic;
using System.Linq;
using Gengine;
using Gengine.CollisionDetection;
using Gengine.Map;
using Gengine.Rendering;

namespace Drakborgen.Prototype {
    public class MapLoader {
        private Map _map;

        public void Load(IWorld world, int id){
            _map = new Map(world.View.Width, world.View.Height, 32);
            _map.Load(id);
        }

        public IEnumerable<IRenderable> RenderTiles(){
            if (_map == null)
                return Enumerable.Empty<IRenderable>();
            return _map.RenderTiles();
        }

        public ICollidableMap CollisionLayer { get { return _map; } }
        public IEnumerable<ICollidable> Doors { get { return _map.Doors;  } }
    }
}
