using System.Drawing;

namespace Age_of_War
{
    // Ranged-enhet - Grottmänniska med slangbella
    public class RangedUnit : Unit
    {
        public RangedUnit(bool isPlayer, float startX, int y)
        : base(isPlayer, startX, y)
        {
            MaxHP = HP = 70;
          Damage = 15;
     Speed = 2.0f; // Dubbel hastighet
        Cost = 80;
   AttackCooldown = 1200;
        }

        public override void Draw(Graphics g)
        {
            int x = (int)PositionX;
            int y = PositionY;
            int direction = IsPlayer ? 1 : -1;
            
            // Rita grottmänniska
            DrawCaveman(g, Color.FromArgb(200, 170, 130), false);
    
            // Rita slangbella
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
    int slingX = armX - armSwingOffset;
    int slingY = y - 22 + armSwingY;
    
    if (isAttacking && attackAnimationTimer >= 100 && attackAnimationTimer < 200)
    {
        // Rita spänd slangbella
        g.DrawLine(new Pen(Brushes.SaddleBrown, 2), slingX - 5, slingY - 5, slingX + 5, slingY + 5);
        g.DrawLine(new Pen(Brushes.SaddleBrown, 2), slingX - 5, slingY + 5, slingX + 5, slingY - 5);
        // Sten i slangbellan
        g.FillEllipse(Brushes.Gray, slingX + (direction * 8), slingY - 2, 5, 5);
    }
    else
    {
        // Rita slapp slangbella
        g.DrawArc(new Pen(Brushes.SaddleBrown, 2), slingX - 6, slingY - 4, 12, 10, 0, 180);
    }
    
    // HP-bar
    if (MaxHP > 0)  // Rita bara HP-bar om MaxHP är satt
{
  g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 10, Hitbox.Width, 5);
    g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 10, (int)(Hitbox.Width * ((float)HP / MaxHP)), 5);
}
}
    }
}
