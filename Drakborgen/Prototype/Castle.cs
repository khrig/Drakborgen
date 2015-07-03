using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gengine.CollisionDetection;
using Gengine.DungeonGenerators;
using Gengine.Map;
using Gengine.Rendering;
using Gengine.UI;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class Castle {
        private readonly Dictionary<int, GameRoom> _rooms;
        private readonly CornerToMidGenerator _dungeonGenerator;
        private int _currentRoom = 1;

        public Castle(){
            _dungeonGenerator = new CornerToMidGenerator();
            _rooms = new Dictionary<int, GameRoom>(30);
            _currentRoomText = new TextNode("text", "", new Vector2(10,10), Color.Azure);
        }

        public Map GenerateCastle(){
            Map map = _dungeonGenerator.CreateDungeon(10, 10);

            for (int y = 0;y < map.Height;y++) {
                for (int x = 0;x < map.Width;x++) {
                    if(map[x, y].IsLegitRoom)
                        _rooms.Add(map[x, y].Id, CreateGameRoom(map[x, y]));
                }
            }

            map.SetStart(0,0);
            _currentRoom = map.GetStartRoom().Id;
            return map;
        }

        private GameRoom CreateGameRoom(Room room){
            var gameRoom = new GameRoom(640, 360, 32, room.X, room.Y);
            gameRoom.CreateTiles("dungeon", room.Doors);
            return gameRoom;
        }

        public void Load(int id){
            _currentRoom = id;
            _currentRoomText.Text = id.ToString();
            Debug.WriteLine(id);
        }

        private readonly TextNode _currentRoomText;
        public TextNode RoomId{
            get { return _currentRoomText; }
        }

        public IEnumerable<IRenderable> RenderTiles(){
            if (_rooms[_currentRoom] == null)
                return Enumerable.Empty<IRenderable>();
            return _rooms[_currentRoom].RenderTiles();
        }

        public GameRoom CurrentRoom { get { return _rooms[_currentRoom]; } }
        public ICollidableMap CollisionLayer { get { return _rooms[_currentRoom]; } }
        public IEnumerable<ICollidable> Doors { get { return _rooms[_currentRoom].Doors; } }
    }
}
