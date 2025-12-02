using System.Windows.Forms;

namespace Age_of_War
{
    // Anpassad panel med dubbelbufrring för att undvika flimmer
    public class DoubleBufferedPanel : Panel
  {
        public DoubleBufferedPanel()
   {
       this.DoubleBuffered = true;
        this.SetStyle(ControlStyles.AllPaintingInWmPaint | 
        ControlStyles.UserPaint | 
         ControlStyles.OptimizedDoubleBuffer, true);
  this.UpdateStyles();
        }
    }
}
