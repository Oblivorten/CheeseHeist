using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class PlayerInputRouter : IMoveInputProvider
    {
        private IMoveInputProvider _currentProvider;

        public float Horizontal =>
            _currentProvider != null ? _currentProvider.Horizontal : 0f;

        public float Vertical =>
            _currentProvider != null ? _currentProvider.Vertical : 0f;

        public void SetProvider(IMoveInputProvider provider)
        {
            _currentProvider = provider;
        }
    }
}