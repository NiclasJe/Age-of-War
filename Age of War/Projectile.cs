using System.Drawing;

namespace Age_of_War
{
    // Projektil för ranged-enheter och turrets
    public class Projectile
    {
        public float X, Y;
        public float VelocityX, VelocityY;
 public bool IsPlayer;
 public int Damage;
    public bool IsActive = true;
    public bool IsTurretShot;  // Om projektilen kommer från en turret
 
     public Projectile(float startX, float startY, float targetX, float targetY, bool isPlayer, int damage, bool isTurretShot = false)
{
     X = startX;
  Y = startY;
   IsPlayer = isPlayer;
   Damage = damage;
      IsTurretShot = isTurretShot;
   
  // Beräkna hastighet mot målet
      float dx = targetX - startX;
  float dy = targetY - startY;
 float distance = (float)System.Math.Sqrt(dx * dx + dy * dy);
            
   if (distance > 0)
     {
   // Turretskott är fyra gånger så snabba som vanliga skott
    float speed = isTurretShot ? 20 : 5;  // 20 pixlar per frame för turrets, 5 för enheter
            VelocityX = (dx / distance) * speed;
      VelocityY = (dy / distance) * speed;
  }
   }
   
      public void Update()
        {
   X += VelocityX;
  Y += VelocityY;
        }
      
  public void Draw(Graphics g)
        {
   if (IsTurretShot)
        {
       // Rita solliknande projektil (gul/orange)
         // Yttre glöd
         using (var glowBrush = new SolidBrush(Color.FromArgb(100, 255, 200, 0)))
            {
         g.FillEllipse(glowBrush, X - 6, Y - 6, 12, 12);
 }
   
  // Huvudkropp (bright yellow/orange)
            using (var coreBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
          new PointF(X - 4, Y - 4),
    new PointF(X + 4, Y + 4),
     Color.Yellow,
        Color.Orange))
            {
    g.FillEllipse(coreBrush, X - 4, Y - 4, 8, 8);
  }
            
            // Ljus center
      g.FillEllipse(Brushes.White, X - 2, Y - 2, 4, 4);

            // Rita små "solar rays" runt projektilen
        using (var rayPen = new Pen(Color.FromArgb(150, 255, 255, 0), 2))
     {
    for (int i = 0; i < 8; i++)
         {
   double angle = (i * System.Math.PI * 2) / 8;
    float rayX = (float)(X + System.Math.Cos(angle) * 5);
       float rayY = (float)(Y + System.Math.Sin(angle) * 5);
           g.DrawLine(rayPen, X, Y, rayX, rayY);
     }
       }
        }
    else
        {
   // Rita normal sten (för slangbella)
            g.FillEllipse(Brushes.DarkGray, X - 3, Y - 3, 6, 6);
            g.FillEllipse(Brushes.Gray, X - 2, Y - 2, 4, 4);
   }
  }
}
}
