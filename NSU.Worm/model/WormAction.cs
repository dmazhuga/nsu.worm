namespace NSU.Worm
{
    public class WormAction
    {
        public static readonly WormAction MoveLeft = new(ActionType.Move, Direction.Left);
        public static readonly WormAction MoveRight = new(ActionType.Move, Direction.Right);
        public static readonly WormAction MoveUp = new(ActionType.Move, Direction.Up);
        public static readonly WormAction MoveDown = new(ActionType.Move, Direction.Down);

        public static readonly WormAction ReproduceLeft = new(ActionType.Reproduce, Direction.Left);
        public static readonly WormAction ReproduceRight = new(ActionType.Reproduce, Direction.Right);
        public static readonly WormAction ReproduceUp = new(ActionType.Reproduce, Direction.Up);
        public static readonly WormAction ReproduceDown = new(ActionType.Reproduce, Direction.Down);

        public static readonly WormAction DoNothing = new(ActionType.DoNothing, Direction.None);

        private WormAction(ActionType type, Direction direction)
        {
            Direction = direction;
            Type = type;
        }

        public Direction Direction { get; }

        public ActionType Type { get; }

        public enum ActionType
        {
            Move,
            Reproduce,
            DoNothing
        }
    }
}