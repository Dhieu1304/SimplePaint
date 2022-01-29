using Contract;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Line2D
{
    public class Line2D : IShape
    {
        private Point2D _start = new Point2D();
        private Point2D _end = new Point2D();
        private int _thickness;
        private SolidColorBrush _color;
        private String _dashStyle;

        public string Name => "Line";

        public void HandleStart(double x, double y)
        {
            _start = new Point2D() { X = x, Y = y };
        }

        public void HandleEnd(double x, double y)
        {
            _end = new Point2D() { X = x, Y = y };
        }

        public UIElement Draw()
        {
            Line l = new Line()
            {
                X1 = _start.X,
                Y1 = _start.Y,
                X2 = _end.X,
                Y2 = _end.Y,
                StrokeThickness = _thickness,
                Stroke = _color,
                StrokeDashArray = DoubleCollection.Parse(_dashStyle),
            };

            return l;
        }

        public IShape Clone()
        {
            return new Line2D();
        }

        public string toString()
        {
            string result = $"{Name} {_start.toString()}, {_end.toString()}, {_thickness}, {_color}, {_dashStyle}";
            return result;
        }

        public IShape Parse(string line)
        {
            string[] tokens = line.Split(new string[] { "Line " }, StringSplitOptions.None);
            string[] parts = tokens[1].Split(new string[] { ", " }, StringSplitOptions.None);

            Point2D p = new Point2D();
            Point2D start = (Point2D)p.Parse(parts[0]);
            Point2D end = (Point2D)p.Parse(parts[1]);
            int thickness = Int32.Parse(parts[2]);

            SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[3]));

            String dashStyle = parts[4];

            Line2D result = new Line2D()
            {
                _start = start,
                _end = end,
                _thickness = thickness,
                _color = color,
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
            //Do nothing
        }

        public void StrokeDashArr(string StrokeDash)
        {
            _dashStyle = StrokeDash;
        }
    }
}
