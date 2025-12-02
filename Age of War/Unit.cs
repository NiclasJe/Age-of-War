using System.Drawing;

namespace Age_of_War
{
    // Abstrakt bas-klass för alla enheter
    public abstract class Unit
    {
        public int HP;
        public int MaxHP;
   public int Damage;
     public int Cost;
        public float Speed;
        public float PositionX;
        public int PositionY;
        public Rectangle Hitbox => new Rectangle((int)PositionX, PositionY - 50, 30, 50);
        public bool IsPlayer; // true = vänster->höger, false = höger->vänster
  public int AttackCooldown; // ms
        public int AttackTimer; // ms
        public bool IsDead => HP <= 0;
        public bool IsBlocked = false; // Om enheten är blockerad av annan enhet/bas
      
        // Animation
        protected int animationFrame = 0;
  protected int animationTimer = 0;
        protected bool isAttacking = false;
        protected int attackAnimationTimer = 0;

        public Unit(bool isPlayer, float startX, int y)
     {
            IsPlayer = isPlayer;
         PositionX = startX;
      PositionY = y;
        }

        // Uppdatera position och attacktimer
   public virtual void Update(int deltaTime)
        {
            // Rör sig endast om inte blockerad
            if (!IsBlocked)
            {
       PositionX += IsPlayer ? Speed : -Speed;
    // Walk animation
   animationTimer += deltaTime;
      if (animationTimer >= 200) // Byt frame var 200ms
{
    animationFrame = (animationFrame + 1) % 4;
    animationTimer = 0;
        }
            }
          
     AttackTimer += deltaTime;
            
       // Attack animation
    if (isAttacking)
    {
            attackAnimationTimer += deltaTime;
            if (attackAnimationTimer >= 300) // Attack animation varar 300ms
        {
          isAttacking = false;
  attackAnimationTimer = 0;
            }
        }
        }

        // Försök attackera en annan enhet
      public bool CanAttack => AttackTimer >= AttackCooldown;
    
        public void ResetAttackTimer() 
        { 
       AttackTimer = 0;
 isAttacking = true;
       attackAnimationTimer = 0;
        }

        // Rita placeholder-grafik
        public abstract void Draw(Graphics g);
        
