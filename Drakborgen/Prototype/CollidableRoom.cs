using System;
using System.Collections.Generic;
using System.Linq;
using Gengine.CollisionDetection;
using Gengine.DungeonGenerators;
using Gengine.Map;
using Gengine.Rendering;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class CollidableRoom : ICollidableMap {
        private readonly int _width;
        private readonly int _height;
        private readonly int _tileSize;
        private readonly int _tileCountX;
        private readonly int _tileCountY;
        public Tile[,] Tiles { get; set; }
        public int TileSize { get { return _tileSize; } }
        public List<ICollidable> Doors { get; private set; }

        public int PositionInGridX { get; set; }
        public int PositionInGridY { get; set; }

        private string _spriteName;

        public int Width{
            get { return _width; }
        }

        public int Height {
            get { return _height; }
        }

        public CollidableRoom(int width, int height, int tileSize, int x, int y){
            _width = width;
            _height = height;
            _tileSize = tileSize;
            _tileCountX = _width/_tileSize;
            _tileCountY = _height / _tileSize + 1;
            PositionInGridX = x;
            PositionInGridY = y;
            Doors = new List<ICollidable>(4);
        }

        public IEnumerable<IRenderable> RenderTiles() {
            for (int x = 0;x < _tileCountX;x++) {
                for (int y = 0;y < _tileCountY;y++) {
                    yield return Tiles[x, y];
                }
            }
        }

        public Tile Tile(int x, int y) {
            return Tiles[x, y];
        }

        public void ForeachTile(Action<Tile> tileAction) {
            for (int x = 0;x < _tileCountX;x++) {
                for (int y = 0;y < _tileCountY;y++) {
                    tileAction(Tiles[x, y]);
                }
            }
        }

        public Tile PositionToTile(float x, float y) {
            var tileX = (int)(x / _tileSize);
            var tileY = (int)(y / _tileSize);
            return Tiles[tileX, tileY];
        }

        public void CreateTiles(string spriteName, List<Door> doors){
            _spriteName = spriteName;
            Tiles = new Tile[_tileCountX, _tileCountY];
            InitializeGrid(doors);
            SetFaces();
        }

        private void InitializeGrid(IEnumerable<Door> doors) {
            AddTiles();
            AddDoors(doors);
        }

        private void AddTiles(){
            for (int x = 0; x < _tileCountX; x++){
                for (int y = 0; y < _tileCountY; y++){
                    Tile tile = CreateTile(x, y);
                    Tiles[x, y] = tile;
                }
            }
        }

        private void AddDoors(IEnumerable<Door> doors) {
            foreach (Door door in doors){
                switch (door.DoorPosition){
                    case DoorPosition.Top: 
                        CreateDoor(door, 9, 0, 10, 0, 308, 316, new Rectangle(5 * _tileSize, 0, _tileSize, _tileSize), new Rectangle(6 * _tileSize, 0, _tileSize, _tileSize));
                        break;
                    case DoorPosition.Bottom:
                        CreateDoor(door, 9, (_tileCountY - 1), 10, (_tileCountY - 1), 308, 40, new Rectangle(5 * _tileSize, 0, _tileSize, _tileSize), new Rectangle(6 * _tileSize, 0, _tileSize, _tileSize));
                        break;
                    case DoorPosition.Left:
                        CreateDoor(door, 0, (_tileCountY - 1) / 2, 0, 6, 565, 180, new Rectangle(5 * _tileSize, 0, _tileSize, _tileSize), new Rectangle(6 * _tileSize, 0, _tileSize, _tileSize));
                        break;
                    case DoorPosition.Right:
                        CreateDoor(door, 19, (_tileCountY - 1) / 2, 19, 6, 40, 180, new Rectangle(5 * _tileSize, 0, _tileSize, _tileSize), new Rectangle(6 * _tileSize, 0, _tileSize, _tileSize));
                        break;
                }
            }
        }

        private void CreateDoor(Door door, int x, int y, int x2, int y2, int targetX, int targetY, Rectangle textureSourceLeft, Rectangle textSourceRight) {
            var tileWithMapTransition = new TileWithMapTransition(_spriteName, new Vector2(x * _tileSize, y * _tileSize), textureSourceLeft, false);
            tileWithMapTransition.TargetTileMap = door.NextRoomId;
            tileWithMapTransition.TargetX = targetX;
            tileWithMapTransition.TargetY = targetY;
            Doors.Add(tileWithMapTransition);
            Tiles[x, y] = tileWithMapTransition;

            tileWithMapTransition = new TileWithMapTransition(_spriteName, new Vector2(x2 * _tileSize, y2 * _tileSize), textSourceRight, false);
            tileWithMapTransition.TargetTileMap = door.NextRoomId;
            tileWithMapTransition.TargetX = targetX;
            tileWithMapTransition.TargetY = targetY;
            Doors.Add(tileWithMapTransition);
            Tiles[x2, y2] = tileWithMapTransition;
        }

        private Tile CreateTile(int x, int y){
            Tile tile;
            if (y == 0 || x == 0 || x == _tileCountX - 1 || y == _tileCountY - 1) {
                tile = new Tile(_spriteName, new Vector2(x * _tileSize, y * _tileSize), GetTileSource(x, y));
            }
            else{
                tile = new Tile(_spriteName, new Vector2(x * _tileSize, y * _tileSize), new Rectangle(1 * _tileSize, 32, _tileSize, _tileSize), false);
            }
            return tile;
        }

        private Rectangle GetTileSource(int x, int y){
            if(x == 0 && y == 0) // top left
                return new Rectangle(0, 0, _tileSize, _tileSize);
            if (x == _tileCountX - 1 && y == 0) // top right
                return new Rectangle(64, 0, _tileSize, _tileSize);
            if (x == 0 && y == _tileCountY - 1) // bottom left
                return new Rectangle(0, 64, _tileSize, _tileSize);
            if (x == _tileCountX - 1 && y == _tileCountY - 1) // bottom right
                return new Rectangle(64, 64, _tileSize, _tileSize);

            if(x == 0) // left row
                return new Rectangle(0, 32, _tileSize, _tileSize);
            if (y == 0) // top row
                return new Rectangle(32, 0, _tileSize, _tileSize);
            if (x == _tileCountX - 1) // right row
                return new Rectangle(64, 32, _tileSize, _tileSize);
            if (y == _tileCountY - 1) // bottom row
                return new Rectangle(32, 64, _tileSize, _tileSize);

            return new Rectangle(2*32, 3 * 32, _tileSize, _tileSize);
        }

        private void SetFaces() {
            for (int x = 0;x < _tileCountX;x++) {
                for (int y = 0;y < _tileCountY;y++) {
                    var tile = Tiles[x, y];
                    if (tile.IsSolid) {
                        if (x - 1 > 0 && !Tiles[x - 1, y].IsSolid){
                            tile.FaceLeft = true;
                        }
                        if (x + 1 < _tileCountX && !Tiles[x + 1, y].IsSolid) {
                            tile.FaceRight = true;
                        }
                        if (y - 1 > 0 && !Tiles[x, y - 1].IsSolid) {
                            tile.FaceTop = true;
                        }
                        if (y + 1 < _tileCountY && !Tiles[x, y + 1].IsSolid) {
                            tile.FaceBottom = true;
                        }
                    }
                }
            }
        }
    }
}
