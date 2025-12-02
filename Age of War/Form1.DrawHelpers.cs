using System;
using System.Drawing;
using System.Windows.Forms;

namespace Age_of_War
{
    // Partial class för att hålla alla ritningshjälpmetoder
    public partial class Form1
    {
 // Hjälpmetoder för att rita miljö
        private void DrawHill(Graphics g, int x, int y, int width, int height, Color color)
   {
            using (var brush = new SolidBrush(color))
 {
            g.FillEllipse(brush, x, y, width, height * 2);
            }
        }

        private void DrawForest(Graphics g, int x, int y, int treeCount)
        {
      for (int i = 0; i < treeCount; i++)
        {
                DrawTree(g, x + (i * 40), y + (i % 2 * 10));
    }
        }

    private void DrawTree(Graphics g, int x, int y)
        {
            g.FillRectangle(Brushes.SaddleBrown, x, y, 15, 60);
            
  Color leafColor = Color.FromArgb(34, 139, 34);
 g.FillEllipse(new SolidBrush(leafColor), x - 20, y - 40, 55, 50);
            g.FillEllipse(new SolidBrush(Color.FromArgb(40, 160, 40)), x - 15, y - 55, 45, 45);
       g.FillEllipse(new SolidBrush(Color.FromArgb(30, 120, 30)), x - 10, y - 65, 35, 40);
        }

        private void DrawClouds(Graphics g)
        {
        Color cloudColor = Color.FromArgb(220, 255, 255, 255);
          Color darkCloudColor = Color.FromArgb(200, 230, 230, 230);

            DrawCloud(g, 80, 40, cloudColor, 1.2f);
      DrawCloud(g, 250, 70, darkCloudColor, 0.8f);
     DrawCloud(g, 400, 50, cloudColor, 1.0f);
     DrawCloud(g, 600, 85, darkCloudColor, 1.1f);
            DrawCloud(g, 800, 35, cloudColor, 0.9f);
            DrawCloud(g, 1000, 60, darkCloudColor, 1.3f);
  DrawCloud(g, this.ClientSize.Width - 200, 70, cloudColor, 1.0f);
        }

        private void DrawCloud(Graphics g, int x, int y, Color color, float scale)
  {
    using (var brush = new SolidBrush(color))
      {
       g.FillEllipse(brush, x, y, (int)(60 * scale), (int)(30 * scale));
           g.FillEllipse(brush, x + (int)(20 * scale), y - (int)(10 * scale), (int)(50 * scale), (int)(35 * scale));
      g.FillEllipse(brush, x + (int)(40 * scale), y, (int)(55 * scale), (int)(30 * scale));
       g.FillEllipse(brush, x + (int)(10 * scale), y + (int)(10 * scale), (int)(40 * scale), (int)(20 * scale));
            }
        }

    private void DrawBirds(Graphics g)
 {
int frame = (Environment.TickCount / 500) % 2;
    
            DrawBird(g, 200, 100, frame);
            DrawBird(g, 350, 80, (frame + 1) % 2);
        DrawBird(g, 700, 120, frame);
   DrawBird(g, this.ClientSize.Width - 300, 90, (frame + 1) % 2);
 }

        private void DrawBird(Graphics g, int x, int y, int frame)
    {
  using (var birdPen = new Pen(Color.FromArgb(60, 60, 60), 2))
      {
        if (frame == 0)
    {
    g.DrawArc(birdPen, x - 8, y - 5, 8, 8, 180, 180);
  g.DrawArc(birdPen, x + 8, y - 5, 8, 8, 0, 180);
        }
           else
          {
    g.DrawArc(birdPen, x - 8, y, 8, 8, 0, 180);
       g.DrawArc(birdPen, x + 8, y, 8, 8, 180, 180);
      }
              g.DrawLine(birdPen, x - 3, y, x + 3, y);
            }
    }

