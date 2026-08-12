using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CheeseHeist.Core;
using CheeseHeist.Systems;

namespace CheeseHeist.UI
{
    public class ResultsScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _finalScoreText;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private GameObject _newRecordBadge;
        [SerializeField] private Button _restartButton;

        private GameFlowSystem _gameFlow;
        private GameSessionData _session;
        private bool _isNewRecordThisRun;

        public void Initialize(GameEvents events, GameFlowSystem gameFlow, GameSessionData session)
        {
            _gameFlow = gameFlow;
            _session = session;

            events.OnGameStateChanged += HandleStateChanged;
            events.OnNewHighScore += _ => _isNewRecordThisRun = true;
            _restartButton.onClick.AddListener(() => _gameFlow.RequestRestart());

            _panel.SetActive(false);
            if (_newRecordBadge != null) _newRecordBadge.SetActive(false);
        }

        private void HandleStateChanged(GameState state)
        {
            bool showResults = state == GameState.Results;
            _panel.SetActive(showResults);

            if (showResults)
            {
                _finalScoreText.text = $"Final score: {_session.Score}";
                _highScoreText.text = $"Best: {_session.HighScore}";

                if (_newRecordBadge != null)
                {
                    _newRecordBadge.SetActive(_isNewRecordThisRun);
                }
                _isNewRecordThisRun = false;
            }
        }
    }
}