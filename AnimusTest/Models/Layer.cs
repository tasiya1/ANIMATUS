using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace AnimusTest.Models {
    public class Layer {
        public string Name { get; set; }
        //public List<Shape> Shapes { get; set; } = new List<Shape>(); // тестові шейпи
        public List<Stroke> Strokes { get; set; } = new List<Stroke>();

        private Bitmap cashedImage = null;
        private bool isDirty = false;

        public void GetRenderedBitmap()
        {

        }

        public void RenderBitmap()
        {

        }

    }


}
