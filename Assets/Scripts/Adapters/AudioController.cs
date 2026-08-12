using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class AudioController : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;

        [Header("Cheese Collected")]
        [SerializeField] private AudioClip _cheeseCollected;
        [SerializeField, Range(0f, 2f)] private float _cheeseCollectedVolume = 1f;

        [Header("Life Lost")]
        [SerializeField] private AudioClip _lifeLost;
        [SerializeField, Range(0f, 2f)] private float _lifeLostVolume = 1f;

        [Header("Cat Caught")]
        [SerializeField] private AudioClip _caught;
        [SerializeField, Range(0f, 2f)] private float _caughtVolume = 1f;

        [Header("Music")]
        [SerializeField] private AudioClip _gameplayMusic;

        private int _lastLives = -1;

        public void Initialize(GameEvents events)
        {
            events.OnCheeseCollected += _ => PlaySfx(_cheeseCollected, _cheeseCollectedVolume);

            events.OnLivesChanged += lives =>
            {
                if (_lastLives >= 0 && lives < _lastLives)
                {
                    PlaySfx(_lifeLost, _lifeLostVolume);
                }
                _lastLives = lives;
            };

            events.OnCatCaught += () => PlaySfx(_caught, _caughtVolume);

            PlayMusic();
        }

        private void PlaySfx(AudioClip clip, float volume)
        {
            if (clip == null || _sfxSource == null) return;
            _sfxSource.PlayOneShot(clip, volume);
        }

        private void PlayMusic()
        {
            if (_gameplayMusic == null || _musicSource == null) return;
            if (_musicSource.isPlaying) return;

            _musicSource.clip = _gameplayMusic;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }
}