using System.Collections.Generic;

namespace NSU.Worm
{
    public class WormBehaviourProvider : IWormBehaviourProvider
    {
        private readonly Dictionary<string, IWormBehaviour> _behaviours;

        public WormBehaviourProvider()
        {
            _behaviours = new Dictionary<string, IWormBehaviour>();
        }

        public IWormBehaviour GetBehaviour(Worm worm)
        {
            return _behaviours[worm.Name];
        }

        public void RegisterBehaviour(Worm worm, IWormBehaviour behaviour)
        {
            _behaviours.Add(worm.Name, behaviour);
        }

        public void DeleteBehaviour(Worm worm)
        {
            _behaviours.Remove(worm.Name);
        }
    }
}