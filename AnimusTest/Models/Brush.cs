using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AnimusTest.Utils;
using SkiaSharp;

namespace AnimusTest.Models
{
    public abstract class Brush
    {

        public SKBitmap brushTexture;
        public float brushSize = 32f;
        public SKColor color = new SKColor(0,0,0,1);
        public SKRect brushStamp;

    public virtual void Draw(SKPoint previousPoint, SKPoint currentPoint, SKCanvas canvas)
        {
            float halfSize = brushSize / 2f;

            List <(int x, int y)> pathPoints = MathUtils.PathByBresenham(
                (int)previousPoint.X, (int)previousPoint.Y,
                (int)currentPoint.X, (int)currentPoint.Y
            );

            for (int i = 0; i < pathPoints.Count; i++)
            {
                float x = pathPoints[i].x;
                float y = pathPoints[i].y;

                var destRect = new SKRect(
                    x - halfSize,
                    y - halfSize,
                    x + halfSize,
                    y + halfSize
                );
                
                canvas.DrawBitmap(brushTexture, destRect);
            }
        }
        public BrushType Type { get; }

        public virtual void TintBrush(SKColor color)
        {
            SKBitmap tinted = new SKBitmap(brushTexture.Width, brushTexture.Height);
            using var canvas = new SKCanvas(tinted);

            using var paint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateBlendMode(color, SKBlendMode.SrcATop),
                IsAntialias = true
            };

            canvas.DrawBitmap(brushTexture, 0, 0, paint);
            brushTexture = tinted;
        }

        public virtual void UpdateBrushSize(float brushSize)
        {
            this.brushSize = brushSize;
        }
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
        Calligraphy,
        Sponge,
        Charcoal
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
