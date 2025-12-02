using System.Drawing;

namespace Age_of_War
{
    // Ridande soldat - Grottmänniska på ödla
    public class CavalryUnit : Unit
    {
        public CavalryUnit(bool isPlayer, float startX, int y)
    : base(isPlayer, startX, y)
        {
       MaxHP = HP = 180;
            Damage = 35;
       Speed = 2.0f;
    Cost = 200;  // Ändrat från 120 till 200
  AttackCooldown = 900;
        }

  public override void Draw(Graphics g)
        {
   int x = (int)PositionX;
  int y = PositionY;
  int direction = IsPlayer ? 1 : -1;
    
    // Rita ödla i profil först (under ryttaren)
   Color lizardColor = Color.FromArgb(100, 150, 50);
    
    // Ödlekropp (oval)
   g.FillEllipse(new SolidBrush(lizardColor), x, y - 22, 45, 20);
    
    // Ödlehuvud (i profil)
  int headX = x + (IsPlayer ? 43 : -8);
    g.FillEllipse(new SolidBrush(lizardColor), headX, y - 24, 14, 12);
    
    // Öga
    int eyeX = headX + (IsPlayer ? 10 : 3);
    g.FillEllipse(Brushes.Yellow, eyeX, y - 21, 4, 4);
  g.FillEllipse(Brushes.Black, eyeX + 1, y - 20, 2, 2);
    
    // Mun
  g.DrawLine(Pens.Black, headX + (IsPlayer ? 12 : 2), y - 16, headX + (IsPlayer ? 14 : 0), y - 15);
    
    // Ödlesvans
    int tailX = x + (IsPlayer ? -8 : 45);
    for (int i = 0; i < 4; i++)
    {
        int tailSegmentX = tailX + (direction * -i * 5);
int tailWidth = 10 - i * 2;
        g.FillEllipse(new SolidBrush(lizardColor), tailSegmentX, y - 20 + i, tailWidth, 8);
    }
    
    // Ödleben (walk animation)
    int legOffset1 = 0, legOffset2 = 0;
    if (!IsBlocked)
    {
        if (animationFrame % 4 == 1)
     {
legOffset1 = 4;
        legOffset2 = -2;
        }
 else if (animationFrame % 4 == 3)
        {
            legOffset1 = -2;
    legOffset2 = 4;
      }
    }
    
    // Främre ben
    g.FillRectangle(new SolidBrush(lizardColor), x + 32, y - 8 + legOffset1, 5, 10);
    // Bakre ben
    g.FillRectangle(new SolidBrush(lizardColor), x + 10, y - 8 + legOffset2, 5, 10);
    
 // Rita ryttare (grottmänniska) ovanpå ödlan i profil
    int riderX = x + 15;
  int riderY = y - 22;
    Color skinColor = Color.FromArgb(210, 180, 140);
    
    // Ryttarens ben (på ödlans sidor - bakre först)
    g.FillRectangle(new SolidBrush(skinColor), riderX - 3, riderY - 5, 5, 12);
    
    // Kropp
    g.FillEllipse(new SolidBrush(skinColor), riderX + 3, riderY - 35, 16, 22);
 
    // Huvud
    g.FillEllipse(new SolidBrush(skinColor), riderX + 5, riderY - 48, 14, 14);
    
    // Öga
    int riderEyeX = riderX + (IsPlayer ? 15 : 8);
    g.FillEllipse(Brushes.White, riderEyeX, riderY - 43, 4, 4);
    g.FillEllipse(Brushes.Black, riderEyeX + 1, riderY - 42, 2, 2);
    
    // Hår
    g.FillEllipse(Brushes.Brown, riderX + 5, riderY - 50, 5, 5);
    g.FillEllipse(Brushes.Brown, riderX + 9, riderY - 52, 5, 5);
    g.FillEllipse(Brushes.Brown, riderX + 13, riderY - 50, 5, 5);
    
    // Ryttarens främre ben
    g.FillRectangle(new SolidBrush(skinColor), riderX + 20, riderY - 5, 5, 12);
    
    // Arm och stor klubba
    int armSwingOffset = 0;
    int armSwingY = 0;
    if (isAttacking)
    {
        if (attackAnimationTimer < 100)
        {
         armSwingOffset = -10 * direction;
       armSwingY = -8;
     }
        else if (attackAnimationTimer < 200)
    {
        armSwingOffset = 15 * direction;
       armSwingY = 3;
        }
        else
        {
   armSwingOffset = 5 * direction;
       armSwingY = 0;
        }
    }
    
    int armX = riderX + (IsPlayer ? 18 : 5);
    // Överarm
    g.FillRectangle(new SolidBrush(skinColor), armX - (armSwingOffset / 2), riderY - 32 + (armSwingY / 2), 5, 10);
 // Underarm
    g.FillRectangle(new SolidBrush(skinColor), armX - armSwingOffset, riderY - 24 + armSwingY, 5, 10);
    
    // Stor klubba
    int clubX = armX - armSwingOffset + (direction * 2);
    int clubY = riderY - 16 + armSwingY;
    
    // Klubbskaft
    g.FillRectangle(Brushes.SaddleBrown, clubX, clubY, 5, 22);
    // Stor klubbhuvud
    g.FillEllipse(Brushes.DarkGray, clubX - 4, clubY - 9, 13, 13);
  g.FillEllipse(Brushes.Gray, clubX - 2, clubY - 7, 9, 9);
    
    // HP-bar
    if (MaxHP > 0)  // Rita bara HP-bar om MaxHP är satt
{
 g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 10, Hitbox.Width, 5);
    g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 10, (int)(Hitbox.Width * ((float)HP / MaxHP)), 5);
}
 }
    }
}
