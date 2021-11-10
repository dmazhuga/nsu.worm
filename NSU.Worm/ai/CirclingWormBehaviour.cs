using System;
using System.ComponentModel;

namespace NSU.Worm
{
    /// <summary>
    /// Червь кружится вокруг указанной точки. Если находится не в ней и не в её радиусе, ничего не делает.
    /// </summary>
    public class CirclingWormBehaviour : IWormBehaviour
    {
        private const int ReproductionLifeRequirement = 15;
        
        private readonly Position _circleCenter;

        public CirclingWormBehaviour(Worm worm, Position circleCenter)
        {
            WormName = worm.Name;
            _circleCenter = circleCenter;
        }

        public CirclingWormBehaviour(Worm worm, int circleCenterX, int circleCenterY)
        {
            WormName = worm.Name;
            _circleCenter = new Position(circleCenterX, circleCenterY);
        }

        public string WormName { get; }

        public WormAction GetAction(IWorldState worldState)
        {
            var worm = worldState.GetWorm(WormName);

            if (worm is null)
            {
                throw new ArgumentException($"Provided state has no worm for name {WormName}");
            }
            
            var position = worm.Position;
            var life = worm.Life;
            
            var circleRelativePosition = position.DirectionRelativeTo(_circleCenter);

            if (life >= ReproductionLifeRequirement)
            {
                return circleRelativePosition switch
                {
                    Direction.None => WormAction.ReproduceUp,
                    Direction.Up => WormAction.ReproduceUp,
                    Direction.UpRight => WormAction.ReproduceUp,
                    Direction.Right => WormAction.ReproduceRight,
                    Direction.DownRight => WormAction.ReproduceRight,
                    Direction.Down => WormAction.ReproduceDown,
                    Direction.DownLeft => WormAction.ReproduceDown,
                    Direction.Left => WormAction.ReproduceLeft,
                    Direction.UpLeft => WormAction.ReproduceLeft,
                    
                    _ => throw new InvalidEnumArgumentException(
                        $"Unsupported circle position: {circleRelativePosition}")
                };
            } 

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

        public IWormBehaviour CopyForWorm(Worm worm)
        {
            return new CirclingWormBehaviour(worm, worm.Position);
        }
    }
}