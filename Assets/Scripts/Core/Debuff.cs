namespace CheeseHeist.Core
{
    public class Debuff
    {
        public float SpeedMultiplier;
        public float MinControlMultiplier;
        public float Duration;
        public float Elapsed;

        public bool IsExpired => Elapsed >= Duration;

        public float CurrentControlMultiplier
        {
            get
            {
                if (Duration <= 0f) return 1f;
                float t = Elapsed / Duration;
                if (t > 1f) t = 1f;
                return MinControlMultiplier + (1f - MinControlMultiplier) * t;
            }
        }
    }
}