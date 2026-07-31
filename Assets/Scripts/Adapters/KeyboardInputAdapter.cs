using UnityEngine;
using UnityEngine.InputSystem;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class KeyboardInputAdapter : IMoveInputProvider
    {
        private readonly InputAction _moveAction;

        public KeyboardInputAdapter(InputActionAsset inputActions)
        {
            _moveAction = inputActions.FindAction("Player/Move");

            _moveAction.Enable();
        }

        public float Horizontal => _moveAction.ReadValue<Vector2>().x;

        public float Vertical => _moveAction.ReadValue<Vector2>().y;
    }
}