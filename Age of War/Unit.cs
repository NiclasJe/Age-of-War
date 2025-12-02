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
   
        // Dödsanimation
        public bool IsDying = false;  // Om enheten håller på att dö
        public int DeathTimer = 0;  // ms sedan död
        public const int DEATH_DURATION = 3000;  // 3 sekunder
        public bool IsFullyDead => IsDying && DeathTimer >= DEATH_DURATION;  // Färdig att tas bort
      public float DeathAlpha => IsDying ? 1f - ((float)DeathTimer / DEATH_DURATION) : 1f;  // Fade-out
        public float DeathCollapseProgress => IsDying ? Math.Min(1f, (float)DeathTimer / 500f) : 0f;  // Faller ihop på 500ms

        public Unit(bool isPlayer, float startX, int y)
     {
      IsPlayer = isPlayer;
   PositionX = startX;
    PositionY = y;
        }

        // Uppdatera position och attacktimer
   public virtual void Update(int deltaTime)
        {
         // Om enheten dör, starta dödsanimation
            if (HP <= 0 && !IsDying)
            {
       IsDying = true;
   DeathTimer = 0;
            }
       
   // Uppdatera dödsanimation
      if (IsDying)
            {
           DeathTimer += deltaTime;
         return;  // Gör inget annat när enheten dör
            }
            
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
  int y = PositionY + (int)(DeathCollapseProgress * 40);  // LÄGG TILL collapseOffset HÄR!
       
        // Riktning (vänster eller höger)
        int direction = IsPlayer ? 1 : -1;
 
  // Beräkna alpha för fade-out
     int alpha = (int)(DeathAlpha * 255);
    
 // Modifiera färger med alpha - ANVÄND DESSA ISTÄLLET!
         Color fadedSkinColor = Color.FromArgb(alpha, skinColor);
   Color fadedDarkSkin = Color.FromArgb(alpha, skinColor.R - 30, skinColor.G - 30, skinColor.B - 30);
    Color fadedLightSkin = Color.FromArgb(alpha, 
         Math.Min(255, skinColor.R + 20), 
Math.Min(255, skinColor.G + 20), 
    Math.Min(255, skinColor.B + 20));
         Color fadedBrown = Color.FromArgb(alpha, Color.Brown);
       Color fadedSaddleBrown = Color.FromArgb(alpha, Color.SaddleBrown);
       Color fadedFurColor = Color.FromArgb(alpha, 101, 67, 33);
     Color fadedHairColor = Color.FromArgb(alpha, Color.Brown);
  Color fadedDarkHair = Color.FromArgb(alpha, 80, 50, 20);

        // Beräkna walk animation offset för ben och armar
 int frontLegOffsetX = 0;
  int frontLegOffsetY = 0;
int backLegOffsetX = 0;
          int backLegOffsetY = 0;
int frontArmSwing = 0;
       int backArmSwing = 0;
          
       if (!IsBlocked && !isAttacking && !IsDying)  // Ingen animation när man dör
     {
       // Smooth walk cycle med sinus-kurva för naturligare rörelse
     float walkCycle = (animationFrame % 4) / 4.0f;
 
     // Ben - svingar naturligt framåt och bakåt med både X och Y komponenter
if (animationFrame % 4 == 0)
      {
             frontLegOffsetX = 0;
         frontLegOffsetY = 0;
         backLegOffsetX = 0;
            backLegOffsetY = 0;
frontArmSwing = 0;
         backArmSwing = 0;
  }
      else if (animationFrame % 4 == 1)
 {
         // Främre ben går framåt (upp och fram)
         frontLegOffsetX = 8 * direction;
       frontLegOffsetY = -6;  // Lyfter upp lite
         // Bakre ben går bakåt (ner och bak)
                backLegOffsetX = -4 * direction;
    backLegOffsetY = 2;
 
       frontArmSwing = -8;   // Armen svingar bakåt när benet går framåt
        backArmSwing = 6;   // Bakre armen svingar framåt
    }
            else if (animationFrame % 4 == 2)
     {
      frontLegOffsetX = 0;
 frontLegOffsetY = 0;
       backLegOffsetX = 0;
    backLegOffsetY = 0;
           frontArmSwing = 0;
   backArmSwing = 0;
  }
       else // animationFrame % 4 == 3
                {
                // Bakre ben går framåt (upp och fram)
      backLegOffsetX = 8 * direction;
         backLegOffsetY = -6;  // Lyfter upp lite
          // Främre ben går bakåt (ner och bak)
  frontLegOffsetX = -4 * direction;
           frontLegOffsetY = 2;
          
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
            
         // Beräkna skuggfärger
   Color darkSkin = Color.FromArgb(skinColor.R - 30, skinColor.G - 30, skinColor.B - 30);
  Color lightSkin = Color.FromArgb(
       Math.Min(255, skinColor.R + 20), 
         Math.Min(255, skinColor.G + 20), 
       Math.Min(255, skinColor.B + 20)
  );
         
     // Rita bakre ben först (bakom kroppen)
            int backLegX = x + 12 + backLegOffsetX;
   
   // Bakre lår med muskler
g.FillRectangle(new SolidBrush(fadedSkinColor), backLegX, y - 28 + backLegOffsetY, 6, 14);
 g.DrawLine(new Pen(fadedDarkSkin, 1), backLegX + 2, y - 26 + backLegOffsetY, backLegX + 2, y - 16 + backLegOffsetY);
       
   // Bakre knä (lite rundning)
            g.FillEllipse(new SolidBrush(fadedDarkSkin), backLegX + 1, y - 16 + backLegOffsetY, 4, 4);
     
    // Bakre vad med muskler
     g.FillRectangle(new SolidBrush(fadedSkinColor), backLegX, y - 14 + backLegOffsetY, 6, 14);
       g.DrawLine(new Pen(fadedDarkSkin, 1), backLegX + 3, y - 12 + backLegOffsetY, backLegX + 3, y - 4 + backLegOffsetY);
      
 // Bakre fot med tår
   g.FillEllipse(new SolidBrush(fadedSaddleBrown), backLegX - 2 + (direction * 2), y - 2, 10, 5);
    // Tår på bakre fot
            for (int i = 0; i < 3; i++)
         {
 g.FillEllipse(new SolidBrush(fadedDarkSkin), backLegX - 1 + (direction * 2) + (i * 2), y - 1, 2, 2);
 }
            
        // Rita bakre arm (bakom kroppen)
  if (!hasClub && !isAttacking)
     {
    int backArmX = x + 12;
           
            // Bakre axel
     g.FillEllipse(new SolidBrush(fadedDarkSkin), backArmX - 1, y - 40, 6, 6);
         
      // Bakre överarm med muskler
    g.FillRectangle(new SolidBrush(fadedSkinColor), backArmX + (backArmSwing / 2), y - 38 + (backArmSwing / 3), 6, 10);
g.DrawLine(new Pen(fadedDarkSkin, 1), backArmX + 2 + (backArmSwing / 2), y - 37 + (backArmSwing / 3), backArmX + 2 + (backArmSwing / 2), y - 30 + (backArmSwing / 3));
   
            // Bakre armbåge
       g.FillEllipse(new SolidBrush(fadedDarkSkin), backArmX + 1 + (backArmSwing / 2), y - 30 + (backArmSwing / 3), 4, 4);
     
    // Bakre underarm
       g.FillRectangle(new SolidBrush(fadedSkinColor), backArmX + backArmSwing, y - 30 + (backArmSwing / 2), 6, 10);
    
    // Bakre hand med fingrar
     g.FillEllipse(new SolidBrush(fadedSkinColor), backArmX + backArmSwing - 1, y - 22 + (backArmSwing / 2), 6, 5);
     // Fingrar
 g.FillRectangle(new SolidBrush(fadedDarkSkin), backArmX + backArmSwing + 3, y - 21 + (backArmSwing / 2), 2, 3);
   g.FillRectangle(new SolidBrush(fadedDarkSkin), backArmX + backArmSwing + 3, y - 19 + (backArmSwing / 2), 2, 2);
    }
  
      // Kropp (oval) med mer detaljer
      g.FillEllipse(new SolidBrush(fadedSkinColor), x + 5, y - 42, 20, 28);
          
    // Bröstmuskler
   g.DrawArc(new Pen(fadedDarkSkin, 2), x + 8, y - 38, 7, 8, 180, 180);
            g.DrawArc(new Pen(fadedDarkSkin, 2), x + 13, y - 38, 7, 8, 180, 180);
        
  // Revben (subtila linjer)
   for (int i = 0; i < 3; i++)
      {
    g.DrawLine(new Pen(fadedDarkSkin, 1), x + 10, y - 28 + (i * 3), x + 18, y - 28 + (i * 3));
       }
 
      // Primitiv klädsel - djurskinn runt midjan
 g.FillEllipse(new SolidBrush(fadedFurColor), x + 8, y - 22, 14, 10);
   // Päls-textur
      for (int i = 0; i < 6; i++)
   {
  g.DrawLine(new Pen(Color.FromArgb(alpha, 80, 50, 20), 2), x + 9 + (i * 2), y - 20, x + 9 + (i * 2), y - 15);
            }
       
// Bälte av läder/vin
            g.DrawLine(new Pen(fadedSaddleBrown, 2), x + 7, y - 21, x + 22, y - 21);
            
   // Huvud (cirkel i profil)
       g.FillEllipse(new SolidBrush(fadedSkinColor), x + 8, y - 55, 18, 18);
            
    // Öronkontur (baktill)
   if (!IsPlayer)  // Syns bara från vänster sida
g.FillEllipse(new SolidBrush(fadedDarkSkin), x + 10, y - 48, 4, 6);
      else  // Syns bara från höger sida
       g.FillEllipse(new SolidBrush(fadedDarkSkin), x + 22, y - 48, 4, 6);
     
     // Ansiktsdetaljer - mer detaljerat
  
     // Ögonbryn
         int eyeBrowX = x + (IsPlayer ? 19 : 12);
 g.DrawLine(new Pen(fadedBrown, 2), eyeBrowX - 2, y - 52, eyeBrowX + 3, y - 52);

  // Öga med ögonlock
      int eyeX = x + (IsPlayer ? 20 : 12);
      g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.White)), eyeX, y - 50, 5, 5);
 g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Black)), eyeX + 1, y - 49, 3, 3);
