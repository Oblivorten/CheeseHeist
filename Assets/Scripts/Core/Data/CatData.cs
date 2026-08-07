namespace CheeseHeist.Core
{
    public class CatData
    {
        public Vector3Data Position;
        public Vector3Data Velocity;
        public Vector3Data FacingDirection = new Vector3Data(0f, 0f, 1f);
        public CatState State;
        public float StateTimer;
        public bool IsCaught;
    }
}