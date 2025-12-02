using System;
using System.Drawing;
using System.Windows.Forms;

namespace Age_of_War
{
    public partial class Form1 : Form
    {
        private Game game;
        private System.Windows.Forms.Timer gameTimer;
        private int lastUpdateTime;
        private DoubleBufferedPanel battlefieldPanel;
        private UnitButton btnSword, btnRanged, btnCavalry;
        private UnitButton btnToggleTurret;
        private UnitButton btnBasicTurret, btnAdvancedTurret, btnPremiumTurret;
        private UnitButton btnSellTurret;  // Knapp för att sälja turret
        private UnitButton btnExpandBase;  // Knapp för att bygga ut basen
        private UnitButton btnToggleSoldiers;
        private bool showingTurrets = false;
        private Type selectedTurretType = null;  // Vilken turret som är vald för placering
private int selectedTurretCost = 0;
private bool sellingTurret = false;  // Om vi är i säljläge
private Turret previewTurret = null;  // För att visa transparent turret vid musen
private Point mousePosition;  // Håller koll på musens position
private Label lblGold, lblBaseHP, lblEnemyBaseHP;
private Button btnPause;
private Button btnUltimate;  // Ultimate attack knapp
private Panel pausePanel;
private Button btnResume;
private Button btnExit;
private bool isPaused = false;

public Form1()
{
    InitializeComponent();
    this.WindowState = FormWindowState.Normal;  // Ändrat från Maximized
    this.FormBorderStyle = FormBorderStyle.Sizable;  // Ändrat från None
    this.Size = new Size(1280, 720);  // Standardstorlek
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Resize += (s, e) => ResizeBattlefield();

    game = new Game(this.ClientSize.Width, this.ClientSize.Height);

    // Battlefield panel med dubbelbufrring
    battlefieldPanel = new DoubleBufferedPanel
    {
        Location = new Point(0,0),
        Size = new Size(this.ClientSize.Width, this.ClientSize.Height),
        BackColor = Color.Transparent,
        Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
    };
    
    battlefieldPanel.Paint += BattlefieldPanel_Paint;
battlefieldPanel.MouseClick += BattlefieldPanel_MouseClick;
battlefieldPanel.MouseMove += BattlefieldPanel_MouseMove;  // Lägg till mouse-move hantering
    this.Controls.Add(battlefieldPanel);

    // Pause button
    btnPause = new Button { Text = "Pausa", Size = new Size(100,40), Location = new Point(this.ClientSize.Width -120,20), Anchor = AnchorStyles.Top | AnchorStyles.Right };
    btnPause.Click += BtnPause_Click;
    this.Controls.Add(btnPause);
    btnPause.BringToFront();

    // Ultimate attack button
    btnUltimate = new Button 
    { 
        Text = "",  // Tom text, vi ritar egen ikon
        Size = new Size(80, 80),
        Location = new Point(this.ClientSize.Width - 120, 70),
        Anchor = AnchorStyles.Top | AnchorStyles.Right,
        BackColor = Color.FromArgb(139, 69, 19)
};
btnUltimate.Click += BtnUltimate_Click;
btnUltimate.Paint += BtnUltimate_Paint;
this.Controls.Add(btnUltimate);
btnUltimate.BringToFront();

    // Pause panel
    pausePanel = new Panel
    {
        Size = new Size(300,200),
        Location = new Point((this.ClientSize.Width -300) /2, (this.ClientSize.Height -200) /2),
        BackColor = Color.FromArgb(200,50,50,50),
        Visible = false
    };
    btnResume = new Button { Text = "Fortsätt", Size = new Size(150,40), Location = new Point(75,40) };
    btnResume.Click += BtnResume_Click;
    btnExit = new Button { Text = "Avsluta", Size = new Size(150,40), Location = new Point(75,110) };
    btnExit.Click += BtnExit_Click;
    pausePanel.Controls.Add(btnResume);
    pausePanel.Controls.Add(btnExit);
    this.Controls.Add(pausePanel);
    pausePanel.BringToFront();

    // UI-knappar med bilder av soldater
    btnSword = new UnitButton(typeof(Soldier), 50) { Location = new Point(20, 20) };
    btnRanged = new UnitButton(typeof(RangedUnit), 80) { Location = new Point(130, 20) };
    btnCavalry = new UnitButton(typeof(CavalryUnit), 200) { Location = new Point(240, 20) };
    btnToggleTurret = new UnitButton(typeof(BasicTurret), 0) { Location = new Point(350, 20) };  // Visar BasicTurret

    btnSword.Click += (s, e) => SpawnUnit(typeof(Soldier));
    btnRanged.Click += (s, e) => SpawnUnit(typeof(RangedUnit));
    btnCavalry.Click += (s, e) => SpawnUnit(typeof(CavalryUnit));
    btnToggleTurret.Click += (s, e) => ToggleToTurrets();

    this.Controls.Add(btnSword);
    this.Controls.Add(btnRanged);
    this.Controls.Add(btnCavalry);
    this.Controls.Add(btnToggleTurret);
    btnSword.BringToFront();
    btnRanged.BringToFront();
    btnCavalry.BringToFront();
    btnToggleTurret.BringToFront();

    // Turret-knappar (dolda initialt)
    btnBasicTurret = new UnitButton(typeof(BasicTurret), 500) { Location = new Point(20, 20), Visible = false };
    btnAdvancedTurret = new UnitButton(typeof(AdvancedTurret), 1000) { Location = new Point(130, 20), Visible = false };
    btnPremiumTurret = new UnitButton(typeof(PremiumTurret), 1500) { Location = new Point(240, 20), Visible = false };
    btnSellTurret = new UnitButton(null, 0, true) { Location = new Point(350, 20), Visible = false };  // Sell-knapp
    btnExpandBase = new UnitButton(null, 0, false, true) { Location = new Point(460, 20), Visible = false };  // Expand-knapp
    btnToggleSoldiers = new UnitButton(typeof(Soldier), 0) { Location = new Point(570, 20), Visible = false };  // Flyttat längre till höger

    btnBasicTurret.Click += (s, e) => SelectTurret(typeof(BasicTurret), 500);
    btnAdvancedTurret.Click += (s, e) => SelectTurret(typeof(AdvancedTurret), 1000);
    btnPremiumTurret.Click += (s, e) => SelectTurret(typeof(PremiumTurret), 1500);
    btnSellTurret.Click += (s, e) => ActivateSelling();
    btnExpandBase.Click += (s, e) => ExpandBase();
    btnToggleSoldiers.Click += (s, e) => ToggleToSoldiers();

    this.Controls.Add(btnBasicTurret);
    this.Controls.Add(btnAdvancedTurret);
    this.Controls.Add(btnPremiumTurret);
    this.Controls.Add(btnSellTurret);
    this.Controls.Add(btnExpandBase);
    this.Controls.Add(btnToggleSoldiers);
    btnBasicTurret.BringToFront();
    btnAdvancedTurret.BringToFront();
    btnPremiumTurret.BringToFront();
    btnSellTurret.BringToFront();
    btnExpandBase.BringToFront();
    btnToggleSoldiers.BringToFront();

    // Labels
    lblGold = new Label { Location = new Point(20, 150), Size = new Size(200, 30), Font = new Font("Arial", 14, FontStyle.Bold), BackColor = Color.FromArgb(240, 230, 210) };
lblBaseHP = new Label { Location = new Point(20, 185), Size = new Size(200, 30), Font = new Font("Arial", 12), BackColor = Color.FromArgb(240, 230, 210) };
lblEnemyBaseHP = new Label { Location = new Point(this.ClientSize.Width - 220, 185), Size = new Size(200, 30), Font = new Font("Arial", 12), TextAlign = ContentAlignment.TopRight, BackColor = Color.FromArgb(240, 230, 210) };
    this.Controls.Add(lblGold);
    this.Controls.Add(lblBaseHP);
    this.Controls.Add(lblEnemyBaseHP);
    lblGold.BringToFront();
    lblBaseHP.BringToFront();
    lblEnemyBaseHP.BringToFront();

    // Game loop
    gameTimer = new System.Windows.Forms.Timer { Interval =33 }; // ~30 FPS
    gameTimer.Tick += GameTimer_Tick;
    gameTimer.Start();
    lastUpdateTime = Environment.TickCount;
}

private void ResizeBattlefield()
{
    battlefieldPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height);
    btnPause.Location = new Point(this.ClientSize.Width - 120, 20);
    btnUltimate.Location = new Point(this.ClientSize.Width - 130, 70);  // Justerad för större knapp
  pausePanel.Location = new Point((this.ClientSize.Width - pausePanel.Width) / 2, (this.ClientSize.Height - pausePanel.Height) / 2);
    lblEnemyBaseHP.Location = new Point(this.ClientSize.Width - 220, 185);

