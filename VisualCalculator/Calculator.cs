using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class Calculator
    {
        // --- FIELDS ---
        private static Calculator _instance;
        private List<Operation> _operationList;

        // --- CONSTRUCTOR ---
        // constructor is private: only GetInstance() can call it
        private Calculator()
        {
            _operationList = new List<Operation>();
            _operationList.Add(new Operation());
        }

        // --- PROPERTIES ---
        public string FullString
        {
            get
            {
                string output = "";
                foreach (Operation o in _operationList)
                {
                    output += o.OutputString;
                }
                return output;
            }
        }
        public Operation LastOperation
        {
            get
            {
                return _operationList.Last();
            }
        }

        // --- METHODS ---
        public static Calculator GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Calculator();
            }
            return _instance;
        }
        // run all operations and return the calculated result
        // if all operations run successfully, return true; otherwise, false
        public Boolean Calculate()
        {
            int? calculatedNum;
            int currentNum = 0;

            if (_operationList.Count > 1)
            {
                foreach (Operation o in _operationList)
                {
                    if (o.HasDigits)
                    {
                        calculatedNum = o.Calculate(currentNum);
                        if (calculatedNum == null)
                        {
                            return false; // an error has occurred in calculation
                        }

                        currentNum = (int)calculatedNum;
                    }
                    else
                    {
                        return false; // no value was provided; an error has occurred
                    }
                }
            }
            else
            {
                return false; // only one operation, and thus no calculation to be performed
            }
            
            // if all operations run successfully
            Operation op = new Operation();
            op.AppendDigit(currentNum);

            // save the current list before moving on
            Caretaker.GetInstance().AddMemento(Save());

            _operationList.Clear(); // empty the list
            _operationList.Add(op);
            return true;
        }
        public void AddOperation(Operation op)
        {
            // save the current list before moving on
            Caretaker.GetInstance().AddMemento(Save());
            _operationList.Add(op);
        }
        // create a memento of the calculator's current state
        // this is called for all calculator functions that change the operation list in some way
        public Memento Save()
        {
            return new Memento(this);
        }

        // this class handles prior states for the edit history
        public class Memento
        {
            // --- FIELDS ---
            private Calculator _calc; // contains a reference to the object that originated it
            private List<Operation> _operationList;
            private string _fullString; // since the operation list won't change, this can be stored directly

            // --- CONSTRUCTOR ---
            // since Memento is nested within Calculator, it can access Calculator's private fields
            // this allows the fields of Calculator to be private, while still being handled by a separate object
            public Memento(Calculator calc)
            {
                _calc = calc;
                _operationList = new List<Operation>();

                // performs a mix of a deep and shallow copy of the original _operationList
                // only the most recent operation is duplicated; the rest simply copy the pointers
                // this is because the earlier operations cannot be modified by the user, and will stay identical
                // thus, duplicating earlier operations would simply use up unnecessary memory in the heap
                for (int i = 0; i < calc._operationList.Count - 1; i++)
                {
                    _operationList.Add(calc._operationList[i]);
                }
                // the final element is a clone of the original operation
                _operationList.Add(calc.LastOperation.Duplicate());

                _fullString = calc.FullString;
            }
            
            // --- PROPERTIES ---
            public string FullString
            {
                get { return _fullString; }
            }

            // --- METHODS ---
            // copy the operationlist from this memento back into calculator
            public void Restore()
            {
                _calc._operationList = _operationList;
            }
        }
    }
}
