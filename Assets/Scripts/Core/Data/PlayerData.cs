namespace CheeseHeist.Core
{
    public class PlayerData
    {
        public Vector3Data Position;
        public Vector3Data Velocity;
        public Vector3Data FacingDirection = new Vector3Data(0f, 0f, 1f);
        public float MoveSpeed;
        public float SpeedMultiplier = 1f;
        public float ControlMultiplier = 1f;
    }
}