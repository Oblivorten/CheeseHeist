namespace CheeseHeist.Adapters
{
    [System.Serializable]
    public class SkidConfig
    {
        public float CollisionRadius = 0.4f;
        public int GraceSegments = 6;
        public float SpeedMultiplier = 0.5f;
        public float MinControlMultiplier = 0.05f;
        public float Duration = 1.5f;
    }
}