using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimusTest.Models {
    public class Frame
    {
        public List<Layer> Layers { get; } = new();
        public Frame(int width, int height, int layerCount)
        {
            for (int i = 0; i < layerCount; i++)
                Layers.Add(new Layer(width, height));
        }
    }
}
