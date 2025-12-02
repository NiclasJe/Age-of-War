using System;
using System.Drawing;

namespace Age_of_War
{
 // Popup som visar hur mycket guld man fick när man dödar en enhet
    public class GoldPopup
    {
        public float X, Y;
    public int Amount;
        public int Lifetime;  // ms kvar
        public const int MAX_LIFETIME = 1500;  // 1.5 sekunder
        public bool IsActive = true;
        public float VelocityY = -1.5f;  // Rör sig uppåt
        
 public GoldPopup(float x, float y, int amount)
   {
  X = x;
   Y = y;
     Amount = amount;
      Lifetime = MAX_LIFETIME;
        }
        
  public void Update(int deltaTime)
        {
            Lifetime -= deltaTime;
    if (Lifetime <= 0)
        {
          IsActive = false;
     return;
        }
            
        // Rör sig uppåt
     Y += VelocityY;
  }
        
        public void Draw(Graphics g)
        {
if (!IsActive) return;
    
       // Beräkna alpha baserat på lifetime (fade out)
   float alpha = (float)Lifetime / MAX_LIFETIME;
            int alphaValue = (int)(alpha * 255);
            
        // Guld-ikon
  using (var goldBrush = new SolidBrush(Color.FromArgb(alphaValue, Color.Gold)))
     {
       g.FillEllipse(goldBrush, X - 8, Y - 8, 16, 16);
       }
            
            using (var outlinePen = new Pen(Color.FromArgb(alphaValue, Color.DarkGoldenrod), 2))
  {
      g.DrawEllipse(outlinePen, X - 8, Y - 8, 16, 16);
  }
            
       // Text med guldmängd
            string text = $"+{Amount}";
   using (var font = new Font("Arial", 14, FontStyle.Bold))
    using (var textBrush = new SolidBrush(Color.FromArgb(alphaValue, Color.Gold)))
      using (var shadowBrush = new SolidBrush(Color.FromArgb(alphaValue / 2, Color.Black)))
          {
    var textSize = g.MeasureString(text, font);
        
                // Skugga
      g.DrawString(text, font, shadowBrush, X + 12, Y - 6);
    
      // Text
      g.DrawString(text, font, textBrush, X + 11, Y - 7);
          }
        }
    }
}
