using System.Drawing;

namespace Age_of_War
{
    // Närstridsenhet - Grottmänniska med klubba
    public class Soldier : Unit
    {
        public Soldier(bool isPlayer, float startX, int y)
            : base(isPlayer, startX, y)
        {
            MaxHP = HP = 100;
        Damage = 20;
       Speed = 2.0f;
Cost = 50;
            AttackCooldown = 1000; // ms
        }

        public override void Draw(Graphics g)
 {
            int x = (int)PositionX;
  int y = PositionY + (int)(DeathCollapseProgress * 40);  // Faller ihop nedåt
  int direction = IsPlayer ? 1 : -1;
    
            // Rita grottmänniska
            DrawCaveman(g, Color.FromArgb(210, 180, 140), true);
    
      // Rita klubba (i handen) - INTE när man dör
            if (!IsDying)
            {
                int armSwingOffset = 0;
    int armSwingY = 0;
  if (isAttacking)
  {
   if (attackAnimationTimer < 100)
        {
armSwingOffset = -8 * direction;
       armSwingY = -5;
      }
      else if (attackAnimationTimer < 200)
   {
  armSwingOffset = 12 * direction;
         armSwingY = 2;
        }
     else
{
 armSwingOffset = 4 * direction;
    armSwingY = 0;
     }
    }
    
    int armX = x + (IsPlayer ? 20 : 8);
    int clubX = armX - armSwingOffset + (direction * 3);
    int clubY = y - 38 + armSwingY;
    
    // Klubbskaft med alpha
  int alpha = (int)(DeathAlpha * 255);
    g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, Color.SaddleBrown)), clubX, clubY, 4, 18);
    // Klubbhuvud med alpha
    g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Gray)), clubX - 3, clubY - 6, 10, 10);
    g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.DarkGray)), clubX - 1, clubY - 4, 6, 6);
   }
    
    // HP-bar - INTE när man dör
if (!IsDying && MaxHP > 0)
{
  g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 10, Hitbox.Width, 5);
    g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 10, (int)(Hitbox.Width * ((float)HP / MaxHP)), 5);
}

        }
    }
}
