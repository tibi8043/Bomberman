using Bombazo.Model;

namespace Bombazo.Presistance {
    public class GameMap {
        public List<Position> Walls = new List<Position>();
        public List<Position> Enemies = new List<Position>();
        public int Size;
    }
}