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
     // Rita grotta med mycket mer detaljer
       Color rockColor = Color.FromArgb(100, 100, 100);
       Color darkRockColor = Color.FromArgb(60, 60, 60);
         Color mossColor = Color.FromArgb(80, 120, 40);
         Color lightRockColor = Color.FromArgb(130, 130, 130);
   
       // Grottväggar (steniga ytor) med gradient
   using (var rockBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
    Hitbox,
     rockColor,
  darkRockColor,
     System.Drawing.Drawing2D.LinearGradientMode.Vertical))
      {
    g.FillRectangle(rockBrush, Hitbox);
            }
    
        // Lager av sten för textur - fler och mer varierade
        for (int i = 0; i < 12; i++)
        {
   int layerY = Hitbox.Y + (i * 15);
     int shade = 70 + (i % 4) * 10;
     g.FillRectangle(new SolidBrush(Color.FromArgb(shade, shade, shade)), 
  Hitbox.X, layerY, Hitbox.Width, 2);
  }
          
          // Sten-blocks (individual rocks in the wall)
         for (int i = 0; i < 8; i++)
            {
         int rockX = Hitbox.X + 5 + (i % 3) * 30;
                int rockY = Hitbox.Y + 10 + (i / 3) * 35;
              g.DrawRectangle(new Pen(Color.FromArgb(50, 50, 50), 1), rockX, rockY, 25, 20);
     }
      
  // Grottöppning (mörk oval)
int openingWidth = Hitbox.Width - 20;
     int openingHeight = Hitbox.Height - 40;
  int openingX = Hitbox.X + 10;
int openingY = Hitbox.Y + 20;
 
        // Mörk bakgrund i grottöppningen med gradient
          using (var caveBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
          new Rectangle(openingX, openingY, openingWidth, openingHeight),
     Color.FromArgb(20, 20, 20),
     Color.Black,
        System.Drawing.Drawing2D.LinearGradientMode.Vertical))
  {
      g.FillEllipse(caveBrush, openingX, openingY, openingWidth, openingHeight);
   }
    
  // Skuggning runt grottöppningen - tjockare
          using (var pen = new Pen(darkRockColor, 12))
        {
       g.DrawEllipse(pen, openingX, openingY, openingWidth, openingHeight);
     }
      
        // Ljusare kant för 3D-effekt
     using (var lightPen = new Pen(lightRockColor, 3))
  {
   g.DrawArc(lightPen, openingX - 2, openingY - 2, openingWidth + 4, openingHeight + 4, 180, 180);
      }
      
 // Mycket mer mossa runt grottöppningen
   for (int i = 0; i < 15; i++)
            {
      int mossX = openingX + ((i * openingWidth) / 15);
     int mossY = openingY + openingHeight - (i % 2 == 0 ? 8 : 5);
     int mossSize = 8 + (i % 3) * 2;
    g.FillEllipse(new SolidBrush(mossColor), mossX, mossY, mossSize, mossSize - 2);
    }
          