    // Uppdatera spelets dimensioner
    if (game != null)
    {
     game.UpdateDimensions(this.ClientSize.Width, this.ClientSize.Height);
    }
}

private void SpawnUnit(Type unitType)
{
    int cost = unitType == typeof(Soldier) ? 50 : unitType == typeof(RangedUnit) ? 80 : 200;  // Ändrat från 120 till 200
    
    // Kontrollera om kön är full
    if (game.GetPlayerQueueCount() >= 5)
    {
        return; // Gör ingenting om kön är full
    }
    
    if (game.PlayerGold >= cost)
    {
        game.PlayerGold -= cost;
        game.QueueUnitSpawn(unitType, true);
    }
}

private void BtnPause_Click(object sender, EventArgs e)
{
    isPaused = true;
    gameTimer.Stop();
    pausePanel.Visible = true;
}

private void BtnResume_Click(object sender, EventArgs e)
{
    isPaused = false;
    gameTimer.Start();
    pausePanel.Visible = false;
}

private void BtnExit_Click(object sender, EventArgs e)
{
    Application.Exit();
}

private void BtnUltimate_Click(object sender, EventArgs e)
{
    if (game.UltimateAvailable)
    {
  game.ActivateUltimate();
    }
}

private void BtnUltimate_Paint(object sender, PaintEventArgs e)
{
    var btn = (Button)sender;
var g = e.Graphics;
    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
  
    // Rita fallande stenar ikon - Större stenar
    // Sten 1 (stor)
    Point[] rock1 = new Point[]
    {
   new Point(20, 20),  // Ökade storleken
        new Point(27, 16),
    new Point(34, 20),
 new Point(32, 30),
        new Point(22, 30)
    };
    using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
        new Rectangle(20, 16, 14, 14),
      Color.Gray,
     Color.DarkGray,
 45f))
    {
        g.FillPolygon(brush, rock1);
  g.DrawPolygon(Pens.Black, rock1);
    }
    
    // Sten 2 (mellan)
    Point[] rock2 = new Point[]
    {
        new Point(42, 28),
        new Point(48, 25),
        new Point(54, 29),
        new Point(51, 37),
        new Point(44, 36)
    };
    using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
        new Rectangle(42, 25, 12, 12),
        Color.LightGray,
        Color.Gray,
        45f))
    {
     g.FillPolygon(brush, rock2);
        g.DrawPolygon(Pens.Black, rock2);
    }
 
    // Sten 3 (liten)
    Point[] rock3 = new Point[]
    {
        new Point(30, 42),
        new Point(35, 39),
      new Point(40, 43),
        new Point(37, 49),
        new Point(32, 48)
    };
    using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
  new Rectangle(30, 39, 10, 10),
        Color.DarkGray,
        Color.FromArgb(60, 60, 60),
     45f))
    {
    g.FillPolygon(brush, rock3);
        g.DrawPolygon(Pens.Black, rock3);
    }
    
  // Rita cooldown overlay om inte tillgänglig
    if (!game.UltimateAvailable)
    {
        float progress = game.GetUltimateCooldownProgress();
        int overlayHeight = (int)((1f - progress) * btn.Height);
        
        using (var overlayBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
      {
      g.FillRectangle(overlayBrush, 0, 0, btn.Width, overlayHeight);
        }
 
        // Rita cooldown-text
        int secondsLeft = game.UltimateCooldown / 1000 + 1;
        using (var font = new Font("Arial", 18, FontStyle.Bold))  // Ökad från 14
  {
         var text = secondsLeft.ToString() + "s";
var textSize = g.MeasureString(text, font);
   g.DrawString(text, font, Brushes.White, 
                (btn.Width - textSize.Width) / 2, 
                (btn.Height - textSize.Height) / 2);
        }
    }
    else
    {
   // Rita "Redo!" text när tillgänglig
   using (var font = new Font("Arial", 12, FontStyle.Bold))  // Ökad från 8
    {
     var text = "Redo!";  // Ändrat från "KLAR!"
     var textSize = g.MeasureString(text, font);
     g.DrawString(text, font, Brushes.Yellow, 
       (btn.Width - textSize.Width) / 2, 
  btn.Height - 18);
   }
    }
}
private void ToggleToTurrets()
{
    showingTurrets = true;
    
    // Dölj soldat-knappar
    btnSword.Visible = false;
    btnRanged.Visible = false;
    btnCavalry.Visible = false;
    btnToggleTurret.Visible = false;
    
    // Visa turret-knappar
    btnBasicTurret.Visible = true;
    btnAdvancedTurret.Visible = true;
    btnPremiumTurret.Visible = true;
    btnSellTurret.Visible = true;
    btnExpandBase.Visible = true;  // Visa expand-knapp
    btnToggleSoldiers.Visible = true;
}

