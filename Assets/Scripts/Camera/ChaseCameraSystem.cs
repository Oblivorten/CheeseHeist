using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.CameraSystem
{
    public class ChaseCameraSystem : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _distanceBehind = 7f;
        [SerializeField] private float _height = 3.5f;
        [SerializeField] private float _lookAheadHeight = 1f;
        [SerializeField] private float _followSmoothTime = 0.15f;
        [SerializeField] private float _rotationSmoothSpeed = 4f;
        [SerializeField] private float _followDirectionTurnSpeed = 150f; 

        private Vector3 _velocity;
        private Vector3 _currentFollowDirection = Vector3.forward;
        private CameraData _cameraData;

        public void Initialize(CameraData cameraData)
        {
            _cameraData = cameraData;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 targetForward = _target.forward;
            targetForward.y = 0f;
            if (targetForward.sqrMagnitude > 0.0001f)
            {
                targetForward.Normalize();
            }
            else
            {
                targetForward = _currentFollowDirection;
            }

            _currentFollowDirection = Vector3.RotateTowards(
                _currentFollowDirection, targetForward,
                _followDirectionTurnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
            _currentFollowDirection.Normalize();

            Vector3 desiredPosition = _target.position
                - _currentFollowDirection * _distanceBehind
                + Vector3.up * _height;

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _followSmoothTime);

            Vector3 lookTarget = _target.position + Vector3.up * _lookAheadHeight;
            Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * _rotationSmoothSpeed);

            WriteCameraData();
        }

        private void WriteCameraData()
        {
            if (_cameraData == null) return;

            Vector3 flatForward = transform.forward;
            flatForward.y = 0f;
            if (flatForward.sqrMagnitude < 0.0001f) flatForward = Vector3.forward;
            flatForward.Normalize();

            Vector3 flatRight = transform.right;
            flatRight.y = 0f;
            if (flatRight.sqrMagnitude < 0.0001f) flatRight = Vector3.right;
            flatRight.Normalize();

            _cameraData.Forward = new Vector3Data(flatForward.x, 0f, flatForward.z);
            _cameraData.Right = new Vector3Data(flatRight.x, 0f, flatRight.z);
        }
    }
}