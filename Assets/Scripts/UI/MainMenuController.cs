using UnityEngine;
using UnityEngine.UI;
using CheeseHeist.Core;
using CheeseHeist.Systems;

namespace CheeseHeist.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _playButton;

        public void Initialize(GameEvents events, GameFlowSystem gameFlow)
        {
            events.OnGameStateChanged += HandleStateChanged;
            _playButton.onClick.AddListener(() => gameFlow.StartGame());

            _panel.SetActive(true); 
        }

        private void HandleStateChanged(GameState state)
        {
            _panel.SetActive(state == GameState.MainMenu);
        }
    }
}