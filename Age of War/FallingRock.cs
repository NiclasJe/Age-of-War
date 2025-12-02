using System;
using System.Drawing;

namespace Age_of_War
{
  // Fallande sten för ultimate attack
    public class FallingRock
    {
      public float X, Y;
        public float VelocityY;
        public int Size;
        public bool IsActive = true;
        private Random random = new Random();
        private int rotationAngle = 0;
        
        public FallingRock(float x, float startY)
  {
            X = x;
 Y = startY;
            VelocityY = 4 + (float)(random.NextDouble() * 2);  // 4-6 pixlar per frame (minskat från 8-12)
            Size = 20 + random.Next(15);  // 20-35 pixlar storlek
        rotationAngle = random.Next(360);
     }
        
        public Rectangle Hitbox => new Rectangle((int)X - Size / 2, (int)Y - Size / 2, Size, Size);
        
    public void Update()
        {
    Y += VelocityY;
     VelocityY += 0.15f;  // Mindre gravitation (var 0.3f)
            rotationAngle += 3;  // Långsammare rotation (var 5)
        }
        
        public void Draw(Graphics g)
   {
            // Spara grafik-state
 var state = g.Save();
     
  // Flytta och rotera
          g.TranslateTransform(X, Y);
            g.RotateTransform(rotationAngle);

            // Rita sten med gradient
      using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
             new Rectangle(-Size/2, -Size/2, Size, Size),
            Color.FromArgb(100, 100, 100),
  Color.FromArgb(60, 60, 60),
      45f))
            {
       // Oregelbunden sten-form
    Point[] rockPoints = new Point[]
      {
                    new Point(-Size/2, -Size/4),
        new Point(-Size/3, -Size/2),
               new Point(0, -Size/2 + 3),
          new Point(Size/3, -Size/2),
        new Point(Size/2, -Size/4),
         new Point(Size/2 - 2, Size/4),
      new Point(Size/4, Size/2),
    new Point(-Size/4, Size/2),
   new Point(-Size/2, Size/4)
};
       
       g.FillPolygon(brush, rockPoints);
   g.DrawPolygon(new Pen(Color.Black, 2), rockPoints);
         }
          
            // Rita skugga under stenen (för djup-effekt)
using (var shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            {
        g.FillEllipse(shadowBrush, -Size/3, Size/2, Size*2/3, Size/4);
          }
 
        // Återställ grafik-state
            g.Restore(state);
      }
    }
}
