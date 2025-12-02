using System;
using System.Collections.Generic;
using System.Drawing;

namespace Age_of_War
{
	public class Game
	{
		public List<Unit> PlayerUnits = new();
		public List<Unit> EnemyUnits = new();
		public List<Projectile> Projectiles = new();
		public Base PlayerBase;
		public Base EnemyBase;

		// Turret-hantering
		public Turret PlayerTurret = null;  // Legacy - behålls för bakåtkompatibilitet
		public List<Turret> PlayerTurrets = new List<Turret>();  // Lista för alla turrets (upp till 4)
		public Rectangle TurretSlot;  // Platsen där första turret kan placeras
		public List<Rectangle> TurretSlots = new List<Rectangle>();  // Alla turret-slots
		public bool ShowTurretSlot = false;  // Visa slot när man ska placera turret
		public bool ShowTurretSellHighlight = false;  // Visa röd ruta runt turret för att sälja
		public int BaseExpansions = 0;  // Antal utbyggnader (max 3)
		public const int MAX_EXPANSIONS = 3;
		public const int EXPANSION_COST = 3000;

		public int PlayerGold = 30000;  // Startguld
		public int EnemyGold = 300;   // Startguld för fiende
		public int IncomePerSecond = 0;  // Ingen passiv inkomst längre
		public int PlayerScore = 0;
		public int EnemyScore = 0;

		// Ultimate attack - Fallande stenar
		public bool UltimateAvailable = true;
		public int UltimateCooldown = 0;  // ms kvar till nästa ultimate
		public const int ULTIMATE_COOLDOWN_TIME = 60000;  // 60 sekunder
		public List<FallingRock> FallingRocks = new List<FallingRock>();
		public bool UltimateActive = false;  // Om ultimate håller på att spawna stenar
		public int UltimateSpawnTimer = 0;  // Timer för att spawna stenar över tid
		public const int ULTIMATE_SPAWN_DURATION = 3000;  // 3 sekunder spawn-period
		public int UltimateRocksToSpawn = 0;  // Antal stenar kvar att spawna
		public int UltimateSpawnInterval = 0;  // Intervall mellan varje sten

		public int BattlefieldY; // Y för marken
		private int screenWidth;
		private int screenHeight;

		public Queue<SpawnQueueItem> PlayerSpawnQueue = new();
		public Queue<SpawnQueueItem> EnemySpawnQueue = new();
		private SpawnQueueItem currentPlayerSpawn = null;
		private SpawnQueueItem currentEnemySpawn = null;

		private Random enemySpawnRandom = new Random();
		private int enemySpawnTimer = 0;
		private int nextEnemySpawnDelay = 5000; // ms till nästa spawn

		public Game(int width, int height)
		{
			screenWidth = width;
			screenHeight = height;
			UpdateDimensions(width, height);
			// Sätt första spawn-delay (snabbare)
			nextEnemySpawnDelay = enemySpawnRandom.Next(1500, 4000); // 1.5-4 sekunder
		}

		// Uppdatera dimensioner när skärmen ändrar storlek
		public void UpdateDimensions(int width, int height)
		{
			screenWidth = width;
			screenHeight = height;
			BattlefieldY = height - 80; // Mark längs ner

			// Spelarbas längst till vänster
			PlayerBase = new Base(true, new Rectangle(20, BattlefieldY - 150, 100, 150));

			// Fiendbas längst till höger
			EnemyBase = new Base(false, new Rectangle(width - 120, BattlefieldY - 150, 100, 150));

			// Uppdatera turret-slots baserat på antal utbyggnader
			UpdateTurretSlots();
		}

		// Uppdatera turret-slots baserat på antal utbyggnader
		private void UpdateTurretSlots()
		{
			TurretSlots.Clear();
			
			// Första slot inbakad i basen
			TurretSlots.Add(new Rectangle(PlayerBase.Hitbox.X + 35, PlayerBase.Hitbox.Y + 15, 30, 30));
			
			// Lägg till slots för varje utbyggnad (torn staplade rakt ovanpå varandra)
			for (int i = 0; i < BaseExpansions; i++)
			{
				int towerX = PlayerBase.Hitbox.X + 35;  // Samma X som bas-sloten - rakt ovanpå
				int towerY = PlayerBase.Hitbox.Y - 40 - (i * 50);  // Högre upp för varje torn, mer mellanrum
				TurretSlots.Add(new Rectangle(towerX, towerY, 30, 30));
			}
			
			// Uppdatera legacy TurretSlot till första slot
			if (TurretSlots.Count > 0)
				TurretSlot = TurretSlots[0];
		}
		// Uppdatera ekonomi
		public void UpdateEconomy(int deltaTime)
		{
			// deltaTime i ms
			PlayerGold += IncomePerSecond * deltaTime / 1000;
			EnemyGold += IncomePerSecond * deltaTime / 1000;
		}