        // Hjälpmetod för att rita grundläggande grottmänniska i profil
        protected void DrawCaveman(Graphics g, Color skinColor, bool hasClub)
        {
    int x = (int)PositionX;
    int y = PositionY;
    
    // Riktning (vänster eller höger)
    int direction = IsPlayer ? 1 : -1;
    
    // Beräkna walk animation offset för ben och armar
    int frontLegOffset = 0;
    int backLegOffset = 0;
    int frontArmSwing = 0;
    int backArmSwing = 0;
    
    if (!IsBlocked && !isAttacking)
    {
        // Smooth walk cycle med sinus-kurva för naturligare rörelse
        float walkCycle = (animationFrame % 4) / 4.0f;
        
        // Ben - går i motsatt fas
        if (animationFrame % 4 == 0)
      {
      frontLegOffset = 0;
            backLegOffset = 0;
            frontArmSwing = 0;
            backArmSwing = 0;
        }
        else if (animationFrame % 4 == 1)
 {
 frontLegOffset = 12;  // Ökat från 8
            backLegOffset = -6;   // Ökat från -4
      frontArmSwing = -8;   // Armen svingar bakåt när benet går framåt
            backArmSwing = 6;// Bakre armen svingar framåt
        }
        else if (animationFrame % 4 == 2)
        {
         frontLegOffset = 0;
 backLegOffset = 0;
         frontArmSwing = 0;
     backArmSwing = 0;
        }
        else // animationFrame % 4 == 3
    {
  frontLegOffset = -6;  // Ökat från -4
   backLegOffset = 12;   // Ökat från 8
       frontArmSwing = 6;    // Framåt
            backArmSwing = -8;    // Bakåt
        }
    }
    
    // Attack animation override
    int armSwingOffset = 0;
    int armSwingY = 0;
    if (isAttacking)
    {
        if (attackAnimationTimer < 100)
        {
     // Dra tillbaka armen
          armSwingOffset = -12 * direction;
            armSwingY = -8;
        }
 else if (attackAnimationTimer < 200)
        {
            // Slå/skjut framåt
            armSwingOffset = 18 * direction;
            armSwingY = 3;
        }
        else
{
         // Återgå
            armSwingOffset = 6 * direction;
            armSwingY = 0;
        }
    }
    
  // Rita bakre ben först (bakom kroppen)
    int backLegX = x + 12;
    g.FillRectangle(new SolidBrush(skinColor), backLegX, y - 28 + backLegOffset, 6, 14);
    g.FillRectangle(new SolidBrush(skinColor), backLegX, y - 14 + backLegOffset, 6, 14);
  // Bakre fot
    g.FillEllipse(new SolidBrush(Color.SaddleBrown), backLegX - 2 + (direction * 2), y - 2, 10, 5);
    
    // Rita bakre arm (bakom kroppen)
    if (!hasClub && !isAttacking)
    {
 int backArmX = x + 12;
        // Överarm
        g.FillRectangle(new SolidBrush(skinColor), backArmX + (backArmSwing / 2), y - 38 + (backArmSwing / 3), 6, 10);
        // Underarm
    g.FillRectangle(new SolidBrush(skinColor), backArmX + backArmSwing, y - 30 + (backArmSwing / 2), 6, 10);
    }
    
    // Kropp (oval)
    g.FillEllipse(new SolidBrush(skinColor), x + 5, y - 42, 20, 28);
    
    // Huvud (cirkel i profil)
    g.FillEllipse(new SolidBrush(skinColor), x + 8, y - 55, 18, 18);
    
    // Öga
  int eyeX = x + (IsPlayer ? 20 : 12);
    g.FillEllipse(Brushes.White, eyeX, y - 50, 5, 5);
    g.FillEllipse(Brushes.Black, eyeX + 1, y - 49, 3, 3);
 
    // Näsa (liten triangel)
    Point[] nose = new Point[]
    {
     new Point(x + (IsPlayer ? 25 : 9), y - 47),
        new Point(x + (IsPlayer ? 27 : 7), y - 45),
        new Point(x + (IsPlayer ? 25 : 9), y - 43)
    };
    g.FillPolygon(new SolidBrush(Color.FromArgb(skinColor.R - 20, skinColor.G - 20, skinColor.B - 20)), nose);
 
    // Mun
    g.DrawArc(Pens.Black, x + (IsPlayer ? 15 : 8), y - 44, 8, 4, 0, 180);
    
    // Hår (rufsigt)
    g.FillEllipse(Brushes.Brown, x + 8, y - 57, 7, 7);
    g.FillEllipse(Brushes.Brown, x + 13, y - 59, 7, 7);
    g.FillEllipse(Brushes.Brown, x + 18, y - 57, 7, 7);
    
    // Rita främre ben (framför kroppen)
    int frontLegX = x + 12;
    g.FillRectangle(new SolidBrush(skinColor), frontLegX, y - 28 + frontLegOffset, 6, 14);
    g.FillRectangle(new SolidBrush(skinColor), frontLegX, y - 14 + frontLegOffset, 6, 14);
    // Främre fot
    g.FillEllipse(new SolidBrush(Color.SaddleBrown), frontLegX - 2 + (direction * 2), y - 2, 10, 5);
    
    // Rita främre arm med vapen (framför kroppen)
 int armX = x + (IsPlayer ? 20 : 8);
    
    if (isAttacking)
    {
        // Attack animation
        // Överarm
        g.FillRectangle(new SolidBrush(skinColor), armX - (armSwingOffset / 2), y - 38 + (armSwingY / 2), 6, 10);
        // Underarm  
        g.FillRectangle(new SolidBrush(skinColor), armX - armSwingOffset, y - 30 + armSwingY, 6, 10);
        // Hand
        g.FillEllipse(new SolidBrush(skinColor), armX - armSwingOffset - 2, y - 22 + armSwingY, 5, 5);
    }
    else
    {
        // Walk animation för främre arm
 // Överarm
        g.FillRectangle(new SolidBrush(skinColor), armX + (frontArmSwing / 2), y - 38 + (frontArmSwing / 3), 6, 10);
        // Underarm
        g.FillRectangle(new SolidBrush(skinColor), armX + frontArmSwing, y - 30 + (frontArmSwing / 2), 6, 10);
      // Hand
  if (!hasClub)
{
            g.FillEllipse(new SolidBrush(skinColor), armX + frontArmSwing - 2, y - 22 + (frontArmSwing / 2), 5, 5);
        }
    }
}
    }
}
