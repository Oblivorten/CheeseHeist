using CheeseHeist.Core;
using CheeseHeist.Systems;

namespace CheeseHeist.Adapters
{
    public class Bootstrap
    {
        public Loop CreateLoop()
        {
            var loop = new Loop();

            var testSystem = new TestSystem();

            loop.AddSystem(testSystem);

            return loop;
        }
    }
}