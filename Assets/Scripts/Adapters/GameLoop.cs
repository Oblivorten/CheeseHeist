using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class GameLoop : MonoBehaviour
    {
        private Loop _loop;

        private void Awake()
        {
            var bootstrap = new Bootstrap();
            _loop = bootstrap.CreateLoop();
        }

        private void Update()
        {
            _loop.Tick(Time.deltaTime);
        }
    }
}