namespace Bombazo.Model {
    public class Position {
        private int _x;
        private int _y;

        public int X {
            get => _x;
            set { _x = value; }
        }

        public int Y {
            get => _y;
            set { _y = value; }
        }

        public Position() {
        }

        public Position(int x, int y) {
            _x = x;
            _y = y;
        }

        public Position(Position other) {
            _x = other._x;
            _y = other._y;
        }

        public override string ToString() {
            return _x + " " + _y;
        }
    }
}