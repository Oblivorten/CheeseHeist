using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    [RequireComponent(typeof(Collider))]
    public class CheesePickup : MonoBehaviour
    {
        [SerializeField] private string _playerTag = "Player";

        private GameEvents _events;
        private int _points;
        private bool _collected;

        public void Initialize(GameEvents events, int points)
        {
            _events = events;
            _points = points;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_collected) return;
            if (!other.CompareTag(_playerTag)) return;

            _collected = true;
            _events.RaiseCheeseCollected(_points);
            Destroy(gameObject);
        }
    }
}