// Pupill
  g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Black)), eyeX + 2, y - 48, 1, 2);
  // Ögonlock
   g.DrawArc(new Pen(fadedDarkSkin, 1), eyeX, y - 50, 5, 3, 180, 180);
      
    // Näsa (mer detaljerad)
 Point[] nose = new Point[]
      {
           new Point(x + (IsPlayer ? 25 : 9), y - 47),
      new Point(x + (IsPlayer ? 27 : 7), y - 45),
 new Point(x + (IsPlayer ? 25 : 9), y - 43)
     };
       g.FillPolygon(new SolidBrush(fadedDarkSkin), nose);
       // Näsborre
g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Black)), x + (IsPlayer ? 26 : 8), y - 45, 1, 2);

            // Mun med läppar
   g.DrawArc(new Pen(Color.FromArgb(alpha, 139, 69, 19), 2), x + (IsPlayer ? 15 : 8), y - 44, 8, 4, 0, 180);
// Underläpp
          g.DrawArc(new Pen(fadedDarkSkin, 1), x + (IsPlayer ? 16 : 9), y - 42, 6, 2, 180, 180);
   
    // Haka (mer definierad)
    g.FillEllipse(new SolidBrush(fadedDarkSkin), x + (IsPlayer ? 17 : 10), y - 40, 5, 4);
      
        // Hår (rufsigt och mer detaljerat)
    // Flera hårlockar
       g.FillEllipse(new SolidBrush(fadedHairColor), x + 8, y - 57, 7, 7);
      g.FillEllipse(new SolidBrush(fadedDarkHair), x + 9, y - 56, 3, 4);
    
       g.FillEllipse(new SolidBrush(fadedHairColor), x + 13, y - 59, 7, 7);
