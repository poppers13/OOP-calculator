using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualCalculator
{
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
}
