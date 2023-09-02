using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
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
}
