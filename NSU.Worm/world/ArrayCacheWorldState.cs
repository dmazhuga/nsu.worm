using System.Collections.Generic;
using System.Text;

namespace NSU.Worm
{
    public class ArrayCacheWorldState : BaseWorldState
    {
        private const int LeftBorder = -10;
        private const int RightBorder = 10;
        private const int TopBorder = 10;
        private const int BottomBorder = -10;

        private const int XSize = -LeftBorder + RightBorder + 1;
        private const int YSize = -BottomBorder + TopBorder + 1;

        private readonly byte[,] _map = new byte[XSize, YSize];

        public ArrayCacheWorldState(List<Worm> worms) : base(worms)
        {
        }

        public override void Move(Worm worm, Position position)
        {
            var oldPosition = worm.Position;

            base.Move(worm, position);

            if (InBorders(position))
            {
                _map[ConvertX(position.X), ConvertY(position.Y)] = (byte) WorldState.Tile.Worm;
            }

            if (InBorders(oldPosition))
            {
                _map[ConvertX(oldPosition.X), ConvertY(oldPosition.Y)] = (byte) WorldState.Tile.Empty;
            }
        }

        public override void Put(Worm worm, Position position)
        {
            base.Put(worm, position);

            if (InBorders(position))
            {
                _map[ConvertX(position.X), ConvertY(position.Y)] = (byte) WorldState.Tile.Worm;
            }
        }

        public override void Remove(Worm worm)
        {
            base.Remove(worm);

            if (InBorders(worm.Position))
            {
                _map[ConvertX(worm.Position.X), ConvertY(worm.Position.Y)] = (byte) WorldState.Tile.Empty;
            }
        }

        public override WorldState.Tile Get(Position position)
        {
            if (InBorders(position))
            {
                return (WorldState.Tile) _map[ConvertX(position.X), ConvertY(position.Y)];
            }

            return base.Get(position);
        }

        private bool InBorders(Position position)
        {
            return position.X >= LeftBorder && position.X <= RightBorder &&
                   position.Y >= BottomBorder && position.Y <= TopBorder;
        }

        private int ConvertX(int x)
        {
            return x - LeftBorder;
        }

        private int ConvertY(int y)
        {
            return y - BottomBorder;
        }
    }
}