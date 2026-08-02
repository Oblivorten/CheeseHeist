using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    
    public class UnityTimeController : ITimeController
    {
        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
    }
}