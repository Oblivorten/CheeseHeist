using UnityEngine;
using UnityEngine.InputSystem;

namespace CheeseHeist.Adapters
{
    [System.Serializable]
    public class SceneReferences
    {
        public InputActionAsset InputActions;
        public PlayerBody PlayerBody;
        public ObstacleCollisionAdapter ObstacleCollision;
        public VirtualJoystickInputAdapter Joystick;
        public GyroscopeInputAdapter Gyroscope;
        public TrailSegmentView TrailView;
    }
}