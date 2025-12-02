using System.Drawing;

namespace Age_of_War
{
    // Tung enhet - Stor grottmänniska med megaklubba
    public class TankUnit : Unit
    {
public TankUnit(bool isPlayer, float startX, int y)
 : base(isPlayer, startX, y)
   {
 MaxHP = HP = 250;
    Damage = 40;
       Speed = 2.0f;
   Cost = 200;
  AttackCooldown = 1800;
   }

    public override void Draw(Graphics g)
    {
 int x = (int)PositionX;
    int y = PositionY;
    int direction = IsPlayer ? 1 : -1;
    
    // Rita stor grottmänniska i profil (större än vanlig)
    Color skinColor = Color.FromArgb(200, 160, 120);
    
    // Beräkna walk animation offset för ben med X och Y komponenter
    int frontLegOffsetX = 0;
    int frontLegOffsetY = 0;
    int backLegOffsetX = 0;
    int backLegOffsetY = 0;
    
    if (!IsBlocked && !isAttacking)
    {
     if (animationFrame % 4 == 0)
 {
            frontLegOffsetX = 0;
            frontLegOffsetY = 0;
    backLegOffsetX = 0;
 backLegOffsetY = 0;
        }
    else if (animationFrame % 4 == 1)
     {
   // Främre ben går framåt (upp och fram)
 frontLegOffsetX = 10 * direction;
          frontLegOffsetY = -8;  // Lyfter upp lite
            // Bakre ben går bakåt (ner och bak)
    backLegOffsetX = -5 * direction;
            backLegOffsetY = 3;
    }
        else if (animationFrame % 4 == 2)
        {
        frontLegOffsetX = 0;
            frontLegOffsetY = 0;
          backLegOffsetX = 0;
            backLegOffsetY = 0;
        }
        else // animationFrame % 4 == 3
        {
     // Bakre ben går framåt (upp och fram)
            backLegOffsetX = 10 * direction;
    backLegOffsetY = -8;// Lyfter upp lite
         // Främre ben går bakåt (ner och bak)
 frontLegOffsetX = -5 * direction;
 frontLegOffsetY = 3;
        }
    }
    
    // Attack animation
    int armSwingOffset = 0;
    int armSwingY = 0;
    if (isAttacking)
    {
  if (attackAnimationTimer < 100)
        {
    armSwingOffset = -12 * direction;
       armSwingY = -8;
        }
        else if (attackAnimationTimer < 200)
        {
            armSwingOffset = 18 * direction;
  armSwingY = 4;
      }
        else
        {
    armSwingOffset = 6 * direction;
      armSwingY = 0;
  }
    }
    
    // Bakre ben
    int backLegX = x + 14 + backLegOffsetX;
    g.FillRectangle(new SolidBrush(skinColor), backLegX, y - 32 + backLegOffsetY, 8, 16);
    g.FillRectangle(new SolidBrush(skinColor), backLegX, y - 16 + backLegOffsetY, 8, 16);
    g.FillEllipse(new SolidBrush(Color.SaddleBrown), backLegX - 2 + (direction * 2), y - 2, 12, 6);
    
    // Bakre arm
int backArmX = x + 14;
    g.FillRectangle(new SolidBrush(skinColor), backArmX, y - 48, 7, 12);
    g.FillRectangle(new SolidBrush(skinColor), backArmX, y - 38, 7, 12);

    // Kropp (större oval)
    g.FillEllipse(new SolidBrush(skinColor), x + 6, y - 52, 24, 34);
    
    // Huvud (större)
    g.FillEllipse(new SolidBrush(skinColor), x + 9, y - 68, 22, 22);
    
    // Öga
    int eyeX = x + (IsPlayer ? 24 : 14);
    g.FillEllipse(Brushes.White, eyeX, y - 62, 6, 6);
    g.FillEllipse(Brushes.Black, eyeX + 2, y - 61, 3, 3);
    
    // Näsa
    Point[] nose = new Point[]
    {
        new Point(x + (IsPlayer ? 30 : 10), y - 58),
        new Point(x + (IsPlayer ? 33 : 7), y - 55),
        new Point(x + (IsPlayer ? 30 : 10), y - 52)
  };
    g.FillPolygon(new SolidBrush(Color.FromArgb(skinColor.R - 20, skinColor.G - 20, skinColor.B - 20)), nose);
    
    // Hår (rufsigt)
    g.FillEllipse(Brushes.Brown, x + 9, y - 71, 8, 8);
    g.FillEllipse(Brushes.Brown, x + 15, y - 73, 8, 8);
    g.FillEllipse(Brushes.Brown, x + 21, y - 71, 8, 8);
    
    // Främre ben
    int frontLegX = x + 14 + frontLegOffsetX;
  g.FillRectangle(new SolidBrush(skinColor), frontLegX, y - 32 + frontLegOffsetY, 8, 16);
    g.FillRectangle(new SolidBrush(skinColor), frontLegX, y - 16 + frontLegOffsetY, 8, 16);
    g.FillEllipse(new SolidBrush(Color.SaddleBrown), frontLegX - 2 + (direction * 2), y - 2, 12, 6);
    
    // Främre arm med megaklubba
    int armX = x + (IsPlayer ? 24 : 10);
    // Överarm
    g.FillRectangle(new SolidBrush(skinColor), armX - (armSwingOffset / 2), y - 48 + (armSwingY / 2), 7, 12);
 // Underarm
    g.FillRectangle(new SolidBrush(skinColor), armX - armSwingOffset, y - 38 + armSwingY, 7, 12);
    
    // Megaklubba
    int clubX = armX - armSwingOffset + (direction * 3);
    int clubY = y - 28 + armSwingY;
    
    // Stort klubbskaft
    g.FillRectangle(Brushes.SaddleBrown, clubX, clubY, 6, 28);
  // Stor klubbhuvud
 g.FillEllipse(Brushes.DarkGray, clubX - 6, clubY - 12, 18, 18);
    g.FillEllipse(Brushes.Gray, clubX - 4, clubY - 10, 14, 14);
    
    // HP-bar
    if (MaxHP > 0)  // Rita bara HP-bar om MaxHP är satt
{
    g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 10, Hitbox.Width, 5);
    g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 10, (int)(Hitbox.Width * ((float)HP / MaxHP)), 5);
}
}
    }
}
