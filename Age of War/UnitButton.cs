using System;
using System.Drawing;
using System.Windows.Forms;

namespace Age_of_War
{
    // Anpassad knapp som visar en enhet och dess kostnad
    public class UnitButton : Button
 {
    private Type unitType;
        private int cost;
        private Bitmap unitImage;
        private bool isSellButton;  // Om detta är en "sälj"-knapp
        private bool isExpandButton;  // Om detta är en "expand"-knapp

        public UnitButton(Type unitType, int cost, bool isSellButton = false, bool isExpandButton = false)
        {
   this.unitType = unitType;
       this.cost = cost;
   this.isSellButton = isSellButton;
   this.isExpandButton = isExpandButton;
   this.Size = new Size(100, 120);
    this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderColor = Color.SaddleBrown;
    this.FlatAppearance.BorderSize = 3;
   this.BackColor = Color.FromArgb(240, 230, 210);
            
          // Skapa en bild av enheten
   CreateUnitImage();
     
    // Rita om när knappen behöver uppdateras
            this.Paint += UnitButton_Paint;
     }

        private void CreateUnitImage()
{
    if (isSellButton || isExpandButton)
    {
  // Skapa ingen bild för sell/expand-knapp, vi ritar en ikon istället
  return;
    }
    
    unitImage = new Bitmap(80, 80);
  using (Graphics g = Graphics.FromImage(unitImage))
    {
  g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);
      
    // Rita enheten/turret centrerad i bilden
    if (unitType == typeof(Soldier))
        {
            Unit tempUnit = new Soldier(true, 40, 70);
   int originalMaxHP = tempUnit.MaxHP;
   tempUnit.MaxHP = 0;
   tempUnit.Draw(g);
      tempUnit.MaxHP = originalMaxHP;
  }
      else if (unitType == typeof(RangedUnit))
   {
     Unit tempUnit = new RangedUnit(true, 40, 70);
   int originalMaxHP = tempUnit.MaxHP;
   tempUnit.MaxHP = 0;
    tempUnit.Draw(g);
        tempUnit.MaxHP = originalMaxHP;
        }
        else if (unitType == typeof(CavalryUnit))
 {
    Unit tempUnit = new CavalryUnit(true, 40, 70);
   int originalMaxHP = tempUnit.MaxHP;
   tempUnit.MaxHP = 0;
   tempUnit.Draw(g);
    tempUnit.MaxHP = originalMaxHP;
}
        else if (unitType == typeof(BasicTurret))
        {
     Turret tempTurret = new BasicTurret(true, 40, 60);  // Justerad Y för mindre turret
   int originalMaxHP = tempTurret.MaxHP;
  tempTurret.MaxHP = 0;
 tempTurret.Draw(g);
            tempTurret.MaxHP = originalMaxHP;
 }
        else if (unitType == typeof(AdvancedTurret))
  {
  Turret tempTurret = new AdvancedTurret(true, 40, 60);  // Justerad Y för mindre turret
   int originalMaxHP = tempTurret.MaxHP;
   tempTurret.MaxHP = 0;
  tempTurret.Draw(g);
     tempTurret.MaxHP = originalMaxHP;
    }
  else if (unitType == typeof(PremiumTurret))
   {
   Turret tempTurret = new PremiumTurret(true, 40, 62);  // Justerad Y för mindre turret
      int originalMaxHP = tempTurret.MaxHP;
          tempTurret.MaxHP = 0;
   tempTurret.Draw(g);
      tempTurret.MaxHP = originalMaxHP;
 }
        else
   {
     Unit tempUnit = new Soldier(true, 40, 70);
   int originalMaxHP = tempUnit.MaxHP;
tempUnit.MaxHP = 0;
    tempUnit.Draw(g);
   tempUnit.MaxHP = originalMaxHP;
 }
    }
}

  private void UnitButton_Paint(object sender, PaintEventArgs e)
        {
   var g = e.Graphics;
   g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
  
    // Rita bakgrund
            using (var bgBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
      this.ClientRectangle,
       Color.FromArgb(240, 230, 210),
       Color.FromArgb(220, 200, 180),
     System.Drawing.Drawing2D.LinearGradientMode.Vertical))
{
g.FillRectangle(bgBrush, this.ClientRectangle);
     }

if (isSellButton)
    {
      // Rita "sälj"-ikon (röd cirkel med vitt X och mynt-symbol)
  // Stor röd cirkel
        using (var redBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
   new Rectangle(20, 20, 60, 60),
   Color.Red,
   Color.DarkRed,
    System.Drawing.Drawing2D.LinearGradientMode.Vertical))
 {
            g.FillEllipse(redBrush, 20, 20, 60, 60);
  }
    g.DrawEllipse(new Pen(Color.DarkRed, 3), 20, 20, 60, 60);
        