g.FillEllipse(new SolidBrush(fadedDarkHair), x + 14, y - 58, 3, 4);
      
 g.FillEllipse(new SolidBrush(fadedHairColor), x + 18, y - 57, 7, 7);
  g.FillEllipse(new SolidBrush(fadedDarkHair), x + 19, y - 56, 3, 4);
       
  // Extra tofsar
       g.FillEllipse(new SolidBrush(fadedHairColor), x + 11, y - 60, 4, 5);
  g.FillEllipse(new SolidBrush(fadedHairColor), x + 16, y - 60, 4, 5);
   
        // Skägg (grottmänniskor har skägg!)
g.FillEllipse(new SolidBrush(fadedHairColor), x + (IsPlayer ? 15 : 10), y - 42, 6, 6);
      g.FillEllipse(new SolidBrush(fadedHairColor), x + (IsPlayer ? 18 : 8), y - 40, 5, 5);
      // Skägg-textur
   for (int i = 0; i < 3; i++)
   {
   g.DrawLine(new Pen(fadedDarkHair, 1), x + (IsPlayer ? 16 : 11) + i, y - 40, x + (IsPlayer ? 16 : 11) + i, y - 37);
 }
        
        // Rita främre ben (framför kroppen)
        int frontLegX = x + 12 + frontLegOffsetX;
         
            // Främre lår med muskler
    g.FillRectangle(new SolidBrush(fadedSkinColor), frontLegX, y - 28 + frontLegOffsetY, 6, 14);
     g.DrawLine(new Pen(fadedLightSkin, 1), frontLegX + 3, y - 26 + frontLegOffsetY, frontLegX + 3, y - 16 + frontLegOffsetY);
    g.DrawLine(new Pen(fadedDarkSkin, 1), frontLegX + 1, y - 26 + frontLegOffsetY, frontLegX + 1, y - 16 + frontLegOffsetY);
     
   // Främre knä (lite rundning)
 g.FillEllipse(new SolidBrush(fadedDarkSkin), frontLegX + 1, y - 16 + frontLegOffsetY, 4, 4);
   
     // Främre vad med muskler
       g.FillRectangle(new SolidBrush(fadedSkinColor), frontLegX, y - 14 + frontLegOffsetY, 6, 14);
         g.DrawLine(new Pen(fadedLightSkin, 1), frontLegX + 4, y - 12 + frontLegOffsetY, frontLegX + 4, y - 4 + frontLegOffsetY);
      g.DrawLine(new Pen(fadedDarkSkin, 1), frontLegX + 2, y - 12 + frontLegOffsetY, frontLegX + 2, y - 4 + frontLegOffsetY);
 
     // Främre fot med tår
       g.FillEllipse(new SolidBrush(fadedSaddleBrown), frontLegX - 2 + (direction * 2), y - 2, 10, 5);
    // Tår på främre fot
    for (int i = 0; i < 3; i++)
  {
  g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, 101, 67, 33)), frontLegX - 1 + (direction * 2) + (i * 2), y - 1, 2, 2);
            }
      
    // Rita främre arm med vapen (framför kroppen)