		// Uppdatera enheter och strider
		public void UpdateUnits(int deltaTime)
		{
			// Först: Återställ alla blockeringar
			foreach (var unit in PlayerUnits)
				unit.IsBlocked = false;
			foreach (var unit in EnemyUnits)
				unit.IsBlocked = false;

			// Andra: Kontrollera kollisioner mellan egna enheter (blockering)
			for (int i = 0; i < PlayerUnits.Count; i++)
			{
				for (int j = i + 1; j < PlayerUnits.Count; j++)
				{
					if (Utils.Intersects(PlayerUnits[i].Hitbox, PlayerUnits[j].Hitbox))
					{
						// Den bakre enheten blockeras
						if (PlayerUnits[i].PositionX < PlayerUnits[j].PositionX)
							PlayerUnits[i].IsBlocked = true;
						else
							PlayerUnits[j].IsBlocked = true;
					}
				}
			}

			for (int i = 0; i < EnemyUnits.Count; i++)
			{
				for (int j = i + 1; j < EnemyUnits.Count; j++)
				{
					// Ökat avstånd för fiendens enheter så HP-barer syns bättre
					float distance = Math.Abs(EnemyUnits[i].PositionX - EnemyUnits[j].PositionX);
					if (distance < 45) // Extra avstånd mellan fiendens enheter
					{
						// Den bakre enheten blockeras (för fienden: den som är längre till höger)
						if (EnemyUnits[i].PositionX > EnemyUnits[j].PositionX)
							EnemyUnits[i].IsBlocked = true;
						else
							EnemyUnits[j].IsBlocked = true;
					}
				}
			}

			// Tredje: Kontrollera kollisioner med fiendeenheter och baser
			foreach (var p in PlayerUnits)
			{
				// Kolla kollision med fiendebas
				if (Utils.Intersects(p.Hitbox, EnemyBase.Hitbox))
				{
					p.IsBlocked = true;
				}

				// Kolla kollision med fiendeenheter
				foreach (var e in EnemyUnits)
				{
					// Ökat avstånd för strid - enheter stannar längre ifrån varandra
					float combatDistance = Math.Abs(p.PositionX - e.PositionX);

					// Närstridssoldater stannar på 40 pixlar avstånd (minskat från 50)
					if ((p is Soldier || p is CavalryUnit || p is TankUnit) &&
						 (e is Soldier || e is CavalryUnit || e is TankUnit))
					{
						if (combatDistance < 40)
						{
							p.IsBlocked = true;
							e.IsBlocked = true;
						}
					}
					// Om någon är ranged, större avstånd
					else if (p is RangedUnit || e is RangedUnit)
					{
						if (combatDistance < 60)
						{
							p.IsBlocked = true;
							e.IsBlocked = true;
						}
					}
					// Direkt kollision (backup)
					else if (Utils.Intersects(p.Hitbox, e.Hitbox))
					{
						p.IsBlocked = true;
						e.IsBlocked = true;
					}
				}
			}

			foreach (var e in EnemyUnits)
			{
				// Kolla kollision med spelarbas
				if (Utils.Intersects(e.Hitbox, PlayerBase.Hitbox))
				{
					e.IsBlocked = true;
				}
			}

			// Fjärde: Uppdatera positioner (endast om inte blockerade)
			foreach (var unit in PlayerUnits)
				unit.Update(deltaTime);
			foreach (var unit in EnemyUnits)
				unit.Update(deltaTime);

			// Femte: Strid och skada
			for (int i = PlayerUnits.Count - 1; i >= 0; i--)
			{
				var p = PlayerUnits[i];

				// Kolla kollision med fiendebas
				if (Utils.Intersects(p.Hitbox, EnemyBase.Hitbox))
				{
					if (p.CanAttack)
					{
						EnemyBase.HP -= p.Damage;
						p.ResetAttackTimer();
					}
				}

				foreach (var e in EnemyUnits)
				{
					float distance = System.Math.Abs(p.PositionX - e.PositionX);

					// Närstridssoldater (Soldier, CavalryUnit, TankUnit) - kan slå på närmare avstånd
					if ((p is Soldier || p is CavalryUnit || p is TankUnit) &&
					(e is Soldier || e is CavalryUnit || e is TankUnit))
					{
						if (distance < 60)  // Ökat från 55 till 60 så de når varandra
						{
							if (p.CanAttack)
							{
								e.HP -= p.Damage;
								p.ResetAttackTimer();
							}
							if (e.CanAttack)
							{
								p.HP -= e.Damage;
								e.ResetAttackTimer();
							}
						}
					}
					// RangedUnit kan skjuta på avstånd
					else if (p is RangedUnit)
					{
						if (distance < 100)
						{
							if (p.CanAttack)
							{
								// Skapa projektil
								Projectiles.Add(new Projectile(p.PositionX, p.PositionY - 30, e.PositionX, e.PositionY - 25, true, p.Damage));
								p.ResetAttackTimer();
							}
						}
					}
				}

				if (p.IsDead)
				{
					PlayerUnits.RemoveAt(i);
					PlayerScore++;
					// Fienden har oändligt med guld - ingen anledning att ge mer
				}
			}

			for (int i = EnemyUnits.Count - 1; i >= 0; i--)
			{
				var e = EnemyUnits[i];

				// Kolla kollision med spelarbas
				if (Utils.Intersects(e.Hitbox, PlayerBase.Hitbox))
				{
					if (e.CanAttack)
					{
						PlayerBase.HP -= e.Damage;
						e.ResetAttackTimer();
					}
				}

				// Strid med spelarenheter
				foreach (var p in PlayerUnits)
				{
					float distance = System.Math.Abs(p.PositionX - e.PositionX);

					// Närstrid för fiendens melee-enheter - kan slå på närmare avstånd
					if ((e is Soldier || e is CavalryUnit || e is TankUnit) &&
   (p is Soldier || p is CavalryUnit || p is TankUnit))
    {
        if (distance < 60)  // Ökat från 55 till 60 så de når varandra
   {
   if (e.CanAttack)
            {
   p.HP -= e.Damage;
    e.ResetAttackTimer();
      }
}
    }
    // RangedUnit för fienden
  else if (e is RangedUnit)
    {
   if (distance < 100)
     {
       if (e.CanAttack)
  {
    // Skapa projektil
   Projectiles.Add(new Projectile(e.PositionX, e.PositionY - 30, p.PositionX, p.PositionY - 25, false, e.Damage));
    e.ResetAttackTimer();
      }
   }
 }
}

				if (e.IsDead)
				{
					EnemyUnits.RemoveAt(i);
					EnemyScore++;
					// Spelaren får guld baserat på enhetstyp
					int goldReward = e is Soldier ? 50 : e is RangedUnit ? 80 : e is CavalryUnit ? 200 : 50;  // Ändrat från 120 till 200
					PlayerGold += goldReward;
				}
			}

			// Uppdatera och hantera projektiler
			for (int i = Projectiles.Count - 1; i >= 0; i--)
			{
				var proj = Projectiles[i];
				proj.Update();

				// Kolla om projektilen träffar marken
				if (proj.Y >= BattlefieldY)
				{
				proj.IsActive = false;
					Projectiles.RemoveAt(i);
				continue;  // Gå till nästa projektil
				}

				// Kolla om projektilen träffar en enhet
				if (proj.IsPlayer)
				{
				foreach (var e in EnemyUnits)
			   {
			       if (Utils.Intersects(new Rectangle((int)proj.X - 3, (int)proj.Y - 3, 6, 6), e.Hitbox))
            {
    e.HP -= proj.Damage;
   proj.IsActive = false;
       break;
   }
     }
// Kolla om den träffar fiendebasen
     if (Utils.Intersects(new Rectangle((int)proj.X - 3, (int)proj.Y - 3, 6, 6), EnemyBase.Hitbox))
        {
    proj.IsActive = false;
  }
    }
    else
    {
     foreach (var p in PlayerUnits)
        {
   if (Utils.Intersects(new Rectangle((int)proj.X - 3, (int)proj.Y - 3, 6, 6), p.Hitbox))
    {
       p.HP -= proj.Damage;
    proj.IsActive = false;
    break;
   }
  }
    // Kolla om den träffar spelarbasen
        if (Utils.Intersects(new Rectangle((int)proj.X - 3, (int)proj.Y - 3, 6, 6), PlayerBase.Hitbox))
        {
            proj.IsActive = false;
        }
 }

    // Ta bort inaktiva projektiler eller projektiler utanför skärmen
    if (!proj.IsActive || proj.X < 0 || proj.X > screenWidth)
    {
   Projectiles.RemoveAt(i);
    }
}

// Uppdatera turret
if (PlayerTurret != null)
{
    PlayerTurret.Update(deltaTime);

    // Hitta närmaste fiende inom räckvidd
    Unit nearestEnemy = null;
    float nearestDistance = float.MaxValue;
    float turretRange = GetTurretRange(PlayerTurret);

    foreach (var enemy in EnemyUnits)
    {
        float distance = Math.Abs(enemy.PositionX - PlayerTurret.PositionX);
  if (distance < turretRange && distance < nearestDistance)
        {
     nearestEnemy = enemy;
     nearestDistance = distance;
 }
    }

    // Skjut på närmaste fiende
    if (nearestEnemy != null && PlayerTurret.CanAttack)
 {
   Projectiles.Add(new Projectile(
       PlayerTurret.PositionX,
      PlayerTurret.PositionY - 20,
      nearestEnemy.PositionX,
            nearestEnemy.PositionY - 25,
   true,
   PlayerTurret.Damage / 2,
  true));
   PlayerTurret.ResetAttackTimer();
    }
}

// Uppdatera alla turrets i listan
foreach (var turret in PlayerTurrets)
{
    turret.Update(deltaTime);
    
    // Hitta närmaste fiende inom räckvidd för denna turret
    Unit nearestEnemy = null;
    float nearestDistance = float.MaxValue;
    float turretRange = GetTurretRange(turret);

    foreach (var enemy in EnemyUnits)
    {
        float distance = Math.Abs(enemy.PositionX - turret.PositionX);
        if (distance < turretRange && distance < nearestDistance)
    {
   nearestEnemy = enemy;
            nearestDistance = distance;
 }
    }

    // Skjut på närmaste fiende
    if (nearestEnemy != null && turret.CanAttack)
    {
        Projectiles.Add(new Projectile(
     turret.PositionX,
            turret.PositionY - 20,
      nearestEnemy.PositionX,
            nearestEnemy.PositionY - 25,
 true,
     turret.Damage / 2,
    true));
 turret.ResetAttackTimer();
    }
}
        }  // Slut på UpdateUnits

// Hjälpmetod för att få räckvidd baserat på turret-typ
private float GetTurretRange(Turret turret)
{
    if (turret is PremiumTurret)
        return screenWidth / 2;
    else if (turret is AdvancedTurret)
     return screenWidth * 0.4f;
    else
 return screenWidth * 0.25f;
}

public void UpdateEnemySpawn()
{
    enemySpawnTimer += 33;
    
    if (enemySpawnTimer >= nextEnemySpawnDelay)
    {
        if (EnemySpawnQueue.Count + (currentEnemySpawn != null ? 1 : 0) < 3)
        {
int t = enemySpawnRandom.Next(3);
            Type unitType = t switch
          {
        0 => typeof(Soldier),
   1 => typeof(RangedUnit),
   2 => typeof(CavalryUnit),
         _ => typeof(Soldier)
            };
   
         QueueUnitSpawn(unitType, false);
        }
  
        enemySpawnTimer = 0;
        nextEnemySpawnDelay = enemySpawnRandom.Next(1500, 4000);
    }
}

public void QueueUnitSpawn(Type unitType, bool isPlayer)
{
    int spawnTime = unitType == typeof(Soldier) ? 3000 :
        unitType == typeof(RangedUnit) ? 5000 : 10000;
    
    var queueItem = new SpawnQueueItem(unitType, spawnTime, isPlayer);
    
    if (isPlayer)
    {
        if (PlayerSpawnQueue.Count + (currentPlayerSpawn != null ? 1 : 0) < 5)
    {
    PlayerSpawnQueue.Enqueue(queueItem);
        }
    }
    else
    {
        EnemySpawnQueue.Enqueue(queueItem);
    }
}

public void UpdateSpawnQueues(int deltaTime)
{
    if (currentPlayerSpawn == null && PlayerSpawnQueue.Count > 0)
    {
currentPlayerSpawn = PlayerSpawnQueue.Dequeue();
    }
    
    if (currentPlayerSpawn != null)
    {
    currentPlayerSpawn.Update(deltaTime);
 
  if (currentPlayerSpawn.IsReady)
        {
     float spawnX = PlayerBase.Hitbox.Left + 20;
       Unit unit = currentPlayerSpawn.UnitType == typeof(Soldier)
                ? new Soldier(true, spawnX, BattlefieldY)
      : currentPlayerSpawn.UnitType == typeof(RangedUnit)
                ? new RangedUnit(true, spawnX, BattlefieldY)
      : new CavalryUnit(true, spawnX, BattlefieldY);
    
    PlayerUnits.Add(unit);
  currentPlayerSpawn = null;
        }
    }
  
    if (currentEnemySpawn == null && EnemySpawnQueue.Count > 0)
    {
  currentEnemySpawn = EnemySpawnQueue.Dequeue();
    }
    
    if (currentEnemySpawn != null)
    {
    currentEnemySpawn.Update(deltaTime);
        
        if (currentEnemySpawn.IsReady)
   {
     float spawnX = EnemyBase.Hitbox.Right - 20;
            bool canSpawn = true;
    
            foreach (var existingUnit in EnemyUnits)
            {
        if (Math.Abs(existingUnit.PositionX - spawnX) < 100)
                {
          canSpawn = false;
         break;
      }
            }
          
       if (canSpawn)
       {
       Unit unit = currentEnemySpawn.UnitType == typeof(Soldier)
  ? new Soldier(false, spawnX, BattlefieldY)
     : currentEnemySpawn.UnitType == typeof(RangedUnit)
     ? new RangedUnit(false, spawnX, BattlefieldY)
              : new CavalryUnit(false, spawnX, BattlefieldY);
    
      EnemyUnits.Add(unit);
      currentEnemySpawn = null;
     }
     }
    }
}

public float GetPlayerSpawnProgress()
{
    return currentPlayerSpawn?.Progress ?? 0f;
}

public int GetPlayerQueueCount()
{
    return PlayerSpawnQueue.Count + (currentPlayerSpawn != null ? 1 : 0);
}

public void ActivateTurretPlacement()
{
    ShowTurretSlot = true;
}

public void DeactivateTurretPlacement()
{
    ShowTurretSlot = false;
}

public void ActivateTurretSelling()
{
    ShowTurretSellHighlight = true;
}

public void DeactivateTurretSelling()
{
    ShowTurretSellHighlight = false;
}

public bool SellTurret()
{
  if (PlayerTurret == null)
        return false;
    
    int refund = PlayerTurret.Cost / 2;
    PlayerGold += refund;
    
PlayerTurret = null;
    ShowTurretSellHighlight = false;
    
    return true;
}

public bool SellTurretAt(Point clickPosition)
{
    for (int i = PlayerTurrets.Count - 1; i >= 0; i--)
  {
        if (PlayerTurrets[i].Hitbox.Contains(clickPosition))
    {
    int refund = PlayerTurrets[i].Cost / 2;
            PlayerGold += refund;
        PlayerTurrets.RemoveAt(i);
            ShowTurretSellHighlight = false;
   return true;
        }
    }
    return false;
}

public bool PlaceTurret(Type turretType, int cost, int specificSlotIndex = -1)
{
    if (PlayerGold < cost)
    return false;
    
    int slotIndex = -1;
    
    if (specificSlotIndex >= 0 && specificSlotIndex < TurretSlots.Count)
    {
        var slot = TurretSlots[specificSlotIndex];
        bool occupied = PlayerTurrets.Any(t =>
        Math.Abs(t.PositionX - (slot.X + slot.Width / 2)) < 5 &&
  Math.Abs(t.PositionY - (slot.Y + slot.Height)) < 5);
      
 if (!occupied)
        slotIndex = specificSlotIndex;
    }
    else
    {
        for (int i = 0; i < TurretSlots.Count; i++)
        {
    bool occupied = PlayerTurrets.Any(t =>
 Math.Abs(t.PositionX - (TurretSlots[i].X + TurretSlots[i].Width / 2)) < 5 &&
       Math.Abs(t.PositionY - (TurretSlots[i].Y + TurretSlots[i].Height)) < 5);
            
if (!occupied)
   {
        slotIndex = i;
         break;
       }
        }
  }
    
    if (slotIndex == -1)
    return false;
    
    var selectedSlot = TurretSlots[slotIndex];
    float turretX = selectedSlot.X + selectedSlot.Width / 2;
    int turretY = selectedSlot.Y + selectedSlot.Height;
    
    Turret newTurret = null;
    if (turretType == typeof(BasicTurret))
        newTurret = new BasicTurret(true, turretX, turretY);
    else if (turretType == typeof(AdvancedTurret))
        newTurret = new AdvancedTurret(true, turretX, turretY);
    else if (turretType == typeof(PremiumTurret))
        newTurret = new PremiumTurret(true, turretX, turretY);
    else
      return false;
  
    PlayerTurrets.Add(newTurret);
    PlayerGold -= cost;
    ShowTurretSlot = false;
    return true;
}

public bool ExpandBase()
{
    if (BaseExpansions >= MAX_EXPANSIONS)
        return false;
    
    if (PlayerGold < EXPANSION_COST)
        return false;
    
  PlayerGold -= EXPANSION_COST;
    BaseExpansions++;
    UpdateTurretSlots();
    return true;
}

// Aktivera ultimate attack - Fallande stenar
public void ActivateUltimate()
{
    if (!UltimateAvailable)
     return;
    
    // Beräkna antal stenar och spawn-intervall
    Random rand = new Random();
    int rockCount = rand.Next(8, 13);  // 8-12 stenar

    UltimateRocksToSpawn = rockCount;
    UltimateSpawnInterval = ULTIMATE_SPAWN_DURATION / rockCount;  // Jämnt fördelat över 3 sekunder
    UltimateSpawnTimer = 0;
    UltimateActive = true;
    
    // Sätt cooldown
  UltimateAvailable = false;
    UltimateCooldown = ULTIMATE_COOLDOWN_TIME;
}

// Uppdatera ultimate attack cooldown och fallande stenar
public void UpdateUltimate(int deltaTime)
{
    // Uppdatera cooldown
    if (!UltimateAvailable)
    {
        UltimateCooldown -= deltaTime;
        if (UltimateCooldown <= 0)
{
            UltimateCooldown = 0;
            UltimateAvailable = true;
        }
    }
    
   // Uppdatera spawn av nya stenar under ultimate-perioden
    if (UltimateActive)
    {
        UltimateSpawnTimer += deltaTime;
        
        // Spawna en sten varje intervall
        while (UltimateRocksToSpawn > 0 && UltimateSpawnTimer >= UltimateSpawnInterval)
  {
            Random rand = new Random();
            float rockX = rand.Next(100, screenWidth - 100);
     float rockY = -50 - rand.Next(100);
            
            FallingRocks.Add(new FallingRock(rockX, rockY));
    
  UltimateRocksToSpawn--;
            UltimateSpawnTimer -= UltimateSpawnInterval;
        }
   
        // Avsluta ultimate när alla stenar har spawnats
        if (UltimateRocksToSpawn <= 0)
        {
    UltimateActive = false;
            UltimateSpawnTimer = 0;
 }
    }
    
    // Uppdatera fallande stenar
 for (int i = FallingRocks.Count - 1; i >= 0; i--)
    {
        var rock = FallingRocks[i];
        rock.Update();
     
        // Kolla om stenen träffar marken
        if (rock.Y >= BattlefieldY)
{
rock.IsActive = false;
    FallingRocks.RemoveAt(i);
      continue;
        }
   
        // Kolla om stenen träffar fiendeenheter (instant kill)
        for (int j = EnemyUnits.Count - 1; j >= 0; j--)
        {
   var enemy = EnemyUnits[j];
     if (Utils.Intersects(rock.Hitbox, enemy.Hitbox))
  {
         // Instant kill!
       EnemyUnits.RemoveAt(j);
             EnemyScore++;
     
    // Ge guld
          int goldReward = enemy is Soldier ? 50 : enemy is RangedUnit ? 80 : enemy is CavalryUnit ? 200 : 50;
              PlayerGold += goldReward;
         
                // Ta bort stenen
       rock.IsActive = false;
      FallingRocks.RemoveAt(i);
      break;
         }
 }
    }
}

// Få cooldown-progress i procent (0-1)
public float GetUltimateCooldownProgress()
{
    if (UltimateAvailable)
        return 1f;
    
    return 1f - ((float)UltimateCooldown / ULTIMATE_COOLDOWN_TIME);
}
    }
}
