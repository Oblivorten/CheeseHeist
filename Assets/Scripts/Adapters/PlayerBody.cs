using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBody : MonoBehaviour
    {
        private PlayerData _playerData;
        private Rigidbody _rigidbody;
        private Vector3 _spawnPosition;
        private Quaternion _spawnRotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(PlayerData playerData, GameEvents events)
        {
            _playerData = playerData;
            events.OnRestartRequested += HandleRestart;
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

        private void HandleRestart()
        {
            _rigidbody.position = _spawnPosition;
            _rigidbody.rotation = _spawnRotation;
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}