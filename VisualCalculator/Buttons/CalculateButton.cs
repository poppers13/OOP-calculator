using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class CalculateButton : Button 
    {
        // --- CONSTRUCTOR ---
        public CalculateButton(string label, double x, double y, int size, Font font, float width, float height, Color labelClr, Color backClr) : base(label, x, y, size, font, width, height, labelClr, backClr) { }

        // --- METHODS ---
        public override Boolean Activate()
        {
            Calculator calc = Calculator.GetInstance();
            Boolean success = calc.Calculate();
            return success;
        }
    }
}
