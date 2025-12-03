using System.Drawing;

namespace Age_of_War
{
    // Ridande soldat - Grottmänniska på ödla
    public class CavalryUnit : Unit
    {
        public CavalryUnit(bool isPlayer, float startX, int y)
    : base(isPlayer, startX, y)
        {
       MaxHP = HP = 180;
            Damage = 35;
       Speed = 2.0f;
    Cost = 200;  // Ändrat från 120 till 200
  AttackCooldown = 900;
        }

  public override void Draw(Graphics g)
 {
   int x = (int)PositionX;
  int y = PositionY + (int)(DeathCollapseProgress * 40);  // Faller ihop nedåt
  int direction = IsPlayer ? 1 : -1;
  
    // Beräkna alpha för fade-out
    int alpha = (int)(DeathAlpha * 255);
  
    // Färger med alpha
    Color lizardColor = Color.FromArgb(alpha, 100, 150, 50);
  Color darkLizard = Color.FromArgb(alpha, 70, 110, 30);
    Color lightLizard = Color.FromArgb(alpha, 130, 180, 70);
    Color skinColor = Color.FromArgb(alpha, 210, 180, 140);
    Color darkSkin = Color.FromArgb(alpha, 180, 150, 110);
    Color lightSkin = Color.FromArgb(alpha, 230, 200, 160);
    
    // Ödlesvans (bakgrundslager)
    int tailX = x + (IsPlayer ? -8 : 45);
    for (int i = 0; i < 5; i++)
    {
        int tailSegmentX = tailX + (direction * -i * 6);
        int tailWidth = 12 - i * 2;
     int tailHeight = 9 - i;
        
     // Svans-segment med skuggning
      g.FillEllipse(new SolidBrush(lizardColor), tailSegmentX, y - 20 + i, tailWidth, tailHeight);
  // Ljus sida
        g.FillEllipse(new SolidBrush(lightLizard), tailSegmentX + 1, y - 19 + i, tailWidth / 2, tailHeight - 2);
        // Tagg på svansen
     if (i < 3)
        {
  Point[] spike = new Point[]
          {
       new Point(tailSegmentX + tailWidth / 2, y - 22 + i),
      new Point(tailSegmentX + tailWidth / 2 - 2, y - 19 + i),
         new Point(tailSegmentX + tailWidth / 2 + 2, y - 19 + i)
      };
         g.FillPolygon(new SolidBrush(darkLizard), spike);
        }
    }
    
    // Ödlekropp (oval) med mer detaljer
    g.FillEllipse(new SolidBrush(lizardColor), x, y - 22, 45, 20);
  
    // Fjäll på kroppen
    for (int i = 0; i < 7; i++)
    {
     for (int j = 0; j < 3; j++)
        {
   int scaleX = x + 5 + (i * 6);
    int scaleY = y - 20 + (j * 5);
      g.FillEllipse(new SolidBrush(darkLizard), scaleX, scaleY, 4, 3);
        }
    }
    
    // Ödlens rygg (ryggrad)
    for (int i = 0; i < 8; i++)
    {
Point[] backSpike = new Point[]
 {
      new Point(x + 5 + (i * 5), y - 24),
    new Point(x + 3 + (i * 5), y - 22),
 new Point(x + 7 + (i * 5), y - 22)
        };
        g.FillPolygon(new SolidBrush(darkLizard), backSpike);
    }
    
    // Sadel på ödlan (primitiv)
    Color saddleColor = Color.FromArgb(alpha, 101, 67, 33);
    
    // Lägg till en svajig rörelse för kroppen när ödlan går
  int bodyBounce = 0;
    if (!IsBlocked && !IsDying)
    {
      if (animationFrame % 4 == 1 || animationFrame % 4 == 3)
        {
            bodyBounce = -2;  // Kroppen rör sig lite uppåt när benen lyfts
        }
    }
    
    g.FillEllipse(new SolidBrush(saddleColor), x + 12, y - 24 + bodyBounce, 22, 8);
    // Sadel-textur
    g.DrawLine(new Pen(Color.FromArgb(alpha, Color.SaddleBrown), 2), x + 14, y - 22 + bodyBounce, x + 32, y - 22 + bodyBounce);
    // Sadel-remmar
 g.DrawLine(new Pen(Color.FromArgb(alpha, Color.SaddleBrown), 2), x + 18, y - 20 + bodyBounce, x + 18, y - 10);
 g.DrawLine(new Pen(Color.FromArgb(alpha, Color.SaddleBrown), 2), x + 28, y - 20 + bodyBounce, x + 28, y - 10);
    
    // Ödlehuvud (i profil) - mer detaljerat med svajig rörelse
    int headX = x + (IsPlayer ? 43 : -8);
    int headBounce = bodyBounce / 2;  // Huvudet rör sig hälften så mycket som kroppen
    
 // Huvud
    g.FillEllipse(new SolidBrush(lizardColor), headX, y - 24 + headBounce, 14, 12);
    // Ljus undersida
    g.FillEllipse(new SolidBrush(lightLizard), headX + 2, y - 19 + headBounce, 10, 6);
    
    // Nos-rygg med fjäll
    for (int i = 0; i < 3; i++)
    {
    g.FillEllipse(new SolidBrush(darkLizard), headX + 3 + (i * 3), y - 26 + headBounce, 3, 2);
    }
  
    // Öga med ögonlock
    int eyeX = headX + (IsPlayer ? 10 : 3);
    g.FillEllipse(Brushes.Yellow, eyeX, y - 21 + headBounce, 4, 4);
    g.FillEllipse(Brushes.Black, eyeX + 1, y - 20 + headBounce, 2, 2);
    // Ögonlock (reptil-stil)
    g.DrawArc(new Pen(darkLizard, 1), eyeX, y - 21 + headBounce, 4, 3, 180, 180);
    
 // Mun med tänder
    g.DrawLine(new Pen(Color.Black, 2), headX + (IsPlayer ? 12 : 2), y - 16 + headBounce, headX + (IsPlayer ? 14 : 0), y - 15 + headBounce);
    // Små tänder
    for (int i = 0; i < 2; i++)
    {
g.DrawLine(Pens.White, headX + (IsPlayer ? 12 : 2) + i, y - 16 + headBounce, headX + (IsPlayer ? 12 : 2) + i, y - 18 + headBounce);
    }
    
    // Näsborre
    g.FillEllipse(Brushes.Black, headX + (IsPlayer ? 12 : 2), y - 23 + headBounce, 2, 1);
    
    // Ödleben (walk animation) - mer detaljerade och smidigare
    int legOffset1X = 0, legOffset1Y = 0;
    int legOffset2X = 0, legOffset2Y = 0;
    
if (!IsBlocked && !IsDying)  // Ingen animation när man dör
    {
// Smooth walk cycle med sinus-kurva för naturligare rörelse
        if (animationFrame % 4 == 0)
  {
            legOffset1X = 0;
 legOffset1Y = 0;
         legOffset2X = 0;
    legOffset2Y = 0;
      }
      else if (animationFrame % 4 == 1)
{
            // Främre ben går framåt (upp och fram)
       legOffset1X = 6 * direction;
         legOffset1Y = -4;  // Lyfter upp lite
    // Bakre ben går bakåt (ner och bak)
            legOffset2X = -3 * direction;
       legOffset2Y = 2;
        }
   else if (animationFrame % 4 == 2)
 {
          legOffset1X = 0;
   legOffset1Y = 0;
      legOffset2X = 0;
  legOffset2Y = 0;
        }
        else // animationFrame % 4 == 3
        {
// Bakre ben går framåt (upp och fram)
     legOffset2X = 6 * direction;
            legOffset2Y = -4;  // Lyfter upp lite
      // Främre ben går bakåt (ner och bak)
        legOffset1X = -3 * direction;
   legOffset1Y = 2;
        }
    }
    
    // Bakre ben med mer detalj
    g.FillRectangle(new SolidBrush(lizardColor), x + 10 + legOffset2X, y - 8 + legOffset2Y, 5, 10);
    g.DrawLine(new Pen(darkLizard, 1), x + 12 + legOffset2X, y - 7 + legOffset2Y, x + 12 + legOffset2X, y - 2 + legOffset2Y);
    // Fot med klor
    g.FillEllipse(new SolidBrush(lizardColor), x + 8 + legOffset2X, y - 2 + legOffset2Y, 7, 4);
for (int i = 0; i < 3; i++)
    {
        g.DrawLine(new Pen(Color.Black, 1), x + 9 + (i * 2) + legOffset2X, y - 2 + legOffset2Y, x + 9 + (i * 2) + legOffset2X, y + legOffset2Y);
    }
    
    // Främre ben med mer detalj
    g.FillRectangle(new SolidBrush(lizardColor), x + 32 + legOffset1X, y - 8 + legOffset1Y, 5, 10);
    g.DrawLine(new Pen(lightLizard, 1), x + 34 + legOffset1X, y - 7 + legOffset1Y, x + 34 + legOffset1X, y - 2 + legOffset1Y);
    // Fot med klor
    g.FillEllipse(new SolidBrush(lizardColor), x + 30 + legOffset1X, y - 2 + legOffset1Y, 7, 4);
    for (int i = 0; i < 3; i++)
    {
        g.DrawLine(new Pen(Color.Black, 1), x + 31 + (i * 2) + legOffset1X, y - 2 + legOffset1Y, x + 31 + (i * 2) + legOffset1X, y + legOffset1Y);
    }
    
    // Rita ryttare (grottmänniska) ovanpå ödlan - justerad position för att sitta NED i sadeln
    int riderX = x + 15;
    int riderY = y - 8 + bodyBounce;  // Ryttaren svänger med ödlans kropp
 
    // Ryttarens bakre ben (på ödlans sidor)
    g.FillRectangle(new SolidBrush(skinColor), riderX - 3, riderY + 10, 5, 6);  // Ännu kortare ben
    g.DrawLine(new Pen(darkSkin, 1), riderX - 1, riderY + 11, riderX - 1, riderY + 14);
  // Bakre fot i stigbygel
    g.DrawLine(new Pen(Color.SaddleBrown, 2), riderX - 3, riderY + 14, riderX - 3, riderY + 17);
    g.FillRectangle(new SolidBrush(Color.SaddleBrown), riderX - 5, riderY + 15, 4, 3);
    
    // Kropp med mer detaljer
    g.FillEllipse(new SolidBrush(skinColor), riderX + 3, riderY - 35, 16, 22);
 
    // Bröstmuskler
    g.DrawArc(new Pen(darkSkin, 1), riderX + 5, riderY - 32, 6, 6, 180, 180);
    g.DrawArc(new Pen(darkSkin, 1), riderX + 10, riderY - 32, 6, 6, 180, 180);
  
  // Primitiv klädsel
 Color furColor = Color.FromArgb(alpha, 101, 67, 33);
    g.FillEllipse(new SolidBrush(furColor), riderX + 6, riderY - 20, 12, 8);
    // Päls-textur
    for (int i = 0; i < 5; i++)
  {
g.DrawLine(new Pen(Color.FromArgb(alpha, 80, 50, 20), 1), riderX + 7 + (i * 2), riderY - 18, riderX + 7 + (i * 2), riderY - 15);
    }
    
    // Huvud (mer detaljerat)
    g.FillEllipse(new SolidBrush(skinColor), riderX + 5, riderY - 48, 14, 14);
  
    // Öra
  if (!IsPlayer)
     g.FillEllipse(new SolidBrush(darkSkin), riderX + 6, riderY - 44, 3, 5);
    else
      g.FillEllipse(new SolidBrush(darkSkin), riderX + 15, riderY - 44, 3, 5);
    
// Ögonbryn - FIXAD!
    int eyeBrowX = riderX + (IsPlayer ? 14 : 8);
    int eyeBrowY = riderY - 45;  // Använd en separat Y-variabel
 g.DrawLine(new Pen(Color.FromArgb(alpha, Color.Brown), 1), eyeBrowX - 1, eyeBrowY, eyeBrowX + 2, eyeBrowY);
    
    // Öga med detaljer
    int riderEyeX = riderX + (IsPlayer ? 15 : 8);
    g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.White)), riderEyeX, riderY - 43, 4, 4);
    g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Black)), riderEyeX + 1, riderY - 42, 2, 2);
    // Pupill
    g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Black)), riderEyeX + 1, riderY - 42, 1, 2);
 
    // Näsa
    Point[] nose = new Point[]
    {
 new Point(riderX + (IsPlayer ? 18 : 6), riderY - 41),
  new Point(riderX + (IsPlayer ? 19 : 5), riderY - 39),
        new Point(riderX + (IsPlayer ? 18 : 6), riderY - 37)
};
    g.FillPolygon(new SolidBrush(darkSkin), nose);
    // Näsborre
  g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Black)), riderX + (IsPlayer ? 18 : 6), riderY - 39, 1, 1);
    
  // Mun
    g.DrawArc(new Pen(Color.FromArgb(alpha, 139, 69, 19), 1), riderX + (IsPlayer ? 11 : 6), riderY - 38, 6, 3, 0, 180);
  
    // Skägg (krigar-stil)
 g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Brown)), riderX + (IsPlayer ? 11 : 7), riderY - 37, 5, 4);
    g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Brown)), riderX + (IsPlayer ? 13 : 6), riderY - 35, 4, 3);
    
    // Hår (mer detaljerat)
