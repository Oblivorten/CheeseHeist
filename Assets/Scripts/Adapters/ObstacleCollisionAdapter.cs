using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    [RequireComponent(typeof(Collider))]
    public class ObstacleCollisionAdapter : MonoBehaviour
    {
        [SerializeField] private string _obstacleTag = "Obstacle";

        private GameEvents _events;

        public void Initialize(GameEvents events)
        {
            _events = events;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(_obstacleTag))
            {
                _events?.RaisePlayerCollision();
                _events?.RaiseObstacleHit();
            }
        }
    }
}