using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class UndoButton : Button 
    {
        // --- CONSTRUCTOR ---
        public UndoButton(string label, double x, double y, int size, Font font, float width, float height, Color labelClr, Color backClr) : base(label, x, y, size, font, width, height, labelClr, backClr) { }

        // --- METHODS ---
        public override Boolean Activate()
        {
            Caretaker.GetInstance().Undo();
            return true;
        }
    }
}
