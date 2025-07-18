using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using SkiaSharp;

namespace AnimusTest.Models {
    public class Layer {

        public SKBitmap Bitmap { get; }
        public bool IsVisible { get; set; } = true;
        public float Opacity { get; set; } = 1f;

        public int width;
        public int height;

        public int top = 0;
        public int left = 0;

        public Layer(int width, int height)
        {
            Bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var canvas = new SKCanvas(Bitmap);
            canvas.Clear(SKColors.Transparent);
        }

        public Layer(SKBitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public void GetRenderedBitmap()
        {

        }

        public void RenderBitmap()
        {

        }

    }


}
