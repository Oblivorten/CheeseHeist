namespace CheeseHeist.Adapters
{
    [System.Serializable]
    public class CatConfig
    {
        public float SpawnHeight = 0.5f;
        public float InitialOffsetDistance = 4f;
        public float BaseSpeed = 4f;
        public float LungeSpeedMultiplier = 1.8f;
        public float LungeWindowDuration = 5f;
        public float CatchRadius = 0.8f;
        public float VelocityTurnRateDegrees = 100f;
        public float FacingTurnRateDegrees = 600f;   
    }
}