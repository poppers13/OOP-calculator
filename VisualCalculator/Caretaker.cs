using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class Caretaker
    {
        // --- FIELDS ---
        private static Caretaker _instance;
        List<Calculator.Memento> _mementoList;

        // --- CONSTRUCTOR ---
        private Caretaker()
        {
            _mementoList = new List<Calculator.Memento>();
        }

        // --- PROPERTIES ---
        // shows the most recent operations
        public string HistoryString
        {
            get
            {
                string output = "";
                for (int i = _mementoList.Count - 1; i >= 0; i--)
                {
                    output += _mementoList[i].FullString;
                    if (i > 0) 
                    {
                        output += ",\n"; // if there are more mementos to read
                    }
                }
                return output;
            }
        }

        // --- METHODS ---
        public static Caretaker GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Caretaker();
            }
            return _instance;
        }
        // return the calculator to the state from its most recent memento,
        // then remove the memento from the list
        public void Undo()
        {
            if (_mementoList.Count > 0)
            {
                Calculator.Memento m = _mementoList.Last();
                m.Restore(); // restore the calculator to the memento's state
                _mementoList.Remove(m); // remove the final element from the list
            }
            else
            {
                Console.WriteLine("There are no actions to undo");
            }
        }
        public void AddMemento(Calculator.Memento m)
        {
            _mementoList.Add(m);
        }
    }
}
