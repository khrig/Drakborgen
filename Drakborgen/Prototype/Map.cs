using System;
using System.Collections.Generic;
using Gengine.CollisionDetection;
using Gengine.Entities;
using Gengine.Map;
using Gengine.Rendering;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class Map : ICollidableMap {
        private readonly int _width;
        private readonly int _height;
        private readonly int _tileSize;
        private readonly int _tileCountX;
        private readonly int _tileCountY;
        public Tile[,] Tiles { get; set; }
        public int TileSize { get { return _tileSize; } }
        public List<ICollidable> Doors { get; private set; }
 
        public Map(int width, int height, int tileSize){
            _width = width;
            _height = height;
            _tileSize = tileSize;
            _tileCountX = _width/_tileSize + 1;
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
                var tileWithMapTransition = new TileWithMapTransition("tiles32.png", new Vector2(x*_tileSize, y*_tileSize), new Rectangle(6*_tileSize, 0, _tileSize, _tileSize), false);
                tileWithMapTransition.TargetTileMap = 2;
                tileWithMapTransition.TargetX = 100;
                tileWithMapTransition.TargetY = 100;
                Doors.Add(tileWithMapTransition);
                tile = tileWithMapTransition;
            } // Collidable tiles
            else if (y == 0 || x == 0 || x*_tileSize == _width - _tileSize || y*_tileSize >= _height - _tileSize){
                tile = new Tile("tiles32.png", new Vector2(x*_tileSize, y*_tileSize), new Rectangle(7*_tileSize, 0, _tileSize, _tileSize));
            }
            else{
                tile = new Tile("tiles32.png", new Vector2(x*_tileSize, y*_tileSize), new Rectangle(8*_tileSize, 0, _tileSize, _tileSize), false);
            }
            return tile;
        }

        private Tile CreateTileInverse(int x, int y) {
            Tile tile;
            if ((x == 9 || x == 10) && y == _tileCountY - 1) {
                var tileWithMapTransition = new TileWithMapTransition("tiles32.png", new Vector2(x * _tileSize, y * _tileSize), new Rectangle(6 * _tileSize, 0, _tileSize, _tileSize), false);
                tileWithMapTransition.TargetTileMap = 1;
                tileWithMapTransition.TargetX = 300;
                tileWithMapTransition.TargetY = 200;
                Doors.Add(tileWithMapTransition);
                tile = tileWithMapTransition;
            } // Collidable tiles
            else if (y == 0 || x == 0 || x * _tileSize == _width - _tileSize || y * _tileSize >= _height - _tileSize) {
                tile = new Tile("tiles32.png", new Vector2(x * _tileSize, y * _tileSize), new Rectangle(8 * _tileSize, 0, _tileSize, _tileSize));
            } else {
                tile = new Tile("tiles32.png", new Vector2(x * _tileSize, y * _tileSize), new Rectangle(7 * _tileSize, 0, _tileSize, _tileSize), false);
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
