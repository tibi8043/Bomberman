using Bombazo.Model;

namespace Bombazo.Presistence {
    public class GameMap {
        private List<Position> _walls = new List<Position>();
        private List<Position> _enemies = new List<Position>();
        private int _size;

        public List<Position> Walls => _walls;
        public List<Position> Enemies => _enemies;

        public int Size {
            get => _size;
            set => _size = value;
        }
    }
}