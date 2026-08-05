using UnityEngine;
using UnityEngine.InputSystem;
using CheeseHeist.UI;
using CheeseHeist.CameraSystem;

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
        public CatBody CatBody;
        public HUDController HUD;
        public ResultsScreenController ResultsScreen;
        public LevelSpawner LevelSpawner;
        public ChaseCameraSystem Camera;
    }
}