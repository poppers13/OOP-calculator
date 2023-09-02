using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class ModeButton : Button
    {
        // --- FIELDS ---
        private CalculationStrategy _mode;

        // --- CONSTRUCTOR ---
        public ModeButton(CalculationStrategy mode, string label, double x, double y, int size, Font font, float width, float height, Color labelClr, Color backClr) : base(label, x, y, size, font, width, height, labelClr, backClr)
        {
            _mode = mode;
        }

        // --- METHODS ---
        // if the most recent operation is empty, then change its strategy
        // otherwise, create a new strategy
        public override Boolean Activate()
        {
            Calculator calc = Calculator.GetInstance();

            if (calc.LastOperation.HasDigits)
            {
                calc.AddOperation(new Operation(_mode));
                return true;
            }
            else
            {
                return calc.LastOperation.ChangeStrategy(_mode);
            }
        }
    }
}
