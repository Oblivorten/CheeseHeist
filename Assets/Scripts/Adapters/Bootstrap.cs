using CheeseHeist.Core;
using CheeseHeist.Systems;
using UnityEngine.InputSystem;

namespace CheeseHeist.Adapters
{
    public class Bootstrap
    {
        public Loop CreateLoop(InputActionAsset inputActions, PlayerBody playerBody)
        {
            var loop = new Loop();

            var playerData = new PlayerData
            {
                MoveSpeed = 5f
            }; 

            var inputProvider = new KeyboardInputAdapter(inputActions);

            var movementSystem = new MovementSystem(playerData, inputProvider);

            playerBody.Initialize(playerData);

            loop.AddSystem(movementSystem);

            return loop;
        }
    }
}