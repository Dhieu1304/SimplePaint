using System;
using System.Windows;
using System.Windows.Media;

namespace Contract
{
    public interface IShape
    {
        string Name { get; }
        void HandleStart(double x, double y);
        void HandleEnd(double x, double y);
        void Thickness(int x);
        void Color(SolidColorBrush color);
        void FillColor(SolidColorBrush fillColor);
        void StrokeDashArr(string StrokeDash);

        UIElement Draw();
        IShape Clone();

        string toString();
        IShape Parse(string line);
    }
}
