namespace Bombazo.Model {
    public class Player : IEntity {
        private Position _position = new Position();
        private GameTable _gameTable;

        public Position GetPosition => _position;
        public bool IsEnemy => false;
        public bool IsPlayer => true;

        public Player(GameTable table) {
            _position.X = 0;
            _position.Y = 0;
            _gameTable = table;
            _gameTable.ForcedSetField(_position, FieldType.PLAYER);
        }

        public void Move(Direction direction) {
            var original = new Position(_position);

            var next = direction
                switch {
                    Direction.UP => new Position(--_position.X, _position.Y),
                    Direction.DOWN => new Position(++_position.X, _position.Y),
                    Direction.LEFT => new Position(_position.X, --_position.Y),
                    Direction.RIGHT => new Position(_position.X, ++_position.Y),
                    _ => throw new ArgumentException("Undefined enum constant", nameof(direction))
                };
            if (_gameTable.SetField(next, FieldType.PLAYER, this)) {
                if (_gameTable.PlayerIsCaught) {
                    _gameTable.SetField(original, FieldType.PATH, null);
                    throw new GameOverException();
                }
                else {
                    _gameTable.SetField(original, FieldType.PATH, null);
                }
            }
            else {
                _position = original;
            }
            
        }

        public void PlantBomb() {
            if (_gameTable.GetFieldType(_position) != FieldType.PLAYERANDBOMB) {
                Position copyPosition = new Position(_position);
                Bomb bomb = new Bomb(copyPosition, _gameTable);
                bomb.SetBomb();
            }
        }
    }
}