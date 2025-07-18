using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimusTest.Controls;
using SkiaSharp;

namespace AnimusTest.Models
{
    public class Pen : Brush
    {
        public Pen()
        {

            this.brushTexture = FileController.LoadBitmap(Path.Combine("Media", "BrushTextures", "pencil.png"));
        }
        public BrushType Type => BrushType.Pen;

        public void Draw()
        {
            throw new NotImplementedException();
        }
    }

    public class Pencil : Brush
    {
        SKPaint paint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 32f,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeCap = SKStrokeCap.Round,
            IsDither = true
        };

        public Pencil()
        {
            this.brushTexture = FileController.LoadBitmap(Path.Combine("Media", "BrushTextures", "pencil.png"));
            this.TintBrush(SKColors.Black);
        }

        public override void Draw(SKPoint previousPoint, SKPoint currentPoint, SKCanvas canvas)
        {
            canvas.DrawLine(previousPoint, currentPoint, paint);

        }

        public override void TintBrush(SKColor color)
        {
            this.paint.Color = color;
            this.color = color;
        }

        public override void UpdateBrushSize(float brushSize)
        {
            this.paint.StrokeWidth = brushSize;
            this.brushSize = brushSize;
        }
        public BrushType Type => BrushType.Pencil;

    }

    public class Marker : Brush
    {
        public Marker()
        {
            this.brushTexture = FileController.LoadBitmap(Path.Combine("Media", "BrushTextures", "marker.png"));
            this.TintBrush(SKColors.Blue);
        }
        public BrushType Type => BrushType.Marker;

    }

    public class Charcoal : Brush
    {
        public Charcoal()
        {
            this.brushTexture = FileController.LoadBitmap(Path.Combine("Media", "BrushTextures", "charcoal.png"));
            this.TintBrush(SKColors.Black);
        }
        public BrushType Type => BrushType.Charcoal;

    }

    public class Acryl : Brush
    {
        public Acryl()
        {
            this.brushTexture = FileController.LoadBitmap(Path.Combine("Media", "BrushTextures", "acryl.png"));
            this.TintBrush(SKColors.LightPink);
        }
        public BrushType Type => BrushType.Acrylic;

    }

    public class Sponge : Brush
    {
        public Sponge()
        {
            this.brushTexture = FileController.LoadBitmap(Path.Combine("Media", "BrushTextures", "sponge.png"));
            this.TintBrush(SKColors.YellowGreen);
        }
        public BrushType Type => BrushType.Sponge;

    }

    public class Airbrush : Brush
    {
        public Airbrush()
        {
            this.brushTexture = FileController.LoadBitmap(Path.Combine("Media", "BrushTextures", "airbrush.png"));
            this.TintBrush(SKColors.LightBlue);
        }
        public BrushType Type => BrushType.Airbrush;
    }
}   
