using UnityEngine;

namespace CheeseHeist.CameraSystem
{
    public class ChaseCameraSystem : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _distanceBehind = 7f;
        [SerializeField] private float _height = 3.5f;
        [SerializeField] private float _lookAheadHeight = 1f;
        [SerializeField] private float _followSmoothTime = 0.15f;
        [SerializeField] private float _rotationSmoothSpeed = 6f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 desiredPosition = _target.position
                - _target.forward * _distanceBehind
                + Vector3.up * _height;

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _followSmoothTime);

            Vector3 lookTarget = _target.position + Vector3.up * _lookAheadHeight;
            Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * _rotationSmoothSpeed);
        }
    }
}