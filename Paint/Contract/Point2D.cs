using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Contract
{
    public class Point2D : IShape
    {
        public double X { get; set; }
        public double Y { get; set; }

        public string Name => "Point";

        public void HandleStart(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void HandleEnd(double x, double y)
        {
            X = x;
            Y = y;
        }

        public UIElement Draw()
        {
            Line l = new Line()
            {
                X1 = X,
                Y1 = Y,
                X2 = X,
                Y2 = Y,
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Red),
            };

            return l;
        }

        public IShape Clone()
        {
            return new Point2D();
        }

        public string toString()
        {
            string result = $"({X} {Y})";
            return result;
        }

        public IShape Parse(string line)
        {
            //(1 1)
            int firstIndex = line.IndexOf(" ");
            string x = line.Substring(1, firstIndex - 1);
            string y = line.Substring(firstIndex + 1, line.Length - firstIndex - 2);

            Point2D result = new Point2D() { X = Double.Parse(x), Y = Double.Parse(y) };

            return result;
        }

        public void Thickness(int x)
        {
            //Do nothing
        }

        public void Color(SolidColorBrush color)
        {
            //Do nothing
        }

        public void FillColor(SolidColorBrush color)
        {
            //Do nothing
        }

        public void StrokeDashArr(string StrokeDash)
        {
            //Do nothing
        }
    }
}
