using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Age_of_War
{
    public class MainMenu
    {
        private Rectangle playButton;
        private Rectangle exitButton;
 private bool playButtonHover = false;
        private bool exitButtonHover = false;
     private int animationFrame = 0;
        
        public MainMenu(int screenWidth, int screenHeight)
        {
UpdateDimensions(screenWidth, screenHeight);
        }
        
        public void UpdateDimensions(int screenWidth, int screenHeight)
        {
       int buttonWidth = 300;
    int buttonHeight = 70;
            int centerX = screenWidth / 2 - buttonWidth / 2;
            int centerY = screenHeight / 2;
         
  playButton = new Rectangle(centerX, centerY + 20, buttonWidth, buttonHeight);
 exitButton = new Rectangle(centerX, centerY + 110, buttonWidth, buttonHeight);
    }
        
        public void Update(int deltaTime)
  {
            animationFrame += deltaTime;
        }
        
        public void CheckHover(Point mousePosition)
        {
  playButtonHover = playButton.Contains(mousePosition);
   exitButtonHover = exitButton.Contains(mousePosition);
        }
        
        public bool IsPlayButtonClicked(Point mousePosition)
 {
            return playButton.Contains(mousePosition);
        }
        
        public bool IsExitButtonClicked(Point mousePosition)
    {
            return exitButton.Contains(mousePosition);
        }
        
        public void Draw(Graphics g, int screenWidth, int screenHeight)
        {
 g.SmoothingMode = SmoothingMode.AntiAlias;
            
         // Bakgrund med gradient - episk himmel
       using (var skyBrush = new LinearGradientBrush(
    new Rectangle(0, 0, screenWidth, screenHeight),
  Color.FromArgb(20, 30, 80),      // Mörk midnattsblå
       Color.FromArgb(100, 50, 150), // Lila/violett
       LinearGradientMode.Vertical))
    {
        g.FillRectangle(skyBrush, 0, 0, screenWidth, screenHeight);
            }
    
 // Stjärnor
          Random rand = new Random(42);
  for (int i = 0; i < 100; i++)
   {
              int x = rand.Next(screenWidth);
    int y = rand.Next(screenHeight);
  int brightness = 150 + rand.Next(105);
      int size = rand.Next(1, 3);
  
      // Blinkande effekt
     int twinkle = (animationFrame / 500 + i) % 20;
       if (twinkle < 10)
    brightness = Math.Max(50, brightness - twinkle * 15);
     
       // Säkerställ att alla färgvärden är inom 0-255
         int blueValue = Math.Min(255, brightness + 50);
   g.FillEllipse(new SolidBrush(Color.FromArgb(brightness, brightness, blueValue)), x, y, size, size);
  }
     
          // Måne
using (var moonBrush = new PathGradientBrush(
           new Point[] {
new Point(screenWidth - 200, 100),
       new Point(screenWidth - 140, 70),
     new Point(screenWidth - 80, 100),
       new Point(screenWidth - 140, 130)
    }))
  {
                moonBrush.CenterColor = Color.FromArgb(255, 255, 220);
     moonBrush.SurroundColors = new Color[] {
 Color.FromArgb(240, 240, 200),
      Color.FromArgb(235, 235, 190),
       Color.FromArgb(240, 240, 200),
         Color.FromArgb(235, 235, 190)
  };
     g.FillEllipse(moonBrush, screenWidth - 240, 50, 120, 120);
      }
    
            // Silhuetter av berg i förgrunden
          DrawMountainSilhouette(g, screenWidth, screenHeight);
    
       // Titel - "AGE OF WAR" med epic stil
DrawTitle(g, screenWidth, screenHeight);
   
 // Play-knapp
DrawButton(g, playButton, "STARTA SPELET", playButtonHover, Color.FromArgb(200, 50, 50), Color.FromArgb(255, 80, 80));
     
        // Exit-knapp
            DrawButton(g, exitButton, "AVSLUTA", exitButtonHover, Color.FromArgb(80, 80, 80), Color.FromArgb(120, 120, 120));
    
    // Version info
 using (var font = new Font("Arial", 10, FontStyle.Italic))
 {
 g.DrawString("v1.0 - Epic Strategy Game", font, Brushes.LightGray, 10, screenHeight - 30);
}
    }

        private void DrawTitle(Graphics g, int screenWidth, int screenHeight)
  {
    string title = "AGE OF WAR";
            
       // Titelns position
    int titleY = screenHeight / 2 - 150;
       
            // Stora bokstäver med outline och glow
       using (var titleFont = new Font("Impact", 80, FontStyle.Bold))
            {
        var textSize = g.MeasureString(title, titleFont);
    float titleX = (screenWidth - textSize.Width) / 2;
        
   // Glow-effekt (flera lager)
   for (int glow = 20; glow > 0; glow -= 4)
   {
        int alpha = 10 + (20 - glow) * 2;
              using (var glowBrush = new SolidBrush(Color.FromArgb(alpha, 255, 200, 100)))
         {
            g.DrawString(title, titleFont, glowBrush, titleX - glow/2, titleY - glow/2);
     g.DrawString(title, titleFont, glowBrush, titleX + glow/2, titleY + glow/2);
         }
         }
       
      // Svart outline
      using (var path = new GraphicsPath())
  {
  path.AddString(title, titleFont.FontFamily, (int)titleFont.Style, titleFont.Size * 1.3f, 
            new PointF(titleX, titleY), StringFormat.GenericDefault);
           
     using (var outlinePen = new Pen(Color.Black, 8))
    {
         g.DrawPath(outlinePen, path);
           }
  }
    
    // Gradient-fyllning (guld till orange)
     using (var titleBrush = new LinearGradientBrush(
          new RectangleF(titleX, titleY, textSize.Width, textSize.Height),
         Color.Gold,
         Color.OrangeRed,
           LinearGradientMode.Vertical))
         {
   g.DrawString(title, titleFont, titleBrush, titleX, titleY);
        }
      
       // Highlight på toppen
    using (var highlightBrush = new LinearGradientBrush(
 new RectangleF(titleX, titleY, textSize.Width, 30),
          Color.FromArgb(150, 255, 255, 200),
         Color.FromArgb(0, 255, 255, 200),
       LinearGradientMode.Vertical))
         {
       g.DrawString(title, titleFont, highlightBrush, titleX, titleY);
     }
         }
          
            // Undertext
         string subtitle = "Försvara din bas - Erövra fiendens!";
     using (var subtitleFont = new Font("Arial", 18, FontStyle.Bold | FontStyle.Italic))
   {
   var textSize = g.MeasureString(subtitle, subtitleFont);
         float subtitleX = (screenWidth - textSize.Width) / 2;
  
           // Skugga
       g.DrawString(subtitle, subtitleFont, Brushes.Black, subtitleX + 2, titleY + 100 + 2);
           // Text
using (var gradientBrush = new LinearGradientBrush(
             new RectangleF(subtitleX, titleY + 100, textSize.Width, textSize.Height),
    Color.LightGoldenrodYellow,
              Color.White,
   LinearGradientMode.Horizontal))
      {
   g.DrawString(subtitle, subtitleFont, gradientBrush, subtitleX, titleY + 100);
    }
    }
      }
        
        private void DrawButton(Graphics g, Rectangle rect, string text, bool hover, Color normalColor, Color hoverColor)
        {
   // Välj färg baserat på hover
         Color baseColor = hover ? hoverColor : normalColor;
            
    // Knapp-animation vid hover
            int inflate = hover ? 5 : 0;
            Rectangle buttonRect = rect;
          if (inflate > 0)
  {
                buttonRect.Inflate(inflate, inflate);
    }
    
         // Gradient-bakgrund
        using (var buttonBrush = new LinearGradientBrush(
                buttonRect,
                baseColor,
          Color.FromArgb(baseColor.A, Math.Max(0, baseColor.R - 50), Math.Max(0, baseColor.G - 50), Math.Max(0, baseColor.B - 50)),
     LinearGradientMode.Vertical))
       {
      // Rundade hörn
         using (var path = GetRoundedRect(buttonRect, 15))
                {
       g.FillPath(buttonBrush, path);
      
    // Outline
        using (var outlinePen = new Pen(Color.FromArgb(200, 255, 255, 255), hover ? 4 : 3))
  {
       g.DrawPath(outlinePen, path);
           }
         }
    }
            
   // Highlight på toppen av knappen
            Rectangle highlightRect = new Rectangle(buttonRect.X, buttonRect.Y, buttonRect.Width, buttonRect.Height / 3);
            using (var highlightBrush = new LinearGradientBrush(
       highlightRect,
     Color.FromArgb(80, 255, 255, 255),
          Color.FromArgb(0, 255, 255, 255),
           LinearGradientMode.Vertical))
            {
           using (var path = GetRoundedRect(highlightRect, 15))
 {
        g.FillPath(highlightBrush, path);
  }
            }

       // Text med skugga
  using (var font = new Font("Arial", 24, FontStyle.Bold))
            {
           var textSize = g.MeasureString(text, font);
                float textX = buttonRect.X + (buttonRect.Width - textSize.Width) / 2;
        float textY = buttonRect.Y + (buttonRect.Height - textSize.Height) / 2;
    
    // Skugga
        g.DrawString(text, font, Brushes.Black, textX + 2, textY + 2);
       
  // Text
   g.DrawString(text, font, Brushes.White, textX, textY);
                
       // Glow vid hover
      if (hover)
       {
 using (var glowBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 150)))
      {
   g.DrawString(text, font, glowBrush, textX - 1, textY - 1);
            g.DrawString(text, font, glowBrush, textX + 1, textY + 1);
         }
   }
            }
  }
   
  private void DrawMountainSilhouette(Graphics g, int screenWidth, int screenHeight)
        {
            // Fjärran berg (mörkast)
  Point[] distantMountains = new Point[]
            {
     new Point(0, screenHeight),
        new Point(0, screenHeight - 180),
      new Point(200, screenHeight - 200),
         new Point(400, screenHeight - 150),
                new Point(600, screenHeight - 220),
        new Point(800, screenHeight - 180),
                new Point(screenWidth, screenHeight - 140),
      new Point(screenWidth, screenHeight)
            };
            using (var brush = new SolidBrush(Color.FromArgb(20, 20, 40)))
      {
  g.FillPolygon(brush, distantMountains);
        }
            
   // Mellersta berg
  Point[] midMountains = new Point[]
  {
       new Point(0, screenHeight),
    new Point(0, screenHeight - 120),
      new Point(300, screenHeight - 160),
        new Point(500, screenHeight - 100),
    new Point(700, screenHeight - 170),
          new Point(screenWidth, screenHeight - 110),
     new Point(screenWidth, screenHeight)
      };
  using (var brush = new SolidBrush(Color.FromArgb(30, 30, 50)))
         {
    g.FillPolygon(brush, midMountains);
    }
            
    // Närliggande berg (ljusast)
    Point[] nearMountains = new Point[]
      {
          new Point(0, screenHeight),
    new Point(0, screenHeight - 80),
    new Point(250, screenHeight - 120),
    new Point(450, screenHeight - 60),
              new Point(650, screenHeight - 130),
  new Point(screenWidth, screenHeight - 90),
      new Point(screenWidth, screenHeight)
       };
            using (var brush = new SolidBrush(Color.FromArgb(40, 40, 60)))
         {
       g.FillPolygon(brush, nearMountains);
            }
}
   
 private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
 {
            GraphicsPath path = new GraphicsPath();
 path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
     path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
          path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
      path.CloseFigure();
         return path;
        }
    }
}
