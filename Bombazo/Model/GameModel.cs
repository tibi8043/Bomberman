using System.Timers;
using Bombazo.Presistence;

namespace Bombazo.Model {
    public class GameModel : IDisposable {
        private Player? _player;
        private GameTable? _gameTable;
        private bool _gameOver;
        private GameOverType? _overReason;
        private bool _paused;
        private int _elapsedSeconds;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public System.Timers.Timer Timer => _timer;
        public GameTable? GameTable => _gameTable;
        public Player? Player => _player;
        public GameOverType? OverReason => _overReason;
        public bool GameOver => _gameOver;
        public int ElapsedSeconds => _elapsedSeconds;

        public int KilledEnemiesCount() {
            if (_gameTable is not null) {
                return _gameTable.EnemyList.Count(enemy => enemy.GetIsDead);
            }

            return 0;
        }

        public bool GameIsPaused => _paused;

        public GameModel() {
            _timer.Elapsed += Tick;
        }

        public static async Task<GameModel> GameModelFactory(string path) {
            GameModel model = new GameModel();
            GameMap game = await LoadGame.LoadAsync(path);
            Position pos;
            model._overReason = null;
            model._gameTable = new GameTable(game.Size);
            model._player = new Player(model._gameTable);

            for (int i = 0; i < game.Walls.Count(); i++) {
                pos = game.Walls[i];
                model._gameTable.ForcedSetField(pos, FieldType.WALL);
            }

            for (int i = 0; i < game.Enemies.Count(); i++) {
                pos = game.Enemies[i];
                model._gameTable.EnemyList.Add(new Enemy(pos));
                model._gameTable.ForcedSetField(pos, FieldType.ENEMY);
            }

            model._timer.Interval = 1000;
            model._timer.Start();
            return model;
        }

        private void Tick(object? sender, ElapsedEventArgs args) {
            MoveGuardsOnTick();
            if (!_gameOver) {
                _elapsedSeconds++;
            }
        }

        private void MoveGuardsOnTick() {
            try {
                if (_gameTable != null && !IsGameOver() && !_paused) {
                    _gameTable.MoveEnemies();
                }
            }
            catch (GameOverException) {
                _overReason = GameOverType.LOSE;
                _gameOver = true;
            }
        }

        public void PlayerInteract(UserInput input) {
            if (_player is null) {
                return;
            }

            if (!IsGameOver() && !_paused) {
                try {
                    switch (input) {
                        case UserInput.UP:
                            _player.Move(Direction.UP);
                            break;
                        case UserInput.DOWN:
                            _player.Move(Direction.DOWN);
                            break;
                        case UserInput.LEFT:
                            _player.Move(Direction.LEFT);
                            break;
                        case UserInput.RIGHT:
                            _player.Move(Direction.RIGHT);
                            break;
                        case UserInput.PLANT:
                            _player.PlantBomb();
                            break;
                    }
                }
                catch (GameOverException) {
                    _overReason = GameOverType.LOSE;
                    _gameOver = true;
                }
            }
        }

        public bool IsGameOver() {
            if (_gameTable is null) return false;
            //Felrobbantotta magát            
            if (_gameTable.GetPlayerIsDead) {
                _overReason = GameOverType.DEAD;
                _gameOver = true;
                Dispose();
                return true;
            }

            //üres az enemies lista ezért meghaltak
            if (_gameTable.EnemyList.TrueForAll(enemy => enemy.GetIsDead)) {
                _overReason = GameOverType.WIN;
                _gameOver = true;
                Dispose();
                return true;
            }

            //Elkapták
            if (_gameTable.PlayerIsCaught) {
                _overReason = GameOverType.LOSE;
                _gameOver = true;
                Dispose();
                return true;
            }

            return false;
        }

        public void PauseGame() {
            if (!_gameOver && CanBePaused()) {
                _paused = !_paused;
            }
        }

        private bool CanBePaused() {
            if (_gameTable is null) return false;
            foreach (var item in _gameTable.Table) {
                if (item.IsExplosive()) {
                    return false;
                }
            }

            return true;
        }

        public void Dispose() {
            _timer.Close();
        }
    }
}