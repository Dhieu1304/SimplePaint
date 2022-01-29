using Contract;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rectangle2D
{
    public class Rectangle2D : IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();
        private int _thickness;
        private SolidColorBrush _color;
        private SolidColorBrush _fillColor;
        private String _dashStyle = "";

        public string Name => "Rectangle";

        public UIElement Draw()
        {
            var rect = new Rectangle()
            {
                Width = Math.Abs(_rightBottom.X - _leftTop.X),
                Height = Math.Abs(_rightBottom.Y - _leftTop.Y),
                StrokeThickness = _thickness,
                Stroke = _color,
                Fill = _fillColor,
                StrokeDashArray = DoubleCollection.Parse(_dashStyle),
            };

            if (_leftTop.X < _rightBottom.X && _leftTop.Y < _rightBottom.Y)
            {
                Canvas.SetLeft(rect, _leftTop.X);
                Canvas.SetTop(rect, _leftTop.Y);
            }
            else if (_leftTop.X > _rightBottom.X && _leftTop.Y > _rightBottom.Y)
            {
                Canvas.SetLeft(rect, _rightBottom.X);
                Canvas.SetTop(rect, _rightBottom.Y);
            }
            else if (_leftTop.X > _rightBottom.X && _leftTop.Y < _rightBottom.Y)
            {
                Canvas.SetLeft(rect, _rightBottom.X);
                Canvas.SetTop(rect, _leftTop.Y);
            }
            else if (_leftTop.X < _rightBottom.X && _leftTop.Y > _rightBottom.Y)
            {
                Canvas.SetLeft(rect, _leftTop.X);
                Canvas.SetTop(rect, _rightBottom.Y);
            }

            return rect;
        }

        public void HandleStart(double x, double y)
        {
            _leftTop = new Point2D() { X = x, Y = y };
        }

        public void HandleEnd(double x, double y)
        {
            _rightBottom = new Point2D() { X = x, Y = y };
        }

        public IShape Clone()
        {
            return new Rectangle2D();
        }

        public string toString()
        {
            string result = $"{Name} {_leftTop.toString()}, {_rightBottom.toString()}, {_thickness}, {_color}, {_fillColor}, {_dashStyle}";
            return result;
        }

        public IShape Parse(string line)
        {
            string[] tokens = line.Split(new string[] { "Rectangle " }, StringSplitOptions.None);
            string[] parts = tokens[1].Split(new string[] { ", " }, StringSplitOptions.None);

            Point2D p = new Point2D();
            Point2D start = (Point2D)p.Parse(parts[0]);
            Point2D end = (Point2D)p.Parse(parts[1]);
            int thickness = Int32.Parse(parts[2]);
            SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[3]));
            SolidColorBrush fillColor = new SolidColorBrush();
            if (parts[4] != "")
            {
                fillColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[4]));
            }
            String dashStyle = parts[5];

            Rectangle2D result = new Rectangle2D()
            {
                _leftTop = start,
                _rightBottom = end,
                _thickness = thickness,
                _color = color,
                _fillColor = fillColor,
                _dashStyle = dashStyle
            };

            return result;
        }

        public void Thickness(int x)
        {
            _thickness = x;
        }

        public void Color(SolidColorBrush color)
        {
            _color = color;
        }

        public void FillColor(SolidColorBrush fillColor)
        {
            _fillColor = fillColor;
        }

        public void StrokeDashArr(string StrokeDash)
        {
            _dashStyle = StrokeDash;
        }
    }
}
