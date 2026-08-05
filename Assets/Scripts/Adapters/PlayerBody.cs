using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBody : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeedDegrees = 720f;

        private PlayerData _playerData;
        private Rigidbody _rigidbody;
        private Vector3 _spawnPosition;
        private Quaternion _spawnRotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _spawnPosition = _rigidbody.position;
            _spawnRotation = _rigidbody.rotation;
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

            var horizontalVelocity = new Vector3(v.X, 0f, v.Z);
            if (horizontalVelocity.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity.normalized, Vector3.up);
                _rigidbody.MoveRotation(
                    Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeedDegrees * Time.fixedDeltaTime));
            }
        }

        private void HandleRestart()
        {
            _rigidbody.position = _spawnPosition;
            _rigidbody.rotation = _spawnRotation;
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}