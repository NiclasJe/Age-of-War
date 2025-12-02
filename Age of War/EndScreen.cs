using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Age_of_War
{
    public class EndScreen
    {
        private bool playerWon;
        private Rectangle playAgainButton;
 private Rectangle exitButton;
        private bool playAgainButtonHover = false;
        private bool exitButtonHover = false;
        private int animationTimer = 0;
        private float explosionRadius = 0;
        private float titleScale = 0;
        private float buttonAlpha = 0;
        
        public EndScreen(bool playerWon, int screenWidth, int screenHeight)
        {
this.playerWon = playerWon;
   UpdateDimensions(screenWidth, screenHeight);
}
        
        public void UpdateDimensions(int screenWidth, int screenHeight)
        {
            int buttonWidth = 280;
     int buttonHeight = 65;
            int centerX = screenWidth / 2 - buttonWidth / 2;
         int centerY = screenHeight / 2 + 80;
            
      playAgainButton = new Rectangle(centerX, centerY, buttonWidth, buttonHeight);
            exitButton = new Rectangle(centerX, centerY + 90, buttonWidth, buttonHeight);
    }
     
        public void Update(int deltaTime)
        {
            animationTimer += deltaTime;
            
    // Explosion animation (första 800ms)
  if (animationTimer < 800)
       {
     explosionRadius = (animationTimer / 800f) * 500;
          }
            else
        {
   explosionRadius = 500;
   }
  
          // Titel scale-in (200-1200ms)
       if (animationTimer >= 200 && animationTimer < 1200)
       {
      titleScale = ((animationTimer - 200) / 1000f);
            }
         else if (animationTimer >= 1200)
  {
      titleScale = 1f;
            }
       
  // Knappar fade-in (800-1600ms)
            if (animationTimer >= 800 && animationTimer < 1600)
          {
           buttonAlpha = ((animationTimer - 800) / 800f);
         }
        else if (animationTimer >= 1600)
      {
     buttonAlpha = 1f;
    }
        }
  
        public void CheckHover(Point mousePosition)
        {
       playAgainButtonHover = playAgainButton.Contains(mousePosition);
     exitButtonHover = exitButton.Contains(mousePosition);
        }
        
      public bool IsPlayAgainButtonClicked(Point mousePosition)
     {
         return playAgainButton.Contains(mousePosition) && buttonAlpha >= 1f;
        }
   
        public bool IsExitButtonClicked(Point mousePosition)
        {
  return exitButton.Contains(mousePosition) && buttonAlpha >= 1f;
        }
        
        public void Draw(Graphics g, int screenWidth, int screenHeight)
        {
          g.SmoothingMode = SmoothingMode.AntiAlias;
 
       // Semi-transparent overlay
    Color overlayColor = playerWon 
     ? Color.FromArgb(180, 20, 60, 20)  // Grön för vinst
          : Color.FromArgb(180, 60, 20, 20); // Röd för förlust
    using (var overlayBrush = new SolidBrush(overlayColor))
 {
  g.FillRectangle(overlayBrush, 0, 0, screenWidth, screenHeight);
   }
  
          // Explosion burst animation
            if (explosionRadius > 0)
 {
                int centerX = screenWidth / 2;
                int centerY = screenHeight / 2 - 50;
        
      // Flera ringar
      for (int i = 0; i < 5; i++)
          {
     float ringRadius = explosionRadius - (i * 80);
      if (ringRadius > 0)
    {
         int alpha = Math.Max(0, (int)(120 - (explosionRadius / 500f) * 120) - i * 20);
     Color burstColor = playerWon 
                 ? Color.FromArgb(alpha, 100, 255, 100)
       : Color.FromArgb(alpha, 255, 100, 100);
         
           using (var burstPen = new Pen(burstColor, 6 - i))
      {
       g.DrawEllipse(burstPen, 
        centerX - ringRadius, centerY - ringRadius, 
             ringRadius * 2, ringRadius * 2);
         }
  }
    }
 
     // Partiklar som flyger utåt
     Random rand = new Random(42);
     for (int i = 0; i < 20; i++)
         {
               double angle = (i / 20.0) * Math.PI * 2;
            float particleDistance = explosionRadius * 0.8f;
        float particleX = centerX + (float)(Math.Cos(angle) * particleDistance);
      float particleY = centerY + (float)(Math.Sin(angle) * particleDistance);
          
         Color particleColor = playerWon 
             ? Color.FromArgb(200, 150, 255, 150)
              : Color.FromArgb(200, 255, 150, 150);
             
      int particleSize = 8 - (int)(explosionRadius / 100);
if (particleSize > 0)
        {
               g.FillEllipse(new SolidBrush(particleColor), 
    particleX - particleSize/2, particleY - particleSize/2, 
       particleSize, particleSize);
    }
       }
          }
            
         // Titel med scale animation
    if (titleScale > 0)
 {
     string title = playerWon ? "SEGER!" : "FÖRLUST!";
      Color titleColor = playerWon ? Color.Gold : Color.OrangeRed;
     
   using (var titleFont = new Font("Impact", 90 * titleScale, FontStyle.Bold))
              {
   var textSize = g.MeasureString(title, titleFont);
        float titleX = (screenWidth - textSize.Width) / 2;
     float titleY = screenHeight / 2 - 100 - textSize.Height / 2;
              
     // Pulserar lite
   float pulseScale = 1f + (float)Math.Sin(animationTimer / 200.0) * 0.05f;
          
          var originalTransform = g.Transform;
       g.TranslateTransform(screenWidth / 2, titleY + textSize.Height / 2);
            g.ScaleTransform(pulseScale, pulseScale);
 g.TranslateTransform(-screenWidth / 2, -(titleY + textSize.Height / 2));
          
         // Glow
 for (int glow = 25; glow > 0; glow -= 5)
      {
        int alpha = 8 + (25 - glow);
        Color glowColor = playerWon 
   ? Color.FromArgb(alpha, 200, 255, 100)
           : Color.FromArgb(alpha, 255, 150, 100);
       
        using (var glowBrush = new SolidBrush(glowColor))
          {
       g.DrawString(title, titleFont, glowBrush, titleX - glow/2, titleY - glow/2);
    g.DrawString(title, titleFont, glowBrush, titleX + glow/2, titleY + glow/2);
            }
                    }
       
       // Outline
      using (var path = new GraphicsPath())
      {
         path.AddString(title, titleFont.FontFamily, (int)titleFont.Style, titleFont.Size * 1.3f,
       new PointF(titleX, titleY), StringFormat.GenericDefault);
      
 using (var outlinePen = new Pen(Color.Black, 10))
        {
     g.DrawPath(outlinePen, path);
               }
         }
    
            // Main text with gradient
       using (var titleBrush = new LinearGradientBrush(
              new RectangleF(titleX, titleY, textSize.Width, textSize.Height),
   titleColor,
                playerWon ? Color.Yellow : Color.DarkRed,
    LinearGradientMode.Vertical))
       {
             g.DrawString(title, titleFont, titleBrush, titleX, titleY);
     }
          
          // Highlight
  using (var highlightBrush = new LinearGradientBrush(
      new RectangleF(titleX, titleY, textSize.Width, 40),
        Color.FromArgb(120, 255, 255, 255),
   Color.FromArgb(0, 255, 255, 255),
         LinearGradientMode.Vertical))
{
               g.DrawString(title, titleFont, highlightBrush, titleX, titleY);
    }
           
  g.Transform = originalTransform;
   }
         
         // Undertext
 string subtitle = playerWon 
        ? "Du har erövrat fiendens bas!" 
             : "Din bas har fallit...";
     
  using (var subtitleFont = new Font("Arial", 20, FontStyle.Bold | FontStyle.Italic))
             {
       var textSize = g.MeasureString(subtitle, subtitleFont);
        float subtitleX = (screenWidth - textSize.Width) / 2;
           float subtitleY = screenHeight / 2 - 20;
           
           // Skugga
 g.DrawString(subtitle, subtitleFont, Brushes.Black, subtitleX + 2, subtitleY + 2);

         // Text
        Color subtitleColor = playerWon 
    ? Color.LightGreen 
           : Color.LightCoral;
     g.DrawString(subtitle, subtitleFont, new SolidBrush(subtitleColor), subtitleX, subtitleY);
       }
      }
            
      // Knappar med fade-in
       if (buttonAlpha > 0)
   {
  // Play Again button
       DrawButton(g, playAgainButton, "SPELA IGEN", playAgainButtonHover, 
         Color.FromArgb((int)(buttonAlpha * 255), 50, 150, 50),
        Color.FromArgb((int)(buttonAlpha * 255), 80, 200, 80),
            buttonAlpha);
    
                // Exit button
      DrawButton(g, exitButton, "AVSLUTA", exitButtonHover,
    Color.FromArgb((int)(buttonAlpha * 255), 100, 100, 100),
           Color.FromArgb((int)(buttonAlpha * 255), 140, 140, 140),
    buttonAlpha);
    }
            
            // Konfetti om spelaren vann (efter animation är klar)
     if (playerWon && animationTimer > 1200)
      {
  DrawConfetti(g, screenWidth, screenHeight);
          }
        }

        private void DrawButton(Graphics g, Rectangle rect, string text, bool hover, Color normalColor, Color hoverColor, float alpha)
      {
         Color baseColor = hover ? hoverColor : normalColor;
            
 // Animation vid hover
         int inflate = hover ? 5 : 0;
     Rectangle buttonRect = rect;
            if (inflate > 0)
  {
 buttonRect.Inflate(inflate, inflate);
    }
   
 // Gradient bakgrund
using (var buttonBrush = new LinearGradientBrush(
  buttonRect,
       baseColor,
         Color.FromArgb(baseColor.A, Math.Max(0, baseColor.R - 40), Math.Max(0, baseColor.G - 40), Math.Max(0, baseColor.B - 40)),
     LinearGradientMode.Vertical))
        {
  using (var path = GetRoundedRect(buttonRect, 12))
    {
      g.FillPath(buttonBrush, path);
    
        // Outline
     using (var outlinePen = new Pen(Color.FromArgb((int)(alpha * 200), 255, 255, 255), hover ? 3 : 2))
    {
          g.DrawPath(outlinePen, path);
  }
    }
     }

 // Highlight
   Rectangle highlightRect = new Rectangle(buttonRect.X, buttonRect.Y, buttonRect.Width, buttonRect.Height / 3);
      using (var highlightBrush = new LinearGradientBrush(
            highlightRect,
              Color.FromArgb((int)(alpha * 60), 255, 255, 255),
      Color.FromArgb(0, 255, 255, 255),
    LinearGradientMode.Vertical))
        {
             using (var path = GetRoundedRect(highlightRect, 12))
  {
     g.FillPath(highlightBrush, path);
     }
            }
       
            // Text
        using (var font = new Font("Arial", 22, FontStyle.Bold))
        {
     var textSize = g.MeasureString(text, font);
      float textX = buttonRect.X + (buttonRect.Width - textSize.Width) / 2;
                float textY = buttonRect.Y + (buttonRect.Height - textSize.Height) / 2;
          
       // Skugga
           g.DrawString(text, font, new SolidBrush(Color.FromArgb((int)(alpha * 255), 0, 0, 0)), textX + 2, textY + 2);
  
           // Text
     g.DrawString(text, font, new SolidBrush(Color.FromArgb((int)(alpha * 255), 255, 255, 255)), textX, textY);
             
     // Glow vid hover
             if (hover)
       {
using (var glowBrush = new SolidBrush(Color.FromArgb((int)(alpha * 80), 255, 255, 150)))
         {
          g.DrawString(text, font, glowBrush, textX - 1, textY - 1);
               g.DrawString(text, font, glowBrush, textX + 1, textY + 1);
              }
           }
    }
        }
   
      private void DrawConfetti(Graphics g, int screenWidth, int screenHeight)
  {
     Random rand = new Random(42);
            int confettiCount = 50;
        
      for (int i = 0; i < confettiCount; i++)
            {
    // Beräkna position baserat på tid och seed
           float timeOffset = ((animationTimer - 1200) / 50f + i * 10) % 100;
       float x = (i * 47.3f) % screenWidth;
              float y = (timeOffset * 10 - 50) % screenHeight;
       
       // Olika färger
           Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Magenta, Color.Cyan };
  Color color = colors[i % colors.Length];
     
 // Rotation
     float rotation = (animationTimer / 30f + i * 20) % 360;
  
                var originalTransform = g.Transform;
    g.TranslateTransform(x, y);
          g.RotateTransform(rotation);
                
          // Rita konfetti-bit (rektangel)
    g.FillRectangle(new SolidBrush(color), -3, -8, 6, 16);
   
    g.Transform = originalTransform;
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
