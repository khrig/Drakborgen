using System;
using System.Collections.Generic;
using Gengine.CollisionDetection;
using Gengine.Map;
using Gengine.Rendering;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class Room : ICollidableMap {
        private readonly int _width;
        private readonly int _height;
        private readonly int _tileSize;
        private readonly int _tileCountX;
        private readonly int _tileCountY;
        public Tile[,] Tiles { get; set; }
        public int TileSize { get { return _tileSize; } }
        public List<ICollidable> Doors { get; private set; }

        private string _spriteName = "dungeon";

        public int Width{
            get { return _width; }
        }

        public int Height {
            get { return _height; }
        }

        public Room(int width, int height, int tileSize){
            _width = width;
            _height = height;
            _tileSize = tileSize;
            _tileCountX = _width/_tileSize;
            _tileCountY = _height / _tileSize + 1;
            Doors = new List<ICollidable>(4);
        }

        private void CreateTiles(int room) {
            Tiles = new Tile[_tileCountX, _tileCountY];
            InitializeGrid(room);
            SetFaces();
        }

        private void InitializeGrid(int room){
            for (int x = 0; x < _tileCountX; x++){
                for (int y = 0; y < _tileCountY; y++){
                    Tile tile;
                    if(room == 1)
                        tile = CreateTile(x, y);
                    else
                        tile = CreateTileInverse(x, y);
                    
                    Tiles[x, y] = tile;
                }
            }
        }

        private Tile CreateTile(int x, int y){
            Tile tile;
            if (IsDoor(x, y)){
                int doorTile = 0;
                if (x == 9)
                    doorTile = 5*_tileSize;
                else
                    doorTile = 6 * _tileSize;


                var tileWithMapTransition = new TileWithMapTransition(_spriteName, new Vector2(x * _tileSize, y * _tileSize), new Rectangle(doorTile, 0, _tileSize, _tileSize), false);
                tileWithMapTransition.TargetTileMap = 2;
                tileWithMapTransition.TargetX = 308;
                tileWithMapTransition.TargetY = 316;
                Doors.Add(tileWithMapTransition);
                tile = tileWithMapTransition;
            } // Collidable tiles
            else if (y == 0 || x == 0 || x == _tileCountX - 1 || y == _tileCountY - 1) {
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

        private Tile CreateTileInverse(int x, int y) {
            Tile tile;
            if ((x == 9 || x == 10) && y == _tileCountY - 1){
                Rectangle doorTile;
                if (x == 9)
                    doorTile = new Rectangle(5 * _tileSize, 3*32, _tileSize, _tileSize);
                else
                    doorTile = new Rectangle(6 * _tileSize, 3*32, _tileSize, _tileSize);

                var tileWithMapTransition = new TileWithMapTransition(_spriteName, new Vector2(x * _tileSize, y * _tileSize), doorTile, false);
                tileWithMapTransition.TargetTileMap = 1;
                tileWithMapTransition.TargetX = 304;
                tileWithMapTransition.TargetY = 34;
                Doors.Add(tileWithMapTransition);
                tile = tileWithMapTransition;
            } // Collidable tiles
            else if (y == 0 || x == 0 || x == _tileCountX - 1 || y == _tileCountY - 1) {
                tile = new Tile(_spriteName, new Vector2(x * _tileSize, y * _tileSize), GetTileSource(x, y));
            } else {
                tile = new Tile(_spriteName, new Vector2(x * _tileSize, y * _tileSize), new Rectangle(1 * _tileSize, 32, _tileSize, _tileSize), false);
            }
            return tile;
        }

        private static bool IsDoor(int x, int y){
            return (x == 9 || x == 10) && y == 0;
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

        public IEnumerable<IRenderable> RenderTiles(){
            for (int x = 0; x < _tileCountX; x++){
                for (int y = 0; y < _tileCountY; y++){
                    yield return Tiles[x, y];
                }
            }
        }

        public Tile Tile(int x, int y){
            return Tiles[x,y];
        }

        public void ForeachTile(Action<Tile> tileAction){
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

        public void Load(int targetTileMap){
            Doors.Clear();
            CreateTiles(targetTileMap);
        }
    }
}
