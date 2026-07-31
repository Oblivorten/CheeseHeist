using CheeseHeist.Core;
using CheeseHeist.Systems;
using UnityEngine.InputSystem;

namespace CheeseHeist.Adapters
{
    public class Bootstrap
    {
        public Loop CreateLoop(
            InputActionAsset inputActions, 
            PlayerBody playerBody,
            VirtualJoystickInputAdapter joystick,
            GyroscopeInputAdapter gyroscope)
        {
            var loop = new Loop();

            var playerData = new PlayerData
            {
                MoveSpeed = 5f
            };

            var keyboard = new KeyboardInputAdapter(inputActions);

            var inputRouter = new PlayerInputRouter();

            inputRouter.SetProvider(joystick);

            var movementSystem = new MovementSystem(
                playerData, 
                inputRouter);

            playerBody.Initialize(playerData);

            loop.AddSystem(movementSystem);

            return loop;
        }
    }
}