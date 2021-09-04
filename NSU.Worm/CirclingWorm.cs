using System.ComponentModel;

namespace NSU.Worm
{
    /// <summary>
    /// Червь, который кружится вокруг точки, в которой изначально появился.
    /// </summary>
    public class CirclingWorm : Worm
    {
        public CirclingWorm(string name, int xPosition, int yPosition) : base(name, xPosition, yPosition)
        {
            _circleCenter = new Position(xPosition, yPosition);
            _circleRelativePosition = Direction.None;
        }

        private Position _circleCenter;

        private Direction _circleRelativePosition;

        public override WormAction GetAction()
        {
            UpdateCircleRelativePosition();
            
            return _circleRelativePosition switch
            {
                Direction.None => WormAction.MoveUp,
                Direction.Up => WormAction.MoveRight,
                Direction.UpRight => WormAction.MoveDown,
                Direction.Right => WormAction.MoveDown,
                Direction.DownRight => WormAction.MoveLeft,
                Direction.Down => WormAction.MoveLeft,
                Direction.DownLeft => WormAction.MoveUp,
                Direction.Left => WormAction.MoveUp,
                Direction.UpLeft => WormAction.MoveRight,
                
                _ => throw new InvalidEnumArgumentException($"Unsupported circle position: {_circleRelativePosition}")
            };
        }

        private void UpdateCircleRelativePosition()
        {
            _circleRelativePosition = Position.DirectionRelativeTo(_circleCenter);
        }
    }
}