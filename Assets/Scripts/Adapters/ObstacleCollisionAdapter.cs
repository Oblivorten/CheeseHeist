using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    [RequireComponent(typeof(Collider))]
    public class ObstacleCollisionAdapter : MonoBehaviour
    {
        [SerializeField] private string _obstacleTag = "Obstacle";

        private GameEvents _events;
        private GameSessionData _session;

        public void Initialize(GameEvents events, GameSessionData session)
        {
            _events = events;
            _session = session;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.CompareTag(_obstacleTag)) return;
            if (_session != null && _session.IsInvulnerable) return;

            _events?.RaisePlayerCollision();
            _events?.RaiseObstacleHit();
        }
    }
}