private void ToggleToSoldiers()
{
    showingTurrets = false;
    selectedTurretType = null;
    selectedTurretCost = 0;
    previewTurret = null;
    sellingTurret = false;
    game.DeactivateTurretPlacement();
    game.DeactivateTurretSelling();
 
    // Visa soldat-knappar
    btnSword.Visible = true;
    btnRanged.Visible = true;
    btnCavalry.Visible = true;
    btnToggleTurret.Visible = true;
 
    // Dölj turret-knappar
    btnBasicTurret.Visible = false;
    btnAdvancedTurret.Visible = false;
    btnPremiumTurret.Visible = false;
    btnSellTurret.Visible = false;
    btnExpandBase.Visible = false;  // Dölj expand-knapp
    btnToggleSoldiers.Visible = false;
}

private void ExpandBase()
{
    if (game.ExpandBase())
    {
    // Utbyggnad lyckades
        battlefieldPanel.Invalidate();
    }
}

private void ActivateSelling()
{
  if (game.PlayerTurrets.Count == 0)
      return;  // Inga turrets att sälja

    // Avbryt placeringsläge om det är aktivt
    selectedTurretType = null;
    selectedTurretCost = 0;
    previewTurret = null;
    game.DeactivateTurretPlacement();
    
    // Aktivera säljläge
    sellingTurret = true;
    game.ActivateTurretSelling();
}

