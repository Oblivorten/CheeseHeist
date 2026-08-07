using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CheeseHeist.Core;
using CheeseHeist.Systems;

namespace CheeseHeist.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _livesText;
        [SerializeField] private TMP_Text _catDistanceText;
        [SerializeField] private GameObject _pauseOverlay;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;

        private GameFlowSystem _gameFlow;
        private PlayerData _player;
        private CatData _cat;

        public void Initialize(GameEvents events, GameFlowSystem gameFlow, GameSessionData session, PlayerData player, CatData cat)
        {
            _gameFlow = gameFlow;
            _player = player;
            _cat = cat;

            events.OnScoreChanged += HandleScoreChanged;
            events.OnLivesChanged += HandleLivesChanged;
            events.OnGameStateChanged += HandleStateChanged;

            _pauseButton.onClick.AddListener(() => _gameFlow.TogglePause());
            _resumeButton.onClick.AddListener(() => _gameFlow.TogglePause());

            HandleScoreChanged(session.Score);
            HandleLivesChanged(session.Lives);
            _pauseOverlay.SetActive(false);
        }

        private void Update()
        {
            if (_player == null || _cat == null) return;

            float dx = _player.Position.X - _cat.Position.X;
            float dz = _player.Position.Z - _cat.Position.Z;
            float distance = Mathf.Sqrt(dx * dx + dz * dz);
            _catDistanceText.text = $"Cat: {distance:F1}m";
        }

        private void HandleScoreChanged(int score) => _scoreText.text = $"Score: {score}";
        private void HandleLivesChanged(int lives) => _livesText.text = $"Lives: {lives}";

        private void HandleStateChanged(GameState state)
        {
            _pauseOverlay.SetActive(state == GameState.Paused);
        }
    }
}