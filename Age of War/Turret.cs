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

    // Grundläggande turret (500 guld) - Enkel träkatapult
    public class BasicTurret : Turret
  {
    public BasicTurret(bool isPlayer, float posX, int posY) : base(isPlayer, posX, posY)
        {
    MaxHP = HP = 200;
  Damage = 7;  // Halverad från 15
  Cost = 500;
        AttackCooldown = 800; // ms
      }

     public override void Draw(Graphics g)
    {
        int x = (int)PositionX;
        int y = PositionY;

        // Beräkna katapult-vinkel baserat på attack animation
     int armAngle = isAttacking ? (attackAnimationTimer < 150 ? 45 : -20) : 0;

        // Bas/stativ av trä
        Color woodColor = Color.SaddleBrown;
        Color darkWood = Color.FromArgb(101, 67, 33);
        
        // Vänster ben
        Point[] leftLeg = new Point[]
    {
     new Point(x - 12, y),
  new Point(x - 8, y),
   new Point(x - 3, y - 25),
new Point(x - 6, y - 25)
  };
        g.FillPolygon(new SolidBrush(woodColor), leftLeg);
        g.DrawPolygon(new Pen(darkWood, 1), leftLeg);
        
        // Höger ben
        Point[] rightLeg = new Point[]
        {
        new Point(x + 3, y - 25),
            new Point(x + 6, y - 25),
            new Point(x + 8, y),
       new Point(x + 12, y)
  };
        g.FillPolygon(new SolidBrush(woodColor), rightLeg);
      g.DrawPolygon(new Pen(darkWood, 1), rightLeg);
        
   // Horisontell stödbjälke
   g.FillRectangle(new SolidBrush(darkWood), x - 10, y - 26, 20, 3);
        
        // Pivotpunkt (rep och trä)
        g.FillEllipse(new SolidBrush(darkWood), x - 3, y - 28, 6, 6);
        g.DrawEllipse(new Pen(Color.Black, 1), x - 3, y - 28, 6, 6);
        
   // Katapultarm (trästock)
        int armLength = 18;
  int armX = x;
 int armY = y - 25;
        double angleRad = armAngle * Math.PI / 180;
        int armEndX = armX + (int)(armLength * Math.Sin(angleRad));
        int armEndY = armY - (int)(armLength * Math.Cos(angleRad));
        
        g.DrawLine(new Pen(woodColor, 4), armX, armY, armEndX, armEndY);
 g.DrawLine(new Pen(darkWood, 2), armX, armY, armEndX, armEndY);
        
  // Sked/skål i änden (för stenen)
      g.FillEllipse(new SolidBrush(darkWood), armEndX - 4, armEndY - 4, 8, 8);
        
  // Projektil (sten) i skedan när inte skjuter
        if (!isAttacking || attackAnimationTimer < 150)
        {
g.FillEllipse(Brushes.Gray, armEndX - 3, armEndY - 3, 6, 6);
  g.DrawEllipse(Pens.Black, armEndX - 3, armEndY - 3, 6, 6);
    }
        
   // Rep som håller armen (spänd)
        g.DrawLine(new Pen(Color.FromArgb(139, 90, 43), 2), x, y - 5, x, y - 25);
        
        // HP-bar
        if (MaxHP > 0)
  {
   g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 7, Hitbox.Width, 4);
       g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 7, (int)(Hitbox.Width * ((float)HP / MaxHP)), 4);
}
    }
    }

    // Avancerad turret (1000 guld) - Förstärkt katapult med metall
    public class AdvancedTurret : Turret
    {
  public AdvancedTurret(bool isPlayer, float posX, int posY) : base(isPlayer, posX, posY)
   {
         MaxHP = HP = 300;
   Damage = 12;  // Halverad från 25
 Cost = 1000;
    AttackCooldown = 600; // ms
      }

   public override void Draw(Graphics g)
   {
            int x = (int)PositionX;
       int y = PositionY;
  
      // Beräkna katapult-vinkel
            int armAngle = isAttacking ? (attackAnimationTimer < 150 ? 50 : -25) : 0;

         // Färger
 Color woodColor = Color.SaddleBrown;
  Color darkWood = Color.FromArgb(101, 67, 33);
            Color metalColor = Color.Gray;
            Color darkMetal = Color.DarkGray;
            
            // Större och starkare bas
    // Vänster ben med metallförstärkning
     Point[] leftLeg = new Point[]
    {
new Point(x - 15, y),
  new Point(x - 10, y),
     new Point(x - 4, y - 30),
  new Point(x - 8, y - 30)
      };
   g.FillPolygon(new SolidBrush(woodColor), leftLeg);
   g.DrawPolygon(new Pen(darkWood, 2), leftLeg);
   // Metallband
  g.DrawLine(new Pen(metalColor, 2), x - 13, y - 5, x - 6, y - 25);
  g.DrawLine(new Pen(darkMetal, 1), x - 12, y - 5, x - 5, y - 25);
  
    // Höger ben med metallförstärkning
  Point[] rightLeg = new Point[]
        {
       new Point(x + 4, y - 30),
  new Point(x + 8, y - 30),
    new Point(x + 10, y),
            new Point(x + 15, y)
};
        g.FillPolygon(new SolidBrush(woodColor), rightLeg);
        g.DrawPolygon(new Pen(darkWood, 2), rightLeg);
// Metallband
        g.DrawLine(new Pen(metalColor, 2), x + 6, y - 25, x + 13, y - 5);
 g.DrawLine(new Pen(darkMetal, 1), x + 5, y - 25, x + 12, y - 5);
     
  // Horisontell stödbjälke (tjockare)
g.FillRectangle(new SolidBrush(darkWood), x - 13, y - 31, 26, 4);
            g.DrawLine(new Pen(metalColor, 1), x - 13, y - 29, x + 13, y - 29);
   
    // Metallförstärkt pivotpunkt
 g.FillEllipse(new SolidBrush(metalColor), x - 5, y - 33, 10, 10);
     g.FillEllipse(new SolidBrush(darkMetal), x - 3, y - 31, 6, 6);
     g.DrawEllipse(new Pen(Color.Black, 2), x - 5, y - 33, 10, 10);
  
     // Kraftigare katapultarm
       int armLength = 22;
     int armX = x;
       int armY = y - 28;
     double angleRad = armAngle * Math.PI / 180;
        int armEndX = armX + (int)(armLength * Math.Sin(angleRad));
  int armEndY = armY - (int)(armLength * Math.Cos(angleRad));
      
     g.DrawLine(new Pen(woodColor, 5), armX, armY, armEndX, armEndY);
     g.DrawLine(new Pen(metalColor, 2), armX, armY, armEndX, armEndY);
        g.DrawLine(new Pen(darkWood, 1), armX, armY, armEndX, armEndY);
        
   // Större sked med metallkanter
   g.FillEllipse(new SolidBrush(darkWood), armEndX - 5, armEndY - 5, 10, 10);
  g.DrawEllipse(new Pen(metalColor, 2), armEndX - 5, armEndY - 5, 10, 10);
        
        // Dubbla projektiler (stenar)
   if (!isAttacking || attackAnimationTimer < 150)
{
    g.FillEllipse(new SolidBrush(Color.Gray), armEndX - 4, armEndY - 3, 7, 7);
 g.DrawEllipse(Pens.Black, armEndX - 4, armEndY - 3, 7, 7);
      g.FillEllipse(new SolidBrush(Color.DarkGray), armEndX + 1, armEndY - 2, 5, 5);
   }
     
 // Dubbla rep (starkare spännkraft)
        g.DrawLine(new Pen(Color.FromArgb(139, 90, 43), 3), x - 2, y - 8, x - 2, y - 28);
        g.DrawLine(new Pen(Color.FromArgb(139, 90, 43), 3), x + 2, y - 8, x + 2, y - 28);
   
        // Metallband på rep
     g.DrawLine(new Pen(metalColor, 1), x - 2, y - 15, x + 2, y - 15);
      g.DrawLine(new Pen(metalColor, 1), x - 2, y - 22, x + 2, y - 22);
        
        // HP-bar
        if (MaxHP > 0)
 {
         g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 7, Hitbox.Width, 4);
      g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 7, (int)(Hitbox.Width * ((float)HP / MaxHP)), 4);
}
    }
    }

    // Premium turret (1500 guld) - Massiv katapult med gulddetaljer
  public class PremiumTurret : Turret
{
    public PremiumTurret(bool isPlayer, float posX, int posY) : base(isPlayer, posX, posY)
   {
    MaxHP = HP = 500;
   Damage = 20;  // Halverad från 40
  Cost = 1500;
       AttackCooldown = 1000; // ms
      }

