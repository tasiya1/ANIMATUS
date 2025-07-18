using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace AnimusTest.Models {
    public class Frame
    {
        public List<Layer> Layers { get; } = new();

        private SKBitmap cashedBitmap;
        public bool isDirty = false;
        public Frame(int width, int height, int layerCount)
        {
            for (int i = 0; i < layerCount; i++)
                Layers.Add(new Layer(width, height));
        }

        public Frame()
        {
        }
    }
}
