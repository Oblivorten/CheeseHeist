using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBody : MonoBehaviour, IResettable
    {
        [SerializeField] private float _rotationSpeedDegrees = 720f;

        private PlayerData _playerData;
        private Rigidbody _rigidbody;
        private Vector3 _spawnPosition;
        private Quaternion _spawnRotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Initialize(PlayerData playerData)
        {
            _playerData = playerData;
            _spawnPosition = _rigidbody.position;
            _spawnRotation = _rigidbody.rotation;
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
            _rigidbody.angularVelocity = Vector3.zero;

            var facing = _playerData.FacingDirection;
            var facingVector = new Vector3(facing.X, 0f, facing.Z);
            if (facingVector.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(facingVector.normalized, Vector3.up);
                _rigidbody.MoveRotation(
                    Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _rotationSpeedDegrees * Time.fixedDeltaTime));
            }
        }

        public void ResetState()
        {
            _rigidbody.position = _spawnPosition;
            _rigidbody.rotation = _spawnRotation;
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
}