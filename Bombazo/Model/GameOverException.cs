namespace Bombazo.Model {
    public class GameOverException : Exception {
        public GameOverException() : base("Vége a játéknak") {
        }
    }
}