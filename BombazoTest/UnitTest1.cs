using Bombazo.Model;

namespace BombazoTest {
    [TestClass]
    public class UnitTest1 {
        private GameModel _gameModel = new GameModel();

        [TestInitialize]
        public void TestInitialize() {
            string path = "TestMap.txt";
            _gameModel = GameModel.GameModelFactory(path).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void  IsTableInitialized() {
            Assert.IsFalse(_gameModel.GameTable is null);
        }
        
        [TestMethod]
        public void IsGameOver() {
            Assert.AreEqual(_gameModel.IsGameOver(), false);
        }

        [TestMethod]
        public void CheckPlayerInit() {
            Assert.AreEqual(_gameModel.Player?.Position.X, 0);
            Assert.AreEqual(_gameModel.Player?.Position.Y, 0);
        }

        [TestMethod]
        public void CheckEnemiesInit() {
            Assert.AreEqual(_gameModel.GameTable?.EnemyList.Count, 2);
        }

        [TestMethod]
        public void CheckPlayerAndBombInit() {
            _gameModel.Player?.PlantBomb();
            if (_gameModel.Player?.Position != null)
                Assert.AreEqual(_gameModel.GameTable?.GetFieldType(_gameModel.Player.Position),
                    FieldType.PLAYERANDBOMB);
        }

        [TestMethod]
        public void CheckBombInit() {
            _gameModel.Player?.PlantBomb();
            _gameModel.Player?.Move(Direction.RIGHT);
            Assert.AreEqual(_gameModel.GameTable?.GetFieldType(new Position(0, 0)), FieldType.BOMB);
        }

        [TestMethod]
        public void DieInExplosion() {
            _gameModel.GameTable?.ForcedSetField(new Position(0, 1), FieldType.EXPLOSION);
            _gameModel.Player?.Move(Direction.RIGHT);
            Assert.IsTrue(_gameModel.IsGameOver());
            Assert.IsTrue(_gameModel.OverReason == GameOverType.DEAD);
            Assert.IsTrue(_gameModel.GameTable?.PlayerIsDead);
        }

        [TestMethod]
        public void KillEnemyInExplosion() {
            _gameModel.GameTable?.SetField(new Position(2, 9), FieldType.EXPLOSION,
                new Bomb(new Position(2, 9), _gameModel.GameTable));
            Assert.IsTrue(_gameModel.KilledEnemiesCount() >= 1);
        }

        [TestMethod]
        public void WinGame() {
            _gameModel.GameTable?.SetField(new Position(2, 9), FieldType.EXPLOSION,
                new Bomb(new Position(2, 9), _gameModel.GameTable));
            _gameModel.GameTable?.SetField(new Position(6, 2), FieldType.EXPLOSION,
                new Bomb(new Position(6, 2), _gameModel.GameTable));
            Assert.IsTrue(_gameModel.KilledEnemiesCount() == _gameModel.GameTable?.EnemyList.Count);
            Assert.IsTrue(_gameModel.IsGameOver());
            Assert.IsTrue(_gameModel.OverReason == GameOverType.WIN);
        }

        [TestMethod]
        public void LoseGame() {
            _gameModel.GameTable?.SetField(new Position(2, 9), FieldType.PLAYER, _gameModel.Player);
            Assert.IsTrue(_gameModel.IsGameOver());
            Assert.IsTrue(_gameModel.OverReason == GameOverType.LOSE);
        }
    }
}