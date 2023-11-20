namespace Bombazo.Model {
    public enum Direction {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public interface IEntity {
        Position GetPosition { get; }
        bool IsPlayer { get; }
        bool IsEnemy { get; }
        void Move(Direction direction);
    }
}