// Extra mossa på sidorna
            for (int i = 0; i < 5; i++)
     {
     g.FillEllipse(new SolidBrush(Color.FromArgb(70, 110, 35)), 
  openingX - 5, openingY + 20 + (i * 20), 12, 15);
                g.FillEllipse(new SolidBrush(Color.FromArgb(70, 110, 35)), 
    openingX + openingWidth - 7, openingY + 25 + (i * 18), 12, 15);
  }
   
     // Fler stalaktiter/stalagmiter vid öppningen
            Point[] stalactite1 = new Point[]
      {
 new Point(openingX + 15, openingY + 5),
        new Point(openingX + 20, openingY + 30),
       new Point(openingX + 10, openingY + 5)
 };
      g.FillPolygon(new SolidBrush(darkRockColor), stalactite1);
      g.FillPolygon(new SolidBrush(Color.FromArgb(80, 80, 80)), 
     new Point[] { 
      new Point(openingX + 16, openingY + 5), 
        new Point(openingX + 18, openingY + 25), 
        new Point(openingX + 14, openingY + 5) 
      });
   
  Point[] stalactite2 = new Point[]
         {
        new Point(openingX + openingWidth - 15, openingY + 5),
       new Point(openingX + openingWidth - 20, openingY + 30),
        new Point(openingX + openingWidth - 10, openingY + 5)
   };
        g.FillPolygon(new SolidBrush(darkRockColor), stalactite2);
    
       // Mellanstor stalaktit
     Point[] stalactite3 = new Point[]
            {
          new Point(openingX + openingWidth / 2, openingY + 10),
          new Point(openingX + openingWidth / 2 + 5, openingY + 25),
     new Point(openingX + openingWidth / 2 - 5, openingY + 10)
            };
            g.FillPolygon(new SolidBrush(Color.FromArgb(70, 70, 70)), stalactite3);
          
       // Stalagmiter från botten
      Point[] stalagmite1 = new Point[]
            {
    new Point(openingX + 30, openingY + openingHeight),
    new Point(openingX + 35, openingY + openingHeight - 20),
            new Point(openingX + 40, openingY + openingHeight)
        };
      g.FillPolygon(new SolidBrush(darkRockColor), stalagmite1);
     
     // Många fler sprickor i berget för mer textur
       using (var crackPen = new Pen(Color.FromArgb(40, 40, 40), 2))
{
         g.DrawLine(crackPen, Hitbox.X + 5, Hitbox.Y + 30, Hitbox.X + 25, Hitbox.Y + 60);
       g.DrawLine(crackPen, Hitbox.Right - 25, Hitbox.Y + 40, Hitbox.Right - 5, Hitbox.Y + 80);
         g.DrawLine(crackPen, Hitbox.X + 15, Hitbox.Y + 50, Hitbox.X + 35, Hitbox.Y + 90);
         g.DrawLine(crackPen, Hitbox.X + 50, Hitbox.Y + 20, Hitbox.X + 70, Hitbox.Y + 70);
 g.DrawLine(crackPen, Hitbox.Right - 45, Hitbox.Y + 60, Hitbox.Right - 30, Hitbox.Y + 100);
    }
    
    // Småstenar vid basen av grottan
            for (int i = 0; i < 6; i++)
            {
                int stoneX = Hitbox.X + 10 + (i * 15);
      int stoneY = Hitbox.Bottom - 15 + (i % 2) * 5;
             Point[] stone = new Point[]
         {
      new Point(stoneX, stoneY),
         new Point(stoneX + 8, stoneY - 3),
       new Point(stoneX + 12, stoneY + 5),
        new Point(stoneX + 4, stoneY + 8)
          };
        g.FillPolygon(new SolidBrush(Color.FromArgb(90, 90, 90)), stone);
}
            
    // Fackla på sidan av grottan (om det är spelarens bas)
     if (IsPlayer)
    {
       int torchX = Hitbox.Right + 10;
            int torchY = Hitbox.Y + 40;
                
     // Fackelhållare (metall)
        g.FillRectangle(new SolidBrush(Color.FromArgb(80, 80, 80)), torchX, torchY, 5, 30);
    g.FillEllipse(new SolidBrush(Color.FromArgb(60, 60, 60)), torchX - 3, torchY, 11, 8);
      
     // Fackelpinne
     g.FillRectangle(Brushes.SaddleBrown, torchX + 8, torchY - 25, 4, 30);
     
   // Eld (animated effect could be added)
             int flameFrame = (Environment.TickCount / 100) % 3;
    Color flameColor = flameFrame == 0 ? Color.Orange : flameFrame == 1 ? Color.OrangeRed : Color.Yellow;
      Point[] flame = new Point[]
        {
    new Point(torchX + 10, torchY - 25),
  new Point(torchX + 6, torchY - 35),
         new Point(torchX + 10, torchY - 45),
            new Point(torchX + 14, torchY - 35)
  };
            g.FillPolygon(new SolidBrush(flameColor), flame);
       g.FillEllipse(new SolidBrush(Color.FromArgb(150, 255, 200, 0)), torchX + 8, torchY - 28, 4, 6);
            }
   
   // Växter/buskar vid grottan
     for (int i = 0; i < 4; i++)
       {
                int plantX = Hitbox.X - 10 + (i * 30);
  int plantY = Hitbox.Bottom - 20;
            
      // Stjälk
   g.DrawLine(new Pen(Color.FromArgb(60, 100, 40), 2), plantX, plantY, plantX, plantY - 15);
          
    // Blad
          g.FillEllipse(new SolidBrush(Color.FromArgb(80, 140, 60)), plantX - 5, plantY - 18, 10, 8);
           g.FillEllipse(new SolidBrush(Color.FromArgb(70, 130, 50)), plantX - 4, plantY - 12, 8, 6);
            }
            
        // Primitiva symboler på väggen (höhlmålningar)
            using (var symbolPen = new Pen(Color.FromArgb(139, 69, 19), 2))
       {
     // Enkel symbol - stick figure
             int symX = Hitbox.X + 15;
       int symY = Hitbox.Y + 40;
     g.DrawEllipse(symbolPen, symX, symY, 5, 5);
                g.DrawLine(symbolPen, symX + 2, symY + 5, symX + 2, symY + 15);
      g.DrawLine(symbolPen, symX + 2, symY + 8, symX - 3, symY + 12);
       g.DrawLine(symbolPen, symX + 2, symY + 8, symX + 7, symY + 12);
                
          // Jakt-symbol
     int symX2 = Hitbox.Right - 25;
     g.DrawLine(symbolPen, symX2, Hitbox.Y + 50, symX2 + 8, Hitbox.Y + 50);
  g.DrawLine(symbolPen, symX2 + 8, Hitbox.Y + 50, symX2 + 6, Hitbox.Y + 47);
      g.DrawLine(symbolPen, symX2 + 8, Hitbox.Y + 50, symX2 + 6, Hitbox.Y + 53);
 }
       
  // HP-bar ritas nu separat i Form1.cs för att alltid ligga högst i z-level
 }
 }
}