private void SelectTurret(Type turretType, int cost)
{
    if (game.PlayerGold < cost)
  return;  // Inte tillräckligt med guld
 
    // Avbryt säljläge
    sellingTurret = false;
    game.DeactivateTurretSelling();
    
    selectedTurretType = turretType;
    selectedTurretCost = cost;
  game.ActivateTurretPlacement();
  
    // Skapa en preview-turret för att visa vid musen
    if (turretType == typeof(BasicTurret))
        previewTurret = new BasicTurret(true, 0, 0);
    else if (turretType == typeof(AdvancedTurret))
        previewTurret = new AdvancedTurret(true, 0, 0);
    else if (turretType == typeof(PremiumTurret))
     previewTurret = new PremiumTurret(true, 0, 0);
}

private void BattlefieldPanel_MouseMove(object sender, MouseEventArgs e)
{
  mousePosition = e.Location;
    
    // Uppdatera preview-turretens position
    if (previewTurret != null)
    {
  previewTurret.PositionX = e.X;
        previewTurret.PositionY = e.Y;
        battlefieldPanel.Invalidate();  // Rita om för att visa preview
    }
}

private void GameTimer_Tick(object sender, EventArgs e)
{
    if (isPaused) return;

    int now = Environment.TickCount;
    int delta = now - lastUpdateTime;
    lastUpdateTime = now;

    game.UpdateEconomy(delta);
game.UpdateSpawnQueues(delta);
    game.UpdateEnemySpawn();
    game.UpdateUnits(delta);
    game.UpdateUltimate(delta);  // Uppdatera ultimate attack

    // Uppdatera UI
    lblGold.Text = $"Guld: {game.PlayerGold}";
    lblBaseHP.Text = $"Bas HP: {game.PlayerBase.HP}";
    lblEnemyBaseHP.Text = $"Fiende Bas HP: {game.EnemyBase.HP}";
    
    // Uppdatera ultimate-knappen för cooldown-animation
    btnUltimate.Invalidate();

    // Kontrollera vinst/förlust
    if (game.PlayerBase.HP <= 0 || game.EnemyBase.HP <= 0)
    {
        gameTimer.Stop();
 MessageBox.Show(game.PlayerBase.HP <= 0 ? "Du förlorade!" : "Du vann!");
        Application.Exit();
    }

    battlefieldPanel.Invalidate();
}

