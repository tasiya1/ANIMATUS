using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace AnimusTest.Models
{
    public class Brush
    {
        public SKPaint body;
        public void Draw() { }
        public BrushType Type { get; }
    }

    public enum BrushType
    {
        Pen,
        Pencil,
        Airbrush,
        Spray,
        Crayon,
        Marker,
        Watercolor,
        Pastel,
        Oil,
        Acrylic,
        Ink,
        Chalk,
        Calligraphy
        /*           
        Stencil,
        Blending,
        Eraser,
        Smudge,
        Texture,
        Gradient,
        Pattern,
        Fill,
        Stroke,
        Shape,
        Line,
        Rectangle,
        Ellipse,
        Polygon,
        Polyline,
        Path,
        Freeform,
        Selection,
        Clone,
        Transform,
        Move,
        Rotate,
        Scale,
        Skew,
        Flip,
        Crop,
        Resize,
        Mirror,
        Distort,
        Warp,
        Perspective,
        Shear,
        SkewX,
        SkewY,
        SkewZ,
        Twist,
        Bend,
        Stretch
        */
        // 
    }
}
