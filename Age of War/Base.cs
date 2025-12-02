using System.Drawing;

namespace Age_of_War
{
    // Bas för spelare/fiende - Grotta
    public class Base
    {
    public int HP;
        public int MaxHP;
      public Rectangle Hitbox;
        public bool IsPlayer;

        public Base(bool isPlayer, Rectangle hitbox)
        {
          IsPlayer = isPlayer;
            MaxHP = HP = 1000;
      Hitbox = hitbox;
  }

    public void Draw(Graphics g)
        {
     // Rita grotta
       Color rockColor = Color.FromArgb(100, 100, 100);
       Color darkRockColor = Color.FromArgb(60, 60, 60);
         Color mossColor = Color.FromArgb(80, 120, 40);
       
            // Grottväggar (steniga ytor)
   g.FillRectangle(new SolidBrush(rockColor), Hitbox);
    
        // Lager av sten för textur
        for (int i = 0; i < 5; i++)
        {
        int layerY = Hitbox.Y + (i * 30);
     g.FillRectangle(new SolidBrush(Color.FromArgb(80 + i * 5, 80 + i * 5, 80 + i * 5)), 
  Hitbox.X, layerY, Hitbox.Width, 3);
            }
      
     // Grottöppning (mörk oval)
int openingWidth = Hitbox.Width - 20;
     int openingHeight = Hitbox.Height - 40;
  int openingX = Hitbox.X + 10;
            int openingY = Hitbox.Y + 20;
            
        // Mörk bakgrund i grottöppningen
            g.FillEllipse(new SolidBrush(Color.Black), 
          openingX, openingY, openingWidth, openingHeight);
    
            // Skuggning runt grottöppningen
            using (var pen = new Pen(darkRockColor, 8))
        {
         g.DrawEllipse(pen, openingX, openingY, openingWidth, openingHeight);
          }
      
 // Mossa runt grottöppningen
   for (int i = 0; i < 8; i++)
            {
      int mossX = openingX + (i * 12);
     int mossY = openingY + openingHeight - 5;
    g.FillEllipse(new SolidBrush(mossColor), mossX, mossY, 10, 8);
            }
   
            // Stalaktiter/stalagmiter vid öppningen
            Point[] stalactite1 = new Point[]
            {
 new Point(openingX + 15, openingY + 5),
            new Point(openingX + 20, openingY + 25),
           new Point(openingX + 10, openingY + 5)
      };
            g.FillPolygon(new SolidBrush(darkRockColor), stalactite1);
   
            Point[] stalactite2 = new Point[]
         {
        new Point(openingX + openingWidth - 15, openingY + 5),
       new Point(openingX + openingWidth - 20, openingY + 25),
        new Point(openingX + openingWidth - 10, openingY + 5)
   };
            g.FillPolygon(new SolidBrush(darkRockColor), stalactite2);
     
     // Sprickor i berget
       using (var crackPen = new Pen(Color.FromArgb(40, 40, 40), 2))
{
         g.DrawLine(crackPen, Hitbox.X + 5, Hitbox.Y + 30, Hitbox.X + 25, Hitbox.Y + 60);
         g.DrawLine(crackPen, Hitbox.Right - 25, Hitbox.Y + 40, Hitbox.Right - 5, Hitbox.Y + 80);
    }
       
  // HP-bar (stor och tydlig över grottan)
            int hpBarWidth = Hitbox.Width + 40;
        int hpBarX = Hitbox.X - 20;
   int hpBarY = Hitbox.Y - 25;
            
          // Bakgrund
        g.FillRectangle(Brushes.DarkRed, hpBarX, hpBarY, hpBarWidth, 15);
      // HP
        float hpPercent = (float)HP / MaxHP;
            Color hpColor = hpPercent > 0.6f ? Color.Green : hpPercent > 0.3f ? Color.Orange : Color.Red;
            g.FillRectangle(new SolidBrush(hpColor), hpBarX, hpBarY, (int)(hpBarWidth * hpPercent), 15);
            // Kant
     g.DrawRectangle(Pens.Black, hpBarX, hpBarY, hpBarWidth, 15);
       
 // HP-text
       string hpText = $"{HP} / {MaxHP}";
    g.DrawString(hpText, new Font("Arial", 10, FontStyle.Bold), Brushes.White, 
   hpBarX + (hpBarWidth / 2) - 30, hpBarY);
        }
    }
}
