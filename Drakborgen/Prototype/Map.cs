using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Gengine.Entities;
using Gengine.Map;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class Map : ICollidableMap {
        private readonly int _width;
        private readonly int _height;
        private readonly int _tileSize;
        private readonly List<Tile> _tiles;
        public Tile[,] Tiles { get; set; }
        public int TileSize { get { return _tileSize; } }

        public Map(int width, int height, int tileSize){
            _width = width;
            _height = height;
            _tileSize = tileSize;
            _tiles = new List<Tile>((_width * _height) / _tileSize);

            CreateTiles();
        }

        public void CreateTiles() {
            int tileCountX = _width / _tileSize + 1;
            int tileCountY = _height / _tileSize + 1;

            Tiles = new Tile[tileCountX, tileCountY];

            for (int x = 0;x < tileCountX;x++) {
                for (int y = 0;y < tileCountY;y++) {
                    if (y == 0 || x == 0 || x*_tileSize == _width - _tileSize || y > tileCountY - 2){
                        var tile = new Tile("tiles32.png", new Vector2(x*_tileSize, y*_tileSize), new Rectangle(7*_tileSize, 0, _tileSize, _tileSize));
                        _tiles.Add(tile);
                        Tiles[x, y] = tile;
                    }
                    else{
                        var tile = new Tile("tiles32.png", new Vector2(x*_tileSize, y*_tileSize), new Rectangle(8*_tileSize, 0, _tileSize, _tileSize), false);
                        _tiles.Add(tile);
                        Tiles[x, y] = tile;
                    }
                }
            }

            SetFaces();
        }

        private void SetFaces() {
            int tileCountX = _width / _tileSize + 1;
            int tileCountY = _height / _tileSize + 1;
            for (int x = 0; x < tileCountX; x++){
                for (int y = 0; y < tileCountY; y++){
                    var tile = Tiles[x, y];
                    if (tile.IsSolid) {
                        if (x - 1 > 0 && !Tiles[x - 1, y].IsSolid){
                            tile.FaceLeft = true;
                        }
                        if (x + 1 < tileCountX && !Tiles[x + 1, y].IsSolid) {
                            tile.FaceRight = true;
                        }
                        if (y - 1 > 0 && !Tiles[x, y - 1].IsSolid) {
                            tile.FaceTop = true;
                        }
                        if (y + 1 < tileCountY && !Tiles[x, y + 1].IsSolid) {
                            tile.FaceBottom = true;
                        }
                    }
                }
            }
        }

        public IEnumerable<IRenderable> RenderTiles(){
            return _tiles;
        }

        public Tile Tile(int x, int y){
            return Tiles[x,y];
        }

        public void ClearDebug(){
            _tiles.ForEach(t => t.DebugDraw = false);
        }

        public Tile PositionToTile(float x, float y) {
            var tileX = (int)(x / _tileSize);
            var tileY = (int)(y / _tileSize);
            return Tiles[tileX, tileY];
        }

        public void CreateCollisionLayer() {
            int tileCountX = _width / _tileSize;
            int tileCountY = _height / _tileSize;

            Tiles = new Tile[tileCountX, tileCountY];

            for (int x = 0;x < tileCountX;x++) {
                for (int y = 0;y < tileCountY;y++) {
                    Tile tile = _tiles.FirstOrDefault(t => t.Position.X == x * _tileSize && t.Position.Y == y * _tileSize);
                    if (tile == null) {
                        Tiles[x, y] = new Tile("tiles32.png", new Vector2(x, y), new Rectangle(0, 0, _tileSize, _tileSize), false);
                    } else {
                        Tiles[x, y] = new Tile(tile.TextureName, new Vector2(x, y), tile.Position, tile.SourceRectangle);
                    }
                }
            }
        }
    }

    //public class Tile : IRenderable, ICollidable {
    //    public Tile(string textureName, Vector2 renderPosition, Rectangle sourceRectangle, bool isSolid = false) {
    //        TextureName = textureName;
    //        RenderPosition = renderPosition;
    //        SourceRectangle = sourceRectangle;
    //        IsSolid = isSolid;
    //        BoundingBox = new Rectangle((int)renderPosition.X, (int)renderPosition.Y, sourceRectangle.Width, sourceRectangle.Height);
    //    }

    //    public bool IsSolid { get; set; }
    //    public string TextureName { get; private set; }
    //    public Vector2 RenderPosition { get; private set; }
    //    public Rectangle SourceRectangle { get; private set; }
    //    public Rectangle BoundingBox { get; private set; }
    //}
}
