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
        private Loop _loop;

        private void Awake()
        {
            var bootstrap = new Bootstrap();
            _loop = bootstrap.CreateLoop(
                _inputActions,
                _playerBody,
                _joyStick,
                _gyroscope);
        }

        private void Update()
        {
            _loop.Tick(Time.deltaTime);
        }
    }
}