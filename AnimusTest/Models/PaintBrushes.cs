using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace AnimusTest.Models
{
    public class Pen : Brush
    {
        public Pen()
        {
            this.body = new SKPaint
            {
                Color = SKColors.Purple,
                StrokeWidth = 1,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Stroke
            };
        }
        public BrushType Type => BrushType.Pen;

        public void Draw()
        {
            throw new NotImplementedException();
        }
    }
}
