using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace AnimusTest.Models {
    public class Layer {
        public string title { get; set; } = "Untitled";
        //public List<Shape> Shapes { get; set; } = new List<Shape>(); // тестові шейпи
        public List<Stroke> Strokes { get; set; } = new List<Stroke>();
        public bool IsVisible { get; set; } = true;
        public float Opacity { get; set; } = 1f;

        private Bitmap cashedBitmap = null; 
        private bool isDirty = false;

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
