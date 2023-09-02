using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
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
}
