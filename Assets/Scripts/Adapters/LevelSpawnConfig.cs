using UnityEngine;

namespace CheeseHeist.Adapters
{
    [System.Serializable]
    public class LevelSpawnConfig
    {
        public Vector2 ArenaHalfExtents = new Vector2(20f, 20f);
        public float SafeZoneRadius = 5f;     
        public float MinSpacing = 2f;          
        public int MaxPlacementAttempts = 30; 
        public GameObject[] ObstaclePrefabs;
        public int ObstacleCount = 6;
        public GameObject CheesePrefab;
        public int CheeseCount = 8;
    }
}