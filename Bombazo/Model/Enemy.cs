namespace Bombazo.Model {
    public class Enemy : IEntity {
        private Position _position;
        private readonly Random _numDirection = new Random();
        private bool _isDead;
        private Direction _direction;
        public bool IsEnemy => true;
        public bool IsPlayer => false;

        public Direction Direction => _direction;

        public Position GetPosition => _position;

        internal Position SetPosition {
            set => _position = value;
        }

        public bool GetIsDead => _isDead;

        internal bool SetIsDead {
            set => _isDead = value;
        }

        public Enemy(Position position) {
            _position = position;
            _isDead = false;
            _direction = (Direction)_numDirection.Next(0, 4);
        }

        public void Move(Direction dir) {
        }

        public void GetNewDirection() {
            _direction = (Direction)_numDirection.Next(0, 4);
        }
    }
}