using Contract;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Eraser
{
    public class Eraser : IShape
    {
        private Polyline _line = new Polyline();
        private int _thickness;
        public string Name => "Eraser";

        public IShape Clone()
        {
            return new Eraser();
        }

        public void Color(SolidColorBrush color)
        {
            //Do nothing
        }

        public UIElement Draw()
        {
            _line.StrokeThickness = _thickness;
            _line.Stroke = new SolidColorBrush(Colors.White);

            return _line;
        }

        public void FillColor(Color fillColor)
        {
            //Do nothing
        }

        public void FillColor(SolidColorBrush fillColor)
        {
            //Do nothing
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

            string[] tokens = line.Split(new string[] { "Eraser " }, StringSplitOptions.None);
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

            IShape result = new Eraser()
            {
                _line = _l,
                _thickness = thickness
            };

            return result;
        }

        public void StrokeDashArr(string StrokeDash)
        {
            //Do nothing
        }

        public void Thickness(int x)
        {
            _thickness = x + 2;
        }

        public string toString()
        {
            int count = _line.Points.Count;
            string allPoint = "";
            for (int i = 0; i < count; i++)
            {
                allPoint += $"{_line.Points[i].X} {_line.Points[i].Y}|";
            }

            string result = $"{Name} {allPoint}, {_thickness}";
            return result;
        }
    }
}
