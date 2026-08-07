using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private SceneReferences _refs;
        [SerializeField] private InputSource _activeInputSource = InputSource.Keyboard;
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private TrailConfig _trailConfig;
        [SerializeField] private SkidConfig _skidConfig;
        [SerializeField] private SlowdownConfig _slowdownConfig;
        [SerializeField] private LivesConfig _livesConfig;
        [SerializeField] private CatConfig _catConfig;
        [SerializeField] private CheeseConfig _cheeseConfig;
        [SerializeField] private DifficultyConfig _difficultyConfig;
        [SerializeField] private LevelSpawnConfig _levelSpawnConfig;
        [SerializeField] private IdleConfig _idleConfig;

        private GameContext _context;

        private void Awake()
        {
            var events = new GameEvents();

            _refs.LevelSpawner.Initialize(events, _levelSpawnConfig, _cheeseConfig, _refs.PlayerBody.transform);
            _refs.LevelSpawner.SpawnInitialLevel();

            var bootstrap = new Bootstrap();
            _context = bootstrap.CreateGame(
                events, _refs, _activeInputSource, _movementConfig, _trailConfig, _skidConfig, _slowdownConfig,
                _livesConfig, _catConfig, _difficultyConfig, _idleConfig);

            _refs.HUD.Initialize(_context.Events, _context.GameFlow, _context.Session, _context.Player, _context.Cat);
            _refs.ResultsScreen.Initialize(_context.Events, _context.GameFlow, _context.Session);
            _refs.MainMenu.Initialize(_context.Events, _context.GameFlow);
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            _refs.PlayerBody.SyncPositionToData();
            _context.Loop.Tick(dt);
            _refs.PlayerBody.ApplyVelocity();
            _refs.TrailView.Sync(_context.TrailSystem.Segments);
            _refs.CatBody.Sync(_context.Cat);
            _refs.LevelSpawner.TickCheeseSpawner(dt);
        }
    }
}