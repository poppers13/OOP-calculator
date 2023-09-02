using System;
using SplashKitSDK;

namespace VisualCalculator {
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
}