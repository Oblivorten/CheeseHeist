using UnityEngine;
using CheeseHeist.Core;
using UnityEngine.InputSystem;

namespace CheeseHeist.Adapters
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _inputActions;
        [SerializeField] private PlayerBody _playerBody;
        private Loop _loop;

        private void Awake()
        {
            var bootstrap = new Bootstrap();
            _loop = bootstrap.CreateLoop(_inputActions, _playerBody);
        }

        private void Update()
        {
            _loop.Tick(Time.deltaTime);
        }
    }
}