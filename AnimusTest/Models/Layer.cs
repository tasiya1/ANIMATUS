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
        public string title { get; set; } = "Untitled";
        //public List<Shape> Shapes { get; set; } = new List<Shape>(); // тестові шейпи
        public List<Stroke> Strokes { get; set; } = new List<Stroke>();
        public bool IsVisible { get; set; } = true;
        public float Opacity { get; set; } = 1f;

        private Bitmap cashedBitmap = null; 
        private bool isDirty = false;
        private int width;
        private int height;

        public Layer(int width, int height)
        {
            Bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var canvas = new SKCanvas(Bitmap);
            canvas.Clear(SKColors.Transparent);
        }

        public void GetRenderedBitmap()
        {

        }

        public void RenderBitmap()
        {

        }

        public void Remove(Stroke stroke)
        {
            //throw new NotImplementedException();
            Strokes.Remove(stroke);
        }

        public void Add(Stroke stroke)
        {
            //throw new NotImplementedException();
            Strokes.Add(stroke);
        }
    }


}
