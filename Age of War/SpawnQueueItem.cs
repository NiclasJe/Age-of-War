using System;

namespace Age_of_War
{
    // Klass för att hålla information om en enhet som ska spawnas
    public class SpawnQueueItem
    {
        public Type UnitType;
        public int SpawnTime; // ms det tar att spawna
        public int TimeElapsed; // ms som har gått
        public bool IsPlayer;

        public SpawnQueueItem(Type unitType, int spawnTime, bool isPlayer)
     {
 UnitType = unitType;
   SpawnTime = spawnTime;
      TimeElapsed = 0;
    IsPlayer = isPlayer;
  }

        public bool IsReady => TimeElapsed >= SpawnTime;
        
        public void Update(int deltaTime)
        {
     TimeElapsed += deltaTime;
  }
        
        public float Progress => (float)TimeElapsed / SpawnTime;
    }
}