        private void DrawFlowers(Graphics g)
        {
            for (int i = 0; i < 15; i++)
  {
             int flowerX = 50 + (i * this.ClientSize.Width / 15) + (i % 3) * 10;
      int flowerY = game.BattlefieldY + 12;
     
             Color[] flowerColors = new Color[] 
         { 
        Color.Red, 
       Color.Yellow, 
Color.Pink, 
          Color.Purple,
               Color.Orange 
           };
        Color flowerColor = flowerColors[i % flowerColors.Length];
   
         g.DrawLine(new Pen(Color.FromArgb(50, 120, 50), 2), flowerX, flowerY, flowerX, flowerY - 12);
              
        for (int j = 0; j < 5; j++)
          {
   double angle = (j * 72) * Math.PI / 180;
      int petalX = flowerX + (int)(4 * Math.Cos(angle));
         int petalY = flowerY - 12 + (int)(4 * Math.Sin(angle));
           g.FillEllipse(new SolidBrush(flowerColor), petalX - 2, petalY - 2, 4, 4);
           }
                
   g.FillEllipse(new SolidBrush(Color.FromArgb(255, 200, 0)), flowerX - 2, flowerY - 14, 4, 4);
}
        }

        private void DrawBushes(Graphics g)
        {
      Color bushColor = Color.FromArgb(60, 130, 60);
            Color darkBushColor = Color.FromArgb(50, 110, 50);
       
       DrawBush(g, 100, game.BattlefieldY + 5, bushColor, darkBushColor);
            DrawBush(g, 300, game.BattlefieldY + 8, bushColor, darkBushColor);
   DrawBush(g, 550, game.BattlefieldY + 6, bushColor, darkBushColor);
            DrawBush(g, 900, game.BattlefieldY + 7, bushColor, darkBushColor);
            DrawBush(g, this.ClientSize.Width - 250, game.BattlefieldY + 5, bushColor, darkBushColor);
  }

  private void DrawBush(Graphics g, int x, int y, Color bushColor, Color darkBushColor)
        {
            g.FillEllipse(new SolidBrush(darkBushColor), x - 15, y, 20, 18);
            g.FillEllipse(new SolidBrush(bushColor), x - 5, y - 5, 22, 20);
            g.FillEllipse(new SolidBrush(bushColor), x + 5, y, 18, 16);
      g.FillEllipse(new SolidBrush(Color.FromArgb(70, 140, 70)), x - 2, y - 3, 15, 15);
        }

        private void DrawButterflies(Graphics g)
        {
            int time = Environment.TickCount / 200;
            
            for (int i = 0; i < 3; i++)
  {
 int butterflyX = 150 + (i * 400) + (int)(Math.Sin((time + i * 100) * 0.05) * 30);
       int butterflyY = game.BattlefieldY - 50 + (int)(Math.Cos((time + i * 100) * 0.03) * 20);
          
         DrawButterfly(g, butterflyX, butterflyY, time + i);
          }
 }

   private void DrawButterfly(Graphics g, int x, int y, int animOffset)
        {
       int wingFrame = (animOffset % 4) < 2 ? 0 : 1;
            
     Color[] butterflyColors = new Color[] { Color.Orange, Color.Purple, Color.Yellow };
        Color wingColor = butterflyColors[(animOffset / 10) % butterflyColors.Length];
   
        g.DrawLine(new Pen(Color.Black, 2), x, y, x, y + 6);
   
         if (wingFrame == 0)
            {
        Point[] leftWing = new Point[]
          {
   new Point(x, y + 2),
         new Point(x - 6, y),
          new Point(x - 5, y + 5)
                };
    Point[] rightWing = new Point[]
         {
                 new Point(x, y + 2),
           new Point(x + 6, y),
       new Point(x + 5, y + 5)
    };
   g.FillPolygon(new SolidBrush(wingColor), leftWing);
     g.FillPolygon(new SolidBrush(wingColor), rightWing);
            }
            else
            {
        Point[] leftWing = new Point[]
  {
  new Point(x, y + 2),
    new Point(x - 3, y),
        new Point(x - 2, y + 5)
                };
 Point[] rightWing = new Point[]
       {
       new Point(x, y + 2),
      new Point(x + 3, y),
         new Point(x + 2, y + 5)
                };
       g.FillPolygon(new SolidBrush(wingColor), leftWing);
                g.FillPolygon(new SolidBrush(wingColor), rightWing);
            }
        }

