using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class Operation
    {
        // --- FIELDS ---
        CalculationStrategy _strategy;
        int? _storedNum;

        // --- CONSTRUCTOR ---
        public Operation(CalculationStrategy strategy)
        {
            _strategy = strategy;
            _storedNum = null;
        }
        public Operation() : this(null) { } // if no strategy provided, just leave it null

        // --- PROPERTIES ---
        public Boolean HasDigits
        {
            get
            {
                if (_storedNum == null)
                {
                    return false;
                }
                return true;
            }
        }
        public string OutputString
        {
            get
            {
                if (_strategy != null)
                {
                    return _strategy.GenerateString(DisplayNum);
                }

                return $" {DisplayNum}"; // if the first operation, then just return the number
            }
        }
        // like _storedNum, but uses 0 in place of null
        private int ActiveNum
        {
            get
            {
                if (_storedNum == null)
                {
                    return 0;
                }
                else
                {
                    return (int)_storedNum;
                }
            }
        }
        // like _storedNum, but as a string
        private string DisplayNum
        {
            get
            {
                if (_storedNum == null)
                {
                    return "_";
                }
                else
                {
                    return _storedNum.ToString();
                }
            }
        }

        // --- METHODS ---
        // moves all digits one place to the left, then adds the new digit
        // if _storedNum is null, it will be replaced by an integer
        public void AppendDigit(int digit)
        {
            _storedNum = ActiveNum * 10 + digit;
        }
        // swaps out the current strategy
        public Boolean ChangeStrategy(CalculationStrategy mode)
        {
            if (_strategy != null)
            {
                _strategy = mode;
                return true;
            }
            return false; // if strategy is null, then it cannot be changed
        }
        // execute the strategy's calculate method and return the result
        public int? Calculate(int numA)
        {
            if (_strategy != null)
            {
                return _strategy.Calculate(numA, ActiveNum);
            }

            return _storedNum;
        }
        // creates an identical clone of this object and returns it
        public Operation Duplicate()
        {
            Operation output = new Operation(_strategy);
            if (HasDigits)
            {
                output.AppendDigit(ActiveNum);
            }

            return output;
        }
    }
}
