using Contract;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Text
{
    public class Text : IShape
    {
        private string _text;
        private FontFamily _fontFamily;
        private double _fontSize;
        private FontWeight _fontWeight;
        private FontStyle _fontStyle;
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();

        private SolidColorBrush _color;
        private SolidColorBrush _fillColor;

        private TextDecorationCollection _decoration;
        private string _decora;

        public string Name => "Text";

        public IShape Clone()
        {
            return new Text();
        }

        public void Color(SolidColorBrush color)
        {
            _color = color;
        }

        public UIElement Draw()
        {
            TextBlock result = new TextBlock()
            {
                Width = Math.Abs(_rightBottom.X - _leftTop.X),
                Height = Math.Abs(_rightBottom.Y - _leftTop.Y),
                Background = _fillColor,
                TextWrapping = TextWrapping.Wrap,
                Text = _text,
                FontFamily = _fontFamily,
                FontSize = _fontSize,
                FontStyle = _fontStyle,
                FontWeight = _fontWeight,
                TextDecorations = _decoration,
                Foreground = _color,

            };

            if (_leftTop.X < _rightBottom.X && _leftTop.Y < _rightBottom.Y)
            {
                Canvas.SetLeft(result, _leftTop.X);
                Canvas.SetTop(result, _leftTop.Y);
            }
            else if (_leftTop.X > _rightBottom.X && _leftTop.Y > _rightBottom.Y)
            {
                Canvas.SetLeft(result, _rightBottom.X);
                Canvas.SetTop(result, _rightBottom.Y);
            }
            else if (_leftTop.X > _rightBottom.X && _leftTop.Y < _rightBottom.Y)
            {
                Canvas.SetLeft(result, _rightBottom.X);
                Canvas.SetTop(result, _leftTop.Y);
            }
            else if (_leftTop.X < _rightBottom.X && _leftTop.Y > _rightBottom.Y)
            {
                Canvas.SetLeft(result, _leftTop.X);
                Canvas.SetTop(result, _rightBottom.Y);
            }

            return result;
        }

        public void FillColor(SolidColorBrush fillColor)
        {
            _fillColor = fillColor;
        }

        public void HandleEnd(double x, double y)
        {
            _leftTop = new Point2D() { X = x, Y = y };
        }

        public void HandleStart(double x, double y)
        {
            _rightBottom = new Point2D() { X = x, Y = y };
        }

        public IShape Parse(string line)
        {
            string[] tokens = line.Split(new string[] { "Text " }, StringSplitOptions.None);
            string[] parts = tokens[1].Split(new string[] { "| " }, StringSplitOptions.None);

            Point2D p = new Point2D();
            Point2D leftTop = (Point2D)p.Parse(parts[0]);
            Point2D rightBottom = (Point2D)p.Parse(parts[1]);

            string text = parts[2];
            SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[3]));

            SolidColorBrush fillColor = new SolidColorBrush();
            if (parts[4] != "")
            {
                fillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[4]));
            }
            
            FontFamily fontFamily = new FontFamily(parts[5]);
            double fontSize = Double.Parse(parts[6]);
            FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(parts[7]);
            FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(parts[8]);

            TextDecorationCollection decorations;
            if (parts[9] == "")
            {
                decorations = null;
            }
            else
            {
                decorations = TextDecorations.Underline;
            }

            Text result = new Text()
            {
                _text = text,
                _leftTop = leftTop,
                _rightBottom = rightBottom,
                _color = color,
                _fillColor = fillColor,
                _fontFamily = fontFamily,
                _fontSize = fontSize,
                _fontWeight = fontWeight,
                _fontStyle = fontStyle,
                _decoration = decorations
            };

            return result;
        }

        public void StrokeDashArr(string StrokeDash)
        {
            string[] tokens = StrokeDash.Split(new string[] { "| " }, StringSplitOptions.None);

            _text = tokens[0];
            _fontFamily = new FontFamily(tokens[1]);
            _fontSize = Double.Parse(tokens[2]);
            _fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(tokens[3]);
            _fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(tokens[4]);
            _decora = tokens[5];

            if (tokens[5] == "")
            {
                _decoration = null;
            }
            else
            {
                _decoration = TextDecorations.Underline;
            }

        }

        public void Thickness(int x)
        {
            //Do nothing
        }

        public string toString()
        {
            string result = $"{Name} {_leftTop.toString()}| {_rightBottom.toString()}| "
                + $"{_text}| {_color}| {_fillColor}| {_fontFamily}| {_fontSize}| {_fontWeight}| {_fontStyle}| "
                + $"{_decora}";
            return result;
        }
    }
}