        private void DrawGrass(Graphics g)
        {
            Color grassColor1 = Color.FromArgb(60, 180, 60);
 Color grassColor2 = Color.FromArgb(50, 160, 50);
 Color grassColor3 = Color.FromArgb(70, 190, 70);

     using (var grassPen1 = new Pen(grassColor1, 2))
            using (var grassPen2 = new Pen(grassColor2, 2))
            using (var grassPen3 = new Pen(grassColor3, 1))
          {
  for (int i = 0; i < this.ClientSize.Width; i += 15)
         {
int grassX = i + (i % 3 * 5);
              int grassY = game.BattlefieldY + 15;
      
               Pen currentPen = i % 3 == 0 ? grassPen1 : i % 3 == 1 ? grassPen2 : grassPen3;
         int height1 = 8 + (i % 4);
        int height2 = 10 + (i % 3);
         int height3 = 7 + (i % 5);
    
            g.DrawLine(currentPen, grassX, grassY, grassX + 2, grassY - height1);
     g.DrawLine(currentPen, grassX + 5, grassY, grassX + 3, grassY - height2);
          g.DrawLine(currentPen, grassX + 10, grassY, grassX + 12, grassY - height3);
         
      if (i % 2 == 0)
   {
        g.DrawLine(grassPen3, grassX + 7, grassY, grassX + 8, grassY - 5);
}
      }
         }
   }

    private void DrawRocks(Graphics g)
    {
            Color rockColor = Color.FromArgb(120, 120, 120);
            Color darkRockColor = Color.FromArgb(80, 80, 80);
            Color mossColor = Color.FromArgb(80, 120, 40);
         
  DrawRock(g, 150, game.BattlefieldY + 20, 15, rockColor, darkRockColor, mossColor);
       DrawRock(g, 350, game.BattlefieldY + 25, 12, rockColor, darkRockColor, mossColor);
  DrawRock(g, 600, game.BattlefieldY + 22, 18, rockColor, darkRockColor, mossColor);
            DrawRock(g, 850, game.BattlefieldY + 24, 14, rockColor, darkRockColor, mossColor);
      DrawRock(g, this.ClientSize.Width - 300, game.BattlefieldY + 24, 16, rockColor, darkRockColor, mossColor);
        DrawRock(g, this.ClientSize.Width - 500, game.BattlefieldY + 21, 13, rockColor, darkRockColor, mossColor);
        }

  private void DrawRock(Graphics g, int x, int y, int size, Color rockColor, Color darkRockColor, Color mossColor)
        {
  using (var shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
    g.FillEllipse(shadowBrush, x - 2, y + size - 2, size + 8, size / 2);
            }

            using (var brush = new SolidBrush(rockColor))
     using (var darkBrush = new SolidBrush(darkRockColor))
     {
            Point[] rockPoints = new Point[]
     {
      new Point(x, y + size / 2),
    new Point(x + size / 3, y),
    new Point(x + size * 2 / 3, y + size / 4),
 new Point(x + size, y + size / 2),
    new Point(x + size * 2 / 3, y + size),
      new Point(x + size / 3, y + size)
       };
    g.FillPolygon(brush, rockPoints);
         
        Point[] darkSide = new Point[]
           {
      new Point(x + size * 2 / 3, y + size / 4),
       new Point(x + size, y + size / 2),
   new Point(x + size * 2 / 3, y + size)
  };
    g.FillPolygon(darkBrush, darkSide);
      }
        
      for (int i = 0; i < 3; i++)
            {
    int mossX = x + (i * size / 3) + 2;
    int mossY = y + size / 2 + (i % 2) * 3;
      g.FillEllipse(new SolidBrush(mossColor), mossX, mossY, 4, 3);
          }
   }
    }
}
