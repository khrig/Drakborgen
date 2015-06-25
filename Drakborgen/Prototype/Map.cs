using System.Collections.Generic;
using System.Linq;
using Gengine.Entities;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class Map {
        private readonly int _width;
        private readonly int _height;
        private readonly int _tileSize;
        private List<Tile> _tiles;

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
            
            for (int x = 0;x < tileCountX;x++) {
                for (int y = 0;y < tileCountY;y++) {
                    if (y == 0 || x == 0 || x * _tileSize == _width - _tileSize || y > tileCountY - 2)
                        _tiles.Add(new Tile("tiles32.png", new Vector2(x * _tileSize, y * _tileSize), new Rectangle(7 * _tileSize, 0, _tileSize, _tileSize)));
                    else
                        _tiles.Add(new Tile("tiles32.png", new Vector2(x * _tileSize, y * _tileSize), new Rectangle(8 * _tileSize, 0, _tileSize, _tileSize)));
                }
            }
        }

        public IEnumerable<IRenderable> RenderTiles(){
            return _tiles;
        }

        public Tile GetTile(int x, int y){
            return _tiles.FirstOrDefault(t => t.RenderPosition.X == x && t.RenderPosition.Y == y);
        }
    }

    public class Tile : IRenderable, ICollidable {
        public Tile(string textureName, Vector2 renderPosition, Rectangle sourceRectangle, bool isSolid = false) {
            TextureName = textureName;
            RenderPosition = renderPosition;
            SourceRectangle = sourceRectangle;
            IsSolid = isSolid;
            BoundingBox = new Rectangle((int)renderPosition.X, (int)renderPosition.Y, sourceRectangle.Width, sourceRectangle.Height);
        }

        public bool IsSolid { get; set; }
        public string TextureName { get; private set; }
        public Vector2 RenderPosition { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public Rectangle BoundingBox { get; private set; }
    }
}
