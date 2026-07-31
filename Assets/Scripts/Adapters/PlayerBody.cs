using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBody : MonoBehaviour
    {
        private PlayerData _playerData;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void ApplyVelocity()
        {
            var velocity = _playerData.Velocity;
            _rigidbody.linearVelocity = new Vector3(velocity.X, velocity.Y, velocity.Z);
        }
    }
}