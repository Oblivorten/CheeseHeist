using UnityEngine;
using UnityEngine.InputSystem;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class GyroscopeInputAdapter : MonoBehaviour, IMoveInputProvider
    {
        [SerializeField] private float _sensitivity = 2f;

        private Quaternion _calibrationRotation;
        private Vector2 _input;

        public float Horizontal => _input.x;
        public float Vertical => _input.y;

        private void OnEnable()
        {
            if (AttitudeSensor.current != null)
            {
                InputSystem.EnableDevice(AttitudeSensor.current);
            }
            Calibrate();
        }

        private void OnDisable()
        {
            if (AttitudeSensor.current != null)
            {
                InputSystem.DisableDevice(AttitudeSensor.current);
            }
        }

        private void Update()
        {
            if (AttitudeSensor.current == null)
            {
                return;
            }

            Quaternion currentRotation = AttitudeSensor.current.attitude.ReadValue();
            Quaternion relativeRotation = Quaternion.Inverse(_calibrationRotation) * currentRotation;
            Vector3 rotation = relativeRotation.eulerAngles;

            float horizontal = NormalizeAngle(rotation.y) * _sensitivity;
            float vertical = NormalizeAngle(rotation.x) * _sensitivity;

            _input = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);
        }

        public void Calibrate()
        {
            if (AttitudeSensor.current != null)
            {
                _calibrationRotation = AttitudeSensor.current.attitude.ReadValue();
            }
        }

        private float NormalizeAngle(float angle)
        {
            if (angle > 180f)
            {
                angle -= 360f;
            }
            return angle / 45f;
        }
    }
}