Color hairColor = Color.FromArgb(alpha, Color.Brown);
    Color darkHair = Color.FromArgb(alpha, 80, 50, 20);
    
    // Flera hårlockar
    g.FillEllipse(new SolidBrush(hairColor), riderX + 5, riderY - 50, 5, 5);
    g.FillEllipse(new SolidBrush(darkHair), riderX + 6, riderY - 49, 2, 3);

    g.FillEllipse(new SolidBrush(hairColor), riderX + 9, riderY - 52, 5, 5);
    g.FillEllipse(new SolidBrush(darkHair), riderX + 10, riderY - 51, 2, 3);
  
g.FillEllipse(new SolidBrush(hairColor), riderX + 13, riderY - 50, 5, 5);
    g.FillEllipse(new SolidBrush(darkHair), riderX + 14, riderY - 49, 2, 3);
 
    // Hjälm/huvudbonad av läder
    g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.SaddleBrown)), riderX + 7, riderY - 51, 10, 6);

  // Ryttarens främre ben
  g.FillRectangle(new SolidBrush(skinColor), riderX + 20, riderY + 10, 5, 6);  // Ännu kortare ben
    g.DrawLine(new Pen(lightSkin, 1), riderX + 22, riderY + 11, riderX + 22, riderY + 14);
    // Främre fot i stigbygel
 g.DrawLine(new Pen(Color.FromArgb(alpha, Color.SaddleBrown), 2), riderX + 22, riderY + 14, riderX + 22, riderY + 17);
    g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, Color.SaddleBrown)), riderX + 20, riderY + 15, 4, 3);
 
    // Arm och stor klubba med attack animation - INTE när man dör
    if (!IsDying)
    {
        int armSwingOffset = 0;
        int armSwingY = 0;
 if (isAttacking)
    {
        if (attackAnimationTimer < 100)
        {
   armSwingOffset = -10 * direction;
        armSwingY = -8;
    }
        else if (attackAnimationTimer < 200)
  {
      armSwingOffset = 15 * direction;
    armSwingY = 3;
  }
        else
        {
     armSwingOffset = 5 * direction;
 armSwingY = 0;
    }
}
    
    int armX = riderX + (IsPlayer ? 18 : 5);
    
    // Axel
    g.FillEllipse(new SolidBrush(lightSkin), armX - 2, riderY - 35, 6, 6);
    
 // Överarm med muskler
    g.FillRectangle(new SolidBrush(skinColor), armX - (armSwingOffset / 2), riderY - 32 + (armSwingY / 2), 5, 10);
    g.DrawLine(new Pen(lightSkin, 1), armX - (armSwingOffset / 2) + 3, riderY - 30 + (armSwingY / 2), armX - (armSwingOffset / 2) + 3, riderY - 24 + (armSwingY / 2));
    // Biceps bulge
    g.FillEllipse(new SolidBrush(lightSkin), armX - (armSwingOffset / 2) + 1, riderY - 29 + (armSwingY / 2), 3, 4);
  
    // Armbåge
    g.FillEllipse(new SolidBrush(darkSkin), armX - (armSwingOffset / 2) + 1, riderY - 24 + (armSwingY / 2), 3, 3);
    
// Underarm
    g.FillRectangle(new SolidBrush(skinColor), armX - armSwingOffset, riderY - 24 + armSwingY, 5, 10);
    
    // Hand som griper vapnet
    g.FillEllipse(new SolidBrush(skinColor), armX - armSwingOffset - 1, riderY - 16 + armSwingY, 5, 4);
  // Fingrar
  g.FillRectangle(new SolidBrush(darkSkin), armX - armSwingOffset + 1, riderY - 15 + armSwingY, 2, 2);
  
    // Stor krigar-klubba
    int clubX = armX - armSwingOffset + (direction * 2);
    int clubY = riderY - 30 + armSwingY;  // Ändrat från riderY - 16 till riderY - 30 så klubban är högre upp
    
    // Klubbskaft med detaljer
 g.FillRectangle(new SolidBrush(Color.FromArgb(alpha, Color.SaddleBrown)), clubX, clubY, 5, 22);
    // Grepp-linjer
    for (int i = 0; i < 4; i++)
    {
   g.DrawLine(new Pen(Color.FromArgb(alpha, 101, 67, 33), 1), clubX, clubY + 5 + (i * 4), clubX + 5, clubY + 5 + (i * 4));
    }
    
 // Stor klubbhuvud med taggar
  g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.DarkGray)), clubX - 4, clubY - 9, 13, 13);
 g.FillEllipse(new SolidBrush(Color.FromArgb(alpha, Color.Gray)), clubX - 2, clubY - 7, 9, 9);
    // Taggar på klubban
    for (int i = 0; i < 4; i++)
    {
        Point[] spike = new Point[]
    {
  new Point(clubX + 2, clubY - 9 + (i * 3)),
    new Point(clubX - 1, clubY - 7 + (i * 3)),
new Point(clubX + 1, clubY - 6 + (i * 3))
   };
     g.FillPolygon(new SolidBrush(Color.FromArgb(alpha, Color.Black)), spike);
    }
    }
    
    // HP-bar - INTE när man dör
    if (!IsDying && MaxHP > 0)
    {
  g.FillRectangle(Brushes.DarkRed, Hitbox.X, Hitbox.Y - 10, Hitbox.Width, 5);
      g.FillRectangle(Brushes.Green, Hitbox.X, Hitbox.Y - 10, (int)(Hitbox.Width * ((float)HP / MaxHP)), 5);
    }
}
    }
}
