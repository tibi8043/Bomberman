using System.Data;
using System.Dynamic;

namespace Bombazo.Model {
    public class Enemy : IEntity {
        private Position _position;
        private bool _isDead;
        private List<Direction> _directions = new List<Direction>();
        private Direction _actualDirection;
        public bool IsEnemy => true;
        public bool IsPlayer => false;

        public Direction ActualDirection => _actualDirection;
        public List<Direction> Directions => _directions;

        public Position GetPosition => _position;

        internal Position SetPosition {
            set => _position = value;
        }

        public bool GetIsDead => _isDead;

        internal bool SetIsDead {
            set => _isDead = value;
        }

        public Enemy(Position position) {
            _directions.Add(Direction.UP);
            _directions.Add(Direction.DOWN);
            _directions.Add(Direction.LEFT);
            _directions.Add(Direction.RIGHT);
            ShuffleDirections();
            
            _position = position;
            _isDead = false;
            _actualDirection = _directions[0];
            _directions.RemoveAt(0);
        }

        public void Move(Direction dir) {
        }

        private void ShuffleDirections() {
                _directions = _directions.OrderBy(a => Guid.NewGuid()).ToList();
        }
        public void SetNewDirection() {
            if(_directions.Count == 0) {
                _directions.Add(Direction.UP);
                _directions.Add(Direction.DOWN);
                _directions.Add(Direction.LEFT);
                _directions.Add(Direction.RIGHT);
                ShuffleDirections();
            }
            _actualDirection = _directions[0];
            _directions.RemoveAt(0);
        }
    }
}