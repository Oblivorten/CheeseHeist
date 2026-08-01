using UnityEngine;

namespace CheeseHeist.Adapters
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private SceneReferences _refs;
        [SerializeField] private InputSource _activeInputSource = InputSource.Keyboard;
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private TrailConfig _trailConfig;
        [SerializeField] private SkidConfig _skidConfig;
        [SerializeField] private LivesConfig _livesConfig;
        [SerializeField] private CatConfig _catConfig;
        [SerializeField] private CheeseConfig _cheeseConfig;
        [SerializeField] private DifficultyConfig _difficultyConfig;

        private GameContext _context;

        private void Awake()
        {
            var bootstrap = new Bootstrap();
            _context = bootstrap.CreateGame(
                _refs, _activeInputSource, _movementConfig, _trailConfig, _skidConfig, _livesConfig, _catConfig, _cheeseConfig, _difficultyConfig);

            _context.Events.OnLivesChanged += lives => Debug.Log($"Lives: {lives}");
            _context.Events.OnGameOver += () => Debug.Log("GAME OVER (lives)");
            _context.Events.OnCatCaught += () => Debug.Log("CAUGHT BY CAT");
            _context.Events.OnScoreChanged += score => Debug.Log($"Score: {score}");
            _context.Events.OnDifficultyChanged += m => Debug.Log($"Difficulty: {m:F2}x");
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            _refs.PlayerBody.SyncPositionToData();
            _context.Loop.Tick(dt);
            _refs.PlayerBody.ApplyVelocity();
            _refs.TrailView.Sync(_context.TrailSystem.Segments);
            _refs.CatBody.Sync(_context.Cat);
        }
    }
}