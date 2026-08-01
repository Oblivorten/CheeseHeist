using CheeseHeist.Core;
using CheeseHeist.Systems;

namespace CheeseHeist.Adapters
{
    public class GameContext
    {
        public Loop Loop;
        public TrailSystem TrailSystem;
        public GameEvents Events;
        public GameSessionData Session;
        public CatData Cat;
    }
}