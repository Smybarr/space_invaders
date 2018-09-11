using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class DebugPrinter
    {
        static public void PrintAzulColor(Azul.Color pColor)
        {
            Debug.WriteLine("  Color(r, g, b, a): {0} {1} {2} {3}", pColor.red, pColor.green, pColor.blue, pColor.alpha);
        }

        static public void PrintAzulRect(Azul.Rect pRect)
        {
            Debug.WriteLine("   Rect(x, y, w, h): {0} {1} {2} {3}", pRect.x, pRect.y, pRect.width, pRect.height);
        }
    }
}