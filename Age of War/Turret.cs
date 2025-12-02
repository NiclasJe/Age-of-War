using System.Drawing;

namespace Age_of_War
{
    // Abstrakt bas-klass för turrets (stationära försvarstorn)
    public abstract class Turret
    {
        public int HP;
    public int MaxHP;
  public int Damage;
        public int Cost;
      public float PositionX;
        public int PositionY;
    public Rectangle Hitbox => new Rectangle((int)PositionX - 10, PositionY - 40, 20, 40);  // Hälften så stort (var 30x60)
        public bool IsPlayer;
        public int AttackCooldown; // ms
        public int AttackTimer; // ms
        public bool IsDead => HP <= 0;
        
        // Animation för attack
        protected int attackAnimationTimer = 0;
        protected bool isAttacking = false;

        public Turret(bool isPlayer, float posX, int posY)
        {
  IsPlayer = isPlayer;
            PositionX = posX;
     PositionY = posY;
   }

        public virtual void Update(int deltaTime)
      {
            AttackTimer += deltaTime;
            
  if (isAttacking)
            {
     attackAnimationTimer += deltaTime;
        if (attackAnimationTimer >= 300)
   {
    isAttacking = false;
     attackAnimationTimer = 0;
                }
 }
  }

   public bool CanAttack => AttackTimer >= AttackCooldown;
 
        public void ResetAttackTimer()
        {
    AttackTimer = 0;
      isAttacking = true;
            attackAnimationTimer = 0;
    }

        public abstract void Draw(Graphics g);
  }

    // Grundläggande turret (500 guld)
    public class BasicTurret : Turret
  {
        public BasicTurret(bool isPlayer, float posX, int posY) : base(isPlayer, posX, posY)
        {
    MaxHP = HP = 200;
          Damage = 15;
  Cost = 500;
            AttackCooldown = 800; // ms
        }

        public override void Draw(Graphics g)
    {
        int x = (int)PositionX;
        int y = PositionY;

        // Bas/plattform - mindre
        g.FillRectangle(Brushes.SaddleBrown, x - 13, y - 7, 26, 7);
        
        // Torn kropp (sten) - hälften så stor
        g.FillRectangle(Brushes.Gray, x - 10, y - 33, 20, 26);
  g.DrawRectangle(Pens.DarkGray, x - 10, y - 33, 20, 26);
        
        // Kanon - mindre
        int cannonOffset = isAttacking ? (attackAnimationTimer < 150 ? -2 : 2) : 0;
        g.FillRectangle(Brushes.DarkSlateGray, x - 3 + cannonOffset, y - 23, 13, 5);
        g.FillEllipse(Brushes.Black, x + 9 + cannonOffset, y - 22, 3, 3);
 
        // Detaljer - mindre
        g.FillRectangle(Brushes.DarkGray, x - 8, y - 30, 16, 2);
        g.FillRectangle(Brushes.DarkGray, x - 8, y - 17, 16, 2);
        
        // HP-bar
        if (MaxHP > 0)
  {
      g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 7, Hitbox.Width, 4);
            g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 7, (int)(Hitbox.Width * ((float)HP / MaxHP)), 4);
        }
    }
    }

    // Avancerad turret (1000 guld)
    public class AdvancedTurret : Turret
    {
        public AdvancedTurret(bool isPlayer, float posX, int posY) : base(isPlayer, posX, posY)
        {
            MaxHP = HP = 300;
            Damage = 25;
 Cost = 1000;
    AttackCooldown = 600; // ms
      }

   public override void Draw(Graphics g)
        {
            int x = (int)PositionX;
       int y = PositionY;
  
            // Bas/plattform (större) - mindre
   g.FillRectangle(Brushes.SaddleBrown, x - 15, y - 8, 30, 8);
     
          // Torn kropp (metall) - hälften så stor
 using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
   new Rectangle(x - 10, y - 36, 20, 28),
            Color.LightSlateGray,
              Color.DarkSlateGray,
              System.Drawing.Drawing2D.LinearGradientMode.Vertical))
       {
 g.FillRectangle(brush, x - 10, y - 36, 20, 28);
    }
            g.DrawRectangle(new Pen(Color.Black, 2), x - 10, y - 36, 20, 28);
        
  // Dubbla kanoner - mindre
    int cannonOffset = isAttacking ? (attackAnimationTimer < 150 ? -3 : 3) : 0;
  g.FillRectangle(Brushes.Black, x - 3 + cannonOffset, y - 28, 15, 4);
        g.FillRectangle(Brushes.Black, x - 3 + cannonOffset, y - 20, 15, 4);
      
        // Detaljer/pansar - mindre
        g.DrawRectangle(Pens.Yellow, x - 8, y - 33, 16, 4);
        g.DrawRectangle(Pens.Yellow, x - 8, y - 27, 16, 4);
        g.DrawRectangle(Pens.Yellow, x - 8, y - 20, 16, 4);

     // HP-bar
      if (MaxHP > 0)
 {
         g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 7, Hitbox.Width, 4);
          g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 7, (int)(Hitbox.Width * ((float)HP / MaxHP)), 4);
}
    }
    }

    // Premium turret (1500 guld)
  public class PremiumTurret : Turret
{
        public PremiumTurret(bool isPlayer, float posX, int posY) : base(isPlayer, posX, posY)
        {
    MaxHP = HP = 500;
     Damage = 40;
       Cost = 1500;
            AttackCooldown = 1000; // ms
        }

     public override void Draw(Graphics g)
  {
    int x = (int)PositionX;
    int y = PositionY;
     
     // Stor bas/plattform - mindre
        g.FillRectangle(Brushes.SaddleBrown, x - 17, y - 10, 34, 10);
      g.DrawRectangle(Pens.Black, x - 17, y - 10, 34, 10);
      
        // Massivt torn (guld/metall) - hälften så stor
   using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
            new Rectangle(x - 12, y - 43, 24, 33),
  Color.Gold,
            Color.DarkGoldenrod,
            System.Drawing.Drawing2D.LinearGradientMode.Vertical))
        {
     g.FillRectangle(brush, x - 12, y - 43, 24, 33);
        }
        g.DrawRectangle(new Pen(Color.Black, 2), x - 12, y - 43, 24, 33);
 
     // Massiv kanon - mindre
     int cannonOffset = isAttacking ? (attackAnimationTimer < 150 ? -3 : 3) : 0;
  g.FillRectangle(Brushes.DarkSlateGray, x - 5 + cannonOffset, y - 30, 18, 8);
        g.FillEllipse(Brushes.Black, x + 12 + cannonOffset, y - 29, 4, 6);

        // Extra kanonrör ovanför - mindre
  g.FillRectangle(Brushes.DarkSlateGray, x - 3 + cannonOffset, y - 38, 14, 5);
        
        // Kristaller/ädelstenar - mindre
        g.FillEllipse(Brushes.Cyan, x - 7, y - 23, 5, 5);
     g.FillEllipse(Brushes.Cyan, x + 2, y - 23, 5, 5);
  g.FillEllipse(Brushes.Red, x - 3, y - 17, 5, 5);
 
        // HP-bar
        if (MaxHP > 0)
        {
     g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 7, Hitbox.Width, 4);
       g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 7, (int)(Hitbox.Width * ((float)HP / MaxHP)), 4);
      }
    }
    }
}
