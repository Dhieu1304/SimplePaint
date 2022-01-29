using Contract;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Image2D
{
    public class Image2D : IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();
        private ImageBrush _img;
        private string _imgSrc = "";
        public string Name => "Image";

        public IShape Clone()
        {
            return new Image2D();
        }

        public void Color(SolidColorBrush color)
        {
            //Do nothing
        }

        public UIElement Draw()
        {
            var rect = new Rectangle()
            {
                Width = Math.Abs(_rightBottom.X - _leftTop.X),
                Height = Math.Abs(_rightBottom.Y - _leftTop.Y),
                Fill = _img,
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

        public void FillColor(SolidColorBrush fillColor)
        {
            //Do nothing
        }

        public void HandleEnd(double x, double y)
        {
            _rightBottom = new Point2D() { X = x, Y = y };
        }

        public void HandleStart(double x, double y)
        {
            _leftTop = new Point2D() { X = x, Y = y };
        }

        public IShape Parse(string line)
        {
            string[] tokens = line.Split(new string[] { "Image " }, StringSplitOptions.None);
            string[] parts = tokens[1].Split(new string[] { ", " }, StringSplitOptions.None);

            Point2D p = new Point2D();
            Point2D start = (Point2D)p.Parse(parts[0]);
            Point2D end = (Point2D)p.Parse(parts[1]);
            ImageBrush img = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(parts[2], UriKind.Absolute))
            };

            Image2D result = new Image2D()
            {
                _leftTop = start,
                _rightBottom = end,
                _img = img,
                _imgSrc = parts[2]
            };

            return result;
        }

        public void StrokeDashArr(string StrokeDash)
        {
            _imgSrc = StrokeDash;
            _img = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(_imgSrc, UriKind.Absolute))
            };
        }

        public void Thickness(int x)
        {
            //Do nothing
        }

        public string toString()
        {
            string result = $"{Name} {_leftTop.toString()}, {_rightBottom.toString()}, {_imgSrc}";
            Debug.WriteLine("String: " + result);
            return result;
        }
    }
}
