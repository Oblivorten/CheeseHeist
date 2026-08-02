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
        [SerializeField] private GameObject _pauseOverlay;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;

        private GameFlowSystem _gameFlow;

        public void Initialize(GameEvents events, GameFlowSystem gameFlow, GameSessionData session)
        {
            _gameFlow = gameFlow;

            events.OnScoreChanged += HandleScoreChanged;
            events.OnLivesChanged += HandleLivesChanged;
            events.OnGameStateChanged += HandleStateChanged;

            _pauseButton.onClick.AddListener(() => _gameFlow.TogglePause());
            _resumeButton.onClick.AddListener(() => _gameFlow.TogglePause());

            HandleScoreChanged(session.Score);
            HandleLivesChanged(session.Lives);
            _pauseOverlay.SetActive(false);
        }

        private void HandleScoreChanged(int score) => _scoreText.text = $"Score: {score}";
        private void HandleLivesChanged(int lives) => _livesText.text = $"Lives: {lives}";

        private void HandleStateChanged(GameState state)
        {
            _pauseOverlay.SetActive(state == GameState.Paused);
        }
    }
}