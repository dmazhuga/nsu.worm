namespace NSU.Worm
{
    public class WormAction
    {
        public static readonly WormAction MoveLeft = new WormAction(ActionType.Move, Direction.Left);
        public static readonly WormAction MoveRight = new WormAction(ActionType.Move, Direction.Right);
        public static readonly WormAction MoveUp = new WormAction(ActionType.Move, Direction.Up);
        public static readonly WormAction MoveDown = new WormAction(ActionType.Move, Direction.Down);
        public static readonly WormAction DoNothing = new WormAction(ActionType.DoNothing, Direction.None);
        
        private WormAction(ActionType type, Direction direction)
        {
            Direction = direction;
            Type = type;
        }
        
        public Direction Direction { get; }
        
        public ActionType Type { get; }
        
        public enum ActionType
        {
            Move, DoNothing
        }
    }
}