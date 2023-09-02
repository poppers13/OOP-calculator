using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class DivisionStrategy : CalculationStrategy
    {
        // --- METHODS ---
        public int? Calculate(int numA, int numB)
        {
            // if dividing by zero, cancel the calculation
            if (numB == 0)
            {
                return null;
            }
            return (numA / numB);
        }
        public string GenerateString(string num)
        {
            return " / " + num;
        }
    }
}
