using System.ComponentModel;

namespace NSU.Worm
{
    /// <summary>
    /// Червь кружится вокруг указанной точки. Если находится не в ней и не в её радиусе, ничего не делает.
    /// </summary>
    public class CirclingWormAI : WormAI
    {
        private readonly Position _circleCenter;

        public CirclingWormAI(Position circleCenter)
        {
            _circleCenter = circleCenter;
        }

        public CirclingWormAI(int circleCenterX, int circleCenterY)
        {
            _circleCenter = new Position(circleCenterX, circleCenterY);
        }

        public WormAction GetNextAction(Position position)
        {
            var circleRelativePosition = position.DirectionRelativeTo(_circleCenter);

            return circleRelativePosition switch
            {
                Direction.None => position == _circleCenter ? WormAction.MoveUp : WormAction.DoNothing,
                Direction.Up => WormAction.MoveRight,
                Direction.UpRight => WormAction.MoveDown,
                Direction.Right => WormAction.MoveDown,
                Direction.DownRight => WormAction.MoveLeft,
                Direction.Down => WormAction.MoveLeft,
                Direction.DownLeft => WormAction.MoveUp,
                Direction.Left => WormAction.MoveUp,
                Direction.UpLeft => WormAction.MoveRight,

                _ => throw new InvalidEnumArgumentException($"Unsupported circle position: {circleRelativePosition}")
            };
        }
    }
}