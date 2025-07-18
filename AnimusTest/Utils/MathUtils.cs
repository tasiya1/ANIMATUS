using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimusTest.Utils
{
    public static class MathUtils
    {
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static List<(int x, int y)> PathByBresenham(int x0, int y0, int x1, int y1)
        {
            List<(int x, int y)> points = new List<(int x, int y)>();

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                points.Add((x0, y0));

                if (x0 == x1 && y0 == y1) break;

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }

            return points;
        }
    }
}
