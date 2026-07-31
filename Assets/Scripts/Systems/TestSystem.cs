using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Systems
{
    public class TestSystem : ITickable
    {
        public void Tick(float deltaTime)
        {
            Debug.Log($"TestSystem Tick: {deltaTime}");
        }
    }
}