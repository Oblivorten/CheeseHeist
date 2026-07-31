using UnityEngine;

namespace CheeseHeist.Adapters
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private SceneReferences _refs;
        [SerializeField] private InputSource _activeInputSource = InputSource.Keyboard;
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private TrailConfig _trailConfig;

        private GameContext _context;

        private void Awake()
        {
            var bootstrap = new Bootstrap();
            _context = bootstrap.CreateGame(_refs, _activeInputSource, _movementConfig, _trailConfig);
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            _refs.PlayerBody.SyncPositionToData();
            _context.Loop.Tick(dt);
            _refs.PlayerBody.ApplyVelocity();
            _refs.TrailView.Sync(_context.TrailSystem.Segments);
        }
    }
}