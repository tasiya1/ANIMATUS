using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnimusTest.Models
{
    public class Stroke
    {
        public double Width { get; set; } = 2;
        public Color Color { get; set; } = Colors.Black;
        public List<System.Windows.Point> Points { get; set; } = new List<System.Windows.Point>();

        public Polyline ToPolyline()
        {
            Polyline polyline = new Polyline
            {
                StrokeThickness = Width,
                Stroke = new SolidColorBrush(Color),
                Points = new System.Windows.Media.PointCollection(Points)
            };
            return polyline;
        }
    }
}
