using System.Collections.Generic;
using Gengine.DungeonGenerators;
using Gengine.Rendering;
using Gengine.UI;
using Microsoft.Xna.Framework;

namespace Drakborgen.Prototype {
    public class MiniMap{
        private readonly string _texture;
        private readonly Vector2 _miniMapRenderPosition;
        private readonly int _tileSize;
        private DungeonMap _dungeonMap;
        private readonly int _halfMiniMapSize;
        private readonly List<IRenderable> _renderableTiles;

        public MiniMap(int x, int y, int tileSize, string texture, int padding){
            _halfMiniMapSize = padding;
            _miniMapRenderPosition = new Vector2(x,y);
            _tileSize = tileSize;
            _texture = texture;
            _renderableTiles = new List<IRenderable>();
        }

        public void SetMap(DungeonMap dungeonMap){
            _dungeonMap = dungeonMap;
        }

        public IEnumerable<IRenderable> GetRenderables(){
            return _renderableTiles;
        }

        public void UpdateMiniMap(int currentX, int currentY){
            _renderableTiles.Clear();
            CreateSpriteRepresentation(currentX, currentY);
        }

        private void CreateSpriteRepresentation(int currentX, int currentY) {
            int yMin = currentY - _halfMiniMapSize >= 0 ? currentY - _halfMiniMapSize : 0;
            int yMax = currentY + _halfMiniMapSize < _dungeonMap.Height ? currentY + _halfMiniMapSize : _dungeonMap.Height - 1;
            int xMin = currentX - _halfMiniMapSize >= 0 ? currentX - _halfMiniMapSize : 0;
            int xMax = currentX + _halfMiniMapSize < _dungeonMap.Width ? currentX + _halfMiniMapSize : _dungeonMap.Width - 1;
            for (int y = yMin, minimapY = 0;y <= yMax;y++, minimapY++) {
                for (int x = xMin, minimapX = 0;x <= xMax;x++, minimapX++) {
                    if (_dungeonMap[x, y].IsLegitRoom){
                        UpdateRoomType(x, y, currentX, currentY);
                        _renderableTiles.Add(CreateRenderableRoom(_dungeonMap[x, y], _miniMapRenderPosition.X + minimapX * _tileSize, _miniMapRenderPosition.Y + minimapY * _tileSize));
                    }
                }
            }
        }

        private void UpdateRoomType(int x, int y, int currentX, int currentY) {
            Room room = _dungeonMap[x, y];
            if (x == currentX && y == currentY)
                room.Type = room.Type != RoomType.Start && room.Type != RoomType.Boss ? RoomType.Current : room.Type;
            else
                room.Type = room.Type == RoomType.Current ? room.Type = RoomType.VisitedRoom : room.Type;
        }

        private IRenderable CreateRenderableRoom(Room room, float xPos, float yPos) {
            return new SpriteNode(_texture, new Vector2(xPos, yPos), GetTextureSourceRectangle(room.Type));
        }

        private static Rectangle GetTextureSourceRectangle(RoomType type) {
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
                    return new Rectangle(0, 0, 0, 0);
            }
        }
    }
}