private void BattlefieldPanel_MouseClick(object sender, MouseEventArgs e)
{
    if (sellingTurret)
    {
      // Kontrollera om man klickar på någon turret
   if (game.SellTurretAt(e.Location))
        {
      sellingTurret = false;
    battlefieldPanel.Invalidate();
       return;
        }
        else
 {
   // Klickat utanför - avbryt
   sellingTurret = false;
  game.DeactivateTurretSelling();
   battlefieldPanel.Invalidate();
     return;
 }
    }
    
  if (selectedTurretType != null && game.ShowTurretSlot)
    {
  // Kolla om klicket är inom någon turret-slot
  bool placed = false;
    for (int i = 0; i < game.TurretSlots.Count; i++)
    {
        var slot = game.TurretSlots[i];
        if (slot.Contains(e.Location))
  {
   if (game.PlaceTurret(selectedTurretType, selectedTurretCost, i))  // Skicka med slot-index
     {
       selectedTurretType = null;
         selectedTurretCost = 0;
 previewTurret = null;
             placed = true;
          break;
    }
 }
    }
    
 if (!placed)
    {
   // Klickat utanför slot - avbryt
    selectedTurretType = null;
   selectedTurretCost = 0;
 previewTurret = null;
        game.DeactivateTurretPlacement();
    }
    
    battlefieldPanel.Invalidate();
}
}

