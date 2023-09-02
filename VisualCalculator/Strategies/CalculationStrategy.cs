using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public interface CalculationStrategy
    {
        // --- METHODS ---
        // calculate will return null for invalid calculations
        public int? Calculate(int numA, int numB);
        public string GenerateString(string num);
    }
}
