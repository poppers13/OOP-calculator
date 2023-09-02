using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class NumberButton : Button
    {
        // --- CONSTRUCTOR ---
        // NOTE: the label provided must be able to be converted to an integer (ideally a single-digit number)
        public NumberButton(string label, double x, double y, int size, Font font, float width, float height, Color labelClr, Color backClr) : base(label, x, y, size, font, width, height, labelClr, backClr) { }

        // --- METHODS ---
        // append this button's digit to the most recent operation
        public override Boolean Activate()
        {
            Calculator calc = Calculator.GetInstance();
            Caretaker.GetInstance().AddMemento(calc.Save());
            try
            {
                calc.LastOperation.AppendDigit(int.Parse(Label));
                return true;
            }
            catch
            {
                Caretaker.GetInstance().Undo(); // immediately undo the save made
                return false;
            }
        }
    }
}
