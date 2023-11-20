using System.Timers;

namespace Bombazo.Model {
    public class Bomb : IDisposable {
        private Position _position;
        private GameTable _gameTable;
        private int _counter;
        private bool _killedPlayer = false;
        private readonly System.Timers.Timer _bombTimer;

        public Position Position => _position;
        public GameTable GameTable => _gameTable;

        public bool KilledPlayer => _killedPlayer;

        public Bomb(Position position, GameTable gameTable) {
            _bombTimer = new System.Timers.Timer();
            _position = position;
            _gameTable = gameTable;
            _bombTimer.Enabled = true;
            _counter = 4;
        }

        public void SetBomb() {
            _gameTable.SetField(_position, FieldType.PLAYERANDBOMB, null);
            if (_bombTimer.Enabled) {
                _bombTimer.Interval = 1000;
                _bombTimer.Elapsed += ExplosionCountDown;
            }
        }

        private void ExplosionCountDown(object? sender, ElapsedEventArgs elapsedEventArgs) {
            _counter--;
            if (_counter == 1) {
                Explosion();

                if (_gameTable.GetFieldType(_position) == FieldType.PLAYERANDBOMB) {
                    _gameTable.SetPlayerIsDead = true;
                    throw new GameOverException();
                }
            }

            if (_counter == 0) {
                SetBackPath();
                Dispose();
            }
        }

        private void Explosion() {
            for (int i = 0; i <= 3; i++) {
                for (int j = 0; j <= 3; j++) {
                    Position a = new Position(_position.X + i, _position.Y + j);
                    Position b = new Position(_position.X - i, _position.Y - j);
                    Position c = new Position(_position.X + i, _position.Y - j);
                    Position d = new Position(_position.X - i, _position.Y + j);

                    _gameTable.SetField(a, FieldType.EXPLOSION, this);
                    _gameTable.SetField(b, FieldType.EXPLOSION, this);
                    _gameTable.SetField(c, FieldType.EXPLOSION, this);
                    _gameTable.SetField(d, FieldType.EXPLOSION, this);
                }
            }
        }

        private void SetBackPath() {
            for (int i = 0; i <= 3; i++) {
                for (int j = 0; j <= 3; j++) {
                    _gameTable.SetField(new Position(_position.X + i, _position.Y + j), FieldType.PATH, this);
                    _gameTable.SetField(new Position(_position.X - i, _position.Y - j), FieldType.PATH, this);
                    _gameTable.SetField(new Position(_position.X + i, _position.Y - j), FieldType.PATH, this);
                    _gameTable.SetField(new Position(_position.X - i, _position.Y + j), FieldType.PATH, this);
                }
            }

            _gameTable.ForcedSetField(_position, FieldType.PATH);
            _bombTimer.Dispose();
        }

        public void Dispose() {
            _bombTimer.Stop();
        }
    }
}