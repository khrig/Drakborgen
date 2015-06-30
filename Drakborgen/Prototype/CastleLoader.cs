using System.Collections.Generic;
using System.Linq;
using Gengine.CollisionDetection;
using Gengine.Map;
using Gengine.Rendering;

namespace Drakborgen.Prototype {
    public class CastleLoader {
        private Room _room;

        private readonly Dictionary<int, Room> _rooms;

        public CastleLoader(){
            _rooms = new Dictionary<int, Room>(120);
        }

        public void GenerateCastle() {
            var room = new Room(640, 360, 32);
            room.Load(1);
            var room2 = new Room(640, 360, 32);
            room2.Load(2);
            _rooms.Add(1, room);
            _rooms.Add(2, room2);
        }

        public void Load(int id){
            _room = _rooms[id];
        }

        public IEnumerable<IRenderable> RenderTiles(){
            if (_room == null)
                return Enumerable.Empty<IRenderable>();
            return _room.RenderTiles();
        }

        public ICollidableMap CollisionLayer { get { return _room; } }
        public IEnumerable<ICollidable> Doors { get { return _room.Doors;  } }
    }
}