private void BattlefieldPanel_Paint(object sender, PaintEventArgs e)
{
    var g = e.Graphics;
    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    
    // Himmel med gradient
    using (var skyBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
        new Rectangle(0, 0, this.ClientSize.Width, game.BattlefieldY),
        Color.FromArgb(135, 206, 235),
        Color.FromArgb(70, 130, 180),
  System.Drawing.Drawing2D.LinearGradientMode.Vertical))
    {
        g.FillRectangle(skyBrush, 0, 0, this.ClientSize.Width, game.BattlefieldY);
    }
    
    // Rita moln
 DrawClouds(g);
    
    // Bakgrundskullar
    Color distantHillColor = Color.FromArgb(100, 120, 100);
    DrawHill(g, -100, game.BattlefieldY - 150, 400, 150, distantHillColor);
    DrawHill(g, 300, game.BattlefieldY - 180, 500, 180, Color.FromArgb(90, 110, 90));
    DrawHill(g, 700, game.BattlefieldY - 160, 450, 160, distantHillColor);
    DrawHill(g, this.ClientSize.Width - 400, game.BattlefieldY - 170, 500, 170, Color.FromArgb(95, 115, 95));
    
    // Mellangrunds träd och kullar
    Color midHillColor = Color.FromArgb(80, 140, 60);
    DrawHill(g, -50, game.BattlefieldY - 100, 350, 100, midHillColor);
    DrawForest(g, 200, game.BattlefieldY - 60, 5);
    DrawHill(g, 400, game.BattlefieldY - 120, 400, 120, Color.FromArgb(75, 130, 55));
  DrawForest(g, 800, game.BattlefieldY - 60, 4);
    DrawHill(g, this.ClientSize.Width - 350, game.BattlefieldY - 110, 400, 110, midHillColor);
    
    // Mark
    g.FillRectangle(Brushes.SandyBrown, 0, game.BattlefieldY, this.ClientSize.Width, 80);
    
 // Gräs på marken
    using (var grassBrush1 = new SolidBrush(Color.FromArgb(50, 150, 50)))
    using (var grassBrush2 = new SolidBrush(Color.FromArgb(40, 130, 40)))
  {
      g.FillRectangle(grassBrush1, 0, game.BattlefieldY, this.ClientSize.Width, 12);
        g.FillRectangle(grassBrush2, 0, game.BattlefieldY + 10, this.ClientSize.Width, 5);
    }

 DrawGrass(g);
 DrawRocks(g);

    // Baser
    game.PlayerBase.Draw(g);
    game.EnemyBase.Draw(g);

    // Rita utbyggda torn på basen
    for (int i = 0; i < game.BaseExpansions; i++)
    {
        int towerX = game.PlayerBase.Hitbox.X + 25;  // Centrerat på basen (samma som slot)
        int towerY = game.PlayerBase.Hitbox.Y - 40 - (i * 50);  // Samma spacing som slots
        int towerHeight = 45;  // Fast höjd för alla torn
        int towerWidth = 40;   // Bredare för att se bättre
        
        // Torn-kropp
        using (var towerBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
 new Rectangle(towerX, towerY, towerWidth, towerHeight),
      Color.FromArgb(100, 100, 100),
  Color.FromArgb(60, 60, 60),
        System.Drawing.Drawing2D.LinearGradientMode.Vertical))
    {
  g.FillRectangle(towerBrush, towerX, towerY, towerWidth, towerHeight);
    }
    g.DrawRectangle(new Pen(Color.Black, 2), towerX, towerY, towerWidth, towerHeight);
    
    // Torn-detaljer (fönster)
    g.FillRectangle(Brushes.DarkGoldenrod, towerX + 8, towerY + 10, 6, 8);
    g.FillRectangle(Brushes.DarkGoldenrod, towerX + 26, towerY + 10, 6, 8);
    g.FillRectangle(Brushes.DarkGoldenrod, towerX + 17, towerY + 25, 6, 8);
}

    // Rita turret-slots
    if (game.ShowTurretSlot)
    {
        foreach (var slot in game.TurretSlots)
        {
          using (var slotBrush = new SolidBrush(Color.FromArgb(150, 0, 255, 0)))
      {
 g.FillRectangle(slotBrush, slot);
            }
g.DrawRectangle(new Pen(Color.LimeGreen, 3), slot);
        }
        
      if (game.TurretSlots.Count > 0)
        {
   using (var font = new Font("Arial", 10, FontStyle.Bold))
            {
              g.DrawString("Klicka här!", font, Brushes.White, 
           game.TurretSlots[0].X + 5, game.TurretSlots[0].Y - 20);
        }
        }
    }

    // Rita alla turrets
    foreach (var turret in game.PlayerTurrets)
    {
        turret.Draw(g);
        
        if (game.ShowTurretSellHighlight)
        {
            using (var redPen = new Pen(Color.Red, 3))
      {
     var hitbox = turret.Hitbox;
      g.DrawRectangle(redPen, hitbox.X - 3, hitbox.Y - 3, hitbox.Width + 6, hitbox.Height + 6);
            }
    
    int blinkFrame = (Environment.TickCount / 200) % 2;
         if (blinkFrame == 0)
  {
          using (var semiRedBrush = new SolidBrush(Color.FromArgb(80, 255, 0, 0)))
    {
        var hitbox = turret.Hitbox;
     g.FillRectangle(semiRedBrush, hitbox.X - 3, hitbox.Y - 3, hitbox.Width + 6, hitbox.Height + 6);
                }
  }
            
   using (var font = new Font("Arial", 9, FontStyle.Bold))
            {
 int refund = turret.Cost / 2;
                string text = $"Sälj ({refund}g)";
        var textSize = g.MeasureString(text, font);
g.DrawString(text, font, Brushes.Yellow, 
        turret.PositionX - textSize.Width / 2, 
   turret.PositionY - 50);
          }
        }
    }

    // Rita transparent preview-turret
    if (previewTurret != null && selectedTurretType != null)
    {
        var originalState = g.Save();
        
        var colorMatrix = new System.Drawing.Imaging.ColorMatrix();
        colorMatrix.Matrix33 = 0.5f;
        
      var imageAttributes = new System.Drawing.Imaging.ImageAttributes();
    imageAttributes.SetColorMatrix(colorMatrix, 
      System.Drawing.Imaging.ColorMatrixFlag.Default,
     System.Drawing.Imaging.ColorAdjustType.Bitmap);
        
    using (var tempBitmap = new Bitmap(60, 60))
        {
            using (var tempG = Graphics.FromImage(tempBitmap))
  {
     tempG.Clear(Color.Transparent);
              tempG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    
                var tempTurret = previewTurret;
           float oldX = tempTurret.PositionX;
           int oldY = tempTurret.PositionY;

        tempTurret.PositionX = 30;
     tempTurret.PositionY = 50;
           tempTurret.Draw(tempG);
    
   tempTurret.PositionX = oldX;
  tempTurret.PositionY = oldY;
    }
    
     g.DrawImage(tempBitmap, 
       new Rectangle(mousePosition.X - 30, mousePosition.Y - 50, 60, 60),
      0, 0, 60, 60,
    GraphicsUnit.Pixel,
             imageAttributes);
  }
      
     g.Restore(originalState);
        
        using (var font = new Font("Arial", 9, FontStyle.Bold))
        {
            g.DrawString("Klicka på grön ruta för att placera\nKlicka utanför för att avbryta", 
      font, Brushes.Yellow, mousePosition.X - 70, mousePosition.Y + 20);
     }
    }

    // Rita spawn-progress
    float progress = game.GetPlayerSpawnProgress();
    if (progress > 0)
    {
        int barWidth = 150;
        int barHeight = 20;
        int barX = game.PlayerBase.Hitbox.Right + 20;
        int barY = game.BattlefieldY - 180;
      
        g.FillRectangle(Brushes.DarkGray, barX, barY, barWidth, barHeight);
        g.FillRectangle(Brushes.LightGreen, barX, barY, (int)(barWidth * progress), barHeight);
        g.DrawRectangle(Pens.Black, barX, barY, barWidth, barHeight);
        
   g.DrawString($"Spawnar... {(int)(progress * 100)}%", 
            new Font("Arial", 10), Brushes.White, barX, barY - 20);
    }

    int queueCount = game.GetPlayerQueueCount();
    if (queueCount > 1)
    {
  g.DrawString($"I kö: {queueCount - 1}", 
            new Font("Arial", 12, FontStyle.Bold), Brushes.Yellow, 
            game.PlayerBase.Hitbox.Right + 20, game.BattlefieldY - 210);
    }

    // Enheter
    foreach (var unit in game.PlayerUnits)
        unit.Draw(g);
    foreach (var unit in game.EnemyUnits)
        unit.Draw(g);

    // Projektiler
    foreach (var projectile in game.Projectiles)
        projectile.Draw(g);

    // Fallande stenar (ultimate attack)
    foreach (var rock in game.FallingRocks)
        rock.Draw(g);
}

