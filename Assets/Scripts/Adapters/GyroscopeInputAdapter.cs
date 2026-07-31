using UnityEngine;
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

        private void Awake()
        {
            Input.gyro.enabled = true;

            Calibrate();
        }

        private void Update()
        {
            Quaternion currentRotation = Input.gyro.attitude;

            Quaternion relativeRotation =
                Quaternion.Inverse(_calibrationRotation) * currentRotation;

            Vector3 rotation = relativeRotation.eulerAngles;

            float horizontal = NormalizeAngle(rotation.y);
            float vertical = NormalizeAngle(rotation.x);

            horizontal *= _sensitivity;
            vertical *= _sensitivity;

            _input = new Vector2(horizontal, vertical);

            _input = Vector2.ClampMagnitude(_input, 1f);
        }

        public void Calibrate()
        {
            _calibrationRotation = Input.gyro.attitude;
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