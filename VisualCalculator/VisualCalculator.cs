using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
    public class Program
    {
        public static void Main()
        {
            Window window = new Window("Calculator", 800, 600);
            // singleton classes are stored inside variables for the sake of convenience
            Calculator mainCalc = Calculator.GetInstance();
            Caretaker mainCaretaker = Caretaker.GetInstance();
            GUIHandler mainGUI = GUIHandler.GetInstance();

            do
            {
                SplashKit.ProcessEvents();

                // if the user has clicked, check if they clicked any buttons
                if (SplashKit.MouseClicked(MouseButton.LeftButton))
                {
                    Button button = mainGUI.CheckButtons(SplashKit.MousePosition());
                    if (button != null)
                    {
                        // activate the button
                        if (button.Activate())
                        {
                            // if it activated successfully
                            mainGUI.MainDisplay.Label = mainCalc.FullString;
                            mainGUI.HistoryDisplay.Label = mainCaretaker.HistoryString;
                        }
                        else
                        {
                            // if it failed to activate
                            Console.WriteLine($"The button labelled '{button.Label}' failed to activate.");
                        }
                    }
                }

                // draw all GUI elements
                SplashKit.ClearScreen();
                mainGUI.Draw();

                SplashKit.RefreshScreen();
            } while (!window.CloseRequested);

            SplashKit.FreeAllFonts();
        }
    }
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
    public class GUIHandler
    {
        // --- FIELDS ---
        private static GUIHandler _instance;
        private TextDisplay _mainDisplay;
        private TextDisplay _historyDisplay;
        private List<Button> _buttonList;

        // --- CONSTRUCTOR ---
        private GUIHandler(Font font)
        {
            _mainDisplay = new TextDisplay(" _", 34, 32, 80, font, Color.Black);
            _historyDisplay = new TextDisplay("", 34, 122, 40, font, Color.Gray);

            _buttonList = new List<Button>(new Button[] {
                    new NumberButton("1", 392, 184, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("2", 492, 184, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("3", 592, 184, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("4", 392, 284, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("5", 492, 284, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("6", 592, 284, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("7", 392, 384, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("8", 492, 384, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("9", 592, 384, 40, font, 80, 80, Color.Black, Color.Gray),
                    new NumberButton("0", 492, 484, 40, font, 80, 80, Color.Black, Color.Gray),
                    new ModeButton(new AdditionStrategy(), "+", 692, 184, 40, font, 80, 80, Color.DarkGreen, Color.BrightGreen),
                    new ModeButton(new SubtractionStrategy(), "-", 692, 284, 40, font, 80, 80, Color.DeepPink, Color.LightPink),
                    new ModeButton(new MultiplicationStrategy(), "x", 692, 384, 40, font, 80, 80, Color.OrangeRed, Color.DarkOrange),
                    new ModeButton(new DivisionStrategy(), "/", 692, 484, 40, font, 80, 80, Color.DarkBlue, Color.LightBlue),
                    new UndoButton("Undo", 392, 484, 24, font, 80, 80, Color.Red, Color.DarkRed),
                    new CalculateButton("=", 592, 484, 40, font, 80, 80, Color.White, Color.Black)
                });
        }

        // --- PROPERTIES ---
        public TextDisplay MainDisplay
        {
            get { return _mainDisplay; }
        }
        public TextDisplay HistoryDisplay
        {
            get { return _historyDisplay; }
        }

        // --- METHODS ---
        public static GUIHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GUIHandler(SplashKit.LoadFont("NoName37", @"C:\Users\aidan\OneDrive - Swinburne University\Object Oriented Programming\Portfolio\[!] Custom Program (6.2, 6.3, 6.4, 6.5)\6.5HD (Custom Program)\VisualCalculator\Resources\NoName37.otf"));
            }
            return _instance;
        }

        public void Draw()
        {
            foreach (Button b in _buttonList)
            {
                b.Draw();
            }

            _mainDisplay.Draw();
            _historyDisplay.Draw();
        }

        public Button CheckButtons(Point2D mousePos)
        {
            foreach (Button b in _buttonList)
            {
                if (b.IsAt(mousePos))
                {
                    return b;
                }
            }

            return null; // if no buttons were clicked on
        }
    }
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
    public class TextDisplay
    {
        // --- FIELDS ---
        private string _label;
        private double _x, _y;
        private int _fontSize;
        private Font _font;
        private Color _labelColor;

        // --- CONSTRUCTOR ---
        public TextDisplay(string label, double x, double y, int size, Font font, Color color)
        {
            _label = label;
            _x = x;
            _y = y;
            _fontSize = size;
            _font = font;
            _labelColor = color;
        }

        // --- PROPERTIES ---
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
        public double X
        {
            get { return _x; }
        }
        public double Y
        {
            get { return _y; }
        }
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        public Font Font
        {
            get { return _font; }
        }
        public Color LabelColor
        {
            get { return _labelColor; }
            set { _labelColor = value; }
        }

        // --- METHODS ---
        public virtual void Draw()
        {
            SplashKit.DrawText(_label, _labelColor, _font, _fontSize, _x, _y);
        }
        public virtual void Draw(double x_offset, double y_offset)
        {
            SplashKit.DrawText(_label, _labelColor, _font, _fontSize, _x + x_offset, _y + y_offset);
        }
    }
    public abstract class Button : TextDisplay
    {
        // --- FIELDS ---
        private float _height, _width;
        private Color _backColor;
        private double _xOffset, _yOffset;

        // --- CONSTRUCTOR ---
        public Button(string label, double x, double y, int size, Font font, float width, float height, Color labelClr, Color backClr) : base(label, x, y, size, font, labelClr)
        {
            _width = width;
            _height = height;
            _backColor = backClr;
            // calculate height and width of text, and thus offset
            _xOffset = (_width / 2) - (SplashKit.TextWidth(Label, Font, FontSize) / 2);
            _yOffset = (_height / 2) - (SplashKit.TextHeight(Label, Font, FontSize) / 2);
        }

        // --- METHODS ---
        public Boolean IsAt(Point2D mousePos)
        {
            return (mousePos.X < X + _width && X < mousePos.X && mousePos.Y < Y + _height && Y < mousePos.Y);
        }
        public override void Draw()
        {
            SplashKit.FillRectangle(_backColor, X, Y, _width, _height);
            base.Draw(_xOffset, _yOffset); // draw the original text with offset
        }
        // if activation fails, return false
        public abstract Boolean Activate();
    }
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
    public interface CalculationStrategy
    {
        // --- METHODS ---
        // calculate will return null for invalid calculations
        public int? Calculate(int numA, int numB);
        public string GenerateString(string num);
    }
    public class AdditionStrategy : CalculationStrategy
    {
        // --- METHODS ---
        public int? Calculate(int numA, int numB)
        {
            return (numA + numB);
        }
        public string GenerateString(string num)
        {
            return " + " + num;
        }
    }
    public class SubtractionStrategy : CalculationStrategy
    {
        // --- METHODS ---
        public int? Calculate(int numA, int numB)
        {
            return (numA - numB);
        }
        public string GenerateString(string num)
        {
            return " - " + num;
        }
    }
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
