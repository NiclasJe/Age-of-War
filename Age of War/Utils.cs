using System.Drawing;

namespace Age_of_War
{
    public static class Utils
    {
   // Returnerar true om två rektanglar överlappar
    public static bool Intersects(Rectangle a, Rectangle b)
   {
   return a.IntersectsWith(b);
        }
    }
}
