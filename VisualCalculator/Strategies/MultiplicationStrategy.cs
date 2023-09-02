using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class MultiplicationStrategy : CalculationStrategy
    {
        // --- METHODS ---
        public int? Calculate(int numA, int numB)
        {
            return (numA * numB);
        }
        public string GenerateString(string num)
        {
            return " x " + num;
        }
    }
}
