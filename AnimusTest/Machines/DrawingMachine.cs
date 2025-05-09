using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AnimusTest.Models;
using SkiaSharp;

namespace AnimusTest.Machines
{
    public class DrawingMachine
    {
        public bool isErasing { get; set; } = false;
        public bool isDrawing { get; set; } = false;
        private Models.Brush currentBrush;
        private List<Models.Brush> Brushes = new List<Models.Brush>();

        private SKBitmap bitmap;
        private SKCanvas drawingCanvas;

        private SKPoint previousPoint;

        public DrawingMachine(SKBitmap bitmap, SKCanvas drawingCanvas) {
            this.bitmap = bitmap;
            this.drawingCanvas = drawingCanvas;
            currentBrush = new Models.Pen(); // ДОДАТИ ІНІЦІАЛІЗАЦІЮ МАСИВУ ПЕНЗЛІВ
            // В МАЙБУТНЬОМУ (було б добре) Створити загальний клас Tool, від якого успадковуються Figure, Brush etc., а від них - відповідно підвиди інструментів(Figure: Rectangle, Line, Ellipse, Brush: Pen, Oil, Chalk...)
        }

        public void setCanvas(SKCanvas canvas)
        {
            this.drawingCanvas = canvas;
        }

        public void ToggleEraser()
        {
            if (isErasing) {
                currentBrush.body.BlendMode = SKBlendMode.SrcOver;
                isErasing = false;
            } else
            {
                currentBrush.body.BlendMode = SKBlendMode.Clear;
                isErasing = true;
            }
        }

        public void StartDrawing(SKPoint point)
        {
            isDrawing = true;
            previousPoint = point;
            
        }

        public void ContinueDrawing(SKPoint point)
        {
            drawingCanvas.DrawLine(previousPoint, point, currentBrush.body);
            
            previousPoint = point;
        }

        public void EndDrawing()
        {
            isDrawing = false;
        }


        public void setBrush(Models.Brush brush)
        {
            currentBrush = brush;
        }
    }

    public static class ExtensionMethods
    {
        public static SKPoint ToSKPoint(this System.Windows.Point point)
        {
            return new SKPoint((float)point.X, (float)point.Y);
        }
    }
}
