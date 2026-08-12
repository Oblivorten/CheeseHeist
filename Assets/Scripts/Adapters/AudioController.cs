using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class AudioController : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;

        [Header("SFX")]
        [SerializeField] private AudioClip _cheeseCollected;
        [SerializeField] private AudioClip _lifeLost;
        [SerializeField] private AudioClip _caught;
        [SerializeField] private AudioClip _uiClick;

        [Header("Music")]
        [SerializeField] private AudioClip _menuMusic;
        [SerializeField] private AudioClip _gameplayMusic;

        private int _lastLives = -1;

        public void Initialize(GameEvents events)
        {
            events.OnCheeseCollected += _ => PlaySfx(_cheeseCollected);

            events.OnLivesChanged += lives =>
            {
                if (_lastLives >= 0 && lives < _lastLives)
                {
                    PlaySfx(_lifeLost);
                }
                _lastLives = lives;
            };

            events.OnCatCaught += () => PlaySfx(_caught);

            events.OnGameStateChanged += HandleStateChanged;
        }

        public void PlayUiClick() => PlaySfx(_uiClick);

        private void HandleStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    PlayMusic(_menuMusic);
                    break;
                case GameState.Playing:
                    PlayMusic(_gameplayMusic);
                    break;
            }
        }

        private void PlaySfx(AudioClip clip)
        {
            if (clip == null || _sfxSource == null) return;
            _sfxSource.PlayOneShot(clip);
        }

        private void PlayMusic(AudioClip clip)
        {
            if (clip == null || _musicSource == null) return;
            if (_musicSource.clip == clip && _musicSource.isPlaying) return;

            _musicSource.clip = clip;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }
}