// Hjälpmetoder för att rita miljö
private void DrawHill(Graphics g, int x, int y, int width, int height, Color color)
{
    using (var brush = new SolidBrush(color))
    {
        g.FillEllipse(brush, x, y, width, height * 2);
 }
}

private void DrawClouds(Graphics g)
{
    Color cloudColor = Color.FromArgb(200, 255, 255, 255);
    DrawCloud(g, 100, 50, cloudColor);
    DrawCloud(g, 400, 80, cloudColor);
    DrawCloud(g, 800, 40, cloudColor);
    DrawCloud(g, this.ClientSize.Width - 200, 70, cloudColor);
}

private void DrawCloud(Graphics g, int x, int y, Color color)
{
    using (var brush = new SolidBrush(color))
    {
g.FillEllipse(brush, x, y, 60, 30);
        g.FillEllipse(brush, x + 20, y - 10, 50, 35);
        g.FillEllipse(brush, x + 40, y, 55, 30);
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

private void DrawGrass(Graphics g)
{
    Color grassColor = Color.FromArgb(60, 180, 60);
    using (var grassPen = new Pen(grassColor, 2))
    {
        for (int i = 0; i < this.ClientSize.Width; i += 20)
     {
  int grassX = i + (i % 3 * 5);
            int grassY = game.BattlefieldY + 15;
            g.DrawLine(grassPen, grassX, grassY, grassX + 2, grassY - 8);
     g.DrawLine(grassPen, grassX + 5, grassY, grassX + 3, grassY - 10);
   g.DrawLine(grassPen, grassX + 10, grassY, grassX + 12, grassY - 7);
    }
    }
}

private void DrawRocks(Graphics g)
{
    Color rockColor = Color.FromArgb(120, 120, 120);
    DrawRock(g, 150, game.BattlefieldY + 20, 15, rockColor);
    DrawRock(g, 350, game.BattlefieldY + 25, 12, rockColor);
    DrawRock(g, 600, game.BattlefieldY + 22, 18, rockColor);
    DrawRock(g, this.ClientSize.Width - 300, game.BattlefieldY + 24, 14, rockColor);
}

private void DrawRock(Graphics g, int x, int y, int size, Color color)
{
    using (var brush = new SolidBrush(color))
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
    }
}
    }
}
