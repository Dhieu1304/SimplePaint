using Contract;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Brushes
{
    public class Brushes : IShape
    {
        private Polyline _line = new Polyline();
        private int _thickness;
        private SolidColorBrush _color;
        private String _dashStyle;
        public string Name => "Brushes";

        public IShape Clone()
        {
            return new Brushes();
        }

        public void Color(SolidColorBrush color)
        {
            _color = color;
        }

        public UIElement Draw()
        {
            _line.StrokeThickness = _thickness;
            _line.Stroke = _color;
            _line.StrokeDashArray = DoubleCollection.Parse(_dashStyle);

            return _line;
        }

        public void FillColor(SolidColorBrush fillColor)
        {
            //Do Nothing
        }

        public void HandleEnd(double x, double y)
        {
            Point _end = new Point(x, y);
            _line.Points.Add(_end);
        }

        public void HandleStart(double x, double y)
        {
            Point _start = new Point(x, y);
            _line.Points.Add(_start);
        }

        public IShape Parse(string line)
        {
            Polyline _l = new Polyline();

            string[] tokens = line.Split(new string[] { "Brushes " }, StringSplitOptions.None);
            string[] parts = tokens[1].Split(new string[] { ", " }, StringSplitOptions.None);

            string[] words = parts[0].Split(new string[] { "|" }, StringSplitOptions.None);
            //2 2
            for (int i = 0; i < words.Length - 1; i++)
            {
                string[] number = words[i].Split(new string[] { " " }, StringSplitOptions.None);
                Point p = new Point(Double.Parse(number[0]), Double.Parse(number[1]));
                _l.Points.Add(p);
            }

            int thickness = Int32.Parse(parts[1]);
            SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[2]));
            String dashStyle = parts[3];

            IShape result = new Brushes()
            {
                _line = _l,
                _thickness = thickness,
                _color = color,
                _dashStyle = dashStyle
            };

            return result;
        }

        public void StrokeDashArr(string StrokeDash)
        {
            _dashStyle = StrokeDash;
        }

        public void Thickness(int x)
        {
            _thickness = x;
        }

        public string toString()
        {
            int count = _line.Points.Count;
            string allPoint = "";
            for (int i = 0; i < count; i++)
            {
                allPoint += $"{_line.Points[i].X} {_line.Points[i].Y}|";
            }

            string result = $"{Name} {allPoint}, {_thickness}, {_color}, {_dashStyle}";
            return result;
        }
    }
}
