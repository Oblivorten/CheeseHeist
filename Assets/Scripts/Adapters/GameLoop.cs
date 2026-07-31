using UnityEngine;
using CheeseHeist.Core;
using UnityEngine.InputSystem;

namespace CheeseHeist.Adapters
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private PlayerBody _playerBody;
        [SerializeField] private VirtualJoystickInputAdapter _joyStick;
        [SerializeField] private GyroscopeInputAdapter _gyroscope;
        [SerializeField] private InputSource _activeInputSource = InputSource.Keyboard;
        [SerializeField] private float _acceleration = 20f;
        [SerializeField] private float _deceleration = 25f;

        private Loop _loop;

        private void Awake()
        {
            var bootstrap = new Bootstrap();
            _loop = bootstrap.CreateLoop(
                _inputActions,
                _playerBody,
                _joyStick,
                _gyroscope,
                _activeInputSource,
                _acceleration,
                _deceleration);
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;
            _loop.Tick(dt);
            _playerBody.ApplyVelocity();
        }
    }
}