     public override void Draw(Graphics g)
  {
    int x = (int)PositionX;
    int y = PositionY;
     
  // Beräkna katapult-vinkel
   int armAngle = isAttacking ? (attackAnimationTimer < 150 ? 55 : -30) : 0;

     // Färger - lyxiga
   Color woodColor = Color.FromArgb(139, 90, 43);
     Color darkWood = Color.FromArgb(101, 67, 33);
       Color goldColor = Color.Gold;
Color darkGold = Color.DarkGoldenrod;
         Color crystalColor = Color.Cyan;
         
// Massiv bas med guldfötter
   // Vänster ben - extra brett
  Point[] leftLeg = new Point[]
   {
   new Point(x - 18, y),
new Point(x - 12, y),
       new Point(x - 5, y - 35),
      new Point(x - 10, y - 35)
    };
g.FillPolygon(new SolidBrush(woodColor), leftLeg);
       g.DrawPolygon(new Pen(darkWood, 2), leftLeg);
// Guldband
       g.DrawLine(new Pen(goldColor, 3), x - 16, y - 5, x - 7, y - 30);
    g.DrawLine(new Pen(darkGold, 1), x - 15, y - 5, x - 6, y - 30);
     
  // Höger ben - extra brett
  Point[] rightLeg = new Point[]
        {
   new Point(x + 5, y - 35),
     new Point(x + 10, y - 35),
          new Point(x + 12, y),
   new Point(x + 18, y)
   };
   g.FillPolygon(new SolidBrush(woodColor), rightLeg);
  g.DrawPolygon(new Pen(darkWood, 2), rightLeg);
  // Guldband
     g.DrawLine(new Pen(goldColor, 3), x + 7, y - 30, x + 16, y - 5);
     g.DrawLine(new Pen(darkGold, 1), x + 6, y - 30, x + 15, y - 5);
     
     // Horisontell stödbjälke (tjockast)
    g.FillRectangle(new SolidBrush(darkWood), x - 16, y - 36, 32, 5);
   g.FillRectangle(new SolidBrush(goldColor), x - 15, y - 35, 30, 2);
    
   // Guldförstärkt pivotpunkt med kristall
        g.FillEllipse(new SolidBrush(goldColor), x - 6, y - 38, 12, 12);
g.FillEllipse(new SolidBrush(darkGold), x - 4, y - 36, 8, 8);
  g.FillEllipse(new SolidBrush(crystalColor), x - 2, y - 34, 4, 4);
     g.DrawEllipse(new Pen(Color.Black, 2), x - 6, y - 38, 12, 12);
      
   // Massiv katapultarm med gulddetaljer
       int armLength = 26;
     int armX = x;
       int armY = y - 32;
    double angleRad = armAngle * Math.PI / 180;
        int armEndX = armX + (int)(armLength * Math.Sin(angleRad));
   int armEndY = armY - (int)(armLength * Math.Cos(angleRad));
   
        g.DrawLine(new Pen(woodColor, 6), armX, armY, armEndX, armEndY);
    g.DrawLine(new Pen(goldColor, 3), armX, armY, armEndX, armEndY);
     g.DrawLine(new Pen(darkWood, 1), armX, armY, armEndX, armEndY);
  
 // Guldband på armen
      int midX = armX + (armEndX - armX) / 2;
   int midY = armY + (armEndY - armY) / 2;
  g.DrawLine(new Pen(goldColor, 4), midX - 2, midY - 2, midX + 2, midY + 2);
 
     // Stor förstärkt sked med kristaller
 g.FillEllipse(new SolidBrush(darkWood), armEndX - 6, armEndY - 6, 12, 12);
        g.DrawEllipse(new Pen(goldColor, 3), armEndX - 6, armEndY - 6, 12, 12);
        
   // Kristallprydnader på skedan
   g.FillEllipse(new SolidBrush(crystalColor), armEndX - 5, armEndY - 1, 3, 3);
   g.FillEllipse(new SolidBrush(Color.Red), armEndX + 2, armEndY - 1, 3, 3);
 
        // Tre projektiler (stora stenar med kristaller)
 if (!isAttacking || attackAnimationTimer < 150)
        {
// Stor sten
      g.FillEllipse(new SolidBrush(Color.Gray), armEndX - 4, armEndY - 3, 8, 8);
g.DrawEllipse(Pens.Black, armEndX - 4, armEndY - 3, 8, 8);
      // Kristall på stenen
  g.FillEllipse(new SolidBrush(crystalColor), armEndX - 1, armEndY, 2, 2);
    
  // Mindre stenar
  g.FillEllipse(new SolidBrush(Color.DarkGray), armEndX - 5, armEndY + 2, 5, 5);
    g.FillEllipse(new SolidBrush(Color.LightGray), armEndX + 2, armEndY + 2, 5, 5);
}
     
  // Tre kraftiga rep med guldband
        g.DrawLine(new Pen(Color.FromArgb(139, 90, 43), 4), x - 3, y - 10, x - 3, y - 32);
        g.DrawLine(new Pen(Color.FromArgb(139, 90, 43), 4), x, y - 10, x, y - 32);
   g.DrawLine(new Pen(Color.FromArgb(139, 90, 43), 4), x + 3, y - 10, x + 3, y - 32);
    
   // Guldband på repen
        g.DrawLine(new Pen(goldColor, 2), x - 4, y - 18, x + 4, y - 18);
        g.DrawLine(new Pen(goldColor, 2), x - 4, y - 25, x + 4, y - 25);
     
        // Dekorativa kristaller på basen
   g.FillEllipse(new SolidBrush(crystalColor), x - 10, y - 35, 4, 4);
        g.FillEllipse(new SolidBrush(Color.Red), x + 6, y - 35, 4, 4);
        
        // HP-bar
        if (MaxHP > 0)
        {
  g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 7, Hitbox.Width, 4);
       g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 7, (int)(Hitbox.Width * ((float)HP / MaxHP)), 4);
      }
    }
    }
}
