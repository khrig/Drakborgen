using System.Collections.Generic;
using Gengine.DungeonGenerators;
using Gengine.Rendering;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class MiniMap{
        private readonly string _texture;
        private readonly int _positionX;
        private readonly int _positionY;
        private readonly int _tileSize;
        private int _currentRoomX;
        private int _currentRoomY;
        private Map _map;
        private const int _mapSize = 2;

        public MiniMap(int x, int y, int tileSize, string texture) {
            _positionX = x;
            _positionY = y;
            _tileSize = tileSize;
            _texture = texture;
        }

        public void SetMap(Map map){
            _map = map;
        }

        public IEnumerable<IRenderable> GetRenderables(){
            int yMin = _currentRoomY - _mapSize >= 0 ? _currentRoomY - _mapSize : 0;
            int yMax = _currentRoomY + _mapSize < _map.Height ? _currentRoomY + _mapSize : _map.Height - 1;
            int xMin = _currentRoomX - _mapSize >= 0 ? _currentRoomX - _mapSize : 0;
            int xMax = _currentRoomX + _mapSize < _map.Width ? _currentRoomX + _mapSize : _map.Width - 1;
            int xx = 0, yy = 0;
            for (int y = yMin;y <= yMax;y++) {
                for (int x = xMin;x <= xMax;x++) {
                    if (_map[x, y].IsLegitRoom){
                        yield return CreateMiniMapTileFromRoom(_map[x, y], _positionX + xx * _tileSize, _positionY + yy * _tileSize);
                    }
                    xx++;
                }
                yy++;
                xx = 0;
            }

            //for (int y = 0;y < _map.Height;y++) {
            //    for (int x = 0; x < _map.Width; x++){
            //        if (_map[x, y].IsLegitRoom && IsInRange(x, y)){
            //            yield return
            //                CreateMiniMapTileFromRoom(_map[x, y], _positionX + x * _tileSize, _positionY + y*_tileSize);
            //        }
            //    }
            //}
        }

        private bool IsInRange(int x, int y){
            return x > _currentRoomX - _mapSize && x < _currentRoomX + _mapSize && y > _currentRoomY - _mapSize && y < _currentRoomY + _mapSize;
        }

        private MiniMapTile CreateMiniMapTileFromRoom(Room room, int xPos, int yPos) {
            var tile = new MiniMapTile(_texture, room.Type);
            tile.SetPosition(xPos, yPos);
            return tile;
        }

        public void SetCurrentRoomPosition(int xStart, int yStart){
            _currentRoomX = xStart;
            _currentRoomY = yStart;
            for (int x = 0; x < _map.Width; x++){
                for (int y = 0; y < _map.Height; y++){
                    _map[x, y].Type = _map[x, y].Type == RoomType.Current ? _map[x, y].Type = RoomType.VisitedRoom : _map[x, y].Type;
                }
            }
            var room = _map[xStart, yStart];
            room.Type = room.Type != RoomType.Start && room.Type != RoomType.Boss ? RoomType.Current : room.Type;
        }
    }

    public class MiniMapTile : IRenderable {
        public string TextureName { get; set; }
        public Vector2 RenderPosition { get { return _renderPosition; } }
        public Rectangle SourceRectangle { get; set; }
        public bool DebugDraw { get; set; }
        public bool Visible { get; set; }
        private readonly RoomType _roomType;
        private Vector2 _renderPosition;

        public MiniMapTile(string texture, RoomType roomType) {
            TextureName = texture;
            _roomType = roomType;
            SourceRectangle = GetSourceRectangle(roomType);
            _renderPosition = Vector2.Zero;
        }

        public void Visit() {
            if (_roomType == RoomType.UnvisitedRoom || _roomType == RoomType.VisitedRoom)
                SourceRectangle = GetSourceRectangle(RoomType.Current);
            Visible = true;
        }

        public void Leave() {
            if (_roomType == RoomType.UnvisitedRoom || _roomType == RoomType.VisitedRoom)
                SourceRectangle = GetSourceRectangle(RoomType.VisitedRoom);
        }

        private Rectangle GetSourceRectangle(RoomType type) {
            switch (type) {
                case RoomType.VisitedRoom:
                    return new Rectangle(16, 0, 17, 16);
                case RoomType.Current:
                    return new Rectangle(32, 0, 16, 16);
                case RoomType.Start:
                    return new Rectangle(0, 0, 16, 16);
                case RoomType.UnvisitedRoom:
                    return new Rectangle(64, 0, 16, 16);
                case RoomType.Boss:
                    return new Rectangle(48, 0, 16, 16);
                default:
                    return new Rectangle(0,0,0,0);
            }
        }

        public void SetPosition(int x, int y){
            _renderPosition.X = x;
            _renderPosition.Y = y;
        }
    }
}
