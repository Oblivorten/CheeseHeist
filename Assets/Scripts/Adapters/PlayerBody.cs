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

        public void SyncPositionToData()
        {
            var p = _rigidbody.position;
            _playerData.Position = new Vector3Data(p.x, p.y, p.z);
        }

        public void ApplyVelocity()
        {
            var v = _playerData.Velocity;
            _rigidbody.linearVelocity = new Vector3(v.X, v.Y, v.Z);
        }
    }
}