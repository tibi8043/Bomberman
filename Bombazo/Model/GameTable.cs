namespace Bombazo.Model {
    public class GameTable {
        private Field[,] _table;
        private readonly List<Enemy> _enemyList = new List<Enemy>();
        private bool _playerIsCaught = false;
        private bool _playerIsDead = false;
        private int _mapSize;
        public List<Enemy> EnemyList => _enemyList;
        public bool PlayerIsCaught => _playerIsCaught;

        public bool GetPlayerIsDead => _playerIsDead;

        internal bool SetPlayerIsDead {
            set => _playerIsDead = value;
        }

        public int MapSize => _mapSize;
        public Field[,] Table => _table;

        public GameTable(int size) {
            this._mapSize = size;
            this._table = new Field[_mapSize, _mapSize];
            FillMapWithPath();
        }

        public void MoveEnemy(Enemy enemy) {
            var original = new Position(enemy.GetPosition);
            Position next = enemy.Direction
                switch {
                    Direction.UP => new Position(enemy.GetPosition.X, ++enemy.GetPosition.Y),
                    Direction.DOWN => new Position(enemy.GetPosition.X, --enemy.GetPosition.Y),
                    Direction.LEFT => new Position(--enemy.GetPosition.X, enemy.GetPosition.Y),
                    Direction.RIGHT => new Position(++enemy.GetPosition.X, enemy.GetPosition.Y),
                    _ => throw new ArgumentException("Undefined enum constant")
                };
            if (!enemy.GetIsDead) {
                if (SetField(next, FieldType.ENEMY, enemy)) {
                    if (GetFieldType(next) == FieldType.EXPLOSION) {
                        enemy.SetIsDead = true;
                    }

                    if (PlayerIsCaught) {
                        SetField(original, FieldType.PATH, null);
                        throw new GameOverException();
                    }
                    else {
                        SetField(original, FieldType.PATH, null);
                    }
                }
                else {
                    enemy.SetPosition = original;
                    enemy.GetNewDirection();
                    MoveEnemy(enemy);
                }
            }
        }

        public void MoveEnemies() {
            _enemyList.ForEach((enemy) => {
                if (!enemy.GetIsDead) {
                    MoveEnemy(enemy);
                }
            });
        }

        public bool SetField(Position position, FieldType fieldType, Object? caller) {
            if (position.Y < 0 || position.Y > _mapSize - 1) {
                return false;
            }

            if (position.X < 0 || position.X > _mapSize - 1) {
                return false;
            }

            if (GetFieldType(position) == FieldType.WALL) {
                return false;
            }

            if (GetFieldType(position) == FieldType.BOMB) {
                return false;
            }

            if (caller is Enemy && GetFieldType(position) == FieldType.ENEMY) {
                return false;
            }

            if (caller is Player) {
                _playerIsDead = GetFieldType(position) == FieldType.EXPLOSION;
                if (_playerIsDead) {
                    return false;
                }

                _playerIsCaught = GetFieldType(position) == FieldType.ENEMY;
                if (_playerIsCaught) {
                    return false;
                }
            }

            if (caller is Enemy) {
                _playerIsCaught = GetFieldType(position) == FieldType.PLAYER;
                if (GetFieldType(position) == FieldType.EXPLOSION) {
                    foreach (Enemy enemy in _enemyList) {
                        if (enemy.GetPosition == position) {
                            enemy.SetIsDead = true;
                        }
                    }
                }
            }

            //azért kell hogy ha felrobban az egyik bomba és a másikon állok akkor ne haljak meg
            if (caller is Bomb && GetFieldType(position) == FieldType.PLAYERANDBOMB) {
                return false;
            }

            if (GetFieldType(position) == FieldType.PLAYERANDBOMB) {
                ForcedSetField(position, FieldType.BOMB);
                return false;
            }

            //Ha körülötte áll, meghal a player
            if (caller is Bomb && GetFieldType(position) == FieldType.PLAYER) {
                _playerIsDead = true;
                return false;
            }

            if (caller is Bomb && GetFieldType(position) == FieldType.ENEMY && fieldType == FieldType.EXPLOSION) {
                foreach (Enemy enemy in EnemyList) {
                    if (enemy.GetPosition.X == position.X && enemy.GetPosition.Y == position.Y) {
                        enemy.SetIsDead = true;
                    }
                }
            }

            _table[position.X, position.Y] = new Field(fieldType);

            return true;
        }

        private void FillMapWithPath() {
            for (int i = 0; i < _mapSize; i++) {
                for (int j = 0; j < _mapSize; j++) {
                    _table[i, j] = new Field(FieldType.PATH);
                }
            }
        }

        public bool ForcedSetField(Position position, FieldType fieldType) {
            if (position.Y < 0 || position.Y > _mapSize - 1) {
                return false;
            }

            if (position.X < 0 || position.X > _mapSize - 1) {
                return false;
            }

            _table[position.X, position.Y] = new Field(fieldType);
            return true;
        }

        public FieldType GetFieldType(Position position) {
            return _table[position.X, position.Y].FieldType;
        }
    }
}