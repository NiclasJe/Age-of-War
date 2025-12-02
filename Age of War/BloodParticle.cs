using System;
using System.Drawing;

namespace Age_of_War
{
    // Blodpartikel som visas när en enhet tar skada
    public class BloodParticle
    {
  public float X, Y;
   public float VelocityX, VelocityY;
        public int Size;
        public int Lifetime;  // ms kvar
        public int MaxLifetime;
        public Color BloodColor;
        public bool IsActive = true;
        
        public BloodParticle(float startX, float startY)
   {
     X = startX;
            Y = startY;
            
Random rand = new Random(Guid.NewGuid().GetHashCode());
  
            // Slumpmässig hastighet i olika riktningar
            VelocityX = (float)(rand.NextDouble() * 4 - 2);  // -2 till 2
VelocityY = (float)(rand.NextDouble() * 4 - 2);  // -2 till 2
         
Size = 2 + rand.Next(4);  // 2-5 pixlar
            
    MaxLifetime = 300 + rand.Next(200);  // 300-500ms
  Lifetime = MaxLifetime;
      
 // Lite variation i blodets färg
          int redVariation = 200 + rand.Next(56);  // 200-255
         BloodColor = Color.FromArgb(redVariation, 0, 0);
        }
        
   public void Update(int deltaTime)
        {
            X += VelocityX;
        Y += VelocityY;
     
            // Gravitation
   VelocityY += 0.15f;
            
         // Luftmotstånd
 VelocityX *= 0.98f;
        
Lifetime -= deltaTime;
     if (Lifetime <= 0)
              IsActive = false;
        }
        
      public void Draw(Graphics g)
        {
   // Fade ut när lifetime går mot 0
         float alpha = (float)Lifetime / MaxLifetime;
    int alphaValue = (int)(255 * alpha);
     
  Color fadeColor = Color.FromArgb(alphaValue, BloodColor);
            
   using (var brush = new SolidBrush(fadeColor))
    {
  g.FillEllipse(brush, X - Size / 2, Y - Size / 2, Size, Size);
            }
   }
    }
}
