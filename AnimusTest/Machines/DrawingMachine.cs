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

        private Project Project { get; set; } = null;

        private SKPoint previousPoint;



        // ВИДАЛИТИ НАХ!



        public SKCanvas activeCanvas { get; set; } = null;
        //public int activeFrameIndex = 0;

        private readonly Project project;
        
        //public int ActiveLayerIndex = 0;

        public DrawingMachine(Project project) {


            currentBrush = new Models.Pen(); // ДОДАТИ ІНІЦІАЛІЗАЦІЮ МАСИВУ ПЕНЗЛІВ
            // В МАЙБУТНЬОМУ (було б добре) Створити загальний клас Tool, від якого успадковуються Figure, Brush etc., а від них - відповідно підвиди інструментів(Figure: Rectangle, Line, Ellipse, Brush: Pen, Oil, Chalk...)
            this.project = project;

            
        }


        public void DrawTest()
        {
            currentBrush.body.Color = SKColors.Red;
            SetActiveLayer(1);
            StartDrawing(new SKPoint(0, 0));
            ContinueDrawing(new SKPoint(300, 500));
            EndDrawing();
            currentBrush.body.Color = SKColors.Black;
            SetActiveLayer(0);
        }

        public void setCanvas(SKCanvas canvas)
        {
            this.drawingCanvas = canvas;
        }

        public void SetActiveLayer(int index)
        {
            if (index >= 0 && index < project.CurrentFrame.Layers.Count)
                project.CurrentLayerIndex = index;
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

            var layer = project.CurrentLayer;
            activeCanvas = new SKCanvas(layer.Bitmap);
        }

        public void ContinueDrawing(SKPoint point)
        {
            if (!isDrawing || activeCanvas == null) return;

            //drawingCanvas.DrawLine(previousPoint, point, currentBrush.body);
            activeCanvas.DrawLine(previousPoint, point, currentBrush.body);
            previousPoint = point;
        }

        public void EndDrawing() { 
            isDrawing = false;
            activeCanvas?.Dispose();
            activeCanvas = null;
        }


        public void setBrush(Models.Brush brush)
        {
            currentBrush = brush;
        }
        RasterLayer CreateNewLayer(int width, int height)
        {
            var bmp = new SKBitmap(width, height);
            using (var canvas = new SKCanvas(bmp))
                canvas.Clear(SKColors.Transparent);

            return new RasterLayer { Bitmap = bmp };
        }


    }

}
