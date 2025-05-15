using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace AnimusTest.Models
{
    public class RasterLayer
    {
        public SKBitmap Bitmap { get; set; }
        public bool IsVisible { get; set; } = true;
        public float Opacity { get; set; } = 1f;

    }
}