        // Vitt X
 using (var whitePen = new Pen(Color.White, 4))
        {
     g.DrawLine(whitePen, 35, 35, 65, 65);
     g.DrawLine(whitePen, 65, 35, 35, 65);
  }
  
        // Guld-mynt symbol nedanför
        g.FillEllipse(Brushes.Gold, 35, 85, 30, 30);
        g.DrawEllipse(new Pen(Color.DarkGoldenrod, 2), 35, 85, 30, 30);
  using (var font = new Font("Arial", 14, FontStyle.Bold))
  {
       g.DrawString("$", font, Brushes.DarkGoldenrod, 43, 90);
        }
  }
    else if (isExpandButton)
{
    // Rita "expand"-ikon (torn som byggs uppåt)
    // Bas-torn
    g.FillRectangle(Brushes.Gray, 30, 50, 40, 40);
  g.DrawRectangle(new Pen(Color.DarkGray, 2), 30, 50, 40, 40);
    
    // Extra torn ovanpå (mindre)
    g.FillRectangle(Brushes.LightGray, 35, 35, 30, 20);
    g.DrawRectangle(new Pen(Color.DarkGray, 2), 35, 35, 30, 20);
    
    // Ytterligare torn (ännu mindre)
    g.FillRectangle(Brushes.Silver, 40, 23, 20, 15);
    g.DrawRectangle(new Pen(Color.DarkGray, 2), 40, 23, 20, 15);
    
    // Uppåtpil för att indikera expansion
    using (var arrowBrush = new SolidBrush(Color.FromArgb(200, 0, 255, 0)))
    {
        Point[] arrow = new Point[]
      {
            new Point(50, 10),  // Topp
            new Point(42, 20),  // Vänster
            new Point(47, 20),  // Vänster mitt
       new Point(47, 28),  // Vänster botten
       new Point(53, 28),  // Höger botten
 new Point(53, 20),  // Höger mitt
         new Point(58, 20) // Höger
      };
        g.FillPolygon(arrowBrush, arrow);
        g.DrawPolygon(new Pen(Color.Green, 2), arrow);
    }
    
    // Pris nedanför
    using (var font = new Font("Arial", 10, FontStyle.Bold))
  {
        g.DrawString("3000g", font, Brushes.DarkGoldenrod, 28, 95);
    }
}
else
{
        // Rita enhets-bilden (eller turret-bilden för toggle)
   if (unitImage != null)
   {
            g.DrawImage(unitImage, 10, 10, 80, 80);
        }

    // Rita pris-etikett (bara om cost > 0)
        if (cost > 0)
        {
   Rectangle priceRect = new Rectangle(5, 95, 90, 22);
            using (var priceBrush = new SolidBrush(Color.FromArgb(200, 139, 69, 19)))
   {
    g.FillRoundedRectangle(priceBrush, priceRect, 5);
   }

// Rita guld-ikon
   g.FillEllipse(Brushes.Gold, 10, 99, 14, 14);
   g.DrawEllipse(new Pen(Color.DarkGoldenrod, 2), 10, 99, 14, 14);

            // Rita pris-text
  string priceText = cost.ToString();
   using (var font = new Font("Arial", 12, FontStyle.Bold))
    {
     var textSize = g.MeasureString(priceText, font);
    g.DrawString(priceText, font, Brushes.White,
     priceRect.X + priceRect.Width / 2 - textSize.Width / 2 + 10,
         priceRect.Y + priceRect.Height / 2 - textSize.Height / 2);
   }
  }
    }

    // Rita kant
    g.DrawRectangle(new Pen(Color.SaddleBrown, 3), 1, 1, this.Width - 3, this.Height - 3);
}

      protected override void Dispose(bool disposing)
        {
            if (disposing && unitImage != null)
            {
    unitImage.Dispose();
      }
   base.Dispose(disposing);
        }
    }
    
    // Extension method för rundade rektanglar
    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
        {
          using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
         path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
       path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
         path.CloseFigure();
      g.FillPath(brush, path);
            }
      }
    }
}