int armX = x + (IsPlayer ? 20 : 8);
    
     if (isAttacking)
    {
   // Attack animation
         
  // Främre axel
  g.FillEllipse(new SolidBrush(fadedLightSkin), armX - 2, y - 40, 7, 7);
     
      // Överarm med muskler (biceps)
 g.FillRectangle(new SolidBrush(fadedSkinColor), armX - (armSwingOffset / 2), y - 38 + (armSwingY / 2), 6, 10);
     g.DrawLine(new Pen(fadedLightSkin, 1), armX - (armSwingOffset / 2) + 4, y - 36 + (armSwingY / 2), armX - (armSwingOffset / 2) + 4, y - 30 + (armSwingY / 2));
 // Biceps bulge
     g.FillEllipse(new SolidBrush(fadedLightSkin), armX - (armSwingOffset / 2) + 1, y - 35 + (armSwingY / 2), 4, 5);
     
    // Armbåge
  g.FillEllipse(new SolidBrush(fadedDarkSkin), armX - (armSwingOffset / 2) + 1, y - 30 + (armSwingY / 2), 4, 4);
 
    // Underarm  
    g.FillRectangle(new SolidBrush(fadedSkinColor), armX - armSwingOffset, y - 30 + armSwingY, 6, 10);
    
    // Hand med fingrar
    g.FillEllipse(new SolidBrush(fadedSkinColor), armX - armSwingOffset - 2, y - 22 + armSwingY, 5, 5);
    // Fingrar som griper vapnet
    g.FillRectangle(new SolidBrush(fadedDarkSkin), armX - armSwingOffset, y - 21 + armSwingY, 2, 3);
   g.FillRectangle(new SolidBrush(fadedDarkSkin), armX - armSwingOffset, y - 19 + armSwingY, 2, 2);
  }
    else
    {
        // Walk animation för främre arm
             
  // Främre axel
   g.FillEllipse(new SolidBrush(fadedLightSkin), armX - 2, y - 40, 7, 7);
   
    // Överarm med muskler
    g.FillRectangle(new SolidBrush(fadedSkinColor), armX + (frontArmSwing / 2), y - 38 + (frontArmSwing / 3), 6, 10);
    g.DrawLine(new Pen(fadedLightSkin, 1), armX + (frontArmSwing / 2) + 4, y - 36 + (frontArmSwing / 3), armX + (frontArmSwing / 2) + 4, y - 30 + (frontArmSwing / 3));
      
   // Armbåge
     g.FillEllipse(new SolidBrush(fadedDarkSkin), armX + (frontArmSwing / 2) + 1, y - 30 + (frontArmSwing / 3), 4, 4);
      
 // Underarm
    g.FillRectangle(new SolidBrush(fadedSkinColor), armX + frontArmSwing, y - 30 + (frontArmSwing / 2), 6, 10);
        
     // Hand
  if (!hasClub)
  {
       g.FillEllipse(new SolidBrush(fadedSkinColor), armX + frontArmSwing - 2, y - 22 + (frontArmSwing / 2), 5, 5);
    // Fingrar
  g.FillRectangle(new SolidBrush(fadedDarkSkin), armX + frontArmSwing + 1, y - 21 + (frontArmSwing / 2), 2, 3);
     g.FillRectangle(new SolidBrush(fadedDarkSkin), armX + frontArmSwing + 1, y - 19 + (frontArmSwing / 2), 2, 2);
         }
     }
 }
    }
}
